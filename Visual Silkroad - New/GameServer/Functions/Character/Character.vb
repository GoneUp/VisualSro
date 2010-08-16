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
                    writer.QWord(ClientList.OnCharListing(Index_).Chars(i).Experience)
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

            If free = True Or nick.StartsWith("[GM]") = True Then
                writer.Byte(1)
            Else
                writer.Byte(2)
                writer.Byte(13)
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

            'Check it

            If model >= 1907 And model <= 1932 = False And model >= 14717 And model <= 14743 = False Then
                'Wrong Model Code! 
                GameServer.Server.Dissconnect(Index_)
                Console.WriteLine(String.Format("[Character Creation][Wrong Model: {0}][Index: {1}]", model, Index_))
            End If

            Dim _refitems(4) As cItem
            _refitems(1) = GetItemByID(_items(1))
            _refitems(2) = GetItemByID(_items(2))
            _refitems(3) = GetItemByID(_items(3))
            _refitems(4) = GetItemByID(_items(4))

            For i = 1 To 4
                If _refitems(i).ITEM_TYPE_NAME.EndsWith("_DEF") = False Then
                    GameServer.Server.Dissconnect(Index_)
                    Console.WriteLine(String.Format("[Character Creation][Wrong Item: {0}][Index: {1}]", _refitems(i).ITEM_TYPE_NAME, Index_))
                End If
                Debug.Print("[Character Creation][" & i & "][ID:" & _items(i) & "]")
            Next

            'Creation
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Character)
            writer.Byte(1) 'create

            Dim free As Boolean = GameServer.DatabaseCore.CheckNick(nick)
            If free = False Or nick.StartsWith("[GM]") = True Then
                writer.Byte(4)
                writer.Byte(13)
                GameServer.Server.Send(writer.GetBytes, Index_)

            Else

                GameServer.DatabaseCore.CharCount += 1
                Array.Resize(GameServer.DatabaseCore.Chars, GameServer.DatabaseCore.CharCount)

                Dim newchar As New [cChar]
                Dim NewCharacterIndex As Integer = GameServer.DatabaseCore.CharCount - 1

                DatabaseCore.Chars(NewCharacterIndex) = New [cChar]
                DatabaseCore.Chars(NewCharacterIndex).AccountID = ClientList.OnCharListing(Index_).LoginInformation.Id
                DatabaseCore.Chars(NewCharacterIndex).CharacterName = nick
                DatabaseCore.Chars(NewCharacterIndex).CharacterId = DatabaseCore.UniqueIdCounter
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
                Dim hit As UShort = 11

                DatabaseCore.Chars(NewCharacterIndex).MinPhy = phyatkmin
                DatabaseCore.Chars(NewCharacterIndex).MaxPhy = phyatkmax
                DatabaseCore.Chars(NewCharacterIndex).MinMag = magatkmin
                DatabaseCore.Chars(NewCharacterIndex).MaxMag = magatkmax
                DatabaseCore.Chars(NewCharacterIndex).PhyDef = phydefmin
                DatabaseCore.Chars(NewCharacterIndex).MagDef = magdefmin
                DatabaseCore.Chars(NewCharacterIndex).Hit = hit
                DatabaseCore.Chars(NewCharacterIndex).Parry = parrymin


                GameServer.DataBase.SaveQuery("INSERT INTO characters (account, name, chartype, volume) VALUE ('" & ClientList.OnCharListing(Index_).LoginInformation.Id & "','" & nick & "','" & model & "','" & volume & "')")

                ' Masterys

                If model >= 1907 And model <= 1932 Then
                    'Chinese Char
                    Dim mastery As New cMastery
                    mastery.OwnerID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = 1

                    '257 - 259
                    mastery.MasteryID = 257
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 258
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 259
                    AddMasteryToDB(mastery)

                    '273 - 276
                    mastery.MasteryID = 273
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 274
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 275
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 276
                    AddMasteryToDB(mastery)


                ElseIf model >= 14717 And model <= 14743 Then

                    'Europe Char
                    '513 - 518

                    Dim mastery As New cMastery
                    mastery.OwnerID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = 1
                    mastery.MasteryID = 513
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 514
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 515
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 516
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 517
                    AddMasteryToDB(mastery)

                    mastery.MasteryID = 518
                    AddMasteryToDB(mastery)
                End If


                'ITEMS

                '1 =  Body
                '2 = Legs
                '3 = foot
                '4 = Waffe

                Dim item As New cInvItem
                item.OwnerCharID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                item.Pk2Id = _items(1)
                item.Plus = Math.Round(Rnd() * 3, 0)
                item.Amount = 1
                item.Slot = 1
                AddItemToDB(item)

                item = New cInvItem
                item.OwnerCharID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                item.Pk2Id = _items(2)
                item.Plus = Math.Round(Rnd() * 3, 0)
                item.Amount = 1
                item.Slot = 4
                AddItemToDB(item)

                item = New cInvItem
                item.OwnerCharID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                item.Pk2Id = _items(3)
                item.Plus = Math.Round(Rnd() * 3, 0)
                item.Amount = 1
                item.Slot = 5
                AddItemToDB(item)

                item = New cInvItem
                item.OwnerCharID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                item.Pk2Id = _items(4)
                item.Plus = Math.Round(Rnd() * 5, 0)
                item.Amount = 1
                item.Slot = 6
                AddItemToDB(item)

                If _items(4) = 3632 Or _items(4) = 3633 Then 'Sword or Blade need a Shield
                    item = New cInvItem
                    item.OwnerCharID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                    item.Pk2Id = 251
                    item.Plus = Math.Round(Rnd() * 5, 0)
                    item.Amount = 1
                    item.Slot = 7
                    AddItemToDB(item)

                ElseIf _items(4) = 3636 Then 'Bow --> Give some Arrows
                    item = New cInvItem
                    item.OwnerCharID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                    item.Pk2Id = 62
                    item.Amount = 123
                    item.Slot = 7
                    AddItemToDB(item)

                ElseIf _items(4) = 10730 Or _items(4) = 10734 Or _items(4) = 10737 Then 'EU Weapons who need a staff
                    item = New cInvItem
                    item.OwnerCharID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                    item.Pk2Id = 10738
                    item.Plus = Math.Round(Rnd() * 5, 0)
                    item.Amount = 1
                    item.Slot = 7
                    AddItemToDB(item)

                ElseIf _items(4) = -1 Then 'Bow --> Give some Arrows
                    item = New cInvItem
                    item.OwnerCharID = DatabaseCore.Chars(NewCharacterIndex).CharacterId
                    item.Pk2Id = 62
                    item.Amount = 123
                    item.Slot = 7
                    AddItemToDB(item)
                End If

                'Finish

                writer.Byte(1) 'sucess
                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub
        Public Sub AddItemToDB(ByVal item As cInvItem)

            Dim NewIndex As UInteger = DatabaseCore.ItemCount + 1
            DatabaseCore.ItemCount = NewIndex
            Dim i = DatabaseCore.AllItems.Length
            Array.Resize(DatabaseCore.AllItems, NewIndex)
            Dim i2 = DatabaseCore.AllItems.Length

            DatabaseCore.AllItems(NewIndex - 1) = item

            DataBase.SaveQuery(String.Format("INSERT INTO items(itemtype, owner, plusvalue, slot) VALUE ('{0}','{1}','{2}','{3}')", item.Pk2Id, item.OwnerCharID, item.Plus, item.Slot))
        End Sub

        Public Sub AddMasteryToDB(ByVal toadd As cMastery)

            Dim NewIndex As UInteger = DatabaseCore.MasteryCount + 1
            DatabaseCore.MasteryCount = NewIndex
            Dim i = DatabaseCore.Masterys.Length
            Array.Resize(DatabaseCore.Masterys, NewIndex)
            Dim i2 = DatabaseCore.Masterys.Length

            DatabaseCore.Masterys(NewIndex - 1) = toadd

            DataBase.SaveQuery(String.Format("INSERT INTO masteries(owner, mastery, level) VALUE ('{0}',{1},{2})", toadd.OwnerID, toadd.MasteryID, toadd.Level))
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

            'Main
            If ClientList.OnCharListing(Index_).Chars(0).CharacterName = SelectedNick Then
                PlayerData(Index_) = ClientList.OnCharListing(Index_).Chars(0)

            ElseIf ClientList.OnCharListing(Index_).Chars(1).CharacterName = SelectedNick Then
                PlayerData(Index_) = ClientList.OnCharListing(Index_).Chars(1)

            ElseIf ClientList.OnCharListing(Index_).Chars(2).CharacterName = SelectedNick Then
                PlayerData(Index_) = ClientList.OnCharListing(Index_).Chars(2)

            ElseIf ClientList.OnCharListing(Index_).Chars(3).CharacterName = SelectedNick Then
                PlayerData(Index_) = ClientList.OnCharListing(Index_).Chars(3)

            End If


            Dim chari As [cChar] = PlayerData(Index_)
            writer = New PacketWriter
            writer.Create(ServerOpcodes.CharacterInfo)
            writer.DWord(2289569290)  '@@@@@@@@@@@@@@@@@
            writer.DWord(chari.Model)  ' Character Model
            writer.Byte(chari.Volume)  ' Volume & Height
            writer.Byte(chari.Level)  ' Level
            writer.Byte(chari.Level)  ' Highest Level

            writer.QWord(chari.Experience)  ' EXP Bar
            writer.DWord(chari.SkillPointBar)  ' SP Bar
            writer.QWord(chari.Gold)  ' Gold Amount
            writer.DWord(chari.SkillPoints)  ' SP Amount
            writer.Word(chari.Attributes)  ' Stat Points
            writer.Byte(chari.BerserkBar)  ' Berserk Bar
            writer.DWord(0)  ' @@@@@@@@@@@@@
            writer.DWord(chari.CHP)  ' HP
            writer.DWord(chari.CMP)  ' MP
            writer.Byte(1)  ' Icon
            writer.Byte(0)  ' Daily PK (/15)
            writer.Word(0)  ' Total PK
            writer.DWord(0)  ' PK Penalty Point
            writer.Byte(0)  ' Rank
            '''''''''''''''''''/


            writer.Byte(chari.MaxSlots)  ' Max Item Slot (0 Minimum + 13) (96 Maximum + 13)
            writer.Byte(0)  ' Amount of Items  


            writer.Byte(4)  ' Avatar Item Max
            writer.Byte(0)  ' Amount of Avatars

            writer.Byte(0)  ' Duplicate List (00 - None) (01 - Duplicate)

            writer.Byte(1) 'mastery start
            writer.DWord(257) ' Mastery
            writer.Byte(140) ' Mastery Level
            writer.Byte(1) ' Mastery Start
            writer.DWord(258) ' Mastery
            writer.Byte(140) ' Mastery Level
            writer.Byte(1) ' Mastery Start
            writer.DWord(259) ' Mastery
            writer.Byte(140) ' Mastery Level
            writer.Byte(1) ' Mastery Start
            writer.DWord(273) ' Mastery
            writer.Byte(140) ' Mastery Level
            writer.Byte(1) ' Mastery Start
            writer.DWord(274) ' Mastery
            writer.Byte(140) ' Mastery Level
            writer.Byte(1) ' Mastery Start
            writer.DWord(275) ' Mastery
            writer.Byte(140) ' Mastery Level
            writer.Byte(1) ' Mastery Start
            writer.DWord(276) ' Mastery
            writer.Byte(140) ' Mastery Level

            writer.Byte(2) 'mastery end
            writer.Byte(0) 'mastery end

            writer.Byte(1)
            writer.DWord(3)
            writer.Byte(1)
            writer.Byte(2)


            writer.Word(1)  ' Amount of Completed Quests
            writer.DWord(1) 'event

            writer.Word(0)  ' Amount of Pending Quests


            '''''''''''''''''''''/
            ' ID, Position, State, Speed

            writer.DWord(chari.UniqueId)  ' Unique ID
            writer.Byte(chari.XSector)  ' X Sector
            writer.Byte(chari.YSector)  ' Y Sector
            writer.Float(chari.X)  ' X
            writer.Float(chari.Z)  ' Z
            writer.Float(chari.Y)  ' Y
            writer.Word(0)  ' Angle
            writer.Byte(0)  ' Destination
            writer.Byte(1)  ' Walk & Run Flag
            writer.Byte(0)  ' No Destination
            writer.Word(0)  ' Angle
            writer.Byte(0)  ' Death Flag
            writer.Byte(0)  ' Movement Flag
            writer.Byte(chari.Berserk)  ' Berserker Flag
            writer.Float(chari.WalkSpeed)  ' Walking Speed
            writer.Float(chari.RunSpeed)  ' Running Speed
            writer.Float(chari.BerserkSpeed)  ' Berserk Speed
            '''''''''''''''''''''/


            writer.Byte(0)  ' Buff Flag

            '''''''''''''''''''''/
            ' Name, Job, PK
            writer.Word(chari.CharacterName.Length)  ' Player Name Length
            writer.String(chari.CharacterName)  ' Player Name
            writer.Word(0)  ' Alias Name Length
            'writer.String("")  ' Alias Name
            writer.Byte(0)  ' Job Level
            writer.Byte(1)  ' Job Type
            writer.DWord(0)  ' @@@@@@@@@@@@@ ' TRADER || CURRENT EXP
            writer.DWord(0)  ' @@@@@@@@@@@@@ ' THIEF  ||
            writer.DWord(0)  ' @@@@@@@@@@@@@ ' HUNTER ||
            writer.Byte(0)  ' @@@@@@@@@@@@@ ' TRADER LEVEL??
            writer.Byte(0)  ' @@@@@@@@@@@@@ ' THIEF LEVEL ??   ALL THESE RELATED TO OLD JOB SYSTEM?
            writer.Byte(0)  ' @@@@@@@@@@@@@ ' HUNTER LEVEL ??
            writer.Byte(&HFF)  ' PK Flag
            '''''''''''''''''''''/


            '''''''''''''''''''''/  
            ' Account

            writer.Word(1)  ' @@@@@@@@@@@@@
            writer.DWord(0)  ' @@@@@@@@@@@@@
            writer.Word(0)  ' @@@@@@@@@@@@@
            writer.DWord(chari.AccountID)  ' Account ID
            writer.Byte(0)  'GM Flag
            writer.Byte(7)  ' @@@@@@@@@@@@@
            '''''''''''''''''''''/


            writer.Byte(0)  ' Number of Hotkeys


            ' Autopotion
            writer.Byte(0)  ' HP Slot
            writer.Byte(0)  ' HP Value
            writer.Byte(0)  ' MP Slot
            writer.Byte(0)  ' MP Value
            writer.Byte(0)  ' Abnormal Slot
            writer.Byte(0)  ' Abnormal Value
            writer.Byte(0)  ' Potion Delay


            writer.Byte(0)  ' Amount of Players Blocked


            ' Other
            writer.Word(1)  'unknown
            writer.Word(1)
            writer.Byte(0)
            writer.Byte(1)


            Server.Send(writer.GetBytes, Index_)



            writer = New PacketWriter
            writer.Create(ServerOpcodes.LoadingEnd)
            Server.Send(writer.GetBytes, Index_)


            writer = New PacketWriter
            writer.Create(ServerOpcodes.CharacterID)
            writer.DWord(chari.UniqueId) 'charid
            writer.Word(13) 'moon pos
            writer.Byte(9) 'hours
            writer.Byte(28) 'minute
            Server.Send(writer.GetBytes, Index_)

            StatsPacket(Index_)

        End Sub

        Public Sub StatsPacket(ByVal index_ As Integer)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.CharacterStats)
            writer.DWord(PlayerData(index_).MinPhy)
            writer.DWord(PlayerData(index_).MaxPhy)
            writer.DWord(PlayerData(index_).MinMag)
            writer.DWord(PlayerData(index_).MaxMag)
            writer.Word(PlayerData(index_).PhyDef)
            writer.Word(PlayerData(index_).MagDef)
            writer.Word(PlayerData(index_).Hit)
            writer.Word(PlayerData(index_).Parry)
            writer.DWord(PlayerData(index_).HP)
            writer.DWord(PlayerData(index_).MP)
            writer.Word(PlayerData(index_).Strength)
            writer.Word(PlayerData(index_).Intelligence)
            Server.Send(writer.GetBytes, index_)

        End Sub
    End Module
End Namespace
