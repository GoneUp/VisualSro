Namespace GameServer.Functions
    Module ItemSpawn

        Public ItemList As New List(Of cItemDrop)

        Public Function CreateItemSpawnPacket(ByVal Item_ As cItemDrop) As Byte()
            Dim refitem As cItem = GetItemByID(Item_.Item.Pk2Id)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleSpawn)
            writer.DWord(Item_.Item.Pk2Id)
            Select Case refitem.CLASS_A
                Case 1 'Equipment
                    writer.Byte(Item_.Item.Plus)
            End Select
            writer.DWord(Item_.UniqueID)
            writer.Byte(Item_.Position.XSector)
            writer.Byte(Item_.Position.YSector)
            writer.Float(Item_.Position.X)
            writer.Float(Item_.Position.Z)
            writer.Float(Item_.Position.Y)

            writer.Byte(1)
            writer.DWord(UInt32.MaxValue)
            writer.Byte(0)
            writer.Byte(6)
            writer.DWord(Item_.DroppedBy)

            Return writer.GetBytes
        End Function

        Public Sub DropItem(ByVal Item As cInvItem, ByVal Position As Position)
            Dim tmp_ As New cItemDrop
            tmp_.UniqueID = DatabaseCore.GetUnqiueID
            tmp_.DroppedBy = Item.OwnerCharID
            tmp_.Position = Position
            tmp_.Item = Item

            ItemList.Add(tmp_)

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
        End Sub

        Public Sub RemoveItem(ByVal ItemIndex As Integer)
            Dim _item As cItemDrop = ItemList(ItemIndex)
            Server.SendIfMobIsSpawned(CreateDespawnPacket(_item.UniqueID), _item.UniqueID)
            ItemList.RemoveAt(ItemIndex)


            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).SpawnedItems.Contains(_item.UniqueID) = True Then
                        PlayerData(i).SpawnedItems.Remove(_item.UniqueID)
                    End If
                End If
            Next

        End Sub
    End Module

    Class cItemDrop
        Public UniqueID As UInteger
        Public DroppedBy As UInteger
        Public Position As Position
        Public Item As cInvItem
    End Class
End Namespace
