Namespace GameServer.Functions
    Module Movement

        Public Sub OnPlayerMovement(ByVal Index_ As Integer, ByVal packet As PacketReader)

            If PlayerData(Index_).Busy = True Or PlayerData(Index_).Attacking = True Then
                Exit Sub
            End If

            Dim tag As Byte = packet.Byte
            If tag = 1 Then
                Dim to_pos As New Position
                to_pos.XSector = packet.Byte
                to_pos.YSector = packet.Byte

                If IsInCave(to_pos) = False Then
                    to_pos.X = packet.WordInt
                    to_pos.Z = packet.WordInt
                    to_pos.Y = packet.WordInt
                    Debug.Print("x: " & to_pos.X & " y: " & to_pos.Y)
                Else
                    'In Cave
                    to_pos.X = packet.DWordInt
                    to_pos.Z = packet.DWordInt
                    to_pos.Y = packet.DWordInt
                End If


                OnMoveUser(Index_, to_pos)
            ElseIf tag = 0 Then
                Dim tag2 As Byte = packet.Byte
                Dim to_angle As UShort = packet.Word
                Dim to_grad As Single = (to_angle / 65535) * 360
                SendPm(Index_, "You are tyring to Angle Move to: " & to_grad, "Debug")

            End If
        End Sub

        Public Sub OnMoveUser(ByVal Index_ As Integer, ByVal ToPos As Position)
            Try
                Dim Distance As Single = CalculateDistance(PlayerData(Index_).Position, ToPos)
                Dim WalkTime As Single
                Select Case PlayerData(Index_).Pos_Tracker.SpeedMode
                    Case cPositionTracker.enumSpeedMode.Walking
                        WalkTime = (Distance / PlayerData(Index_).WalkSpeed) * 10000
                    Case cPositionTracker.enumSpeedMode.Running
                        WalkTime = (Distance / PlayerData(Index_).RunSpeed) * 10000
                    Case cPositionTracker.enumSpeedMode.Zerking
                        WalkTime = (Distance / PlayerData(Index_).BerserkSpeed) * 10000
                End Select

                'If Distance < 10000 Then

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Movement)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Byte(1) 'destination
                writer.Byte(ToPos.XSector)
                writer.Byte(ToPos.YSector)

                If IsInCave(ToPos) = False Then
                    writer.Byte(BitConverter.GetBytes(CShort(ToPos.X)))
                    writer.Byte(BitConverter.GetBytes(CShort(ToPos.Z)))
                    writer.Byte(BitConverter.GetBytes(CShort(ToPos.Y)))
                Else
                    'In Cave
                    writer.Byte(BitConverter.GetBytes(CInt(ToPos.X)))
                    writer.Byte(BitConverter.GetBytes(CInt(ToPos.Z)))
                    writer.Byte(BitConverter.GetBytes(CInt(ToPos.Y)))
                End If

                writer.Byte(1) '1= source
                writer.Byte(PlayerData(Index_).Position.XSector)
                writer.Byte(PlayerData(Index_).Position.YSector)
                writer.Byte(BitConverter.GetBytes(CShort(PlayerData(Index_).Position.X * -1)))
                writer.Byte(BitConverter.GetBytes(PlayerData(Index_).Position.Z))
                writer.Byte(BitConverter.GetBytes(CShort(PlayerData(Index_).Position.Y * -1)))


                DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

                PlayerData(Index_).Pos_Tracker.Move(ToPos)
                PlayerMoveTimer(Index_).Interval = 250
                PlayerMoveTimer(Index_).Start()
                ' End If

            Catch ex As Exception
                Console.WriteLine("OnMoveUser::error...")
                Debug.Write(ex)
            End Try

        End Sub

        ''' <summary>
        ''' Moves a User To a Object Based on the Range
        ''' </summary>
        ''' <param name="Index_"></param>
        ''' <param name="ObjectPos"></param>
        ''' <param name="Range"></param>
        ''' <returns>The Walktime in ms</returns>
        ''' <remarks></remarks>
        Public Function MoveUserToObject(ByVal Index_ As Integer, ByVal ObjectPos As Position, ByVal Range As Integer) As Single
            Dim ToPos As Position = ObjectPos

            Dim distance_x As Double = PlayerData(Index_).Position.ToGameX - ToPos.ToGameX
            Dim distance_y As Double = PlayerData(Index_).Position.ToGameY - ToPos.ToGameY
            Dim distance As Double = Math.Sqrt((distance_x * distance_x) + (distance_y * distance_y))

            If distance > Range Then
                Dim Cosinus As Double = Math.Cos(distance_x / distance)
                Dim Sinus As Double = Math.Sin(distance_y / distance)

                Dim distance_x_new As Double = Range * Cosinus
                Dim distance_y_new As Double = Range * Sinus

                ToPos.X = GetXOffset(ToPos.ToGameX + distance_x_new)
                ToPos.Y = GetYOffset(ToPos.ToGameY + distance_y_new)
                ToPos.XSector = GetXSecFromGameX(ToPos.ToGameX)
                ToPos.YSector = GetYSecFromGameY(ToPos.ToGameY)
                Debug.Print(ToPos.ToGameX & " Y " & ToPos.ToGameY)
            End If

            Dim WalkTime As Single
            Select Case PlayerData(Index_).Pos_Tracker.SpeedMode
                Case cPositionTracker.enumSpeedMode.Walking
                    WalkTime = (distance / PlayerData(Index_).WalkSpeed) * 10000
                Case cPositionTracker.enumSpeedMode.Running
                    WalkTime = (distance / PlayerData(Index_).RunSpeed) * 10000
                Case cPositionTracker.enumSpeedMode.Zerking
                    WalkTime = (distance / PlayerData(Index_).BerserkSpeed) * 10000
            End Select

            OnMoveUser(Index_, ToPos)
            Return WalkTime
        End Function
        Public Sub ObjectSpawnCheck(ByVal Index_ As Integer)
            Try
                ObjectDeSpawnCheck(Index_)

                '=============Players============
                For refindex As Integer = 0 To Server.MaxClients
                    Dim othersock As Net.Sockets.Socket = ClientList.GetSocket(refindex)
                    If (othersock IsNot Nothing) AndAlso (PlayerData(refindex) IsNot Nothing) AndAlso (othersock.Connected) AndAlso Index_ <> refindex Then
                        If CheckRange(PlayerData(Index_).Pos_Tracker.GetCurPos, PlayerData(refindex).Position) Then
                            If PlayerData(refindex).SpawnedPlayers.Contains(Index_) = False And PlayerData(Index_).Invisible = False Then
                                Server.Send(CreateSpawnPacket(Index_), refindex)
                                PlayerData(refindex).SpawnedPlayers.Add(Index_)
                            End If
                            If PlayerData(Index_).SpawnedPlayers.Contains(refindex) = False Then
                                Server.Send(CreateSpawnPacket(refindex), Index_)
                                PlayerData(Index_).SpawnedPlayers.Add(refindex)
                            End If
                        End If
                    End If
                Next refindex
                '===========MOBS===================

                For Each key In MobList.Keys.ToList
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)
                        If CheckRange(PlayerData(Index_).Pos_Tracker.GetCurPos, Mob_.Position) Then
                            Dim obj As Object = GetObjectById(Mob_.Pk2ID)
                            If PlayerData(Index_).SpawnedMonsters.Contains(Mob_.UniqueID) = False Then
                                Server.Send(CreateMonsterSpawnPacket(Mob_, obj), Index_)
                                PlayerData(Index_).SpawnedMonsters.Add(Mob_.UniqueID)
                            End If
                        End If
                    End If
                Next

                '===========NPCS===================
                For Each key In NpcList.Keys.ToList
                    If NpcList.ContainsKey(key) Then
                        Dim Npc_ As cNPC = NpcList.Item(key)
                        If CheckRange(PlayerData(Index_).Pos_Tracker.GetCurPos, Npc_.Position) Then
                            If PlayerData(Index_).SpawnedNPCs.Contains(Npc_.UniqueID) = False Then
                                Server.Send(CreateNPCGroupSpawnPacket(Npc_.UniqueID), Index_)
                                PlayerData(Index_).SpawnedNPCs.Add(Npc_.UniqueID)
                            End If
                        End If
                    End If
                Next


                '===========ITEMS===================
                For Each key In ItemList.Keys.ToList
                    If ItemList.ContainsKey(key) Then
                        Dim Item_ As cItemDrop = ItemList(key)
                        If CheckRange(PlayerData(Index_).Pos_Tracker.GetCurPos, ItemList(key).Position) Then
                            If PlayerData(Index_).SpawnedItems.Contains(Item_.UniqueID) = False Then
                                Server.Send(CreateItemSpawnPacket(Item_), Index_)
                                PlayerData(Index_).SpawnedItems.Add(Item_.UniqueID)
                            End If
                        End If
                    End If
                Next


            Catch ex As Exception
                Log.WriteSystemLog("SpawnCheckError:: Message: " & ex.Message & " Stack:" & ex.StackTrace)
            End Try
        End Sub


        Public Sub ObjectDeSpawnCheck(ByVal Index_ As Integer)
            Try
                For Other_Index = 0 To Server.MaxClients
                    If PlayerData(Other_Index) IsNot Nothing And PlayerData(Index_).SpawnedPlayers.Contains(Other_Index) Then
                        If CheckRange(PlayerData(Index_).Pos_Tracker.GetCurPos, PlayerData(Other_Index).Position) = False Then
                            'Despawn for both
                            Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueId), Other_Index)
                            PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)
                            Server.Send(CreateDespawnPacket(PlayerData(Other_Index).UniqueId), Index_)
                            PlayerData(Index_).SpawnedPlayers.Remove(Other_Index)
                        End If
                    End If
                Next

                For Each key In MobList.Keys.ToList
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)
                        If PlayerData(Index_).SpawnedMonsters.Contains(Mob_.UniqueID) = True Then
                            If CheckRange(PlayerData(Index_).Pos_Tracker.GetCurPos, Mob_.Position) = False Then
                                Server.Send(CreateDespawnPacket(Mob_.UniqueID), Index_)
                                PlayerData(Index_).SpawnedMonsters.Remove(Mob_.UniqueID)
                            End If
                        End If
                    End If
                Next

                For Each key In NpcList.Keys.ToList
                    If NpcList.ContainsKey(key) Then
                        Dim Npc_ As cNPC = NpcList.Item(key)
                        If PlayerData(Index_).SpawnedNPCs.Contains(Npc_.UniqueID) = True Then
                            If CheckRange(PlayerData(Index_).Pos_Tracker.GetCurPos, Npc_.Position) = False Then
                                Server.Send(CreateDespawnPacket(Npc_.UniqueID), Index_)
                                PlayerData(Index_).SpawnedNPCs.Remove(Npc_.UniqueID)
                            End If
                        End If
                    End If
                Next


                For Each key In ItemList.Keys.ToList
                    If ItemList.ContainsKey(key) Then
                        Dim _item As cItemDrop = ItemList(key)
                        If PlayerData(Index_).SpawnedItems.Contains(_item.UniqueID) = True Then
                            If CheckRange(PlayerData(Index_).Pos_Tracker.GetCurPos, _item.Position) = False Then
                                Server.Send(CreateDespawnPacket(_item.UniqueID), Index_)
                                PlayerData(Index_).SpawnedItems.Remove(_item.UniqueID)
                            End If
                        End If
                    End If
                Next
            Catch ex As Exception
                Log.WriteSystemLog("DeSpawnCheckError:: Message: " & ex.Message & " Stack:" & ex.StackTrace)
            End Try
        End Sub

        Public Sub CheckForCaveTeleporter(ByVal Index_ As Integer)
            For i = 0 To RefCaveTeleporter.Count - 1
                If CalculateDistance(PlayerData(Index_).Position, RefCaveTeleporter(i).FromPosition) <= RefCaveTeleporter(i).Range Then
                    'In Range --> Teleport
                    Dim Point_ As TeleportPoint_ = GetTeleportPoint(RefCaveTeleporter(i).ToTeleporterID)

                    If PlayerData(Index_).Level < Point_.MinLevel And Point_.MinLevel > 0 Then
                        'Level too low
                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.Teleport_Start)
                        writer.Byte(2)
                        writer.Byte(&H15)
                        writer.Byte(&H1C)
                        Server.Send(writer.GetBytes, Index_)
                    ElseIf PlayerData(Index_).Level > Point_.MaxLevel And Point_.MaxLevel > 0 Then
                        'Level too high
                        SendNotice("Cannot Teleport because your Level is too high.", Index_)
                    ElseIf PlayerData(Index_).Busy = False Then
                        PlayerData(Index_).Busy = True
                        PlayerData(Index_).Position = Point_.ToPos

                        DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                    End If
                End If
            Next
        End Sub

        Public Function CheckRange(ByVal Pos_1 As Position, ByVal Pos_2 As Position) As Boolean
            If CalculateDistance(Pos_1, Pos_2) <= Settings.Server_Range Then
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
