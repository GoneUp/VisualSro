Imports SRFramework

Namespace Functions
    Module UseItemScroll
        Public Sub OnUseReturnScroll(ByVal slot As Byte, ByVal Index_ As Integer)
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)
            Dim writer As New PacketWriter

            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                writer.Create(ServerOpcodes.GAME_ITEM_USE)
                writer.Byte(2)   'error
                writer.Byte(&H5D)    'already teleporting
                Server.Send(writer.GetBytes, Index_)
                Exit Sub
            End If

            If invItem.ItemID <> 0 And PlayerData(Index_).Alive Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 1 Then
                    UpdateItemAmout(Index_, Slot, -1)
                    UpdateState(&HB, 1, Index_)

                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(slot)
                    writer.Word(item.Data)
                    writer.Word(&HC30)
                    writer.Byte(refitem.CLASS_B)
                    writer.Byte(refitem.CLASS_C)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)

                    UsingItemTimer(Index_).Interval = refitem.USE_TIME_HP
                    UsingItemTimer(Index_).Start()
                    PlayerData(Index_).UsedItem = UseItemTypes.ReturnScroll
                    PlayerData(Index_).Busy = True
                End If
            End If
        End Sub

        Public Sub OnUseReverseScroll(ByVal slot As Byte, ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim tag As Byte = packet.Byte '2 = recall, 3= dead point, 7 = special point

            Dim writer As New PacketWriter

            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                writer.Create(ServerOpcodes.GAME_ITEM_USE)
                writer.Byte(2) 'error
                writer.Byte(&H5D)    'already teleporting
                Server.Send(writer.GetBytes, Index_)
                Exit Sub
            End If

            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)

            If invItem.ItemID <> 0 And PlayerData(Index_).Alive Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 3 Then 'Check for right Item
                    UpdateItemAmout(Index_, Slot, -1)
                    UpdateState(&HB, 1, Index_)

                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(slot)
                    writer.Word(item.Data)
                    writer.Word(&HC30)
                    writer.Byte(refitem.CLASS_B)
                    writer.Byte(refitem.CLASS_C)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)

                    UsingItemTimer(Index_).Interval = refitem.USE_TIME_HP
                    UsingItemTimer(Index_).Start()

                    Select Case tag
                        Case 2
                            PlayerData(Index_).UsedItem = UseItemTypes.ReverseScrollRecall
                        Case 3
                            PlayerData(Index_).UsedItem = UseItemTypes.ReverseScrollDead
                        Case 7
                            PlayerData(Index_).UsedItem = UseItemTypes.ReverseScrollPoint
                            PlayerData(Index_).UsedItemParameter = packet.DWord
                    End Select

                    PlayerData(Index_).Busy = True
                End If
            End If
        End Sub


        Public Sub OnReturnScrollCancel(ByVal Index_ As Integer)
            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                PlayerData(Index_).UsedItem = UseItemTypes.None
                UsingItemTimer(Index_).Stop()
                UpdateState(&HB, 0, Index_)
                PlayerData(Index_).Busy = False
            End If
        End Sub


        Public Sub OnUseBerserkScroll(ByVal slot As Byte, ByVal Index_ As Integer)
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)

            If invItem.ItemID <> 0 Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 1 And refitem.CLASS_C = 8 Then
                    UpdateItemAmout(Index_, Slot, -1)

                    PlayerData(Index_).BerserkBar = 5
                    UpdateBerserk(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(slot)
                    writer.Word(item.Data)
                    writer.Word(&HC30)
                    writer.Byte(refitem.CLASS_B)
                    writer.Byte(refitem.CLASS_C)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)
                End If
            End If
        End Sub
    End Module
End Namespace
