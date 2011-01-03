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

            writer.Byte(1)
            writer.DWord(UInt32.MaxValue)
            writer.Byte(0)
            writer.Byte(6)
            writer.DWord(Item_.DroppedBy)

            Return writer.GetBytes
        End Function

        Public Sub DropItem(ByVal Item As cInvItem, ByVal Position As Position)
            Dim tmp_ As New cItemDrop
            tmp_.UniqueID = GameDB.GetUnqiueID
            tmp_.DroppedBy = Item.OwnerCharID
            tmp_.Position = Position
            tmp_.Item = FillItem(Item)

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
            Server.SendIfItemIsSpawned(CreateDespawnPacket(_item.UniqueID), _item.UniqueID)
            ItemList.RemoveAt(ItemIndex)


            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).SpawnedItems.Contains(_item.UniqueID) = True Then
                        PlayerData(i).SpawnedItems.Remove(_item.UniqueID)
                    End If
                End If
            Next

        End Sub

        Public Sub PickUp(ByVal ItemIndex As UInteger, ByVal Index_ As Integer)
            Dim distance As Double = CalculateDistance(PlayerData(Index_).Position, ItemList(ItemIndex).Position)

            If distance >= 5 Then
                'Out Of Range
                OnMoveUser(Index_, ItemList(ItemIndex).Position)
                Dim Traveltime = distance / PlayerData(Index_).RunSpeed
                PickUpTimer(Index_).Interval = Traveltime
                PickUpTimer(Index_).Start()

            Else
                UpdateState(1, 1, Index_)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.PickUp_Move)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Byte(ItemList(ItemIndex).Position.XSector)
                writer.Byte(ItemList(ItemIndex).Position.YSector)
                writer.Float(ItemList(ItemIndex).Position.X)
                writer.Float(ItemList(ItemIndex).Position.X)
                writer.Float(ItemList(ItemIndex).Position.X)
                writer.Word(0)
                Server.SendToAllInRange(writer.GetBytes, PlayerData(Index_).Position)

                writer.Create(ServerOpcodes.PickUp_Item)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Byte(0)

                If ItemList(Index_).Item.Pk2Id = 1 Or ItemList(Index_).Item.Pk2Id = 2 Or ItemList(Index_).Item.Pk2Id = 3 Then
                    If ItemList(Index_).Item.Amount > 0 Then
                        PlayerData(Index_).Gold += ItemList(Index_).Item.Amount
                        UpdateGold(Index_)
                    End If
                Else
                    Dim slot As Byte = GetFreeItemSlot(Index_)
                    Dim ref As cItem = GetItemByID(ItemList(Index_).Item.Pk2Id)
                    Dim temp_item As cInvItem = Inventorys(Index_).UserItems(slot)

                    temp_item.Pk2Id = ItemList(Index_).Item.Pk2Id
                    temp_item.OwnerCharID = PlayerData(Index_).CharacterId
                    temp_item.Durability = ItemList(Index_).Item.Durability
                    temp_item.Plus = ItemList(Index_).Item.Plus
                    temp_item.Amount = ItemList(Index_).Item.Amount

                    UpdateItem(Inventorys(Index_).UserItems(slot)) 'SAVE IT


                    writer.Create(ServerOpcodes.ItemMove)
                    writer.Byte(1)
                    writer.Byte(6) 'type = new item
                    writer.Byte(Inventorys(Index_).UserItems(slot).Slot)

                    AddItemDataToPacket(Inventorys(Index_).UserItems(slot), writer)

                    Server.Send(writer.GetBytes, Index_)
                End If
            End If




        End Sub
    End Module

    Class cItemDrop
        Public UniqueID As UInteger
        Public DroppedBy As UInteger
        Public Position As Position
        Public Item As cInvItem
    End Class
End Namespace
