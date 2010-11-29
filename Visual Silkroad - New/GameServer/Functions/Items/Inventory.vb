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
                Case 24 'Buy From Item Mall
                    OnBuyItemFromMall(packet, index_)
                Case Else
                    Console.WriteLine("[INVENTORY][TAG: " & type & "]")
            End Select

            Inventorys(index_).ReOrderItems(index_)
        End Sub
        Public Sub OnNormalMove(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim oldslot As Byte = packet.Byte
            Dim newslot As Byte = packet.Byte
            Dim amout As UInt16 = packet.Word

            If PlayerData(index_).InExchange = True Then
                Exit Sub
            End If



            If Inventorys(index_).UserItems(oldslot).Pk2Id <> 0 Then
                Dim SourceItem As cInvItem = FillItem(Inventorys(index_).UserItems(oldslot))
                Dim DestItem As cInvItem = FillItem(Inventorys(index_).UserItems(newslot))
                Dim _SourceRef As cItem = GetItemByID(SourceItem.Pk2Id)



                If SourceItem.Slot <= 12 And DestItem.Slot >= 13 Then
                    'Uneuqip
                    If DestItem.Pk2Id = 0 Then
                        DestItem.Pk2Id = SourceItem.Pk2Id
                        DestItem.Durability = SourceItem.Durability
                        DestItem.Plus = SourceItem.Plus
                        DestItem.Amount = SourceItem.Amount
                        Inventorys(index_).UserItems(newslot) = DestItem

                        SourceItem.Pk2Id = 0
                        SourceItem.Amount = 0
                        SourceItem.Plus = 0
                        SourceItem.Durability = 30
                        Inventorys(index_).UserItems(oldslot) = SourceItem
                    ElseIf DestItem.Pk2Id <> 0 Then
                        Dim _DestRef As cItem = GetItemByID(DestItem.Pk2Id)
                        If _DestRef.CLASS_A = 1 Then 'Only Equipment
                            SourceItem.Slot = newslot
                            Inventorys(index_).UserItems(newslot) = SourceItem

                            DestItem.Slot = oldslot
                            Inventorys(index_).UserItems(oldslot) = DestItem
                        Else
                            Exit Sub
                        End If
                    End If

                ElseIf DestItem.Slot <= 12 And SourceItem.Slot >= 13 Then
                    'Equip a Item
                    If DestItem.Pk2Id = 0 Then
                        DestItem.Pk2Id = SourceItem.Pk2Id
                        DestItem.Durability = SourceItem.Durability
                        DestItem.Plus = SourceItem.Plus
                        DestItem.Amount = SourceItem.Amount
                        Inventorys(index_).UserItems(newslot) = DestItem

                    ElseIf DestItem.Pk2Id <> 0 Then
                        Dim _DestRef As cItem = GetItemByID(DestItem.Pk2Id)
                        If _DestRef.CLASS_A = 1 Then 'Only Equipment
                            SourceItem.Slot = newslot
                            Inventorys(index_).UserItems(newslot) = SourceItem

                            DestItem.Slot = oldslot
                            Inventorys(index_).UserItems(oldslot) = DestItem
                        End If
                    End If


                ElseIf DestItem.Slot >= 12 And SourceItem.Slot >= 13 Then
                    'Normal Move in Inventory
                    If DestItem.Pk2Id = 0 Then
                        If amout = SourceItem.Amount Or _SourceRef.CLASS_A = 1 Then
                            'Complete Move
                            Inventorys(index_).UserItems(newslot).Pk2Id = SourceItem.Pk2Id
                            Inventorys(index_).UserItems(newslot).Durability = SourceItem.Durability
                            Inventorys(index_).UserItems(newslot).Plus = SourceItem.Plus
                            Inventorys(index_).UserItems(newslot).Amount = SourceItem.Amount

                            Inventorys(index_).UserItems(oldslot).Pk2Id = 0
                            Inventorys(index_).UserItems(oldslot).Amount = 0
                            Inventorys(index_).UserItems(oldslot).Plus = 0
                            Inventorys(index_).UserItems(oldslot).Durability = 30
                        ElseIf amout < SourceItem.Amount Then
                            'Disturb
                            Inventorys(index_).UserItems(newslot).Pk2Id = SourceItem.Pk2Id
                            Inventorys(index_).UserItems(newslot).Durability = SourceItem.Durability
                            Inventorys(index_).UserItems(newslot).Plus = SourceItem.Plus
                            Inventorys(index_).UserItems(newslot).Amount = amout

                            Inventorys(index_).UserItems(oldslot).Amount -= amout 'Reduce it
                            Inventorys(index_).UserItems(oldslot) = SourceItem
                        End If
                    ElseIf DestItem.Pk2Id <> 0 Then

                        If _SourceRef.CLASS_A <> 1 Then
                            'ETC --> Stacking
                            If DestItem.Pk2Id = SourceItem.Pk2Id And DestItem.Amount + amout <= _SourceRef.MAX_STACK Then
                                DestItem.Amount += amout
                                Inventorys(index_).UserItems(newslot) = DestItem

                                If SourceItem.Amount - amout > 0 Then
                                    SourceItem.Amount -= amout 'Reduce it
                                    Inventorys(index_).UserItems(oldslot) = SourceItem
                                Else
                                    Inventorys(index_).UserItems(oldslot).Pk2Id = 0
                                    Inventorys(index_).UserItems(oldslot).Amount = 0
                                    Inventorys(index_).UserItems(oldslot).Plus = 0
                                    Inventorys(index_).UserItems(oldslot).Durability = 30
                                End If

                            ElseIf DestItem.Pk2Id = SourceItem.Pk2Id And DestItem.Amount + amout >= _SourceRef.MAX_STACK Then
                                'Only stack a part of the item
                                Dim tostack As UInteger = _SourceRef.MAX_STACK - DestItem.Amount
                                DestItem.Amount += tostack
                                Inventorys(index_).UserItems(newslot) = DestItem


                                If SourceItem.Amount - tostack > 0 Then
                                    SourceItem.Amount -= tostack  'Reduce it
                                    Inventorys(index_).UserItems(oldslot) = SourceItem
                                Else
                                    Inventorys(index_).UserItems(oldslot).Pk2Id = 0
                                    Inventorys(index_).UserItems(oldslot).Amount = 0
                                    Inventorys(index_).UserItems(oldslot).Plus = 0
                                    Inventorys(index_).UserItems(oldslot).Durability = 30
                                End If
                            Else
                                SourceItem.Slot = newslot
                                Inventorys(index_).UserItems(newslot) = SourceItem

                                DestItem.Slot = oldslot
                                Inventorys(index_).UserItems(oldslot) = DestItem
                            End If
                        Else
                            SourceItem.Slot = newslot
                            Inventorys(index_).UserItems(newslot) = SourceItem

                            DestItem.Slot = oldslot
                            Inventorys(index_).UserItems(oldslot) = DestItem
                        End If

                    End If
                End If
                UpdateItem(Inventorys(index_).UserItems(newslot))
                UpdateItem(Inventorys(index_).UserItems(oldslot))
            End If


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
                Server.SendToAllInRange(CreateUnEquippacket(index_, oldslot, newslot), PlayerData(index_).Position)
                PlayerData(index_).SetCharGroundStats()
                PlayerData(index_).AddItemsToStats(index_)
                OnStatsPacket(index_)
            ElseIf newslot <= 12 Then
                'Equip
                Server.SendToAllInRange(CreateEquippacket(index_, oldslot, newslot), PlayerData(index_).Position)
                PlayerData(index_).SetCharGroundStats()
                PlayerData(index_).AddItemsToStats(index_)
                OnStatsPacket(index_)
            End If
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

            DropItem(Inventorys(index_).UserItems(slot), PlayerData(index_).Position)
            Inventorys(index_).UserItems(slot) = fake_item



            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.ItemMove)
            writer.Byte(1)
            writer.Byte(&HF)
            writer.Byte(slot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, index_)
        End Sub

