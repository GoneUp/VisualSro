Imports SRFramework

Namespace Functions
    Module GameMaster
        Public Sub OnGM(ByVal packet As PacketReader, ByVal Index_ As Integer)

            Dim tag As Byte = packet.Word
            Debug.Print("[GM][Tag:" & tag & "]")

            If PlayerData(Index_).GM = True Then
                Select Case tag
                    Case GmTypes.MakeMonster
                        OnCreateObject(packet, Index_)

                    Case GmTypes.MakeItem ' Create Item
                        OnGmCreateItem(packet, Index_)

                    Case GmTypes.WayPoints 'Teleport
                        OnGmTeleport(packet, Index_)

                    Case GmTypes.MoveToUser
                        OnMoveToUser(packet, Index_)

                    Case GmTypes.RecallUser
                        OnRecallUser(packet, Index_)

                    Case GmTypes.Ban
                        OnBanUserGMCLoader(packet, Index_)

                    Case GmTypes.GoTown
                        OnGoTown(Index_)

                    Case GmTypes.ToTown
                        OnMoveUserToTown(packet, Index_)

                    Case GmTypes.KillMob
                        OnKillObject(packet, Index_)

                    Case GmTypes.Invincible
                        OnInvincible(Index_)

                    Case GmTypes.Invisible
                        OnInvisible(Index_)
                End Select
            Else
                Server.Disconnect(Index_)

                Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "GM", "Unautorized", "Gm_Command_Try:" & tag)
                'Hack Versuch
            End If
        End Sub

        Private Sub OnGmCreateItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim pk2ID As UInteger = packet.DWord
            Dim plus As Byte = packet.Byte 'Or Count
            Dim slot As Byte = GetFreeItemSlot(Index_)

            If slot <> -1 Then
                Dim tempItem As New cItem
                tempItem.ObjectID = pk2ID
                tempItem.CreatorName = PlayerData(Index_).CharacterName & "#GM"

                Dim refitem As cRefItem = GetItemByID(pk2ID)
                If refitem.CLASS_A = 1 Then
                    'Equip
                    tempItem.Plus = plus
                    tempItem.Data = refitem.MAX_DURA
                ElseIf refitem.CLASS_A = 2 Then
                    'Pet

                ElseIf refitem.CLASS_A = 3 Then
                    'Etc
                    tempItem.Data = plus
                End If


                Dim id As UInt64 = ItemManager.AddItem(tempItem)
                Inventorys(Index_).UserItems(slot).ItemID = id
                ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(slot), InvItemTypes.Inventory)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                writer.Byte(1)
                writer.Byte(6) 'type = new item
                writer.Byte(slot)

                AddItemDataToPacket(tempItem, writer)

                Server.Send(writer.GetBytes, Index_)

                Debug.Print("[ITEM CREATE][Info][Slot:{0}][ID:{1}][Dura:{2}][Amout:{3}][Plus:{4}]", slot,
                            tempItem.ObjectID, tempItem.Data, tempItem.Data, tempItem.Plus)

                If Settings.LogGM Then
                    Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "GM", "Item_Create",
                                     String.Format("Slot:{0}, ID:{1}, Dura:{2}, Amout:{3}, Plus:{4}", slot,
                                                   tempItem.ObjectID, tempItem.Data, tempItem.Data,
                                                   tempItem.Plus))
                End If
            End If
        End Sub

        Private Sub OnGmTeleport(ByVal packet As PacketReader, ByVal Index_ As Integer)

            Dim toPos As New Position
            toPos.XSector = packet.Byte
            toPos.YSector = packet.Byte
            toPos.X = packet.Float
            toPos.Z = packet.Float
            toPos.Y = packet.Float
            Dim angle As UInt16 = packet.Word 'Not sure 


            PlayerData(Index_).SetPosition = toPos
            PlayerData(Index_).TeleportType = TeleportTypes.GM

            GameDB.SavePosition(Index_)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_TELEPORT_ANNONCE)
            writer.Byte(PlayerData(Index_).Position.XSector)
            writer.Byte(PlayerData(Index_).Position.YSector)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnSetWeather(ByVal type As Byte, ByVal strength As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_WEATHER)
            writer.Byte(type)
            writer.Byte(Strength)
            Server.SendToAllIngame(writer.GetBytes)
        End Sub

        Private Sub OnMoveToUser(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nameLength As Byte = packet.Word
            Dim name As String = packet.String(nameLength)

            For i As Integer = 0 To Server.MaxClients - 1
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).CharacterName = name Then

                        PlayerData(Index_).SetPosition = PlayerData(i).Position
                        GameDB.SavePosition(Index_)

                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)

                        Exit For
                    End If
                End If
            Next
        End Sub

        Private Sub OnRecallUser(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nameLength As UInt16 = packet.Word
            Dim name As String = packet.String(nameLength)

            For i As Integer = 0 To Server.MaxClients - 1
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).CharacterName = name Then

                        PlayerData(i).SetPosition = PlayerData(Index_).Position
                        GameDB.SavePosition(Index_)

                        OnTeleportUser(i, PlayerData(i).Position.XSector, PlayerData(i).Position.YSector)
                        Exit For
                    End If
                End If
            Next
        End Sub

        Private Sub OnBanUserGMCLoader(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim userLoader As New GameDB.GameUserLoader(packet, Index_)
            AddHandler userLoader.GetCallback, AddressOf OnBanUser
            userLoader.LoadFromGlobal(PlayerData(Index_).AccountID)
        End Sub

        Private Sub OnBanUser(user As cUser, ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nameLength As UInt16 = packet.Word
            Dim name As String = packet.String(nameLength)

            For i As Integer = 0 To GameDB.Chars.Length - 1
                If GameDB.Chars(i).CharacterName = name Then
                    user.Banned = True
                    user.BannReason = String.Format("You got banned by: {0}'", PlayerData(Index_).CharacterName)
                    user.BannTime = Date.Now.AddYears(1)

                    Dim userLoader As New GameDB.GameUserLoader(Index_)
                    userLoader.UpdateGlobal(user)

                    SendPm(Index_, "User got banned.", "[SERVER]")
                    Exit For
                End If
            Next

            For i As Integer = 0 To Server.OnlineClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).CharacterName = name Then
                        Server.Disconnect(i)
                    End If
                End If
            Next i

            If Settings.LogGM Then
                Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "GM", "Ban", String.Format("Banned User:" & name))
            End If
        End Sub

        Private Sub OnGoTown(ByVal Index_ As Integer)
            'Teleport the GM to Town
            PlayerData(Index_).SetPosition = PlayerData(Index_).PositionReturn   'Set new Pos
            GameDB.SavePosition(Index_)
            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
        End Sub

        Private Sub OnMoveUserToTown(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nameLength As UInt16 = packet.Word
            Dim name As String = packet.String(nameLength)

            For i As Integer = 0 To Server.MaxClients - 1
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).CharacterName = name Then
                        OnGoTown(i)
                    End If
                End If
            Next
        End Sub

        Private Sub OnCreateObject(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim objectid As UInteger = packet.DWord
            Dim count As Byte = packet.Byte
            Dim type As Integer = packet.Byte

            Dim refobject As SilkroadObject = GetObject(objectid)

            Dim selector As String = refobject.CodeName.Substring(0, 3)

            Select Case selector
                Case "MOB"
                    For i = 1 To count
                        SpawnMob(objectid, type, PlayerData(Index_).Position, 0, -1, PlayerData(Index_).ChannelId)
                    Next
                Case "NPC"
                    SpawnNPC(objectid, PlayerData(Index_).Position, 0)
            End Select

            If Settings.LogGM Then
                Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "GM", "Monster_Spawn",
                                 String.Format("PK2ID: {0}, Monster_Name: {1} Type: {2}", objectid, refobject.CodeName,
                                               count))
            End If

            ObjectSpawnCheck(Index_)
        End Sub

        Private Sub OnKillObject(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim uniqueId As UInteger = Packet.DWord


            Dim list = MobList.Keys.ToList
            For i = 0 To list.Count - 1
                Dim key As UInt32 = list(i)
                If MobList.ContainsKey(key) Then
                    Dim Mob_ As cMonster = MobList.Item(key)
                    If Mob_.UniqueID = uniqueId Then
                        MobAddDamageFromPlayer(Mob_.HPCur, Index_, Mob_.UniqueID, False)
                        GetEXPFromMob(Mob_)
                        KillMob(Mob_.UniqueID)
                    End If
                End If
            Next
        End Sub

        Private Sub OnInvincible(ByVal Index_ As Integer)
            If PlayerData(Index_).Invincible = False Then
                PlayerData(Index_).Invincible = True
            Else
                PlayerData(Index_).Invincible = False
            End If
        End Sub

        Private Sub OnInvisible(ByVal Index_ As Integer)
            If PlayerData(Index_).Invisible = False Then
                PlayerData(Index_).Invisible = True
                DespawnPlayer(Index_)
                UpdateState(4, 4, Index_)
            Else
                PlayerData(Index_).Invisible = False
                UpdateState(4, 13, Index_)
            End If
        End Sub

        Public Sub OnWorldStatus(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_GOLD_UPDATE)

        End Sub

        Private Enum GmTypes
            FindUser = 1
            GoTown = 2
            ToTown = 3
            WorldStatus = 4
            MakeMonster = 6
            MakeItem = 7
            MoveToUser = 8
            Ban = 13
            Invisible = 14
            Invincible = 15
            WayPoints = 16
            RecallUser = 17
            KillMob = 20
            MoveToNpc = 49
        End Enum
    End Module
End Namespace
