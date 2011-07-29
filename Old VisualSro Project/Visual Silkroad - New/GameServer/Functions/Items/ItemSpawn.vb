Namespace GameServer.Functions
    Module ItemSpawn

        Public Function CreateItemSpawnPacket(ByVal Item_ As cItemDrop) As Byte()
            Dim refitem As cItem = GetItemByID(Item_.Item.Pk2Id)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleSpawn)
            writer.DWord(Item_.Item.Pk2Id)
            Select Case refitem.CLASS_A
                Case 1 'Equipment
                    writer.Byte(Item_.Item.Plus)
                Case 3 'Etc
                    If refitem.CLASS_B = 5 And refitem.CLASS_C = 0 Then
                        'Gold...
                        writer.DWord(Item_.Item.Amount)
                    End If
            End Select
            writer.DWord(Item_.UniqueID)
            writer.Byte(Item_.Position.XSector)
            writer.Byte(Item_.Position.YSector)
            writer.Float(Item_.Position.X)
            writer.Float(Item_.Position.Z)
            writer.Float(Item_.Position.Y)
            writer.Word(0) 'angle

            writer.Byte(0)
            ' writer.DWord(UInt32.MaxValue)
            writer.Byte(0)
            writer.Byte(6)
            writer.DWord(Item_.DroppedBy)

            Return writer.GetBytes
        End Function

        Public Function DropItem(ByVal Item As cInvItem, ByVal Position As Position) As UInteger
            Dim tmp_ As New cItemDrop
            tmp_.UniqueID = Id_Gen.GetUnqiueId
            tmp_.DroppedBy = Item.OwnerCharID
            tmp_.Position = Position
            tmp_.Item = FillItem(Item)
            tmp_.DespawnTime = Date.Now.AddMinutes(3)

            If tmp_.Item.Pk2Id = 1 Then
                'Gold...
                If tmp_.Item.Amount <= 1000 Then
                    tmp_.Item.Pk2Id = 1
                ElseIf tmp_.Item.Amount > 1000 And tmp_.Item.Amount <= 10000 Then
                    tmp_.Item.Pk2Id = 2
                ElseIf tmp_.Item.Amount > 10000 Then
                    tmp_.Item.Pk2Id = 3
                End If
            End If

            ItemList.Add(tmp_.UniqueID, tmp_)

            For refindex As Integer = 0 To Server.MaxClients
                Dim socket As Net.Sockets.Socket = ClientList.GetSocket(refindex)
                Dim player As [cChar] = PlayerData(refindex) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                    If CheckRange(player.Position, Position) Then
                        If PlayerData(refindex).SpawnedItems.Contains(tmp_.UniqueID) = False Then
                            Server.Send(CreateItemSpawnPacket(tmp_), refindex)
                            PlayerData(refindex).SpawnedItems.Add(tmp_.UniqueID)
                        End If
                    End If
                End If
            Next refindex

            Return tmp_.UniqueID
        End Function

        Public Sub RemoveItem(ByVal UniqueId As UInteger)
            Dim _item As cItemDrop = ItemList(UniqueId)
            Server.SendIfItemIsSpawned(CreateDespawnPacket(_item.UniqueID), _item.UniqueID)
            ItemList.Remove(UniqueId)


            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).SpawnedItems.Contains(_item.UniqueID) = True Then
                        PlayerData(i).SpawnedItems.Remove(_item.UniqueID)
                    End If
                End If
            Next

        End Sub
    End Module
End Namespace