#Region "Exchange"
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
#End Region

        Private Function CheckItemGender(ByVal tmpItem As cItem, ByVal Index_ As Integer) As Boolean
            Dim Gender As Integer = 0

            If (PlayerData(Index_).Model >= 1907 And PlayerData(Index_).Model <= 1919) Or (PlayerData(Index_).Model >= 14717 And PlayerData(Index_).Model <= 14729) Then
                Gender = 1
            End If
            If (PlayerData(Index_).Model >= 1920 And PlayerData(Index_).Model <= 1932) Or (PlayerData(Index_).Model >= 14730 And PlayerData(Index_).Model <= 14742) Then
                Gender = 0
            End If

            If Gender = tmpItem.GENDER Or tmpItem.GENDER = 2 Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function CheckLevel(ByVal tmpItem As cInvItem, ByVal Index_ As Integer)
            If GetItemByID(tmpItem.Pk2Id).LV_REQ >= PlayerData(Index_).Level Then
                Return True
            Else
                Return False
            End If
        End Function

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
            DataBase.SaveQuery(String.Format("UPDATE items SET itemtype='0', plusvalue='0', durability='0', quantity='0' WHERE owner='{0}' AND itemnumber='item{1}'", PlayerData(Index_).CharacterId, slot))
        End Sub

        Public Function GetFreeItemSlot(ByVal Index_ As Integer) As Byte
            For i = 13 To Inventorys(Index_).UserItems.Length - 1
                If Inventorys(Index_).UserItems(i).Pk2Id = 0 Then
                    Return i
                End If
            Next
        End Function


        Private Function FillItem(ByVal From_item As cInvItem) As cInvItem
            Dim tmp_ As New cInvItem
            tmp_.DatabaseID = From_item.DatabaseID
            tmp_.Pk2Id = From_item.Pk2Id
            tmp_.OwnerCharID = From_item.OwnerCharID
            tmp_.Plus = From_item.Plus
            tmp_.Slot = From_item.Slot
            tmp_.Amount = From_item.Amount
            tmp_.Durability = From_item.Durability

            Return tmp_
        End Function
    End Module
End Namespace
