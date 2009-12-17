Option Strict Off
Option Explicit On
Module modTeleport
	Private fData As String
	Private pLen As Short
	Private i As Short
	Private x As Short
	Public Function Teleport(ByRef index As Short) As Object
		Dim npcidss As String
		Dim tptype As String
		Dim ObjectID As String
		ObjectID = Left(sData, 8)
		Dim resim As String
		Dim TeleportType As String
		
		TeleportType = Mid(sData, 1, 2) & Mid(sData, 11, 2)
		
		fData = "0300CB300000"
		fData = fData & "020100"
		For i = 1 To UBound(PlayerData)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "17340000"
        fData = fData & ObjectID
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        fData = "00000A330000"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "95B4"
        fData = fData & "0000"
        fData = fData & "01"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        If TeleportType = "0502" Then 'jangan to dw
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Place = "02"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(PlayerData(index).Place), "Place", "character", PlayerData(index).CharPath)
            resim = WordFromInteger(26265)
        ElseIf TeleportType = "060B" Then  'dwto hotan
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Place = "03"
            resim = WordFromInteger(23687)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(PlayerData(index).Place), "Place", "character", PlayerData(index).CharPath)
        ElseIf TeleportType = "0102" Then  'hotan to dw
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Place = "02"
            resim = WordFromInteger(26265)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(PlayerData(index).Place), "Place", "character", PlayerData(index).CharPath)
        ElseIf TeleportType = "0601" Then  'dwto jangan
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Place = "01"
            resim = WordFromInteger(25000)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(PlayerData(index).Place), "Place", "character", PlayerData(index).CharPath)
        Else
            resim = WordFromInteger(26265)
        End If

        fData = "6933"
        fData = fData & "0000"
        'fData = fData & "A861"
        fData = fData & resim
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function
    Public Function Teleport2(ByRef index As Short) As Object
        Dim TeleportToX As String
        Dim TeleportToY As String
        Dim TeleportToXY As String
        Dim TeleportToYY As String
        Dim TeleportType As String
        TeleportType = Mid(sData, 1, 2) & Mid(sData, 11, 2)
        'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim MonsterDataBase As DAO.Recordset
        Dim BookMark As Object
        OpenSremuDataBase()
        MonsterDataBase = DataBases.OpenRecordset("Tp", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        MonsterDataBase.Index = "IDS"
        MonsterDataBase.Seek(">=", TeleportType)

        With MonsterDataBase
            TeleportToX = .Fields("XX").Value
            TeleportToY = .Fields("YY").Value
            TeleportToXY = .Fields("XY").Value
            TeleportToYY = .Fields("ZZ").Value
        End With

        fData = "9D37"
        fData = fData & "0000"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        Dim gold1 As String
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GoldInInventory konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        gold1 = DWordFromInteger(PlayerData(index).GoldInInventory)
        Dim gold2 As String
        gold2 = DWordFromInteger("1232")

        fData = "B332"
        fData = fData & "0000"
        '============character info start============
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Chartype konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).Chartype)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Volume konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).Volume)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).level)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().HighLevel konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).HighLevel)
        fData = fData & gold2 '"06A140"  'Gold
        fData = fData & "00000000" 'Experience
        fData = fData & "C8000000" 'SP/EXP Bar
        fData = fData & gold1 '"06A140"  'Gold
        fData = fData & "00000000" 'Gold
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).Skillpoints)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttributePoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).AttributePoints) 'Attributes
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkBar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).BerserkBar) 'Berserk Bar
        fData = fData & "00000000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).HP)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MP)
        fData = fData & "00" 'Noob icon on top of head. (could be other ID, everything over "2" seems to work)
        fData = fData & "00" 'Daily PK
        fData = fData & "0000" 'PK Level
        fData = fData & "00000000" 'Murderer level

        '============item list start============
        fData = fData & ByteFromInteger(45) 'Max item slot

        fData = fData & ByteFromInteger(ItemAmount(index)) 'Number of items

        'fData = fData & modGlobal.ItemData_Renamed(index)
        fData = fData & modGlobal.ItemData(index)

        fData = fData & "04" 'Extra list
        fData = fData & "00" 'Empty extra list
        Call BuildItemPackets(index)
        '============mastery list start============
        fData = fData & "00" 'Mastery list
        'Masteries
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Race konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
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
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).NumSkills konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        For i = 1 To PlayerData(index).NumSkills
            fData = fData & "01"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().SkillList konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).SkillList(i)
            fData = fData & "01"
        Next i
        fData = fData & "020101000000"

        '============quest list start============
        fData = fData & ByteFromInteger(0) '# of completed quests
        fData = fData & "00" 'No quests.
        'MsgBox (PlayerData(index).TeleportToXs)
        '============other data start============
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        'fData = fData & TeleportToXY
        fData = fData & ByteFromInteger(CShort(TeleportToXY)) 'X-section
        fData = fData & ByteFromInteger(CShort(TeleportToYY)) 'Y-section
        fData = fData & Inverse(Float2Hex((CDbl(TeleportToX) - (CDbl(TeleportToXY) - 135) * 192) * 10))
        fData = fData & "00000000"
        fData = fData & Inverse(Float2Hex((CDbl(TeleportToY) - (CDbl(TeleportToYY) - 92) * 192) * 10))
        fData = fData & "A822" 'Player angle
        fData = fData & "000100" 'Angledata(playerstandingstill)
        fData = fData & "A822" 'Player angle(0x000(rightofradar)>0ffff)

        fData = fData & "0000" 'State - 02 = dead
        fData = fData & "00" 'Berserker
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex(PlayerData(index).WalkSpeed)) 'Playerspeed while walking
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex(PlayerData(index).RunSpeed)) 'Playerspeed while running
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex(PlayerData(index).BerserkSpeed)) 'Playerspeed while berserk
        fData = fData & "00"

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(Len(PlayerData(index).Charname))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & cv_HexFromString(PlayerData(index).Charname)

        'This data contains guildname, etc... Unsupported for now.
        fData = fData & "00000001000000000000000000000000000000FF00000000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AccountId konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).AccountId
        fData = fData & "0007000000000000000000010001000000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(TeleportToXY), "XSection", "character", PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(TeleportToYY), "YSection", "character", PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(TeleportToX), "XPos", "character", PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(TeleportToY), "YPos", "character", PlayerData(index).CharPath)

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        modGlobal.GameSocket(index).SendData(cv_StringFromHex("0000DB310000"))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        modGlobal.GameSocket(index).SendData(cv_StringFromHex("0800A6320000" & PlayerData(index).CharID & "09000B21"))

        'If PlayerData(index).Place = "01" Then
        'Call JanganNpc(index)
        'ElseIf PlayerData(index).Place = "02" Then
        'Call DowhangNpc(index)
        'ElseIf PlayerData(index).Place = "03" Then
        'Call HotanNpc(index)
        'End If
        'Call SpawnPlayerObjects(index)
        'Call BroadCastPlayerData(index)
        Call Guild(index)
    End Function
    Public Function ReturnT(ByRef index As Short) As Object
        Dim npcidss As String
        Dim tptype As String
        Dim ObjectID As String
        ObjectID = Left(sData, 8)
        Dim resim As String
        Dim TeleportType As String

        TeleportType = Mid(sData, 1, 2) & Mid(sData, 11, 2)

        fData = "0300CB300000"
        fData = fData & "020100"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "17340000"
        fData = fData & ObjectID
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        fData = "00000A330000"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "95B4"
        fData = fData & "0000"
        fData = fData & "01"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        If TeleportType = "0502" Then 'jangan to dw
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Place = "02"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(PlayerData(index).Place), "Place", "character", PlayerData(index).CharPath)
            resim = WordFromInteger(26265)
        ElseIf TeleportType = "060B" Then  'dwto hotan
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Place = "03"
            resim = WordFromInteger(23687)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(PlayerData(index).Place), "Place", "character", PlayerData(index).CharPath)
        ElseIf TeleportType = "0102" Then  'hotan to dw
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Place = "02"
            resim = WordFromInteger(26265)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(PlayerData(index).Place), "Place", "character", PlayerData(index).CharPath)
        ElseIf TeleportType = "0601" Then  'dwto jangan
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Place = "01"
            resim = WordFromInteger(25000)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(PlayerData(index).Place), "Place", "character", PlayerData(index).CharPath)
        Else
            resim = WordFromInteger(26265)
        End If

        fData = "6933"
        fData = fData & "0000"
        'fData = fData & "A861"
        fData = fData & resim
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function
    Public Function ReturnT2(ByRef index As Short) As Object
        Dim TeleportToX As String
        Dim TeleportToY As String
        Dim TeleportToXY As String
        Dim TeleportToYY As String
        Dim TeleportType As String



        TeleportToX = CStr(6430)
        TeleportToY = CStr(1096)
        TeleportToXY = CStr(167)
        TeleportToYY = CStr(97)


        fData = "9D37"
        fData = fData & "0000"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        Dim gold1 As String
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GoldInInventory konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        gold1 = DWordFromInteger(PlayerData(index).GoldInInventory)
        Dim gold2 As String
        gold2 = DWordFromInteger("1232")

        fData = "B332"
        fData = fData & "0000"
        '============character info start============
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Chartype konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).Chartype)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Volume konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).Volume)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).level)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().HighLevel konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).HighLevel)
        fData = fData & gold2 '"06A140"  'Gold
        fData = fData & "00000000" 'Experience
        fData = fData & "C8000000" 'SP/EXP Bar
        fData = fData & gold1 '"06A140"  'Gold
        fData = fData & "00000000" 'Gold
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).Skillpoints)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttributePoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).AttributePoints) 'Attributes
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkBar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).BerserkBar) 'Berserk Bar
        fData = fData & "00000000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).HP)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MP)
        fData = fData & "00" 'Noob icon on top of head. (could be other ID, everything over "2" seems to work)
        fData = fData & "00" 'Daily PK
        fData = fData & "0000" 'PK Level
        fData = fData & "00000000" 'Murderer level

        '============item list start============
        fData = fData & ByteFromInteger(45) 'Max item slot

        fData = fData & ByteFromInteger(ItemAmount(index)) 'Number of items

        'fData = fData & modGlobal.ItemData_Renamed(index)
        fData = fData & modGlobal.ItemData(index)

        fData = fData & "04" 'Extra list
        fData = fData & "00" 'Empty extra list
        Call BuildItemPackets(index)
        '============mastery list start============
        fData = fData & "00" 'Mastery list
        'Masteries
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Race konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
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
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).NumSkills konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        For i = 1 To PlayerData(index).NumSkills
            fData = fData & "01"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().SkillList konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).SkillList(i)
            fData = fData & "01"
        Next i
        fData = fData & "020101000000"

        '============quest list start============
        fData = fData & ByteFromInteger(0) '# of completed quests
        fData = fData & "00" 'No quests.
        'MsgBox (PlayerData(index).TeleportToXs)
        '============other data start============
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        'fData = fData & TeleportToXY
        fData = fData & ByteFromInteger(CShort(TeleportToXY)) 'X-section
        fData = fData & ByteFromInteger(CShort(TeleportToYY)) 'Y-section
        fData = fData & Inverse(Float2Hex((CDbl(TeleportToX) - (CDbl(TeleportToXY) - 135) * 192) * 10))
        fData = fData & "00000000"
        fData = fData & Inverse(Float2Hex((CDbl(TeleportToY) - (CDbl(TeleportToYY) - 92) * 192) * 10))
        fData = fData & "A822" 'Player angle
        fData = fData & "000100" 'Angledata(playerstandingstill)
        fData = fData & "A822" 'Player angle(0x000(rightofradar)>0ffff)

        fData = fData & "0000" 'State - 02 = dead
        fData = fData & "00" 'Berserker
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex(PlayerData(index).WalkSpeed)) 'Playerspeed while walking
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex(PlayerData(index).RunSpeed)) 'Playerspeed while running
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex(PlayerData(index).BerserkSpeed)) 'Playerspeed while berserk
        fData = fData & "00"

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(Len(PlayerData(index).Charname))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & cv_HexFromString(PlayerData(index).Charname)

        'This data contains guildname, etc... Unsupported for now.
        fData = fData & "00000001000000000000000000000000000000FF00000000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AccountId konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).AccountId
        fData = fData & "0007000000000000000000010001000000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(TeleportToXY), "XSection", "character", PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(TeleportToYY), "YSection", "character", PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(TeleportToX), "XPos", "character", PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(TeleportToY), "YPos", "character", PlayerData(index).CharPath)

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        modGlobal.GameSocket(index).SendData(cv_StringFromHex("0000DB310000"))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        modGlobal.GameSocket(index).SendData(cv_StringFromHex("0800A6320000" & PlayerData(index).CharID & "09000B21"))

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().respawnnpc konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        NPCList(index).respawnnpc = 0
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(index).respawnnpc konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If NPCList(index).respawnnpc = 0 Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf PlayerData(index).Place = "01" Then
            Call JanganNpc(index)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().respawnnpc konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            NPCList(index).respawnnpc = 1
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(index).respawnnpc konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf NPCList(index).respawnnpc = 0 Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf PlayerData(index).Place = "02" Then
            Call DowhangNpc(index)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().respawnnpc konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            NPCList(index).respawnnpc = 1
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(index).respawnnpc konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf NPCList(index).respawnnpc = 0 Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Place konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf PlayerData(index).Place = "03" Then
            Call HotanNpc(index)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().respawnnpc konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            NPCList(index).respawnnpc = 1
        End If
        Call SpawnPlayerObjects(index)
        Call BroadCastPlayerData(index)
    End Function
    Public Function ReturnPoint(ByRef index As Short) As Object

        fData = "0DB2"
        fData = fData & "0000"
        fData = fData & "01"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function
End Module