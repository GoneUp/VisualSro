Option Strict Off
Option Explicit On
Module modGM
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
	
	Public Function SendNotice(ByRef sMessage As String) As Object
		
		fData = "6736"
		fData = fData & "0000"
		fData = fData & "07"
		fData = fData & WordFromInteger(Len(sMessage))
		
		Dim sTemp As String
		For i = 1 To Len(sMessage)
			'Fuck unicode.
			sTemp = sTemp & cv_HexFromString(Mid(sMessage, i, 1)) & "00"
		Next i
		fData = fData & sTemp
		
		pLen = (Len(fData) - 8) / 2
		fData = WordFromInteger(pLen) & fData
		
		For i = 1 To UBound(PlayerData)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function

    Public Function CreateEquip1(ByRef index As Short, ByRef item As Short, ByRef plus As Short, ByRef Slot As Short) As Object

        Dim hplus As String
        Dim hslot As String
        Dim additem As String
        Dim addplus As String

        'Creates equipment only '.item <itemid> <plusvalue> <inventoryslot>'

        additem = CStr(item)
        addplus = CStr(plus)
        For i = 13 To 45
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If CharItems(index, i).ID = 0 Then
                Slot = i
                i = 45
            End If
        Next i

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite("CH", "type", "item" & Slot, PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite("1", "amount", "item" & Slot, PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(additem, "ID", "item" & Slot, PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(addplus, "plusvalue", "item" & Slot, PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite("100", "durability", "item" & Slot, PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite("0", "phyreinforce", "item" & Slot, PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite("0", "magreinforce", "item" & Slot, PlayerData(index).CharPath)

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, Slot).Type = "CH"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, Slot).amount = 1
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, Slot).ID = item
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, Slot).PlusValue = plus
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, Slot).Durability = 10
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, Slot).PhyReinforce = 0
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, Slot).MagReinforce = 0


        hplus = Hex(plus)
        hslot = Hex(Slot)
        If Len(hplus) = 1 Then hplus = "0" & hplus
        If Len(hslot) = 1 Then hslot = "0" & hslot

        fData = "2500"
        fData = fData & "6DB0"
        fData = fData & "0000"
        fData = fData & "0106"
        fData = fData & hslot
        fData = fData & DWordFromInteger(item)
        fData = fData & hplus
        fData = fData & "2298019C00000000" '?
        fData = fData & "310000000250000000140000000500000004000000"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function

    Public Function SetWeather(ByRef Weather As Short, ByRef Intensity As Short) As Object

        fData = "DE3B"
        fData = fData & "0000"
        fData = fData & "0" & Weather '1 = Normal, 2 = Rain, 3 = snow
        fData = fData & ByteFromInteger(Intensity) 'From 00(nothing) to FF(ultra^^)
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function

    Public Function ChangeSpeed(ByRef index As Short, ByRef Speed As Double) As Object

        fData = "0C00"
        fData = fData & "6F37"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "00008041" 'Playerspeed while walking
        fData = fData & Inverse(Float2Hex(Speed))

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).RunSpeed = Speed

    End Function
End Module