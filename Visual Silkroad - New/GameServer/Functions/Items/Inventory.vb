Namespace GameServer.Functions
    Module Inventory
        Public Sub OnInventory(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim type As Byte = packet.Byte

            Select Case type
                Case 0 'normales Movement
                    OnNormalMove(packet, index_)
                Case 4 'Inventory --> Exchange
                    OnExchangeAddItem(packet, index_)
                Case 5 'Exchange --> Inventory
                    OnExchangeRemoveItem(packet, index_)
                Case 7 'drop
                    OnDropItem(packet, index_)
                Case 13  'Exchange Gold
                    OnExchangeAddGold(packet, index_)

                Case Else
                    Debug.Print("[INVENTORY][TAG: " & type & "]")
            End Select
        End Sub
        Public Sub OnNormalMove(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim oldslot As Byte = packet.Byte
            Dim newslot As Byte = packet.Byte
            Dim amout As UInt16 = packet.Word

            If PlayerData(index_).InExchange = True Then
                Exit Sub
            End If

            If Inventorys(index_).UserItems(oldslot).Pk2Id <> 0 Then
                If oldslot >= 0 Then
                    'Normal Move
                    Dim old_item As cInvItem = Inventorys(index_).UserItems(oldslot)
                    Dim new_item As cInvItem = Inventorys(index_).UserItems(newslot)
                    Dim replace_item As New cInvItem

                    old_item.Slot = newslot

                    'Update then new Item Slot
                    DataBase.SaveQuery(String.Format("UPDATE items SET itemtype='{0}', plusvalue='{1}', durability='{2}', quantity='{3}' WHERE owner='{4}' AND itemnumber='item{5}'", old_item.Pk2Id, old_item.Plus, old_item.Durability, old_item.Amount, old_item.OwnerCharID, newslot))
                    'Clean the Old Slot
                    DeleteItemFromDB(oldslot, index_)

                    If Inventorys(index_).UserItems(newslot).Pk2Id <> 0 Then
                        'Auf dem neuen Slot ist ein Item
                        new_item.Slot = oldslot
                        Inventorys(index_).UserItems(oldslot) = new_item
                        DataBase.SaveQuery(String.Format("UPDATE items SET itemtype='{0}', plusvalue='{1}', durability='{2}', quantity='{3}' WHERE owner='{4}' AND itemnumber='item{5}'", new_item.Pk2Id, new_item.Plus, new_item.Durability, new_item.Amount, new_item.OwnerCharID, oldslot))
                    Else
                        new_item.Slot = oldslot
                        Inventorys(index_).UserItems(oldslot) = new_item
                    End If

                    Inventorys(index_).UserItems(newslot) = old_item


                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.ItemMove)
					writer.Byte(1) 'success
                    writer.Byte(0) 'type
                    writer.Byte(oldslot)
                    writer.Byte(newslot)
                    writer.Word(amout)
                    writer.Byte(0) 'end
                    Server.Send(writer.GetBytes, index_)

                    If oldslot <= 12 Then
                        'Unequip
                        Server.SendToAllInRange(CreateUnEquippacket(index_, oldslot, newslot), index_)
                    ElseIf newslot <= 12 Then
                        'Equip
                        Server.SendToAllInRange(CreateEquippacket(index_, oldslot, newslot), index_)
                    End If
                End If
            End If

            Inventorys(index_).ReOrderItems(index_)
        End Sub

        Public Sub OnDropItem(ByVal packet As PacketReader, ByVal index_ As Integer) 'Only to delete Items for now
            Dim slot As Byte = packet.Byte
            Dim ref_item As cItem = GetItemByID(Inventorys(index_).UserItems(slot).Pk2Id)

            DeleteItemFromDB(slot, index_)

            Dim fake_item As cInvItem = Inventorys(index_).UserItems(slot)
            fake_item.Pk2Id = 0
            fake_item.Durability = 0
            fake_item.Plus = 0
            fake_item.Amount = 0

            Inventorys(index_).UserItems(slot) = fake_item

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.ItemMove)
            writer.Byte(1)
            writer.Byte(&HF)
            writer.Byte(slot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, index_)

            If ref_item IsNot Nothing Then
                SendNotice("Destroyed Item: " & ref_item.ITEM_TYPE_NAME)
            End If


            Inventorys(index_).ReOrderItems(index_)
        End Sub

        Public Sub OnExchangeAddItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim ExListInd As Integer = PlayerData(index_).ExchangeID

            If ExListInd = -1 Or Inventorys(index_).UserItems(slot).Pk2Id = 0 Then 'Security...
                Exit Sub
            End If

            If ExchangeData(ExListInd).Player1Index = index_ Then
                For i = 0 To 11 'Find free Exchange Slot
                    If ExchangeData(ExListInd).Items1(i) = -1 Then
                        ExchangeData(ExListInd).Items1(i) = slot

                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.ItemMove)
                        writer.Byte(1)
                        writer.Byte(4)
                        writer.Byte(slot)
                        writer.Byte(0)
                        Server.Send(writer.GetBytes, index_)

                        Exchange.OnExchangeUpdateItems(ExListInd)
                        Exit For
                    End If
                Next

            ElseIf ExchangeData(ExListInd).Player2Index = index_ Then
                For i = 0 To 11 'Find free Exchange Slot
                    If ExchangeData(ExListInd).Items2(i) = -1 Then
                        ExchangeData(ExListInd).Items2(i) = slot


                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.ItemMove)
                        writer.Byte(1)
                        writer.Byte(4)
                        writer.Byte(slot)
                        writer.Byte(0)
                        Server.Send(writer.GetBytes, index_)

                        Exchange.OnExchangeUpdateItems(ExListInd)
                        Exit For
                    End If
                Next
            End If




        End Sub

        Public Sub OnExchangeRemoveItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim ExListInd As Integer = PlayerData(index_).ExchangeID

            If ExListInd = -1 Then 'Security...
                Exit Sub
            End If

            If ExchangeData(ExListInd).Player1Index = index_ Then
                If ExchangeData(ExListInd).Items1(slot) <> -1 Then
                    ExchangeData(ExListInd).Items1(slot) = -1


                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.ItemMove)
                    writer.Byte(1)
                    writer.Byte(5)
                    writer.Byte(0)
                    Server.Send(writer.GetBytes, index_)
                Else
                    'Error-Item dosent live
                    Exit Sub
                End If

            ElseIf ExchangeData(ExListInd).Player2Index = index_ Then
                If ExchangeData(ExListInd).Items1(slot) <> -1 Then
                    ExchangeData(ExListInd).Items1(slot) = -1


                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.ItemMove)
                    writer.Byte(1)
                    writer.Byte(5)
                    writer.Byte(0)
                    Server.Send(writer.GetBytes, index_)
                Else
                    'Error-Item dosent live
                    Exit Sub
                End If
            End If

            Exchange.OnExchangeUpdateItems(ExListInd)


        End Sub

        Public Sub OnExchangeAddGold(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim add_gold As UInt32 = packet.DWord
            Dim writer As New PacketWriter

            If PlayerData(index_).InExchange = True Then
                If PlayerData(index_).Gold - add_gold >= 0 Then 'Prevent negative gold
                    If ExchangeData(PlayerData(index_).ExchangeID).Player1Index = index_ Then
                        ExchangeData(PlayerData(index_).ExchangeID).Player1Gold = add_gold
                        writer.Create(ServerOpcodes.ItemMove)
                        writer.Byte(1)
                        writer.Byte(&HD)
                        writer.QWord(add_gold)
                        Server.Send(writer.GetBytes, index_)

                        writer.Create(ServerOpcodes.Exchange_Gold)
                        writer.Byte(2)
                        writer.QWord(add_gold)
                        Server.Send(writer.GetBytes, PlayerData(index_).InExchangeWith)

                    ElseIf ExchangeData(PlayerData(index_).ExchangeID).Player2Index = index_ Then
                        ExchangeData(PlayerData(index_).ExchangeID).Player2Gold = add_gold

                        writer.Create(ServerOpcodes.ItemMove)
                        writer.Byte(1)
                        writer.Byte(&HD)
                        writer.QWord(add_gold)
                        Server.Send(writer.GetBytes, index_)

                        writer.Create(ServerOpcodes.Exchange_Gold)
                        writer.Byte(2)
                        writer.QWord(add_gold)
                        Server.Send(writer.GetBytes, PlayerData(index_).InExchangeWith)
                    End If
                End If
            End If
        End Sub
        Private Function CreateEquippacket(ByVal Index_ As Integer, ByVal Old_Slot As Byte, ByVal New_Slot As Byte) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.EquipItem)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Byte(New_Slot)
            writer.DWord(Inventorys(Index_).UserItems(New_Slot).Pk2Id)
            writer.Byte(Inventorys(Index_).UserItems(New_Slot).Plus)
            Return writer.GetBytes
        End Function

        Private Function CreateUnEquippacket(ByVal Index_ As Integer, ByVal Old_Slot As Byte, ByVal New_Slot As Byte) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.UnEquipItem)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Byte(Old_Slot)
            writer.DWord(Inventorys(Index_).UserItems(New_Slot).Pk2Id)
            Return writer.GetBytes
        End Function
        Public Sub DeleteItemFromDB(ByVal slot As Byte, ByVal Index_ As Integer)
            DataBase.SaveQuery(String.Format("UPDATE items SET itemtype='0', plusvalue='0', durability='0', quantity='0' WHERE owner='{0}' AND itemnumber='item{1}'", PlayerData(Index_).UniqueId, slot))
        End Sub

        Public Function GetFreeItemSlot(ByVal Index_ As Integer) As Byte
            For i = 13 To Inventorys(Index_).UserItems.Length - 1
                If Inventorys(Index_).UserItems(i).Pk2Id = 0 Then
                    Return i
                End If
            Next
        End Function
    End Module
End Namespace
