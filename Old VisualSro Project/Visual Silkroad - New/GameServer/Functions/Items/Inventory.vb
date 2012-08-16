Imports SRFramework

Namespace Functions
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
                Case 13 'Exchange Gold
                    OnExchangeAddGold(packet, index_)
                Case 24 'Buy From Item Mall
                    OnBuyItemFromMall(packet, index_)
                Case 35 'Unqequip Avatar
                    OnAvatarUnEquip(packet, index_)
                Case 36 'Equip Avatar 
                    OnAvatarEquip(packet, index_)
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


            If Inventorys(Index_).UserItems(Old_Slot).ItemID <> 0 Then
                Dim SourceInvItem As cInventoryItem = Inventorys(Index_).UserItems(Old_Slot)
                Dim SourceItem As cItem = GameDB.Items(SourceInvItem.ItemID)
                Dim _SourceRef As cRefItem = GetItemByID(SourceItem.ObjectID)

                Dim DestInvItem As cInventoryItem = Inventorys(Index_).UserItems(New_Slot)



                If SourceInvItem.Slot <= 12 And DestInvItem.Slot >= 13 Then
                    'Uneuqip
                    If DestInvItem.ItemID = 0 Then
                        'Empty Slot
                        Inventorys(Index_).UserItems(New_Slot).ItemID = SourceInvItem.ItemID
                        Inventorys(Index_).UserItems(Old_Slot).ItemID = 0

                    ElseIf DestInvItem.ItemID <> 0 Then
                        'Slot is not empty, switch 
                        Dim DestItem As cItem = GameDB.Items(DestInvItem.ItemID)
                        Dim _DestRef As cRefItem = GetItemByID(DestItem.ObjectID)

                        If _DestRef.CLASS_A = 1 And CheckItemGender(_DestRef, Index_) And CheckLevel(_DestRef, Index_) Then 'Only Equipment

                            Inventorys(Index_).UserItems(New_Slot).ItemID = SourceInvItem.ItemID
                            Inventorys(Index_).UserItems(Old_Slot).ItemID = 0

                        ElseIf CheckItemGender(_DestRef, Index_) = False Then
                            OnItemMoveError(Index_, &H16, &H18)
                            Exit Sub
                        ElseIf CheckLevel(_DestRef, Index_) = False Then
                            OnItemMoveError(Index_, &H6C, &H18)
                            Exit Sub
                        End If
                    End If

                ElseIf DestInvItem.Slot <= 12 And SourceInvItem.Slot >= 13 Then
                    'Equip a Item
                    If DestInvItem.ItemID = 0 Then
                        'Empty Slot
                        If _SourceRef.CLASS_A = 1 And CheckItemGender(_SourceRef, Index_) And CheckLevel(_SourceRef, Index_) Then
                            Inventorys(Index_).UserItems(New_Slot).ItemID = SourceInvItem.ItemID
                            Inventorys(Index_).UserItems(Old_Slot).ItemID = 0

                        ElseIf CheckItemGender(_SourceRef, Index_) = False Then
                            OnItemMoveError(Index_, &H16, &H18)
                            Exit Sub
                        ElseIf CheckLevel(_SourceRef, Index_) = False Then
                            OnItemMoveError(Index_, &H6C, &H18)
                            Exit Sub
                        End If

                    ElseIf DestInvItem.ItemID <> 0 Then
                        'Slot not empty, switch
                        Dim DestItem As cItem = GameDB.Items(DestInvItem.ItemID)
                        Dim _DestRef As cRefItem = GetItemByID(DestItem.ObjectID)

                        If _SourceRef.CLASS_A = 1 And CheckItemGender(_SourceRef, Index_) And CheckLevel(_SourceRef, Index_) Then
                            Inventorys(Index_).UserItems(New_Slot).ItemID = SourceInvItem.ItemID
                            Inventorys(Index_).UserItems(Old_Slot).ItemID = 0

                        ElseIf CheckItemGender(_SourceRef, Index_) = False Then
                            OnItemMoveError(Index_, &H16, &H18)
                            Exit Sub
                        ElseIf CheckLevel(_SourceRef, Index_) = False Then
                            OnItemMoveError(Index_, &H6C, &H18)
                            Exit Sub
                        End If
                    End If


                ElseIf DestInvItem.Slot >= 13 And SourceInvItem.Slot >= 13 Then
                    'Normal Move in Inventory
                    If DestInvItem.ItemID = 0 Then
                        'Empty
                        If amout = SourceItem.Data Or _SourceRef.CLASS_A = 1 Or _SourceRef.CLASS_A = 2 Then
                            'Complete Move
                            Inventorys(Index_).UserItems(New_Slot).ItemID = SourceInvItem.ItemID
                            Inventorys(Index_).UserItems(Old_Slot).ItemID = 0

                        ElseIf amout < SourceItem.Data Then
                            'Disturb
                            Dim newItem As New cItem
                            newItem.ObjectID = SourceItem.ObjectID
                            newItem.CreatorName = PlayerData(Index_).CharacterName & "#MOVD"
                            newItem.Data = amout
                            Inventorys(Index_).UserItems(New_Slot).ItemID = ItemManager.AddItem(newItem)


                            SourceItem.Data -= amout 'Reduce it
                            ItemManager.UpdateItem(SourceItem)
                        End If

                    ElseIf DestInvItem.ItemID <> 0 Then
                        Dim DestItem As cItem = GameDB.Items(DestInvItem.ItemID)

                        If _SourceRef.CLASS_A = 3 Then
                            'ETC --> Stacking
                            If DestItem.ObjectID = SourceItem.ObjectID And DestItem.Data + amout <= _SourceRef.MAX_STACK Then
                                DestItem.Data += amout
                                ItemManager.UpdateItem(DestItem)

                                UpdateAmout(Index_, SourceInvItem.Slot, amout * -1)

                            ElseIf DestItem.ObjectID = SourceItem.ObjectID And DestItem.Data + amout > _SourceRef.MAX_STACK Then
                                'Only stack a part of the item
                                Dim tostack As UInteger = _SourceRef.MAX_STACK - DestItem.Data
                                DestItem.Data += tostack
                                ItemManager.UpdateItem(DestItem)

                                UpdateAmout(Index_, SourceInvItem.Slot, tostack * -1)
                            End If
                        Else
                            Inventorys(Index_).UserItems(New_Slot).ItemID = SourceInvItem.ItemID
                            Inventorys(Index_).UserItems(Old_Slot).ItemID = DestInvItem.ItemID
                        End If
                    End If
                Else
                    'Equip Move
                    Inventorys(Index_).UserItems(New_Slot).ItemID = SourceInvItem.ItemID
                    Inventorys(Index_).UserItems(Old_Slot).ItemID = DestInvItem.ItemID

                End If


                ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(Old_Slot), cInventoryItem.Type.Inventory)
                ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(New_Slot), cInventoryItem.Type.Inventory)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                writer.Byte(1) 'success
                writer.Byte(0) 'type
                writer.Byte(Old_Slot)
                writer.Byte(New_Slot)
                writer.Word(amout)
                writer.Byte(0)  'end
                Server.Send(writer.GetBytes, Index_)

                If Old_Slot <= 12 Then
                    'Unequip
                    Server.SendIfPlayerIsSpawned(CreateUnEquippacket(Index_, Old_Slot, New_Slot, False), Index_)
                    PlayerData(Index_).SetCharGroundStats()
                    PlayerData(Index_).AddItemsToStats(Index_)
                    OnStatsPacket(Index_)
                ElseIf New_Slot <= 12 Then
                    'Equip
                    Server.SendIfPlayerIsSpawned(CreateEquippacket(Index_, Old_Slot, New_Slot, False), Index_)
                    PlayerData(Index_).SetCharGroundStats()
                    PlayerData(Index_).AddItemsToStats(Index_)
                    OnStatsPacket(Index_)
                End If

            Else
                'item dosent existis
                OnItemMoveError(Index_, &H6C, &H18)
            End If
        End Sub

