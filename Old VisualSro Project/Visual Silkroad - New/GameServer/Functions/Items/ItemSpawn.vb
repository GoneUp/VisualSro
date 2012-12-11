Imports System.Net.Sockets
Imports SRFramework

Namespace Functions
    Module ItemSpawn
        Public Sub CreateItemSpawnPacket(ByVal Item_ As cItemDrop, ByVal writer As PacketWriter, ByVal includePacketHeader As Boolean)
            Dim refitem As cRefItem = GetItemByID(Item_.Item.ObjectID)

            If includePacketHeader Then
                writer.Create(ServerOpcodes.GAME_SINGLE_SPAWN)
            End If

            writer.DWord(Item_.Item.ObjectID)
            Select Case refitem.CLASS_A
                Case 1 'Equipment
                    writer.Byte(Item_.Item.Plus)
                Case 3 'Etc
                    If refitem.CLASS_B = 5 And refitem.CLASS_C = 0 Then
                        'Gold...
                        writer.DWord(Item_.Item.Data)
                    End If
            End Select
            writer.DWord(Item_.UniqueID)
            writer.Byte(Item_.Position.XSector)
            writer.Byte(Item_.Position.YSector)
            writer.Float(Item_.Position.X)
            writer.Float(Item_.Position.Z)
            writer.Float(Item_.Position.Y)
            writer.Word(0)  'angle

            writer.Byte(0) ' writer.DWord(UInt32.MaxValue)
            writer.Byte(0)
            writer.Byte(6)
            writer.DWord(Item_.DroppedBy)
        End Sub

        Public Function DropItem(ByVal invItem As cInventoryItem, ByVal item As cItem, ByVal position As Position) As UInteger
            Dim tmp_ As New cItemDrop
            tmp_.UniqueID = Id_Gen.GetUnqiueId
            tmp_.DroppedBy = invItem.OwnerID
            tmp_.Position = Position
            tmp_.ChannelId = Settings.ServerWorldChannel
            tmp_.Item = item
            tmp_.DespawnTime = Date.Now.AddMinutes(3)

            If tmp_.Item.ObjectID = 1 Then
                'Gold...
                If tmp_.Item.Data <= 1000 Then
                    tmp_.Item.ObjectID = 1
                ElseIf tmp_.Item.Data > 1000 And tmp_.Item.Data <= 10000 Then
                    tmp_.Item.ObjectID = 2
                ElseIf tmp_.Item.Data > 10000 Then
                    tmp_.Item.ObjectID = 3
                End If
            End If

            ItemList.Add(tmp_.UniqueID, tmp_)

            For refindex As Integer = 0 To Server.MaxClients - 1
                Dim socket As Socket = Server.ClientList.GetSocket(refindex)
                Dim player As cCharacter = PlayerData(refindex)
                'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                    If CheckRange(player.Position, Position) Then
                        If PlayerData(refindex).SpawnedItems.Contains(tmp_.UniqueID) = False Then
                            Dim tmpSpawn As New GroupSpawn
                            tmpSpawn.AddObject(tmp_.UniqueID)
                            Server.Send(tmpSpawn.GetBytes(GroupSpawn.GroupSpawnMode.SPAWN), refindex)
                            PlayerData(refindex).SpawnedItems.Add(tmp_.UniqueID)
                        End If
                    End If
                End If
            Next refindex

            Return tmp_.UniqueID
        End Function

        Public Sub RemoveItem(ByVal uniqueId As UInteger)
            Dim item As cItemDrop = ItemList(UniqueId)
            Server.SendIfItemIsSpawned(CreateSingleDespawnPacket(item.UniqueID), item.UniqueID)
            ItemList.Remove(UniqueId)


            For i = 0 To Server.MaxClients - 1
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).SpawnedItems.Contains(item.UniqueID) = True Then
                        PlayerData(i).SpawnedItems.Remove(item.UniqueID)
                    End If
                End If
            Next
        End Sub
    End Module
End Namespace
