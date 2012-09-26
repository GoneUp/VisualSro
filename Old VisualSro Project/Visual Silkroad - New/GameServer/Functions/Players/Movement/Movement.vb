Imports System.Net.Sockets
Imports SRFramework

Namespace Functions
    Module Movement
        Public Sub OnPlayerMovement(ByVal Index_ As Integer, ByVal packet As PacketReader)

            If PlayerData(Index_).Busy = True Or PlayerData(Index_).Attacking = True Then
                Exit Sub
            End If

            Dim tag As Byte = packet.Byte
            If tag = 1 Then
                Dim toPos As New Position
                toPos.XSector = packet.Byte
                toPos.YSector = packet.Byte

                If IsInCave(toPos) = False Then
                    toPos.X = packet.WordInt
                    toPos.Z = packet.WordInt
                    toPos.Y = packet.WordInt
                    Debug.Print("x: " & toPos.X & " y: " & toPos.Y)
                Else
                    'In Cave
                    toPos.X = packet.DWordInt
                    toPos.Z = packet.DWordInt
                    toPos.Y = packet.DWordInt
                End If


                OnMoveUser(Index_, toPos)
            ElseIf tag = 0 Then
                Dim tag2 As Byte = packet.Byte
                Dim toAngle As UShort = packet.Word
                Dim toGrad As Single = (toAngle / 65535) * 360
                SendPm(Index_, "You are tyring to Angle Move to: " & toGrad, "Debug")

            End If
        End Sub

        Public Sub OnMoveUser(ByVal Index_ As Integer, ByVal toPos As Position)
            Try
                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_MOVEMENT)
                writer.DWord(PlayerData(Index_).UniqueID)
                writer.Byte(1)
                'destination
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

                writer.Byte(1)
                '1= source
                writer.Byte(PlayerData(Index_).Position.XSector)
                writer.Byte(PlayerData(Index_).Position.YSector)
                writer.Byte(BitConverter.GetBytes(CShort(PlayerData(Index_).Position.X * -1)))
                writer.Byte(BitConverter.GetBytes(PlayerData(Index_).Position.Z))
                writer.Byte(BitConverter.GetBytes(CShort(PlayerData(Index_).Position.Y * -1)))

                GameDB.SavePosition(Index_)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

                PlayerData(Index_).PosTracker.Move(ToPos)
                PlayerMoveTimer(Index_).Interval = 100
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
        ''' <param name="objectPos"></param>
        ''' <param name="Range"></param>
        ''' <returns>The Walktime in ms</returns>
        ''' <remarks></remarks>
        Public Function MoveUserToObject(ByVal Index_ As Integer, ByVal objectPos As Position, ByVal Range As Integer) As Single
            Dim toPos As Position = objectPos

            Dim distanceX As Double = PlayerData(Index_).Position.ToGameX - toPos.ToGameX
            Dim distanceY As Double = PlayerData(Index_).Position.ToGameY - toPos.ToGameY
            Dim distance As Double = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY))

            If distance > Range Then
                Dim cosinus As Double = Math.Cos(distanceX / distance)
                Dim sinus As Double = Math.Sin(distanceY / distance)

                Dim distanceXNew As Double = Range * cosinus
                Dim distanceYNew As Double = Range * sinus

                toPos.X = GetXOffset(toPos.ToGameX + distanceXNew)
                toPos.Y = GetYOffset(toPos.ToGameY + distanceYNew)
                toPos.XSector = GetXSecFromGameX(toPos.ToGameX)
                toPos.YSector = GetYSecFromGameY(toPos.ToGameY)
                Debug.Print(toPos.ToGameX & " Y " & toPos.ToGameY)
            End If

            Dim walkTime As Single
            Select Case PlayerData(Index_).PosTracker.SpeedMode
                Case cPositionTracker.enumSpeedMode.Walking
                    walkTime = (distance / PlayerData(Index_).WalkSpeed) * 10000
                Case cPositionTracker.enumSpeedMode.Running
                    walkTime = (distance / PlayerData(Index_).RunSpeed) * 10000
                Case cPositionTracker.enumSpeedMode.Zerking
                    walkTime = (distance / PlayerData(Index_).BerserkSpeed) * 10000
            End Select

            OnMoveUser(Index_, toPos)
            Return walkTime
        End Function

        Public Sub ObjectSpawnCheck(ByVal Index_ As Integer)
            Try
                ObjectDeSpawnCheck(Index_)

                Dim spawnCollector As New GroupSpawn

                '=============Players============
                For refindex As Integer = 0 To Server.MaxClients - 1
                    Dim othersock As Socket = Server.ClientList.GetSocket(refindex)
                    'Socket checks..
                    If (othersock IsNot Nothing) AndAlso (PlayerData(refindex) IsNot Nothing) AndAlso (othersock.Connected) AndAlso Index_ <> refindex Then
                        'Player in Range?
                        If CheckRange(PlayerData(Index_).PosTracker.GetCurPos, PlayerData(refindex).Position) Then
                            'Channel Check..
                            If PlayerData(Index_).ChannelId = PlayerData(refindex).ChannelId Or PlayerData(Index_).AvoidChannels Or PlayerData(refindex).AvoidChannels Then
                                'Already spawned?
                                If PlayerData(refindex).SpawnedPlayers.Contains(Index_) = False And PlayerData(Index_).Invisible = False Then
                                    'To opponent
                                    Dim writer As New PacketWriter
                                    CreatePlayerSpawnPacket(Index_, writer, True)
                                    Server.Send(writer.GetBytes, refindex)

                                    PlayerData(refindex).SpawnedPlayers.Add(Index_)
                                End If
                                If PlayerData(Index_).SpawnedPlayers.Contains(refindex) = False Then
                                    'To me
                                    spawnCollector.AddObject(PlayerData(refindex).UniqueID)

                                    PlayerData(Index_).SpawnedPlayers.Add(refindex)
                                End If
                            End If
                        End If
                    End If
                Next refindex
                If spawnCollector.Count > 0 Then
                    spawnCollector.Send(Index_, GroupSpawn.GroupSpawnMode.SPAWN)
                    spawnCollector.Clear()
                End If

                '===========MOBS===================

                For Each key In MobList.Keys.ToList
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)
                        If CheckRange(PlayerData(Index_).PosTracker.GetCurPos, Mob_.Position) Then
                            If PlayerData(Index_).ChannelId = Mob_.ChannelId Or PlayerData(Index_).AvoidChannels Or Mob_.AvoidChannels Then
                                Dim obj As Object = GetObject(Mob_.Pk2ID)
                                If PlayerData(Index_).SpawnedMonsters.Contains(Mob_.UniqueID) = False Then
                                    spawnCollector.AddObject(Mob_.UniqueID)
                                    PlayerData(Index_).SpawnedMonsters.Add(Mob_.UniqueID)
                                End If
                            End If
                        End If
                    End If
                Next
                If spawnCollector.Count > 0 Then
                    spawnCollector.Send(Index_, GroupSpawn.GroupSpawnMode.SPAWN)
                    spawnCollector.Clear()
                End If

                '===========NPCS===================

                For Each key In NpcList.Keys.ToList
                    If NpcList.ContainsKey(key) Then
                        Dim Npc_ As cNPC = NpcList.Item(key)
                        If CheckRange(PlayerData(Index_).PosTracker.GetCurPos, Npc_.Position) Then
                            If PlayerData(Index_).ChannelId = Npc_.ChannelId Or PlayerData(Index_).AvoidChannels Or Npc_.AvoidChannels Then
                                If PlayerData(Index_).SpawnedNPCs.Contains(Npc_.UniqueID) = False Then
                                    spawnCollector.AddObject(Npc_.UniqueID)
                                    PlayerData(Index_).SpawnedNPCs.Add(Npc_.UniqueID)
                                End If
                            End If
                        End If
                    End If
                Next
                If spawnCollector.Count > 0 Then
                    spawnCollector.Send(Index_, GroupSpawn.GroupSpawnMode.SPAWN)
                    spawnCollector.Clear()
                End If


                '===========ITEMS===================
                For Each key In ItemList.Keys.ToList
                    If ItemList.ContainsKey(key) Then
                        Dim Item_ As cItemDrop = ItemList(key)
                        If CheckRange(PlayerData(Index_).PosTracker.GetCurPos, ItemList(key).Position) Then
                            If PlayerData(Index_).ChannelId = Item_.ChannelId Or PlayerData(Index_).AvoidChannels Or Item_.AvoidChannels Then
                                If PlayerData(Index_).SpawnedItems.Contains(Item_.UniqueID) = False Then
                                    'spawnCollector.AddObject(Item_.UniqueID)
                                    Dim writer As New PacketWriter
                                    CreateItemSpawnPacket(Item_, writer, True)
                                    Server.Send(writer.GetBytes, Index_)
                                    PlayerData(Index_).SpawnedItems.Add(Item_.UniqueID)
                                End If
                            End If
                        End If
                    End If
                Next
                If spawnCollector.Count > 0 Then
                    spawnCollector.Send(Index_, GroupSpawn.GroupSpawnMode.SPAWN)
                    spawnCollector.Clear()
                End If

            Catch ex As Exception
                Log.WriteSystemLog("SpawnCheckError:: Message: " & ex.Message & " Stack:" & ex.StackTrace)
            End Try
        End Sub


        Private Sub ObjectDeSpawnCheck(ByVal Index_ As Integer)
            Try
                Dim spawnCollector As New GroupSpawn

                For otherIndex = 0 To Server.MaxClients - 1
                    If PlayerData(otherIndex) IsNot Nothing And PlayerData(Index_).SpawnedPlayers.Contains(otherIndex) Then
                        'Despawn if 
                        'a) Player is out of range
                        'b) Player is not in out Channel anymore (expect if AvoidChannels is set on our or on the other Player) 
                        If CheckRange(PlayerData(Index_).PosTracker.GetCurPos, PlayerData(otherIndex).Position) = False Or _
                            (PlayerData(Index_).ChannelId <> PlayerData(otherIndex).ChannelId) And _
                            PlayerData(Index_).AvoidChannels = False And PlayerData(otherIndex).AvoidChannels = False Then

                            'Despawn for both
                            'To oppopnent over an external packet
                            Server.Send(CreateSingleDespawnPacket(PlayerData(Index_).UniqueID), otherIndex)
                            PlayerData(otherIndex).SpawnedPlayers.Remove(Index_)

                            'To me over the groupspawn packet
                            spawnCollector.AddObject(PlayerData(otherIndex).UniqueID)
                            PlayerData(Index_).SpawnedPlayers.Remove(otherIndex)
                        End If
                    End If
                Next
                If spawnCollector.Count > 0 Then
                    spawnCollector.Send(Index_, GroupSpawn.GroupSpawnMode.DESPAWN)
                    spawnCollector.Clear()
                End If

                For Each key In MobList.Keys.ToList
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)
                        If PlayerData(Index_).SpawnedMonsters.Contains(Mob_.UniqueID) = True Then
                            If CheckRange(PlayerData(Index_).PosTracker.GetCurPos, Mob_.Position) = False Or _
                            (PlayerData(Index_).ChannelId <> Mob_.ChannelId) And PlayerData(Index_).AvoidChannels = False And Mob_.AvoidChannels = False Then
                                spawnCollector.AddObject(Mob_.UniqueID)
                                PlayerData(Index_).SpawnedMonsters.Remove(Mob_.UniqueID)
                            End If
                        End If
                    End If
                Next
                If spawnCollector.Count > 0 Then
                    spawnCollector.Send(Index_, GroupSpawn.GroupSpawnMode.DESPAWN)
                    spawnCollector.Clear()
                End If

                For Each key In NpcList.Keys.ToList
                    If NpcList.ContainsKey(key) Then
                        Dim Npc_ As cNPC = NpcList.Item(key)
                        If PlayerData(Index_).SpawnedNPCs.Contains(Npc_.UniqueID) = True Then
                            If CheckRange(PlayerData(Index_).PosTracker.GetCurPos, Npc_.Position) = False Or _
                            (PlayerData(Index_).ChannelId <> Npc_.ChannelId) And PlayerData(Index_).AvoidChannels = False And Npc_.AvoidChannels = False Then

                                spawnCollector.AddObject(Npc_.UniqueID)
                                PlayerData(Index_).SpawnedNPCs.Remove(Npc_.UniqueID)
                            End If
                        End If
                    End If
                Next
                If spawnCollector.Count > 0 Then
                    spawnCollector.Send(Index_, GroupSpawn.GroupSpawnMode.DESPAWN)
                    spawnCollector.Clear()
                End If

                For Each key In ItemList.Keys.ToList
                    If ItemList.ContainsKey(key) Then
                        Dim Item_ As cItemDrop = ItemList(key)
                        If PlayerData(Index_).SpawnedItems.Contains(Item_.UniqueID) = True Then
                            If CheckRange(PlayerData(Index_).PosTracker.GetCurPos, Item_.Position) = False Or _
                            (PlayerData(Index_).ChannelId <> Item_.ChannelId) And PlayerData(Index_).AvoidChannels = False And Item_.AvoidChannels = False Then

                                spawnCollector.AddObject(Item_.UniqueID)
                                PlayerData(Index_).SpawnedItems.Remove(Item_.UniqueID)
                            End If
                        End If
                    End If
                Next
                If spawnCollector.Count > 0 Then
                    spawnCollector.Send(Index_, GroupSpawn.GroupSpawnMode.DESPAWN)
                    spawnCollector.Clear()
                End If

            Catch ex As Exception
                Log.WriteSystemLog("DeSpawnCheckError:: Message: " & ex.Message & " Stack:" & ex.StackTrace)
            End Try
        End Sub

        Public Sub CheckForCaveTeleporter(ByVal Index_ As Integer)
            If PlayerData(Index_) IsNot Nothing Then
                For i = 0 To RefCaveTeleporter.Count - 1
                    If CalculateDistance(PlayerData(Index_).Position, RefCaveTeleporter(i).FromPosition) <= RefCaveTeleporter(i).Range Then
                        'In Range --> Teleport
                        Dim point As TeleportPoint = GetTeleportPointByNumber(RefCaveTeleporter(i).ToTeleporterID)
                        '####################### Notice : Dont work
                        'TODO: Rework this
                        Dim link As TeleportLink = point.Links(0)

                        If PlayerData(Index_).Level < link.MinLevel And link.MinLevel > 0 Then
                            'Level too low
                            Dim writer As New PacketWriter
                            writer.Create(ServerOpcodes.GAME_TELEPORT_START)
                            writer.Byte(2)
                            writer.Byte(&H15)
                            writer.Byte(&H1C)
                            Server.Send(writer.GetBytes, Index_)
                        ElseIf PlayerData(Index_).Level > link.MaxLevel And link.MaxLevel > 0 Then
                            'Level too high
                            SendNotice("Cannot Teleport because your Level is too high.", Index_)
                        ElseIf PlayerData(Index_).Busy = False Then
                            PlayerData(Index_).Busy = True
                            PlayerData(Index_).SetPosition = point.ToPos

                            Database.SaveQuery(
                                String.Format(
                                    "UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'",
                                    PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector,
                                    Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z),
                                    Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                        End If
                    End If
                Next
            End If
        End Sub

        Public Function CheckRange(ByVal pos1 As Position, ByVal pos2 As Position) As Boolean
            If CalculateDistance(pos1, pos2) <= Settings.ServerRange Then
                Return True
            Else
                Return False
            End If
        End Function
    End Module
End Namespace
