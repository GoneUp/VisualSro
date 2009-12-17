Option Strict Off
Option Explicit On
Module modTrading
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
	
	Public Function Handle_Exchange_Request(ByRef index As Short, ByRef data As String) As Object
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ExchangeTargetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		PlayerData(index).ExchangeTargetID = Left(data, 8)
		For i = 1 To UBound(PlayerData)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ExchangeTargetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If PlayerData(i).CharID = PlayerData(index).ExchangeTargetID Then
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetWinSock konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				PlayerData(index).ExchangeTargetWinSock = i
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetWinSock konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				PlayerData(i).ExchangeTargetWinSock = index
				Exit For
			End If
		Next i
		
		fData = "9333" & "0000"
		fData = fData & "01" '01 = Shows the target the request window
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & PlayerData(index).ExchangeTargetID
		pLen = (Len(fData) - 8) / 2
		fData = WordFromInteger(pLen) & fData
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetWinSock konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        modGlobal.GameSocket(PlayerData(index).ExchangeTargetWinSock).SendData(cv_StringFromHex(fData))


    End Function

    Public Function Handle_Exchange_Answer(ByRef index As Short, ByRef data As String) As Object

        Select Case Mid(data, 3, 2)
            Case "00" 'Target refused the request
                fData = "5734" & "0000"
                fData = fData & "28"
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetWinSock konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                modGlobal.GameSocket(PlayerData(index).ExchangeTargetWinSock).SendData(cv_StringFromHex(fData))
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


            Case "01" 'Target accepted the request
                fData = "1932" & "0000"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & PlayerData(index).ExchangeTargetID
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData

                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().IsInExchange konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).IsInExchange = True


                fData = "37B2" & "0000"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & "01" & PlayerData(index).ExchangeTargetID
                pLen = (Len(fData) - 8) / 2
                fData = WordFromInteger(pLen) & fData

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetWinSock konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                modGlobal.GameSocket(PlayerData(index).ExchangeTargetWinSock).SendData(cv_StringFromHex(fData))

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetWinSock konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().IsInExchange konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(PlayerData(index).ExchangeTargetWinSock).IsInExchange = True
        End Select

    End Function

    Public Function Handle_Exchange_Cancle(ByRef index As Short, ByRef data As String) As Object

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).IsInExchange konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).IsInExchange = True Then
            fData = "5734" & "0000"
            fData = fData & "2C"
            pLen = (Len(fData) - 8) / 2
            fData = WordFromInteger(pLen) & fData

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetWinSock konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            modGlobal.GameSocket(PlayerData(index).ExchangeTargetWinSock).SendData(cv_StringFromHex(fData))
            modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))



            fData = "DBB2" & "0000"
            fData = fData & "01"
            pLen = (Len(fData) - 8) / 2
            fData = WordFromInteger(pLen) & fData

            modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


            fData = "DBB2" & "0000"
            fData = fData & "021B"
            pLen = (Len(fData) - 8) / 2
            fData = WordFromInteger(pLen) & fData

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetWinSock konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            modGlobal.GameSocket(PlayerData(index).ExchangeTargetWinSock).SendData(cv_StringFromHex(fData))
            modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        End If

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().IsInExchange konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).IsInExchange = False
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ExchangeTargetWinSock konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().IsInExchange konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(PlayerData(index).ExchangeTargetWinSock).IsInExchange = False

    End Function
End Module