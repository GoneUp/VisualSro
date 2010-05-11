Namespace GameServer.Functions
    Module Character

        Public Sub HandleCharPacket(ByVal Index_ As Integer, ByVal pack As GameServer.PacketReader)

            Dim tag As Byte = pack.Byte

            Select Case tag
                Case 1 'Crweate
                    CreateChar(pack, Index_)
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
            writer.Create(ServerOpcodes.Character)

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

            writer.Create(ServerOpcodes.Character)
            writer.Byte(4) 'nick check

            If free = True Then
                writer.Byte(1)
            Else
                writer.Byte(2)
                writer.Byte(1)
                '13 invalid 10 server error


            End If

            GameServer.Server.Send(writer.GetBytes, Index_)


        End Sub

        Public Sub CreateChar(ByVal pack As PacketReader, ByVal Index_ As Integer)


            Dim nick As String = pack.String(pack.Word)
            Dim model As UInt32 = pack.DWord
            Dim volume As Byte = pack.Byte
            Dim _items(4) As UInt32
            _items(1) = pack.DWord
            _items(2) = pack.DWord
            _items(3) = pack.DWord
            _items(4) = pack.DWord

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Character)
            writer.Byte(1) 'create

            Dim free As Boolean = GameServer.DatabaseCore.CheckNick(nick)
            If free = False Then
                writer.Byte(4)
                writer.Byte(10)
                GameServer.Server.Send(writer.GetBytes, Index_)

            Else

                GameServer.DatabaseCore.CharCount += 1
                Array.Resize(GameServer.DatabaseCore.Chars, GameServer.DatabaseCore.CharCount)

                Dim newchar As New [cChar]
                Dim NewCharacterIndex As Integer = GameServer.DatabaseCore.CharCount - 1

                Debug.Print("Nick: " & nick & "NewCharIndex: " & NewCharacterIndex)

                DatabaseCore.Chars(NewCharacterIndex) = New [cChar]
                DatabaseCore.Chars(NewCharacterIndex).AccountID = ClientList.OnCharListing(Index_).LoginInformation.Id
                DatabaseCore.Chars(NewCharacterIndex).CharacterName = nick
                'DatabaseCore.Chars(NewCharacterIndex).CharacterId = DatabaseCore.Chars(NewCharacterIndex - 1).CharacterId + 1
                DatabaseCore.Chars(NewCharacterIndex).UniqueId = DatabaseCore.UniqueIdCounter
                DatabaseCore.UniqueIdCounter += 1
                DatabaseCore.Chars(NewCharacterIndex).HP = 200
                DatabaseCore.Chars(NewCharacterIndex).MP = 200
                DatabaseCore.Chars(NewCharacterIndex).CHP = 200
                DatabaseCore.Chars(NewCharacterIndex).CMP = 200
                DatabaseCore.Chars(NewCharacterIndex).Model = model
                DatabaseCore.Chars(NewCharacterIndex).Volume = volume
                DatabaseCore.Chars(NewCharacterIndex).Level = 1

                DatabaseCore.Chars(NewCharacterIndex).WalkSpeed = 16
                DatabaseCore.Chars(NewCharacterIndex).RunSpeed = 50
                DatabaseCore.Chars(NewCharacterIndex).BerserkSpeed = 100
                DatabaseCore.Chars(NewCharacterIndex).Strength = 20
                DatabaseCore.Chars(NewCharacterIndex).Intelligence = 20
                DatabaseCore.Chars(NewCharacterIndex).PVP = 0
                DatabaseCore.Chars(NewCharacterIndex).XSector = 168
                DatabaseCore.Chars(NewCharacterIndex).YSector = 98
                DatabaseCore.Chars(NewCharacterIndex).X = 978
                DatabaseCore.Chars(NewCharacterIndex).Z = 1097
                DatabaseCore.Chars(NewCharacterIndex).Y = 40
                DatabaseCore.Chars(NewCharacterIndex).MaxSlots = 45

                Dim magdefmin As Double = 3.0
                Dim phydefmin As Double = 6.0
                Dim parrymin As UShort = 11
                Dim phyatkmin As UShort = 6
                Dim phyatkmax As UShort = 9
                Dim magatkmin As UShort = 6
                Dim magatkmax As UShort = 10

                DatabaseCore.Chars(NewCharacterIndex).MinPhy = phyatkmin
                DatabaseCore.Chars(NewCharacterIndex).MaxPhy = phyatkmax
                DatabaseCore.Chars(NewCharacterIndex).MinMag = magatkmin
                DatabaseCore.Chars(NewCharacterIndex).MaxMag = magatkmax
                DatabaseCore.Chars(NewCharacterIndex).PhyDef = phydefmin
                DatabaseCore.Chars(NewCharacterIndex).MagDef = magdefmin
                DatabaseCore.Chars(NewCharacterIndex).Hit = 11
                DatabaseCore.Chars(NewCharacterIndex).Parry = parrymin


                GameServer.DataBase.InsertData("INSERT INTO characters (account, name, chartype, volume) VALUE ('" & ClientList.OnCharListing(Index_).LoginInformation.Id & "','" & nick & "','" & model & "','" & volume & "')")

                ' Masterys

                If model >= 1907 And model <= 1932 Then
                    'Chinese Char
                    DatabaseCore.MasteryCount += 7
                    Array.Resize(DatabaseCore.Masterys, DatabaseCore.MasteryCount)

                    Dim mastery As New cMastery
                    mastery.OwnerID = DatabaseCore.Chars(NewCharacterIndex).CharacterId

                    '257 - 259
                    mastery.MasteryID = 257
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 7) = mastery

                    mastery.MasteryID = 258
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 6) = mastery

                    mastery.MasteryID = 259
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 5) = mastery

                    '273 - 276
                    mastery.MasteryID = 273
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 4) = mastery

                    mastery.MasteryID = 274
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 3) = mastery

                    mastery.MasteryID = 275
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 2) = mastery

                    mastery.MasteryID = 276
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 1) = mastery

                    Dim b As Integer = DatabaseCore.MasteryCount
                    Dim lastid As Integer = DatabaseCore.Masterys(DatabaseCore.MasteryCount - 7).UniqueID

                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 1 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','257','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 2 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','258','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 3 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','273','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 4 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','274','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 5 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','275','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 6 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','276','0')")

                ElseIf model >= 14717 And model <= 14743 Then

                    'Europe Char
                    DatabaseCore.MasteryCount += 6
                    Array.Resize(DatabaseCore.Masterys, DatabaseCore.MasteryCount)

                    Dim mastery As New cMastery
                    mastery.OwnerID = DatabaseCore.Chars(NewCharacterIndex).CharacterId

                    '513 - 518
                    mastery.MasteryID = 513
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 6) = mastery

                    mastery.MasteryID = 514
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 5) = mastery

                    mastery.MasteryID = 515
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 4) = mastery

                    mastery.MasteryID = 516
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 3) = mastery

                    mastery.MasteryID = 517
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 2) = mastery

                    mastery.MasteryID = 518
                    DatabaseCore.Masterys(DatabaseCore.MasteryCount - 1) = mastery

                    Dim lastid As Integer = DatabaseCore.Masterys(DatabaseCore.MasteryCount - 7).UniqueID

                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 1 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','513','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 2 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','514','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 3 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','515','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 4 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','516','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 5 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','517','0')")
                    DataBase.InsertData("INSERT INTO masteries(id, owner, mastery, level) VALUE ('" & lastid + 6 & "','" & DatabaseCore.Chars(NewCharacterIndex).CharacterId & "','518','0')")


                End If


                'TODO: ITEMS




                'Finish

                writer.Byte(1) 'sucess
                Server.Send(writer.GetBytes, Index_)



            End If




        End Sub

        Public Sub CharLoading(ByVal Index_ As Integer, ByVal pack As PacketReader)

            Dim SelectedNick As String = pack.String(pack.Word)

            'Reply
            'Loading Start
            'Char Info Packet
            'Loading End

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.IngameReqRepley)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)

            writer = New PacketWriter
            writer.Create(ServerOpcodes.LoadingStart)
            Server.Send(writer.GetBytes, Index_)

            writer = New PacketWriter
            writer.Create(ServerOpcodes.CharacterInfo)

            If ClientList.OnCharListing(Index_).Chars(0).CharacterName = SelectedNick Then
                PlayerData(Index_) = ClientList.OnCharListing(Index_).Chars(0)

            ElseIf ClientList.OnCharListing(Index_).Chars(1).CharacterName = SelectedNick Then
                PlayerData(Index_) = ClientList.OnCharListing(Index_).Chars(1)

            ElseIf ClientList.OnCharListing(Index_).Chars(2).CharacterName = SelectedNick Then
                PlayerData(Index_) = ClientList.OnCharListing(Index_).Chars(2)

            ElseIf ClientList.OnCharListing(Index_).Chars(3).CharacterName = SelectedNick Then
                PlayerData(Index_) = ClientList.OnCharListing(Index_).Chars(3)

            End If



            writer = New PacketWriter
            writer.Create(ServerOpcodes.LoadingEnd)
            Server.Send(writer.GetBytes, Index_)




        End Sub

    End Module
End Namespace
