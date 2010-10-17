Namespace GameServer.Functions
    Module Players

        'Players
        Public PlayerData(5000) As [cChar]
        'items
        Public Inventorys(5000) As cInventory
        'Mobs
        'Skills
        'Exchange
        Public ExchangeData As New List(Of cExchange)


        Public Sub OnPlayerMovement(ByVal Index_ As Integer, ByVal packet As PacketReader)

            If PlayerData(Index_).Busy = True Then
                Exit Sub
            End If

            Dim tag As Byte = packet.Byte
            If tag = 1 Then
                '01 A8 60 61 02 FE FF 70 04
                Dim to_pos As New Position
                to_pos.XSector = packet.Byte
                to_pos.YSector = packet.Byte
                to_pos.X = CInt(packet.Word)
                Dim zByte As Byte() = packet.ByteArray(2) 'real z converting
                to_pos.Z = BitConverter.ToInt16(zByte, 0)
                to_pos.Y = CInt(packet.Word)


                Debug.Print(to_pos.XSector & " -- " & to_pos.YSector & " -- " & to_pos.X & " -- " & to_pos.Z & " -- " & to_pos.Y)
                Debug.Print("X:" & ((to_pos.XSector - 135) * 192) + (to_pos.X / 10) & " Y:" & ((to_pos.YSector - 92) * 192) + (to_pos.Y / 10))


                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Movement)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Byte(1) 'destination
                writer.Byte(to_pos.XSector)
                writer.Byte(to_pos.YSector)
                writer.Word(CUInt(to_pos.X))
                writer.Byte(zByte)
                writer.Word(CUInt(to_pos.Y))
                writer.Byte(0) '1= source
                'writer.Byte(PlayerData(Index_).Position.XSector)
                'writer.Byte(PlayerData(Index_).Position.YSector)
                'writer.Word(CUInt(PlayerData(Index_).Position.X))
                'writer.DWord(PlayerData(Index_).Position.Z)
                'writer.Word(CUInt(PlayerData(Index_).Position.Y))



                DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).UniqueId))
                PlayerData(Index_).Position = to_pos
                SpawnMe(Index_)
                SpawnOtherPlayer(Index_) 'Spawn before sending the Packet is to prevent chrashes
                DespawnPlayerRange(Index_)

                SpawnNPCRange(Index_)
                DeSpawnNPCRange(Index_)
                SpawnMobRange(Index_)
                DeSpawnMobRange(Index_)

                Server.SendToAllInRange(writer.GetBytes, Index_)
            End If
        End Sub
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
            inventory = GameServer.DatabaseCore.FillInventory(chari)

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

            writer.Byte(4) 'avatar start
            writer.Byte(0) '0 avatars fro now
            writer.Byte(0)

            writer.DWord(chari.UniqueId)
            writer.Byte(chari.Position.XSector)
            writer.Byte(chari.Position.YSector)
            writer.Float(chari.Position.X)
            writer.Float(chari.Position.Z)
            writer.Float(chari.Position.Y)

            writer.Word(chari.Angle)
            writer.Byte(0) 'dest
            writer.Byte(1) 'walk run flag
            writer.Byte(0) 'dest
            writer.Word(chari.Angle)
            writer.Byte(0) ' death flag
            writer.Byte(chari.ActionFlag) 'action flag
            writer.Byte(chari.Berserk) 'berserk activated
            writer.Float(chari.WalkSpeed)
            writer.Float(chari.RunSpeed)
            writer.Float(chari.BerserkSpeed)

            writer.Byte(0) 'no buffs for now

            writer.Word(chari.CharacterName.Length)
            writer.String(chari.CharacterName)

            writer.Byte(0) 'job lvl
            writer.Byte(1) 'job type
            writer.DWord(0) 'trader exp
            writer.DWord(0) 'theif exp
            writer.DWord(0) 'hunter exp
            writer.DWord(0)
            writer.DWord(0)
            writer.DWord(0)
            writer.DWord(0)
            writer.Byte(0)
            writer.Byte(chari.PVP)
			writer.Byte(1)

            Return writer.GetBytes
        End Function

        Public Function CreateSpawnPacket(ByVal Index As Integer, ByVal ToPos As Position) As Byte()

            Dim chari As [cChar] = PlayerData(Index) 'Only for faster Code writing

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleSpawn)
            writer.DWord(chari.Model)
            writer.Byte(chari.Volume)
            writer.Byte(0)
            writer.Byte(chari.HelperIcon)

            'items
            Dim inventory As New cInventory(chari.MaxSlots)
            inventory = GameServer.DatabaseCore.FillInventory(chari)

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

            writer.Byte(4) 'avatar start
            writer.Byte(0) '0 avatars fro now
            writer.Byte(0)

            writer.DWord(chari.UniqueId)
            writer.Byte(chari.Position.XSector)
            writer.Byte(chari.Position.YSector)
            writer.Float(chari.Position.X)
            writer.Float(chari.Position.Z)
            writer.Float(chari.Position.Y)

            writer.Word(chari.Angle)
            writer.Byte(1) 'dest
            writer.Byte(1) 'walk run flag
            writer.Byte(ToPos.XSector)
            writer.Byte(ToPos.YSector)
            writer.Word(ToPos.X)
            writer.Word(ToPos.Z)
            writer.Word(ToPos.Y)

            writer.Byte(1) 'movement flag
            writer.Byte(chari.ActionFlag) 'action flag
            writer.Byte(chari.Berserk) 'berserk activated

            writer.Float(chari.WalkSpeed)
            writer.Float(chari.RunSpeed)
            writer.Float(chari.BerserkSpeed)

            writer.Byte(0) 'no buffs for now

            writer.Word(chari.CharacterName.Length)
            writer.String(chari.CharacterName)

            writer.Byte(0) 'job lvl
            writer.Byte(1) 'job type
            writer.DWord(0) 'trader exp
            writer.DWord(0) 'theif exp
            writer.DWord(0) 'hunter exp
            writer.DWord(0)
            writer.DWord(0)
            writer.DWord(0)
            writer.DWord(0)
            writer.Byte(0)
            writer.Byte(chari.PVP)
            writer.Byte(1)

            Return writer.GetBytes
        End Function

        Public Function CreateDespawnPacket(ByVal UniqueID As Integer) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleDespawn)
            writer.DWord(UniqueID)
            Return writer.GetBytes
        End Function


        Public Sub SpawnMe(ByVal Index As Integer)
            Dim range As Integer = ServerRange

            For refindex As Integer = 0 To Server.MaxClients
                Dim socket As Net.Sockets.Socket = ClientList.GetSocket(refindex)
                Dim player As [cChar] = PlayerData(refindex) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso Index <> refindex Then
                    Dim distance As Long = Math.Round(Math.Sqrt((PlayerData(Index).Position.X - player.Position.X) ^ 2 + (PlayerData(Index).Position.Y - player.Position.Y) ^ 2)) 'Calculate Distance
                    If distance < range Then
                        If PlayerData(refindex).SpawnedPlayers.Contains(Index) = False Then
                            Server.Send(CreateSpawnPacket(Index), refindex)
                            PlayerData(refindex).SpawnedPlayers.Add(Index)
                        End If
                    End If
                End If
            Next refindex

        End Sub

        Public Sub SpawnMeAtMovement(ByVal Index As Integer, ByVal ToPos As Position)
            Dim range As Integer = ServerRange

            For refindex As Integer = 0 To Server.MaxClients
                Dim socket As Net.Sockets.Socket = ClientList.GetSocket(refindex)
                Dim player As [cChar] = PlayerData(refindex) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso Index <> refindex Then
                    Dim distance As Long = Math.Round(Math.Sqrt((PlayerData(Index).Position.X - player.Position.X) ^ 2 + (PlayerData(Index).Position.Y - player.Position.Y) ^ 2)) 'Calculate Distance
                    If distance < range Then
                        If PlayerData(refindex).SpawnedPlayers.Contains(Index) = False Then
                            Server.Send(CreateSpawnPacket(Index, ToPos), refindex)
                            PlayerData(refindex).SpawnedPlayers.Add(Index)
                        End If
                    End If
                End If
            Next refindex

        End Sub

        Public Sub SpawnOtherPlayer(ByVal Index As Integer)
            Dim range As Integer = ServerRange

            For refindex As Integer = 0 To Server.MaxClients
                Dim socket As Net.Sockets.Socket = ClientList.GetSocket(refindex)
                Dim player As [cChar] = PlayerData(refindex) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso Index <> refindex Then
                    Dim distance As Long = Math.Round(Math.Sqrt((PlayerData(Index).Position.X - player.Position.X) ^ 2 + (PlayerData(Index).Position.Y - player.Position.Y) ^ 2)) 'Calculate Distance
                    If distance < range Then
                        If PlayerData(Index).SpawnedPlayers.Contains(refindex) = False Then
                            Server.Send(CreateSpawnPacket(refindex), Index)
                            PlayerData(Index).SpawnedPlayers.Add(refindex)
                        End If
                    End If
                End If
            Next refindex
        End Sub

        Public Sub DespawnPlayerTeleport(ByVal Index_ As Integer)

            For i = 0 To PlayerData(Index_).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index_).SpawnedPlayers(i)
                If PlayerData(Other_Index).SpawnedPlayers.Contains(Index_) = True Then
                    Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueId), Other_Index)
                    PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)

                    Server.Send(CreateDespawnPacket(PlayerData(Other_Index).UniqueId), Index_)
                    PlayerData(Index_).SpawnedPlayers.Remove(Other_Index)
                End If
            Next


            For i = 0 To NpcList.Count - 1
                If PlayerData(Index_).SpawnedNPCs.Contains(i) = True Then
                    Dim _npc As cNPC = NpcList(i)
                    Server.Send(CreateDespawnPacket(_npc.UniqueID), Index_)
                    PlayerData(Index_).SpawnedNPCs.Remove(i)
                End If
            Next


            For i = 0 To MobList.Count - 1
                If PlayerData(Index_).SpawnedMonsters.Contains(i) = True Then
                    Dim _mob As cMonster = MobList(i)
                    Server.Send(CreateDespawnPacket(_mob.UniqueID), Index_)
                    PlayerData(Index_).SpawnedMonsters.Remove(i)
                End If
            Next

        End Sub

        Public Sub DespawnPlayer(ByVal Index_ As Integer)
            For i = 0 To PlayerData(Index_).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index_).SpawnedPlayers(i)
                If PlayerData(Other_Index).SpawnedPlayers.Contains(Index_) = True Then
                    Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueId), Other_Index)
                    PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)
                End If
            Next

            PlayerData(Index_).SpawnedNPCs.Clear()
            PlayerData(Index_).SpawnedMonsters.Clear()
        End Sub

        Public Sub DespawnPlayerRange(ByVal Index_ As Integer)
            Dim range As Integer = ServerRange

            For i = 0 To PlayerData(Index_).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index_).SpawnedPlayers(i)
                If PlayerData(i) IsNot Nothing Then
                    Dim distance As Long = Math.Round(Math.Sqrt((PlayerData(Index_).Position.X - PlayerData(Other_Index).Position.X) ^ 2 + (PlayerData(Index_).Position.Y - PlayerData(Other_Index).Position.Y) ^ 2)) 'Calculate Distance
                    If distance > range Then
                        'Despawn for both
                        If PlayerData(Index_).SpawnedPlayers.Contains(Other_Index) Then
                            Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueId), Other_Index)
                            PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)
                            Server.Send(CreateDespawnPacket(PlayerData(Other_Index).UniqueId), Index_)
                            PlayerData(Index_).SpawnedPlayers.Remove(Other_Index)
                        End If
                    End If
                End If
            Next
        End Sub

    End Module
End Namespace
