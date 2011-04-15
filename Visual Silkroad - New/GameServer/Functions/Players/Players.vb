Namespace GameServer.Functions
    Module Players

        'Players
        Public PlayerData(5000) As [cChar]
        'items
        Public Inventorys(5000) As cInventory
        'Exchange
        Public ExchangeData As New List(Of cExchange)


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
            writer.DWord(chari.Model)
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
            writer.Byte(0) '0 avatars fro now
            writer.Byte(0) 'Duplicate List

            writer.DWord(chari.UniqueId)
            writer.Byte(chari.Position.XSector)
            writer.Byte(chari.Position.YSector)
            writer.Float(chari.Position.X)
            writer.Float(chari.Position.Z)
            writer.Float(chari.Position.Y)

            writer.Word(chari.Angle)
            writer.Byte(0) 'dest
            If chari.MovementType = MoveType_.Walk Then
                writer.Byte(0) 'Walking
            ElseIf chari.MovementType = MoveType_.Run Then
                writer.Byte(1) 'Running
            End If


            writer.Byte(0) 'dest
            writer.Word(chari.Angle)

            writer.Byte(chari.Alive) ' death flag
            writer.Byte(chari.ActionFlag) 'action flag
            writer.Byte(chari.Berserk) 'berserk activated
            writer.Float(chari.WalkSpeed)
            writer.Float(chari.RunSpeed)
            writer.Float(chari.BerserkSpeed)

            writer.Byte(0) 'no buffs for now

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

            For i = 0 To NpcList.Count - 1
                If PlayerData(Index_).SpawnedNPCs.Contains(i) = True Then
                    Dim _npc As cNPC = NpcList(i)
                    Server.Send(CreateDespawnPacket(_npc.UniqueID), Index_)
                    PlayerData(Index_).SpawnedNPCs.Remove(i)
                End If
            Next


            For i = 0 To MobList1.Keys.Count - 1
                Dim Mob_ As cMonster = MobList1(MobList1.Keys(i))
                If PlayerData(Index_).SpawnedMonsters.Contains(Mob_.UniqueID) = True Then
                    Server.Send(CreateDespawnPacket(Mob_.UniqueID), Index_)
                    PlayerData(Index_).SpawnedMonsters.Remove(i)
                End If
            Next


            For i = 0 To ItemList.Count - 1
                If PlayerData(Index_).SpawnedItems.Contains(i) = True Then
                    Dim _item As cItemDrop = ItemList(i)
                    Server.Send(CreateDespawnPacket(_item.UniqueID), Index_)
                    PlayerData(Index_).SpawnedItems.Remove(i)
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

            PlayerData(Index_).SpawnedPlayers.Clear()
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
            PlayerData(Index_).AttackedId = 0
            PlayerData(Index_).UsingSkillId = 0
            PlayerData(Index_).SkillOverId = 0
            PlayerData(Index_).CastingId = 0
            PlayerData(Index_).AttackType = AttackType_.Normal
            PlayerData(Index_).LastSelected = 0
            PlayerData(Index_).UsedItem = UseItemTypes.None
            PlayerData(Index_).UsedItemParameter = 0
            PlayerData(Index_).PickUpId = 0

            PlayerData(Index_).InExchange = False
            PlayerData(Index_).ExchangeID = 0

            If PlayerData(Index_).InStall = True Then

            End If

            PlayerData(Index_).InStall = False
            PlayerData(Index_).StallID = 0


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
