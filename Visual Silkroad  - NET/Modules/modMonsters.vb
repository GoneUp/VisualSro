Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Module modMonsters
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
	Public tim As Short
	Public dex As Short
	
	
	Public Function SpawnMonster(ByRef index As Short, ByRef monster_id As Integer, ByRef monster_type As Short, ByRef X_Pos As Double, ByRef Y_Pos As Double) As Object
		
		Dim MobsID As String
		Dim Walking As String
		'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim MonsterDataBase As DAO.Recordset
		Dim BookMark As Object
		'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase2 müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
		Dim MonsterDataBase2 As DAO.Recordset
		Dim BookMark2 As Object
		Dim MobHP As String
		Dim Xps As Integer
		Dim MobLvl As String
		Dim DistanceX As Double
		Dim DistanceY As Double
		Dim mobmag As Short
		Dim mobphy As Short
		'--------------------
		'dim pozycjaX = X_Pos
		'dim  pozycjaY = Y_Pos
		'--------------------
		
		OpenSremuDataBase()
		MonsterDataBase = DataBases.OpenRecordset("Monsters", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
		
		fData = "D730"
		fData = fData & "0000"
		fData = fData & Inverse(DecToHexLong(monster_id))
		MobsID = (Inverse(DecToHexLong(CInt(Rnd() * 65535) + 10001)))
		fData = fData & MobsID
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & Inverse(Float2Hex((X_Pos - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
		fData = fData & "00000000" 'Z
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & Inverse(Float2Hex((Y_Pos - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
		fData = fData & "DC72"
		fData = fData & "00" '01 walking
		fData = fData & "01"
		
		If Walking = "01" Then
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			fData = fData & Inverse(WordFromInteger((X_Pos - 10) - ((PlayerData(index).XSection) - 135) * 192)) 'X walking to coords
			fData = fData & "0000" 'Z
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			fData = fData & Inverse(WordFromInteger((Y_Pos - 2) - ((PlayerData(index).YSection) - 92) * 192)) 'Y walking to coords
		End If
		
		fData = fData & "00"
		fData = fData & "DC72"
		fData = fData & "0100"
		fData = fData & "00" 'Berserker
		fData = fData & "00004041" 'Playerspeed while walking
		fData = fData & "00000442" 'Playerspeed while running
		fData = fData & "0000C842" 'Playerspeed while berserk
		fData = fData & "00"
		fData = fData & "00"
		
		fData = fData & ByteFromInteger(monster_type)
		
		fData = fData & "01"
		pLen = (Len(fData) - 8) / 2
		fData = WordFromInteger(pLen) & fData
		
		
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		BookMark = VB6.CopyArray(MonsterDataBase.BookMark)
		
		MonsterDataBase.index = "Id"
		MonsterDataBase.Seek(">=", monster_id)
		
		With MonsterDataBase
			MobHP = .Fields("HP").Value 'Finds mob Hp
			MobLvl = .Fields("Lvl").Value
			Xps = .Fields("Xp").Value
			mobmag = .Fields("PhysDef").Value
			mobphy = .Fields("MagDef").Value
		End With
		
		If MonsterDataBase.NoMatch Then
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			MonsterDataBase.BookMark = BookMark
		Else
			For x = 1 To 500 'number of mobs in world was 50
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If Mobs(x).ID = "" Then
					'If monster_type = 4 Then
					' MobHP = MobHP * 20
					' ElseIf monster_type = 5 Then
					' MobHP = MobHP * 100
					' ElseIf monster_type = 1 Then
					' MobHP = MobHP * 2
					'End If
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).HP = MobHP
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).ID = MobsID
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).Type = monster_id
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).XPos = X_Pos
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).YPos = Y_Pos
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).XSector = PlayerData(index).XSection
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).YSector = PlayerData(index).YSection
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).Xps = Xps
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Special konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).Special = monster_type
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().mDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).mDef = mobmag
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().pDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					Mobs(x).pDef = mobphy
					Exit For
				End If
			Next x
			
			If monster_type = CDbl("16") Then
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).HP = Mobs(x).HP * 10
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).Xps = Xps * 10
			ElseIf monster_type = CDbl("1") Then 
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).HP = Mobs(x).HP * 2
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).Xps = Xps * 2
			ElseIf monster_type = CDbl("3") Then 
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).HP = Mobs(x).HP * 2
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).Xps = Xps * 2
			ElseIf monster_type = CDbl("4") Then 
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).HP = Mobs(x).HP * 20
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).Xps = Xps * 20
			ElseIf monster_type = CDbl("5") Then 
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).HP = Mobs(x).HP * 100
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).Xps = Xps * 100
			ElseIf monster_type = CDbl("6") Then 
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).HP = Mobs(x).HP * 4
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).Xps = Xps * 4
			ElseIf monster_type = CDbl("17") Then 
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).HP = Mobs(x).HP * 15
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).Xps = Xps * 20
			ElseIf monster_type = CDbl("20") Then 
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).HP = Mobs(x).HP * 200
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				Mobs(x).Xps = Xps * 200
			End If
		End If
		For i = 1 To UBound(PlayerData)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i


        'If this is a UniqueMonsterSpawn, we should inform the players
        If (monster_id = CDbl("1954")) Or (monster_id = CDbl("7550")) Or (monster_id = CDbl("7551")) Then 'Tiger Girl
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("1954"))
            UniqueID(1) = MobsID
        End If
        If (monster_id = CDbl("1982")) Or (monster_id = CDbl("7557")) Or (monster_id = CDbl("7558")) Then 'Uruchi
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("1982"))
            UniqueID(2) = MobsID
        End If
        If (monster_id = CDbl("2002")) Or (monster_id = CDbl("7559")) Or (monster_id = CDbl("7560")) Then 'Isyutary
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("2002"))
            UniqueID(3) = MobsID
        End If
        If (monster_id = CDbl("3810")) Or (monster_id = CDbl("7561")) Or (monster_id = CDbl("7562")) Then 'Lord Yarkan
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("3810"))
            UniqueID(4) = MobsID
        End If
        If (monster_id = CDbl("14749")) Or (monster_id = CDbl("23638")) Then 'Cerberos
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("23638"))
            UniqueID(5) = MobsID
        End If
        If (monster_id = CDbl("14760")) Or (monster_id = CDbl("14778")) Or (monster_id = CDbl("23639")) Then 'Captin Ivy
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("23639"))
            UniqueID(6) = MobsID
        End If
        frmMain.MobWalkTimer.Enabled = True
        'frmMain.MobWalkTimer(i).Enabled = True
    End Function




    Public Function SpawnMonsterZerk(ByRef index As Short, ByRef monster_id As Integer, ByRef monster_type As Short, ByRef X_Pos As Double, ByRef Y_Pos As Double) As Object

        Dim MobsID As String
        Dim Walking As String
        'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim MonsterDataBase As DAO.Recordset
        Dim BookMark As Object
        'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase2 müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim MonsterDataBase2 As DAO.Recordset
        Dim BookMark2 As Object
        Dim MobHP As String
        Dim Xps As Integer
        Dim MobLvl As String
        Dim DistanceX As Double
        Dim DistanceY As Double
        Dim mobmag As Short
        Dim mobphy As Short
        '--------------------
        'dim pozycjaX = X_Pos
        'dim  pozycjaY = Y_Pos
        '--------------------

        OpenSremuDataBase()
        MonsterDataBase = DataBases.OpenRecordset("Monsters", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table

        fData = "D730"
        fData = fData & "0000"
        fData = fData & Inverse(DecToHexLong(monster_id))
        MobsID = (Inverse(DecToHexLong(CInt(Rnd() * 65535) + 10001)))
        fData = fData & MobsID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((X_Pos - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
        fData = fData & "00000000" 'Z
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((Y_Pos - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
        fData = fData & "DC72"
        fData = fData & "00" '01 walking
        fData = fData & "01"

        If Walking = "01" Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(WordFromInteger((X_Pos - 10) - ((PlayerData(index).XSection) - 135) * 192)) 'X walking to coords
            fData = fData & "0000" 'Z
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(WordFromInteger((Y_Pos - 2) - ((PlayerData(index).YSection) - 92) * 192)) 'Y walking to coords
        End If

        fData = fData & "00"
        fData = fData & "DC72"
        fData = fData & "0100"
        fData = fData & "01" 'Berserker
        fData = fData & "00004041" 'Playerspeed while walking
        fData = fData & "00000442" 'Playerspeed while running
        fData = fData & "0000C842" 'Playerspeed while berserk
        fData = fData & "00"
        fData = fData & "00"

        fData = fData & ByteFromInteger(monster_type)

        fData = fData & "01"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData


        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(MonsterDataBase.Bookmark)

        MonsterDataBase.Index = "Id"
        MonsterDataBase.Seek(">=", monster_id)

        With MonsterDataBase
            MobHP = .Fields("HP").Value 'Finds mob Hp
            MobLvl = .Fields("Lvl").Value
            Xps = .Fields("Xp").Value
            mobmag = .Fields("PhysDef").Value
            mobphy = .Fields("MagDef").Value
        End With

        If MonsterDataBase.NoMatch Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            MonsterDataBase.Bookmark = BookMark
        Else
            For x = 1 To 500 'number of mobs in world was 50
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Mobs(x).ID = "" Then
                    'If monster_type = 4 Then
                    ' MobHP = MobHP * 20
                    ' ElseIf monster_type = 5 Then
                    ' MobHP = MobHP * 100
                    ' ElseIf monster_type = 1 Then
                    ' MobHP = MobHP * 2
                    'End If
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).HP = MobHP
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).ID = MobsID
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).Type = monster_id
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).XPos = X_Pos
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).YPos = Y_Pos
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).XSector = PlayerData(index).XSection
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).YSector = PlayerData(index).YSection
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).Xps = Xps
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Special konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).Special = monster_type
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().mDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).mDef = mobmag
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().pDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).pDef = mobphy
                    Exit For
                End If
            Next x

            If monster_type = CDbl("16") Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).HP = Mobs(x).HP * 10
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).Xps = Xps * 10
            ElseIf monster_type = CDbl("1") Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).HP = Mobs(x).HP * 2
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).Xps = Xps * 2
            ElseIf monster_type = CDbl("3") Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).HP = Mobs(x).HP * 2
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).Xps = Xps * 2
            ElseIf monster_type = CDbl("4") Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).HP = Mobs(x).HP * 20
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).Xps = Xps * 20
            ElseIf monster_type = CDbl("5") Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).HP = Mobs(x).HP * 100
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).Xps = Xps * 100
            ElseIf monster_type = CDbl("6") Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).HP = Mobs(x).HP * 4
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).Xps = Xps * 4
            ElseIf monster_type = CDbl("17") Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).HP = Mobs(x).HP * 15
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).Xps = Xps * 20
            ElseIf monster_type = CDbl("20") Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).HP = Mobs(x).HP * 200
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(x).Xps = Xps * 200
            End If
        End If
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i


        'If this is a UniqueMonsterSpawn, we should inform the players
        If (monster_id = CDbl("1954")) Or (monster_id = CDbl("7550")) Or (monster_id = CDbl("7551")) Then 'Tiger Girl
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("1954"))
            UniqueID(1) = MobsID
        End If
        If (monster_id = CDbl("1982")) Or (monster_id = CDbl("7557")) Or (monster_id = CDbl("7558")) Then 'Uruchi
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("1982"))
            UniqueID(2) = MobsID
        End If
        If (monster_id = CDbl("2002")) Or (monster_id = CDbl("7559")) Or (monster_id = CDbl("7560")) Then 'Isyutary
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("2002"))
            UniqueID(3) = MobsID
        End If
        If (monster_id = CDbl("3810")) Or (monster_id = CDbl("7561")) Or (monster_id = CDbl("7562")) Then 'Lord Yarkan
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("3810"))
            UniqueID(4) = MobsID
        End If
        If (monster_id = CDbl("14749")) Or (monster_id = CDbl("23638")) Then 'Cerberos
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("23638"))
            UniqueID(5) = MobsID
        End If
        If (monster_id = CDbl("14760")) Or (monster_id = CDbl("14778")) Or (monster_id = CDbl("23639")) Then 'Captin Ivy
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("23639"))
            UniqueID(6) = MobsID
        End If
        frmMain.MobWalkTimer.Enabled = True
        'frmMain.MobWalkTimer(i).Enabled = True
    End Function



    'UPGRADE_NOTE: Kill wurde aktualisiert auf Kill_Renamed. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Function UniqueAnnounce(ByRef Kill_Renamed As Boolean, ByRef PlayerName As String, ByRef MobId As Short) As Object

        fData = "5830"
        fData = fData & "0000"

        If Kill_Renamed = True Then
            fData = fData & "06" 'Kill
        Else
            fData = fData & "05" 'Spawn
        End If

        fData = fData & DWordFromInteger(MobId)

        If Kill_Renamed = True Then
            fData = fData & WordFromInteger(Len(PlayerName))
            fData = fData & cv_HexFromString(PlayerName)
        End If

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function
    Public Function RemoveMob(ByRef index As Short) As Object
        fData = "0400"
        fData = fData & "AB36"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Mobs(index).ID

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Mobs(index).ID = ""

    End Function

    Public Function RemoveDeadMob(ByRef index As Short) As Object
        frmMain.DeathTimer(index).Enabled = False

        frmMain.DeathTimer.Unload(index)

        fData = "AB36"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Mobs(index).ID
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Mobs(index).ID = ""

    End Function

    Public Function MobWalk(ByRef index As Short, ByRef mobindex As Short, ByRef walk As Boolean) As Object
        Dim DistanceX As Double
        Dim DistanceY As Double
        Dim Xspeed As Short
        Dim disx As Double
        Dim disy As Double
        Dim z As Short
        Dim Distance As Double
        'If Mobs(mobindex).HP = 0 Then
        'Call RemoveDeadMob(index)
        'Exit Function
        'End If
        Dim playeri As Short
        If walk = True Then

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(mobindex).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Mobs(mobindex).ID = "" Then Exit Function
            'If PlayerData(index).Attacking = False Then


            DistanceX = (Rnd() * 3) + 2
            DistanceY = (Rnd() * 3) + 3

            If ((Rnd() * 10) + 0) >= 5.5 Then
                DistanceX = DistanceX * -1.5
                DistanceY = DistanceY * -2.5
            End If

            fData = "38B7"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Mobs(mobindex).ID
            fData = fData & "01"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(Mobs(mobindex).XSector)) 'X sector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(Mobs(mobindex).YSector)) 'ySector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(mobindex).XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & WordFromInteger(((Mobs(mobindex).XPos + DistanceX) - (Mobs(mobindex).XSector - 135) * 192) * 10) 'X
            fData = fData & "0000" 'Z
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(mobindex).YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & WordFromInteger(((Mobs(mobindex).YPos + DistanceY) - ((Mobs(mobindex).YSector - 92)) * 192) * 10) 'Y
            fData = fData & "00"
            pLen = (Len(fData) - 8) / 2
            fData = WordFromInteger(pLen) & fData
            If Mid(fData, 1, 4) <> "0E00" Then Exit Function
            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

        End If
    End Function
    Public Function MobSendAttack(ByRef index As Short) As Object
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).HP = 0 Then Exit Function
        frmMain.MobWalkTimer.Enabled = False
        Dim ActionType As String
        'UPGRADE_WARNING: Die untere Begrenzung des Arrays ObjectID wurde von 1 in 0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        Dim ObjectID(9) As String
        Dim Action As String
        Dim Tag As String
        Dim SkillID As String
        Dim CastingID As String
        Dim Crit As String
        Dim AfterState As String
        Dim Damage As String
        Dim NumTargets As Short
        Dim RangedSkill As Short
        Dim DistanceX As Double
        Dim DistanceY As Double
        Dim Distance As Double
        Dim z As Short
        Dim mobs1 As Short
        For z = 1 To UBound(PlayerData)
            mobs1 = z
        Next z
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Mobs(dex).XPos < PlayerData(index).XPos Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            DistanceX = PlayerData(index).XPos - Mobs(dex).XPos
        Else
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            DistanceX = PlayerData(index).XPos - Mobs(dex).XPos
        End If

        If DistanceX < 0 And DistanceX <> 0 Then
            Distance = DistanceX * -1
        Else
            Distance = DistanceX
        End If

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Mobs(dex).YPos < PlayerData(index).YPos Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            DistanceY = PlayerData(index).YPos - Mobs(dex).YPos
        Else
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            DistanceY = PlayerData(index).YPos - Mobs(dex).YPos
        End If

        If DistanceY < 0 And DistanceY <> 0 Then
            Distance = DistanceY * -1 + Distance
        Else
            Distance = Distance + DistanceY
        End If

        If Distance > 2 Then 'if distance is greater then attack distance then walk to mob
            DistanceX = DistanceX - ((DistanceX * (1.6 + 0.2)) / Distance) '1.6 is the attack distance of my weapon
            DistanceY = DistanceY - ((DistanceY * (1.6 + 0.2)) / Distance) '0.2 is the mob attackable distance(depends on size of mob)
            'build walk packet
            fData = "0E00"
            fData = fData & "38B7"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Mobs(dex).ID
            fData = fData & "01"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(Mobs(dex).XSector)) 'X sector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(Mobs(dex).YSector)) 'ySector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).XSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & WordFromInteger(((Mobs(dex).XPos + DistanceX) - (Mobs(dex).XSector - 135) * 192) * 10) 'X
            fData = fData & "0000" 'Z
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).YSector konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & WordFromInteger(((Mobs(dex).YPos + DistanceY) - ((Mobs(dex).YSector - 92)) * 192) * 10) 'Y
            fData = fData & "00"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(dex).XPos = Mobs(dex).XPos + DistanceX
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(dex).YPos = Mobs(dex).YPos + DistanceY


            For i = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).Ingame = True Then
                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i
        End If

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Speed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Mobs(mobs1).Speed = "50"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Attacking = True
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).AttackingID = ObjectID(1)
        If DistanceX < 0 And DistanceX <> 0 Then DistanceX = DistanceX * -1
        If DistanceY < 0 And DistanceY <> 0 Then DistanceY = DistanceY * -1 '50*((50*1.25)/100)+((15/50)*100))  100*((100*1.25)/100)+((20/100)*100))
        'frmMain.WalkAttackDelay(index).Interval = (DistanceY + DistanceX) * 10000 / (((Mobs(index).Speed * (Mobs(index).Speed * 1.05)) / 100) + ((20 / Mobs(index).Speed) * 105))
        'frmMain.WalkAttackDelay(index).Enabled = True
        Call mobattack(index)
    End Function

    Public Function mobattack(ByRef index As Short) As Object
        Dim BookMark As Object
        Dim attacktype As String
        Dim attacktype2 As String
        Dim attacktype3 As String
        Dim attacktype4 As String
        Dim tattacktype As String
        Dim SMinA As Short
        Dim SMaxA As Short
        Dim SPer As Short
        Dim dmg1 As Short
        Dim dmg2 As Short
        Dim skills As Object
        Dim tdmg As Short
        Dim tdmg1 As Double
        Dim db As ADODB.Connection
        Dim rs As ADODB.Recordset
        Dim tablolar As Object
        Dim tablo As Object
        Dim sdmg As Short
        Dim ObjectID2 As String
        Dim attackid As String
        Dim ID As String
        Dim PhyMax As Short
        Dim PhyMin As Short
        Dim MagDef As Short
        Dim PhyDef As Short
        Dim Mob As Integer
        Dim Def As Short
        Dim NDamage As Short
        Dim RDamage As Short
        Dim Crit As String
        Dim Damage As String
        Dim AfterState As String
        Dim z As Short
        Dim mobs1 As Short
        Dim d As Short
        'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim MonsterDataBase As DAO.Recordset
        If frmMain.mobattack.Enabled = True Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(index).ArtHP = 0 Then Exit Function
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Mobs(dex).HP < 0 Then Exit Function
            OpenSremuDataBase()
            MonsterDataBase = DataBases.OpenRecordset("Monsters", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            BookMark = VB6.CopyArray(MonsterDataBase.Bookmark)

            MonsterDataBase.Index = "Id"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            MonsterDataBase.Seek(">=", Mobs(dex).Type)

            With MonsterDataBase
                attacktype = .Fields("Skill1").Value
                attacktype2 = .Fields("skill2").Value
                attacktype3 = .Fields("Skill3").Value
            End With
            'UPGRADE_WARNING: Array hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skills konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            skills = New Object() {attacktype, attacktype2}
            Randomize(VB.Timer())
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skills() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            tattacktype = skills(Int(Rnd() * (UBound(skills) + 1)))

            If CDbl(tattacktype) = 0 Then
                tattacktype = attacktype
            End If
            OpenDB()
            If attacktype = "" Then
                attacktype = CStr(161)
            End If

            db = New ADODB.Connection
            rs = New ADODB.Recordset
            db.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & My.Application.Info.DirectoryPath & "\DataBase\SremuDatabase.mdb;")
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts tablolar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            tablolar = "Skilldata_10000|Skilldata_15000|SkillData_5000|Skilldata_20000|Skilldata_25000|Skilldata_30000|Skilldata_35000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts tablolar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts tablo konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            tablo = Split(tablolar, "|")
            For x = 0 To UBound(tablo)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts tablo() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                rs.Open("SELECT * FROM " & tablo(x) & " WHERE Alan2 = '" & tattacktype & "'", db, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

                If rs.RecordCount > 0 Then
                    SMinA = rs.Fields("Min").Value
                    SMaxA = rs.Fields("Max").Value
                    SPer = rs.Fields("Per").Value
                    'smana = rs("Mana")
                    Exit For
                End If
                rs.Close()
            Next x
            sdmg = Val(CStr(SMinA - SMaxA))
            dmg2 = ((Rnd() * sdmg) + SMinA)
            tdmg1 = CDbl(Val(CStr(dmg2)) * Val(CStr(SPer)))
            tdmg = tdmg1 / 100
            For z = 1 To UBound(PlayerData)
                mobs1 = z
            Next z
            frmMain.MobWalkTimer.Enabled = False
            'PhyMax = PlayerData(index).MaxPhyAtk
            'PhyMin = PlayerData(index).MinPhyAtk

            'attackid = PlayerData(mobatckid).CharID
            'attackid = PlayerData(1).CharID
            'Mobs(index).Attacking = True

            'Build Attack Packet
            fData = "45B2"
            fData = fData & "0000"
            fData = fData & "0102"
            fData = fData & DWordFromInteger(tattacktype)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Mobs(dex).ID
            fData = fData & (Inverse(DecToHexLong(Int(Rnd() * 1048575) + 65536))) 'over id
            fData = fData & mobatckid 'attackid 'mobatckid
            fData = fData & "01"
            fData = fData & "01" 'Num Attacks
            fData = fData & "01" 'Num Mobs
            fData = fData & mobatckid 'mobatckid


            If ((Rnd() * 100) + 10) > 100 Then 'the 10 should be the crit of the weapon
                Crit = "02"
            Else
                Crit = "01"
            End If

            'For x = 1 To 500
            'PhyDef = Mobs(index).MobPhysDef
            'Exit For
            'Next x

            RDamage = ((Rnd() * 100) + 500)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).MagDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().PhyDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Def = PlayerData(index).PhyDef + PlayerData(index).MagDef
            'RDamage = RDamage * 2
            NDamage = 150 - 100
            Damage = CStr(tdmg - (Def))

            If CDbl(Damage) < 0 Then
                Damage = CStr(10)
            End If

            If Crit = "02" Then Damage = CStr(CDbl(Damage) * 2)

            Damage = Inverse(DecToHex10Long(CShort(Damage)))

            For i = 1 To UBound(PlayerData) ' number of mobs in world
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If mobatckid = PlayerData(i).CharID Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).ArtHP <= 0 Then Exit Function
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(i).ArtHP = PlayerData(i).ArtHP - CDbl(HexToDec(Inverse(Damage)))
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).ArtHP < 0 Then
                        AfterState = "80" 'dead
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        Mobs(dex).Attacking = False
                        frmMain.mobattack.Enabled = False
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().SelectedID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).SelectedID = ""
                        'PlayerData(mobatckid).CharID = ""
                    Else
                        'Call MobSendAttack(index)
                        AfterState = "00"
                        frmMain.mobattack.Enabled = True
                    End If
                    Exit For
                End If
            Next i

            fData = fData & AfterState
            fData = fData & Crit
            fData = fData & Damage
            fData = fData & "0000"
            pLen = (Len(fData) - 8) / 2
            fData = WordFromInteger(pLen) & fData

            'Broadcast attack data.
            For x = 1 To UBound(PlayerData)
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(x).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(x).Ingame = True Then
                    modGlobal.GameSocket(x).SendData(cv_StringFromHex(fData))
                End If
            Next x

            If AfterState = "80" Then

                'remove dead mob after 4 seconds and player isnt attacking anymore

                'attacking is over packet
                fData = "0200"
                fData = fData & "CDB2"
                fData = fData & "0000"
                fData = fData & "0200"
                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i

            End If
        End If

    End Function
    Public Function SpawnMonster2(ByRef index As Short, ByRef mob1 As Short) As Object
        Dim monster_id As Integer
        Dim monster_type As Short
        Dim xsect As String
        Dim X_Pos As Object
        Dim zpos As String
        Dim Y_Pos As Object
        OpenDB()
        monster_type = 0
        rstGetRecord = New ADODB.Recordset
        With rstGetRecord
            .let_ActiveConnection(rstConnGenericRecordset)
            .CursorLocation = ADODB.CursorLocationEnum.adUseClient
            .CursorType = ADODB.CursorTypeEnum.adOpenDynamic
        End With

        strSQL = "SELECT * FROM NpcAll WHERE [Number] = '" & mob1 & "'"

        rstGetRecord.Open(strSQL)
        monster_id = rstGetRecord.Fields("Alan1").Value
        xsect = rstGetRecord.Fields("Alan2").Value
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts X_Pos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        X_Pos = rstGetRecord.Fields("Alan3").Value
        zpos = rstGetRecord.Fields("Alan4").Value
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Y_Pos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Y_Pos = rstGetRecord.Fields("Alan5").Value
        Dim MobsID As String
        Dim Walking As String
        'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim MonsterDataBase As DAO.Recordset
        Dim BookMark As Object
        Dim MobHP As String
        Dim Xps As Integer
        Dim MobLvl As String
        Dim DistanceX As Double
        Dim DistanceY As Double
        Dim xy As String
        Dim xsec As Short
        Dim ysec As Short
        Dim mXPos As Double
        Dim mYPos As Double
        Dim XPos As Short
        Dim YPos As Short
        Dim XPos1 As Short
        Dim YPos1 As Short
        Dim XPos2 As String
        Dim YPos2 As String
        Dim XPos3 As Double
        Dim YPos3 As Double
        'MsgBox (X_Pos)
        'MsgBox (Y_Pos)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts X_Pos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        X_Pos = System.Math.Round(X_Pos)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Y_Pos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Y_Pos = System.Math.Round(Y_Pos)
        xy = WordFromInteger(xsect)
        xsec = IntegerFromWord(Mid(xy, 1, 2))
        ysec = IntegerFromWord(Mid(xy, 3, 2))

        'XPos2 = IntegerFromWord(Mid(XPos1, 1, 4))
        'YPos2 = IntegerFromWord(Mid(YPos1, 1, 4))
        'XPos3 = Val(XPos + XPos2)
        'YPos3 = Val(YPos + YPos2)

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts X_Pos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        mXPos = CDbl(xsec - 135) * 192 + X_Pos / 10
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Y_Pos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        mYPos = CDbl(ysec - 92) * 192 + Y_Pos / 10
        Debug.Print(mXPos)
        Debug.Print(mYPos)

        OpenSremuDataBase()
        MonsterDataBase = DataBases.OpenRecordset("Monsters", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        Walking = "03" 'mobs dont walk yet 00 = Off 01 = Wrong walk 02 = Walk 03 = WalkTimer
        fData = "D730"
        fData = fData & "0000"
        fData = fData & Inverse(DecToHexLong(monster_id))
        MobsID = (Inverse(DecToHexLong(CInt(Rnd() * 65535) + 10001)))
        fData = fData & MobsID
        fData = fData & Inverse(ByteFromInteger(xsec))
        fData = fData & Inverse(ByteFromInteger(ysec))
        fData = fData & Inverse(Float2Hex(((mXPos) - ((xsec) - 135) * 192) * 10)) 'DWordFromInteger(mXPos) ' 'X
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Y_Pos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts X_Pos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(X_Pos - Y_Pos) 'DWordFromInteger(zpos) '"00000000" 'Z
        fData = fData & Inverse(Float2Hex(((mYPos) - ((ysec) - 92) * 192) * 10)) 'DWordFromInteger(mYPos) '  'Y
        fData = fData & "DC72"
        fData = fData & "00" '01 walking
        fData = fData & "01"
        fData = fData & "00"
        fData = fData & "DC72"
        fData = fData & "0100"
        fData = fData & "00" 'Berserker
        fData = fData & "00004041" 'Playerspeed while walking
        fData = fData & "00000442" 'Playerspeed while running
        fData = fData & "0000C842" 'Playerspeed while berserk
        fData = fData & "00"
        fData = fData & "00"

        fData = fData & ByteFromInteger(monster_type)

        fData = fData & "01"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData


        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(MonsterDataBase.Bookmark)

        MonsterDataBase.Index = "Id"
        MonsterDataBase.Seek(">=", monster_id)

        With MonsterDataBase
            MobHP = .Fields("HP").Value 'Finds mob Hp
            MobLvl = .Fields("Lvl").Value
            Xps = .Fields("Xp").Value
        End With

        Dim sect As String
        If MonsterDataBase.NoMatch Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            MonsterDataBase.Bookmark = BookMark
        Else

            For x = 1 To 500 'number of mobs in world was 50
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Mobs(x).ID = "" Then
                    'If monster_type = 4 Then
                    ' MobHP = MobHP * 20
                    ' ElseIf monster_type = 5 Then
                    ' MobHP = MobHP * 100
                    ' ElseIf monster_type = 1 Then
                    ' MobHP = MobHP * 2
                    'End If
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).HP = MobHP
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).ID = MobsID
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).Type = monster_id
                    'Mobs(x).XPos = mXPos
                    'Mobs(x).YPos = mYPos
                    'Mobs(x).XSector = xsec
                    'Mobs(x).YSector = ysec
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).Xps = Xps
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Special konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).Special = monster_type
                    Exit For
                End If
            Next x
        End If
        If monster_type = CDbl("16") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 10
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 10
        ElseIf monster_type = CDbl("1") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 2
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 2
        ElseIf monster_type = CDbl("3") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 2
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 2
        ElseIf monster_type = CDbl("4") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 20
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 20
        ElseIf monster_type = CDbl("5") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 100
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 100
        ElseIf monster_type = CDbl("6") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 4
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 4
        ElseIf monster_type = CDbl("17") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 15
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 20
        ElseIf monster_type = CDbl("20") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 200
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 200
        End If

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        'If this is a UniqueMonsterSpawn, we should inform the players
        If (monster_id = CDbl("1954")) Or (monster_id = CDbl("7550")) Or (monster_id = CDbl("7551")) Then 'Tiger Girl
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("1954"))
            UniqueID(1) = MobsID
        End If
        If (monster_id = CDbl("1982")) Or (monster_id = CDbl("7557")) Or (monster_id = CDbl("7558")) Then 'Uruchi
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("1982"))
            UniqueID(2) = MobsID
        End If
        If (monster_id = CDbl("2002")) Or (monster_id = CDbl("7559")) Or (monster_id = CDbl("7560")) Then 'Isyutary
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("2002"))
            UniqueID(3) = MobsID
        End If
        If (monster_id = CDbl("3810")) Or (monster_id = CDbl("7561")) Or (monster_id = CDbl("7562")) Then 'Lord Yarkan
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("3810"))
            UniqueID(4) = MobsID
        End If
        If (monster_id = CDbl("14749")) Or (monster_id = CDbl("23638")) Then 'Cerberos
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("23638"))
            UniqueID(5) = MobsID
        End If
        If (monster_id = CDbl("14760")) Or (monster_id = CDbl("14778")) Or (monster_id = CDbl("23639")) Then 'Captin Ivy
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("23639"))
            UniqueID(6) = MobsID
        End If
        'frmMain.MobWalkTimer.Enabled = True
    End Function
    Public Function SpawnMon(ByRef index As Short, ByRef mob1 As Short) As Object
        Dim monster_id As Integer
        Dim monster_type As Short
        Dim xsect As String
        Dim X_Pos As String
        Dim zpos As String
        Dim Y_Pos As String

        monster_id = 1933
        xsect = CStr(25258)
        X_Pos = "812.67999"
        zpos = "75.080002"
        Y_Pos = "392.89999"
        Dim MobsID As String
        Dim Walking As String
        'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim MonsterDataBase As DAO.Recordset
        Dim BookMark As Object
        Dim MobHP As String
        Dim Xps As Integer
        Dim MobLvl As String
        Dim DistanceX As Double
        Dim DistanceY As Double
        Dim xy As String
        Dim xsec As Short
        Dim ysec As Short
        Dim mXPos As Double
        Dim mYPos As Double
        Dim XPos As Short
        Dim YPos As Short
        Dim XPos1 As String
        Dim YPos1 As String
        Dim XPos2 As String
        Dim YPos2 As String
        Dim XPos3 As Double
        Dim YPos3 As Double

        XPos1 = Inverse(DWordFromInteger(X_Pos))
        YPos1 = Inverse(DWordFromInteger(Y_Pos))
        xy = WordFromInteger(xsect)
        xsec = IntegerFromWord(Mid(xy, 1, 2))
        ysec = IntegerFromWord(Mid(xy, 3, 2))
        'XPos2 = IntegerFromWord(Mid(XPos1, 1, 4))
        'YPos2 = IntegerFromWord(Mid(YPos1, 1, 4))
        XPos = (IntegerFromWord(Mid(XPos1, 5, 4))) '& XPos2
        YPos = (IntegerFromWord(Mid(YPos1, 5, 4))) '& YPos2
        'XPos3 = Val(XPos + XPos2)
        'YPos3 = Val(YPos + YPos2)

        mXPos = (xsec - 135) * 192 + CShort(XPos1) / 10
        mYPos = (ysec - 92) * 192 + CShort(YPos1) / 10

        '--------------------
        'dim pozycjaX = X_Pos
        'dim  pozycjaY = Y_Pos
        '--------------------
        OpenSremuDataBase()
        MonsterDataBase = DataBases.OpenRecordset("Monsters", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        Walking = "03" 'mobs dont walk yet 00 = Off 01 = Wrong walk 02 = Walk 03 = WalkTimer
        fData = "D730"
        fData = fData & "0000"
        fData = fData & Inverse(DecToHexLong(monster_id))
        MobsID = (Inverse(DecToHexLong(CInt(Rnd() * 65535) + 10001)))
        fData = fData & MobsID
        fData = fData & Inverse(ByteFromInteger(xsec))
        fData = fData & Inverse(ByteFromInteger(ysec))
        'fData = fData & Inverse(DWordFromInteger(X_Pos))
        'MsgBox (Inverse(DWordFromInteger(X_Pos)))
        fData = fData & Inverse(Float2Hex(((mXPos) - ((xsec) - 135) * 192) * 10)) 'DWordFromInteger(mXPos) ' 'X
        fData = fData & DWordFromInteger(zpos) 'DWordFromInteger(zpos) '"00000000" 'Z
        fData = fData & Inverse(Float2Hex(((mYPos) - ((ysec) - 92) * 192) * 10)) 'DWordFromInteger(mYPos) '  'Y
        'fData = fData & Inverse(DWordFromInteger(Y_Pos))
        fData = fData & "DC72"
        fData = fData & "00" '01 walking
        fData = fData & "01"
        fData = fData & "00"
        fData = fData & "DC72"
        fData = fData & "0100"
        fData = fData & "00" 'Berserker
        fData = fData & "00004041" 'Playerspeed while walking
        fData = fData & "00000442" 'Playerspeed while running
        fData = fData & "0000C842" 'Playerspeed while berserk
        fData = fData & "00"
        fData = fData & "00"

        fData = fData & ByteFromInteger(monster_type)

        fData = fData & "01"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        MsgBox(fData)

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(MonsterDataBase.Bookmark)

        MonsterDataBase.Index = "Id"
        MonsterDataBase.Seek(">=", monster_id)

        With MonsterDataBase
            MobHP = .Fields("HP").Value 'Finds mob Hp
            MobLvl = .Fields("Lvl").Value
            Xps = .Fields("Xp").Value
        End With

        Dim sect As String
        If MonsterDataBase.NoMatch Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            MonsterDataBase.Bookmark = BookMark
        Else

            For x = 1 To 500 'number of mobs in world was 50
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Mobs(x).ID = "" Then
                    'If monster_type = 4 Then
                    ' MobHP = MobHP * 20
                    ' ElseIf monster_type = 5 Then
                    ' MobHP = MobHP * 100
                    ' ElseIf monster_type = 1 Then
                    ' MobHP = MobHP * 2
                    'End If
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).HP = MobHP
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).ID = MobsID
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).Type = monster_id
                    'Mobs(x).XPos = mXPos
                    'Mobs(x).YPos = mYPos
                    'Mobs(x).XSector = xsec
                    'Mobs(x).YSector = ysec
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).Xps = Xps
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Special konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(x).Special = monster_type
                    Exit For
                End If
            Next x
        End If
        If monster_type = CDbl("16") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 10
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 10
        ElseIf monster_type = CDbl("1") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 2
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 2
        ElseIf monster_type = CDbl("3") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 2
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 2
        ElseIf monster_type = CDbl("4") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 20
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 20
        ElseIf monster_type = CDbl("5") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 100
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 100
        ElseIf monster_type = CDbl("6") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 4
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 4
        ElseIf monster_type = CDbl("17") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 15
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 20
        ElseIf monster_type = CDbl("20") Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(x).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).HP = Mobs(x).HP * 200
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Mobs(x).Xps = Xps * 200
        End If

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        'If this is a UniqueMonsterSpawn, we should inform the players
        If (monster_id = CDbl("1954")) Or (monster_id = CDbl("7550")) Or (monster_id = CDbl("7551")) Then 'Tiger Girl
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("1954"))
            UniqueID(1) = MobsID
        End If
        If (monster_id = CDbl("1982")) Or (monster_id = CDbl("7557")) Or (monster_id = CDbl("7558")) Then 'Uruchi
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("1982"))
            UniqueID(2) = MobsID
        End If
        If (monster_id = CDbl("2002")) Or (monster_id = CDbl("7559")) Or (monster_id = CDbl("7560")) Then 'Isyutary
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("2002"))
            UniqueID(3) = MobsID
        End If
        If (monster_id = CDbl("3810")) Or (monster_id = CDbl("7561")) Or (monster_id = CDbl("7562")) Then 'Lord Yarkan
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("3810"))
            UniqueID(4) = MobsID
        End If
        If (monster_id = CDbl("14749")) Or (monster_id = CDbl("23638")) Then 'Cerberos
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("23638"))
            UniqueID(5) = MobsID
        End If
        If (monster_id = CDbl("14760")) Or (monster_id = CDbl("14778")) Or (monster_id = CDbl("23639")) Then 'Captin Ivy
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(False, PlayerData(index).Charname, CShort("23639"))
            UniqueID(6) = MobsID
        End If
        'frmMain.MobWalkTimer.Enabled = True
    End Function
	
	Public Function SpawnMonster3(ByRef index As Short) As Object
		SpawnMonster2(index, 24)
		SpawnMonster2(index, 25)
		SpawnMonster2(index, 26)
		SpawnMonster2(index, 27)
		SpawnMonster2(index, 28)
		SpawnMonster2(index, 29)
		SpawnMonster2(index, 30)
		SpawnMonster2(index, 31)
		SpawnMonster2(index, 32)
		SpawnMonster2(index, 33)
		SpawnMonster2(index, 34)
		SpawnMonster2(index, 35)
		SpawnMonster2(index, 36)
		SpawnMonster2(index, 37)
		SpawnMonster2(index, 38)
		SpawnMonster2(index, 39)
		SpawnMonster2(index, 40)
		SpawnMonster2(index, 41)
		SpawnMonster2(index, 42)
		SpawnMonster2(index, 43)
		SpawnMonster2(index, 44)
		SpawnMonster2(index, 45)
		SpawnMonster2(index, 46)
		SpawnMonster2(index, 47)
		SpawnMonster2(index, 48)
		SpawnMonster2(index, 49)
		SpawnMonster2(index, 50)
		SpawnMonster2(index, 51)
		SpawnMonster2(index, 52)
		SpawnMonster2(index, 53)
		SpawnMonster2(index, 54)
		SpawnMonster2(index, 55)
		SpawnMonster2(index, 56)
		SpawnMonster2(index, 57)
		SpawnMonster2(index, 58)
		SpawnMonster2(index, 59)
		SpawnMonster2(index, 60)
		SpawnMonster2(index, 61)
		SpawnMonster2(index, 62)
		SpawnMonster2(index, 63)
		SpawnMonster2(index, 64)
		SpawnMonster2(index, 65)
		SpawnMonster2(index, 66)
		SpawnMonster2(index, 67)
		SpawnMonster2(index, 68)
		SpawnMonster2(index, 69)
		SpawnMonster2(index, 70)
		SpawnMonster2(index, 71)
		SpawnMonster2(index, 72)
		SpawnMonster2(index, 73)
		SpawnMonster2(index, 74)
		SpawnMonster2(index, 75)
		SpawnMonster2(index, 76)
		SpawnMonster2(index, 77)
		SpawnMonster2(index, 78)
		SpawnMonster2(index, 79)
		SpawnMonster2(index, 80)
		SpawnMonster2(index, 81)
		SpawnMonster2(index, 82)
		SpawnMonster2(index, 83)
		SpawnMonster2(index, 84)
		SpawnMonster2(index, 85)
		SpawnMonster2(index, 86)
		SpawnMonster2(index, 87)
		SpawnMonster2(index, 88)
		SpawnMonster2(index, 89)
		SpawnMonster2(index, 90)
		SpawnMonster2(index, 91)
		SpawnMonster2(index, 92)
		SpawnMonster2(index, 93)
		SpawnMonster2(index, 94)
		SpawnMonster2(index, 95)
		SpawnMonster2(index, 96)
		SpawnMonster2(index, 97)
		SpawnMonster2(index, 98)
		SpawnMonster2(index, 99)
		SpawnMonster2(index, 100)
		SpawnMonster2(index, 101)
		SpawnMonster2(index, 102)
		SpawnMonster2(index, 103)
		SpawnMonster2(index, 104)
		SpawnMonster2(index, 105)
		SpawnMonster2(index, 106)
		SpawnMonster2(index, 107)
		SpawnMonster2(index, 108)
		SpawnMonster2(index, 109)
		SpawnMonster2(index, 110)
		SpawnMonster2(index, 111)
		SpawnMonster2(index, 112)
		SpawnMonster2(index, 113)
		SpawnMonster2(index, 114)
		SpawnMonster2(index, 115)
		SpawnMonster2(index, 116)
		SpawnMonster2(index, 117)
		SpawnMonster2(index, 118)
		SpawnMonster2(index, 119)
		SpawnMonster2(index, 120)
		SpawnMonster2(index, 121)
		SpawnMonster2(index, 122)
		SpawnMonster2(index, 123)
		SpawnMonster2(index, 124)
		SpawnMonster2(index, 125)
		SpawnMonster2(index, 126)
		SpawnMonster2(index, 127)
		SpawnMonster2(index, 128)
		SpawnMonster2(index, 129)
		SpawnMonster2(index, 130)
		SpawnMonster2(index, 131)
		SpawnMonster2(index, 132)
		SpawnMonster2(index, 133)
		SpawnMonster2(index, 134)
		SpawnMonster2(index, 135)
		SpawnMonster2(index, 136)
		SpawnMonster2(index, 137)
		SpawnMonster2(index, 138)
		SpawnMonster2(index, 139)
		SpawnMonster2(index, 140)
		SpawnMonster2(index, 141)
		SpawnMonster2(index, 142)
		SpawnMonster2(index, 143)
		SpawnMonster2(index, 144)
		SpawnMonster2(index, 145)
		SpawnMonster2(index, 146)
		SpawnMonster2(index, 147)
		SpawnMonster2(index, 148)
		SpawnMonster2(index, 149)
		SpawnMonster2(index, 150)
		SpawnMonster2(index, 151)
		SpawnMonster2(index, 152)
		SpawnMonster2(index, 153)
		SpawnMonster2(index, 154)
		SpawnMonster2(index, 155)
		SpawnMonster2(index, 156)
		SpawnMonster2(index, 157)
		SpawnMonster2(index, 158)
		SpawnMonster2(index, 159)
		SpawnMonster2(index, 160)
		SpawnMonster2(index, 161)
		SpawnMonster2(index, 162)
		SpawnMonster2(index, 163)
		SpawnMonster2(index, 164)
		SpawnMonster2(index, 165)
		SpawnMonster2(index, 166)
		SpawnMonster2(index, 167)
		SpawnMonster2(index, 168)
		SpawnMonster2(index, 169)
		SpawnMonster2(index, 170)
		SpawnMonster2(index, 171)
		SpawnMonster2(index, 172)
		SpawnMonster2(index, 173)
		SpawnMonster2(index, 174)
		SpawnMonster2(index, 175)
		SpawnMonster2(index, 176)
		SpawnMonster2(index, 177)
		SpawnMonster2(index, 178)
		SpawnMonster2(index, 179)
		SpawnMonster2(index, 180)
		SpawnMonster2(index, 181)
		SpawnMonster2(index, 182)
		SpawnMonster2(index, 183)
		SpawnMonster2(index, 184)
		SpawnMonster2(index, 185)
		SpawnMonster2(index, 186)
		SpawnMonster2(index, 187)
		SpawnMonster2(index, 188)
		SpawnMonster2(index, 189)
		SpawnMonster2(index, 190)
		SpawnMonster2(index, 191)
		SpawnMonster2(index, 192)
		SpawnMonster2(index, 193)
		SpawnMonster2(index, 194)
		SpawnMonster2(index, 195)
		SpawnMonster2(index, 196)
		SpawnMonster2(index, 197)
		SpawnMonster2(index, 198)
		SpawnMonster2(index, 199)
		SpawnMonster2(index, 200)
		SpawnMonster2(index, 201)
		SpawnMonster2(index, 202)
		SpawnMonster2(index, 203)
		SpawnMonster2(index, 204)
		SpawnMonster2(index, 205)
		SpawnMonster2(index, 206)
		SpawnMonster2(index, 207)
		SpawnMonster2(index, 208)
		SpawnMonster2(index, 209)
		SpawnMonster2(index, 210)
		SpawnMonster2(index, 211)
		SpawnMonster2(index, 212)
		SpawnMonster2(index, 213)
		SpawnMonster2(index, 214)
		SpawnMonster2(index, 215)
		SpawnMonster2(index, 216)
		SpawnMonster2(index, 217)
		SpawnMonster2(index, 218)
		SpawnMonster2(index, 219)
		SpawnMonster2(index, 220)
		SpawnMonster2(index, 221)
		SpawnMonster2(index, 222)
		SpawnMonster2(index, 223)
		SpawnMonster2(index, 224)
		SpawnMonster2(index, 225)
		SpawnMonster2(index, 226)
		SpawnMonster2(index, 227)
		SpawnMonster2(index, 228)
		SpawnMonster2(index, 229)
		SpawnMonster2(index, 230)
		SpawnMonster2(index, 231)
		SpawnMonster2(index, 232)
		SpawnMonster2(index, 233)
		SpawnMonster2(index, 234)
		SpawnMonster2(index, 235)
		SpawnMonster2(index, 236)
		SpawnMonster2(index, 237)
		SpawnMonster2(index, 238)
		SpawnMonster2(index, 239)
		SpawnMonster2(index, 240)
		SpawnMonster2(index, 241)
		SpawnMonster2(index, 242)
		SpawnMonster2(index, 243)
		SpawnMonster2(index, 244)
		SpawnMonster2(index, 245)
		SpawnMonster2(index, 246)
		SpawnMonster2(index, 247)
		SpawnMonster2(index, 248)
		SpawnMonster2(index, 249)
		SpawnMonster2(index, 250)
		SpawnMonster2(index, 251)
		SpawnMonster2(index, 252)
		SpawnMonster2(index, 253)
		SpawnMonster2(index, 254)
		SpawnMonster2(index, 255)
		SpawnMonster2(index, 256)
		SpawnMonster2(index, 257)
		SpawnMonster2(index, 258)
		SpawnMonster2(index, 259)
		SpawnMonster2(index, 260)
		SpawnMonster2(index, 261)
		SpawnMonster2(index, 262)
		SpawnMonster2(index, 263)
		SpawnMonster2(index, 264)
		SpawnMonster2(index, 265)
		SpawnMonster2(index, 266)
		SpawnMonster2(index, 267)
		SpawnMonster2(index, 268)
		SpawnMonster2(index, 269)
		SpawnMonster2(index, 270)
		SpawnMonster2(index, 271)
		SpawnMonster2(index, 272)
		SpawnMonster2(index, 273)
		SpawnMonster2(index, 274)
		SpawnMonster2(index, 275)
		SpawnMonster2(index, 276)
		SpawnMonster2(index, 277)
		SpawnMonster2(index, 278)
		SpawnMonster2(index, 279)
		SpawnMonster2(index, 280)
		SpawnMonster2(index, 281)
		SpawnMonster2(index, 282)
		SpawnMonster2(index, 283)
		SpawnMonster2(index, 284)
		SpawnMonster2(index, 285)
		SpawnMonster2(index, 286)
		SpawnMonster2(index, 287)
		SpawnMonster2(index, 288)
		SpawnMonster2(index, 289)
		SpawnMonster2(index, 290)
		SpawnMonster2(index, 291)
		SpawnMonster2(index, 292)
		SpawnMonster2(index, 293)
		SpawnMonster2(index, 294)
		SpawnMonster2(index, 295)
		SpawnMonster2(index, 296)
		SpawnMonster2(index, 297)
		SpawnMonster2(index, 298)
		SpawnMonster2(index, 299)
		SpawnMonster2(index, 300)
		SpawnMonster2(index, 301)
		SpawnMonster2(index, 302)
		SpawnMonster2(index, 303)
		SpawnMonster2(index, 304)
		SpawnMonster2(index, 305)
		SpawnMonster2(index, 306)
		SpawnMonster2(index, 307)
		SpawnMonster2(index, 308)
		SpawnMonster2(index, 309)
		SpawnMonster2(index, 310)
		SpawnMonster2(index, 311)
		SpawnMonster2(index, 312)
		SpawnMonster2(index, 313)
		SpawnMonster2(index, 314)
		SpawnMonster2(index, 315)
		SpawnMonster2(index, 316)
		SpawnMonster2(index, 317)
		SpawnMonster2(index, 318)
		SpawnMonster2(index, 319)
		SpawnMonster2(index, 320)
		SpawnMonster2(index, 321)
		SpawnMonster2(index, 322)
		SpawnMonster2(index, 323)
		SpawnMonster2(index, 324)
		SpawnMonster2(index, 325)
		SpawnMonster2(index, 326)
		SpawnMonster2(index, 327)
		SpawnMonster2(index, 328)
		SpawnMonster2(index, 329)
		SpawnMonster2(index, 330)
		SpawnMonster2(index, 331)
		SpawnMonster2(index, 332)
		SpawnMonster2(index, 333)
		SpawnMonster2(index, 334)
		SpawnMonster2(index, 335)
		SpawnMonster2(index, 336)
		SpawnMonster2(index, 337)
		SpawnMonster2(index, 338)
		SpawnMonster2(index, 339)
		SpawnMonster2(index, 340)
		SpawnMonster2(index, 341)
	End Function
End Module