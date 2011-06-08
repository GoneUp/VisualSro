Namespace GameServer.Functions
    Module Inventory
        Public Sub OnInventory(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim type As Byte = packet.Byte

            Select Case type
                Case 0 'Normal Movement
                    OnNormalMove(packet, index_)
                Case 1 'Storage Move
                Case 2 'Inventory --> Storage
                Case 3 'Storage --> Inventory
                Case 4 'Inventory --> Exchange
                    OnExchangeAddItem(packet, index_)
                Case 5 'Exchange --> Inventory
                    OnExchangeRemoveItem(packet, index_)
                Case 7 'drop
                    OnDropItem(packet, index_)
                Case 8 'BuyItem
                    OnBuyItem(packet, index_)
                Case 9 'SellItem
                    OnSellItem(packet, index_)
                Case 10 'Drop Gold
                    OnDropGold(packet, index_)
                Case 11 'Gold to Storage
                Case 12 'Storage_Gold to Inventory
                Case 13  'Exchange Gold
                    OnExchangeAddGold(packet, index_)
                Case 24 'Buy From Item Mall
                    OnBuyItemFromMall(packet, index_)
                Case 36 'Equip Avatar 
                    Dim Old_Slot As Byte = packet.Byte
                    Dim New_Slot As Byte = packet.Byte
                    SendNotice("Old: " & Old_Slot & " New: " & New_Slot)
                Case Else

                    Console.WriteLine("[INVENTORY][TAG: " & type & "]")
            End Select

        End Sub
        Public Sub OnNormalMove(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim Old_Slot As Byte = packet.Byte
            Dim New_Slot As Byte = packet.Byte
            Dim amout As UInt16 = packet.Word

            If PlayerData(Index_).InExchange Or PlayerData(Index_).InStall Or PlayerData(Index_).Alive = False Then
                Exit Sub
            End If



            If Inventorys(Index_).UserItems(Old_Slot).Pk2Id <> 0 Then
                Dim SourceItem As cInvItem = FillItem(Inventorys(Index_).UserItems(Old_Slot))
                Dim DestItem As cInvItem = FillItem(Inventorys(Index_).UserItems(New_Slot))
                Dim _SourceRef As cItem = GetItemByID(SourceItem.Pk2Id)


                If SourceItem.Slot <= 12 And DestItem.Slot >= 13 Then
                    'Uneuqip
                    If DestItem.Pk2Id = 0 Then
                        Inventorys(Index_).UserItems(New_Slot) = SourceItem
                        Inventorys(Index_).UserItems(New_Slot).Slot = New_Slot

                        Inventorys(Index_).UserItems(Old_Slot) = ClearItem(DestItem)
                        Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot
                    ElseIf DestItem.Pk2Id <> 0 Then
                        Dim _DestRef As cItem = GetItemByID(DestItem.Pk2Id)
                        If _DestRef.CLASS_A = 1 And CheckItemGender(_DestRef, Index_) And CheckLevel(_DestRef, Index_) Then 'Only Equipment
                            Inventorys(Index_).UserItems(New_Slot) = SourceItem
                            Inventorys(Index_).UserItems(New_Slot).Slot = New_Slot

                            Inventorys(Index_).UserItems(Old_Slot) = DestItem
                            Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot

                        ElseIf CheckItemGender(_DestRef, Index_) = False Then
                            OnItemMoveError(Index_, &H16, &H18)
                            Exit Sub
                        ElseIf CheckLevel(_DestRef, Index_) = False Then
                            OnItemMoveError(Index_, &H6C, &H18)
                            Exit Sub
                        End If
                    End If

                ElseIf DestItem.Slot <= 12 And SourceItem.Slot >= 13 Then
                    'Equip a Item
                    If DestItem.Pk2Id = 0 Then
                        If _SourceRef.CLASS_A = 1 And CheckItemGender(_SourceRef, Index_) And CheckLevel(_SourceRef, Index_) Then
                            Inventorys(Index_).UserItems(New_Slot) = SourceItem
                            Inventorys(Index_).UserItems(New_Slot).Slot = New_Slot

                            Inventorys(Index_).UserItems(Old_Slot) = ClearItem(DestItem)
                            Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot
                        ElseIf CheckItemGender(_SourceRef, Index_) = False Then
                            OnItemMoveError(Index_, &H16, &H18)
                            Exit Sub
                        ElseIf CheckLevel(_SourceRef, Index_) = False Then
                            OnItemMoveError(Index_, &H6C, &H18)
                            Exit Sub
                        End If
                    ElseIf DestItem.Pk2Id <> 0 Then
                        Dim _DestRef As cItem = GetItemByID(DestItem.Pk2Id)
                        If _SourceRef.CLASS_A = 1 And CheckItemGender(_SourceRef, Index_) And CheckLevel(_SourceRef, Index_) Then
                            Inventorys(Index_).UserItems(New_Slot) = SourceItem
                            Inventorys(Index_).UserItems(New_Slot).Slot = New_Slot

                            Inventorys(Index_).UserItems(Old_Slot) = DestItem
                            Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot

                        ElseIf CheckItemGender(_SourceRef, Index_) = False Then
                            OnItemMoveError(Index_, &H16, &H18)
                            Exit Sub
                        ElseIf CheckLevel(_SourceRef, Index_) = False Then
                            OnItemMoveError(Index_, &H6C, &H18)
                            Exit Sub
                        End If
                    End If


                ElseIf DestItem.Slot >= 12 And SourceItem.Slot >= 13 Then
                    'Normal Move in Inventory
                    If DestItem.Pk2Id = 0 Then
                        If amout = SourceItem.Amount Or _SourceRef.CLASS_A = 1 Or _SourceRef.CLASS_A = 2 Then
                            'Complete Move
                            Inventorys(Index_).UserItems(New_Slot) = SourceItem
                            Inventorys(Index_).UserItems(New_Slot).Slot = New_Slot

                            Inventorys(Index_).UserItems(Old_Slot) = ClearItem(DestItem)
                            Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot
                        ElseIf amout < SourceItem.Amount Then
                            'Disturb
                            Inventorys(Index_).UserItems(New_Slot).Pk2Id = SourceItem.Pk2Id
                            Inventorys(Index_).UserItems(New_Slot).Durability = SourceItem.Durability
                            Inventorys(Index_).UserItems(New_Slot).Plus = SourceItem.Plus
                            Inventorys(Index_).UserItems(New_Slot).Amount = amout

                            Inventorys(Index_).UserItems(Old_Slot).Amount -= amout 'Reduce it
                        End If
                    ElseIf DestItem.Pk2Id <> 0 Then
                        If _SourceRef.CLASS_A = 3 Then
                            'ETC --> Stacking
                            If DestItem.Pk2Id = SourceItem.Pk2Id And DestItem.Amount + amout <= _SourceRef.MAX_STACK Then
                                DestItem.Amount += amout
                                Inventorys(Index_).UserItems(New_Slot) = DestItem

                                If SourceItem.Amount - amout > 0 Then
                                    SourceItem.Amount -= amout 'Reduce it
                                    Inventorys(Index_).UserItems(Old_Slot) = SourceItem
                                    Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot
                                Else
                                    'Remove it
                                    Inventorys(Index_).UserItems(Old_Slot) = ClearItem(SourceItem)
                                    Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot
                                End If

                            ElseIf DestItem.Pk2Id = SourceItem.Pk2Id And DestItem.Amount + amout >= _SourceRef.MAX_STACK Then
                                'Only stack a part of the item
                                Dim tostack As UInteger = _SourceRef.MAX_STACK - DestItem.Amount
                                DestItem.Amount += tostack
                                Inventorys(Index_).UserItems(New_Slot) = DestItem


                                If SourceItem.Amount - tostack > 0 Then
                                    SourceItem.Amount -= tostack  'Reduce it
                                    Inventorys(Index_).UserItems(Old_Slot) = SourceItem
                                    Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot
                                Else
                                    'Remove it
                                    Inventorys(Index_).UserItems(Old_Slot) = ClearItem(SourceItem)
                                    Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot
                                End If
                            Else
                                Inventorys(Index_).UserItems(New_Slot) = SourceItem
                                Inventorys(Index_).UserItems(New_Slot).Slot = New_Slot

                                Inventorys(Index_).UserItems(Old_Slot) = DestItem
                                Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot
                            End If
                        Else
                            'Eqquip Move
                            Inventorys(Index_).UserItems(New_Slot) = SourceItem
                            Inventorys(Index_).UserItems(New_Slot).Slot = New_Slot

                            Inventorys(Index_).UserItems(Old_Slot) = DestItem
                            Inventorys(Index_).UserItems(Old_Slot).Slot = Old_Slot
                        End If
                    End If
                End If

                UpdateItem(Inventorys(Index_).UserItems(Old_Slot))
                UpdateItem(Inventorys(Index_).UserItems(New_Slot))

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.ItemMove)
                writer.Byte(1) 'success
                writer.Byte(0) 'type
                writer.Byte(Old_Slot)
                writer.Byte(New_Slot)
                writer.Word(amout)
                writer.Byte(0) 'end
                Server.Send(writer.GetBytes, Index_)

                If Old_Slot <= 12 Then
                    'Unequip
                    Server.SendIfPlayerIsSpawned(CreateUnEquippacket(Index_, Old_Slot, New_Slot), Index_)
                    PlayerData(Index_).SetCharGroundStats()
                    PlayerData(Index_).AddItemsToStats(Index_)
                    OnStatsPacket(Index_)
                ElseIf New_Slot <= 12 Then
                    'Equip
                    Server.SendIfPlayerIsSpawned(CreateEquippacket(Index_, Old_Slot, New_Slot), Index_)
                    PlayerData(Index_).SetCharGroundStats()
                    PlayerData(Index_).AddItemsToStats(Index_)
                    OnStatsPacket(Index_)
                End If

            Else
                'item dosent existis
                OnItemMoveError(Index_, &H6C, &H18)
            End If


        End Sub

#Region "Drop"
        Public Sub OnDropItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            If PlayerData(index_).InExchange Or PlayerData(index_).InStall Then
                Exit Sub
            End If


            Dim slot As Byte = packet.Byte
            Dim ref_item As cItem = GetItemByID(Inventorys(index_).UserItems(slot).Pk2Id)
            Dim item_uniqueid As UInteger = DropItem(Inventorys(index_).UserItems(slot), PlayerData(index_).Position)

            DeleteItemFromDB(slot, index_)
            Inventorys(index_).UserItems(slot) = ClearItem(Inventorys(index_).UserItems(slot))


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.ItemDelete)
            writer.DWord(item_uniqueid)
            Server.Send(writer.GetBytes, index_)

            writer.Create(ServerOpcodes.ItemMove)
            writer.Byte(1)
            writer.Byte(&HF)
            writer.Byte(slot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, index_)
        End Sub

        Public Sub OnDropGold(ByVal packet As PacketReader, ByVal index_ As Integer)
            If PlayerData(index_).InExchange Or PlayerData(index_).InStall Then
                Exit Sub
            End If


            Dim amout As UInt64 = packet.QWord

            If CLng(PlayerData(index_).Gold) - amout >= 0 Then
                Dim item As New cInvItem
                item.Amount = amout
                item.Pk2Id = 1
                item.OwnerCharID = PlayerData(index_).UniqueId

                DropItem(item, PlayerData(index_).Position)

                PlayerData(index_).Gold -= amout
                UpdateGold(index_)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.ItemMove)
                writer.Byte(1)
                writer.Byte(10)
                writer.QWord(amout)
                Server.Send(writer.GetBytes, index_)
            End If
        End Sub