#Region "Drop/PickUp"

        Public Sub OnDropItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            If PlayerData(index_).InExchange Or PlayerData(index_).InStall Then
                Exit Sub
            End If


            Dim slot As Byte = packet.Byte
            Dim invItem As cInventoryItem = Inventorys(index_).UserItems(slot)
            Dim item As cItem = GameDB.Items(invItem.ItemID)
            Dim ref_item As cRefItem = GetItemByID(item.ObjectID)
            Dim item_uniqueid As UInteger = DropItem(invItem, item, PlayerData(index_).Position)

            invItem.ItemID = 0
            ItemManager.UpdateInvItem(invItem, cInventoryItem.Type.Inventory)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_DELETE)
            writer.DWord(item_uniqueid)
            Server.Send(writer.GetBytes, index_)

            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
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
                Dim invItem As New cInventoryItem
                invItem.OwnerID = PlayerData(index_).UniqueID
                Dim item As New cItem
                item.Data = amout
                item.ObjectID = 1


                DropItem(invItem, item, PlayerData(index_).Position)

                PlayerData(index_).Gold -= amout
                UpdateGold(index_)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                writer.Byte(1)
                writer.Byte(10)
                writer.QWord(amout)
                Server.Send(writer.GetBytes, index_)
            End If
        End Sub

        Public Sub OnPickUp(ByVal UniqueId As UInteger, ByVal Index_ As Integer)
            Dim _item As cItemDrop = ItemList(UniqueId)
            Dim distance As Double = CalculateDistance(PlayerData(Index_).Position, _item.Position)

            If distance >= 5 Then
                'Out Of Range
                Dim TravelTime As Single = MoveUserToObject(Index_, _item.Position, 5)
                PickUpTimer(Index_).Interval = TravelTime
                PickUpTimer(Index_).Start()

            Else
                UpdateState(1, 1, Index_)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_PICKUP_MOVE)
                writer.DWord(PlayerData(Index_).UniqueID)
                writer.Byte(_item.Position.XSector)
                writer.Byte(_item.Position.YSector)
                writer.Float(_item.Position.X)
                writer.Float(_item.Position.Z)
                writer.Float(_item.Position.Y)
                writer.Word(0)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.GAME_PICKUP_ITEM)
                writer.DWord(PlayerData(Index_).UniqueID)
                writer.Byte(0)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

                RemoveItem(UniqueId)

                If _item.Item.ObjectID = 1 Or _item.Item.ObjectID = 2 Or _item.Item.ObjectID = 3 Then
                    If _item.Item.Data > 0 Then
                        PlayerData(Index_).Gold += _item.Item.Data
                        UpdateGold(Index_)
                    End If
                Else
                    Dim slot As Byte = GetFreeItemSlot(Index_)
                    If slot <> -1 Then
                        Dim ref As cRefItem = GetItemByID(_item.Item.ObjectID)
                        Inventorys(Index_).UserItems(slot).ItemID = _item.Item.ID
                        ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(slot), cInventoryItem.Type.Inventory)

                        writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                        writer.Byte(1)
                        writer.Byte(6) 'type = new item
                        writer.Byte(Inventorys(Index_).UserItems(slot).Slot)

                        AddItemDataToPacket(_item.Item, writer)

                        Server.Send(writer.GetBytes, Index_)
                    End If
                End If
            End If
        End Sub

