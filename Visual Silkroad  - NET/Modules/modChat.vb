Option Strict Off
Option Explicit On
Module modChat
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
	Dim spawned As Short
	
	Public Function HandleChat(ByRef index As Short) As Object
		
		'0103 0400 7400 6500 7300 7400
		Dim stype As Short
		Dim tData As String
		stype = CShort("&H" & Mid(sData, 1, 2))
		tData = sData
		
		Dim cLength As Short
		Dim sChat As String
		Dim sCommands() As String
		Dim sMessage As String
		Select Case stype
			
			Case 1 'Say chat
				
				
				cLength = CDbl("&H" & Mid(sData, 7, 2) & Mid(sData, 5, 2)) * 4
				sData = Right(sData, cLength)
				
				'Split unicode strings
				For i = 1 To cLength
					sChat = sChat & Mid(sData, i, 2)
					i = i + 3
				Next i
				sChat = cv_StringFromHex(sChat)
				sCommands = Split(sChat, " ")
				
				'Parse chatmessages for GM's.
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).GM konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If PlayerData(index).GM = True Then
					Select Case sCommands(0)
						
						Case ".spawnall"
							
							mobspawn(index) 'all mobs spawn
							
						Case ".guild"
							
							Guild(index)
							
						Case ".kick"
							
							'Drop this player.
							If UBound(sCommands) > 0 Then
								For i = 1 To UBound(PlayerData)
									'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									If PlayerData(i).Charname = sCommands(1) Then
										QuitGame(i, "01")
										Debug.Print("Player '" & sCommands(1) & "' kicked!")
									End If
								Next i
							End If
							GoTo SendConfirmation
							
						Case ".spawn"
							
							If UBound(sCommands) = 4 Then 'User only specified ID.
								SpawnMonster(index, CShort(sCommands(1)), CShort(sCommands(2)), CDbl(sCommands(3)), CDbl(sCommands(4)))
							ElseIf UBound(sCommands) = 2 Then 
								'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								SpawnMonster(index, CShort(sCommands(1)), CShort(sCommands(2)), CDbl(PlayerData(index).XPos), CDbl(PlayerData(index).YPos))
							ElseIf UBound(sCommands) = 1 Then 
								'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								SpawnMonster(index, CShort(sCommands(1)), CShort(0), CDbl(PlayerData(index).XPos), CDbl(PlayerData(index).YPos))
							End If
							GoTo SendConfirmation
							
						Case ".spawnzerk"
							
							If UBound(sCommands) = 4 Then 'User only specified ID.
								SpawnMonsterZerk(index, CShort(sCommands(1)), CShort(sCommands(2)), CDbl(sCommands(3)), CDbl(sCommands(4)))
							ElseIf UBound(sCommands) = 2 Then 
								'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								SpawnMonsterZerk(index, CShort(sCommands(1)), CShort(sCommands(2)), CDbl(PlayerData(index).XPos), CDbl(PlayerData(index).YPos))
							ElseIf UBound(sCommands) = 1 Then 
								'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								SpawnMonsterZerk(index, CShort(sCommands(1)), CShort(0), CDbl(PlayerData(index).XPos), CDbl(PlayerData(index).YPos))
							End If
							GoTo SendConfirmation
							
						Case ".spawn10"
							
							If UBound(sCommands) = 4 Then 'User only specified ID.
								Do While (spawned < 10)
									SpawnMonster(index, CShort(sCommands(1)), CShort(sCommands(2)), CDbl(sCommands(3)), CDbl(sCommands(4)))
									spawned = spawned + 1
								Loop 
							ElseIf UBound(sCommands) = 2 Then 
								Do While (spawned < 10)
									'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									SpawnMonster(index, CShort(sCommands(1)), CShort(sCommands(2)), CDbl(PlayerData(index).XPos + spawned), CDbl(PlayerData(index).YPos))
									spawned = spawned + 1
								Loop 
							ElseIf UBound(sCommands) = 1 Then 
								Do While (spawned < 10)
									'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									SpawnMonster(index, CShort(sCommands(1)), CShort(0), CDbl(PlayerData(index).XPos + spawned), CDbl(PlayerData(index).YPos))
									spawned = spawned + 1
								Loop 
							End If
							
							spawned = 0
							GoTo SendConfirmation
							
						Case ".spawn10zerk"
							
							If UBound(sCommands) = 4 Then 'User only specified ID.
								Do While (spawned < 10)
									SpawnMonsterZerk(index, CShort(sCommands(1)), CShort(sCommands(2)), CDbl(sCommands(3)), CDbl(sCommands(4)))
									spawned = spawned + 1
								Loop 
							ElseIf UBound(sCommands) = 2 Then 
								Do While (spawned < 10)
									'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									SpawnMonsterZerk(index, CShort(sCommands(1)), CShort(sCommands(2)), CDbl(PlayerData(index).XPos + spawned), CDbl(PlayerData(index).YPos))
									spawned = spawned + 1
								Loop 
							ElseIf UBound(sCommands) = 1 Then 
								Do While (spawned < 10)
									'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
									SpawnMonsterZerk(index, CShort(sCommands(1)), CShort(0), CDbl(PlayerData(index).XPos + spawned), CDbl(PlayerData(index).YPos))
									spawned = spawned + 1
								Loop 
							End If
							
							spawned = 0
							GoTo SendConfirmation
							
						Case ".teleport"
							
							GoTo SendConfirmation
							
						Case ".notice"
							If UBound(sCommands) > 0 Then 'Message entered.
								For i = 1 To UBound(sCommands)
									sMessage = sMessage & sCommands(i) & " "
								Next i
								SendNotice(sMessage)
							Else
								SendNotice("Welcome to SroMuX!")
							End If
							
							GoTo SendConfirmation
							
						Case ".speed"
							
							If UBound(sCommands) = 1 Then
								If IsNumeric(CInt(sCommands(1))) = True Then 'protects server from a crash
									ChangeSpeed(index, CInt(sCommands(1)))
								End If
							End If
							GoTo SendConfirmation
							
						Case ".spawnnpc"
							
							If IsNumeric(CInt(sCommands(1))) = True Then
								SpawnNPC(index, CInt(sCommands(1)), 1)
							End If
							
						Case ".berserk"
							
							If UBound(sCommands) = 0 Then
								HandleBerserk(index, True)
							End If
							GoTo SendConfirmation
							
						Case ".zerk"
							
							If UBound(sCommands) = 0 Then
								HandleBerserk(index, True)
							End If
							GoTo SendConfirmation
							
						Case ".item"
							
							If UBound(sCommands) = 3 Then
								If IsNumeric(CShort(sCommands(1))) = True And IsNumeric(CShort(sCommands(2))) = True And IsNumeric(CShort(sCommands(3))) = True Then
									CreateEquip1(index, CShort(sCommands(1)), CShort(sCommands(2)), CShort(sCommands(3)))
								End If
							End If
							GoTo SendConfirmation
							
						Case ".transport"
							
							If UBound(sCommands) = 1 Then
								SummonTransport(index, CShort(sCommands(1)))
							End If
							GoTo SendConfirmation
							
						Case ".pet"
							
							If UBound(sCommands) = 1 Then
								SummonGrowthPet(index, CShort(sCommands(1)))
							End If
							GoTo SendConfirmation
							
						Case ".weather"
							
							If UBound(sCommands) = 2 Then
								If (IsNumeric(CShort(sCommands(1))) = True) And (IntegerFromWord(sCommands(2)) < 256) Then
									SetWeather(CShort(sCommands(1)), IntegerFromWord(sCommands(2)))
								End If
							End If
							
						Case ".lvlup"
							
							'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            modGlobal.GameSocket(index).SendData(cv_StringFromHex("0400B0360000" & PlayerData(index).CharID))
							
							'SpawnMonster3 index
							fData = "2231"
							fData = fData & "0000"
							'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
							fData = fData & PlayerData(index).CharID
							fData = fData & "0008"
							pLen = (Len(fData) - 8) / 2
							fData = WordFromInteger(pLen) & fData
							For i = 1 To UBound(PlayerData)
								'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
								If PlayerData(i).Ingame = True Then
                                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
								End If
							Next i
							'SpawnMonster3 index
							
						Case ".hotan"
							Call HotanNpc(index)
							
						Case ".jangan"
							Call JanganNpc(index)
							
						Case ".donwhang"
							Call DowhangNpc(index)
						Case ".europe"
							Call EuropeNpc(index)
							
					End Select
				End If
				
				'Send chat messages to everyone:
				fData = "6736"
				fData = fData & "0000"
				fData = fData & "01" 'Type
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				fData = fData & PlayerData(index).CharID
				fData = fData & Right(tData, Len(tData) - 4)
				pLen = (Len(fData) - 8) / 2
				fData = WordFromInteger(pLen) & fData
				
				For i = 1 To UBound(PlayerData)
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
					End If
				Next i
				
