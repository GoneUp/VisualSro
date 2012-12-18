Imports SRFramework

Namespace Functions
    Module Character
        Public Sub HandleCharPacket(ByVal pack As PacketReader, ByVal Index_ As Integer)
            If SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.AUTH Or SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.CHARLIST Then
                SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.CHARLIST
            Else
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim tag As Byte = pack.Byte

            Select Case tag
                Case 1 'Crweate
                    OnCreateChar(pack, Index_)
                Case 2 'Char List
                    OnCharList(Index_)
                Case 3 'Char delete
                    OnDeleteChar(pack, Index_)
                Case 4 'Nick Check
                    OnCheckNick(pack, Index_)
                Case 5 'Restore
                    OnRestoreChar(pack, Index_)
            End Select
        End Sub

        Public Sub OnCharList(ByVal Index_ As Integer)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_CHARACTER)

            GameDB.FillCharList(CharListing(Index_))

            writer.Byte(2)  'Char List
            writer.Byte(1)

            writer.Byte(CharListing(Index_).NumberOfChars)

            If CharListing(Index_).NumberOfChars = 0 Then

                'unknown byte thief - hunterr flag?
                writer.Byte(0)
                Server.Send(writer.GetBytes, Index_)

            ElseIf CharListing(Index_).NumberOfChars > 0 Then
                For i = 0 To (CharListing(Index_).NumberOfChars - 1)
                    If CharListing(Index_).Chars(i) Is Nothing Then
                        Log.WriteSystemLog("Charlist, Char is nothing, AccID: " & CharListing(Index_).LoginInformation.Name)
                        Server.Disconnect(Index_)
                        Exit Sub
                    End If

                    With CharListing(Index_).Chars(i)
                        writer.DWord(.Pk2ID)
                        writer.Word(.CharacterName.Length)
                        writer.String(.CharacterName)
                        writer.Byte(.Volume)
                        writer.Byte(.Level)
                        writer.QWord(.Experience)
                        writer.Word(.Strength)
                        writer.Word(.Intelligence)
                        writer.Word(.Attributes)
                        writer.DWord(.CHP)
                        writer.DWord(.CMP)
                        If .Deleted = True Then
                            Dim diff As Long = DateDiff(DateInterval.Minute, DateTime.Now, .DeletionTime)
                            writer.Byte(1) 'to delete
                            writer.DWord(diff)
                        Else
                            writer.Byte(0)
                        End If

                        writer.Word(0) 'Job Alias
                        writer.Byte(0) 'In Academy

                        'Now Items
                        Dim inventory As cInventory = New cInventory(.CharacterId, .MaxInvSlots, .MaxAvatarSlots)

                        Dim playerItemCount As Integer = 0
                        For slot = 0 To 9
                            If inventory.UserItems(slot).ItemID <> 0 Then
                                playerItemCount += 1
                            End If
                        Next

                        writer.Byte(playerItemCount)

                        For slot = 0 To 9
                            If inventory.UserItems(slot).ItemID <> 0 Then
                                Dim item As cItem = GameDB.Items(inventory.UserItems(slot).ItemID)
                                writer.DWord(item.ObjectID)
                                writer.Byte(item.Plus)
                            End If
                        Next

                        'Avatars

                        Dim avatarItemCount As Integer = 0
                        For slot = 0 To 4
                            If inventory.AvatarItems(slot).ItemID <> 0 Then
                                avatarItemCount += 1
                            End If
                        Next

                        writer.Byte(avatarItemCount)

                        For slot = 0 To 4
                            If inventory.AvatarItems(slot).ItemID <> 0 Then
                                Dim item As cItem = GameDB.Items(inventory.AvatarItems(slot).ItemID)
                                writer.DWord(item.ObjectID)
                                writer.Byte(item.Plus)
                            End If
                        Next
                    End With
                Next


                'unknown byte thief - hunterr flag?
                writer.Byte(0)

                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub

        Private Sub OnCheckNick(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nick As String = packet.String(packet.Word)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_CHARACTER)
            writer.Byte(4)
            'nick check

            If GameDB.CheckNick(nick) And CheckForAbuse(nick) = False Then
                writer.Byte(1)
            Else
                writer.Byte(2)
                writer.Byte(&H10)
                writer.Byte(4)
                '260
            End If

            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnSendNickError(ByVal Index_ As Integer, ByVal errornum As UInt16)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_CHARACTER)
            writer.Byte(4)
            writer.Byte(2)
            writer.Word(errornum)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Private Function CheckForAbuse(ByVal nick As String) As Boolean
            Dim tmp As String = nick.ToLowerInvariant
            For i = 0 To RefAbuseList.Count - 1
                If tmp.Contains(RefAbuseList(i)) = True Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Sub OnDeleteChar(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nick As String = packet.String(packet.Word)
            For i = 0 To CharListing(Index_).NumberOfChars - 1
                If CharListing(Index_).Chars(i).CharacterName = nick Then
                    CharListing(Index_).Chars(i).Deleted = True
                    Dim dat As DateTime = DateTime.Now
                    Dim dat1 = dat.AddDays(7)
                    CharListing(Index_).Chars(i).DeletionTime = dat1
                    Database.SaveQuery(
                        String.Format("UPDATE characters SET deletion_mark='1', deletion_time='{0}' where id='{1}'",
                                      dat1.ToString, CharListing(Index_).Chars(i).CharacterId))

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_CHARACTER)
                    writer.Byte(3)   'type = delte
                    writer.Byte(1) 'success
                    Server.Send(writer.GetBytes, Index_)
                End If
            Next
        End Sub

        Private Sub OnRestoreChar(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nick As String = packet.String(packet.Word)
            For i = 0 To CharListing(Index_).NumberOfChars - 1
                If CharListing(Index_).Chars(i).CharacterName = nick Then
                    CharListing(Index_).Chars(i).Deleted = False
                    Database.SaveQuery(String.Format("UPDATE characters SET deletion_mark='0' where id='{0}'",
                                                     CharListing(Index_).Chars(i).CharacterId))

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_CHARACTER)
                    writer.Byte(5) 'type = restore
                    writer.Byte(1) 'success
                    Server.Send(writer.GetBytes, Index_)
                End If
            Next
        End Sub

        Private Sub OnCreateChar(ByVal pack As PacketReader, ByVal Index_ As Integer)
            Dim nick As String = pack.String(pack.Word)
            Dim model As UInt32 = pack.DWord
            Dim volume As Byte = pack.Byte
            Dim items(4) As UInt32
            items(1) = pack.DWord
            items(2) = pack.DWord
            items(3) = pack.DWord
            items(4) = pack.DWord

            'Check it

            If IsCharChinese(model) = False And IsCharEurope(model) = False Then
                'Wrong Model Code! 
                Server.Disconnect(Index_)
                Log.WriteSystemLog(String.Format("[Character Creation][Wrong Model: {0}][Index: {1}]", model, Index_))
                Exit Sub
            End If

            Dim refitems(4) As cRefItem
            refitems(1) = GetItemByID(items(1))
            refitems(2) = GetItemByID(items(2))
            refitems(3) = GetItemByID(items(3))
            refitems(4) = GetItemByID(items(4))

            For i = 1 To 4
                If refitems(i).ITEM_TYPE_NAME.EndsWith("_DEF") = False Then
                    Server.Disconnect(Index_)
                    Log.WriteSystemLog(String.Format("[Character Creation][Wrong Item: {0}][Index: {1}]",
                                                     refitems(i).ITEM_TYPE_NAME, Index_))
                    Exit Sub
                End If
                Debug.Print("[Character Creation][" & i & "][ID:" & items(i) & "]")
            Next

            'Creation
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_CHARACTER)
            writer.Byte(1)
            'create


            If GameDB.CheckNick(nick) And CheckForAbuse(nick) = True Then
                writer.Byte(2)
                writer.Word(216)
                Server.Send(writer.GetBytes, Index_)
            Else

                Array.Resize(GameDB.Chars, GameDB.Chars.Count + 1)

                Dim newCharacterIndex As Integer = GameDB.Chars.Count - 1

                GameDB.Chars(newCharacterIndex) = New cCharacter

                With GameDB.Chars(newCharacterIndex)
                    .AccountID = CharListing(Index_).LoginInformation.AccountID
                    .CharacterName = nick
                    .CharacterId = Id_Gen.GetNewCharId
                    .UniqueID = Id_Gen.GetUnqiueId
                    .HP = 200
                    .MP = 200
                    .CHP = 200
                    .CMP = 200
                    .Pk2ID = model
                    .Volume = volume
                    .Level = Settings.PlayerStartLevel
                    .Gold = Settings.PlayerStartGold
                    .SkillPoints = Settings.PlayerStartSP
                    .GM = Settings.PlayerStartGM
                    .Strength = 20
                    .Intelligence = 20
                    .PVP = 0
                    .MaxInvSlots = 45
                    .PositionDead = Settings.PlayerStartReturnPos
                    .PositionRecall = Settings.PlayerStartReturnPos
                    .PositionReturn = Settings.PlayerStartReturnPos

                    If IsCharChinese(model) Then
                        .PosTracker = New cPositionTracker(Settings.PlayerStartPosCh, 16, 50, 100)
                    ElseIf IsCharEurope(model) Then
                        .PosTracker = New cPositionTracker(Settings.PlayerStartPosEu, 16, 50, 100)
                    End If


                    Const magdefmin As Double = 3.0
                    Const phydefmin As Double = 6.0
                    Const phyatkmin As UShort = 6
                    Const phyatkmax As UShort = 9
                    Const magatkmin As UShort = 6
                    Const magatkmax As UShort = 9
                    Const hit As UShort = 11
                    Const parry As UShort = 11

                    .MinPhy = phyatkmin
                    .MaxPhy = phyatkmax
                    .MinMag = magatkmin
                    .MaxMag = magatkmax
                    .PhyDef = phydefmin
                    .MagDef = magdefmin
                    .Hit = hit
                    .Parry = parry

                    .SetCharGroundStats()

                    Database.SaveQuery(
                        String.Format(
                            "INSERT INTO characters (id, account, name, chartype, volume, level, gold, sp, gm) VALUE ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                            .CharacterId, .AccountID,
                            .CharacterName, .Pk2ID,
                            .Volume, .Level,
                            .Gold, .SkillPoints, CInt(.GM)))
                    Database.SaveQuery(
                        String.Format(
                            "UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'",
                           .Position.XSector, .Position.YSector,
                            Math.Round(.Position.X),
                            Math.Round(.Position.Z),
                            Math.Round(.Position.Y),
                            .CharacterId))
                    Database.SaveQuery(String.Format("INSERT INTO char_pos (OwnerCharID) VALUE ('{0}')",
                                                     .CharacterId))
                    Database.SaveQuery(String.Format("INSERT INTO guild_member (charid) VALUE ('{0}')",
                                                     .CharacterId))

                    ' Masterys
                    Dim mastery As New cMastery

                    If IsCharChinese(model) Then
                        'Chinese Char
                        '257 - 259

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 257
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 258
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 259
                        AddMasteryToDB(mastery)

                        '273 - 276
                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 273
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 274
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 275
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 276
                        AddMasteryToDB(mastery)


                    ElseIf IsCharEurope(model) Then

                        'Europe Char
                        '513 - 518
                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 513
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 514
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 515
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 516
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 517
                        AddMasteryToDB(mastery)

                        mastery = New cMastery
                        mastery.OwnerID = .CharacterId
                        mastery.Level = Settings.PlayerStartMasteryLevel
                        mastery.MasteryID = 518
                        AddMasteryToDB(mastery)
                    End If

                    'Job Mastery
                    mastery.OwnerID = .CharacterId
                    mastery.Level = Settings.PlayerStartMasteryLevel
                    mastery.MasteryID = 1000
                    AddMasteryToDB(mastery)


                    'ITEMS
                    'Inventory
                    For slot = 0 To 109
                        Dim tmpItem As New cInventoryItem
                        tmpItem.OwnerID = .CharacterId
                        tmpItem.Slot = slot
                        tmpItem.ItemID = 0
                        ItemManager.AddInvItem(tmpItem, cInventoryItem.Type.Inventory)
                    Next
                    'Avatar
                    For slot = 0 To .MaxAvatarSlots
                        Dim tmpItem As New cInventoryItem
                        tmpItem.OwnerID = .CharacterId
                        tmpItem.Slot = slot
                        tmpItem.ItemID = 0
                        ItemManager.AddInvItem(tmpItem, cInventoryItem.Type.AvatarInventory)
                    Next
                    'Storage
                    For slot = 0 To 255
                        Dim tmpItem As New cInventoryItem
                        tmpItem.OwnerID = .CharacterId
                        tmpItem.Slot = slot
                        tmpItem.ItemID = 0
                        ItemManager.AddInvItem(tmpItem, cInventoryItem.Type.Storage)
                    Next


                    '1 = Body = Inv 1
                    '2 = Legs = Inv 4
                    '3 = foot = Inv 5
                    '4 = Waffe = Inv 6
                    Dim item As New cItem 'Body
                    Dim id As UInt64 = 0

                    item.ObjectID = items(1)
                    item.Plus = Math.Round(Rand.Next(Settings.PlayerStartItemsPlusMin, Settings.PlayerStartItemsPlusMax))
                    item.Data = refitems(1).MAX_DURA
                    item.CreatorName = .CharacterName & "#START"
                    id = ItemManager.AddItem(item)
                    ItemManager.UpdateInvItem(.CharacterId, 1, id, cInventoryItem.Type.Inventory)

                    item = New cItem 'legs
                    item.ObjectID = items(2)
                    item.Plus = Math.Round(Rand.Next(Settings.PlayerStartItemsPlusMin, Settings.PlayerStartItemsPlusMax))
                    item.Data = refitems(2).MAX_DURA
                    item.CreatorName = .CharacterName & "#START"
                    id = ItemManager.AddItem(item)
                    ItemManager.UpdateInvItem(.CharacterId, 4, id, cInventoryItem.Type.Inventory)

                    item = New cItem 'Foot
                    item.ObjectID = items(3)
                    item.Plus = Math.Round(Rand.Next(Settings.PlayerStartItemsPlusMin, Settings.PlayerStartItemsPlusMax))
                    item.Data = refitems(3).MAX_DURA
                    item.CreatorName = .CharacterName & "#START"
                    id = ItemManager.AddItem(item)
                    ItemManager.UpdateInvItem(.CharacterId, 5, id, cInventoryItem.Type.Inventory)

                    item = New cItem 'Weapon
                    item.ObjectID = items(4)
                    item.Plus = Math.Round(Rand.Next(Settings.PlayerStartItemsPlusMin, Settings.PlayerStartItemsPlusMax))
                    item.Data = refitems(4).MAX_DURA
                    item.CreatorName = .CharacterName & "#START"
                    id = ItemManager.AddItem(item)
                    ItemManager.UpdateInvItem(.CharacterId, 6, id, cInventoryItem.Type.Inventory)

                    If items(4) = 3632 Or items(4) = 3633 Then 'Sword or Blade need a Shield
                        item = New cItem
                        item.ObjectID = 251
                        item.Plus = Math.Round(Rand.Next(Settings.PlayerStartItemsPlusMin, Settings.PlayerStartItemsPlusMax))
                        item.Data = GetItemByID(251).MAX_DURA
                        item.CreatorName = .CharacterName & "#START"
                        id = ItemManager.AddItem(item)
                        ItemManager.UpdateInvItem(.CharacterId, 7, id, cInventoryItem.Type.Inventory)


                    ElseIf items(4) = 3636 Then 'Bow --> Give some Arrows
                        item = New cItem
                        item.ObjectID = 62
                        item.Data = 100
                        item.CreatorName = .CharacterName & "#START"
                        id = ItemManager.AddItem(item)
                        ItemManager.UpdateInvItem(.CharacterId, 7, id, cInventoryItem.Type.Inventory)

                    ElseIf items(4) = 10730 Or items(4) = 10734 Or items(4) = 10737 Then 'EU Weapons who need a shield
                        item = New cItem
                        item.ObjectID = 10738
                        item.Plus = Math.Round(Rand.Next(Settings.PlayerStartItemsPlusMin, Settings.PlayerStartItemsPlusMax))
                        item.Data = GetItemByID(251).MAX_DURA
                        item.CreatorName = .CharacterName & "#START"
                        id = ItemManager.AddItem(item)
                        ItemManager.UpdateInvItem(.CharacterId, 7, id, cInventoryItem.Type.Inventory)

                    ElseIf items(4) = 10733 Then 'Armbrust --> Bolt
                        item = New cItem
                        item.ObjectID = 10376
                        item.Data = 100
                        item.CreatorName = .CharacterName & "#START"
                        id = ItemManager.AddItem(item)
                        ItemManager.UpdateInvItem(.CharacterId, 7, id, cInventoryItem.Type.Inventory)
                    End If

                    'Hotkeys
                    For i = 0 To 50
                        Dim toadd As New cHotKey
                        toadd.OwnerID = .CharacterId
                        toadd.Slot = i
                        AddHotKeyToDB(toadd)
                    Next

                    'Mods
                    GameMod.Damage.OnPlayerCreate(.CharacterId, Index_)

                End With


                writer.Byte(1)  'Finish
                Server.Send(writer.GetBytes, Index_) 'success
            End If
        End Sub

        Private Sub AddMasteryToDB(ByVal toadd As cMastery)
            Array.Resize(GameDB.Masterys, GameDB.Masterys.Length + 1)
            GameDB.Masterys(GameDB.Masterys.Length - 1) = toadd

            Database.SaveQuery(String.Format("INSERT INTO char_mastery(owner, mastery, level) VALUE ('{0}','{1}','{2}')",
                                             toadd.OwnerID, toadd.MasteryID, toadd.Level))
        End Sub

        Private Sub AddHotKeyToDB(ByVal toadd As cHotKey)
            GameDB.Hotkeys.Add(toadd)
            Database.SaveQuery(String.Format("INSERT INTO hotkeys(OwnerID, slot) VALUE ('{0}','{1}')", toadd.OwnerID,
                                             toadd.Slot))
        End Sub

        Public Sub CharLoading(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If CharListing(Index_) IsNot Nothing And SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.CHARLIST Then
                SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.GOING_INGAME
            Else
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim selectedNick As String = packet.String(packet.Word)
            Dim writer As New PacketWriter

            writer.Create(ServerOpcodes.GAME_INGAME_REQ_REPLY)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)

            'Main
            For i = 0 To CharListing(Index_).Chars.Count - 1
                If CharListing(Index_).Chars(i).CharacterName = selectedNick Then
                    PlayerData(Index_) = CharListing(Index_).Chars(i)
                    PlayerData(Index_).UniqueID = Id_Gen.GetUnqiueId()

                    Inventorys(Index_) = New cInventory(CharListing(Index_).Chars(i).CharacterId, CharListing(Index_).Chars(i).MaxInvSlots, CharListing(Index_).Chars(i).MaxAvatarSlots)

                    SessionInfo(Index_).Charname = selectedNick
                    Exit For
                End If
            Next

            'No Char....
            If SessionInfo(Index_).Charname = "" Or PlayerData(Index_) Is Nothing Or Inventorys(Index_) Is Nothing Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            SendCharacterIngame(Index_)
        End Sub

        Public Sub OnJoinWorldRequest(ByVal Index_ As Integer)
            If SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.GOING_INGAME Then
                SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.INGAME
            Else
                Server.Disconnect(Index_)
                Exit Sub
            End If

            PlayerData(Index_).Ingame = True

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_JOIN_WORLD_REPLY)
            writer.Byte(1)
            writer.Byte(&HB4)
            Server.Send(writer.GetBytes, Index_)

            'UpdateState(4, 2, Index_) 'Untouchable Status
            OnStatsPacket(Index_)
            OnSendSilksGMCLoader(Index_)
            If PlayerData(Index_).InGuild = True Then
                SendGuildInfo(Index_, False)
                LinkPlayerToGuild(Index_)
            End If

            ObjectSpawnCheck(Index_)
        End Sub

        Public Sub OnCharacterNamechangeRequest(ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim tag As Byte = packet.Byte
            Dim oldCharname As String = packet.String(packet.Word)
            Dim newCharname As String = packet.String(packet.Word)

            GameDB.FillCharList(CharListing(Index_))

            For i = 0 To CharListing(Index_).Chars.Count - 1
                If CharListing(Index_).Chars(i) Is Nothing Then
                    Log.WriteSystemLog("Charlist, Char is nothing, AccID: " & CharListing(Index_).LoginInformation.Name)
                    Server.Disconnect(Index_)
                    Exit Sub
                End If

                With CharListing(Index_).Chars(i)
                    If .CharacterName = oldCharname Then
                        'Found the Char, Change is only possible when it contains a @
                        If .CharacterName.Contains("@") Then
                            'Check the New Name
                            If GameDB.CheckNick(newCharname) And CheckForAbuse(newCharname) = False Then
                                'Free ;)
                                .CharacterName = newCharname

                                GameDB.SaveNameUpdate(.CharacterId, .CharacterName)
                            End If
                        End If
                    End If
                End With
            Next
        End Sub

        Public Sub PlayerCheckDeath(ByVal Index_ As Integer, ByVal setPosToTown As Boolean)
            If PlayerData(Index_).CHP = 0 Then
                PlayerData(Index_).Alive = True
                PlayerData(Index_).CHP = PlayerData(Index_).HP / 2
                GameDB.SaveHP(Index_)
            End If

            If setPosToTown Then
                PlayerData(Index_).SetPosition = PlayerData(Index_).PositionReturn
                GameDB.SavePosition(Index_)
            End If
        End Sub

        Public Function IsCharChinese(ByVal model As UInteger) As Boolean
            If Model >= 1907 And Model <= 1932 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function IsCharEurope(ByVal model As UInteger) As Boolean
            If Model >= 14717 And Model <= 14743 Then
                Return True
            Else
                Return False
            End If
        End Function
    End Module
End Namespace