#End Region

#Region "Exchange"

        Public Sub OnExchangeAddItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim invItem As cInventoryItem = Inventorys(index_).UserItems(slot)
            Dim ExListInd As Integer = PlayerData(index_).ExchangeID

            If ExListInd = -1 Or invItem.ItemID = 0 Or PlayerData(index_).InExchange = False Or invItem.Locked = True Then 'Security...
                Exit Sub
            End If

            If ExchangeData(ExListInd).Player1Index = index_ Then
                For i = 0 To 11 'Find free Exchange Slot
                    If ExchangeData(ExListInd).Items1(i) = -1 Then
                        ExchangeData(ExListInd).Items1(i) = slot
                        Inventorys(index_).UserItems(slot).Locked = True

                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                        writer.Byte(1)
                        writer.Byte(4)
                        writer.Byte(slot)
                        writer.Byte(0)
                        Server.Send(writer.GetBytes, index_)

                        OnExchangeUpdateItems(ExListInd)
                        Exit For
                    End If
                Next

            ElseIf ExchangeData(ExListInd).Player2Index = index_ Then
                For i = 0 To 11 'Find free Exchange Slot
                    If ExchangeData(ExListInd).Items2(i) = -1 Then
                        ExchangeData(ExListInd).Items2(i) = slot
                        Inventorys(index_).UserItems(slot).Locked = True

                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                        writer.Byte(1)
                        writer.Byte(4)
                        writer.Byte(slot)
                        writer.Byte(0)
                        Server.Send(writer.GetBytes, index_)

                        OnExchangeUpdateItems(ExListInd)
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
                    writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
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
                    writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                    writer.Byte(1)
                    writer.Byte(5)
                    writer.Byte(0)
                    Server.Send(writer.GetBytes, index_)
                Else
                    'Error-Item dosent live
                    Exit Sub
                End If
            End If

            OnExchangeUpdateItems(ExListInd)
        End Sub

        Public Sub OnExchangeAddGold(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim add_gold As UInt32 = packet.DWord
            Dim writer As New PacketWriter

            If PlayerData(index_).InExchange = True Then
                If PlayerData(index_).Gold - add_gold >= 0 Then 'Prevent negative gold
                    If ExchangeData(PlayerData(index_).ExchangeID).Player1Index = index_ Then
                        ExchangeData(PlayerData(index_).ExchangeID).Player1Gold = add_gold
                        writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                        writer.Byte(1)
                        writer.Byte(&HD)
                        writer.QWord(add_gold)
                        Server.Send(writer.GetBytes, index_)

                        writer.Create(ServerOpcodes.GAME_EXCHANGE_GOLD)
                        writer.Byte(2)
                        writer.QWord(add_gold)
                        Server.Send(writer.GetBytes, PlayerData(index_).InExchangeWith)

                    ElseIf ExchangeData(PlayerData(index_).ExchangeID).Player2Index = index_ Then
                        ExchangeData(PlayerData(index_).ExchangeID).Player2Gold = add_gold

                        writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                        writer.Byte(1)
                        writer.Byte(&HD)
                        writer.QWord(add_gold)
                        Server.Send(writer.GetBytes, index_)

                        writer.Create(ServerOpcodes.GAME_EXCHANGE_GOLD)
                        writer.Byte(2)
                        writer.QWord(add_gold)
                        Server.Send(writer.GetBytes, PlayerData(index_).InExchangeWith)
                    End If
                End If
            End If
        End Sub

#End Region


#Region "Avatar"
        Public Sub OnAvatarEquip(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim Old_Slot As Byte = packet.Byte
            Dim New_Slot As Byte = packet.Byte
            SendNotice("Old: " & Old_Slot & " New: " & New_Slot)

            If PlayerData(Index_).InExchange Or PlayerData(Index_).InStall Or PlayerData(Index_).Alive = False Then
                Exit Sub
            End If


            If Inventorys(Index_).UserItems(Old_Slot).ItemID <> 0 Then
                Dim SourceInvItem As cInventoryItem = Inventorys(Index_).UserItems(Old_Slot)
                Dim SourceItem As cItem = GameDB.Items(SourceInvItem.ItemID)

                Dim _SourceRef As cRefItem = GetItemByID(SourceItem.ObjectID)
                New_Slot = GetInternalAvatarSlot(_SourceRef)
                Dim DestItem As cInventoryItem = Inventorys(Index_).AvatarItems(New_Slot)

                If DestItem.ItemID = 0 Then
                    Inventorys(Index_).AvatarItems(New_Slot).ItemID = SourceInvItem.ItemID

                    Inventorys(Index_).UserItems(Old_Slot).ItemID = 0
                ElseIf DestItem.ItemID <> 0 Then

                End If
            End If

            ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(Old_Slot), cInventoryItem.Type.Inventory)
            ItemManager.UpdateInvItem(Inventorys(Index_).AvatarItems(New_Slot), cInventoryItem.Type.AvatarInventory)


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1) 'success
            writer.Byte(36) 'type
            writer.Byte(Old_Slot)
            writer.Byte(New_Slot)
            writer.Word(0)
            writer.Byte(0) 'end
            Server.Send(writer.GetBytes, Index_)

            Server.SendIfPlayerIsSpawned(CreateEquippacket(Index_, Old_Slot, New_Slot, True), Index_)
        End Sub

        Public Sub OnAvatarUnEquip(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim Old_Slot As Byte = packet.Byte
            Dim New_Slot As Byte = packet.Byte
            SendNotice("UnOld: " & Old_Slot & " New: " & New_Slot)

            If PlayerData(Index_).InExchange Or PlayerData(Index_).InStall Or PlayerData(Index_).Alive = False Then
                Exit Sub
            End If


           If Inventorys(Index_).UserItems(Old_Slot).ItemID <> 0 Then
                Dim SourceInvItem As cInventoryItem = Inventorys(Index_).AvatarItems(Old_Slot)
                Dim SourceItem As cItem = GameDB.Items(SourceInvItem.ItemID)
                Dim _SourceRef As cRefItem = GetItemByID(SourceItem.ObjectID)

                Dim DestItem As cInventoryItem = Inventorys(Index_).UserItems(New_Slot)

                If DestItem.ItemID = 0 Then
                    Inventorys(Index_).UserItems(New_Slot).ItemID = SourceInvItem.ItemID

                    Inventorys(Index_).AvatarItems(Old_Slot).ItemID = 0
                ElseIf DestItem.ItemID <> 0 Then

                End If
            End If

            ItemManager.UpdateInvItem(Inventorys(Index_).AvatarItems(Old_Slot), cInventoryItem.Type.AvatarInventory)
            ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(New_Slot), cInventoryItem.Type.Inventory)


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1) 'success
            writer.Byte(35) 'type
            writer.Byte(Old_Slot)
            writer.Byte(New_Slot)
            writer.Word(0)
            writer.Byte(0) 'end
            Server.Send(writer.GetBytes, Index_)

            Server.SendIfPlayerIsSpawned(CreateUnEquippacket(Index_, Old_Slot, New_Slot, True), Index_)
        End Sub