SendConfirmation: 
				'Send confirmation to sender.
				fData = "0300"
				fData = fData & "67B3"
				fData = fData & "0000"
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				fData = fData & "0101" & ByteFromInteger(PlayerData(index).ChatIndex)
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
				
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				PlayerData(index).ChatIndex = PlayerData(index).ChatIndex + 1
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If PlayerData(index).ChatIndex = 256 Then PlayerData(index).ChatIndex = 0
				
			Case 2 'Whispers
				MsgBox(sData)
				'Send chat messages to everyone:
				fData = "6736"
				fData = fData & "0000"
				fData = fData & "01" 'Type
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				fData = fData & PlayerData(index).CharID
				fData = fData & Right(tData, Len(tData) - 4)
				pLen = (Len(fData) - 8) / 2
				fData = WordFromInteger(pLen) & fData
				
				For i = 1 To UBound(PlayerData)
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
					End If
				Next i
				
			Case 5 'Guild
				
				'Send chat messages to everyone:
				fData = "6736"
				fData = fData & "0000"
				fData = fData & "01" 'Type
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				fData = fData & PlayerData(index).CharID
				fData = fData & Right(tData, Len(tData) - 4)
				pLen = (Len(fData) - 8) / 2
				fData = WordFromInteger(pLen) & fData
				
				
				For i = 1 To UBound(PlayerData)
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
					End If
				Next i
				
				fData = "0300"
				fData = fData & "67B3"
				fData = fData & "0000"
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				fData = fData & "0101" & ByteFromInteger(PlayerData(index).ChatIndex)
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
				
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				PlayerData(index).ChatIndex = PlayerData(index).ChatIndex + 1
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If PlayerData(index).ChatIndex = 256 Then PlayerData(index).ChatIndex = 0
			Case 11 'Union
				
				'Send chat messages to everyone:
				fData = "6736"
				fData = fData & "0000"
				fData = fData & "01" 'Type
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				fData = fData & PlayerData(index).CharID
				fData = fData & Right(tData, Len(tData) - 4)
				pLen = (Len(fData) - 8) / 2
				fData = WordFromInteger(pLen) & fData
				
				For i = 1 To UBound(PlayerData)
					'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
					If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i
                fData = "0300"
                fData = fData & "67B3"
                fData = fData & "0000"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & "0101" & ByteFromInteger(PlayerData(index).ChatIndex)
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
				
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				PlayerData(index).ChatIndex = PlayerData(index).ChatIndex + 1
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ChatIndex konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If PlayerData(index).ChatIndex = 256 Then PlayerData(index).ChatIndex = 0
				
				
			Case Else
				Debug.Print("Unknown chat type: " & stype)
				
		End Select
		
	End Function
End Module