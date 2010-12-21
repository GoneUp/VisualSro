Namespace GameServer.Functions
    Module Movement

        Public Sub OnPlayerMovement(ByVal Index_ As Integer, ByVal packet As PacketReader)

            If PlayerData(Index_).Busy = True Or PlayerData(Index_).Attacking = True Then
                Exit Sub
            End If

            Dim tag As Byte = packet.Byte
            If tag = 1 Then
                '01 A8 60 61 02 FE FF 70 04
                Dim to_pos As New Position
                to_pos.XSector = packet.Byte
                to_pos.YSector = packet.Byte
                to_pos.X = packet.WordInt
                to_pos.Z = packet.WordInt
                to_pos.Y = packet.WordInt

                'Dim x = PlayerData(Index_).Position.X - to_pos.X
                'Dim y = PlayerData(Index_).Position.Y - to_pos.Y
                'Dim Distance = Math.Sqrt(x * x + y * y)
                'Dim Traveltime = Distance / PlayerData(Index_).RunSpeed
                'Console.WriteLine(Distance & "    " & Traveltime)

                OnMoveUser(Index_, to_pos)
            End If
        End Sub

        Public Sub OnMoveUser(ByVal Index_ As Integer, ByVal ToPos As Position)
            Try
                Dim Distance As Single = CalculateDistance(PlayerData(Index_).Position, ToPos)
                Dim Time As Single
                Select Case PlayerData(Index_).MovementType
                    Case MoveType_.Walk
                        Time = Distance / PlayerData(Index_).WalkSpeed
                    Case MoveType_.Run
                        Time = Distance / PlayerData(Index_).RunSpeed
                    Case MoveType_.Berserk
                        Time = Distance / PlayerData(Index_).BerserkSpeed
                End Select


                If Time > 0 Then
                    PlayerData(Index_).Walking = True
                    PlayerData(Index_).Position_FromPos = PlayerData(Index_).Position
                    PlayerData(Index_).Position_ToPos = ToPos
                    PlayerData(Index_).WalkStart = Date.Now
                    PlayerData(Index_).WalkEnd = Date.Now.AddSeconds(Time)
                Else
                    Debug.Print("Move ERRRR")
                End If


                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Movement)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Byte(1) 'destination
                writer.Byte(ToPos.XSector)
                writer.Byte(ToPos.YSector)
                writer.Byte(BitConverter.GetBytes(CShort(ToPos.X)))
                writer.Byte(BitConverter.GetBytes(CShort(ToPos.Z)))
                writer.Byte(BitConverter.GetBytes(CShort(ToPos.Y)))
                writer.Byte(0) '1= source
                'writer.Byte(PlayerData(Index_).Position.XSector)
                'writer.Byte(PlayerData(Index_).Position.YSector)
                'writer.Byte(BitConverter.GetBytes(CShort(PlayerData(Index_).Position.X)))
                'writer.Float(PlayerData(Index_).Position.Z)
                'writer.Byte(BitConverter.GetBytes(CShort(PlayerData(Index_).Position.Y)))



                DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                ObjectSpawnCheck(Index_)
                Server.SendToAllInRange(writer.GetBytes, PlayerData(Index_).Position)
            Catch ex As Exception
                Console.WriteLine("OnMoveUser::error...")
                Debug.Write(ex)
            End Try

        End Sub
        Public Sub ObjectSpawnCheck(ByVal Index_ As Integer)
            Try
                Dim range As Integer = ServerRange
                ObjectDeSpawnCheck(Index_)

                '=============Players============
                For refindex As Integer = 0 To Server.MaxClients
                    If (ClientList.GetSocket(refindex) IsNot Nothing) AndAlso (PlayerData(refindex) IsNot Nothing) AndAlso ClientList.GetSocket(refindex).Connected AndAlso Index_ <> refindex Then
                        If CheckRange(PlayerData(Index_).Position, PlayerData(refindex).Position) Then
                            If PlayerData(refindex).SpawnedPlayers.Contains(Index_) = False And PlayerData(Index_).Invisible = False Then
                                Server.Send(CreateSpawnPacket(Index_), refindex)
                                PlayerData(refindex).SpawnedPlayers.Add(Index_)

                                LinkPlayerToGuild(Index_)
                            End If
                            If PlayerData(Index_).SpawnedPlayers.Contains(refindex) = False Then
                                Server.Send(CreateSpawnPacket(refindex), Index_)
                                PlayerData(Index_).SpawnedPlayers.Add(refindex)

                                LinkPlayerToGuild(Index_)
                            End If

                        End If
                    End If
                Next refindex
                '===========MOBS===================

                For i = 0 To MobList.Count - 1
                    If CheckRange(PlayerData(Index_).Position, MobList(i).Position) Then 'And CheckSectors(PlayerData(Index_).Position, MobList(i).Position) Then
                        Dim _mob As cMonster = MobList(i)
                        Dim obj As Object = GetObjectById(_mob.Pk2ID)
                        If PlayerData(Index_).SpawnedMonsters.Contains(_mob.UniqueID) = False Then
                            Server.Send(CreateMonsterSpawnPacket(_mob, obj), Index_)
                            PlayerData(Index_).SpawnedMonsters.Add(_mob.UniqueID)
                        End If
                    End If
                Next

                '===========NPCS===================
                For i = 0 To NpcList.Count - 1
                    Dim _npc As cNPC = NpcList(i)
                    If CheckRange(PlayerData(Index_).Position, NpcList(i).Position) And CheckSectors(PlayerData(Index_).Position, NpcList(i).Position) Then
                        If PlayerData(Index_).SpawnedNPCs.Contains(_npc.UniqueID) = False Then
                            Server.Send(CreateNPCGroupSpawnPacket(i), Index_)
                            PlayerData(Index_).SpawnedNPCs.Add(_npc.UniqueID)
                        End If
                    End If
                Next


                '===========ITEMS===================
                For i = 0 To ItemList.Count - 1
                    Dim _item As cItemDrop = ItemList(i)
                    If CheckRange(PlayerData(Index_).Position, ItemList(i).Position) And CheckSectors(PlayerData(Index_).Position, ItemList(i).Position) Then
                        If PlayerData(Index_).SpawnedItems.Contains(_item.UniqueID) = False Then
                            Server.Send(CreateItemSpawnPacket(_item), Index_)
                            PlayerData(Index_).SpawnedItems.Add(_item.UniqueID)
                        End If
                    End If
                Next


            Catch ex As Exception
                Console.WriteLine("ObjectSpawnCheck()::error...")
                Debug.Write(ex)
            End Try
        End Sub


        Public Sub ObjectDeSpawnCheck(ByVal Index_ As Integer)
            Try
                For Other_Index = 0 To Server.MaxClients
                    If PlayerData(Other_Index) IsNot Nothing And PlayerData(Index_).SpawnedPlayers.Contains(Other_Index) Then
                        If CheckRange(PlayerData(Index_).Position, PlayerData(Other_Index).Position) = False Then
                            'Despawn for both
                            Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueId), Other_Index)
                            PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)
                            Server.Send(CreateDespawnPacket(PlayerData(Other_Index).UniqueId), Index_)
                            PlayerData(Index_).SpawnedPlayers.Remove(Other_Index)
                        End If
                    End If
                Next

                For i = 0 To MobList.Count - 1
                    Dim _mob As cMonster = MobList(i)
                    If PlayerData(Index_).SpawnedMonsters.Contains(_mob.UniqueID) = True Then
                        If CheckRange(PlayerData(Index_).Position, _mob.Position) = False Then
                            Server.Send(CreateDespawnPacket(_mob.UniqueID), Index_)
                            PlayerData(Index_).SpawnedMonsters.Remove(_mob.UniqueID)
                        End If
                    End If
                Next

                For i = 0 To NpcList.Count - 1
                    Dim _npc As cNPC = NpcList(i)
                    If PlayerData(Index_).SpawnedNPCs.Contains(_npc.UniqueID) = True Then
                        If CheckRange(PlayerData(Index_).Position, _npc.Position) = False Then
                            Server.Send(CreateDespawnPacket(_npc.UniqueID), Index_)
                            PlayerData(Index_).SpawnedNPCs.Remove(_npc.UniqueID)
                        End If
                    End If
                Next


                For i = 0 To ItemList.Count - 1
                    Dim _item As cItemDrop = ItemList(i)
                    If PlayerData(Index_).SpawnedItems.Contains(_item.UniqueID) = True Then
                        If CheckRange(PlayerData(Index_).Position, _item.Position) = False Then
                            Server.Send(CreateDespawnPacket(_item.UniqueID), Index_)
                            PlayerData(Index_).SpawnedItems.Remove(_item.UniqueID)
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try
        End Sub


        Public Function CheckRange(ByVal Pos_1 As Position, ByVal Pos_2 As Position) As Boolean
            If CalculateDistance(Pos_1, Pos_2) <= ServerRange Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Is for checking if Mob Spawn is possible
        ''' </summary>
        ''' <param name="Pos_1"></param>
        ''' <param name="Pos_2"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckSectors(ByVal Pos_1 As Position, ByVal Pos_2 As Position) As Boolean

            '############
            '# 1 # 2 # 3#
            '# 4 # 5 # 6#
            '# 7 # 8 # 9#
            '############
            Dim PossibleSectors As New List(Of Position)
            For x = -1 To 1
                For y = -1 To 1
                    Dim pos As New Position
                    pos.XSector = Pos_1.XSector + x
                    pos.YSector = Pos_1.YSector + y
                    PossibleSectors.Add(pos)
                Next
            Next

            For Each e As Position In PossibleSectors
                If e.XSector = Pos_2.XSector And e.YSector = Pos_2.YSector Then
                    Return True
                End If
            Next

            Return False
        End Function
    End Module
End Namespace
