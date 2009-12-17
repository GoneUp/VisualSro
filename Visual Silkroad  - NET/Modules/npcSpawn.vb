Option Strict Off
Option Explicit On
Module npcSpawn
	Private fData As String
	Private pLen As Short
	Private i As Short
	Private x As Short
	Private Structure spawndata
		Dim XPos As Double
		Dim YPos As Double
	End Structure
	Public Function SpawnMasNPC(ByRef index As Short, ByRef NPCID As Short, ByRef UniqueID As Short, ByRef pozX As Double, ByRef pozY As Double) As Object
		
		
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
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((pozX - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
        fData = fData & "00000000" 'Z
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((pozY - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
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
	
	
	
	
	
	
	
	Public Function mobspawn(ByRef index As Short) As Object
		
		
		
		SpawnMonster(index, 7562, 0, 6428, 1084)
		SpawnMonster(index, 7562, 0, 6479, 870)
		SpawnMonster(index, 7562, 0, 6489, 866)
		SpawnMonster(index, 7562, 0, 6500, 866)
		SpawnMonster(index, 7562, 0, 6520, 866)
		SpawnMonster(index, 7562, 0, 6540, 866)
		SpawnMonster(index, 7562, 0, 6560, 866)
		'---------------
		SpawnMonster(index, 7562, 0, 6479, 850)
		SpawnMonster(index, 7562, 0, 6489, 850)
		SpawnMonster(index, 7562, 0, 6500, 850)
		SpawnMonster(index, 7562, 0, 6520, 850)
		SpawnMonster(index, 7562, 0, 6540, 850)
		'---------------
		SpawnMonster(index, 7562, 0, 6479, 835)
		SpawnMonster(index, 7562, 0, 6489, 835)
		SpawnMonster(index, 7562, 0, 6500, 835)
		SpawnMonster(index, 7562, 0, 6520, 835)
		SpawnMonster(index, 7562, 0, 6540, 835)
		'----------------
		SpawnMonster(index, 7562, 0, 6479, 820)
		SpawnMonster(index, 7562, 0, 6489, 820)
		SpawnMonster(index, 7562, 0, 6500, 820)
		SpawnMonster(index, 7562, 0, 6520, 820)
		SpawnMonster(index, 7562, 0, 6540, 820)
		'------------
		SpawnMonster(index, 7562, 0, 6479, 800)
		SpawnMonster(index, 7562, 0, 6489, 800)
		SpawnMonster(index, 7562, 0, 6500, 800)
		SpawnMonster(index, 7562, 0, 6520, 800)
		SpawnMonster(index, 7562, 0, 6540, 820)
		
	End Function
End Module