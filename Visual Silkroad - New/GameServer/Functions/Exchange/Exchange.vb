Namespace GameServer.Functions
    Module Exchange

        Public Sub OnExchangeInvite(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Dim Others_ID As UInt32 = Packet.DWord

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Exchange_Invite)

            writer.Byte(1)
            writer.DWord(PlayerData(Index_).UniqueId)

            For i As Integer = 0 To Server.OnlineClient
                If PlayerData(i).UniqueId = Others_ID Then
                    Server.Send(writer.GetBytes, i)
                    PlayerData(Index_).InExchangeWith = i
                    PlayerData(i).InExchangeWith = Index_
                    Exit For
                End If
            Next
        End Sub

        Public Sub OnExchangeInviteReply(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Dim type As Byte = Packet.Byte
            Dim succes As Boolean = Packet.Boolean
            Dim writer As New PacketWriter

            If succes = True Then

                writer.Create(ServerOpcodes.Exchange_Start)
                writer.DWord(PlayerData(PlayerData(Index_).InExchangeWith).UniqueId)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.Exchange_Invite_Reply)
                writer.Byte(1)
                writer.DWord(PlayerData(Index_).UniqueId)
                Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)

                Dim tmp_ex As New cExchange
                tmp_ex.Player1Index = (PlayerData(Index_).InExchangeWith)
                tmp_ex.Player2Index = Index_
                ExchangeData.Add(tmp_ex)
                tmp_ex.ExchangeID = (ExchangeData.IndexOf(tmp_ex))

                PlayerData(Index_).ExchangeID = tmp_ex.ExchangeID
                PlayerData(PlayerData(Index_).InExchangeWith).ExchangeID = tmp_ex.ExchangeID
                PlayerData(Index_).InExchange = True
                PlayerData(PlayerData(Index_).InExchangeWith).InExchange = True

            ElseIf succes = False Then
                writer.Create(ServerOpcodes.Exchange_Error)
                writer.Byte(&H28)
                Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)

                writer.Create(ServerOpcodes.Exchange_Error)
                writer.Byte(&H28)
                Server.Send(writer.GetBytes, Index_)

                PlayerData(PlayerData(Index_).InExchangeWith).InExchangeWith = -1
                PlayerData(Index_).InExchangeWith = -1
                PlayerData(Index_).InExchange = False
                PlayerData(PlayerData(Index_).InExchangeWith).InExchange = False
            End If
        End Sub

        Public Sub OnExchangeUpdateItems(ByVal ExListInd As Integer)


            '======Player 1
            Dim writer As New PacketWriter
            Dim itemcount As Byte = 0
            Dim temp_inv As cInventory = Inventorys((ExchangeData(ExListInd).Player1Index))

            For own_decider = 0 To 1
                itemcount = 0
                writer.Create(ServerOpcodes.Exchange_UpdateItems)
                writer.DWord(PlayerData(ExchangeData(ExListInd).Player1Index).UniqueId)

                For i = 0 To 11 'Count items
                    If ExchangeData(ExListInd).Items1(i) <> -1 Then
                        itemcount += 1
                    End If
                Next
                writer.Byte(itemcount)

                For i = 0 To 11 'Send Item Data
                    If ExchangeData(ExListInd).Items1(i) <> -1 Then

                        Dim _item As cInvItem = temp_inv.UserItems(ExchangeData(ExListInd).Items1(i))
                        Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                        If own_decider = 0 Then
                            writer.Byte(_item.Slot) 'Fromslot
                        End If
                        writer.Byte(i) 'To Slot
                        writer.DWord(_item.Pk2Id)

                        Select Case refitem.CLASS_A 'Hope this 
                            Case 1 'Equipment
                                writer.Byte(_item.Plus)
                                writer.QWord(0)
                                writer.DWord(_item.Durability)
                                writer.Byte(0) '0 blues
                            Case 2 'Pets

                            Case 3 'etc
                                writer.Word(_item.Amount)
                        End Select
                    End If
                Next

                If own_decider = 0 Then
                    Server.Send(writer.GetBytes, ExchangeData(ExListInd).Player1Index)
                Else
                    Server.Send(writer.GetBytes, ExchangeData(ExListInd).Player2Index)
                End If
            Next

            '=========Player 2
            itemcount = 0
            temp_inv = Inventorys((ExchangeData(ExListInd).Player2Index))

            For own_decider = 0 To 1
                itemcount = 0
                writer.Create(ServerOpcodes.Exchange_UpdateItems)
                writer.DWord(PlayerData(ExchangeData(ExListInd).Player2Index).UniqueId)

                For i = 0 To 11 'Count items
                    If ExchangeData(ExListInd).Items2(i) <> -1 Then
                        itemcount += 1
                    End If
                Next
                writer.Byte(itemcount)

                For i = 0 To 11 'Send Item Data
                    If ExchangeData(ExListInd).Items2(i) <> -1 Then

                        Dim _item As cInvItem = temp_inv.UserItems(ExchangeData(ExListInd).Items2(i))
                        Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                        If own_decider = 0 Then
                            writer.Byte(_item.Slot) 'Fromslot
                        End If
                        writer.Byte(i) 'To Slot
                        writer.DWord(_item.Pk2Id)

                        Select Case refitem.CLASS_A 'Hope this 
                            Case 1 'Equipment
                                writer.Byte(_item.Plus)
                                writer.QWord(0)
                                writer.DWord(_item.Durability)
                                writer.Byte(0) '0 blues
                            Case 2 'Pets

                            Case 3 'etc
                                writer.Word(_item.Amount)
                        End Select
                    End If
                Next
                If own_decider = 0 Then
                    Server.Send(writer.GetBytes, ExchangeData(ExListInd).Player2Index)
                Else
                    Server.Send(writer.GetBytes, ExchangeData(ExListInd).Player1Index)
                End If
            Next
        End Sub

        Public Sub OnExchangeConfirm(ByVal Packet As PacketReader, ByVal Index_ As Integer)
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
                writer.Create(ServerOpcodes.Exchange_Confirm_Reply)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.Exchange_Confirm_Other)
                Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)
            End If
        End Sub

        Public Sub OnExchangeApprove(ByVal Packet As PacketReader, ByVal Index_ As Integer)

            If PlayerData(Index_).InExchange = True Then
                Dim exlist As Integer = PlayerData(Index_).ExchangeID
                Dim writer As New PacketWriter

                If ExchangeData(exlist).Player1Index = Index_ Then
                    If ExchangeData(exlist).ApprovePlyr1 = False And ExchangeData(exlist).ApprovePlyr2 = False Then
                        ExchangeData(exlist).ApprovePlyr1 = True

                        writer.Create(ServerOpcodes.Exchange_Approve_Reply)
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

                        writer.Create(ServerOpcodes.Exchange_Approve_Reply)
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
        Public Sub FinishExchange(ByVal ExchangeSession As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Exchange_Finsih)
            Server.Send(writer.GetBytes, ExchangeData(ExchangeSession).Player1Index)
            writer.Create(ServerOpcodes.Exchange_Finsih)
            Server.Send(writer.GetBytes, ExchangeData(ExchangeSession).Player2Index)


            Dim tmp_ex As cExchange = ExchangeData(ExchangeSession)
            'Player 1 items --> Player 2
            For i = 0 To 11
                If tmp_ex.Items1(i) <> -1 Then

                    Dim remove_item As cInvItem = Inventorys(tmp_ex.Player1Index).UserItems(tmp_ex.Items1(i))
                    Dim add_item As cInvItem = Inventorys(tmp_ex.Player1Index).UserItems(tmp_ex.Items1(i))

                    'Remove...
                    DeleteItemFromDB(tmp_ex.Items1(i), tmp_ex.Player1Index)
                    remove_item.Pk2Id = 0
                    remove_item.Durability = 0
                    remove_item.Plus = 0
                    remove_item.Amount = 0

                    Inventorys(tmp_ex.Player1Index).UserItems(tmp_ex.Items1(i)) = remove_item

                    'Add to new...
                    add_item.OwnerCharID = PlayerData(tmp_ex.Player2Index).UniqueId
                    add_item.Slot = GetFreeItemSlot(tmp_ex.Player2Index)
                    Inventorys(tmp_ex.Player2Index).UserItems(add_item.Slot) = add_item
                    UpdateItem(add_item)
                End If
            Next
            PlayerData(tmp_ex.Player1Index).Gold += tmp_ex.Player2Gold



            'Player 2 Items --> Player 1 
            For i = 0 To 11
                If tmp_ex.Items2(i) <> -1 Then

                    Dim remove_item As cInvItem = Inventorys(tmp_ex.Player2Index).UserItems(tmp_ex.Items2(i))
                    Dim add_item As cInvItem = Inventorys(tmp_ex.Player2Index).UserItems(tmp_ex.Items2(i))

                    'Remove...
                    DeleteItemFromDB(tmp_ex.Items2(i), tmp_ex.Player2Index)
                    remove_item.Pk2Id = 0
                    remove_item.Durability = 0
                    remove_item.Plus = 0
                    remove_item.Amount = 0

                    Inventorys(tmp_ex.Player2Index).UserItems(tmp_ex.Items2(i)) = remove_item

                    'Add to new...
                    add_item.OwnerCharID = PlayerData(tmp_ex.Player1Index).UniqueId
                    add_item.Slot = GetFreeItemSlot(tmp_ex.Player1Index)
                    Inventorys(tmp_ex.Player1Index).UserItems(add_item.Slot) = add_item
                    UpdateItem(add_item)
                End If
            Next
            PlayerData(tmp_ex.Player2Index).Gold += tmp_ex.Player1Gold

            'Clean up
            ExchangeData.RemoveAt(ExchangeSession)
            PlayerData(tmp_ex.Player1Index).ExchangeID = -1
            PlayerData(tmp_ex.Player1Index).InExchangeWith = -1
            PlayerData(tmp_ex.Player1Index).InExchange = False

            PlayerData(tmp_ex.Player2Index).ExchangeID = -1
            PlayerData(tmp_ex.Player2Index).InExchangeWith = -1
            PlayerData(tmp_ex.Player2Index).InExchange = False

            Inventorys(tmp_ex.Player1Index).ReOrderItems(tmp_ex.Player1Index)
            Inventorys(tmp_ex.Player2Index).ReOrderItems(tmp_ex.Player2Index)

        End Sub


        Public Sub OnExchangeAbort(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Dim writer As New PacketWriter

            If PlayerData(Index_).InExchange = True Then
                If ExchangeData(PlayerData(Index_).ExchangeID).Aborted = False Then

                    writer.Create(ServerOpcodes.Exchange_Error)
                    writer.Byte(&H2C)
                    Server.Send(writer.GetBytes, Index_)

                    writer.Create(ServerOpcodes.Exchange_Error)
                    writer.Byte(&H2C)
                    Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)

                    ExchangeData(PlayerData(Index_).ExchangeID).Aborted = True
                    ExchangeData(PlayerData(Index_).ExchangeID).AbortedFrom = Index_
                Else
                    If ExchangeData(PlayerData(Index_).ExchangeID).AbortedFrom = Index_ Then
                        writer.Create(ServerOpcodes.Exchange_Abort_Reply)
                        writer.Byte(1)
                        Server.Send(writer.GetBytes, Index_)

                        writer.Create(ServerOpcodes.Exchange_Abort_Reply)
                        writer.Byte(2)
                        writer.Byte(&H1B)
                        Server.Send(writer.GetBytes, Index_)

                        Dim tmp_ex As cExchange = ExchangeData(PlayerData(Index_).ExchangeID)
                        ExchangeData.RemoveAt(PlayerData(Index_).ExchangeID)
                        PlayerData(tmp_ex.Player1Index).ExchangeID = -1
                        PlayerData(tmp_ex.Player1Index).InExchangeWith = -1
                        PlayerData(tmp_ex.Player1Index).InExchange = False


                    Else
                        writer.Create(ServerOpcodes.Exchange_Abort_Reply)
                        writer.Byte(2)
                        writer.Byte(&H1B)
                        Server.Send(writer.GetBytes, Index_)

                        PlayerData(Index_).ExchangeID = -1
                        PlayerData(Index_).InExchangeWith = -1
                        PlayerData(Index_).InExchange = False

                    End If



                End If

            End If
        End Sub
    End Module
End Namespace