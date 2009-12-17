Option Strict Off
Option Explicit On
Module modGameserver
	'SRVB - SREmu VB Open-Source Project
	'Copyright (C) 2008 DarkInc Community
	'
	'This program is free software: you can redistribute it and/or modify
	'it under the terms of the GNU General Public License as published by
	'the Free Software Foundation, either version 3 of the License, or
	'(at your option) any later version.
	'
	'This program is distributed in the hope that it will be useful,
	'but WITHOUT ANY WARRANTY; without even the implied warranty of
	'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	'GNU General Public License for more details.
	'
	'You should have received a copy of the GNU General Public License
	'along with this program.  If not, see <http://www.gnu.org/licenses/>.
	
	Public sData As String
	Private fData As String
	Private pLen As Short
	Private i As Short
	Private x As Short
	
	Public Function ParseGameData(ByRef data As String, ByRef index As Short) As Object
		
		Dim dData As String
		dData = cv_HexFromString(data)
		
		Dim sSize As Short
		Dim sOpcode As String
		
		sSize = CShort("&H" & Mid(dData, 3, 2) & Mid(dData, 1, 2))
		sOpcode = Mid(dData, 7, 2) & Mid(dData, 5, 2)
		If sSize > 0 Then sData = Mid(dData, 13, sSize * 2)
		
		Select Case sOpcode
			
			Case "707B" 'Ping packet???
                Debug.Print("*707B*")
				
			Case "756C" 'UnSummon pet
				UnsummonPet(index)
				
			Case "7683" 'Client enables/disables noob-icon
				HandleLight(index, sData)
				
			Case "7618"
				TerminateTransport(index)
				
			Case "769E"
				RidingMovement(index)
				
			Case "7165"
				LvlMastery(index)
				
			Case "74B5"
				UnSummonTransport(index)
				
			Case "324B"
				HandleEmote(index)
				
			Case "7341"
				HandleBerserk(index, True)
				
			Case "727A" 'Lvl up str
				insertstr(index)
				
			Case "7552" 'lvl up int
				insertint(index)
				
			Case "72CB"
				LvlSkill(index)
				
			Case "745A"
				SelectObject(index)
				
			Case "72CD" 'Player Send Action
				If Mid(sData, 3, 2) = "02" Then
					WalkToItem(index)
				Else
					PlayerSendAttack(index)
				End If
				
			Case "706D" 'Character moves item
				HandleItemMovement(index)
				
			Case "2001" 'Client wants to know who we are
				SendWhoAmI(index)
				
			Case "6100" 'Client whoami. Send patch-info.
				SendPatchInfo(index)
				
			Case "9000"
				'Client accepts communication type.
				
			Case "2002"
				'Ping packet, ignore.
				
			Case "6101"
				'Need proper response...
				
			Case "6103" 'Login...
				HandleLogin(index)
				
			Case "7367"
				HandleChat(index)
				
			Case "72F7" 'Character manipulaiton.
				
				Select Case "&H" & Mid(sData, 1, 2)
					
					Case CStr(2) 'Character listing.
						SendCharacterListing(index)
						
					Case CStr(1) 'Character create.
						CreateCharacter(index, sData)
						
					Case CStr(4) 'Character name check.
						CheckCharacterName(index, sData)
						
					Case Else
						Debug.Print("Unknown 72F7 identifier: " & sData)
						
				End Select
				
			Case "7426" 'Request to go ingame.
				InitGame(index)
				
			Case "7738" 'Movement.
				HandleMovement(index)
				
			Case "7017" 'Client Sit / Stand / etc.
				HandleState(index, sData)
				
			Case "70B7" 'Quit request
				QuitGame(index, sData)
				
				'======================== Stall ===================================
			Case "7049" 'Stallname Request first time
				Handle_Stall_NameRequest(index, sData)
				
			Case "71A8" 'Diverse Stall Functions
				Handle_Stall_Operations(index, sData)
				
			Case "761F" 'Go Inside Stall
				Handle_Stall_GoInside(index, sData)
				
			Case "76E7" 'Leave Stall
				Handle_Stall_Leave(index, sData)
				
			Case "742C" 'Close Stall
				Handle_Stall_Close(index, sData)
				
				'Case "73F9" 'Buy Item
				'Handle_Stall_BuyItem index, sData
				'============================Exchange===============================
				
			Case "7237" 'Exchange Request
				'Handle_Exchange_Request index, sData
				
				
			Case "72DB" 'Exchange Chancle
				Handle_Exchange_Cancle(index, sData)
				
				'============================ Party ================================
			Case "76FF" 'Form Party
				Handle_Party_Form(index, sData)
				
			Case "7535" ' Delete Party
				Handle_Party_Delete(index, sData)
				
			Case "73DC" 'Edit Party
				Handle_Party_Edit(index, sData)
				
				'Case "7588" 'Show Partylist
				'Handle_Party_ShowList index, sData
				
			Case "3393" 'Exchange Accept/Refuse Request
				Party_answer(index)
				'Handle_Exchange_Answer index, sData
				
			Case "70D5"
				Party_Request(index)
				
				'Case "70D5"
				'Party_ index
				
			Case "7338"
				OpenShop(index)
				
			Case "74B3"
				CloseShop(index)
				
			Case "72C3" 'guild storage
				OpenStr(index)
				
			Case "72D0" 'honor rank
				HonorRank(index)
				
			Case "7495"
				Teleport(index)
				Debug.Print("Unknown opcode Teleport: " & dData & " - " & sOpcode)
				
			Case "36DD"
				Teleport2(index)
				Debug.Print("Unknown opcode Teleport: " & dData & " - " & sOpcode)
				
			Case "75BD"
				UseItem(index, dData)
				
			Case "777A"
				ChangeNotice(index)
				
			Case "32DC"
				PlayerSpawn(index)
			Case "7373"
				fData = "7373C76703191B1A"
				pLen = (Len(fData) - 8) / 2
				fData = WordFromInteger(pLen) & fData
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                MsgBox(fData)
            Case "756E"
                LeaveGuild(index)
            Case Else
                Debug.Print("Unknown opcode: " & dData & " - " & sOpcode)

        End Select
        dex = index
    End Function

    Public Function InitGame(ByRef index As Short) As Object

        Dim sCharSize As Short
        Dim Charname As String

        'this is just temporary
        frmMain.CastingTimer.Load(index)
        frmMain.BerserkTimer.Load(index)
        frmMain.AttackDelay.Load(index)
        frmMain.WalkAttackDelay.Load(index)
        frmMain.PickupDelay.Load(index)
        frmMain.BerserkTimer(index).Interval = 12400
        frmMain.BerserkTimer(index).Enabled = False

        sCharSize = CShort("&H" & Mid(sData, 3, 2) & Mid(sData, 1, 2))
        Charname = cv_StringFromHex(Mid(sData, 5, sCharSize * 2))

        PlayerData(index).Charname = Charname 'Update array.

        PlayerData(index).ChatIndex = 0

        'Unsure of these packets.
        modGlobal.GameSocket(index).SendData(cv_StringFromHex("010026B4000001"))
        modGlobal.GameSocket(index).SendData(cv_StringFromHex("00009D370000"))

        fData = "B332"
        fData = fData & "0000"

        '============character info start============

        fData = fData & DWordFromInteger(PlayerData(index).Chartype)
        fData = fData & ByteFromInteger(PlayerData(index).Volume)
        fData = fData & ByteFromInteger(PlayerData(index).level)
        fData = fData & ByteFromInteger(PlayerData(index).HighLevel)
        'fData = fData & "0000000000000000"  'Experience
        fData = fData & DWordFromInteger(PlayerData(index).Exp) 'Experience
        fData = fData & "00000000" 'Experience
        fData = fData & "C800" 'WordFromInteger(PlayerData(index).SpBar) '"C800"          'SP/EXP Bar
        fData = fData & "0000" 'SP/EXP Bar
        fData = fData & DWordFromInteger(PlayerData(index).GoldInInventory) '"06A140"  'Gold
        fData = fData & "00000000" 'Gold
        fData = fData & DWordFromInteger(PlayerData(index).Skillpoints)
        fData = fData & WordFromInteger(PlayerData(index).AttributePoints) 'Attributes
        fData = fData & ByteFromInteger(PlayerData(index).BerserkBar) 'Berserk Bar
        fData = fData & "00000000"
        fData = fData & DWordFromInteger(PlayerData(index).HP)
        fData = fData & DWordFromInteger(PlayerData(index).MP)
        fData = fData & "00" 'Noob icon on top of head. (could be other ID, everything over "2" seems to work)
        fData = fData & "00" 'Daily PK
        fData = fData & "0000" 'PK Level
        fData = fData & "00000000" 'Murderer level

        '============item list start============
        fData = fData & ByteFromInteger(45) 'Max item slot

        fData = fData & ByteFromInteger(ItemAmount(index)) 'Number of items
        fData = fData & modGlobal.ItemData(index)

        fData = fData & "04" 'Extra list
        fData = fData & "00" 'Empty extra list

        '============mastery list start============
        fData = fData & "00" 'Mastery list
        'Masteries

        If PlayerData(index).Race = 0 Then 'Chinese
            fData = fData & "0101010000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(1))
            fData = fData & "0102010000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(2))
            fData = fData & "0103010000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(3))
            fData = fData & "0111010000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(4))
            fData = fData & "0112010000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(5))
            fData = fData & "0113010000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(6))
            fData = fData & "0114010000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(7))
        Else 'euro
            fData = fData & "0101020000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(1))
            fData = fData & "0102020000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(2))
            fData = fData & "0103020000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(3))
            fData = fData & "0104020000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(4))
            fData = fData & "0105020000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(5))
            fData = fData & "0106020000"

            fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(6))
        End If

        '============skill list start============
        fData = fData & "0200"
        For i = 1 To PlayerData(index).NumSkills
            fData = fData & "01"

            fData = fData & PlayerData(index).SkillList(i)
            fData = fData & "01"
        Next i
        fData = fData & "020101000000"

        '============quest list start============
        fData = fData & ByteFromInteger(0) '# of completed quests
        fData = fData & "00" 'No quests.

        '============other data start============

        fData = fData & PlayerData(index).CharID

        fData = fData & ByteFromInteger(PlayerData(index).XSection) 'X-section

        fData = fData & ByteFromInteger(PlayerData(index).YSection) 'Y-section
        fData = fData & Inverse(Float2Hex((PlayerData(index).XPos - ((PlayerData(index).XSection) - 135) * 192) * 10))
        fData = fData & "00000000"
        fData = fData & Inverse(Float2Hex((PlayerData(index).YPos - ((PlayerData(index).YSection) - 92) * 192) * 10))
        fData = fData & "A822" 'Player angle
        fData = fData & "000100" 'Angledata(playerstandingstill)
        fData = fData & "A822" 'Player angle(0x000(rightofradar)>0ffff)

        fData = fData & "0000" 'State - 02 = dead
        fData = fData & "00" 'Berserker
        fData = fData & Inverse(Float2Hex(PlayerData(index).WalkSpeed)) 'Playerspeed while walking
        fData = fData & Inverse(Float2Hex(PlayerData(index).RunSpeed)) 'Playerspeed while running
        fData = fData & Inverse(Float2Hex(PlayerData(index).BerserkSpeed)) 'Playerspeed while berserk
        fData = fData & "00"

        fData = fData & WordFromInteger(Len(PlayerData(index).Charname))
        fData = fData & cv_HexFromString(PlayerData(index).Charname)

        'This data contains guildname, etc... Unsupported for now.
        fData = fData & "00000001000000000000000000000000000000FF00000000"
        fData = fData & PlayerData(index).AccountId
        fData = fData & "0007000000000000000000010001000000"

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        'Unsure of these packets.
        modGlobal.GameSocket(index).SendData(cv_StringFromHex("0000DB310000"))
        modGlobal.GameSocket(index).SendData(cv_StringFromHex("0800A6320000" & PlayerData(index).CharID & "01001108"))

        fData = "2400"
        fData = fData & "3C34"
        fData = fData & "0000"
        fData = fData & DWordFromInteger(PlayerData(index).MinPhyAtk)
        fData = fData & DWordFromInteger(PlayerData(index).MaxPhyAtk)
        fData = fData & DWordFromInteger(PlayerData(index).MinMagAtk)
        fData = fData & DWordFromInteger(PlayerData(index).MaxMagAtk)
        fData = fData & WordFromInteger(PlayerData(index).PhyDef)
        fData = fData & WordFromInteger(PlayerData(index).MagDef)
        fData = fData & WordFromInteger(PlayerData(index).Hit)
        fData = fData & WordFromInteger(PlayerData(index).Parry)
        fData = fData & DWordFromInteger(PlayerData(index).HP)
        fData = fData & DWordFromInteger(PlayerData(index).MP)
        fData = fData & WordFromInteger(PlayerData(index).Strength)
        fData = fData & WordFromInteger(PlayerData(index).Intelligence)
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        SpawnPlayerObjects(index)
        BroadCastPlayerData(index)
        PlayerData(index).Ingame = True
        'Call Guild(index)
        'If PlayerData(index).Place = "01" Then
        'Call JanganNpc(index)


        'ElseIf PlayerData(index).Place = "02" Then
        'Call DowhangNpc(index)


        'ElseIf PlayerData(index).Place = "03" Then
        'Call HotanNpc(index)

        'End If

    End Function

    Public Function SendCharacterListing(ByRef index As Short) As Object
        'Dim AccountDataBase As Recordset
        'Dim BookMark As Variant
        'Dim Chartype As String
        'Dim Volume As String
        'Dim Level As String
        'Dim Strength As String
        'Dim Intelligence As String
        'Dim Skillpoints As String
        'Dim HP As String
        'Dim MP As String
        'Dim HighLevel As String
        'Dim MinPhyAtk As String
        'Dim MaxPhyAtk As String
        'Dim MinMagAtk As String
        'Dim MaxMagAtk As String
        'Dim PhyDef As String
        'Dim MagDef As String
        'Dim Hit As String
        'Dim Parry As String
        'Dim XSection As String
        'Dim YSection As String
        'Dim MasteryLvl(1) As String
        'Dim MasteryLvl(2) As String
        'Dim MasteryLvl(3) As String
        'Dim MasteryLvl(4) As String
        'Dim MasteryLvl(5) As String
        'Dim MasteryLvl(6) As String
        'Dim MasteryLvl(7) As String
        'Dim NumSkills As String
        'Dim BerserkBar As String
        'Dim WalkSpeed As String
        'Dim RunSpeed As String
        'Dim BerserkSpeed As String
        'Dim AttributePoints As String
        'Dim AccountId As String

        'Set AccountDataBase = DataBases.OpenRecordset("Accounts", dbOpenTable)  'DB Table
        'BookMark = AccountDataBase.BookMark

        'AccountDataBase.index = "Id" 'will be added later
        'AccountDataBase.Seek ">=", PlayerData(index).CharPath

        'With AccountDataBase
        '    Chartype
        '    Volume
        '    Level
        '    Strength
        '    Intelligence
        '    Skillpoints
        '    Hp
        '    Mp
        '    HighLevel
        '    MinPhyAtk
        '    MaxPhyAtk
        '    MinMagAtk
        '    MaxMagAtk
        '    PhyDef
        '    MagDef
        '    Hit
        '    Parry
        '    XSection
        '    YSection
        '    MasteryLvl (1)
        '    MasteryLvl (2)
        '    MasteryLvl (3)
        '    MasteryLvl (4)
        '    MasteryLvl (5)
        '    MasteryLvl (6)
        '    MasteryLvl (7)
        '    'NumSkills
        '    BerserkBar
        '    WalkSpeed
        '    RunSpeed
        '    BerserkSpeed
        '    AttributePoints
        '    AccountId
        'End With

        PlayerData(index).Chartype = CShort(iniGetStr("chartype", "character", PlayerData(index).CharPath))
        PlayerData(index).Volume = CShort(iniGetStr("volume", "character", PlayerData(index).CharPath))
        PlayerData(index).level = CShort(iniGetStr("level", "character", PlayerData(index).CharPath))
        PlayerData(index).Strength = CDbl(iniGetStr("strength", "character", PlayerData(index).CharPath))
        PlayerData(index).Intelligence = CDbl(iniGetStr("intelligence", "character", PlayerData(index).CharPath))
        PlayerData(index).Skillpoints = CDbl(iniGetStr("skillpoints", "character", PlayerData(index).CharPath))
        PlayerData(index).HP = CDbl(iniGetStr("hp", "character", PlayerData(index).CharPath))
        PlayerData(index).MP = CDbl(iniGetStr("mp", "character", PlayerData(index).CharPath))
        PlayerData(index).ArtHP = CDbl(iniGetStr("ArtHp", "character", PlayerData(index).CharPath))
        PlayerData(index).ArtMP = CDbl(iniGetStr("ArtMp", "character", PlayerData(index).CharPath))
        PlayerData(index).HighLevel = CDbl(iniGetStr("highlevel", "character", PlayerData(index).CharPath))
        PlayerData(index).MinPhyAtk = CDbl(iniGetStr("minphyatk", "character", PlayerData(index).CharPath))
        PlayerData(index).MaxPhyAtk = CDbl(iniGetStr("maxphyatk", "character", PlayerData(index).CharPath))
        PlayerData(index).MinMagAtk = CDbl(iniGetStr("minmagatk", "character", PlayerData(index).CharPath))
        PlayerData(index).MaxMagAtk = CDbl(iniGetStr("maxmagatk", "character", PlayerData(index).CharPath))
        PlayerData(index).PhyDef = CDbl(iniGetStr("phydef", "character", PlayerData(index).CharPath))
        PlayerData(index).MagDef = CDbl(iniGetStr("magdef", "character", PlayerData(index).CharPath))
        PlayerData(index).Hit = CDbl(iniGetStr("hit", "character", PlayerData(index).CharPath))
        PlayerData(index).Parry = CDbl(iniGetStr("parry", "character", PlayerData(index).CharPath))
        PlayerData(index).XSection = CDbl(iniGetStr("XSection", "character", PlayerData(index).CharPath))
        PlayerData(index).YSection = CDbl(iniGetStr("YSection", "character", PlayerData(index).CharPath))
        PlayerData(index).XPos = CDbl(iniGetStr("XPos", "character", PlayerData(index).CharPath))
        PlayerData(index).YPos = CDbl(iniGetStr("YPos", "character", PlayerData(index).CharPath))

        PlayerData(index).MasteryLvl(1) = CShort(iniGetStr("MasteryLvl(1)", "character", PlayerData(index).CharPath))

        PlayerData(index).MasteryLvl(2) = CShort(iniGetStr("MasteryLvl(2)", "character", PlayerData(index).CharPath))

        PlayerData(index).MasteryLvl(3) = CShort(iniGetStr("MasteryLvl(3)", "character", PlayerData(index).CharPath))

        PlayerData(index).MasteryLvl(4) = CShort(iniGetStr("MasteryLvl(4)", "character", PlayerData(index).CharPath))

        PlayerData(index).MasteryLvl(5) = CShort(iniGetStr("MasteryLvl(5)", "character", PlayerData(index).CharPath))

        PlayerData(index).MasteryLvl(6) = CShort(iniGetStr("MasteryLvl(6)", "character", PlayerData(index).CharPath))

        PlayerData(index).MasteryLvl(7) = CShort(iniGetStr("MasteryLvl(7)", "character", PlayerData(index).CharPath))
        PlayerData(index).NumSkills = CShort(iniGetStr("NumSkills", "character", PlayerData(index).CharPath))
        PlayerData(index).BerserkBar = CShort(iniGetStr("BerserkBar", "character", PlayerData(index).CharPath))
        PlayerData(index).WalkSpeed = CShort(iniGetStr("WalkSpeed", "character", PlayerData(index).CharPath))
        PlayerData(index).RunSpeed = CShort(iniGetStr("RunSpeed", "character", PlayerData(index).CharPath))
        PlayerData(index).BerserkSpeed = CShort(iniGetStr("BerserkSpeed", "character", PlayerData(index).CharPath))
        PlayerData(index).AttributePoints = CShort(iniGetStr("AttributePoints", "character", PlayerData(index).CharPath))
        PlayerData(index).AccountId = iniGetStr("AccountID", "character", PlayerData(index).CharPath)
        PlayerData(index).GoldInInventory = iniGetStr("CharGold", "character", PlayerData(index).CharPath)
        PlayerData(index).Place = iniGetStr("Place", "character", PlayerData(index).CharPath)
        PlayerData(index).Exp = CDbl(iniGetStr("CharExp", "character", PlayerData(index).CharPath))
        PlayerData(index).SpBar = CShort(iniGetStr("SpBar", "character", PlayerData(index).CharPath))
        For i = 1 To (PlayerData(index).NumSkills + 1)
            PlayerData(index).SkillList(i) = iniGetStr("SkillList(" & i & ")", "character", PlayerData(index).CharPath)
        Next i

        If CShort(iniGetStr("GM", "character", PlayerData(index).CharPath)) = 1 Then
            PlayerData(index).GM = True
        End If

        Randomize()
        Dim iRand As Short
        iRand = CShort(Mid(CStr(Rnd(5)), 3, 3))
        PlayerData(index).CharID = DWordFromInteger(iRand)

        fData = "F7B2"
        fData = fData & "0000"
        fData = fData & "0201"
        fData = fData & "01" '# of characters
        '=========STARTCHARACTER=========
        fData = fData & DWordFromInteger(PlayerData(index).Chartype)
        fData = fData & WordFromInteger(Len(PlayerData(index).Charname))
        fData = fData & cv_HexFromString(PlayerData(index).Charname)
        fData = fData & ByteFromInteger(PlayerData(index).Volume)
        fData = fData & ByteFromInteger(PlayerData(index).level)
        fData = fData & DWordFromInteger(PlayerData(index).Exp) 'Experience
        fData = fData & "00000000" 'Experience
        fData = fData & WordFromInteger(PlayerData(index).Strength)
        fData = fData & WordFromInteger(PlayerData(index).Intelligence)
        fData = fData & WordFromInteger(PlayerData(index).AttributePoints) 'Apparently this is attribute points, no SP
        fData = fData & DWordFromInteger(PlayerData(index).HP)
        fData = fData & DWordFromInteger(PlayerData(index).MP)
        fData = fData & "00" 'Marked for deletion
        'fData = fData & "58270000" 'Minutes till deletion.
        fData = fData & "0000"
        fData = fData & "00"

        fData = fData & ByteFromInteger(ListItemAmount(index)) 'Amount of items
        fData = fData & ListItemData(index)
        fData = fData & "00"

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        Debug.Print(fData)
    End Function

    Public Function SendWhoAmI(ByRef index As Short) As Object

        fData = "0E00"
        fData = fData & "0120"
        fData = fData & "0000"
        fData = fData & "0B00"
        fData = fData & cv_HexFromString("AgentServer")
        fData = fData & "00"

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function

    Public Function SendPatchInfo(ByRef index As Short) As Object

        fData = "05000D6002000101000520"
        fData = fData & "0B000D6000000001000154070500000002"
        fData = fData & "05000D6002000101000560"
        fData = fData & "06000D600200000300020002"
        fData = fData & "05000D60020001010000A1"
        fData = fData & "02000D6002000001"
        MsgBox(fData)
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function

    Public Function HandleLogin(ByRef index As Short) As Object

        Dim iNameSize As Short
        Dim iPassSize As Short
        Dim iServerID As Short
        Dim Username As String
        Dim Password As String

        iNameSize = CShort("&H" & Mid(sData, 11, 2) & Mid(sData, 9, 2))
        Username = cv_StringFromHex(Mid(sData, 13, iNameSize * 2))
        iPassSize = CShort("&H" & Mid(sData, 15 + (iNameSize * 2), 2) & Mid(sData, 13 + (iNameSize * 2), 2))
        Password = cv_StringFromHex(Mid(sData, 17 + iNameSize * 2, iPassSize * 2))

        'Build array.
        ReDim Preserve PlayerData(index)
        PlayerData(index).Username = Username
        PlayerData(index).Password = Password
        PlayerData(index).CharPath = (Replace(My.Application.Info.DirectoryPath, "\", "/") & "/accounts/" & Username & ".ini")
        PlayerData(index).Charname = iniGetStr("name", "character", PlayerData(index).CharPath)
        PlayerData(index).Race = CShort(iniGetStr("Race", "character", PlayerData(index).CharPath))

        Dim section As String
        Dim stype As String
        Dim amount As String
        Dim ID As String
        Dim PlusValue As String
        Dim Durability As String
        Dim PhyReinforce As String
        Dim MagReinforce As String

        For i = 0 To 44
            section = "item" & CStr(i)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            stype = iniGetStr("type", section, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            amount = iniGetStr("amount", section, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            ID = iniGetStr("ID", section, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlusValue = iniGetStr("plusvalue", section, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Durability = iniGetStr("durability", section, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PhyReinforce = iniGetStr("phyreinforce", section, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            MagReinforce = iniGetStr("magreinforce", section, PlayerData(index).CharPath)
            If stype <> "(error)" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, i).Type = stype
            End If
            If amount <> "(error)" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, i).amount = amount
            End If
            If ID <> "(error)" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, i).ID = ID
            End If
            If PlusValue <> "(error)" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, i).PlusValue = PlusValue
            End If
            If Durability <> "(error)" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, i).Durability = Durability
            End If
            If PhyReinforce <> "(error)" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, i).PhyReinforce = PhyReinforce
            End If
            If MagReinforce <> "(error)" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, i).MagReinforce = MagReinforce
            End If
        Next i
        BuildItemPackets(index)

        fData = "03A1"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & "2E0768" 'Account ID?
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        'fData = fData & PlayerData(index).AccountId


        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
		
	End Function
End Module