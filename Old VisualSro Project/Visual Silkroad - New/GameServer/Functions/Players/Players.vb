Namespace GameServer.Functions
    Module Players
        ''' <summary>
        ''' For Players
        ''' </summary>
        ''' <param name="Index"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateSpawnPacket(ByVal Index As Integer) As Byte()

            Dim chari As [cChar] = PlayerData(Index) 'Only for faster Code writing

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleSpawn)
            writer.DWord(chari.Pk2Id)
            writer.Byte(chari.Volume)
            writer.Byte(0)
            writer.Byte(chari.HelperIcon)

            'items
            Dim inventory As New cInventory(chari.MaxSlots)
            inventory = GameServer.GameDB.FillInventory(chari)

            Dim PlayerItemCount As Integer = 0
            For b = 0 To 9
                If inventory.UserItems(b).Pk2Id <> 0 And inventory.UserItems(b).Pk2Id <> 62 Then
                    PlayerItemCount += 1
                End If
            Next

            writer.Byte(chari.MaxSlots)
            writer.Byte(PlayerItemCount)

            For b = 0 To 9
                If inventory.UserItems(b).Pk2Id <> 0 Then
                    If inventory.UserItems(b).Pk2Id <> 62 Then 'Dont send arrows
                        writer.DWord(inventory.UserItems(b).Pk2Id)
                        writer.Byte(inventory.UserItems(b).Plus)
                    End If

                End If
            Next

            writer.Byte(4) 'Avatar Slots

            Dim AvatarItemCount As Integer = 0
            For b = 0 To 4
                If inventory.AvatarItems(b).Pk2Id <> 0 Then
                    AvatarItemCount += 1
                End If
            Next

            writer.Byte(AvatarItemCount)

            For b = 0 To 4
                If inventory.AvatarItems(b).Pk2Id <> 0 Then
                    writer.DWord(inventory.AvatarItems(b).Pk2Id)
                    writer.Byte(inventory.AvatarItems(b).Plus)
                End If
            Next

            writer.Byte(0) 'Duplicate List

            writer.DWord(chari.UniqueId)
            writer.Byte(chari.Position.XSector)
            writer.Byte(chari.Position.YSector)
            writer.Float(chari.Position.X)
            writer.Float(chari.Position.Z)
            writer.Float(chari.Position.Y)

            writer.Word(chari.Angle)
            writer.Byte(chari.Pos_Tracker.MoveState) 'dest
            If chari.Pos_Tracker.SpeedMode = cPositionTracker.enumSpeedMode.Walking Then
                writer.Byte(0) 'Walking
            Else
                writer.Byte(1) 'Running + Zerk
            End If

            If chari.Pos_Tracker.MoveState = cPositionTracker.enumMoveState.Standing Then
                writer.Byte(0)  'dest
                writer.Word(chari.Angle)
            ElseIf chari.Pos_Tracker.MoveState = cPositionTracker.enumMoveState.Walking Then
                writer.Byte(chari.Pos_Tracker.WalkPos.XSector)
                writer.Byte(chari.Pos_Tracker.WalkPos.YSector)
                writer.Byte(BitConverter.GetBytes(CShort(chari.Pos_Tracker.WalkPos.X)))
                writer.Byte(BitConverter.GetBytes(CShort(chari.Pos_Tracker.WalkPos.Z)))
                writer.Byte(BitConverter.GetBytes(CShort(chari.Pos_Tracker.WalkPos.Y)))
            End If


            writer.Byte(chari.Alive) ' death flag
            writer.Byte(chari.ActionFlag) 'action flag
            writer.Byte(chari.Berserk) 'berserk activated
            writer.Float(chari.WalkSpeed)
            writer.Float(chari.RunSpeed)
            writer.Float(chari.BerserkSpeed)

            writer.Byte(chari.Buffs.Count) 'no buffs for now
            Dim tmplist As Array = chari.Buffs.Keys.ToArray
            For Each key In tmplist
                If chari.Buffs.ContainsKey(key) Then
                    writer.DWord(chari.Buffs(key).SkillID)
                    writer.DWord(chari.Buffs(key).OverID)
                End If
            Next



            writer.Word(chari.CharacterName.Length)
            writer.String(chari.CharacterName)

            writer.Byte(0) 'Unknown
            writer.Byte(1) 'job type
            writer.Byte(0) 'Job Level
            writer.Byte(0) 'PK Flag
            writer.Byte(0) 'Transport
            writer.Byte(0) 'Unknown
            ' writer.DWord(0) IF Transportflag = 1
            If chari.InStall = True Then
                writer.Byte(4) 'Stall Flag
            Else
                writer.Byte(0)
            End If
            writer.Byte(0) 'Unknwon


            If chari.InGuild = True Then
                Dim guild As cGuild = GameDB.GetGuildWithGuildID(chari.GuildID)
                Dim member As cGuild.GuildMember_ = GetMember(chari.GuildID, chari.CharacterId)

                writer.Word(guild.Name.Length)
                writer.String(guild.Name)
                writer.DWord(guild.GuildID)
                writer.Word(member.GrantName.Length)
                writer.String(member.GrantName)

                writer.DWord(0) 'guild emblem id
                writer.DWord(0) 'union id
                writer.DWord(0) 'union emblem id
            Else

                writer.Word(0)
                writer.DWord(0)
                writer.Word(0)

                writer.DWord(0) 'guild emblem id
                writer.DWord(0) 'union id
                writer.DWord(0) 'union emblem id
            End If

            writer.Byte(0) 'guildwar falg
            writer.Byte(0) 'FW Role

            If chari.InStall = True Then
                Dim Stall_Index As Integer = GetStallIndex(chari.StallID)

                writer.Word(Stalls(Index).StallName.Length)
                writer.UString(Stalls(Index).StallName)
                writer.DWord(0) 'Stall Dekoration ID
            End If


            writer.Byte(0)
            writer.Byte(chari.PVP)
            writer.Byte(4)

            Return writer.GetBytes
        End Function


        Public Function CreateDespawnPacket(ByVal UniqueID As Integer) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleDespawn)
            writer.DWord(UniqueID)
            Return writer.GetBytes
        End Function

        Public Sub DespawnPlayerTeleport(ByVal Index_ As Integer)

            For i = 0 To PlayerData(Index_).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index_).SpawnedPlayers(i)
                If PlayerData(Other_Index) IsNot Nothing And PlayerData(Other_Index).SpawnedPlayers.Contains(Index_) = True Then
                    Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueId), Other_Index)
                    PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)

                    Server.Send(CreateDespawnPacket(PlayerData(Other_Index).UniqueId), Index_)
                End If
            Next

            For Each key In NpcList.Keys.ToList
                If NpcList.ContainsKey(key) Then
                    Dim Npc_ As cNPC = NpcList.Item(key)
                    If PlayerData(Index_).SpawnedNPCs.Contains(Npc_.UniqueID) = True Then
                        Server.Send(CreateDespawnPacket(Npc_.UniqueID), Index_)
                        PlayerData(Index_).SpawnedNPCs.Remove(Npc_.UniqueID)
                    End If
                End If
            Next


            For Each key In MobList.Keys.ToList
                If MobList.ContainsKey(key) Then
                    Dim Mob_ As cMonster = MobList.Item(key)
                    If PlayerData(Index_).SpawnedMonsters.Contains(Mob_.UniqueID) = True Then
                        Server.Send(CreateDespawnPacket(Mob_.UniqueID), Index_)
                        PlayerData(Index_).SpawnedMonsters.Remove(Mob_.UniqueID)
                    End If
                End If
            Next


            For Each key In ItemList.Keys.ToList
                If ItemList.ContainsKey(key) Then
                    If PlayerData(Index_).SpawnedItems.Contains(key) = True Then
                        Dim _item As cItemDrop = ItemList(key)
                        Server.Send(CreateDespawnPacket(_item.UniqueID), Index_)
                        PlayerData(Index_).SpawnedItems.Remove(_item.UniqueID)
                    End If
                End If
            Next

            CleanUpPlayer(Index_)
        End Sub

        Public Sub DespawnPlayer(ByVal Index_ As Integer)
            For i = 0 To PlayerData(Index_).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index_).SpawnedPlayers(i)
                If PlayerData(Other_Index) IsNot Nothing Then
                    If PlayerData(Other_Index).SpawnedPlayers.Contains(Index_) = True Then
                        Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueId), Other_Index)
                        PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)
                    End If
                End If

            Next
        End Sub

        Public Sub CleanUpPlayer(ByVal Index_ As Integer)
            'Cleanup
            PlayerData(Index_).SpawnedPlayers.Clear()
            PlayerData(Index_).SpawnedNPCs.Clear()
            PlayerData(Index_).SpawnedMonsters.Clear()
            PlayerData(Index_).SpawnedItems.Clear()


            PlayerData(Index_).Busy = False
            PlayerData(Index_).Alive = True
            PlayerData(Index_).Attacking = False
            PlayerData(Index_).Skilling = False
            PlayerData(Index_).AttackedId = 0
            PlayerData(Index_).UsingSkillId = 0
            PlayerData(Index_).SkillOverId = 0
            PlayerData(Index_).CastingId = 0
            PlayerData(Index_).AttackType = AttackType_.Normal
            PlayerData(Index_).LastSelected = 0
            PlayerData(Index_).UsedItem = UseItemTypes.None
            PlayerData(Index_).UsedItemParameter = 0
            PlayerData(Index_).PickUpId = 0
            PlayerData(Index_).Berserk = False

            If PlayerData(Index_).InExchange Then
                Exchange_AbortFromServer(Index_)
            End If
            PlayerData(Index_).InExchange = False
            PlayerData(Index_).ExchangeID = -1
            PlayerData(Index_).InExchangeWith = -1

            If PlayerData(Index_).InStall Then
                Stall_Close_Own(Index_)
            End If

            PlayerData(Index_).InStall = False
            PlayerData(Index_).StallID = 0

            Dim tmplist As Array = PlayerData(Index_).Buffs.Keys.ToArray
            For Each key In tmplist
                PlayerData(Index_).Buffs(key).ElaspedTimer_Stop()
                PlayerData(Index_).Buffs(key).Disponse()
            Next
            PlayerData(Index_).Buffs.Clear()
        End Sub

        Public Sub CheckStall(ByVal Index_ As Integer)
            For i = 0 To Stalls.Count - 1
                If Stalls(i).OwnerID = PlayerData(Index_).UniqueId Then
                    Stall_Close_Own(Index_)
                End If
            Next
        End Sub
    End Module
End Namespace