#End Region
#Region "Exchange"
        Public Sub OnExchangeAddItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim ExListInd As Integer = PlayerData(index_).ExchangeID

            If ExListInd = -1 Or Inventorys(index_).UserItems(slot).Pk2Id = 0 Or PlayerData(index_).InExchange = False Or Inventorys(index_).UserItems(slot).Locked = True Then 'Security...
                Exit Sub
            End If

            If ExchangeData(ExListInd).Player1Index = index_ Then
                For i = 0 To 11 'Find free Exchange Slot
                    If ExchangeData(ExListInd).Items1(i) = -1 Then
                        ExchangeData(ExListInd).Items1(i) = slot
                        Inventorys(index_).UserItems(slot).Locked = True

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
                        Inventorys(index_).UserItems(slot).Locked = True

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
                    Inventorys(index_).UserItems(slot).Locked = False

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
                    Inventorys(index_).UserItems(slot).Locked = False

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
#Region "Helper Functions"
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

        Private Function CheckLevel(ByVal tmpItem As cItem, ByVal Index_ As Integer)
            If tmpItem.LV_REQ > PlayerData(Index_).Level Then
                Return False
            Else
                Return True
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

        Public Sub AddItemToDB(ByVal item As cInvItem)
            Array.Resize(GameDB.AllItems, GameDB.AllItems.Count + 1)
            GameDB.AllItems(GameDB.AllItems.Count - 1) = item

            DataBase.SaveQuery(String.Format("INSERT INTO items(itemtype, owner, plusvalue, slot, quantity, durability, itemnumber) VALUE ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", item.Pk2Id, item.OwnerCharID, item.Plus, item.Slot, item.Amount, item.Durability, GetItemTypeDbString(item)))
        End Sub

        Public Sub UpdateItem(ByVal item As cInvItem)
            For i = 0 To GameDB.AllItems.Count - 1
                If GameDB.AllItems(i) IsNot Nothing Then
                    If GameDB.AllItems(i).OwnerCharID = item.OwnerCharID And GameDB.AllItems(i).Slot = item.Slot And GameDB.AllItems(i).ItemType = item.ItemType Then
                        GameDB.AllItems(i) = item
                        DataBase.SaveQuery(String.Format("UPDATE items SET itemtype='{0}', plusvalue='{1}', durability='{2}', quantity='{3}' WHERE owner='{4}' AND itemnumber='{5}'", item.Pk2Id, item.Plus, item.Durability, item.Amount, item.OwnerCharID, GetItemTypeDbString(item)))
                        Exit For
                    End If
                End If
            Next
        End Sub

        Public Sub DeleteItemFromDB(ByVal slot As Byte, ByVal Index_ As Integer)
            DataBase.SaveQuery(String.Format("UPDATE items SET itemtype='0', plusvalue='0', durability='0', quantity='0' WHERE owner='{0}' AND itemnumber='item{1}'", PlayerData(Index_).CharacterId, slot))
        End Sub

        Private Function GetItemTypeDbString(ByVal item As cInvItem) As String
            Select Case item.ItemType
                Case cInvItem.sUserItemType.Inventory
                    Return "item" & item.Slot
                Case cInvItem.sUserItemType.Avatar
                    Return "avatar" & item.Slot
                Case cInvItem.sUserItemType.Storage_User
                    Return "storage" & item.Slot
            End Select
            Return "WTF"
        End Function

        Public Function GetFreeItemSlot(ByVal Index_ As Integer) As Integer
            For i = 13 To Inventorys(Index_).UserItems.Length - 1
                If Inventorys(Index_).UserItems(i).Pk2Id = 0 Then
                    Return i
                End If
            Next
            Return -1
        End Function

        Function FillItem(ByVal From_item As cInvItem) As cInvItem
            Dim tmp_ As New cInvItem
            tmp_.DatabaseID = From_item.DatabaseID
            tmp_.Pk2Id = From_item.Pk2Id
            tmp_.OwnerCharID = From_item.OwnerCharID
            tmp_.Plus = From_item.Plus
            tmp_.Slot = From_item.Slot
            tmp_.Amount = From_item.Amount
            tmp_.Durability = From_item.Durability
            tmp_.Blues = From_item.Blues

            'WhiteStats
            tmp_.PerDurability = From_item.PerDurability
            tmp_.PerPhyRef = From_item.PerPhyRef
            tmp_.PerMagRef = From_item.PerMagRef
            tmp_.PerPhyAtk = From_item.PerPhyAtk
            tmp_.PerMagAtk = From_item.PerMagAtk
            tmp_.PerPhyDef = From_item.PerPhyDef
            tmp_.PerMagDef = From_item.PerMagDef
            tmp_.PerBlock = From_item.PerBlock
            tmp_.PerCritical = From_item.PerCritical
            tmp_.PerAttackRate = From_item.PerAttackRate
            tmp_.PerParryRate = From_item.PerParryRate
            tmp_.PerPhyAbs = From_item.PerPhyAbs
            tmp_.PerMagAbs = From_item.PerMagAbs

            Return tmp_
        End Function

        Public Function ClearItem(ByVal OldItem As cInvItem) As cInvItem
            OldItem.Pk2Id = 0
            OldItem.Amount = 0
            OldItem.Plus = 0
            OldItem.Durability = 30
            OldItem.PerDurability = 0
            OldItem.PerPhyRef = 0
            OldItem.PerMagRef = 0
            OldItem.PerPhyAtk = 0
            OldItem.PerMagAtk = 0
            OldItem.PerPhyDef = 0
            OldItem.PerMagDef = 0
            OldItem.PerBlock = 0
            OldItem.PerCritical = 0
            OldItem.PerAttackRate = 0
            OldItem.PerParryRate = 0
            OldItem.PerPhyAbs = 0
            OldItem.PerMagAbs = 0

            OldItem.Blues = Nothing

            Return OldItem
        End Function

        Public Sub AddItemDataToPacket(ByVal _item As cInvItem, ByVal writer As PacketWriter)
            Dim refitem As cItem = GetItemByID(_item.Pk2Id)
            writer.DWord(_item.Pk2Id)
            Debug.Print(refitem.ITEM_TYPE_NAME & " Type1: " & refitem.CLASS_A & " Type2: " & refitem.CLASS_B & " Type3: " & refitem.CLASS_C)
            Select Case refitem.CLASS_A
                Case 1 'Equipment
                    writer.Byte(_item.Plus)
                    writer.QWord(_item.GetWhiteStats)
                    writer.DWord(_item.Durability)

                    writer.Byte(_item.Blues.Count)
                    For i = 0 To _item.Blues.Count - 1
                        writer.DWord(_item.Blues(i).Typ)
                        writer.DWord(_item.Blues(i).Amount)
                    Next

                    writer.Word(1)
                    writer.Word(2)
                Case 2 'Pets
                    If refitem.CLASS_B = 1 Then
                        Dim name As String = "Test"
                        Select Case refitem.CLASS_C
                            Case 1
                                'Attack
                                writer.Byte(1)
                                'writer.DWord(0)
                                'writer.Byte(0)
                                'writer.Word(name.Length)
                                'writer.String(name)

                            Case 2
                                'Pick
                                writer.Byte(1)
                                'writer.DWord(0)
                                'writer.Byte(0)
                                'writer.Word(name.Length)
                                'writer.String(name)
                                'writer.DWord(0)
                        End Select
                    End If

                Case 3 'etc
                    Select Case refitem.CLASS_B
                        Case 11
                            Select Case refitem.CLASS_C
                                Case 7
                                    writer.Byte(1)
                                    writer.Word(0)
                            End Select
                        Case Else
                            writer.Word(_item.Amount)
                    End Select

            End Select
        End Sub

        Public Sub OnItemMoveError(ByVal Index_ As Integer, ByVal Byte1 As Byte, ByVal Byte2 As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.ItemMove)
            writer.Byte(2)
            writer.Byte(Byte1)
            writer.Byte(Byte2)
            Server.Send(writer.GetBytes, Index_)
        End Sub
#End Region

    End Module
End Namespace
