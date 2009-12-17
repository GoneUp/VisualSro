Option Strict Off
Option Explicit On
Module modParty
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
	
	Public Function Handle_Party_Delete(ByRef index As Short, ByRef data As String) As Object
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PartyOwner konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		PlayerData(index).PartyOwner = False
		PartyExist(IntegerFromWord(Mid(data, 1, 4))) = False
		
		fData = "35B5" & "0000"
		fData = fData & "01"
		fData = fData & Mid(data, 1, 4) & "0000" 'PartyNr.
		
		pLen = (Len(fData) - 8) / 2
		fData = WordFromInteger(pLen) & fData
		For i = 1 To UBound(PlayerData)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i


    End Function

    Public Function Handle_Party_Edit(ByRef index As Short, ByRef data As String) As Object

        fData = "DCB3" & "0000"
        fData = fData & "01"
        fData = fData & Mid(data, 1, 4) & "0000" 'PartyNr
        fData = fData & "00000000"
        fData = fData & Mid(data, 17, 2) 'Values for Exp-share/distrib. and "Only-master-invite"
        fData = fData & Mid(data, 19, 2) 'PartyType (Hunting, Quest, Thief or Trade)
        fData = fData & Mid(data, 21, 2) 'Min lvl
        fData = fData & Mid(data, 23, 2) 'Max lvl
        fData = fData & Mid(data, 25, 4) 'Len of Party title
        fData = fData & Mid(data, 29, Len(data) - 24) 'Party name

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i


    End Function

    Public Function Handle_Party_Form(ByRef index As Short, ByRef data As String) As Object

        PartyCount = PartyCount + 1 'Increase the PartyNr.
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).PartyNr konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).PartyNr = WordFromInteger(PartyCount)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PartyOwner konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).PartyOwner = True
        PartyExist(PartyCount) = True

        fData = "FFB6" & "0000"
        fData = fData & "01"
        fData = fData & WordFromInteger(PartyCount) & "0000"
        fData = fData & "00000000"
        fData = fData & Mid(data, 17, 2) 'Values for Exp-share/distrib. and "Only-master-invite"
        fData = fData & Mid(data, 19, 2) 'PartyType (Hunting, Quest, Thief or Trade)
        fData = fData & Mid(data, 21, 2) 'Min lvl
        fData = fData & Mid(data, 23, 2) 'Max lvl
        fData = fData & Mid(data, 25, 4) 'Len of Party title
        fData = fData & Mid(data, 29, Len(data) - 24) 'Party title

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function
    Public Function Party_Request(ByRef index As Short) As Object
        Dim ObjectID As String
        ObjectID = Left(sData, 8)

        MsgBox(sData)
        fData = "9333" & "0000"
        fData = fData & "02"
        fData = fData & ObjectID
        fData = fData & "05"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function
    Public Function Party_answer(ByRef index As Short) As Object

        Dim ObjectID As String
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().SelectedID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ObjectID = PlayerData(index).SelectedID
        'For i = 1 To UBound(PlayerData)
        'If PlayerData(i).CharID = ObjectID Then
        'PlayerData(index).PTTargetWinSock = i
        'PlayerData(i).PTTargetWinSock = index
        'PlayerData(i).PTTargetname = PlayerData(index).PTTargetname
        'Exit For
        'End If
        'Next i

        Select Case Mid(sData, 1, 2)
            Case "01" 'accpt
                fData = "52B4" & "0000"
                fData = fData & "01"
                fData = fData & ObjectID
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData
                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i
                fData = "D5B0" & "0000"
                fData = fData & "01"
                fData = fData & ObjectID
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData
                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i
                fData = "D635" & "0000"
                fData = fData & "FF"
                fData = fData & ObjectID 'pt targetýd
                fData = fData & "0501" 'pt type
                fData = fData & "FF"
                fData = fData & ObjectID 'pt targetId
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & WordFromInteger(Len(PlayerData(index).Charname))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & cv_HexFromString(PlayerData(index).Charname)
                fData = fData & "7507000044AA516C0F00F402D604010001000000041201000013010000" '?
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData
                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i
                'fData = "D635" & "0000"
                'fData = fData & "FF"
                'fData = fData & "F70C2400" 'pt targetýd
                'fData = fData & "0501" 'pt type
                'fData = fData & "FF"
                'fData = fData & "F70C2400" 'pt targetId
                'fData = fData & "0700" ' charname len
                'fData = fData & "467265655F7472" ' char name
                'fData = fData & "7507000044AA516C0F00F402D604010001000000041201000013010000" '?
                'pLen = (Len(fData) - 8) / 2
                'fData = WordFromInteger(pLen) & fData
                'modGlobal.GameSocket(PlayerData(index).PTTargetWinSock).SendData cv_StringFromHex(fData)
                fData = "583E" & "0000"
                fData = fData & "02"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & "FF" & PlayerData(index).CharID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PTTargetname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & WordFromInteger(Len(PlayerData(index).PTTargetname))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PTTargetname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & cv_HexFromString(PlayerData(index).PTTargetname)
                fData = fData & "803900001AAA506C7307F302D004010001000000040102000000000000"
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData
                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i
                'fData = "AD31" & "0000"
                'fData = fData & "3A01050D"
                'pLen = (Len(fData) - 8) / 2
                'fData = WordFromInteger(pLen) & fData
                'For i = 1 To UBound(PlayerData)
                'If PlayerData(i).Ingame = True Then
                'modGlobal.GameSocket(i).SendData cv_StringFromHex(fData)
                'End If
                'Next i
            Case "02" 'cancel
                fData = "9333" & "0000"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = "9333" & PlayerData(index).CharID
                fData = fData & "02"
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
        End Select
    End Function
End Module