#End Region

#Region "Helper Functions"

        Private Function CheckItemGender(ByVal tmpItem As cRefItem, ByVal Index_ As Integer) As Boolean
            Dim Gender As Integer = 0

            If _
                (PlayerData(Index_).Pk2ID >= 1907 And PlayerData(Index_).Pk2ID <= 1919) Or
                (PlayerData(Index_).Pk2ID >= 14717 And PlayerData(Index_).Pk2ID <= 14729) Then
                Gender = 1
            End If
            If _
                (PlayerData(Index_).Pk2ID >= 1920 And PlayerData(Index_).Pk2ID <= 1932) Or
                (PlayerData(Index_).Pk2ID >= 14730 And PlayerData(Index_).Pk2ID <= 14742) Then
                Gender = 0
            End If

            If Gender = tmpItem.GENDER Or tmpItem.GENDER = 2 Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function CheckLevel(ByVal tmpItem As cRefItem, ByVal Index_ As Integer) As Boolean
            If tmpItem.LV_REQ > PlayerData(Index_).Level Then
                Return False
            Else
                Return True
            End If
        End Function

        Private Function GetInternalAvatarSlot(ByVal _Refitem As cRefItem) As Byte
            If _Refitem.CLASS_A = 1 Then
                Select Case _Refitem.CLASS_B
                    Case 13
                        Select Case _Refitem.CLASS_C
                            Case 1
                                'Hat
                                Return 0
                            Case 2
                                'Body
                                Return 1
                            Case 3
                                'Attach
                                Return 3
                            Case Else
                                Debug.Print(_Refitem.ITEM_TYPE_NAME)
                        End Select
                    Case 14
                        'Nasrun
                        Return 2
                    Case Else
                        Debug.Print(_Refitem.ITEM_TYPE_NAME)
                End Select
            End If
            Return 255
        End Function

        Friend Function GetExternalAvatarSlot(ByVal _Refitem As cRefItem) As Byte
            If _Refitem.CLASS_A = 1 Then
                Select Case _Refitem.CLASS_B
                    Case 13
                        Select Case _Refitem.CLASS_C
                            Case 1
                                'Hat
                                Return 0
                            Case 2
                                'Body
                                Return 1
                            Case 3
                                'Attach
                                Return 1
                        End Select
                    Case 14
                        'Nasrun
                        Return 4
                End Select
            End If
            Return 255
        End Function

        Private Function CreateEquippacket(ByVal Index_ As Integer, ByVal Old_Slot As Byte, ByVal New_Slot As Byte, ByVal Avatar As Boolean) As Byte()
            Dim item As New cItem

            If Avatar Then
                item = GameDB.Items(Inventorys(Index_).AvatarItems(New_Slot).ItemID)
            Else
                item = GameDB.Items(Inventorys(Index_).UserItems(New_Slot).ItemID)
            End If

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_EQUIP_ITEM)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Byte(New_Slot)
            writer.DWord(item.ObjectID)
            writer.Byte(item.Plus)
            Return writer.GetBytes
        End Function

        Private Function CreateUnEquippacket(ByVal Index_ As Integer, ByVal Old_Slot As Byte, ByVal New_Slot As Byte, ByVal Avatar As Boolean) As Byte()
            Dim item As New cItem

            If Avatar Then
                item = GameDB.Items(Inventorys(Index_).AvatarItems(New_Slot).ItemID)
            Else
                item = GameDB.Items(Inventorys(Index_).UserItems(New_Slot).ItemID)
            End If

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_UNEQUIP_ITEM)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Byte(Old_Slot)
            writer.DWord(item.ObjectID)
            Return writer.GetBytes
        End Function

        Public Function GetFreeItemSlot(ByVal Index_ As Integer) As Integer
            For i = 13 To Inventorys(Index_).UserItems.Length - 1
                If Inventorys(Index_).UserItems(i).ItemID = 0 Then
                    Return i
                End If
            Next
            Return -1
        End Function

        Public Sub AddItemDataToPacket(ByVal _item As cItem, ByVal writer As PacketWriter)
            Dim refitem As cRefItem = GetItemByID(_item.ObjectID)
            writer.DWord(0)
            'Unknown since TH Legend

            writer.DWord(_item.ObjectID)
            Debug.Print(refitem.ITEM_TYPE_NAME & " Type1: " & refitem.CLASS_A & " Type2: " & refitem.CLASS_B & " Type3: " & refitem.CLASS_C)
            Select Case refitem.CLASS_A
                Case 1 'Equipment
                    writer.Byte(_item.Plus)
                    writer.QWord(_item.Variance)
                    writer.DWord(_item.Data)

                    writer.Byte(_item.Blues.Count)
                    For i = 0 To _item.Blues.Count - 1
                        writer.DWord(_item.Blues(i).Type)
                        writer.DWord(_item.Blues(i).Amout)
                    Next

                    writer.Word(1)
                    writer.Word(2)
                    writer.Word(3)
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
                                Case Else
                                    writer.Word(_item.Data)
                            End Select
                        Case Else
                            writer.Word(_item.Data)
                    End Select

            End Select
        End Sub

        Public Sub OnItemMoveError(ByVal Index_ As Integer, ByVal Byte1 As Byte, ByVal Byte2 As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(2)
            writer.Byte(Byte1)
            writer.Byte(Byte2)
            Server.Send(writer.GetBytes, Index_)
        End Sub

#End Region
    End Module
End Namespace
