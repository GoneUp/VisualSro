Namespace GameServer.Functions
    Module Character

        Public Sub HandleCharPacket(ByVal Index_ As Integer, ByVal pack As GameServer.PacketReader)

            Dim tag As Byte = pack.Byte

            Select Case tag
                Case 1 'Crweate
                Case 2 'Char List
                    CharList(Index_)
                Case 3 'Char delete
                Case 4 'Nick Check
                    CheckNick(pack, Index_)
                Case 5 'Restore
            End Select
        End Sub

        Public Sub CharList(ByVal Index_ As Integer)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.CharList)

            ClientList.OnCharListing(Index_) = GameServer.DatabaseCore.FillCharList(ClientList.OnCharListing(Index_))

            writer.Byte(2) 'Char List
            writer.Byte(1)

            writer.Byte(ClientList.OnCharListing(Index_).NumberOfChars)

            If ClientList.OnCharListing(Index_).NumberOfChars = 0 Then
                GameServer.Server.Send(writer.GetBytes, Index_)

            ElseIf ClientList.OnCharListing(Index_).NumberOfChars > 0 Then
                For i = 0 To (ClientList.OnCharListing(Index_).NumberOfChars - 1)

                    writer.DWord(ClientList.OnCharListing(Index_).Chars(i).Model)
                    writer.Word(ClientList.OnCharListing(Index_).Chars(i).CharacterName.Length)
                    writer.String(ClientList.OnCharListing(Index_).Chars(i).CharacterName)
                    writer.Byte(ClientList.OnCharListing(Index_).Chars(i).Volume)
                    writer.Byte(ClientList.OnCharListing(Index_).Chars(i).Level)
                    writer.LWord(ClientList.OnCharListing(Index_).Chars(i).Experience)
                    writer.Word(ClientList.OnCharListing(Index_).Chars(i).Strength)
                    writer.Word(ClientList.OnCharListing(Index_).Chars(i).Intelligence)
                    writer.Word(ClientList.OnCharListing(Index_).Chars(i).Attributes)
                    writer.DWord(ClientList.OnCharListing(Index_).Chars(i).CHP)
                    writer.DWord(ClientList.OnCharListing(Index_).Chars(i).CMP)
                    writer.DWord(0)

                    'Now Items
                    Dim inventory As New cInventory(ClientList.OnCharListing(Index_).Chars(i).MaxSlots)
                    inventory = GameServer.DatabaseCore.FillInventory(ClientList.OnCharListing(Index_).Chars(i))

                    Dim PlayerItemCount As Integer = 0
                    For b = 0 To 9
                        If inventory.UserItems(b).Pk2Id <> 0 Then
                            PlayerItemCount += 1
                        End If
                    Next

                    writer.Byte(PlayerItemCount)

                    For b = 0 To 9
                        If inventory.UserItems(b).Pk2Id <> 0 Then
                            writer.DWord(inventory.UserItems(b).Pk2Id)
                            writer.Byte(inventory.UserItems(b).Plus)
                        End If
                    Next

                    writer.Byte(0) 'Char End

                Next

                GameServer.Server.Send(writer.GetBytes, Index_)

            End If






        End Sub

        Public Sub CheckNick(ByVal pack As PacketReader, ByVal Index_ As Integer)

            Dim nick As String = pack.String(pack.Word)
            Dim free As Boolean = GameServer.DatabaseCore.CheckNick(nick)
            Dim writer As New PacketWriter

            writer.Create(ServerOpcodes.CharList)
            writer.Byte(4) 'nick check

            If free = True Then
                writer.Byte(1)
            Else
                writer.Byte(2)
                writer.Byte(10)

            End If

            GameServer.Server.Send(writer.GetBytes, Index_)


        End Sub


    End Module
End Namespace
