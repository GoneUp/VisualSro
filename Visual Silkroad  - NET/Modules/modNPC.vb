Option Strict Off
Option Explicit On
Module modNPC
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
	
	Private fData As String
	Private pLen As Short
	Private i As Short
	Private x As Short
	
	Public Function SpawnNPC(ByRef index As Short, ByRef NPCID As Short, ByRef UniqueID As Short) As Object
		Dim ID As String
		
		fData = "0300CB300000"
		fData = fData & "010100"
		For i = 1 To UBound(PlayerData)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "17340000"
        fData = fData & DWordFromInteger(NPCID)
        'fData = fData & DWordFromInteger(UniqueID)
        ID = (Inverse(DecToHexLong(CInt(Rnd() * 1265535) + 101001)))
        fData = fData & ID

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((PlayerData(index).XPos - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
        fData = fData & "00000000" 'Z
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((PlayerData(index).YPos - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
        fData = fData & "0000"
        fData = fData & "000100"
        fData = fData & "0000"
        fData = fData & "010000"
        fData = fData & "00000000000000000000C842"
        fData = fData & "0000"
        'extra data(different for each shop,npcs that do nothing just use the 0000 above)
        'fData = fData & "0002"
        'fData = fData & "03040000"
        'just testing
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        For i = 1 To UBound(NPCList)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If NPCList(i).ID = "" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ID = ID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).NPCID = NPCID
                Exit For
            End If
        Next i

        fData = "00000A330000"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i


    End Function

    Public Function SelectNPC(ByRef index As Short, ByRef NPCIndex As Short) As Object
        Dim ChatId As String
        'UPGRADE_WARNING: Arrays in Struktur NpcDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim NpcDataBase As DAO.Recordset
        Dim BookMark As Object
        OpenSremuDataBase()
        NpcDataBase = DataBases.OpenRecordset("NpcData", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        NpcDataBase.Index = "npcid1"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        NpcDataBase.Seek(">=", NPCList(NPCIndex).NPCID)
        With NpcDataBase
            ChatId = .Fields("sh2").Value
        End With

        fData = "5AB4"
        fData = fData & "0000"
        fData = fData & "01"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & NPCList(NPCIndex).ID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(NPCIndex).NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If NPCList(NPCIndex).NPCID = "2E080000" Then
            fData = fData & "C00000000000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(NPCIndex).NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf NPCList(NPCIndex).NPCID = "2F080000" Then
            fData = fData & "C0000000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(NPCIndex).NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf NPCList(NPCIndex).NPCID = "30080000" Then
            fData = fData & "C0000000"
        Else
            fData = fData & "00"
            fData = fData & ChatId 'ChatId
            'fData = fData & "00"
        End If

        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function
    Public Function SpawnNPC2(ByRef index As Short, ByRef npcs As Short) As Object
        Dim NPCID As Short
        Dim ID As String
        Dim dbnpcid As String
        Dim dbnpcidd As String
        Dim dbnpcxx As String
        Dim dbnpcyy As String
        Dim dbnpczz As String
        Dim dbnpcxy As String
        Dim dbnpcid2 As String
        Dim dbnpcidd2 As String
        Dim dbnpcxx2 As String
        Dim dbnpcyy2 As String
        Dim dbnpczz2 As String
        Dim dbnpcxy2 As String
        Dim UNKNn As String
        Dim Zone1 As String
        Dim Zone2 As String
        Dim NpcShopID As String
        'UPGRADE_WARNING: Arrays in Struktur NpcDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim NpcDataBase As DAO.Recordset
        Dim BookMark As Object
        OpenSremuDataBase()
        NpcDataBase = DataBases.OpenRecordset("jangannpc", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(NpcDataBase.Bookmark)

        NpcDataBase.Index = "ID"
        NpcDataBase.Seek(">=", npcs)

        With NpcDataBase
            dbnpcid = .Fields("NPCSID").Value 'Finds mob Hp
            dbnpcidd = .Fields("NPCSHID").Value
            dbnpcxx = .Fields("xx").Value
            dbnpcyy = .Fields("zz").Value
            dbnpczz = .Fields("YY").Value
            dbnpcxy = .Fields("xy").Value
            UNKNn = .Fields("UNKN").Value
            Zone1 = .Fields("zone").Value
        End With

        fData = "0300CB300000"
        fData = fData & "010100"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "17340000"
        fData = fData & dbnpcid
        'fData = fData & DWordFromInteger(UniqueID)
        ID = dbnpcidd
        fData = fData & ID
        fData = fData & dbnpcxy 'X sector
        fData = fData & dbnpcxx 'ySector
        fData = fData & dbnpczz
        fData = fData & dbnpczz
        'fData = fData & "0000"
        fData = fData & Zone1
        fData = fData & "010000"
        fData = fData & "00000000000000000000C842"
        fData = fData & "0000"
        'extra data(different for each shop,npcs that do nothing just use the 0000 above)
        'fData = fData & "00020B000080"
        'fData = fData &
        'just testing
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        For i = 1 To UBound(NPCList)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If NPCList(i).ID = "" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ID = ID
                'MsgBox (cv_HexFromWords(dbnpcid))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).NPCID = dbnpcid
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ChatId konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ChatId = UNKNn
                Exit For
            End If
        Next i

        fData = "00000A330000"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i


    End Function
    Public Function JanganNpc(ByRef index As Short) As Object
        SpawnNPC4(index, 40)
        SpawnNPC2(index, 1)
        SpawnNPC2(index, 2)
        SpawnNPC2(index, 3)
        SpawnNPC2(index, 4)
        SpawnNPC2(index, 5)
        SpawnNPC2(index, 6)
        SpawnNPC2(index, 7)
        SpawnNPC2(index, 8)
        SpawnNPC2(index, 9)
        SpawnNPC2(index, 10)
        SpawnNPC2(index, 11)
        SpawnNPC2(index, 12)
        SpawnNPC2(index, 13)
        SpawnNPC2(index, 14)
        SpawnNPC2(index, 15)
        SpawnNPC2(index, 16)
        SpawnNPC2(index, 17)
        SpawnNPC2(index, 18)
        SpawnNPC2(index, 19)
        SpawnNPC2(index, 20)
        SpawnNPC2(index, 21)
        SpawnNPC2(index, 22)
        SpawnNPC2(index, 23)
        SpawnNPC2(index, 24)
        SpawnNPC2(index, 25)
        SpawnNPC2(index, 26)
        SpawnNPC2(index, 27)
        SpawnNPC2(index, 28)
        SpawnNPC2(index, 29)
        SpawnNPC2(index, 30)
        SpawnNPC2(index, 31)
        SpawnNPC2(index, 32)
        SpawnNPC2(index, 33)
        SpawnNPC2(index, 34)
        SpawnNPC2(index, 35)
        SpawnNPC2(index, 36)
        SpawnNPC2(index, 37)
        SpawnNPC2(index, 38)
        SpawnNPC2(index, 39)
        SpawnNPC2(index, 41)
    End Function
    Public Function SpawnNPC4(ByRef index As Short, ByRef npcs As Short) As Object
        'UPGRADE_WARNING: Arrays in Struktur NpcDbJangan müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim NpcDbJangan As DAO.Recordset
        Dim BookMark As Object
        Dim tpid As String
        Dim ID As String
        Dim tpidd As String
        Dim dbnpcxx As String
        Dim dbnpcyy As String
        Dim dbnpczz As String
        Dim dbnpcxy As String
        Dim dbnpcid As String
        OpenSremuDataBase()
        NpcDbJangan = DataBases.OpenRecordset("TeleportData", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(NpcDbJangan.Bookmark)

        NpcDbJangan.Index = "ID"
        NpcDbJangan.Seek(">=", npcs)

        With NpcDbJangan
            tpid = .Fields("NPCSID").Value 'Finds mob Hp
            tpidd = .Fields("NPCSHID").Value
            dbnpcxx = .Fields("xx").Value
            dbnpcyy = .Fields("zz").Value
            dbnpczz = .Fields("YY").Value
            dbnpcxy = .Fields("xy").Value
        End With

        fData = "0300CB300000"
        fData = fData & "010100"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "17340000"
        fData = fData & tpid
        'fData = fData & DWordFromInteger(UniqueID)
        'ID = (Inverse((DecToHexLong(CLng(Rnd * 1265535) + 101001))))
        fData = fData & tpidd
        fData = fData & dbnpcxy 'X sector
        fData = fData & dbnpcxx
        fData = fData & dbnpcyy 'Z
        fData = fData & dbnpczz
        'fData = fData & "0000"
        'fData = fData & "000100"
        'fData = fData & "0000"
        'fData = fData & "010000"
        'fData = fData & "00000000000000000000C842"
        'fData = fData & "0000"
        'extra data(different for each shop,npcs that do nothing just use the 0000 above)
        'fData = fData & "0002"
        'fData = fData & "03040000"
        'just testing
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        For i = 1 To UBound(NPCList)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If NPCList(i).ID = "" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ID = tpidd
                'MsgBox (cv_HexFromWords(dbnpcid))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).NPCID = tpid
                'NPCList(i).ChatId = UNKNn
                Exit For
            End If
        Next i

        fData = "00000A330000"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i


    End Function
    Public Function SpawnNPCDw(ByRef index As Short, ByRef npcs As Short) As Object
        'UPGRADE_WARNING: Arrays in Struktur NpcDbDownhang müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim NpcDbDownhang As DAO.Recordset
        Dim BookMark As Object
        OpenSremuDataBase()
        NpcDbDownhang = DataBases.OpenRecordset("DowhangNpc", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(NpcDbDownhang.Bookmark)

        NpcDbDownhang.Index = "ID"
        NpcDbDownhang.Seek(">=", npcs)

        Dim NPCID As Short
        Dim ID As String
        Dim dbnpcid As String
        Dim dbnpcidd As String
        Dim dbnpcxx As String
        Dim dbnpcyy As String
        Dim dbnpczz As String
        Dim dbnpcxy As String
        Dim dbnpcid2 As String
        Dim dbnpcidd2 As String
        Dim dbnpcxx2 As String
        Dim dbnpcyy2 As String
        Dim dbnpczz2 As String
        Dim dbnpcxy2 As String
        Dim UNKNn As String
        Dim Zone1 As String
        Dim Zone2 As String
        Dim NpcShopID As String

        With NpcDbDownhang
            dbnpcid = .Fields("NPCSID").Value 'Finds mob Hp
            dbnpcidd = .Fields("NPCSHID").Value
            dbnpcxx = .Fields("xx").Value
            dbnpcyy = .Fields("zz").Value
            dbnpczz = .Fields("YY").Value
            dbnpcxy = .Fields("xy").Value
            UNKNn = .Fields("UNKN").Value
            Zone1 = .Fields("zone").Value
        End With

        fData = "0300CB300000"
        fData = fData & "010100"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "17340000"
        fData = fData & dbnpcid
        ID = dbnpcidd
        fData = fData & ID
        fData = fData & dbnpcxy 'X sector
        fData = fData & dbnpcxx 'ySector
        fData = fData & dbnpczz
        fData = fData & dbnpczz
        fData = fData & Zone1
        fData = fData & "010000"
        fData = fData & "00000000000000000000C842"
        fData = fData & "0000"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        For i = 1 To UBound(NPCList)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If NPCList(i).ID = "" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ID = ID
                'MsgBox (cv_HexFromWords(dbnpcid))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).NPCID = dbnpcid
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ChatId konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ChatId = UNKNn
                Exit For
            End If
        Next i

        fData = "00000A330000"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
    End Function

    Public Function DowhangNpc(ByRef index As Short) As Object
        SpawnNPCDw(index, 1)
        SpawnNPCDw(index, 2)
        SpawnNPCDw(index, 3)
        SpawnNPCDw(index, 4)
        SpawnNPCDw(index, 5)
        SpawnNPCDw(index, 6)
        SpawnNPCDw(index, 7)
        SpawnNPCDw(index, 8)
        SpawnNPCDw(index, 9)
        SpawnNPCDw(index, 10)
        SpawnNPCDw(index, 11)
        SpawnNPCDw(index, 12)
        SpawnNPCDw(index, 13)
        SpawnNPCDw(index, 14)
        SpawnNPCDw(index, 15)
        SpawnNPCDw(index, 16)
        SpawnNPCDw(index, 17)
        SpawnNPCDw(index, 18)
        SpawnNPCDw(index, 19)
        SpawnNPCDw(index, 20)
        SpawnNPCDw(index, 21)
        SpawnNPCDw(index, 23)
        SpawnNPCDw(index, 24)
        SpawnNPCDw(index, 25)
        SpawnNPCDw(index, 26)
        SpawnNPC4(index, 41)
    End Function












    Public Function EuropeNpc(ByRef index As Short) As Object
        SpawnNPCDeu(index, 1)
        SpawnNPCDeu(index, 2)
        SpawnNPCDeu(index, 3)
        SpawnNPCDeu(index, 4)
        SpawnNPCDeu(index, 5)
        SpawnNPCDeu(index, 6)
        SpawnNPCDeu(index, 7)
        SpawnNPCDeu(index, 8)
        SpawnNPCDeu(index, 9)
        SpawnNPCDeu(index, 10)
        SpawnNPCDeu(index, 11)
        SpawnNPCDeu(index, 12)
        SpawnNPCDeu(index, 13)
        SpawnNPCDeu(index, 14)
        SpawnNPCDeu(index, 15)
        SpawnNPCDeu(index, 16)
        SpawnNPCDeu(index, 17)
        SpawnNPCDeu(index, 18)
        SpawnNPCDeu(index, 19)
        SpawnNPCDeu(index, 20)
        SpawnNPCDeu(index, 21)
        SpawnNPCDeu(index, 23)
        SpawnNPCDeu(index, 24)
        SpawnNPCDeu(index, 25)
        SpawnNPCDeu(index, 26)
        SpawnNPC4(index, 41)
    End Function


    Public Function SpawnNPCDeu(ByRef index As Short, ByRef npcs As Short) As Object
        'UPGRADE_WARNING: Arrays in Struktur NpcDbEurope müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim NpcDbEurope As DAO.Recordset
        Dim BookMark As Object
        OpenSremuDataBase()
        NpcDbEurope = DataBases.OpenRecordset("EuropeNpc", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(NpcDbEurope.Bookmark)

        NpcDbEurope.Index = "ID"
        NpcDbEurope.Seek(">=", npcs)

        Dim NPCID As Short
        Dim ID As String
        Dim dbnpcid As String
        Dim dbnpcidd As String
        Dim dbnpcxx As String
        Dim dbnpcyy As String
        Dim dbnpczz As String
        Dim dbnpcxy As String
        Dim dbnpcid2 As String
        Dim dbnpcidd2 As String
        Dim dbnpcxx2 As String
        Dim dbnpcyy2 As String
        Dim dbnpczz2 As String
        Dim dbnpcxy2 As String
        Dim UNKNn As String
        Dim Zone1 As String
        Dim Zone2 As String
        Dim NpcShopID As String

        With NpcDbEurope
            dbnpcid = .Fields("NPCSID").Value 'Finds mob Hp
            dbnpcidd = .Fields("NPCSHID").Value
            dbnpcxx = .Fields("xx").Value
            dbnpcyy = .Fields("zz").Value
            dbnpczz = .Fields("YY").Value
            dbnpcxy = .Fields("xy").Value
            UNKNn = .Fields("UNKN").Value
            Zone1 = .Fields("zone").Value
        End With

        fData = "0300CB300000"
        fData = fData & "010100"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "17340000"
        fData = fData & dbnpcid
        ID = dbnpcidd
        fData = fData & ID
        fData = fData & dbnpcxy 'X sector
        fData = fData & dbnpcxx 'ySector
        fData = fData & dbnpczz
        fData = fData & dbnpczz
        fData = fData & Zone1
        fData = fData & "010000"
        fData = fData & "00000000000000000000C842"
        fData = fData & "0000"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        For i = 1 To UBound(NPCList)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If NPCList(i).ID = "" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ID = ID
                'MsgBox (cv_HexFromWords(dbnpcid))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).NPCID = dbnpcid
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ChatId konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ChatId = UNKNn
                Exit For
            End If
        Next i

        fData = "00000A330000"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
    End Function



















    Public Function SpawnHotanNpc(ByRef index As Short, ByRef npcs As Short) As Object
        'UPGRADE_WARNING: Arrays in Struktur NpcDbDownhang müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim NpcDbDownhang As DAO.Recordset
        Dim BookMark As Object
        OpenSremuDataBase()
        NpcDbDownhang = DataBases.OpenRecordset("HotanNpc", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(NpcDbDownhang.Bookmark)

        NpcDbDownhang.Index = "ID"
        NpcDbDownhang.Seek(">=", npcs)

        Dim NPCID As Short
        Dim ID As String
        Dim dbnpcid As String
        Dim dbnpcidd As String
        Dim dbnpcxx As String
        Dim dbnpcyy As String
        Dim dbnpczz As String
        Dim dbnpcxy As String
        Dim dbnpcid2 As String
        Dim dbnpcidd2 As String
        Dim dbnpcxx2 As String
        Dim dbnpcyy2 As String
        Dim dbnpczz2 As String
        Dim dbnpcxy2 As String
        Dim UNKNn As String
        Dim Zone1 As String
        Dim Zone2 As String
        Dim NpcShopID As String

        With NpcDbDownhang
            dbnpcid = .Fields("NPCSID").Value 'Finds mob Hp
            dbnpcidd = .Fields("NPCSHID").Value
            dbnpcxx = .Fields("xx").Value
            dbnpcyy = .Fields("zz").Value
            dbnpczz = .Fields("YY").Value
            dbnpcxy = .Fields("xy").Value
            UNKNn = .Fields("UNKN").Value
            Zone1 = .Fields("zone").Value
        End With

        fData = "0300CB300000"
        fData = fData & "010100"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "17340000"
        fData = fData & dbnpcid
        ID = dbnpcidd
        fData = fData & ID
        fData = fData & dbnpcxy 'X sector
        fData = fData & dbnpcxx
        fData = fData & dbnpczz
        fData = fData & dbnpczz
        fData = fData & Zone1
        fData = fData & "010000"
        fData = fData & "00000000000000000000C842"
        fData = fData & "0000"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        For i = 1 To UBound(NPCList)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If NPCList(i).ID = "" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().xx konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).xx = dbnpcxx
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().xy konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).xy = dbnpcxy
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().zz konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).zz = dbnpczz
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().zone konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).zone = Zone1
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ID = dbnpcidd
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().NPCID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).NPCID = dbnpcid
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList().ChatId konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                NPCList(i).ChatId = UNKNn
                Exit For
            End If
        Next i

        fData = "00000A330000"
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
    End Function
	Public Function HotanNpc(ByRef index As Short) As Object
		SpawnHotanNpc(index, 2)
		SpawnHotanNpc(index, 3)
		SpawnHotanNpc(index, 4)
		SpawnHotanNpc(index, 5)
		SpawnHotanNpc(index, 6)
		SpawnHotanNpc(index, 7)
		SpawnHotanNpc(index, 8)
		SpawnHotanNpc(index, 9)
		SpawnHotanNpc(index, 10)
		SpawnHotanNpc(index, 11)
		SpawnHotanNpc(index, 12)
		SpawnHotanNpc(index, 13)
		SpawnHotanNpc(index, 14)
		SpawnHotanNpc(index, 15)
		SpawnHotanNpc(index, 16)
		SpawnHotanNpc(index, 17)
		SpawnHotanNpc(index, 18)
		SpawnHotanNpc(index, 19)
		SpawnHotanNpc(index, 20)
		SpawnHotanNpc(index, 21)
		SpawnHotanNpc(index, 22)
		SpawnHotanNpc(index, 23)
		SpawnHotanNpc(index, 24)
		SpawnHotanNpc(index, 25)
		SpawnNPC4(index, 42)
	End Function
End Module