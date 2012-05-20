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
                tmp_ex.ExchangeID = Id_Gen.GetExchangeId
                ExchangeData.Add(tmp_ex.ExchangeID, tmp_ex)


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

                PlayerData(PlayerData(Index_).InExchangeWith).InExchangeWith = 0
                PlayerData(PlayerData(Index_).InExchangeWith).InExchange = False
                PlayerData(Index_).InExchangeWith = 0
                PlayerData(Index_).InExchange = False
            End If
        End Sub

        Public Sub OnExchangeUpdateItems(ByVal ExchangeId As Integer)


            '======Player 1
            Dim writer As New PacketWriter
            Dim itemcount As Byte = 0
            Dim temp_inv As cInventory = Inventorys((ExchangeData(ExchangeId).Player1Index))

            For own_decider = 0 To 1
                itemcount = 0
                writer.Create(ServerOpcodes.Exchange_UpdateItems)
                writer.DWord(PlayerData(ExchangeData(ExchangeId).Player1Index).UniqueId)

                For i = 0 To 11 'Count items
                    If ExchangeData(ExchangeId).Items1(i) <> - 1 Then
                        itemcount += 1
                    End If
                Next
                writer.Byte(itemcount)

                For i = 0 To 11 'Send Item Data
                    If ExchangeData(ExchangeId).Items1(i) <> - 1 Then

                        Dim _item As cInvItem = temp_inv.UserItems(ExchangeData(ExchangeId).Items1(i))
                        Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                        If own_decider = 0 Then
                            writer.Byte(_item.Slot)
                            'Fromslot
                        End If
                        writer.Byte(i)
                        'To Slot

                        AddItemDataToPacket(_item, writer)
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
            temp_inv = Inventorys((ExchangeData(ExchangeId).Player2Index))

            For own_decider = 0 To 1
                itemcount = 0
                writer.Create(ServerOpcodes.Exchange_UpdateItems)
                writer.DWord(PlayerData(ExchangeData(ExchangeId).Player2Index).UniqueId)

                For i = 0 To 11 'Count items
                    If ExchangeData(ExchangeId).Items2(i) <> - 1 Then
                        itemcount += 1
                    End If
                Next
                writer.Byte(itemcount)

                For i = 0 To 11 'Send Item Data
                    If ExchangeData(ExchangeId).Items2(i) <> - 1 Then

                        Dim _item As cInvItem = temp_inv.UserItems(ExchangeData(ExchangeId).Items2(i))
                        Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                        If own_decider = 0 Then
                            writer.Byte(_item.Slot)
                            'Fromslot
                        End If
                        writer.Byte(i)
                        'To Slot

                        AddItemDataToPacket(_item, writer)
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

        Public Sub FinishExchange(ByVal ExchangeId As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Exchange_Finsih)
            Server.Send(writer.GetBytes, ExchangeData(ExchangeId).Player1Index)
            writer.Create(ServerOpcodes.Exchange_Finsih)
            Server.Send(writer.GetBytes, ExchangeData(ExchangeId).Player2Index)


            Dim tmp_ex As cExchange = ExchangeData(ExchangeId)
            'Player 1 items --> Player 2
            For i = 0 To 11
                If tmp_ex.Items1(i) <> - 1 Then
                    Dim From_item As cInvItem = Inventorys(tmp_ex.Player1Index).UserItems(tmp_ex.Items1(i))
                    Dim To_Slot As Byte = GetFreeSlotExchage(tmp_ex.Player2Index)
                    Dim To_item As cInvItem = Inventorys(tmp_ex.Player2Index).UserItems(To_Slot)

                    'Overwrite
                    To_item.Pk2Id = From_item.Pk2Id
                    To_item.Durability = From_item.Durability
                    To_item.Plus = From_item.Plus
                    To_item.Amount = From_item.Amount
                    To_item.Blues = From_item.Blues
                    To_item.PerDurability = From_item.PerDurability
                    To_item.PerPhyRef = From_item.PerPhyRef
                    To_item.PerMagRef = From_item.PerMagRef
                    To_item.PerPhyAtk = From_item.PerPhyAtk
                    To_item.PerMagAtk = From_item.PerMagAtk
                    To_item.PerPhyDef = From_item.PerPhyDef
                    To_item.PerMagDef = From_item.PerMagDef
                    To_item.PerBlock = From_item.PerBlock
                    To_item.PerCritical = From_item.PerCritical
                    To_item.PerAttackRate = From_item.PerAttackRate
                    To_item.PerParryRate = From_item.PerParryRate
                    To_item.PerPhyAbs = From_item.PerPhyAbs
                    To_item.PerMagAbs = From_item.PerMagAbs

                    'Add to new...
                    Inventorys(tmp_ex.Player2Index).UserItems(To_item.Slot) = To_item
                    UpdateItem(To_item)

                    'Remove...
                    DeleteItemFromDB(tmp_ex.Items1(i), tmp_ex.Player1Index)
                    Inventorys(tmp_ex.Player1Index).UserItems(tmp_ex.Items1(i)) = ClearItem(From_item)
                End If
            Next
            PlayerData(tmp_ex.Player1Index).Gold += tmp_ex.Player2Gold
            PlayerData(tmp_ex.Player1Index).Gold -= tmp_ex.Player1Gold
            UpdateGold(tmp_ex.Player1Index)


            'Player 2 Items --> Player 1 
            For i = 0 To 11
                If tmp_ex.Items2(i) <> - 1 Then

                    Dim From_item As cInvItem = Inventorys(tmp_ex.Player2Index).UserItems(tmp_ex.Items2(i))
                    Dim To_Slot As Byte = GetFreeSlotExchage(tmp_ex.Player1Index)
                    Dim To_item As cInvItem = Inventorys(tmp_ex.Player1Index).UserItems(To_Slot)

                    'Add to new...
                    To_item.Pk2Id = From_item.Pk2Id
                    To_item.Durability = From_item.Durability
                    To_item.Plus = From_item.Plus
                    To_item.Amount = From_item.Amount
                    To_item.Blues = From_item.Blues
                    To_item.PerDurability = From_item.PerDurability
                    To_item.PerPhyRef = From_item.PerPhyRef
                    To_item.PerMagRef = From_item.PerMagRef
                    To_item.PerPhyAtk = From_item.PerPhyAtk
                    To_item.PerMagAtk = From_item.PerMagAtk
                    To_item.PerPhyDef = From_item.PerPhyDef
                    To_item.PerMagDef = From_item.PerMagDef
                    To_item.PerBlock = From_item.PerBlock
                    To_item.PerCritical = From_item.PerCritical
                    To_item.PerAttackRate = From_item.PerAttackRate
                    To_item.PerParryRate = From_item.PerParryRate
                    To_item.PerPhyAbs = From_item.PerPhyAbs
                    To_item.PerMagAbs = From_item.PerMagAbs

                    Inventorys(tmp_ex.Player1Index).UserItems(To_item.Slot) = To_item
                    UpdateItem(To_item)

                    'Remove...
                    DeleteItemFromDB(tmp_ex.Items2(i), tmp_ex.Player2Index)
                    Inventorys(tmp_ex.Player2Index).UserItems(tmp_ex.Items2(i)) = ClearItem(From_item)

                End If
            Next
            PlayerData(tmp_ex.Player2Index).Gold += tmp_ex.Player1Gold
            PlayerData(tmp_ex.Player2Index).Gold -= tmp_ex.Player2Gold
            UpdateGold(tmp_ex.Player2Index)

            'Clean up
            ExchangeData.Remove(ExchangeId)
            PlayerData(tmp_ex.Player1Index).ExchangeID = 0
            PlayerData(tmp_ex.Player1Index).InExchangeWith = 0
            PlayerData(tmp_ex.Player1Index).InExchange = False

            PlayerData(tmp_ex.Player2Index).ExchangeID = 0
            PlayerData(tmp_ex.Player2Index).InExchangeWith = 0
            PlayerData(tmp_ex.Player2Index).InExchange = False

            For i = 0 To Inventorys(tmp_ex.Player1Index).UserItems.Count - 1
                Inventorys(tmp_ex.Player1Index).UserItems(i).Locked = False
            Next

            For i = 0 To Inventorys(tmp_ex.Player2Index).UserItems.Count - 1
                Inventorys(tmp_ex.Player2Index).UserItems(i).Locked = False
            Next
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
                        ExchangeData.Remove(tmp_ex.ExchangeID)
                        PlayerData(tmp_ex.Player1Index).ExchangeID = 0
                        PlayerData(tmp_ex.Player1Index).InExchangeWith = 0
                        PlayerData(tmp_ex.Player1Index).InExchange = False


                    Else
                        writer.Create(ServerOpcodes.Exchange_Abort_Reply)
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

        Public Sub Exchange_AbortFromServer(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Exchange_Abort_Reply)
            writer.Byte(2)
            writer.Byte(&H1B)
            Server.Send(writer.GetBytes, PlayerData(index_).InExchangeWith)

            PlayerData(index_).ExchangeID = 0
            PlayerData(index_).InExchangeWith = 0
            PlayerData(index_).InExchange = False
        End Sub

        Public Function GetFreeSlotExchage(ByVal Index_ As Integer)
            For r = 13 To Inventorys(Index_).UserItems.Length - 1
                If Inventorys(Index_).UserItems(r).Pk2Id = 0 And Inventorys(Index_).UserItems(r).Locked = False Then
                    Return r
                End If
            Next
            Return - 1
        End Function
    End Module
End Namespace