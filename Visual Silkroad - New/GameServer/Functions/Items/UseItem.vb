Namespace GameServer.Functions
    Module UseItem
        Public Sub OnUseItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim id1 As Byte = packet.Byte
            Dim id2 As Byte = packet.Byte

            Debug.Print("[USE_ITEM][ID1:" & id1 & "][2:" & id2 & "]")

            If id1 = &HEC Then
                Select Case id2
                    Case &H8 'HP-Pot
                        UseHPPot(slot, Index_)
                    Case &H9 'Return Scroll
                        UseReturnScroll(slot, Index_)
                    Case &H10 'MP-Pot 
                        UseMPPot(slot, Index_)
                    Case &HE 'Speed Drug


                End Select

            ElseIf id1 = &HED Then
                 Select id2
                    Case &H19 'Reverse
                        UseReverseScroll(slot, Index_, packet)

                End Select


            End If
        End Sub

        Public Sub UseHPPot(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)

            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                If refitem.CLASS_A = 3 Then 'Check for right Item
                    If refitem.CLASS_B = 1 Then
                        If refitem.CLASS_C = 1 Then
                            If PlayerData(Index_).CHP + refitem.USE_TIME_HP >= PlayerData(Index_).HP Then
                                PlayerData(Index_).CHP = PlayerData(Index_).HP
                            Else
                                PlayerData(Index_).CHP += refitem.USE_TIME_HP
                            End If

                            If Inventorys(Index_).UserItems(Slot).Slot - 1 <= 0 Then
                                'Despawn Item

                                _item.Pk2Id = 0
                                _item.Durability = 0
                                _item.Plus = 0
                                _item.Amount = 0

                                Inventorys(Index_).UserItems(Slot) = _item
                                DeleteItemFromDB(Slot, Index_)

                            ElseIf Inventorys(Index_).UserItems(Slot).Slot - 1 > 0 Then
                                _item.Amount -= 1
                                Inventorys(Index_).UserItems(Slot) = _item
                                UpdateItem(_item)
                            End If


                            UpdateHP(Index_)

                            Dim writer As New PacketWriter
                            writer.Create(ServerOpcodes.ItemUse)
                            writer.Byte(1)
                            writer.Byte(Slot)
                            writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
                            writer.Byte(&HEC)
                            writer.Byte(&H8)
                            Server.Send(writer.GetBytes, Index_)

                            writer.Create(ServerOpcodes.ItemUseOtherPlayer)
                            writer.DWord(PlayerData(Index_).UniqueId)
                            writer.DWord(refitem.ITEM_TYPE)
                            Server.SendToAllInRange(writer.GetBytes, Index_)

                        End If
                    End If
                End If
            End If
        End Sub

        Public Sub UseMPPot(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)

            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                If refitem.CLASS_A = 3 Then 'Check for right Item
                    If refitem.CLASS_B = 1 Then
                        If refitem.CLASS_C = 2 Then
                            If PlayerData(Index_).CMP + refitem.USE_TIME_MP >= PlayerData(Index_).MP Then
                                PlayerData(Index_).CMP = PlayerData(Index_).MP
                            Else
                                PlayerData(Index_).CMP += refitem.USE_TIME_MP
                            End If

                            If Inventorys(Index_).UserItems(Slot).Slot - 1 <= 0 Then
                                'Despawn Item

                                _item.Pk2Id = 0
                                _item.Durability = 0
                                _item.Plus = 0
                                _item.Amount = 0

                                Inventorys(Index_).UserItems(Slot) = _item
                                DeleteItemFromDB(Slot, Index_)

                            ElseIf Inventorys(Index_).UserItems(Slot).Slot - 1 > 0 Then
                                _item.Amount -= 1
                                Inventorys(Index_).UserItems(Slot) = _item
                                UpdateItem(_item)
                            End If


                            UpdateMP(Index_)

                            Dim writer As New PacketWriter
                            writer.Create(ServerOpcodes.ItemUse)
                            writer.Byte(1)
                            writer.Byte(Slot)
                            writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
                            writer.Byte(&HEC)
                            writer.Byte(&H10)
                            Server.Send(writer.GetBytes, Index_)

                            writer.Create(ServerOpcodes.ItemUseOtherPlayer)
                            writer.DWord(PlayerData(Index_).UniqueId)
                            writer.DWord(refitem.ITEM_TYPE)
                            Server.SendToAllInRange(writer.GetBytes, Index_)

                        End If
                    End If
                End If
            End If
        End Sub

        Public Sub UseReverseScroll(ByVal Slot As Byte, ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)
            Dim writer As New PacketWriter
            Dim tag As Byte = packet.Byte '2 = recall 3= dead point

            If tag = 7 Then
                SendPm(Index_, "Teleport to specific Points via Reverse Scroll is not supported yet.", "[SERVER]")
                Exit Sub
            End If

            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                writer.Create(ServerOpcodes.ItemUse)
                writer.Byte(2) 'error
                writer.Byte(&H5D) 'already teleporting
                Server.Send(writer.GetBytes, Index_)
                Exit Sub
            End If

            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 3 Then 'Check for right Item

                    If Inventorys(Index_).UserItems(Slot).Slot - 1 <= 0 Then
                        'Despawn Item

                        _item.Pk2Id = 0
                        _item.Durability = 0
                        _item.Plus = 0
                        _item.Amount = 0

                        Inventorys(Index_).UserItems(Slot) = _item
                        DeleteItemFromDB(Slot, Index_)
                    ElseIf Inventorys(Index_).UserItems(Slot).Slot - 1 > 0 Then
                        _item.Amount -= 1
                        Inventorys(Index_).UserItems(Slot) = _item
                        UpdateItem(_item)
                    End If

                    UpdateState(&HB, 1, Index_)



                    writer.Create(ServerOpcodes.ItemUse)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
                    writer.Byte(&HED)
                    writer.Byte(&H19)
                    Server.Send(writer.GetBytes, Index_)

                    writer.Create(ServerOpcodes.ItemUseOtherPlayer)
                    writer.DWord(PlayerData(Index_).UniqueId)
                    writer.DWord(refitem.ITEM_TYPE)
                    Server.SendToAllInRange(writer.GetBytes, Index_)

                    Timers.UsingItemTimer(Index_).Interval = refitem.USE_TIME_HP
                    Timers.UsingItemTimer(Index_).Start()

                    Select Case tag
                        Case 2
                            PlayerData(Index_).UsedItem = UseItemTypes.Reverse_Scroll_Recall
                        Case 3
                            PlayerData(Index_).UsedItem = UseItemTypes.Reverse_Scroll_Dead
                    End Select

                    PlayerData(Index_).Busy = True
                End If
            End If
        End Sub

        Public Sub UseReturnScroll(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)
            Dim writer As New PacketWriter

            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                writer.Create(ServerOpcodes.ItemUse)
                writer.Byte(2) 'error
                writer.Byte(&H5D) 'already teleporting
                Server.Send(writer.GetBytes, Index_)
                Exit Sub
            End If

            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                If refitem.CLASS_A = 3 Then 'Check for right Item
                    If refitem.CLASS_B = 3 Then
                        If refitem.CLASS_C = 1 Then
                            If Inventorys(Index_).UserItems(Slot).Slot - 1 <= 0 Then
                                'Despawn Item

                                _item.Pk2Id = 0
                                _item.Durability = 0
                                _item.Plus = 0
                                _item.Amount = 0

                                Inventorys(Index_).UserItems(Slot) = _item
                                DeleteItemFromDB(Slot, Index_)
                            ElseIf Inventorys(Index_).UserItems(Slot).Slot - 1 > 0 Then
                                _item.Amount -= 1
                                Inventorys(Index_).UserItems(Slot) = _item
                                UpdateItem(_item)
                            End If

                            UpdateState(&HB, 1, Index_)



                            writer.Create(ServerOpcodes.ItemUse)
                            writer.Byte(1)
                            writer.Byte(Slot)
                            writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
                            writer.Byte(&HEC)
                            writer.Byte(&H9)
                            Server.Send(writer.GetBytes, Index_)

                            writer.Create(ServerOpcodes.ItemUseOtherPlayer)
                            writer.DWord(PlayerData(Index_).UniqueId)
                            writer.DWord(refitem.ITEM_TYPE)
                            Server.SendToAllInRange(writer.GetBytes, Index_)

                            Timers.UsingItemTimer(Index_).Interval = refitem.USE_TIME_HP
                            Timers.UsingItemTimer(Index_).Start()
                            PlayerData(Index_).UsedItem = UseItemTypes.Return_Scroll
                            PlayerData(Index_).Busy = True
                        End If
                    End If
                End If
            End If
        End Sub

        Public Sub OnReturnScroll_Cancel(ByVal Index_ As Integer)
            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                PlayerData(Index_).UsedItem = UseItemTypes.None
                Timers.UsingItemTimer(Index_).Stop()
                UpdateState(&HB, 0, Index_)
            End If
        End Sub
    End Module
End Namespace
