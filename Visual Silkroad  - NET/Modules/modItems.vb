Option Strict Off
Option Explicit On
Module modItems
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
	
	Public Function BuildItemPackets(ByRef index As Short) As Object
		
		'Build Packet Data
		Dim fData As String
		Dim gData As String
		
		ItemAmount(index) = 0
		ListItemAmount(index) = 0
		'Loop through array.
		For i = 0 To UBound(CharItems, 2)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, i).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If CharItems(index, i).Type <> "" Then
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, i).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If CharItems(index, i).Type = "ETC" Then
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					fData = fData & ByteFromInteger(i) & DWordFromInteger(CharItems(index, i).ID) & WordFromInteger(CharItems(index, i).amount)
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, i).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				ElseIf CharItems(index, i).Type = "CH" Then 
					fData = fData & ByteFromInteger(i)
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					fData = fData & DWordFromInteger(CharItems(index, i).ID)
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					fData = fData & ByteFromInteger(CharItems(index, i).PlusValue)
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					fData = fData & WordFromInteger(CharItems(index, i).PhyReinforce * 10) 'Physical reinforce percentage.
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					fData = fData & WordFromInteger(CharItems(index, i).MagReinforce * 10) 'Magical reinforce? Doesn't seem to work (on some items?)
					fData = fData & "0000" '?
					fData = fData & "0000" '?
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					fData = fData & DWordFromInteger(CharItems(index, i).Durability)
					fData = fData & "00" 'Blue stats
				End If
				ItemAmount(index) = ItemAmount(index) + 1 'Keep track of how many items we have.
				'Also build itemlist data
				If i = 1 Or i = 2 Or i = 3 Or i = 4 Or i = 5 Or i = 6 Or i = 7 Or i = 8 Then
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					gData = gData & DWordFromInteger(CharItems(index, i).ID) & ByteFromInteger(CharItems(index, i).PlusValue)
					ListItemAmount(index) = ListItemAmount(index) + 1
				End If
			End If
		Next i
		
        'modGlobal.ItemData_Renamed(index) = fData
        modGlobal.ItemData(index) = fData
		ListItemData(index) = gData
	End Function
	
	Public Function HandleItemMovement(ByRef index As Short) As Object
		
		Dim MoveType As String
		Dim fSlot As Short 'from slot
		Dim tslot As Short 'to slot
		Dim Quantity As Short
		Dim itemid As String
		Dim fItemData As String
		Dim tItemData As String
		Dim Type3 As String
		Dim MinPhy As Short
		Dim MaxPhy As Short
		'UPGRADE_WARNING: Arrays in Struktur ItemDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim ItemDataBase As DAO.Recordset
		Dim BookMark As Object
		Dim MinMagA As Short
		Dim MaxMagA As Short
		Dim PhyDef As Short
		Dim MagDef As Short
		Dim Def As Boolean
		Dim Damage As Boolean
		Dim capetype As String
		Dim ilvl As Short
		MoveType = Mid(sData, 1, 2)
		
		fSlot = CShort(HexToDec(Inverse(Mid(sData, 3, 2))))
		tslot = CShort(HexToDec(Inverse(Mid(sData, 5, 2))))
		Quantity = IntegerFromWord(Mid(sData, 7, 4))
		OpenSremuDataBase()
		ItemDataBase = DataBases.OpenRecordset("Items", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
		
		
		Select Case MoveType
			
			Case "00" 'Move Item
				ItemDataBase.index = "ID"
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				ItemDataBase.Seek(">=", CharItems(index, fSlot).ID)
				With ItemDataBase
					ilvl = .Fields("EquipLvl").Value
				End With
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If PlayerData(index).level < ilvl Then
					If tslot > 13 Then
						GoTo ItemMoved
					End If
					fData = "6DB0"
					fData = fData & "0000"
					fData = fData & "0210"
					pLen = (Len(fData) - 8) / 2
					fData = WordFromInteger(pLen) & fData
                    modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                    Exit Function
                Else
ItemMoved:
                    fData = "0700"
                    fData = fData & "6DB0"
                    fData = fData & "0000"
                    fData = fData & "01"
                    fData = fData & MoveType
                    fData = fData & ByteFromInteger(fSlot)
                    fData = fData & ByteFromInteger(tslot)
                    fData = fData & WordFromInteger(Quantity)
                    fData = fData & "00"
                    modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                End If
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If CharItems(index, fSlot).ID = "3726" Or CharItems(index, fSlot).ID = "3727" Or CharItems(index, fSlot).ID = "3728" Or CharItems(index, fSlot).ID = "3729" Or CharItems(index, fSlot).ID = "3730" Or CharItems(index, fSlot).ID = "3731" Or CharItems(index, fSlot).ID = "3732" Or CharItems(index, fSlot).ID = "3733" Or CharItems(index, fSlot).ID = "3734" Then
                    fData = "3434"
                    fData = fData & "0000"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & PlayerData(index).CharID
                    fData = fData & "02010A"
                    fData = WordFromInteger((Len(fData) - 8) / 2) & fData
                    modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                    frmMain.capetimer.Interval = 7000
                    frmMain.capetimer.Enabled = True
                End If
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
                fData = fData & DWordFromInteger(PlayerData(index).MP)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Strength konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & WordFromInteger(PlayerData(index).Strength)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Intelligence konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & WordFromInteger(PlayerData(index).Intelligence)
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                If fSlot < 13 Then 'item is Being Unequiped
                    fData = "0900"
                    fData = fData & "7C37"
                    fData = fData & "0000"
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & PlayerData(index).CharID
                    fData = fData & ByteFromInteger(fSlot)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fData = fData & DWordFromInteger(CharItems(index, fSlot).ID)
                    For i = 1 To UBound(PlayerData)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If PlayerData(i).Ingame = True Then
                            modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                        End If
                    Next i

                ElseIf tslot < 13 Then  'if item is being equiped
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).level < ilvl Then
                        fData = "6DB0"
                        fData = fData & "0000"
                        fData = fData & "0210"
                        pLen = (Len(fData) - 8) / 2
                        fData = WordFromInteger(pLen) & fData
                        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                        frmMain.capetimer.Enabled = False
                        Exit Function
                    Else

                        fData = "0A00"
                        fData = fData & "1433"
                        fData = fData & "0000"
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & PlayerData(index).CharID
                        fData = fData & ByteFromInteger(tslot)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & DWordFromInteger(CharItems(index, fSlot).ID)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & ByteFromInteger(CharItems(index, fSlot).PlusValue)
                        For i = 1 To UBound(PlayerData)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If PlayerData(i).Ingame = True Then
                                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                            End If
                        Next i
                    End If

                    If Quantity = 0 Then Quantity = 1

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If Quantity < CharItems(index, fSlot).amount Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        CharItems(index, fSlot).amount = CharItems(index, fSlot).amount - Quantity
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        CharItems(index, tslot).amount = Quantity
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    ElseIf Quantity = CharItems(index, fSlot).amount Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fItemData = CharItems(index, fSlot).amount
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        tItemData = CharItems(index, tslot).amount
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        CharItems(index, tslot).amount = fItemData
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        CharItems(index, fSlot).amount = tItemData
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    ElseIf Quantity > CharItems(index, fSlot).amount Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        CharItems(index, tslot).amount = Quantity
                    End If

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fItemData = CharItems(index, fSlot).Durability
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tItemData = CharItems(index, tslot).Durability
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).Durability = fItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).Durability = tItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fItemData = CharItems(index, fSlot).ID
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tItemData = CharItems(index, tslot).ID
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).ID = fItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).ID = tItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fItemData = CharItems(index, fSlot).MagReinforce
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tItemData = CharItems(index, tslot).MagReinforce
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).MagReinforce = fItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).MagReinforce = tItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fItemData = CharItems(index, fSlot).PhyReinforce
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tItemData = CharItems(index, tslot).PhyReinforce
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).PhyReinforce = fItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).PhyReinforce = tItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fItemData = CharItems(index, fSlot).PlusValue
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tItemData = CharItems(index, tslot).PlusValue
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).PlusValue = fItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).PlusValue = tItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fItemData = CharItems(index, fSlot).Type
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tItemData = CharItems(index, tslot).Type
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).Type = fItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).Type = tItemData

                End If
            Case "07" 'Drop Item

                fSlot = CShort(HexToDec(Inverse(Mid(sData, 3, 2))))

                fData = "0300"
                fData = fData & "6DB0"
                fData = fData & "0000"
                fData = fData & "01"
                fData = fData & MoveType
                fData = fData & ByteFromInteger(fSlot)

                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                Call DropItem(index, CStr(fSlot))

            Case "24" 'Equip Avatar Item

                fData = "0700"
                fData = fData & "6DB0"
                fData = fData & "0000"
                fData = fData & "01"
                fData = fData & MoveType
                fData = fData & ByteFromInteger(fSlot)
                fData = fData & ByteFromInteger(tslot)
                fData = fData & "0100"
                fData = fData & "00"
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

                fData = "0A00"
                fData = fData & "1433"
                fData = fData & "0000"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & PlayerData(index).CharID
                fData = fData & ByteFromInteger(fSlot)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & DWordFromInteger(CharItems(index, fSlot).ID)
                fData = fData & "00"
                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts AvatarItemList(index, tslot).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                AvatarItemList(index, tslot).ID = CharItems(index, fSlot).ID

                If Quantity = 0 Then Quantity = 1

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Quantity < CharItems(index, fSlot).amount Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).amount = CharItems(index, fSlot).amount - Quantity
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).amount = Quantity
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ElseIf Quantity = CharItems(index, fSlot).amount Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fItemData = CharItems(index, fSlot).amount
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tItemData = CharItems(index, tslot).amount
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).amount = fItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).amount = tItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ElseIf Quantity > CharItems(index, fSlot).amount Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).amount = Quantity
                End If

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).Durability
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).Durability
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).Durability = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).Durability = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).ID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).ID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).ID = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).ID = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).MagReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).MagReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).MagReinforce = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).MagReinforce = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).PhyReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).PhyReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).PhyReinforce = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).PhyReinforce = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).PlusValue
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).PlusValue
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).PlusValue = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).PlusValue = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).Type
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).Type
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).Type = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).Type = tItemData

            Case "23" 'UnEquip Avatar Item

                fData = "0700"
                fData = fData & "6DB0"
                fData = fData & "0000"
                fData = fData & "01"
                fData = fData & MoveType
                fData = fData & ByteFromInteger(fSlot)
                fData = fData & ByteFromInteger(tslot)
                fData = fData & "0100"
                fData = fData & "00"
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

                fData = "0900"
                fData = fData & "7C37"
                fData = fData & "0000"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & PlayerData(index).CharID
                fData = fData & ByteFromInteger(fSlot)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & DWordFromInteger(CharItems(index, fSlot).ID)
                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts AvatarItemList().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                AvatarItemList(index, fSlot).ID = 0

                If Quantity = 0 Then Quantity = 1

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Quantity < CharItems(index, fSlot).amount Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).amount = CharItems(index, fSlot).amount - Quantity
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).amount = Quantity
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ElseIf Quantity = CharItems(index, fSlot).amount Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    fItemData = CharItems(index, fSlot).amount
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tItemData = CharItems(index, tslot).amount
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).amount = fItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, fSlot).amount = tItemData
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, fSlot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                ElseIf Quantity > CharItems(index, fSlot).amount Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    CharItems(index, tslot).amount = Quantity
                End If

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).Durability
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).Durability
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).Durability = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).Durability = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).ID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).ID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).ID = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).ID = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).MagReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).MagReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).MagReinforce = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).MagReinforce = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).PhyReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).PhyReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).PhyReinforce = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).PhyReinforce = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).PlusValue
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).PlusValue
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).PlusValue = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).PlusValue = tItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fItemData = CharItems(index, fSlot).Type
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                tItemData = CharItems(index, tslot).Type
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, tslot).Type = fItemData
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                CharItems(index, fSlot).Type = tItemData

            Case "08"
                ItemBuy(index)
            Case "09"
                SellItems(index)
            Case "0A"
                Gold(index)
            Case "18"
                itemmall(index)
        End Select

    End Function
    Public Function itemmall(ByRef index As Short) As Object
        'MsgBox (sData)
        fData = "6DB0"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & "08"
        fData = fData & "05" ' id?
        fData = fData & "01"
        fData = fData & ByteFromInteger(43)
        fData = fData & WordFromInteger(4040)
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
    End Function
    Public Function DropItem(ByRef index As Short, ByRef Slot As String) As Object

        Dim randomspot As Double
        Dim ID As String

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If CharItems(index, CInt(Slot)).Type = "CH" Then
            fData = "D730"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & DWordFromInteger(CharItems(index, CInt(Slot)).ID)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & ByteFromInteger(CharItems(index, CInt(Slot)).PlusValue)
            ID = Inverse(DecToHexLong(CInt(Rnd() * 76000000) + 76999999)) 'pickup id
            fData = fData & ID
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
            randomspot = (Rnd() * 1.5) + 0 'to place the item at a random spot around the char
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(((PlayerData(index).XPos + randomspot) - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
            fData = fData & "00000000" 'Z
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(((PlayerData(index).YPos + randomspot) - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
            fData = fData & "AAA6"
            fData = fData & "01"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AccountId konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).AccountId '"FFFFFFFF" 'anyone can pick it up
            fData = fData & "0006"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).CharID
            pLen = (Len(fData) - 8) / 2
            fData = WordFromInteger(pLen) & fData

            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf CharItems(index, CInt(Slot)).Type = "ETC" Then

            fData = "D730"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & DWordFromInteger(CharItems(index, CInt(Slot)).ID)
            fData = fData & (Inverse(DecToHexLong(CInt(Rnd() * 10485750) + 1048575))) 'pickup id
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
            randomspot = CInt(Rnd() * 1.5) + 0 'to place the item at a random spot around the char
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(((PlayerData(index).XPos + randomspot) - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
            fData = fData & "00000000" 'Z
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(((PlayerData(index).YPos + randomspot) - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
            fData = fData & "AAA6"
            fData = fData & "01"
            fData = fData & "FFFFFFFF" 'anyone can pick it up
            fData = fData & "0006"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).CharID
            pLen = (Len(fData) - 8) / 2
            fData = WordFromInteger(pLen) & fData

            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf CharItems(index, CInt(Slot)).Type = "GOLD" Then
            'not done yet
            fData = "D730"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & DWordFromInteger(CharItems(index, CInt(Slot)).ID)
            fData = fData & (Inverse(DecToHexLong(CInt(Rnd() * 10485750) + 1048575))) 'pickup id
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
            randomspot = CInt(Rnd() * 1.5) + 0 'to place the item at a random spot around the char
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(((PlayerData(index).XPos + randomspot) - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
            fData = fData & "00000000" 'Z
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(Float2Hex(((PlayerData(index).YPos + randomspot) - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
            fData = fData & "DC72"
            fData = fData & "01"
            fData = fData & "FFFFFFFF" 'anyone can pick it up
            fData = fData & "0006"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).CharID
            pLen = (Len(fData) - 8) / 2
            fData = WordFromInteger(pLen) & fData

            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

        End If

        'Spawn Item after
        For i = 1 To 500
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Items(i).ID = "" Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).ID = ID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).amount = CharItems(index, CInt(Slot)).amount
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).Durability = CharItems(index, CInt(Slot)).Durability
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).TypeID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).TypeID = CharItems(index, CInt(Slot)).ID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).MagReinforce = CharItems(index, CInt(Slot)).MagReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).PhyReinforce = CharItems(index, CInt(Slot)).PhyReinforce
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).PlusValue = CharItems(index, CInt(Slot)).PlusValue
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).Type = CharItems(index, CInt(Slot)).Type
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).XSector = PlayerData(index).XSection
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).YSector = PlayerData(index).YSection
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).XPos = PlayerData(index).XPos + randomspot
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Items(i).YPos = PlayerData(index).YPos + randomspot
                Exit For
            End If
        Next i

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, CInt(Slot)).amount = 0
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, CInt(Slot)).Durability = 0
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, CInt(Slot)).ID = 0
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, CInt(Slot)).MagReinforce = 0
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, CInt(Slot)).PhyReinforce = 0
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, CInt(Slot)).PlusValue = 0
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        CharItems(index, CInt(Slot)).Type = ""

        fData = "0400"
        fData = fData & "E231"
        fData = fData & "0000"
        fData = fData & ID

    End Function

    Public Function WalkToItem(ByRef index As Short) As Object

        Dim ItemIndex As Short
        Dim Slot As Short
        Dim itemid As String
        Dim DistanceX As Double
        Dim DistanceY As Double
        Dim Distance As Double

        itemid = Mid(sData, 7, 8)
        For i = 1 To 500
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If itemid = Items(i).ID Then
                ItemIndex = i
                Exit For
            End If
        Next i

        'Start
        fData = "0200"
        fData = fData & "CDB2"
        fData = fData & "0000"
        fData = fData & "0101"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
        'Get Distance
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        DistanceX = Items(ItemIndex).XPos - PlayerData(index).XPos
        If DistanceX < 0 And DistanceX <> 0 Then
            Distance = DistanceX * -1
        Else
            Distance = DistanceX
        End If
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        DistanceY = Items(ItemIndex).YPos - PlayerData(index).YPos
        If DistanceY < 0 And DistanceY <> 0 Then
            Distance = Distance + (DistanceY * -1)
        Else
            Distance = Distance + DistanceY
        End If

        'Walk to item
        fData = "38B7"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "01"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(WordFromInteger(((PlayerData(index).XPos) - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
        fData = fData & "0000" 'Z
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(WordFromInteger(((PlayerData(index).YPos) - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
        fData = fData & "01"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(Items(ItemIndex).XSector)) 'X sector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(Items(ItemIndex).YSector)) 'ySector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(ItemIndex).XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(WordFromInteger(((Items(ItemIndex).XPos) - ((Items(ItemIndex).XSector) - 135) * 192) * 10)) 'X
        fData = fData & "0000" 'Z
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(ItemIndex).YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(WordFromInteger(((Items(ItemIndex).YPos) - ((Items(ItemIndex).YSector) - 92) * 192) * 10)) 'Y
        fData = fData & "D742"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PickingItem konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).PickingItem = ItemIndex
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Timer-Eigenschaft PickupDelay.Interval darf nicht den Wert 0 haben. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="169ECF4A-1968-402D-B243-16603CC08604"'
        frmMain.PickupDelay(index).Interval = Distance * 10000 / (((PlayerData(index).RunSpeed * (PlayerData(index).RunSpeed * 1.05)) / 100) + ((20 / PlayerData(index).RunSpeed) * 105))
        frmMain.PickupDelay(index).Enabled = True

    End Function

    Public Function PickupItem(ByRef index As Short) As Object

        Dim Slot As Short
        Dim ItemIndex As Short
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PickingItem konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ItemIndex = PlayerData(index).PickingItem


        fData = "F5B2"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(Items(ItemIndex).XSector)) 'X sector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(Items(ItemIndex).YSector)) 'ySector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(ItemIndex).XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex(((Items(ItemIndex).XPos) - ((Items(ItemIndex).XSector) - 135) * 192) * 10)) 'X
        fData = fData & "00000000" 'Z
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(ItemIndex).YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex(((Items(ItemIndex).YPos) - ((Items(ItemIndex).YSector) - 92) * 192) * 10)) 'Y
        fData = fData & "6A91"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'Despawn Item
        fData = "0400"
        fData = fData & "AB36"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Items(ItemIndex).ID
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        If Slot = 254 Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Items(ItemIndex).Type = "GOLD"
        Else
            For i = 13 To 45
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If CharItems(index, i).ID = 0 Then
                    Slot = i
                    i = 45
                End If
            Next i

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(Items(ItemIndex).Type, "type", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(Items(ItemIndex).amount), "amount", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().TypeID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(Items(ItemIndex).TypeID), "ID", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(Items(ItemIndex).PlusValue), "plusvalue", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(Items(ItemIndex).Durability), "durability", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(Items(ItemIndex).PhyReinforce), "phyreinforce", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(CStr(Items(ItemIndex).MagReinforce), "magreinforce", "item" & Slot, PlayerData(index).CharPath)

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).Type = Items(ItemIndex).Type
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).amount = Items(ItemIndex).amount
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().TypeID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).ID = Items(ItemIndex).TypeID
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).PlusValue = Items(ItemIndex).PlusValue
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).Durability = Items(ItemIndex).Durability
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).PhyReinforce = Items(ItemIndex).PhyReinforce
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, Slot).MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).MagReinforce = Items(ItemIndex).MagReinforce

        End If

        'Add item to inventory
        fData = "6DB0"
        fData = fData & "0000"
        fData = fData & "0106"
        fData = fData & ByteFromInteger(Slot)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().TypeID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(Items(ItemIndex).TypeID)

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(ItemIndex).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Items(ItemIndex).Type = "CH" Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & ByteFromInteger(Items(ItemIndex).PlusValue)
            fData = fData & "00000000000000002700000000" 'item stats, last two bytes are number of blue stats
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items(ItemIndex).Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf Items(ItemIndex).Type = "ETC" Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & WordFromInteger(Items(ItemIndex).amount)
        Else 'items gold
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Items().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & DWordFromInteger(Items(ItemIndex).amount)
        End If
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "0200"
        fData = fData & "CDB2"
        fData = fData & "0000"
        fData = fData & "0200"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PickingItem konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).PickingItem = 0
        frmMain.PickupDelay(index).Enabled = False

    End Function
    Public Function OpenShop(ByRef index As Short) As Object
        Dim Shop As String
        Dim Shop1 As String
        Shop = Left(sData, 8)
        'UPGRADE_WARNING: Arrays in Struktur NpcDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim NpcDataBase As DAO.Recordset
        Dim BookMark As Object
        OpenSremuDataBase()
        NpcDataBase = DataBases.OpenRecordset("NpcData", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        NpcDataBase.Index = "npcid1"
        NpcDataBase.Seek(">=", Shop)
        With NpcDataBase
            Shop1 = .Fields("sh3").Value
        End With
        If CDbl(Shop1) = 0 Then
            Shop1 = "01000000"
        End If

        If Shop = "96000000" Then
            Shop1 = "04000000"
        ElseIf Shop = "A7000000" Then
            Shop1 = "04000000"
        ElseIf Shop = "9C010000" Then
            Shop1 = "04000000"
        ElseIf Shop = "8B010000" Then
            Shop1 = "04000000"
        ElseIf Shop = "80010000" Then
            Shop1 = "04000000"
        End If
        fData = "38B3"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & Shop1
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function
    Public Function CloseShop(ByRef index As Short) As Object
        fData = "B3B4"
        fData = fData & "0000"
        fData = fData & "01"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
    End Function
    Public Function OpenStr(ByRef index As Short) As Object
        fData = "2631"
        fData = fData & "0000"
        fData = fData & "0000000000000000"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "AF30"
        fData = fData & "00002D000000"
        fData = fData & "9600"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "1A32"
        fData = fData & "0000"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
    End Function
    Public Function ItemBuy(ByRef index As Short) As Object
        Dim xx2 As String
        xx2 = Mid(sData, 4, 1)
        'UPGRADE_WARNING: Arrays in Struktur ShopDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim ShopDataBase As DAO.Recordset
        Dim BookMark As Object
        Dim ObjectID As String
        Dim tab1 As String
        Dim Ref1 As String
        ObjectID = Right(sData, 8)
        OpenDB()
        rstGetRecord = New ADODB.Recordset
        With rstGetRecord
            .let_ActiveConnection(rstConnGenericRecordset)
            .CursorLocation = ADODB.CursorLocationEnum.adUseClient
            .CursorType = ADODB.CursorTypeEnum.adOpenDynamic
        End With

        strSQL = "SELECT * FROM ShopData WHERE [Id] = '" & ObjectID & "'"
        rstGetRecord.Open(strSQL)
        xx2 = CStr(CDbl(xx2) + 1)

        tab1 = rstGetRecord.Fields("A" & xx2).Value

        Dim xx1 As String
        xx1 = Mid(sData, 5, 2)
        Dim Slot As Short
        Dim ItemIndex As Short

        'Dim ItemDataBase As Recordset
        OpenDB2()
        rstGetRecord2 = New ADODB.Recordset
        With rstGetRecord2
            .let_ActiveConnection(rstConnGenericRecordset)
            .CursorLocation = ADODB.CursorLocationEnum.adUseClient
            .CursorType = ADODB.CursorTypeEnum.adOpenDynamic
        End With

        strSQL2 = "SELECT * FROM ShopItem WHERE [Ref] = '" & tab1 & "'"
        rstGetRecord2.Open(strSQL2)
        Ref1 = rstGetRecord2.Fields("A" & xx1).Value

        'UPGRADE_WARNING: Arrays in Struktur ItemData müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim ItemData As DAO.Recordset

        Dim sellp As String
        Dim sellp2 As String
        OpenSremuDataBase()
        Dim Quantity2 As String
        Quantity2 = CStr(IntegerFromWord(Mid(sData, 7, 4)))
        ItemData = DataBases.OpenRecordset("Items", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        ItemData.Index = "Name"
        ItemData.Seek(">=", Ref1)

        With ItemData
            sellp = .Fields("BuyPrice").Value
        End With
        sellp = CStr(CDbl(sellp) * CDbl(Quantity2))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GoldInInventory konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sellp2 = CStr(Val(PlayerData(index).GoldInInventory) - Val(sellp))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GoldInInventory konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).GoldInInventory = sellp2
        fData = "B330"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & DWordFromInteger(sellp2)
        fData = fData & "0000000000" 'Gold
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


        For i = 13 To 65
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If CharItems(index, i).ID = "0" Then
                Slot = i
                i = 65
            End If
        Next i
        Dim Quantity1 As String
        Dim itemid As String
        Quantity1 = CStr(IntegerFromWord(Mid(sData, 7, 4)))
        itemid = Mid(sData, 3, 4)
        fData = "6DB0"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & "08"
        fData = fData & itemid ' id?
        fData = fData & "01"
        fData = fData & ByteFromInteger(Slot)
        fData = fData & WordFromInteger(Quantity1)
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


        'UPGRADE_WARNING: Arrays in Struktur ItemDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim ItemDataBase As DAO.Recordset
        Dim itemname As String
        Dim strSQLe As String
        Dim itype As String
        Dim itype1 As String
        Dim iid As String

        OpenSremuDataBase()
        ItemDataBase = DataBases.OpenRecordset("Items", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        ItemDataBase.Index = "Name"
        ItemDataBase.Seek(">=", Ref1)
        'strSQLe = "SELECT * FROM items WHERE [Name] = '" & Ref1 & "'"
        'rstGetRecord2.Open strSQLe
        With ItemDataBase
            itemname = .Fields("ID").Value
            itype = .Fields("Type").Value
        End With
        If itype = "1" Then
            itype1 = "CH"
        Else
            itype1 = "ETC"
        End If
        Dim at As String
        at = CStr(IntegerFromWord(Quantity1))
        If CDbl(at) > 50 Then
            at = "50"
        ElseIf CDbl(at) = 0 Then
            at = "1"
        End If

        If itype1 = "CH" Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("CH", "type", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(at, "amount", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(itemname, "ID", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "plusvalue", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("100", "durability", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "phyreinforce", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "magreinforce", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).Type = itype1
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).amount = at
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).ID = itemname
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).PlusValue = "0"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).Durability = "100"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).PhyReinforce = "0"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).MagReinforce = "0"
        Else
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("ETC", "type", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(at, "amount", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite(itemname, "ID", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "plusvalue", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "durability", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "phyreinforce", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iniWrite("0", "magreinforce", "item" & Slot, PlayerData(index).CharPath)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).Type = itype1
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).amount = at
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).ID = itemname
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).PlusValue = "0"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).Durability = "0"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).PhyReinforce = "0"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            CharItems(index, Slot).MagReinforce = "0"
        End If

    End Function
    Public Function SellItems(ByRef index As Short) As Object
        Dim ItemIndex As Short
        Dim Quantity1 As String
        'UPGRADE_WARNING: Arrays in Struktur ItemDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim ItemDataBase As DAO.Recordset
        Dim BookMark As Object
        Dim sellp As String
        Dim sellp2 As String
        OpenSremuDataBase()
        Quantity1 = CStr(IntegerFromWord(Mid(sData, 3, 2)))
        ItemDataBase = DataBases.OpenRecordset("Items", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        ItemDataBase.Index = "Id"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ItemDataBase.Seek(">=", CharItems(index, CInt(Quantity1)).ID)

        With ItemDataBase
            sellp = .Fields("SellPrice").Value
        End With
        sellp = CStr(CDbl(sellp) * CDbl(Quantity1))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GoldInInventory konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sellp2 = CStr(Val(PlayerData(index).GoldInInventory) + Val(sellp))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GoldInInventory konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).GoldInInventory = sellp2

        fData = "B330"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & DWordFromInteger(sellp2)
        fData = fData & "0000000000" 'Gold
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


        'Quantity1 = Mid(sData, 5, 4)
        Dim npcids As String
        Dim slot1 As String
        npcids = Right(sData, 8)
        slot1 = Left(sData, 8)

        fData = "6DB0"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & slot1
        fData = fData & npcids
        fData = fData & "01"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


    End Function
    Public Function Gold(ByRef index As Short) As Object
        Dim randomspot As Short
        Dim goldm As String
        Dim goldtype As String
        Dim goldtype1 As String
        goldm = Mid(sData, 3, 8)
        goldtype = CStr(IntegerFromWord(Mid(sData, 3, 4)))
        If CDbl(goldtype) > 1000 Then
            goldtype1 = "02000000"
        Else
            goldtype1 = "01000000"
        End If
        fData = "D730"
        fData = fData & "0000"
        fData = fData & "03000000" ' goldtype1 'id
        fData = fData & goldm
        fData = fData & Inverse(DecToHexLong(CInt(Rnd() * 76000000) + 76999999))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((PlayerData(index).XPos - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
        fData = fData & "00000000" 'Z
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((PlayerData(index).YPos - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
        fData = fData & "786F"
        fData = fData & "0000"
        fData = fData & "0500000000"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        Debug.Print(fData)
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        Dim sellp As String
        Dim sellp2 As String
        Dim firstg As String
        Dim firstg1 As String

        sellp = CStr(IntegerFromWord(Mid(sData, 3, 8)))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GoldInInventory konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sellp2 = CStr(Val(PlayerData(index).GoldInInventory) - Val(sellp))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().GoldInInventory konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).GoldInInventory = sellp2

        fData = "B330"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & DWordFromInteger(sellp2)
        fData = fData & "0000000000" 'Gold
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
			End If
		Next i
		
	End Function
	Public Function itemswrite(ByRef index As Short, ByRef fslot1 As Short, ByRef tslot2 As Short) As Object
		Dim amount As String
		Dim ids As String
		Dim pvale As String
		Dim dura As String
		Dim phyr As String
		Dim magr As String
		'MsgBox (tslot)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		amount = CharItems(index, fslot1).amount
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		ids = CharItems(index, fslot1).ID
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Durability konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		dura = CharItems(index, fslot1).Durability
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PlusValue konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		pvale = CharItems(index, fslot1).PlusValue
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().PhyReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		phyr = CharItems(index, fslot1).PhyReinforce
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().MagReinforce konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		magr = CharItems(index, fslot1).MagReinforce
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		iniWrite(CharItems(index, fslot1).Type, "type", "item" & tslot2, PlayerData(index).CharPath)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		iniWrite(amount, "amount", "item" & tslot2, PlayerData(index).CharPath)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		iniWrite(ids, "ID", "item" & tslot2, PlayerData(index).CharPath)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		iniWrite(pvale, "plusvalue", "item" & tslot2, PlayerData(index).CharPath)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		iniWrite(dura, "durability", "item" & tslot2, PlayerData(index).CharPath)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		iniWrite(phyr, "phyreinforce", "item" & tslot2, PlayerData(index).CharPath)
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		iniWrite(magr, "magreinforce", "item" & tslot2, PlayerData(index).CharPath)
	End Function
End Module