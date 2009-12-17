Option Strict Off
Option Explicit On
Module modConvert
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
	
	
	Public Function cv_BytesFromHex(ByVal sInputHex As String) As Object
		' Returns array of bytes from hex string in big-endian order
		' E.g. sHex="FEDC80" will return array {&HFE, &HDC, &H80}
		Dim i As Integer
		Dim M As Integer
		Dim aBytes() As Byte
		If Len(sInputHex) Mod 2 <> 0 Then
			sInputHex = "0" & sInputHex
		End If
		
		M = Len(sInputHex) \ 2
		If M <= 0 Then
			' Version 2: Returns empty array
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts cv_BytesFromHex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			cv_BytesFromHex = VB6.CopyArray(aBytes)
			Exit Function
		End If
		
		ReDim aBytes(M - 1)
		
		For i = 0 To M - 1
			aBytes(i) = Val("&H" & Mid(sInputHex, i * 2 + 1, 2))
		Next 
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts cv_BytesFromHex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		cv_BytesFromHex = VB6.CopyArray(aBytes)
		
	End Function
	
	Public Function cv_WordsFromHex(ByVal sHex As String) As Object
		' Converts string <sHex> with hex values into array of words (long ints)
		' E.g. "fedcba9876543210" will be converted into {&HFEDCBA98, &H76543210}
		Const ncLEN As Short = 8
		Dim i As Integer
		Dim nWords As Integer
		Dim aWords() As Integer
		
		nWords = Len(sHex) \ ncLEN
		If nWords <= 0 Then
			' Version 2: Returns empty array
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts cv_WordsFromHex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			cv_WordsFromHex = VB6.CopyArray(aWords)
			Exit Function
		End If
		
		ReDim aWords(nWords - 1)
		For i = 0 To nWords - 1
			aWords(i) = Val("&H" & Mid(sHex, i * ncLEN + 1, ncLEN))
		Next 
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts cv_WordsFromHex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		cv_WordsFromHex = VB6.CopyArray(aWords)
		
	End Function
	
	Public Function cv_HexFromWords(ByRef aWords As Object) As String
		' Converts array of words (Longs) into a hex string
		' E.g. {&HFEDCBA98, &H76543210} will be converted to "FEDCBA9876543210"
		Const ncLEN As Short = 8
		Dim i As Integer
		Dim nWords As Integer
		Dim sHex As New VB6.FixedLengthString(ncLEN)
		Dim iIndex As Integer
		
		'Set up error handler to catch empty array
		On Error GoTo ArrayIsEmpty
		If Not IsArray(aWords) Then
			Exit Function
		End If
		
		nWords = UBound(aWords) - LBound(aWords) + 1
		cv_HexFromWords = New String(" ", nWords * ncLEN)
		iIndex = 0
		For i = 0 To nWords - 1
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts aWords() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			sHex.Value = Hex(aWords(i))
			sHex.Value = New String("0", ncLEN - Len(sHex.Value)) & sHex.Value
			Mid(cv_HexFromWords, iIndex + 1, ncLEN) = sHex.Value
			iIndex = iIndex + ncLEN
		Next 
		
ArrayIsEmpty: 
		
	End Function
	
	Public Function cv_HexFromBytes(ByRef aBytes() As Byte) As String
		' Returns hex string from array of bytes
		' E.g. aBytes() = {&HFE, &HDC, &H80} will return "FEDC80"
		Dim i As Integer
		Dim iIndex As Integer
		Dim nLen As Integer
		
		'Set up error handler to catch empty array
		On Error GoTo ArrayIsEmpty
		
		nLen = UBound(aBytes) - LBound(aBytes) + 1
		
		cv_HexFromBytes = New String(" ", nLen * 2)
		iIndex = 0
		For i = LBound(aBytes) To UBound(aBytes)
			Mid(cv_HexFromBytes, iIndex + 1, 2) = HexFromByte(aBytes(i))
			iIndex = iIndex + 2
		Next 
		
ArrayIsEmpty: 
		
	End Function
	
	'UPGRADE_NOTE: str wurde aktualisiert auf str_Renamed. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
	Public Function cv_HexFromString(ByRef str_Renamed As String) As String
		' Converts string <str> of ascii chars to string in hex format
		' str may contain chars of any value between 0 and 255.
		' E.g. "abc." will be converted to "6162632E"
		Dim byt As Byte
		Dim i As Integer
		Dim n As Integer
		Dim iIndex As Integer
		Dim sHex As String
		
		n = Len(str_Renamed)
		sHex = New String(" ", n * 2)
		iIndex = 0
		For i = 1 To n
			byt = CByte(Asc(Mid(str_Renamed, i, 1)) And &HFF)
			Mid(sHex, iIndex + 1, 2) = HexFromByte(byt)
			iIndex = iIndex + 2
		Next 
		cv_HexFromString = sHex
		
	End Function
	
	Public Function cv_StringFromHex(ByRef strHex As String) As String
		' Converts string <strHex> in hex format to string of ascii chars
		' with value between 0 and 255.
		' E.g. "6162632E" will be converted to "abc."
		Dim i As Short
		Dim nBytes As Short
		
		nBytes = Len(strHex) \ 2
		cv_StringFromHex = New String(" ", nBytes)
		For i = 0 To nBytes - 1
			Mid(cv_StringFromHex, i + 1, 1) = Chr(Val("&H" & Mid(strHex, i * 2 + 1, 2)))
		Next 
		
	End Function
	
	Public Function cv_GetHexByte(ByVal sInputHex As String, ByRef iIndex As Integer) As Byte
		' Extracts iIndex'th byte from hex string (starting at 1)
		' E.g. cv_GetHexByte("fecdba98", 3) will return &HBA
		Dim i As Integer
		i = 2 * iIndex
		If i > Len(sInputHex) Or i <= 0 Then
			cv_GetHexByte = 0
		Else
			cv_GetHexByte = Val("&H" & Mid(sInputHex, i - 1, 2))
		End If
		
	End Function
	
	Public Function RandHexByte() As String
		'   Returns a random byte as a 2-digit hex string
		Static stbInit As Boolean
		If Not stbInit Then
			Randomize()
			stbInit = True
		End If
		
		RandHexByte = HexFromByte(CByte((Rnd() * 256) And &HFF))
	End Function
	
	Public Function HexFromByte(ByVal x As Object) As String
		' Returns a 2-digit hex string for byte x
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts x konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		x = x And &HFF
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts x konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		If x < 16 Then
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts x konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			HexFromByte = "0" & Hex(x)
		Else
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts x konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			HexFromByte = Hex(x)
		End If
	End Function
	
	
	Public Function testWordsHex() As Object
		
		Dim aWords As Object
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts cv_WordsFromHex() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts aWords konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		aWords = cv_WordsFromHex("FEDCBA9876543210")
		
	End Function
	
	
	
	Public Function NumToHex(ByVal Number As Object) As String
		'this function converts a number to a hex number
		'the Hex() function converts a number to a string
		'of the hex number this does it right
		'it converts it to a long format hex number
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Number konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		NumToHex = Hex(Number)
		If Len(NumToHex) = 1 Then
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Number konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			NumToHex = "0" & Hex(Number)
		Else
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Number konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			NumToHex = Hex(Number)
		End If
	End Function
End Module