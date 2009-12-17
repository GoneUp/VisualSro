Option Strict Off
Option Explicit On
Module modPlayers
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
	
	Public Function BroadCastPlayerData(ByRef index As Short) As Object
		
		fData = "D730"
		fData = fData & "0000"
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Chartype konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & DWordFromInteger(PlayerData(index).Chartype)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Volume konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & ByteFromInteger(PlayerData(index).Volume)
		fData = fData & "01"
		
		fData = fData & ByteFromInteger(45) 'Max item slot
		fData = fData & ByteFromInteger(ListItemAmount(index)) 'Amount of items
		fData = fData & ListItemData(index)
		
		fData = fData & "040000" 'avatar CharItems(04 start 00 number of items 00 end)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & PlayerData(index).CharID
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & ByteFromInteger(PlayerData(index).XSection)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & ByteFromInteger(PlayerData(index).YSection)
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & Inverse(Float2Hex(((PlayerData(index).XPos) - ((PlayerData(index).XSection) - 135) * 192) * 10))
		fData = fData & "00000000"
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & Inverse(Float2Hex((PlayerData(index).YPos - ((PlayerData(index).YSection) - 92) * 192) * 10))
		fData = fData & "A474"
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Walking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		If PlayerData(index).Walking = True Then
			fData = fData & "0101"
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			fData = fData & ByteFromInteger(PlayerData(index).XSection)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			fData = fData & ByteFromInteger(PlayerData(index).YSection)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkingX konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			fData = fData & Inverse(WordFromInteger((PlayerData(index).WalkingX) - ((PlayerData(index).XSection) - 135) * 192)) 'X walking to coords
			fData = fData & "0000" 'Z
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkingY konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			fData = fData & Inverse(WordFromInteger((PlayerData(index).WalkingY) - ((PlayerData(index).YSection) - 92) * 192)) 'Y walking to coords
			fData = fData & "01"
		Else
			fData = fData & "000100"
			fData = fData & "A474"
		End If
		
		fData = fData & "0100"
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Berserking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		If PlayerData(index).Berserking = True Then
			fData = fData & "01"
		Else
			fData = fData & "00"
		End If
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & Inverse(Float2Hex(PlayerData(index).WalkSpeed)) 'Playerspeed while walking
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & Inverse(Float2Hex(PlayerData(index).RunSpeed)) 'Playerspeed while running
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & Inverse(Float2Hex(PlayerData(index).BerserkSpeed)) 'Playerspeed while berserk
		
		'Buff list
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ActiveBuffs konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & ByteFromInteger(PlayerData(index).ActiveBuffs)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ActiveBuffs konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		For x = 1 To PlayerData(index).ActiveBuffs
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BuffList konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			fData = fData & PlayerData(index).BuffList(x)
		Next x
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & WordFromInteger(Len(PlayerData(index).Charname))
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & cv_HexFromString(PlayerData(index).Charname)
		
		' fData = fData & "000100"
		'If PlayerData(index).Transport <> "" Then
		'fData = fData & "0100"
		' Else
		'fData = fData & "000000"
		' End If
		
		'If PlayerData(index).Transport <> "" Then
		'fData = fData & PlayerData(index).TransportID
		'End If
		
		'This data contains guildname, etc... Unsupported for now.
		fData = fData & "000100000000000000000000000000000000000000000000000000000000FF01"
		pLen = (Len(fData) - 8) / 2
		fData = WordFromInteger(pLen) & fData
		
		For i = 1 To UBound(PlayerData)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If i <> index And PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function

    Public Function SpawnPlayerObjects(ByRef index As Short) As Object

        Dim Walking As String 'only temp.
        For i = 1 To UBound(PlayerData)
            If i <> index Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Transport konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Transport <> "" Then
                    'spawn transport
                    fData = "D730"
                    fData = fData & "0000"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Transport konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & PlayerData(i).Transport
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().TransportID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & PlayerData(i).TransportID
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(ByteFromInteger(PlayerData(i).XSection)) 'X sector
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(ByteFromInteger(PlayerData(i).YSection)) 'ySector
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(Float2Hex((PlayerData(i).XPos - ((PlayerData(i).XSection) - 135) * 192) * 10)) 'X
                    fData = fData & "00000000" 'Z
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(Float2Hex((PlayerData(i).YPos - ((PlayerData(i).YSection) - 92) * 192) * 10)) 'Y
                    fData = fData & "DC72"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Walking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Walking = True Then
                        fData = fData & "0101"
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & ByteFromInteger(PlayerData(i).XSection)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & ByteFromInteger(PlayerData(i).YSection)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkingX konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(WordFromInteger((PlayerData(i).WalkingX) - ((PlayerData(i).XSection) - 135) * 192)) 'X walking to coords
                        fData = fData & "0000" 'Z
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkingY konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(WordFromInteger((PlayerData(i).WalkingY) - ((PlayerData(i).YSection) - 92) * 192)) 'Y walking to coords
                        fData = fData & "01"
                    Else
                        fData = fData & "000100"
                        fData = fData & "A474"
                    End If
                    fData = fData & "0100"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Berserking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Berserking = True Then
                        fData = fData & "01"
                    Else : fData = fData & "00"
                    End If
                    fData = fData & "00003442" 'Tranport speed while walking
                    fData = fData & "0000B442" 'Runnning
                    fData = fData & "0000C842" 'Beserk
                    fData = fData & "0000"
                    fData = fData & "01"
                    fData = WordFromInteger((Len(fData) - 8) / 2) & fData
                    modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                End If


                'spawn growth Pet
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).GrowthPetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).GrowthPetID <> "" Then
                    fData = "D730"
                    fData = fData & "0000"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPet konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & DWordFromInteger(PlayerData(i).GrowthPet)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & PlayerData(i).GrowthPetID

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & ByteFromInteger(PlayerData(i).XSection) 'X sector
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & ByteFromInteger(PlayerData(i).YSection) 'ySector
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetXPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(Float2Hex(((PlayerData(i).GrowthPetXPos) - ((PlayerData(i).XSection) - 135) * 192) * 10)) 'X
                    fData = fData & "00000000" 'Z
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetYPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(Float2Hex(((PlayerData(i).GrowthPetYPos) - ((PlayerData(i).YSection) - 92) * 192) * 10)) 'Y
                    fData = fData & "DC72"
                    fData = fData & "00" '01 walking
                    fData = fData & "01"
                    If Walking = "01" Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(ByteFromInteger(PlayerData(i).XSection)) 'X sector
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(ByteFromInteger(PlayerData(i).YSection)) 'ySector
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetXPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(WordFromInteger((PlayerData(i).GrowthPetXPos) - ((PlayerData(i).XSection) - 135) * 192)) 'X
                        fData = fData & "0000" 'Z
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetYPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(WordFromInteger((PlayerData(i).GrowthPetYPos) - ((PlayerData(i).YSection) - 92) * 192)) 'Y
                    End If

                    fData = fData & "00"
                    fData = fData & "DC72"
                    fData = fData & "0100"
                    fData = fData & "02" 'Berserker
                    fData = fData & "00004842" 'Playerspeed while walking
                    fData = fData & "00009642" 'Playerspeed while running
                    fData = fData & "0000C842" 'Playerspeed while berserk
                    fData = fData & "0000"
                    fData = fData & "0000" 'pet name len
                    'fData = fData & cv_StringFromHex("PetName")'pet name
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & WordFromInteger(Len(PlayerData(i).Charname))
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & cv_HexFromString(PlayerData(i).Charname)
                    fData = fData & "0000"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & PlayerData(i).CharID
                    fData = fData & "01"

                    fData = WordFromInteger((Len(fData) - 8) / 2) & fData
                    modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                End If


                'Spawn Player
                fData = "D730"
                fData = fData & "0000"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Chartype konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & DWordFromInteger(PlayerData(i).Chartype)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Volume konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & ByteFromInteger(PlayerData(i).Volume)
                fData = fData & "01" '01

                fData = fData & ByteFromInteger(45) 'Max item slot
                fData = fData & ByteFromInteger(ListItemAmount(i)) 'Amount of items
                fData = fData & ListItemData(i)

                fData = fData & "040000" 'avatar CharItems(04 start 00 number of items 00 end)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & PlayerData(i).CharID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & ByteFromInteger(PlayerData(i).XSection)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & ByteFromInteger(PlayerData(i).YSection)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & Inverse(Float2Hex(((PlayerData(i).XPos) - ((PlayerData(i).XSection) - 135) * 192) * 10))
                fData = fData & "00000000"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & Inverse(Float2Hex((PlayerData(i).YPos - ((PlayerData(i).YSection) - 92) * 192) * 10))
                fData = fData & "A474"

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Walking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Walking = True Then
                    fData = fData & "0101"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & ByteFromInteger(PlayerData(i).XSection)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & ByteFromInteger(PlayerData(i).YSection)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkingX konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(WordFromInteger((PlayerData(i).WalkingX) - ((PlayerData(i).XSection) - 135) * 192)) 'X walking to coords
                    fData = fData & "0000" 'Z
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkingY konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(WordFromInteger((PlayerData(i).WalkingY) - ((PlayerData(i).YSection) - 92) * 192)) 'Y walking to coords
                    fData = fData & "01"
                Else
                    fData = fData & "000100"
                    fData = fData & "A474"
                End If

                fData = fData & "0100"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Berserking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Berserking = True Then
                    fData = fData & "01"
                Else : fData = fData & "00"
                End If

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & Inverse(Float2Hex(PlayerData(i).WalkSpeed)) 'Playerspeed while walking
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & Inverse(Float2Hex(PlayerData(i).RunSpeed)) 'Playerspeed while running
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & Inverse(Float2Hex(PlayerData(i).BerserkSpeed)) 'Playerspeed while berserk

                'Buff list
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ActiveBuffs konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & ByteFromInteger(PlayerData(i).ActiveBuffs)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).ActiveBuffs konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                For x = 1 To PlayerData(i).ActiveBuffs
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BuffList konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & PlayerData(i).BuffList(x)
                Next x

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & WordFromInteger(Len(PlayerData(i).Charname))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & cv_HexFromString(PlayerData(i).Charname)

                'fData = fData & "000100"
                'If PlayerData(i).Transport <> "" Then
                'fData = fData & "0100"
                'Else
                'fData = fData & "000000"
                'End If

                'If PlayerData(i).Transport <> "" Then
                'fData = fData & PlayerData(i).TransportID
                'End If

                'This data contains guildname, etc... Unsupported for now.
                fData = fData & "000100000000000000000000000000000000000000000000000000000000FF01"
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                End If

            End If

        Next i

        'Dim n As Integer
        'For n = 1 To UBound(NPCList)
        'If NPCList(n).ID <> "" Then
        '  fData = "0300CB300000"
        '  fData = fData & "010100"
        '  modGlobal.GameSocket(index).SendData cv_StringFromHex(fData)

        '  fData = "17340000"
        '  fData = fData & NPCList(n).NPCID
        '  fData = fData & NPCList(n).ID
        '  fData = fData & NPCList(n).xy 'X sector
        '  fData = fData & NPCList(n).xx 'ySector
        '  fData = fData & "00000000" 'Z
        'fData = fData & NPCList(n).zz
        '  fData = fData & NPCList(n).zz
        '  fData = fData & NPCList(n).zone
        '  fData = fData & "010000"
        ' fData = fData & "00000000000000000000C842"
        '  fData = fData & "0000"
        ' fData = WordFromInteger(((Len(fData) - 8) / 2)) & fData
        ' modGlobal.GameSocket(index).SendData cv_StringFromHex(fData)


        ' fData = "00000A330000"
        ' modGlobal.GameSocket(index).SendData cv_StringFromHex(fData)


        'End If
        'Next n

    End Function

    Public Function HandleMovement(ByRef index As Short) As Object

        Dim DistanceX As Double
        Dim DistanceY As Double
        Dim Distance As Double

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PickingItem konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).PickingItem = 0
        frmMain.PickupDelay(index).Enabled = False
        frmMain.AttackDelay(index).Enabled = False
        frmMain.WalkAttackDelay(index).Enabled = False
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Attacking = False
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).AttackingID = ""

        'No support for sky-clicking.
        Dim xsect As Short
        Dim ysect As Short
        Dim XPos As Short
        Dim YPos As Short
        If Mid(sData, 1, 2) = "01" Then '00 = sky


            XPos = IntegerFromWord(Mid(sData, 7, 4))
            'MsgBox (IntegerFromWord(Mid(sData, 11, 4)))
            YPos = IntegerFromWord(Mid(sData, 15, 4))

            xsect = CShort("&H" & Mid(sData, 3, 2))
            ysect = CShort("&H" & Mid(sData, 5, 2))
            'MsgBox (sData)

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).XSection = xsect
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).YSection = ysect
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).XPos = (xsect - 135) * 192 + CShort(XPos) / 10
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).YPos = (ysect - 92) * 192 + CShort(YPos) / 10


            fData = "0E00"
            fData = fData & "38B7"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).CharID
            fData = fData & Right(sData, 18) & "00" 'Destination

            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

            'Move Pets
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GrowthPetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(index).GrowthPetID <> "" Then

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GrowthPetXPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(index).XPos < PlayerData(index).GrowthPetXPos Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GrowthPetXPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    DistanceX = PlayerData(index).XPos - PlayerData(index).GrowthPetXPos
                Else
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GrowthPetXPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    DistanceX = PlayerData(index).XPos - PlayerData(index).GrowthPetXPos
                End If

                If DistanceX < 0 And DistanceX <> 0 Then
                    Distance = DistanceX * -1
                Else
                    Distance = DistanceX
                End If

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GrowthPetYPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(index).YPos < PlayerData(index).GrowthPetYPos Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GrowthPetYPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    DistanceY = PlayerData(index).YPos - PlayerData(index).GrowthPetYPos
                Else
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GrowthPetYPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    DistanceY = PlayerData(index).YPos - PlayerData(index).GrowthPetYPos
                End If

                If DistanceY < 0 And DistanceY <> 0 Then
                    Distance = DistanceY * -1 + Distance
                Else
                    Distance = Distance + DistanceY
                End If

                If Distance > 6 Then

                    'set speed so pet doesnt run to far infront
                    fData = "0C00"
                    fData = fData & "6F37"
                    fData = fData & "0000"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & PlayerData(index).GrowthPetID
                    fData = fData & "00008041" 'Playerspeed while walking
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(Float2Hex(PlayerData(index).RunSpeed))

                    For i = 1 To UBound(PlayerData)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If PlayerData(i).Ingame = True Then
                            modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                        End If
                    Next i

                    DistanceX = DistanceX - ((DistanceX * (5)) / Distance)
                    DistanceY = DistanceY - ((DistanceY * (5)) / Distance)
                    'build walk packet
                    fData = "0E00"
                    fData = fData & "38B7"
                    fData = fData & "0000"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & PlayerData(index).GrowthPetID
                    fData = fData & "01"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetXPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & WordFromInteger(((PlayerData(index).GrowthPetXPos + DistanceX) - (PlayerData(index).XSection - 135) * 192) * 10) 'X
                    fData = fData & "0000" 'Z
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetYPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & WordFromInteger(((PlayerData(index).GrowthPetYPos + DistanceY) - ((PlayerData(index).YSection - 92)) * 192) * 10) 'Y
                    fData = fData & "00"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GrowthPetXPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetXPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).GrowthPetXPos = PlayerData(index).GrowthPetXPos + DistanceX
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GrowthPetYPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GrowthPetYPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).GrowthPetYPos = PlayerData(index).GrowthPetYPos + DistanceY
                    For i = 1 To UBound(PlayerData)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If PlayerData(i).Ingame = True Then
                            modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                        End If
                    Next i

                End If

            End If

        End If

    End Function
    Public Function QuitGame(ByRef index As Short, ByRef sData As String) As Object

        'Despawn for all other users.
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = "0400AB360000" & PlayerData(index).CharID
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If i <> index And PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(i).Ingame = False
            End If
        Next i

        'Write our latest known location.
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(PlayerData(index).XSection), "XSection", "character", PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(PlayerData(index).YSection), "YSection", "character", PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(PlayerData(index).XPos), "XPos", "character", PlayerData(index).CharPath)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(PlayerData(index).YPos), "YPos", "character", PlayerData(index).CharPath)
        'Give client a countdown...
        fData = "0300B7B00000"
        fData = fData & "0105" & sData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        'Start countdowns for quit.
        frmMain.tmrQuit.Load(index)
        frmMain.tmrQuit(index).Enabled = True
    End Function

    Public Function CheckCharacterName(ByRef index As Short, ByRef data As String) As Object

        'Should check if character doesn't already exist.
        fData = "0200f7b200000401"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function

    Public Function CreateCharacter(ByRef index As Short, ByRef data As String) As Object

        Dim charerror As String
        Dim charcreated As String
        Dim lcName As Short
        Dim cName As String
        'UPGRADE_NOTE: cType wurde aktualisiert auf cType_Renamed. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim cType_Renamed As String
        Dim cVolume As String
        Dim cItem1 As String
        Dim cItem2 As String
        Dim cItem3 As String
        Dim cItem4 As String
        Dim cRace As String

        lcName = IntegerFromWord(Mid(data, 3, 4))
        lcName = lcName * 2
        cName = cv_StringFromHex(Mid(data, 7, lcName))
        cType_Renamed = CStr(IntegerFromWord(Mid(data, lcName + 7, 4)))
        cVolume = CStr(IntegerFromWord(Mid(data, lcName + 15, 2)))
        cItem1 = CStr(IntegerFromWord(Mid(data, lcName + 17, 4)))
        cItem2 = CStr(IntegerFromWord(Mid(data, lcName + 25, 4)))
        cItem3 = CStr(IntegerFromWord(Mid(data, lcName + 33, 4)))
        cItem4 = CStr(IntegerFromWord(Mid(data, lcName + 41, 4)))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Username konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).CharPath = (Replace(My.Application.Info.DirectoryPath, "\", "/") & "/accounts/" & PlayerData(index).Username & ".ini")

        If CDbl(cType_Renamed) >= 1911 And CDbl(cType_Renamed) <= 1932 Then cRace = CStr(0) Else cRace = CStr(1) '0 = chinese 1 = euro

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If iniGetStr("password", "account", PlayerData(index).CharPath) = "(error)" Then

            fData = "0300F7B20000010204"
            modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
            Exit Function

        Else

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(cName, "name", "character", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(cRace, "Race", "character", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(cType_Renamed, "chartype", "character", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(cVolume, "volume", "character", PlayerData(index).CharPath)

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("CH", "type", "item13", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("1", "amount", "item13", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(cItem1, "ID", "item13", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "plusvalue", "item13", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("100", "durability", "item13", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "phyreinforce", "item13", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "magreinforce", "item13", PlayerData(index).CharPath)

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("CH", "type", "item14", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("1", "amount", "item14", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(cItem2, "ID", "item14", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "plusvalue", "item14", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("100", "durability", "item14", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "phyreinforce", "item14", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "magreinforce", "item14", PlayerData(index).CharPath)

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("CH", "type", "item15", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("1", "amount", "item15", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(cItem3, "ID", "item15", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "plusvalue", "item15", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("100", "durability", "item15", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "phyreinforce", "item15", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "magreinforce", "item15", PlayerData(index).CharPath)

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("CH", "type", "item16", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("1", "amount", "item16", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(cItem4, "ID", "item16", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "plusvalue", "item16", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("100", "durability", "item16", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "phyreinforce", "item16", PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "magreinforce", "item16", PlayerData(index).CharPath)

        End If

        fData = "0200F7B200000101"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function

    Public Function SelectObject(ByRef index As Short) As Object
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().SelectedID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).SelectedID = ""
        Dim HPMPFlag As String
        Dim MobHP As Integer
        Dim MobMP As Integer
        Dim ObjectID As String
        ObjectID = Left(sData, 8)
        'MsgBox (IntegerFromWord(ObjectID))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).SelectedID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).SelectedID = ObjectID Then
            Exit Function
        End If

        For i = 1 To UBound(NPCList)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts NPCList(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If ObjectID = NPCList(i).ID Then
                Call SelectNPC(index, i)
                Exit Function
            End If
        Next i

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).CharID = ObjectID Then
                    HPMPFlag = "10"
                    Exit For
                Else
                    For x = 1 To UBound(Mobs)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If Mobs(x).ID = ObjectID Then
                            HPMPFlag = "01" 'need to check if its a mob or not
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            MobHP = Mobs(x).HP 'then need to get the mob HP and MP
                            MobMP = 1
                            Exit For
                        End If
                    Next x
                End If
            End If
        Next i

        fData = "5AB4"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & ObjectID
        fData = fData & HPMPFlag 'If Its a mob this will be 01 then hp and mp after it
        Select Case HPMPFlag
            Case "01" 'Mob
                fData = fData & Inverse(DecToHexLong(MobHP))
                fData = fData & Inverse(DecToHexLong(MobMP))
            Case "10" 'Player
                fData = fData & "0000"
                fData = fData & "0004"
        End Select
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().SelectedID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).SelectedID = ObjectID

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
    End Function

    Public Function HandleEmote(ByRef index As Short) As Object

        fData = "0500"
        fData = fData & "4B32"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & sData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function

    Public Function HandleState(ByRef index As Short, ByRef data As String) As Object

        If Len(data) > 2 Then GoTo unsupported
        If data = "04" Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).State konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If IntegerFromWord(data & "00") = PlayerData(index).State Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().State konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).State = 0
            Else
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).State konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).State = IntegerFromWord(data & "00")
            End If
        Else
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).State konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).State = IntegerFromWord(data & "00")
        End If

        fData = "0600"
        fData = fData & "2231"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().State konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & "01" & Mid(WordFromInteger(PlayerData(index).State), 1, 2)

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

unsupported:
        Debug.Print("Function : HandleState Data : " & data & " Not Supported!")

    End Function

    Public Function HandleBerserk(ByRef index As Short, ByRef Start As Boolean) As Object

        If Start = True Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkBar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).BerserkBar = 5
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = "060022310000" & PlayerData(index).CharID & "0401"
            frmMain.BerserkTimer(index).Enabled = True
            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

            fData = "0C00"
            fData = fData & "6F37"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).CharID
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(PlayerData(index).WalkSpeed))
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(PlayerData(index).BerserkSpeed))
            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

        Else
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = "060022310000" & PlayerData(index).CharID & "0400"
            frmMain.BerserkTimer(index).Enabled = False
            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

            fData = "0C00"
            fData = fData & "6F37"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).CharID
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().WalkSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(PlayerData(index).WalkSpeed))
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(PlayerData(index).RunSpeed))
            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

        End If

    End Function

    Public Function HandleLight(ByRef index As Short, ByRef data As String) As Object

        'Should save this to file and extract it on start-up.
        fData = "0500"
        fData = fData & "83B6"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID & data

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function

    Public Function UpdateStats(ByRef index As Short) As Object

        fData = "343C"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MinPhyAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MaxPhyAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinMagAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MinMagAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxMagAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MaxMagAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PhyDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).PhyDef)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MagDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).MagDef)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Hit konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Hit)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Parry konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Parry)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).HP)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MP)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Strength konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Strength)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Intelligence konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Intelligence)
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function

    Public Function StartBuff(ByRef index As Short) As Object

        Dim SkillOverID As String

        fData = "05B5"
        fData = fData & "0000"
        fData = fData & "01"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CastingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CastingID
        fData = fData & "00"
        fData = fData & "00000000"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "0C00"
        fData = fData & "19B4"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CastingSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CastingSkillID
        SkillOverID = (Inverse(WordFromInteger(Int(Rnd() * 1048575) + 65536))) & "000"
        fData = fData & SkillOverID

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i


        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ActiveBuffs konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ActiveBuffs konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).ActiveBuffs = PlayerData(index).ActiveBuffs + 1
        For i = 1 To 20 'i think 20 is the most ammount of buffs you can have at one time
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BuffList konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(index).BuffList(i) = "" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BuffList konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CastingSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).BuffList(i) = PlayerData(index).CastingSkillID & SkillOverID
            End If
        Next i
        'MsgBox (sData)
        'MsgBox ("test")
        Call Speeds(index)
    End Function
    Public Function Speeds(ByRef index As Short) As Object
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CastingSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Dim xsect As Short
        Dim ysect As Short
        Dim XPos As Short
        Dim YPos As Short
        Dim zpos As Short
        If PlayerData(index).CastingSkillID = "72000000" Then

            XPos = IntegerFromWord(Mid(sData, 19, 4))
            zpos = IntegerFromWord(Mid(sData, 22, 4))
            YPos = IntegerFromWord(Mid(sData, 27, 4))
            xsect = CShort("&H" & Mid(sData, 15, 2))
            ysect = CShort("&H" & Mid(sData, 17, 2))

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).XSection = xsect
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).YSection = ysect
            'PlayerData(index).XPos = (xsect - 135) * 192 + CInt(XPos) / 10
            'PlayerData(index).YPos = (ysect - 92) * 192 + CInt(YPos) / 10
            'MsgBox (sData)
            'sect = Mid(sData, 15, 4)

            fData = "E330"
            fData = fData & "0000"
            fData = fData & "01"
            fData = fData & Inverse(ByteFromInteger(xsect)) 'X sector
            fData = fData & Inverse(ByteFromInteger(ysect)) 'ySector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & WordFromInteger(((PlayerData(index).XPos + XPos) - (xsect - 135) * 192) * 10) 'X
            fData = fData & WordFromInteger(zpos) 'Z
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & WordFromInteger(((PlayerData(index).YPos + YPos) - ((ysect - 92)) * 192) * 10) 'Y
            fData = fData & "00"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).CharID
            pLen = (Len(fData) - 8) / 2
            fData = WordFromInteger(pLen) & fData
            MsgBox(fData)
            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

        End If
    End Function

    Public Function EndBuff(ByRef index As Short) As Object

        Dim SkillOverID As String
        'SkillOverID = Right(PlayerData(index).BuffList(Buff), 8)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CastingSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        SkillOverID = PlayerData(index).CastingSkillID
        fData = "A0B6"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & SkillOverID
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "0200"
        fData = fData & "CDB2"
        fData = fData & "0000"
        fData = fData & "0101"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "0200"
        fData = fData & "CDB2"
        fData = fData & "0000"
        fData = fData & "0200"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
        'PlayerData(index).BuffList(Buff) = ""
        'PlayerData(index).ActiveBuffs = PlayerData(index).ActiveBuffs - 1

    End Function
    Public Function insertstr(ByRef index As Short) As Object
        fData = "0100"
        fData = fData & "7AB2"
        fData = fData & "0000"
        fData = fData & "01"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "2400"
        fData = fData & "3C34"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MinPhyAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MaxPhyAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinMagAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MinMagAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxMagAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MaxMagAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PhyDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).PhyDef)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MagDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).MagDef)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Hit konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Hit)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Parry konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Parry)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).HP + Val(CStr(10)))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).HP = PlayerData(index).HP + 10
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MP)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Strength konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Strength + 1)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Strength konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Strength konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Strength = PlayerData(index).Strength + 1
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Intelligence konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Intelligence)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).AttributePoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttributePoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).AttributePoints = PlayerData(index).AttributePoints - 1
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


        'modGlobal.GameSocket(index).SendData cv_StringFromHex("0100B27A000001")
    End Function
    Public Function insertint(ByRef index As Short) As Object
        fData = "0100"
        fData = fData & "52B5"
        fData = fData & "0000"
        fData = fData & "01"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "2400"
        fData = fData & "3C34"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MinPhyAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MaxPhyAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinMagAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MinMagAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxMagAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MaxMagAtk)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PhyDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).PhyDef)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MagDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).MagDef)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Hit konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Hit)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Parry konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Parry)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).HP)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).MP + Val(CStr(10)))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).MP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).MP = PlayerData(index).MP + 10
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Strength konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Strength)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Intelligence konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & WordFromInteger(PlayerData(index).Intelligence + 1)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Intelligence konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Intelligence konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Intelligence = PlayerData(index).Intelligence + 1
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).AttributePoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttributePoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).AttributePoints = PlayerData(index).AttributePoints - 1
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        'modGlobal.GameSocket(index).SendData cv_StringFromHex("0100B552000001")
    End Function
    Public Function HonorRank(ByRef index As Short) As Object
        fData = "D0B2"
        fData = fData & "0000"
        fData = fData & "0100"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
    End Function
End Module