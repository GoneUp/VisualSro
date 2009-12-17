Option Strict Off
Option Explicit On
Module modFunctions
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
	
	'UPGRADE_ISSUE: Das Deklarieren eines Parameters als ''As Any'' wird nicht unterstützt. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
	'UPGRADE_ISSUE: Das Deklarieren eines Parameters als ''As Any'' wird nicht unterstützt. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Public Declare Sub CopyMemorySingle Lib "kernel32" Alias "RtlMoveMemory" (ByRef Destination As Single, ByRef Source As Integer, ByVal Length As Integer)
    Public Declare Sub CopyMemoryByte Lib "kernel32" Alias "RtlMoveMemory" (ByRef Destination As Byte, ByRef Source As Single, ByVal Length As Integer)
	'UPGRADE_ISSUE: Das Deklarieren eines Parameters als ''As Any'' wird nicht unterstützt. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
    Private Declare Function GetPrivateProfileString Lib "kernel32.dll" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	
	Private Declare Function WritePrivateProfileString Lib "kernel32.dll"  Alias "WritePrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
	
	Public Function Hex2Float(ByVal tmpHex As String) As Single
		
		Dim TmpSng As Single
		Dim tmpLng As Integer
		tmpLng = CInt("&H" & tmpHex)

        Call CopyMemorySingle((TmpSng), (tmpLng), 4)
		Hex2Float = TmpSng
		
	End Function
	
	Public Function Float2Hex(ByVal TmpFloat As Single) As String
		
		Dim TmpBytes(3) As Byte
		Dim TmpSng As Single
		Dim tmpStr As String
		Dim x As Integer
		TmpSng = TmpFloat

        Call CopyMemoryByte((TmpBytes(0)), (TmpSng), 4)
		For x = 3 To 0 Step -1
			If Len(Hex(TmpBytes(x))) = 1 Then
				tmpStr = tmpStr & "0" & Hex(TmpBytes(x))
			Else
				tmpStr = tmpStr & Hex(TmpBytes(x))
			End If
		Next x
		Float2Hex = tmpStr
		
	End Function
	
	Public Function WordFromInteger(ByRef data As Object) As String

		WordFromInteger = Hex(data)
		If Len(WordFromInteger) = 1 Then
			WordFromInteger = "0" & WordFromInteger & "00"
		ElseIf Len(WordFromInteger) = 2 Then 
			WordFromInteger = WordFromInteger & "00"
		ElseIf Len(WordFromInteger) = 3 Then 
			WordFromInteger = Mid(WordFromInteger, 2, 2) & "0" & Left(WordFromInteger, 1)
		ElseIf Len(WordFromInteger) = 4 Then 
			WordFromInteger = Mid(WordFromInteger, 3, 2) & Left(WordFromInteger, 2)
		End If
	End Function
	
	Public Function IntegerFromWord(ByRef data As String) As Short
		If Len(data) = 2 Then
			IntegerFromWord = CShort("&H" & data)
		ElseIf Len(data) = 4 Then 
			IntegerFromWord = CShort("&H" & Mid(data, 3, 2) & Mid(data, 1, 2))
			'ElseIf Len(data) = 6 Then
			'IntegerFromWord = "&H" & Mid(data, 5, 2) & Mid(data, 3, 2) & Mid(data, 1, 2)
			'ElseIf Len(data) = 8 Then
			'IntegerFromWord = "&H" & Mid(data, 7, 2) & Mid(data, 5, 2) & Mid(data, 3, 2) & Mid(data, 1, 2)
		End If
	End Function
	
	Public Function DWordFromInteger(ByRef data As Object) As String
		Dim i As Object
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts data konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		DWordFromInteger = Hex(data)
		If Len(DWordFromInteger) = 1 Or Len(DWordFromInteger) = 3 Or Len(DWordFromInteger) = 5 Or Len(DWordFromInteger) = 7 Then
			DWordFromInteger = "0" & DWordFromInteger
		End If
		Do While Len(DWordFromInteger) < 8
			DWordFromInteger = "0" & DWordFromInteger
		Loop 
		'Now flip
		Dim sTemp As String
		For i = 8 To 1 Step -1
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts i konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			sTemp = sTemp & Mid(DWordFromInteger, i - 1, 2)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts i konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			i = i - 1
		Next i
		DWordFromInteger = sTemp
	End Function
	
	Public Function ByteFromInteger(ByRef data As Short) As String
		ByteFromInteger = Hex(data)
		If Len(ByteFromInteger) = 1 Then ByteFromInteger = "0" & ByteFromInteger
	End Function
	
	Public Function iniGetStr(ByRef value As String, ByRef section As String, ByRef Pathname As String, Optional ByRef pShowMsgBox As Boolean = True) As String
		
		Dim defaultstr As String ' going to receive the value read from the INI file
		Dim slength As Integer ' going to receive length of the returned string
		defaultstr = Space(255) ' provide enough room for the function to put the value into the buffer
		slength = GetPrivateProfileString(section, value, "(error)", defaultstr, 255, Pathname)
		defaultstr = Left(defaultstr, slength) ' extract the returned string from the buffer
		If defaultstr = "(error)" Then ' we have a problem
			defaultstr = "(error)"
			iniGetStr = defaultstr
			'Dont need below line in if you don't like it!
			'Just to tell you by msgbox it messed up.
			If pShowMsgBox Then
				'Fixit
			End If
		Else ' no problems!
			iniGetStr = defaultstr ' assign String from INI file to INI_getString
		End If
		
	End Function
	
	Public Function iniWrite(ByRef WRITETHIS As String, ByRef value As String, ByRef section As String, ByRef Pathname As String) As Object
		Dim retval As Integer
		retval = WritePrivateProfileString(section, value, WRITETHIS, Pathname)
	End Function
	
	Public Function DecToHexLong(ByRef decvalue As Integer) As String
		Dim HexLen As Short
		DecToHexLong = Hex(decvalue)
		HexLen = Len(DecToHexLong)
		If HexLen < 8 Then
			HexLen = 8 - HexLen
			DecToHexLong = New String("0", HexLen) & DecToHexLong
		End If
	End Function
	
	Public Function DecToHex10Long(ByRef decvalue As Integer) As String
		Dim HexLen As Short
		DecToHex10Long = Hex(decvalue)
		HexLen = Len(DecToHex10Long)
		If HexLen < 10 Then
			HexLen = 10 - HexLen
			DecToHex10Long = New String("0", HexLen) & DecToHex10Long
		End If
	End Function
	
	Public Function DecToHexWord(ByRef decvalue As Short) As String
		Dim DecToWord As Object
		Dim HexLen As Short
		DecToHexWord = Hex(decvalue)
		HexLen = Len(DecToHexWord)
		
		If HexLen < 4 Then
			HexLen = 4 - HexLen
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts DecToWord konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			DecToHexWord = New String("0", HexLen) & DecToWord
		End If
		
	End Function
	
	Public Function Inverse(ByRef DataString As String) As String
		Dim i As Short
		If Len(DataString) Mod 2 = 0 And Len(DataString) > 0 Then
			For i = 1 To Len(DataString) Step 2
				Inverse = Mid(DataString, i, 2) & Inverse
			Next 
			Inverse = "" & Inverse & ""
		Else
			Inverse = DataString
		End If
	End Function
	
	Public Function HexToDec(ByRef HexNumber As String) As String
		Dim i As Integer
		Dim sHexChar As String
		Dim lLength As Integer
		Dim lPosiValue As Double
		Dim Number As Double
		
		lLength = Len(HexNumber)
		For i = lLength To 1 Step -1
			sHexChar = Mid(HexNumber, i, 1)
			lPosiValue = HexToDecChart(sHexChar) * (16 ^ (lLength - i))
			Number = Number + lPosiValue
		Next 
		HexToDec = CStr(Number)
	End Function
	
	Public Function HexToDecChart(ByRef HexCode As String) As Integer
		
		Select Case UCase(HexCode)
			Case "0" : HexToDecChart = 0
			Case "1" : HexToDecChart = 1
			Case "2" : HexToDecChart = 2
			Case "3" : HexToDecChart = 3
				
			Case "4" : HexToDecChart = 4
			Case "5" : HexToDecChart = 5
			Case "6" : HexToDecChart = 6
			Case "7" : HexToDecChart = 7
				
			Case "8" : HexToDecChart = 8
			Case "9" : HexToDecChart = 9
			Case "A" : HexToDecChart = 10
			Case "B" : HexToDecChart = 11
				
			Case "C" : HexToDecChart = 12
			Case "D" : HexToDecChart = 13
			Case "E" : HexToDecChart = 14
			Case "F" : HexToDecChart = 15
				
		End Select
		
	End Function
	
	Function skillup(ByRef eingabe As String, ByRef index As Short) As Object 'made by cherrypopins
		Dim skilladd As String
		Dim altenums As Short
		Dim x As Short
		Dim y As Short
		Dim w As Short
		Dim checkvalue As Short
		Dim checkvalueb As String
		Dim checkvaluec As String
		Dim skillconv As Short
		Dim skillcoa As String
		Dim skillcob As String
		Dim check2 As String
		Dim testvalue As String
		Dim z As Short
		z = 1
		y = 1
		check2 = eingabe
		' --- Blade! ---
		If check2 = "23010000" Or check2 = "2B010000" Or check2 = "33010000" Or check2 = "C4480000" Or check2 = "3B010000" Or check2 = "53010000" Or check2 = "73010000" Or check2 = "9B010000" Or check2 = "BB010000" Or check2 = "FD480000" Or check2 = "24020000" Or check2 = "2F020000" Or check2 = "3A020000" Or check2 = "BE490000" Or check2 = "45020000" Or check2 = "4D020000" Or check2 = "55020000" Or check2 = "D0490000" Or check2 = "5D020000" Or check2 = "6E020000" Or check2 = "330C0000" Or check2 = "7F020000" Or check2 = "88020000" Or check2 = "450C0000" Or check2 = "90020000" Or check2 = "A1020000" Or check2 = "344A0000" Or check2 = "B2020000" Then z = 0
		If check2 = "03000000" Or check2 = "06000000" Or check2 = "1B000000" Or check2 = "1E000000" Or check2 = "1F000000" Or check2 = "21000000" Or check2 = "23000000" Or check2 = "25000000" Or check2 = "27000000" Then z = 0
		' --- Bow! ---
		If check2 = "B1030000" Or check2 = "B9030000" Or check2 = "C1030000" Or check2 = "794B0000" Or check2 = "C9030000" Or check2 = "D1030000" Or check2 = "D9030000" Or check2 = "914B0000" Or check2 = "E1030000" Or check2 = "EC030000" Or check2 = "F7030000" Or check2 = "A64B0000" Or check2 = "02040000" Or check2 = "0A040000" Or check2 = "12040000" Or check2 = "BA4B0000" Or check2 = "1A040000" Or check2 = "1E040000" Or check2 = "080D0000" Or check2 = "22040000" Or check2 = "2A040000" Or check2 = "0D0D0000" Or check2 = "32040000" Or check2 = "3A040000" Or check2 = "DF4B0000" Or check2 = "F24B0000" Or check2 = "F84B0000" Or check2 = "FE4B0000" Or check2 = "42040000" Then z = 0
		If check2 = "47000000" Or check2 = "4A000000" Or check2 = "4D000000" Or check2 = "50000000" Or check2 = "53000000" Or check2 = "55000000" Or check2 = "57000000" Or check2 = "59000000" Then z = 0
		' --- Glavie! ---
		If check2 = "BD020000" Or check2 = "C5020000" Or check2 = "CD020000" Or check2 = "544A0000" Or check2 = "D5020000" Or check2 = "E0020000" Or check2 = "EB020000" Or check2 = "7E4A0000" Or check2 = "F6020000" Or check2 = "FE020000" Or check2 = "06030000" Or check2 = "904A0000" Or check2 = "0E030000" Or check2 = "16030000" Or check2 = "1E030000" Or check2 = "DE4A0000" Or check2 = "DD4A0000" Or check2 = "DC4A0000" Or check2 = "B94A0000" Or check2 = "BA4A0000" Or check2 = "BB4A0000" Or check2 = "BC4A0000" Or check2 = "BD4A0000" Or check2 = "BE4A0000" Or check2 = "26030000" Or check2 = "2E030000" Or check2 = "570C0000" Or check2 = "36030000" Or check2 = "4E030000" Or check2 = "6E030000" Or check2 = "600C0000" Or check2 = "840C0000" Or check2 = "96030000" Or check2 = "9E030000" Or check2 = "5A4B0000" Or check2 = "A6030000" Or check2 = "42040000" Then z = 0
		If check2 = "29000000" Or check2 = "2C000000" Or check2 = "2F000000" Or check2 = "32000000" Or check2 = "35000000" Or check2 = "37000000" Or check2 = "43000000" Or check2 = "45000000" Then z = 0
		' --- Fire! ---
		If check2 = "5A050000" Or check2 = "62050000" Or check2 = "6A050000" Or check2 = "580D0000" Or check2 = "72050000" Or check2 = "77050000" Or check2 = "7C050000" Or check2 = "670D0000" Or check2 = "81050000" Or check2 = "83050000" Or check2 = "85050000" Or check2 = "064D0000" Or check2 = "87050000" Or check2 = "8F050000" Or check2 = "97050000" Or check2 = "0C4D0000" Or check2 = "9F050000" Or check2 = "A4050000" Or check2 = "710D0000" Or check2 = "A9050000" Or check2 = "BA050000" Or check2 = "CB050000" Or check2 = "2D4D0000" Or check2 = "644D0000" Or check2 = "69610000" Or check2 = "694D0000" Or check2 = "DC050000" Then z = 0
		If check2 = "7C000000" Or check2 = "7F000000" Or check2 = "82000000" Or check2 = "85000000" Or check2 = "88000000" Or check2 = "8A000000" Or check2 = "8E000000" Then z = 0
		' --- Force! ---
		If check2 = "E7050000" Or check2 = "EF050000" Or check2 = "F7050000" Or check2 = "770D0000" Or check2 = "FF050000" Or check2 = "04060000" Or check2 = "09060000" Or check2 = "860D0000" Or check2 = "0F500000" Or check2 = "12500000" Or check2 = "0E060000" Or check2 = "16060000" Or check2 = "1E060000" Or check2 = "8D4D0000" Or check2 = "26060000" Or check2 = "2B060000" Or check2 = "30060000" Or check2 = "A24D0000" Or check2 = "35060000" Or check2 = "3A060000" Or check2 = "8C0D0000" Or check2 = "3F060000" Or check2 = "4A060000" Or check2 = "B64D0000" Or check2 = "BD4D0000" Or check2 = "C34D0000" Or check2 = "53060000" Then z = 0
		If check2 = "8F000000" Or check2 = "92000000" Or check2 = "95000000" Or check2 = "98000000" Or check2 = "9B000000" Or check2 = "9D000000" Or check2 = "9F000000" Then z = 0
		' --- Ice! ---
		If check2 = "4D040000" Or check2 = "55040000" Or check2 = "5D040000" Or check2 = "160D0000" Or check2 = "65040000" Or check2 = "6D040000" Or check2 = "75040000" Or check2 = "250D0000" Or check2 = "7D040000" Or check2 = "82040000" Or check2 = "87040000" Or check2 = "1E4C0000" Or check2 = "8C040000" Or check2 = "91040000" Or check2 = "96040000" Or check2 = "2D4C0000" Or check2 = "9B040000" Or check2 = "A6040000" Or check2 = "340D0000" Or check2 = "B1040000" Or check2 = "C2040000" Or check2 = "4A4C0000" Or check2 = "884C0000" Or check2 = "8E4C0000" Or check2 = "944C0000" Or check2 = "9A4C0000" Or check2 = "D3040000" Then z = 0
		If check2 = "5A000000" Or check2 = "5D000000" Or check2 = "60000000" Or check2 = "63000000" Or check2 = "66000000" Or check2 = "68000000" Or check2 = "6A000000" Then z = 0
		' --- Light! ---
		If check2 = "DE040000" Or check2 = "E6040000" Or check2 = "EE040000" Or check2 = "400D0000" Or check2 = "F6040000" Or check2 = "F8040000" Or check2 = "FA040000" Or check2 = "4F0D0000" Or check2 = "FC040000" Or check2 = "07050000" Or check2 = "12050000" Or check2 = "B44C0000" Or check2 = "1D050000" Or check2 = "25050000" Or check2 = "2D050000" Or check2 = "C84C0000" Or check2 = "35050000" Or check2 = "3A050000" Or check2 = "520D0000" Or check2 = "3F050000" Or check2 = "47050000" Or check2 = "E74C0000" Or check2 = "4F050000" Then z = 0
		If check2 = "6E000000" Or check2 = "71000000" Or check2 = "74000000" Or check2 = "77000000" Or check2 = "79000000" Or check2 = "7B000000" Then z = 0
		' ---
		' ==== Check if The Player have skills
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		altenums = CShort(iniGetStr("NumSkills", "character", PlayerData(index).CharPath))
		If altenums = 0 Or z = 0 Then
			' ==== If Player has no skills in Ini write first
			skilladd = altenums + 1 & vbCrLf & "SkillList(" & altenums + 1 & ")=" & eingabe
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			iniWrite(skilladd, "Numskills", "character", PlayerData(index).CharPath)
		Else
			' === If Player has Skills Check if it needs to update
			For x = 1 To altenums
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				checkvalue = CInt("&H" & Left(iniGetStr("SkillList(" & x & ")", "character", PlayerData(index).CharPath), 2))
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				checkvalueb = Right(iniGetStr("SkillList(" & x & ")", "character", PlayerData(index).CharPath), 6)
				skillconv = CInt("&H" & Left(eingabe, 2)) - 1
				skillcoa = Right(eingabe, 6)
				If checkvalue & checkvalueb = skillconv & skillcoa Then
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					iniWrite(eingabe, "SkillList(" & x & ")", "character", PlayerData(index).CharPath)
					x = altenums
					y = 0
				End If
				If x = altenums And y = 1 Then
					skilladd = altenums + 1 & vbCrLf & "SkillList(" & altenums + 1 & ")=" & eingabe
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					iniWrite(skilladd, "Numskills", "character", PlayerData(index).CharPath)
				End If
				
				Debug.Print("-Check Skill " & checkvalue)
			Next x
		End If
		
	End Function
	
	'UPGRADE_NOTE: TimeString wurde aktualisiert auf TimeString_Renamed. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Function TimeString_Renamed(ByRef Seconds As Integer) As String
		
		Dim lHrs As Integer
		Dim lMinutes As Integer
		Dim lSeconds As Integer
		
		lSeconds = Seconds
		
		lHrs = Int(lSeconds / 3600)
		lMinutes = (Int(lSeconds / 60)) - (lHrs * 60)
		lSeconds = Int(lSeconds Mod 60)
		
		Dim sAns As String
		
		
		If lSeconds = 60 Then
			lMinutes = lMinutes + 1
			lSeconds = 0
		End If
		
		If lMinutes = 60 Then
			lMinutes = 0
			lHrs = lHrs + 1
		End If
		
		sAns = VB6.Format(CStr(lHrs), "#####0") & ":" & VB6.Format(CStr(lMinutes), "00") & ":" & VB6.Format(CStr(lSeconds), "00")
		
		TimeString_Renamed = sAns
		
	End Function
End Module