Imports SRFramework

Namespace Functions
    Module Players
        ''' <summary>
        ''' For Players
        ''' </summary>
        ''' <param name="Index"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateSpawnPacket(ByVal Index_ As Integer) As Byte()
            Dim writer As New PacketWriter

            With PlayerData(Index_)
                writer.Create(ServerOpcodes.GAME_SINGLE_SPAWN)
                writer.DWord(.Pk2ID)
                writer.Byte(.Volume)
                writer.Byte(0)
                writer.Byte(0)
                writer.Byte(.HelperIcon)

                'items

                Dim PlayerItemCount As Integer = 0
                For slot = 0 To 9
                    If Inventorys(Index_).UserItems(slot).ItemID <> 0 Then
                        Dim item As cItem = GameDB.Items(Inventorys(Index_).UserItems(slot).ItemID)
                        If item.ObjectID <> 62 Then
                            PlayerItemCount += 1
                        End If
                    End If
                Next

                writer.Byte(.MaxInvSlots)
                writer.Byte(PlayerItemCount)

                For slot = 0 To 9
                    If Inventorys(Index_).UserItems(slot).ItemID <> 0 Then
                        Dim item As cItem = GameDB.Items(Inventorys(Index_).UserItems(slot).ItemID)
                        If item.ObjectID <> 62 Then 'Dont send arrows
                            writer.DWord(item.ObjectID)
                            writer.Byte(item.Plus)
                        End If

                    End If
                Next

                writer.Byte(.MaxAvatarSlots) 'Avatar Slots


                Dim AvatarItemCount As Integer = 0
                For slot = 0 To .MaxAvatarSlots - 1
                    If Inventorys(Index_).AvatarItems(slot).ItemID <> 0 Then
                        AvatarItemCount += 1
                    End If
                Next

                writer.Byte(AvatarItemCount)

                For slot = 0 To .MaxAvatarSlots - 1
                    If Inventorys(Index_).AvatarItems(slot).ItemID <> 0 Then
                        Dim item As cItem = GameDB.Items(Inventorys(Index_).AvatarItems(slot).ItemID)
                        writer.DWord(item.ObjectID)
                        writer.Byte(item.Plus)
                    End If
                Next

                writer.Byte(0) 'Duplicate List


                writer.DWord(.UniqueID)
                writer.Byte(.Position.XSector)
                writer.Byte(.Position.YSector)
                writer.Float(.Position.X)
                writer.Float(.Position.Z)
                writer.Float(.Position.Y)
                writer.Word(.Angle)

                writer.Byte(.PosTracker.MoveState) 'dest

                If .PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Walking Then
                    writer.Byte(0) 'Walking
                Else
                    writer.Byte(1) 'Running + Zerk
                End If

                If .PosTracker.MoveState = cPositionTracker.enumMoveState.Standing Then
                    writer.Byte(0) 'dest
                    writer.Word(.Angle)
                ElseIf .PosTracker.MoveState = cPositionTracker.enumMoveState.Walking Then
                    writer.Byte(.PosTracker.WalkPos.XSector)
                    writer.Byte(.PosTracker.WalkPos.YSector)
                    writer.Byte(BitConverter.GetBytes(CShort(.PosTracker.WalkPos.X)))
                    writer.Byte(BitConverter.GetBytes(CShort(.PosTracker.WalkPos.Z)))
                    writer.Byte(BitConverter.GetBytes(CShort(.PosTracker.WalkPos.Y)))
                End If


                writer.Byte(.Alive) ' death flag
                writer.Byte(.ActionFlag) 'action flag 
                writer.Byte(0) 'jobmode? 3=normal?
                writer.Byte(0)  'unknown
                writer.Byte(.Berserk) 'berserk activated

                writer.Float(.WalkSpeed)
                writer.Float(.RunSpeed)
                writer.Float(.BerserkSpeed)

                writer.Byte(.Buffs.Count)
                'no buffs for now
                Dim tmplist As Array = .Buffs.Keys.ToArray
                For Each key In tmplist
                    If .Buffs.ContainsKey(key) Then
                        writer.DWord(.Buffs(key).SkillID)
                        writer.DWord(.Buffs(key).OverID)
                    End If
                Next


                writer.Word(.CharacterName.Length)
                writer.String(.CharacterName)

                writer.Byte(0) 'job type
                writer.Byte(0) 'Job Level
                writer.Byte(0) 'PK Flag
                writer.Byte(0) 'Transport
                ' writer.DWord(0) IF Transportflag = 1
                If .InStall = True Then
                    writer.Byte(4)
                    'Stall Flag
                Else
                    writer.Byte(0)
                End If


                If .InGuild = True Then
                    Dim guild As cGuild = GameDB.GetGuild(.GuildID)
                    Dim member As cGuild.GuildMember_ = GetMember(.GuildID, .CharacterId)

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
                    writer.DWord(0)  'union id
                    writer.DWord(0) 'union emblem id
                End If

                writer.Byte(0) 'guildwar falg
                writer.Byte(0) 'FW Role


                If .InStall = True Then
                    Dim Stall_Index As Integer = GetStallIndex(.StallID)

                    writer.Word(Stalls(Index_).StallName.Length)
                    writer.UString(Stalls(Index_).StallName)
                    writer.DWord(0) 'Stall Dekoration ID
                End If


                writer.Byte(0)
                writer.Byte(.PVP)
                writer.Byte(3)

            End With
            Return writer.GetBytes
        End Function


        Public Function CreateDespawnPacket(ByVal UniqueID As Integer) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_SINGLE_DESPAWN)
            writer.DWord(UniqueID)
            Return writer.GetBytes
        End Function

        Public Sub DespawnPlayerTeleport(ByVal Index_ As Integer)

            For i = 0 To PlayerData(Index_).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index_).SpawnedPlayers(i)
                If _
                    PlayerData(Other_Index) IsNot Nothing And
                    PlayerData(Other_Index).SpawnedPlayers.Contains(Index_) = True Then
                    Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueID), Other_Index)
                    PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)

                    Server.Send(CreateDespawnPacket(PlayerData(Other_Index).UniqueID), Index_)
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

            CleanUpPlayerComplete(Index_)
        End Sub

        Public Sub DespawnPlayer(ByVal Index_ As Integer)
            For i = 0 To PlayerData(Index_).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index_).SpawnedPlayers(i)
                If PlayerData(Other_Index) IsNot Nothing Then
                    If PlayerData(Other_Index).SpawnedPlayers.Contains(Index_) = True Then
                        Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueID), Other_Index)
                        PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)
                    End If
                End If

            Next
        End Sub

        Public Sub CleanUpPlayerComplete(ByVal Index_ As Integer)
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
            PlayerData(Index_).ExchangeID = 0
            PlayerData(Index_).InExchangeWith = 0

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

            If PlayerData(Index_).PosTracker IsNot Nothing Then
                PlayerData(Index_).PosTracker.StopMove()
            End If
        End Sub

        Public Sub CheckStall(ByVal Index_ As Integer)
            For i = 0 To Stalls.Count - 1
                If Stalls(i).OwnerID = PlayerData(Index_).UniqueID Then
                    Stall_Close_Own(Index_)
                End If
            Next
        End Sub
    End Module
End Namespace
