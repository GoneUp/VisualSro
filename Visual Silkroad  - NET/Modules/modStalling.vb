Option Strict Off
Option Explicit On
Module modStalling
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
	
	Public Function Handle_Stall_Close(ByRef index As Short, ByRef data As String) As Object
		
		fData = "D133" & "0000"
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & PlayerData(index).CharID
		fData = fData & "17"
		pLen = (Len(fData) - 8) / 2
		fData = WordFromInteger(pLen) & fData
		
		For i = 1 To UBound(PlayerData)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "2CB4" & "0000"
        fData = fData & "01"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallIsOpen konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).StallIsOpen = False

    End Function

    Public Function Handle_Stall_GoInside(ByRef index As Short, ByRef data As String) As Object

        Dim StallOwnerID As String
        Dim StallOwnerWinSock As Short

        StallOwnerID = Left(data, 8)
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).CharID = StallOwnerID Then
                StallOwnerWinSock = i
                Exit For
            End If
        Next i

        fData = "AB36" & "0000"
        fData = "C5346203"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "1FB6" & "0000"
        fData = fData & "01"
        fData = fData & StallOwnerID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallWelcomeMessage konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLen = (Len(PlayerData(StallOwnerWinSock).StallWelcomeMessage) / 4)
        fData = fData & WordFromInteger(pLen)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallWelcomeMessage konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(StallOwnerWinSock).StallWelcomeMessage

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(StallOwnerWinSock).StallIsOpen konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Select Case PlayerData(StallOwnerWinSock).StallIsOpen
            Case True
                fData = fData & "0100" 'Send that the target is open allready
            Case False
                fData = fData & "0000" 'Send that the target stall is in modifying status
        End Select

        fData = fData & "FF00"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "6032" & "0000"
        fData = fData & "02" 'Let Player appear in StallVisitor-List
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).CharID = StallOwnerID Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function

    Public Function Handle_Stall_Leave(ByRef index As Short, ByRef data As String) As Object

        fData = "E7B6" & "0000"
        fData = fData & "01"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "6032" & "0000"
        fData = fData & "01" 'Let Player disappear in StallVisitor-List
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

    End Function

    Public Function Handle_Stall_NameRequest(ByRef index As Short, ByRef data As String) As Object

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).StallName konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).StallName = Mid(data, 5, Len(data) - 4)
        fData = "df30" & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallName konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLen = (Len(PlayerData(index).StallName)) / 4 '/4 because [1char] = [2hex chars] + [00]
        fData = fData & WordFromInteger(pLen)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallName konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).StallName & "00000000"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().NewStall konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).NewStall = True

    End Function


    Public Function Handle_Stall_Operations(ByRef index As Short, ByRef data As String) As Object

        Select Case Left(data, 2)

            Case "01" 'change item price
                fData = "A8B1" & "0000"
                fData = fData & "0101"
                fData = fData & "0" & Mid(data, 4, 1) 'Stall slot
                fData = fData & Mid(data, 5, 4) 'Amount
                fData = fData & Mid(data, 9, 8) 'Gold
                fData = fData & "00"

                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData

                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemGold konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).StallItemGold(Mid(data, 4, 1)) = Mid(data, 9, 8)

            Case "02" 'put item in stall
                fData = "A8B1" & "0000"
                fData = fData & "010200"
                For i = 0 To 9 'Add all items which are in stall allready
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItem konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).StallItem(i) = True Then
                        fData = fData & "0" & i 'WordFromInteger(i)                  'Stallslot
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & PlayerData(index).StallItemID(i) 'ItemID
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemInventorySlot konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & PlayerData(index).StallItemInventorySlot(i) 'InventorySlot
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemAmount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & PlayerData(index).StallItemAmount(i) 'Amount
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemGold konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & PlayerData(index).StallItemGold(i) 'Gold
                    End If
                    If i = CDbl(Mid(data, 4, 1)) Then
                        fData = fData & Mid(data, 3, 2) 'Stallslot
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().InventorySlot konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Mid(PlayerData(index).InventorySlot(IntegerFromWord(Mid(data, 5, 2))), 3, Len(PlayerData(index).InventorySlot(IntegerFromWord(Mid(data, 5, 2)))) - 2) 'ItemID
                        fData = fData & Mid(data, 5, 2) 'InventorySlot
                        fData = fData & Mid(data, 7, 4) 'Amount
                        fData = fData & Mid(data, 11, 8) 'Gold

                        'Now save all datas for the server
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemInventorySlot konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).StallItemInventorySlot(Mid(data, 4, 1)) = Mid(data, 5, 2)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().InventorySlot konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).StallItemID(Mid(data, 4, 1)) = Mid(PlayerData(index).InventorySlot(IntegerFromWord(Mid(data, 5, 2))), 3, Len(PlayerData(index).InventorySlot(IntegerFromWord(Mid(data, 5, 2)))) - 2) 'Save ItemID
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemAmount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).StallItemAmount(Mid(data, 4, 1)) = Mid(data, 7, 4)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemGold konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).StallItemGold(Mid(data, 4, 1)) = Mid(data, 11, 8)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItem konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).StallItem(Mid(data, 4, 1)) = True
                    End If
                Next i

                fData = fData & "ff"
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData

                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))



            Case "03" 'remove item from stall
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).StallIsOpen konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(index).StallIsOpen = False Then
                    fData = "A8B1" & "0000"
                    fData = fData & "0103" & "00ff"
                    pLen = (Len(fData) - 8) / 2
                    fData = WordFromInteger(pLen) & fData

                    modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItem konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).StallItem(Mid(data, 4, 1)) = False


                    'Now send all items, which are still in stall
                    fData = "A8B1" & "0000"
                    fData = fData & "010200"
                    For i = 0 To 9
                        If i = CDbl(Mid(data, 4, 1)) Then
                            'do nothing, cause items is removed
                        Else
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItem konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If PlayerData(index).StallItem(i) = True Then
                                fData = fData & "0" & i 'WordFromInteger(i)                 'Stallslot
                                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                fData = fData & PlayerData(index).StallItemID(i) 'ItemID
                                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemInventorySlot konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                fData = fData & PlayerData(index).StallItemInventorySlot(i) 'InventorySlot
                                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemAmount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                fData = fData & PlayerData(index).StallItemAmount(i) 'Amount
                                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallItemGold konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                fData = fData & PlayerData(index).StallItemGold(i) 'Gold
                            End If
                        End If
                    Next i
                    fData = fData & "ff"
                    pLen = (Len(fData) - 8) / 2
                    fData = WordFromInteger(pLen) & fData

                    modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
                End If


            Case "05" 'clicked the Open/Modify button

                fData = "A8B1" & "0000"
                fData = fData & "01" & "05"

                Select Case Mid(data, 3, 2)

                    Case "01" 'Open the stall

                        Select Case Right(data, 2)
                            Case "00" 'without stallnetwork
                                fData = fData & "0100"
                            Case "01" 'with stallnetwork
                                fData = fData & "014A"
                            Case "03" 'Client saved that you want to use stallnetwork
                                fData = fData & "0103"
                                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallIsOpen konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                PlayerData(index).StallIsOpen = True
                        End Select

                    Case "00" 'Modify the stall

                        Select Case Right(data, 2)
                            Case "00" 'without stallnetwork
                                fData = fData & "0000"
                            Case "03" 'with stallnetwork
                                fData = fData & "0003"
                        End Select
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallIsOpen konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).StallIsOpen = False

                End Select

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).StallIsOpen konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(index).StallIsOpen = True Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallIsOpen konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).StallIsOpen = False
                Else
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallIsOpen konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).StallIsOpen = True
                End If

                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData

                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i

            Case "06"

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).StallWelcomeMessage konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).StallWelcomeMessage = Mid(data, 7, Len(data) - 6)

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).NewStall konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(index).NewStall = True Then

                    fData = "49B0" & "0000"
                    fData = fData & "01"
                    pLen = (Len(fData) - 8) / 2
                    fData = WordFromInteger(pLen) & fData
                    For i = 1 To UBound(PlayerData)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If PlayerData(i).Ingame = True Then
                            modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                        End If
                    Next i

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().NewStall konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).NewStall = False

                End If

                fData = "A8B1" & "0000"
                fData = fData & "01"
                fData = fData & "06"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallWelcomeMessage konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                pLen = (Len(PlayerData(index).StallWelcomeMessage)) / 4 '/4 becaus [1char] = [2hex chars] + [00]
                fData = fData & WordFromInteger(pLen)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallWelcomeMessage konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & PlayerData(index).StallWelcomeMessage
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData

                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i

            Case "07"

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).StallName konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).StallName = Mid(data, 7, Len(data) - 6)
                fData = "B734" & "0000"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & PlayerData(index).CharID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallName konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                pLen = (Len(PlayerData(index).StallName)) / 4 '/4 becaus [1char] = [2hex chars] + [00]
                fData = fData & WordFromInteger(pLen)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().StallName konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & PlayerData(index).StallName
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData

                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i

        End Select

    End Function
End Module