Imports SRFramework
Imports System.Net.Sockets

Namespace Functions
    Module ObjectSpawn
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
                    For Each tmpPacket In spawnCollector.GetPackets(GroupSpawnMode.SPAWN)
                        PlayerData(Index_).GroupSpawnPacketsToSend.Enqueue(tmpPacket)
                    Next
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
                    For Each tmpPacket In spawnCollector.GetPackets(GroupSpawnMode.SPAWN)
                        PlayerData(Index_).GroupSpawnPacketsToSend.Enqueue(tmpPacket)
                    Next
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
                    For Each tmpPacket In spawnCollector.GetPackets(GroupSpawnMode.SPAWN)
                        PlayerData(Index_).GroupSpawnPacketsToSend.Enqueue(tmpPacket)
                    Next
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
                    For Each tmpPacket In spawnCollector.GetPackets(GroupSpawnMode.SPAWN)
                        PlayerData(Index_).GroupSpawnPacketsToSend.Enqueue(tmpPacket)
                    Next
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
                    For Each tmpPacket In spawnCollector.GetPackets(GroupSpawnMode.DESPAWN)
                        PlayerData(Index_).GroupSpawnPacketsToSend.Enqueue(tmpPacket)
                    Next
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
                    For Each tmpPacket In spawnCollector.GetPackets(GroupSpawnMode.DESPAWN)
                        PlayerData(Index_).GroupSpawnPacketsToSend.Enqueue(tmpPacket)
                    Next
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
                    For Each tmpPacket In spawnCollector.GetPackets(GroupSpawnMode.DESPAWN)
                        PlayerData(Index_).GroupSpawnPacketsToSend.Enqueue(tmpPacket)
                    Next
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
                    For Each tmpPacket In spawnCollector.GetPackets(GroupSpawnMode.DESPAWN)
                        PlayerData(Index_).GroupSpawnPacketsToSend.Enqueue(tmpPacket)
                    Next
                    spawnCollector.Clear()
                End If

            Catch ex As Exception
                Log.WriteSystemLog("DeSpawnCheckError:: Message: " & ex.Message & " Stack:" & ex.StackTrace)
            End Try
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
