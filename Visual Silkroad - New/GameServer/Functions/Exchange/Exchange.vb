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
            Dim succes As Byte = Packet.Byte
            Dim writer As New PacketWriter

            If succes = 1 Then

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

            Else
                writer.Create(ServerOpcodes.Exchange_Invite_Reply)
                writer.Byte(2)
                Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)

                PlayerData(PlayerData(Index_).InExchangeWith).InExchangeWith = -1
                PlayerData(Index_).InExchangeWith = -1
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
            temp_inv = Inventorys((ExchangeData(ExListInd).Player1Index))

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
        End Sub

        Public Sub OnExchangeConfirm(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            If PlayerData(Index_).InExchange = True Then
                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Exchange_Confirm_Reply)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.Exchange_Confirm_Other)
                Server.Send(writer.GetBytes, PlayerData(Index_).InExchangeWith)
            End If
        End Sub
    End Module
End Namespace