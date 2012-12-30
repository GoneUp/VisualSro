Imports SRFramework

Namespace Functions
    Module ExchangeHandler
        Public Sub OnExchangeInvite(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim othersID As UInt32 = packet.DWord

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_EXCHANGE_INVITE)

            writer.Byte(1)
            writer.DWord(PlayerData(Index_).UniqueID)

            For i As Integer = 0 To Server.OnlineClients
                If PlayerData(i).UniqueID = othersID Then
                    Server.Send(writer.GetBytes, i)
                    PlayerData(Index_).InExchangeWith = i
                    PlayerData(i).InExchangeWith = Index_
                    Exit For
                End If
            Next
        End Sub

        Public Sub OnExchangeInviteReply(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim type As Byte = Packet.Byte
            Dim succes As Boolean = Packet.Boolean
            Dim writer As New PacketWriter

            If succes = True Then

                writer.Create(ServerOpcodes.GAME_EXCHANGE_START)
                writer.DWord(PlayerData(PlayerData(Index_).InExchangeWith).UniqueID)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.GAME_EXCHANGE_INVITE_REPLY)
                writer.Byte(1)
                writer.DWord(PlayerData(Index_).UniqueID)
                Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)

                Dim tmp_ex As New cExchange
                tmp_ex.Player1Index = (PlayerData(Index_).InExchangeWith)
                tmp_ex.Player2Index = Index_
                tmp_ex.ExchangeID = Id_Gen.GetExchangeId
                ExchangeData.Add(tmp_ex.ExchangeID, tmp_ex)


                PlayerData(Index_).ExchangeID = tmp_ex.ExchangeID
                PlayerData(PlayerData(Index_).InExchangeWith).ExchangeID = tmp_ex.ExchangeID
                PlayerData(Index_).InExchange = True
                PlayerData(PlayerData(Index_).InExchangeWith).InExchange = True

            ElseIf succes = False Then
                writer.Create(ServerOpcodes.GAME_EXCHANGE_ERROR)
                writer.Byte(&H28)
                Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)

                writer.Create(ServerOpcodes.GAME_EXCHANGE_ERROR)
                writer.Byte(&H28)
                Server.Send(writer.GetBytes, Index_)

                PlayerData(PlayerData(Index_).InExchangeWith).InExchangeWith = 0
                PlayerData(PlayerData(Index_).InExchangeWith).InExchange = False
                PlayerData(Index_).InExchangeWith = 0
                PlayerData(Index_).InExchange = False
            End If
        End Sub

        Public Sub OnExchangeUpdateItems(ByVal exchangeId As Integer)


            '======Player 1
            Dim writer As New PacketWriter
            Dim itemcount As Byte = 0
            Dim tempInv As cInventory = Inventorys((ExchangeData(ExchangeId).Player1Index))

            For own_decider = 0 To 1
                itemcount = 0
                writer.Create(ServerOpcodes.GAME_EXCHANGE_UPDATE_ITEMS)
                writer.DWord(PlayerData(ExchangeData(ExchangeId).Player1Index).UniqueID)

                For i = 0 To 11 'Count items
                    If ExchangeData(ExchangeId).Items1(i) <> -1 Then
                        itemcount += 1
                    End If
                Next
                writer.Byte(itemcount)

                For i = 0 To 11 'Send Item Data
                    If ExchangeData(ExchangeId).Items1(i) <> -1 Then

                        Dim invItem As cInventoryItem = tempInv.UserItems(ExchangeData(ExchangeId).Items1(i))
                        Dim item As cItem = GameDB.Items(invItem.ItemID)
                        Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                        If own_decider = 0 Then
                            writer.Byte(invItem.Slot) 'Fromslot
                        End If
                        writer.Byte(i) 'To Slot


                        AddItemDataToPacket(item, writer)
                    End If
                Next

                If own_decider = 0 Then
                    Server.Send(writer.GetBytes, ExchangeData(ExchangeId).Player1Index)
                Else
                    Server.Send(writer.GetBytes, ExchangeData(ExchangeId).Player2Index)
                End If
            Next

            '=========Player 2
            itemcount = 0
            tempInv = Inventorys((ExchangeData(ExchangeId).Player2Index))

            For own_decider = 0 To 1
                itemcount = 0
                writer.Create(ServerOpcodes.GAME_EXCHANGE_UPDATE_ITEMS)
                writer.DWord(PlayerData(ExchangeData(ExchangeId).Player2Index).UniqueID)

                For i = 0 To 11 'Count items
                    If ExchangeData(ExchangeId).Items2(i) <> -1 Then
                        itemcount += 1
                    End If
                Next
                writer.Byte(itemcount)

                For i = 0 To 11 'Send Item Data
                    If ExchangeData(ExchangeId).Items2(i) <> -1 Then
                        Dim invItem As cInventoryItem = tempInv.UserItems(ExchangeData(ExchangeId).Items2(i))
                        Dim item As cItem = GameDB.Items(invItem.ItemID)
                        Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                        If own_decider = 0 Then
                            writer.Byte(invItem.Slot) 'Fromslot
                        End If
                        writer.Byte(i)   'To Slot

                        AddItemDataToPacket(item, writer)
                    End If
                Next
                If own_decider = 0 Then
                    'Own Data with own Slots
                    Server.Send(writer.GetBytes, ExchangeData(ExchangeId).Player2Index)
                Else
                    Server.Send(writer.GetBytes, ExchangeData(ExchangeId).Player1Index)
                End If
            Next
        End Sub

        Public Sub OnExchangeConfirm(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If PlayerData(Index_).InExchange = True Then
                Dim exlist As Integer = PlayerData(Index_).ExchangeID

                If ExchangeData(exlist).Player1Index = Index_ Then
                    If ExchangeData(exlist).ConfirmPlyr1 = False Then
                        ExchangeData(exlist).ConfirmPlyr1 = True
                    Else
                        'Hack Attempt
                        Exit Sub
                    End If

                ElseIf ExchangeData(exlist).Player2Index = Index_ Then
                    If ExchangeData(exlist).ConfirmPlyr2 = False Then
                        ExchangeData(exlist).ConfirmPlyr2 = True
                    Else
                        'Hack Attempt
                        Exit Sub
                    End If
                End If
                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_EXCHANGE_CONFIRM_REPLY)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.GAME_EXCHANGE_CONFIRM_OTHER)
                Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)
            End If
        End Sub

        Public Sub OnExchangeApprove(ByVal packet As PacketReader, ByVal Index_ As Integer)

            If PlayerData(Index_).InExchange = True Then
                Dim exlist As Integer = PlayerData(Index_).ExchangeID
                Dim writer As New PacketWriter

                If ExchangeData(exlist).Player1Index = Index_ Then
                    If ExchangeData(exlist).ApprovePlyr1 = False And ExchangeData(exlist).ApprovePlyr2 = False Then
                        ExchangeData(exlist).ApprovePlyr1 = True

                        writer.Create(ServerOpcodes.GAME_EXCHANGE_APPROVE_REPLY)
                        writer.Byte(1)
                        Server.Send(writer.GetBytes, Index_)

                    ElseIf ExchangeData(exlist).ApprovePlyr1 = False And ExchangeData(exlist).ApprovePlyr2 = True Then
                        'Finish Exchange
                        FinishExchange(exlist)
                    Else

                        'Hack Attempt
                        Exit Sub
                    End If

                ElseIf ExchangeData(exlist).Player2Index = Index_ Then

                    If ExchangeData(exlist).ApprovePlyr2 = False And ExchangeData(exlist).ApprovePlyr1 = False Then
                        ExchangeData(exlist).ApprovePlyr2 = True

                        writer.Create(ServerOpcodes.GAME_EXCHANGE_APPROVE_REPLY)
                        writer.Byte(1)
                        Server.Send(writer.GetBytes, Index_)

                    ElseIf ExchangeData(exlist).ApprovePlyr2 = False And ExchangeData(exlist).ApprovePlyr1 = True Then
                        'Finish Exchange
                        FinishExchange(exlist)
                    Else
                        'Hack Attempt
                        Exit Sub
                    End If


                End If
            End If
        End Sub

        Private Sub FinishExchange(ByVal exchangeId As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_EXCHANGE_FINISH)
            Server.Send(writer.GetBytes, ExchangeData(exchangeId).Player1Index)
            writer.Create(ServerOpcodes.GAME_EXCHANGE_FINISH)
            Server.Send(writer.GetBytes, ExchangeData(exchangeId).Player2Index)


            Dim tmpEx As cExchange = ExchangeData(exchangeId)
            'Player 1 items --> Player 2
            For i = 0 To 11
                If tmpEx.Items1(i) <> -1 Then
                    Dim fromItem As cInventoryItem = Inventorys(tmpEx.Player1Index).UserItems(tmpEx.Items1(i))
                    Dim toSlot As Byte = GetFreeSlotExchage(tmpEx.Player2Index)
                    Dim toItem As cInventoryItem = Inventorys(tmpEx.Player2Index).UserItems(toSlot)


                    'Add to new...
                    Inventorys(tmpEx.Player2Index).UserItems(toSlot).ItemID = fromItem.ItemID
                    ItemManager.UpdateInvItem(Inventorys(tmpEx.Player2Index).UserItems(toSlot), InvItemTypes.Inventory)

                    'Remove...
                    Inventorys(tmpEx.Player1Index).UserItems(tmpEx.Items1(i)).ItemID = 0
                    ItemManager.UpdateInvItem(Inventorys(tmpEx.Player1Index).UserItems(tmpEx.Items1(i)), InvItemTypes.Inventory)
                End If
            Next
            PlayerData(tmpEx.Player1Index).Gold += tmpEx.Player2Gold
            PlayerData(tmpEx.Player1Index).Gold -= tmpEx.Player1Gold
            UpdateGold(tmpEx.Player1Index)


            'Player 2 Items --> Player 1 
            For i = 0 To 11
                If tmpEx.Items2(i) <> -1 Then
                    Dim fromItem As cInventoryItem = Inventorys(tmpEx.Player2Index).UserItems(tmpEx.Items2(i))
                    Dim toSlot As Byte = GetFreeSlotExchage(tmpEx.Player1Index)
                    Dim toItem As cInventoryItem = Inventorys(tmpEx.Player1Index).UserItems(toSlot)

                    'Add to Player 1's invenotry
                    Inventorys(tmpEx.Player1Index).UserItems(toItem.Slot).ItemID = fromItem.ItemID
                    ItemManager.UpdateInvItem(Inventorys(tmpEx.Player1Index).UserItems(toItem.Slot), InvItemTypes.Inventory)

                    'Remove...
                    Inventorys(tmpEx.Player2Index).UserItems(tmpEx.Items2(i)).ItemID = 0
                    ItemManager.UpdateInvItem(Inventorys(tmpEx.Player2Index).UserItems(tmpEx.Items2(i)), InvItemTypes.Inventory)

                End If
            Next
            PlayerData(tmpEx.Player2Index).Gold += tmpEx.Player1Gold
            PlayerData(tmpEx.Player2Index).Gold -= tmpEx.Player2Gold
            UpdateGold(tmpEx.Player2Index)

            'Clean up
            ExchangeData.Remove(exchangeId)
            PlayerData(tmpEx.Player1Index).ExchangeID = 0
            PlayerData(tmpEx.Player1Index).InExchangeWith = 0
            PlayerData(tmpEx.Player1Index).InExchange = False

            PlayerData(tmpEx.Player2Index).ExchangeID = 0
            PlayerData(tmpEx.Player2Index).InExchangeWith = 0
            PlayerData(tmpEx.Player2Index).InExchange = False

            For i = 0 To Inventorys(tmpEx.Player1Index).UserItems.Count - 1
                Inventorys(tmpEx.Player1Index).UserItems(i).Locked = False
            Next

            For i = 0 To Inventorys(tmpEx.Player2Index).UserItems.Count - 1
                Inventorys(tmpEx.Player2Index).UserItems(i).Locked = False
            Next
        End Sub


        Public Sub OnExchangeAbort(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim writer As New PacketWriter

            If PlayerData(Index_).InExchange = True Then
                If ExchangeData(PlayerData(Index_).ExchangeID).Aborted = False Then

                    writer.Create(ServerOpcodes.GAME_EXCHANGE_ERROR)
                    writer.Byte(&H2C)
                    Server.Send(writer.GetBytes, Index_)

                    writer.Create(ServerOpcodes.GAME_EXCHANGE_ERROR)
                    writer.Byte(&H2C)
                    Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)

                    ExchangeData(PlayerData(Index_).ExchangeID).Aborted = True
                    ExchangeData(PlayerData(Index_).ExchangeID).AbortedFrom = Index_
                Else
                    If ExchangeData(PlayerData(Index_).ExchangeID).AbortedFrom = Index_ Then
                        writer.Create(ServerOpcodes.GAME_EXCHANGE_ABORT_REPLY)
                        writer.Byte(1)
                        Server.Send(writer.GetBytes, Index_)

                        writer.Create(ServerOpcodes.GAME_EXCHANGE_ABORT_REPLY)
                        writer.Byte(2)
                        writer.Byte(&H1B)
                        Server.Send(writer.GetBytes, Index_)

                        Dim tmpEx As cExchange = ExchangeData(PlayerData(Index_).ExchangeID)
                        ExchangeData.Remove(tmpEx.ExchangeID)
                        PlayerData(tmpEx.Player1Index).ExchangeID = 0
                        PlayerData(tmpEx.Player1Index).InExchangeWith = 0
                        PlayerData(tmpEx.Player1Index).InExchange = False


                    Else
                        writer.Create(ServerOpcodes.GAME_EXCHANGE_ABORT_REPLY)
                        writer.Byte(2)
                        writer.Byte(&H1B)
                        Server.Send(writer.GetBytes, Index_)

                        PlayerData(Index_).ExchangeID = 0
                        PlayerData(Index_).InExchangeWith = 0
                        PlayerData(Index_).InExchange = False

                    End If
                End If
            End If
        End Sub

        Public Sub Exchange_AbortFromServer(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_EXCHANGE_ABORT_REPLY)
            writer.Byte(2)
            writer.Byte(&H1B)
            Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)

            PlayerData(Index_).ExchangeID = 0
            PlayerData(Index_).InExchangeWith = 0
            PlayerData(Index_).InExchange = False
        End Sub

        Private Function GetFreeSlotExchage(ByVal Index_ As Integer) As SByte
            For r = 13 To Inventorys(Index_).UserItems.Length - 1
                If Inventorys(Index_).UserItems(r).ItemID = 0 And Inventorys(Index_).UserItems(r).Locked = False Then
                    Return r
                End If
            Next
            Return -1
        End Function
    End Module
End Namespace