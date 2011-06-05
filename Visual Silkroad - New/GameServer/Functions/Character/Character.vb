Namespace GameServer.Functions
    Module Character

        Public Sub HandleCharPacket(ByVal Index_ As Integer, ByVal pack As GameServer.PacketReader)

            Dim tag As Byte = pack.Byte

            Select Case tag
                Case 1 'Crweate
                    OnCreateChar(pack, Index_)
                Case 2 'Char List
                    OnCharList(Index_)
                Case 3 'Char delete
                    OnDeleteChar(pack, Index_)
                Case 4 'Nick Check
                    OnCheckNick(pack, Index_)
                Case 5 'Restore
                    OnRestoreChar(pack, Index_)
            End Select
        End Sub

        Public Sub OnCharList(ByVal Index_ As Integer)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Character)

            ClientList.CharListing(Index_) = GameServer.GameDB.FillCharList(ClientList.CharListing(Index_))

            writer.Byte(2) 'Char List
            writer.Byte(1)

            writer.Byte(ClientList.CharListing(Index_).NumberOfChars)

            If ClientList.CharListing(Index_).NumberOfChars = 0 Then
                GameServer.Server.Send(writer.GetBytes, Index_)

            ElseIf ClientList.CharListing(Index_).NumberOfChars > 0 Then
                For i = 0 To (ClientList.CharListing(Index_).NumberOfChars - 1)

                    writer.DWord(ClientList.CharListing(Index_).Chars(i).Model)
                    writer.Word(ClientList.CharListing(Index_).Chars(i).CharacterName.Length)
                    writer.String(ClientList.CharListing(Index_).Chars(i).CharacterName)
                    writer.Byte(ClientList.CharListing(Index_).Chars(i).Volume)
                    writer.Byte(ClientList.CharListing(Index_).Chars(i).Level)
                    writer.QWord(ClientList.CharListing(Index_).Chars(i).Experience)
                    writer.Word(ClientList.CharListing(Index_).Chars(i).Strength)
                    writer.Word(ClientList.CharListing(Index_).Chars(i).Intelligence)
                    writer.Word(ClientList.CharListing(Index_).Chars(i).Attributes)
                    writer.DWord(ClientList.CharListing(Index_).Chars(i).CHP)
                    writer.DWord(ClientList.CharListing(Index_).Chars(i).CMP)
                    If ClientList.CharListing(Index_).Chars(i).Deleted = True Then
                        Dim diff As Long = DateDiff(DateInterval.Minute, DateTime.Now, ClientList.CharListing(Index_).Chars(i).DeletionTime)
                        writer.Byte(1) 'to delete
                        writer.DWord(diff)
                    Else
                        writer.Byte(0)
                    End If

                    writer.Word(0) 'Job Alias
                    writer.Byte(0) 'In Academy

                    'Now Items
                    Dim inventory As New cInventory(ClientList.CharListing(Index_).Chars(i).MaxSlots)
                    inventory = GameDB.FillInventory(ClientList.CharListing(Index_).Chars(i))

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
                Server.Send(writer.GetBytes, Index_)

            End If
        End Sub

        Public Sub OnCheckNick(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nick As String = packet.String(packet.Word)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Character)
            writer.Byte(4) 'nick check

            If GameDB.CheckNick(nick) And CheckForAbuse(nick) = False Then
                writer.Byte(1)
            Else
                writer.Byte(2)
                writer.Byte(&H10)
                writer.Byte(4)
                '13 invalid 10 server error
            End If

            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Function CheckForAbuse(ByVal nick As String) As Boolean
            Dim tmp As String = nick.ToLowerInvariant
            For i = 0 To RefAbuseList.Count - 1
                If tmp.Contains(RefAbuseList(i)) = True Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Sub OnDeleteChar(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nick As String = packet.String(packet.Word)
            For i = 0 To ClientList.CharListing(Index_).NumberOfChars - 1
                If ClientList.CharListing(Index_).Chars(i).CharacterName = nick Then
                    ClientList.CharListing(Index_).Chars(i).Deleted = True
                    Dim dat As DateTime = DateTime.Now
                    Dim dat1 = dat.AddDays(7)
                    ClientList.CharListing(Index_).Chars(i).DeletionTime = dat1
                    DataBase.SaveQuery(String.Format("UPDATE characters SET deletion_mark='1', deletion_time='{0}' where id='{1}'", dat1.ToString, ClientList.CharListing(Index_).Chars(i).CharacterId))

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Character)
                    writer.Byte(3) 'type = delte
                    writer.Byte(1) 'success
                    Server.Send(writer.GetBytes, Index_)
                End If
            Next
        End Sub


        Public Sub OnRestoreChar(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nick As String = packet.String(packet.Word)
            For i = 0 To ClientList.CharListing(Index_).NumberOfChars - 1
                If ClientList.CharListing(Index_).Chars(i).CharacterName = nick Then
                    ClientList.CharListing(Index_).Chars(i).Deleted = False
                    DataBase.SaveQuery(String.Format("UPDATE characters SET deletion_mark='0' where id='{0}'", ClientList.CharListing(Index_).Chars(i).CharacterId))

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Character)
                    writer.Byte(5) 'type = restore
                    writer.Byte(1) 'success
                    Server.Send(writer.GetBytes, Index_)
                End If
            Next
        End Sub

        Public Sub OnCreateChar(ByVal pack As PacketReader, ByVal Index_ As Integer)
            Dim nick As String = pack.String(pack.Word)
            Dim model As UInt32 = pack.DWord
            Dim volume As Byte = pack.Byte
            Dim _items(4) As UInt32
            _items(1) = pack.DWord
            _items(2) = pack.DWord
            _items(3) = pack.DWord
            _items(4) = pack.DWord

            'Check it

            If IsCharChinese(model) = False And IsCharEurope(model) = False Then
                'Wrong Model Code! 
                Server.Dissconnect(Index_)
                Log.WriteSystemLog(String.Format("[Character Creation][Wrong Model: {0}][Index: {1}]", model, Index_))
            End If

            Dim _refitems(4) As cItem
            _refitems(1) = GetItemByID(_items(1))
            _refitems(2) = GetItemByID(_items(2))
            _refitems(3) = GetItemByID(_items(3))
            _refitems(4) = GetItemByID(_items(4))

            For i = 1 To 4
                If _refitems(i).ITEM_TYPE_NAME.EndsWith("_DEF") = False Then
                    Server.Dissconnect(Index_)
                    Log.WriteSystemLog(String.Format("[Character Creation][Wrong Item: {0}][Index: {1}]", _refitems(i).ITEM_TYPE_NAME, Index_))
                End If
                Debug.Print("[Character Creation][" & i & "][ID:" & _items(i) & "]")
            Next

            'Creation
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Character)
            writer.Byte(1) 'create
        

            If GameDB.CheckNick(nick) And CheckForAbuse(nick) = True Then
                writer.Byte(2)
                writer.Byte(&H10)
                writer.Byte(4)
                Server.Send(writer.GetBytes, Index_)
            Else

                Array.Resize(GameDB.Chars, GameDB.Chars.Count + 1)

                Dim newchar As New [cChar]
                Dim NewCharacterIndex As Integer = GameDB.Chars.Count - 1

                GameDB.Chars(NewCharacterIndex) = New [cChar]

                With GameDB.Chars(NewCharacterIndex)
                    .AccountID = ClientList.CharListing(Index_).LoginInformation.Id
                    .CharacterName = nick
                    .CharacterId = Id_Gen.GetNewCharId
                    .UniqueId = Id_Gen.GetUnqiueId
                    .HP = 200
                    .MP = 200
                    .CHP = 200
                    .CMP = 200
                    .Model = model
                    .Volume = volume
                    .Level = Settings.Player_StartLevel
                    .Gold = Settings.Player_StartGold
                    .SkillPoints = Settings.Player_StartSP
                    .GM = Settings.Player_StartGM

                    .WalkSpeed = 16
                    .RunSpeed = 50
                    .BerserkSpeed = 100
                    .Strength = 20
                    .Intelligence = 20
                    .PVP = 0
                    .MaxSlots = 45
                    .Position_Dead = Settings.Player_StartReturnPos
                    .Position_Recall = Settings.Player_StartReturnPos
                    .Position_Return = Settings.Player_StartReturnPos
                    If IsCharChinese(model) Then
                        .Pos_Tracker = New cPositionTracker(Settings.Player_StartPos_Ch, .WalkSpeed, .RunSpeed, .BerserkSpeed)
                    ElseIf IsCharEurope(model) Then
                        .Pos_Tracker = New cPositionTracker(Settings.Player_StartPos_Eu, .WalkSpeed, .RunSpeed, .BerserkSpeed)
                    End If
                    Dim magdefmin As Double = 3.0
                    Dim phydefmin As Double = 6.0
                    Dim phyatkmin As UShort = 6
                    Dim phyatkmax As UShort = 9
                    Dim magatkmin As UShort = 6
                    Dim magatkmax As UShort = 9
                    Dim hit As UShort = 11
                    Dim parry As UShort = 11

                    .MinPhy = phyatkmin
                    .MaxPhy = phyatkmax
                    .MinMag = magatkmin
                    .MaxMag = magatkmax
                    .PhyDef = phydefmin
                    .MagDef = magdefmin
                    .Hit = hit
                    .Parry = parry

                    .SetCharGroundStats()
                End With

                DataBase.SaveQuery(String.Format("INSERT INTO characters (id, account, name, chartype, volume, level, gold, sp, gm) VALUE ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", GameDB.Chars(NewCharacterIndex).CharacterId, GameDB.Chars(NewCharacterIndex).AccountID, GameDB.Chars(NewCharacterIndex).CharacterName, GameDB.Chars(NewCharacterIndex).Model, GameDB.Chars(NewCharacterIndex).Volume, GameDB.Chars(NewCharacterIndex).Level, GameDB.Chars(NewCharacterIndex).Gold, GameDB.Chars(NewCharacterIndex).SkillPoints, CInt(GameDB.Chars(NewCharacterIndex).GM)))
                DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", GameDB.Chars(NewCharacterIndex).Position.XSector, GameDB.Chars(NewCharacterIndex).Position.YSector, Math.Round(GameDB.Chars(NewCharacterIndex).Position.X), Math.Round(GameDB.Chars(NewCharacterIndex).Position.Z), Math.Round(GameDB.Chars(NewCharacterIndex).Position.Y), GameDB.Chars(NewCharacterIndex).CharacterId))
                DataBase.SaveQuery(String.Format("INSERT INTO positions (OwnerCharID) VALUE ('{0}')", GameDB.Chars(NewCharacterIndex).CharacterId))
                DataBase.SaveQuery(String.Format("INSERT INTO guild_member (charid) VALUE ('{0}')", GameDB.Chars(NewCharacterIndex).CharacterId))

                ' Masterys

                If IsCharChinese(model) Then
                    'Chinese Char
                    '257 - 259

                    Dim mastery As New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 257
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 258
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 259
                    AddMasteryToDB(mastery)

                    '273 - 276
                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 273
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 274
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 275
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 276
                    AddMasteryToDB(mastery)


                ElseIf IsCharEurope(model) Then

                    'Europe Char
                    '513 - 518
                    Dim mastery As New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 513
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 514
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 515
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 516
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 517
                    AddMasteryToDB(mastery)

                    mastery = New cMastery
                    mastery.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    mastery.Level = Settings.Player_StartMasteryLevel
                    mastery.MasteryID = 518
                    AddMasteryToDB(mastery)
                End If


                'ITEMS
                'Inventory
                For I = 0 To 109
                    Dim to_add As New cInvItem
                    to_add.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                    to_add.Pk2Id = 0
                    to_add.Plus = 0
                    to_add.Amount = 0
                    to_add.Slot = I
                    to_add.ItemType = cInvItem.sUserItemType.Inventory
                    AddItemToDB(to_add)
                Next
                'Avatar
                For I = 0 To 5
                    Dim to_add As New cInvItem
                    to_add.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                    to_add.Pk2Id = 0
                    to_add.Plus = 0
                    to_add.Amount = 0
                    to_add.Slot = I
                    to_add.ItemType = cInvItem.sUserItemType.Avatar
                    AddItemToDB(to_add)
                Next


                '1 =  Body
                '2 = Legs
                '3 = foot
                '4 = Waffe
                Dim item As New cInvItem
                item.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                item.Pk2Id = _items(1)
                item.Plus = 0 'Math.Round(Rnd() * 3, 0)
                item.Amount = 0
                item.Slot = 1
                UpdateItem(item)

                item = New cInvItem
                item.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                item.Pk2Id = _items(2)
                item.Plus = 0 'Math.Round(Rnd() * 3, 0)
                item.Amount = 0
                item.Slot = 4
                UpdateItem(item)

                item = New cInvItem
                item.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                item.Pk2Id = _items(3)
                item.Plus = 0 'Math.Round(Rnd() * 3, 0)
                item.Amount = 0
                item.Slot = 5
                UpdateItem(item)

                item = New cInvItem
                item.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                item.Pk2Id = _items(4)
                item.Plus = 0 'Math.Round(Rnd() * 5, 0)
                item.Amount = 0
                item.Slot = 6
                UpdateItem(item)

                If _items(4) = 3632 Or _items(4) = 3633 Then 'Sword or Blade need a Shield
                    item = New cInvItem
                    item.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                    item.Pk2Id = 251
                    item.Plus = 0 'Math.Round(Rnd() * 9, 0)
                    item.Amount = 0
                    item.Slot = 7
                    UpdateItem(item)


                ElseIf _items(4) = 3636 Then 'Bow --> Give some Arrows
                    item = New cInvItem
                    item.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                    item.Pk2Id = 62
                    item.Amount = 100
                    item.Slot = 7
                    UpdateItem(item)

                ElseIf _items(4) = 10730 Or _items(4) = 10734 Or _items(4) = 10737 Then 'EU Weapons who need a shield
                    item = New cInvItem
                    item.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                    item.Pk2Id = 10738
                    item.Plus = 0 'Math.Round(Rnd() * 9, 0)
                    item.Amount = 0
                    item.Slot = 7
                    UpdateItem(item)

                ElseIf _items(4) = -1 Then 'Armbrust --> Bolt
                    item = New cInvItem
                    item.OwnerCharID = GameDB.Chars(NewCharacterIndex).CharacterId
                    item.Pk2Id = 62
                    item.Amount = 123
                    item.Slot = 7
                    UpdateItem(item)
                End If

                'Hotkeys
                For i = 0 To 50
                    Dim toadd As New cHotKey
                    toadd.OwnerID = GameDB.Chars(NewCharacterIndex).CharacterId
                    toadd.Slot = i
                    AddHotKeyToDB(toadd)
                Next

                'Mods
                [Mod].Damage.OnPlayerCreate(GameDB.Chars(NewCharacterIndex).CharacterId, Index_)


                'Finish
                writer.Byte(1) 'success
                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub

        Public Sub AddMasteryToDB(ByVal toadd As cMastery)
            Array.Resize(GameDB.Masterys, GameDB.Masterys.Length + 1)
            GameDB.Masterys(GameDB.Masterys.Length - 1) = toadd

            DataBase.SaveQuery(String.Format("INSERT INTO masteries(owner, mastery, level) VALUE ('{0}','{1}','{2}')", toadd.OwnerID, toadd.MasteryID, toadd.Level))
        End Sub

        Public Sub AddHotKeyToDB(ByVal toadd As cHotKey)
            GameDB.Hotkeys.Add(toadd)
            DataBase.SaveQuery(String.Format("INSERT INTO hotkeys(OwnerID, slot) VALUE ('{0}','{1}')", toadd.OwnerID, toadd.Slot))
        End Sub
        Public Sub CharLoading(ByVal Index_ As Integer, ByVal pack As PacketReader)
            Dim SelectedNick As String = pack.String(pack.Word)
            Dim writer As New PacketWriter

            writer.Create(ServerOpcodes.IngameReqRepley)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)

            'Main
            For i = 0 To ClientList.CharListing(Index_).Chars.Count - 1
                If ClientList.CharListing(Index_).Chars(i).CharacterName = SelectedNick Then
                    PlayerData(Index_) = ClientList.CharListing(Index_).Chars(i)
                    Dim inventory As New cInventory(ClientList.CharListing(Index_).Chars(i).MaxSlots)
                    Inventorys(Index_) = GameServer.GameDB.FillInventory(ClientList.CharListing(Index_).Chars(i))
                End If
            Next

            'Prepare
            CleanUpPlayer(Index_)
            Player_CheckDeath(Index_, True)
            [Mod].Damage.OnPlayerLogon(Index_)

            'Packet's
            writer = New PacketWriter
            writer.Create(ServerOpcodes.LoadingStart)
            Server.Send(writer.GetBytes, Index_)

            OnCharacterInfo(Index_)

            writer = New PacketWriter
            writer.Create(ServerOpcodes.LoadingEnd)
            Server.Send(writer.GetBytes, Index_)


            writer = New PacketWriter
            writer.Create(ServerOpcodes.CharacterID)
            writer.DWord(PlayerData(Index_).UniqueId) 'charid
            writer.Word(Date.Now.Day) 'moon pos
            writer.Byte(Date.Now.Hour) 'hours
            writer.Byte(Date.Now.Minute) 'minute
            Server.Send(writer.GetBytes, Index_)
        End Sub
        Public Sub OnCharacterInfo(ByVal Index_ As Integer)
            PlayerData(Index_).SetCharGroundStats()
            PlayerData(Index_).AddItemsToStats(Index_)

            Dim writer As New PacketWriter
            Dim chari As [cChar] = PlayerData(Index_)
            writer = New PacketWriter
            writer.Create(ServerOpcodes.CharacterInfo)
            writer.DWord(1273064057)  '@@@@@@@@@@@@@@@@@
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
            writer.Byte(chari.HelperIcon)   ' Icon
            writer.Byte(0)  ' Daily PK (/15)
            writer.Word(0)  ' Total PK
            writer.DWord(0)  ' PK Penalty Point
            writer.Byte(0)  ' Rank
            '''''''''''''''''''/

            'INVENTORY HERE

            Inventorys(Index_).CalculateItemCount()
            writer.Byte(chari.MaxSlots)  ' Max Item Slot (0 Minimum + 13) (4 Seiten x 24 Slots = 96 Maximum + 13 --> 109)
            writer.Byte(Inventorys(Index_).ItemCount)  ' Amount of Items  

            For Each _item As cInvItem In Inventorys(Index_).UserItems
                If _item.Pk2Id <> 0 Then
                    writer.Byte(_item.Slot)

                    AddItemDataToPacket(_item, writer)
                End If
            Next

            Inventorys(Index_).CalculateAvatarCount()
            writer.Byte(5)  ' Avatar Item Max
            writer.Byte(Inventorys(Index_).AvatarCount)  ' Amount of Avatars
            For Each _avatar As cInvItem In Inventorys(Index_).AvatarItems
                If _avatar.Pk2Id <> 0 Then
                    writer.DWord(_avatar.Pk2Id)
                    writer.Byte(_avatar.Plus)
                End If
            Next

            writer.Byte(0)  ' Duplicate List (00 - None) (01 - Duplicate)

            For i = 0 To GameDB.Masterys.Length - 1
                If (GameDB.Masterys(i) IsNot Nothing) AndAlso GameDB.Masterys(i).OwnerID = chari.CharacterId Then
                    writer.Byte(1) 'mastery start
                    writer.DWord(GameDB.Masterys(i).MasteryID) ' Mastery
                    writer.Byte(GameDB.Masterys(i).Level) ' Mastery Level
                End If
            Next

            writer.Byte(2) 'mastery end
            writer.Byte(0) 'mastery end

            For i = 0 To GameDB.Skills.Length - 1
                If (GameDB.Skills(i) IsNot Nothing) AndAlso GameDB.Skills(i).OwnerID = chari.CharacterId Then
                    writer.Byte(1) 'skill start
                    writer.DWord(GameDB.Skills(i).SkillID) 'skill id
                    writer.Byte(1) ' skill end?
                End If
            Next

            writer.Byte(2) 'end


            writer.Word(0)  ' Amount of Completed Quests
            'writer.DWord(1) 'event

            writer.Word(0)  ' Amount of Pending Quests


            '''''''''''''''''''''/
            ' ID, Position, State, Speed

            writer.DWord(chari.UniqueId)  ' Unique ID
            writer.Byte(chari.Position.XSector)  ' X Sector
            writer.Byte(chari.Position.YSector)  ' Y Sector
            writer.Float(chari.Position.X)  ' X
            writer.Float(chari.Position.Z)  ' Z
            writer.Float(chari.Position.Y)  ' Y
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

            writer.DWord(1)  ' @@@@@@@@@@@@@
            writer.DWord(0)  ' @@@@@@@@@@@@@
            writer.DWord(chari.AccountID)  ' Account ID
            writer.Byte(chari.GM)
            writer.Byte(7)  ' @@@@@@@@@@@@@
            '''''''''''''''''''''/

            Dim hotkeycount As UInteger = 0
            For i = 0 To GameDB.Hotkeys.Count - 1
                If GameDB.Hotkeys(i).OwnerID = chari.CharacterId Then
                    If GameDB.Hotkeys(i).Type <> 0 And GameDB.Hotkeys(i).IconID <> 0 Then
                        hotkeycount += 1
                    End If
                End If
            Next

            writer.Byte(hotkeycount)  ' Number of Hotkeys

            For i = 0 To GameDB.Hotkeys.Count - 1
                If GameDB.Hotkeys(i).OwnerID = chari.CharacterId Then
                    If GameDB.Hotkeys(i).Type <> 0 And GameDB.Hotkeys(i).IconID <> 0 Then
                        writer.Byte(GameDB.Hotkeys(i).Slot)
                        writer.Byte(GameDB.Hotkeys(i).Type)
                        writer.DWord(GameDB.Hotkeys(i).IconID)
                    End If
                End If
            Next


            ' Autopotion
            writer.Byte(chari.Pot_HP_Slot)  ' HP Slot
            writer.Byte(chari.Pot_HP_Value)  ' HP Value
            writer.Byte(chari.Pot_MP_Slot)  ' MP Slot
            writer.Byte(chari.Pot_MP_Value)  ' MP Value
            writer.Byte(chari.Pot_Abormal_Slot)  ' Abnormal Slot
            writer.Byte(chari.Pot_Abormal_Value)  ' Abnormal Value
            writer.Byte(chari.Pot_Delay)  ' Potion Delay


            writer.Byte(0)  ' Amount of Players Blocked


            ' Other
            writer.Word(1)  'unknown
            writer.Word(1)
            writer.Byte(0)
            writer.Byte(0)


            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnJoinWorldRequest(ByVal Index_ As Integer)
            PlayerData(Index_).Ingame = True
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.JoinWorldReply)
            writer.Byte(1)
            writer.Byte(&HB4)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.JoinWorldUnknown)
            writer.Word(0)
            ' Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.JoinWorldUnknown2)
            writer.Byte(0)
            '    Server.Send(writer.GetBytes, Index_)

            'UpdateState(4, 2, Index_) 'Untouchable Status
            ObjectSpawnCheck(Index_)
            OnStatsPacket(Index_)
            OnSendSilks(Index_)
            If PlayerData(Index_).InGuild = True Then
                SendGuildInfo(Index_, False)
                LinkPlayerToGuild(Index_)
            End If
        End Sub

        Public Sub Player_CheckDeath(ByVal Index_ As Integer, ByVal SetPosToTown As Boolean)
            If PlayerData(Index_).CHP = 0 Then
                PlayerData(Index_).Alive = True
                PlayerData(Index_).CHP = PlayerData(Index_).HP / 2
                DataBase.SaveQuery(String.Format("UPDATE characters SET cur_hp='{0}', hp='{1}' where id='{2}'", PlayerData(Index_).CHP, PlayerData(Index_).HP, PlayerData(Index_).CharacterId))


                If SetPosToTown Then
                    PlayerData(Index_).Position = PlayerData(Index_).Position_Return
                    DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                End If

            End If
        End Sub

        Public Function IsCharChinese(ByVal Model As UInteger) As Boolean
            If Model >= 1907 And Model <= 1932 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function IsCharEurope(ByVal Model As UInteger) As Boolean
            If Model >= 14717 And Model <= 14743 Then
                Return True
            Else
                Return False
            End If
        End Function
    End Module
End Namespace
