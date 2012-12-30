Imports SRFramework

Namespace Functions
    Module ChatItemLink
        Public Sub OnAddItemLink(packet As PacketReader, Index_ As Integer)
            Dim slot As UInt32 = packet.DWord
            Dim realName As String = packet.String(packet.Word)

            Dim writer As New PacketWriter(ServerOpcodes.GAME_CHAT_ITEM_LINK_ACCEPT)

            'Item Checks
            If slot <= PlayerData(Index_).MaxInvSlots AndAlso Inventorys(Index_).UserItems(slot).ItemID <> 0 Then
                'Name Check
                If realName <> "" Then
                    Dim tmpLink As New cChatLinkItem()
                    tmpLink.LinkID = Id_Gen.GetChatLinkID()
                    tmpLink.ItemID = Inventorys(Index_).UserItems(slot).ItemID
                    tmpLink.RealName = realName
                    tmpLink.CreatorName = PlayerData(Index_).CharacterName

                    ChatLinkItemList.Add(tmpLink.LinkID, tmpLink)

                    writer.Byte(1)
                    writer.QWord(tmpLink.LinkID)
                    writer.Word(tmpLink.RealName.Length)
                    writer.String(tmpLink.RealName)

                Else
                    writer.Byte(2)
                End If

            Else
                writer.Byte(2)
            End If

            Server.Send(writer.GetBytes, Index_)
        End Sub
        Public Sub OnItemLinkInfo(packet As PacketReader, Index_ As Integer)
            Dim chattype As ChatModes = packet.Byte
            Dim receiver As String = ""

            If chattype = ChatModes.PmIncome Then
                receiver = packet.String(packet.Word)
            End If

            Dim count As Byte = packet.Byte

            Dim linkIDs(count - 1) As UInt64
            For i = 0 To count - 1
                linkIDs(i) = packet.QWord
            Next

            Dim confimedIDs As New List(Of UInt64)
            For Each linkID In linkIDs
                If ChatLinkItemList.ContainsKey(linkID) Then
                    confimedIDs.Add(linkID)
                End If
            Next

            SendLinkIDs(confimedIDs, Index_, chattype)
        End Sub

        Private Sub SendLinkIDs(linkIDs As List(Of UInt64), Index_ As Integer, mode As ChatModes) ', receiver As String)
            Dim writer As New PacketWriter(ServerOpcodes.GAME_CHAT_ITEM_LINK)
            writer.Byte(1)
            writer.Byte(linkIDs.Count)
            For Each linkID In linkIDs
                If ChatLinkItemList.ContainsKey(linkID) AndAlso GameDB.Items.ContainsKey(ChatLinkItemList(linkID).ItemID) Then
                    writer.QWord(ChatLinkItemList(linkID).LinkID)
                    AddItemDataToPacket(GameDB.Items(ChatLinkItemList(linkID).ItemID), writer)
                End If
            Next

            Select Case mode
                Case ChatModes.AllChat
                    Server.SendIfPlayerIsSpawned(writer.GetBytes(), Index_)

                Case ChatModes.PmIncome
                    Server.Send(writer.GetBytes, Index_)

                    For i = 0 To Server.OnlineClients - 1
                        If PlayerData(i) IsNot Nothing Then
                            If PlayerData(i).CharacterName = "" Then
                                Server.Send(writer.GetBytes, i)
                                Exit For
                            End If
                        End If
                    Next

                Case ChatModes.GameMaster
                    Server.SendIfPlayerIsSpawned(writer.GetBytes(), Index_)

                Case ChatModes.Party
                Case ChatModes.Guild
                    If PlayerData(Index_).InGuild Then
                        Server.SendToGuild(writer.GetBytes(), PlayerData(Index_).GuildID)
                    End If


                Case ChatModes.Union
                Case ChatModes.Academy

            End Select
        End Sub

    End Module
End Namespace
