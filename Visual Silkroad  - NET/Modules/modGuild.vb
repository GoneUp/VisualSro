Option Strict Off
Option Explicit On
Module modGuild
	Private fData As String
	Private pLen As Short
	Private i As Short
	Private x As Short
	Public Function Guild(ByRef index As Short) As Object
		
		fData = "2231"
		fData = fData & "0000"
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & PlayerData(index).CharID
		fData = fData & "0402"
		pLen = (Len(fData) - 8) / 2
		fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "C432"
        fData = fData & "0000"
        fData = fData & "02000000" 'Guild ID
        fData = fData & WordFromInteger(Len("Guild #2"))
        fData = fData & cv_HexFromString("Guild #2") 'Guild Name
        fData = fData & "05" 'Guild Level
        fData = fData & "AEC10900" 'Guild Points
        fData = fData & WordFromInteger(Len("DSREmu News"))
        fData = fData & cv_HexFromString("DSREmu News") '1. Guild's Name
        fData = fData & WordFromInteger(Len("Check  [dsremu.smf4u.com]  for News or Updates"))
        fData = fData & cv_HexFromString("Check  [dsremu.smf4u.com]  for News or Updates")
        fData = fData & "0100000000"
        fData = fData & "01" 'Number Of Guildmember
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AccountId konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).AccountId
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(Len(PlayerData(index).Charname))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & cv_HexFromString(PlayerData(index).Charname)
        fData = fData & "0A" '00=Master|0a=Member
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).level)
        fData = fData & "39050000" 'Donate Guild Points
        fData = fData & "FF" 'Authority
        fData = fData & "ffffff000000000000000000000000"
        fData = fData & "0A00" & cv_HexFromString("GameMaster") 'Grant Name
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Chartype konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).Chartype)
        fData = fData & "01" 'FortressWar Position
        fData = fData & "03"
        fData = fData & "00" 'End of Guildinformation
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


        fData = "1E34"
        fData = fData & "0000"
        fData = fData & "0000000000000000"
        fData = fData & "01000000" 'Uni Leader Guild's ID
        fData = fData & "02" 'Number of Guilds in Uni
        fData = fData & "01000000" '1. Guild's ID
        fData = fData & WordFromInteger(Len("Guild #1"))
        fData = fData & cv_HexFromString("Guild #1") '1. Guild's Name
        fData = fData & "05" '1. Guild Level
        fData = fData & WordFromInteger(Len("Union Leader"))
        fData = fData & cv_HexFromString("Union Leader") 'Uni Leader's Name
        fData = fData & "7F070000" 'Uni Leader's CharType
        fData = fData & "03" '1. Guild Number of Member
        fData = fData & "02000000" '2. Guild ID
        fData = fData & WordFromInteger(Len("Guild #2"))
        fData = fData & cv_HexFromString("Guild #2") '2. Guild Name
        fData = fData & "05" '2. Guild Level
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(Len(PlayerData(index).Charname))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & cv_HexFromString(PlayerData(index).Charname)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Chartype konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).Chartype)
        fData = fData & "01" '2. Guild Number of Member
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


        fData = "5F36"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "02000000" 'Guild ID
        fData = fData & WordFromInteger(Len("SRVB Developers"))
        fData = fData & cv_HexFromString("SRVB Developers") 'Guild Name
        fData = fData & WordFromInteger(Len("GameMaster"))
        fData = fData & cv_HexFromString("GameMaster") 'Grant Name
        fData = fData & "01" 'Guild Icon ON
        fData = fData & "0000"
        fData = fData & "00000000000000"
        fData = fData & "01" 'Uni Icon ON
        fData = fData & "0000"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "293B"
        fData = fData & "0000"
        fData = fData & "06"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AccountId konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).AccountId
        fData = fData & "0200" 'Guild Member Log on
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
    End Function
    Public Function ChangeNotice(ByRef index As Short) As Object
        Dim noticelen As String
        Dim noticetext As String
        Dim test As String
        noticelen = CStr(IntegerFromWord(Mid(sData, 1, 4)))
        noticetext = Mid(sData, 5, CDbl(noticelen) * 2)
        test = WordFromInteger(noticelen)
        Dim msglen As String
        Dim msglen2 As String
        msglen = Mid(sData, Len(test) + Len(noticetext) + 1, 4)

        fData = "293B"
        fData = fData & "0000"
        fData = fData & "0510"
        fData = fData & WordFromInteger(noticelen)
        fData = fData & noticetext
        fData = fData & msglen 'uzunluk
        'fData = fData & "00"
        msglen2 = CStr(IntegerFromWord(msglen))
        fData = fData & Mid(sData, Len(test) + Len(noticetext) + 5, CDbl(msglen2) * 2)
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
    End Function

    Public Function LeaveGuild(ByRef index As Short) As Object
        'L(06) 00 O(29 3B) 00 00 03 28 99 0A 00 01
        '....7........n....
        'L(04) 00 O(E6 37) 00 00 D6 BE D6 03 01 00 6E B5 00 00 01
        'MsgBox (sData)
        fData = "293B"
        fData = fData & "0000"
        fData = fData & "0028"
        fData = fData & "150A0001"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "E637"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        'fData = fData & "D6BED603"
        fData = fData & "0100"
        fData = fData & "6EB5"
        fData = fData & "000001"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
    End Function
End Module