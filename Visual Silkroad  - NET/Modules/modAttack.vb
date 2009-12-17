Option Strict Off
Option Explicit On
Module modAttack
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
	Private z As Short
	Public deger As Short
	Public mobatckid As String
	Public Function LvlMastery(ByRef index As Short) As Object
		
		Dim Mastery As String
		Mastery = Left(sData, 8)
		
		x = CShort("&H" & Mid(sData, 1, 2))
		If x > 7 Then x = x - 7
		
		'Update SP
		fData = "0600"
		fData = fData & "B330"
		fData = fData & "0000"
		fData = fData & "02"
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		If (PlayerData(index).Skillpoints - 10) < 0 Then Exit Function
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & DWordFromInteger(PlayerData(index).Skillpoints - 10) & "00"
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		iniWrite(CStr(PlayerData(index).Skillpoints), "Skillpoints", "character", PlayerData(index).CharPath)
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        'Update Mastery
        fData = "0600"
        fData = fData & "65B1"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & Mastery
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MasteryLvl konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & ByteFromInteger(PlayerData(index).MasteryLvl(x) + 1)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MasteryLvl konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).MasteryLvl(x) = PlayerData(index).MasteryLvl(x) + 1
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MasteryLvl konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(PlayerData(index).MasteryLvl(x)), "MasteryLvl(" & x & ")", "character", PlayerData(index).CharPath)
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function

    Public Function LvlSkill(ByRef index As Short) As Object

        Dim skill As String

        skill = Left(sData, 8)

        'Update sp
        fData = "0600"
        fData = fData & "B330"
        fData = fData & "0000"
        fData = fData & "02"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If (PlayerData(index).Skillpoints - 10) < 0 Then Exit Function
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Skillpoints = PlayerData(index).Skillpoints - 10
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & DWordFromInteger(PlayerData(index).Skillpoints - 10) & "00"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Skillpoints konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(CStr(PlayerData(index).Skillpoints), "Skillpoints", "character", PlayerData(index).CharPath)
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        'Update Skill
        fData = "0500"
        fData = fData & "CBB2"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & skill
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        'check if you have the same skill just lower lvl in the skill list and replace with  the new one
        'Add to skill list
        Call skillup(Right(fData, 8), index) ' save skills

    End Function

    Public Function PlayerSendAttack(ByRef index As Short) As Object
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).charstate konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).charstate = 10 Then Exit Function
        Dim ActionType As String
        'UPGRADE_WARNING: Die untere Begrenzung des Arrays ObjectID wurde von 1 in 0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        Dim ObjectID(9) As String 'was 9
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
        Dim BookMark As Object
        Dim SkillMp As String
        tim = index

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).UsingSkill konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).UsingSkill = True Then Exit Function

        ActionType = Mid(sData, 3, 2)

        Select Case ActionType

            Case "01" 'regular attack (need each attack id for each weapon?)

                ObjectID(1) = Mid(sData, 7, 8) 'this is the object type id
                Action = "Attack"

            Case "04" 'using a skill or buff

                SkillID = Mid(sData, 5, 8) 'check if its a skill or buff
                Tag = Mid(sData, 13, 2) 'if 01 then an id will follow

                If Tag = "01" Then
                    ObjectID(1) = Mid(sData, 15, 8)
                    Action = "Skill"
                Else
                    Action = "Buff"
                End If

                fData = "0200"
                fData = fData & "CDB2"
                fData = fData & "0000"
                fData = fData & "01"
                fData = fData & Tag
                For i = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(i).Ingame = True Then
                        modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                    End If
                Next i

        End Select

        Dim SkillOverID As String
        Dim mo1 As String
        Dim po1 As String
        Dim f As Short
        Dim mo3 As String
        Dim va As Double
        Dim s As Short
        Dim po3 As String ' number of mobs in world '!500 ' number of mobs in world 200 was
        Select Case Action

            Case "Buff"


                'check if hes casting/busy
                'also check if a lower buff of the same type is on

                fData = "45B2"
                fData = fData & "0000"
                fData = fData & "0100"
                fData = fData & SkillID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                fData = fData & PlayerData(index).CharID
                CastingID = (Inverse(WordFromInteger(Int(Rnd() * 1048575) + 65536))) & "000"
                fData = fData & CastingID
                fData = fData & ObjectID(1)
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

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CastingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).CastingID = CastingID
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CastingSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).CastingSkillID = SkillID
                frmMain.CastingTimer(index).Interval = 2000 ' the casting time
                frmMain.CastingTimer(index).Enabled = True

                'mp update
                'fData = "0B00"
                'fData = fData & "A633"
                'fData = fData & "0000"
                'fData = fData & PlayerData(index).CharID
                'fData = fData & "100002"
                'fData = fData & Inverse(DecToHexLong(PlayerData(index).MP - (SkillMp)))
                'PlayerData(index).MP = PlayerData(index).MP - (SkillMp) 'take away mp cost
                'modGlobal.GameSocket(index).SendData cv_StringFromHex(fData)

                '-------------------------------------------SKILLS----------------------------------------------------
            Case "Skill"
                'check DataBase if the skill is a ranged skill or not

                If RangedSkill = 0 Then '----------------------------------NON RANGED SKILL-----------------------
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).AttackSkillID = SkillID
                    For i = 1 To UBound(Mobs)
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If ObjectID(1) = Mobs(i).ID Then
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            mo1 = Mobs(i).ID
                            Exit For
                        End If
                    Next i
                    If ObjectID(1) = mo1 Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If PlayerData(index).XPos < Mobs(i).XPos Then
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            DistanceX = Mobs(i).XPos - PlayerData(index).XPos
                        Else
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            DistanceX = Mobs(i).XPos - PlayerData(index).XPos
                        End If

                        If DistanceX < 0 And DistanceX <> 0 Then
                            Distance = DistanceX * -1
                        Else
                            Distance = DistanceX
                        End If

                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If PlayerData(index).YPos < Mobs(i).YPos Then
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            DistanceY = Mobs(i).YPos - PlayerData(index).YPos
                        Else
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            DistanceY = Mobs(i).YPos - PlayerData(index).YPos
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
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            fData = fData & PlayerData(index).CharID
                            fData = fData & "01"
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            fData = fData & WordFromInteger(((PlayerData(index).XPos + DistanceX) - (PlayerData(index).XSection - 135) * 192) * 10) 'X
                            fData = fData & "0000" 'Z
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            fData = fData & WordFromInteger(((PlayerData(index).YPos + DistanceY) - ((PlayerData(index).YSection - 92)) * 192) * 10) 'Y
                            fData = fData & "00"
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            PlayerData(index).XPos = PlayerData(index).XPos + DistanceX
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            PlayerData(index).YPos = PlayerData(index).YPos + DistanceY
                            For i = 1 To UBound(PlayerData)
                                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                If PlayerData(i).Ingame = True Then
                                    modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                                End If
                            Next i

                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            PlayerData(index).AttackingID = ObjectID(1)
                            If DistanceX < 0 And DistanceX <> 0 Then DistanceX = DistanceX * -1
                            If DistanceY < 0 And DistanceX <> 0 Then DistanceY = DistanceY * -1
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'UPGRADE_WARNING: Die Timer-Eigenschaft WalkAttackDelay.Interval darf nicht den Wert 0 haben. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="169ECF4A-1968-402D-B243-16603CC08604"'
                            frmMain.WalkAttackDelay(index).Interval = (DistanceY + DistanceX) * 10000 / (((PlayerData(index).RunSpeed * (PlayerData(index).RunSpeed * 1.05)) / 100) + ((20 / PlayerData(index).RunSpeed) * 105))
                            frmMain.WalkAttackDelay(index).Enabled = True
                        Else
                            'If deger = 0 Then
                            'frmMain.mobattack.Enabled = True
                            'deger = 1
                            'End If
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            PlayerData(index).AttackingID = ObjectID(1)
                            'Mobs(index).Attacking = True
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            mobatckid = PlayerData(index).CharID
                            Call mAttackSkill(index)
                            'Call MobSendAttack(Index, i)
                        End If
                    End If
                End If
                For f = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(f).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If ObjectID(1) = PlayerData(f).CharID Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        po1 = PlayerData(f).CharID
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(f).charstate konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If PlayerData(f).charstate = 10 Then Exit Function
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(f).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(f).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If PlayerData(f).ArtHP = PlayerData(f).HP Then
                        End If
                        Exit For
                    End If
                Next f

                If ObjectID(1) = po1 Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(f).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).XPos < PlayerData(f).XPos Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceX = PlayerData(f).XPos - PlayerData(index).XPos
                    Else
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceX = PlayerData(f).XPos - PlayerData(index).XPos
                    End If

                    If DistanceX < 0 And DistanceX <> 0 Then
                        Distance = DistanceX * -1
                    Else
                        Distance = DistanceX
                    End If

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(f).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).YPos < PlayerData(f).YPos Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceY = PlayerData(f).YPos - PlayerData(index).YPos
                    Else
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceY = PlayerData(f).YPos - PlayerData(index).YPos
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
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & PlayerData(index).CharID
                        fData = fData & "01"
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & WordFromInteger(((PlayerData(index).XPos + DistanceX) - (PlayerData(index).XSection - 135) * 192) * 10) 'X
                        fData = fData & "0000" 'Z
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & WordFromInteger(((PlayerData(index).YPos + DistanceY) - ((PlayerData(index).YSection - 92)) * 192) * 10) 'Y
                        fData = fData & "00"
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).XPos = PlayerData(index).XPos + DistanceX
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).YPos = PlayerData(index).YPos + DistanceY
                        For i = 1 To UBound(PlayerData)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If PlayerData(i).Ingame = True Then
                                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                            End If
                        Next i

                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).AttackingID = ObjectID(1)
                        If DistanceX < 0 And DistanceX <> 0 Then DistanceX = DistanceX * -1
                        If DistanceY < 0 And DistanceX <> 0 Then DistanceY = DistanceY * -1
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Timer-Eigenschaft WalkAttackDelay.Interval darf nicht den Wert 0 haben. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="169ECF4A-1968-402D-B243-16603CC08604"'
                        frmMain.WalkAttackDelay(index).Interval = (DistanceY + DistanceX) * 10000 / (((PlayerData(index).RunSpeed * (PlayerData(index).RunSpeed * 1.05)) / 100) + ((20 / PlayerData(index).RunSpeed) * 105))
                        frmMain.WalkAttackDelay(index).Enabled = True
                    Else
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).AttackingID = ObjectID(1)
                        'Mobs(index).Attacking = True
                        'mobatckid = PlayerData(index).CharID
                        Call pAttackSkill(index)
                        'Call MobSendAttack(Index, i)
                    End If

                End If
                'End If


                If RangedSkill = 1 Then '------------------------------------RANGED SKILLS---------------------------

                    'ranged skills are not implemented yet, its just using the non ranged function right now
                End If

                '-------------------------------------------NORMAL ATTACKS-------------------------------------------
            Case "Attack"
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(index).Attacking = True Then Exit Function
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(index).AttackingID = ObjectID(1) Then Exit Function
                For i = 1 To 500
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If ObjectID(1) = Mobs(i).ID Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        mo3 = Mobs(i).ID
                        Exit For
                    End If
                Next i
                If mo3 = ObjectID(1) Then

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).XPos < Mobs(i).XPos Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceX = Mobs(i).XPos - PlayerData(index).XPos
                    Else
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceX = Mobs(i).XPos - PlayerData(index).XPos
                    End If

                    If DistanceX < 0 And DistanceX <> 0 Then
                        Distance = DistanceX * -1
                    Else
                        Distance = DistanceX
                    End If

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).YPos < Mobs(i).YPos Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceY = Mobs(i).YPos - PlayerData(index).YPos
                    Else
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceY = Mobs(i).YPos - PlayerData(index).YPos
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
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & PlayerData(index).CharID
                        fData = fData & "01"
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & WordFromInteger(((PlayerData(index).XPos + DistanceX) - (PlayerData(index).XSection - 135) * 192) * 10) 'X
                        fData = fData & "0000" 'Z
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & WordFromInteger(((PlayerData(index).YPos + DistanceY) - ((PlayerData(index).YSection - 92)) * 192) * 10) 'Y
                        fData = fData & "00"
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).XPos = PlayerData(index).XPos + DistanceX
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).YPos = PlayerData(index).YPos + DistanceY
                        For i = 1 To UBound(PlayerData)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If PlayerData(i).Ingame = True Then
                                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                            End If
                        Next i
                    End If

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).Attacking = True
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).AttackingID = ObjectID(1)
                    If DistanceX < 0 And DistanceX <> 0 Then DistanceX = DistanceX * -1
                    If DistanceY < 0 And DistanceY <> 0 Then DistanceY = DistanceY * -1 '50*((50*1.25)/100)+((15/50)*100))  100*((100*1.25)/100)+((20/100)*100))
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    va = CDbl(Val(CStr((DistanceY + DistanceX) * 10000 / (((PlayerData(index).RunSpeed * (PlayerData(index).RunSpeed * 1.05)) / 100) + ((20 / PlayerData(index).RunSpeed) * 105)))))
                    'UPGRADE_WARNING: Die Timer-Eigenschaft WalkAttackDelay.Interval darf nicht den Wert 0 haben. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="169ECF4A-1968-402D-B243-16603CC08604"'
                    frmMain.WalkAttackDelay(index).Interval = va
                    frmMain.WalkAttackDelay(index).Enabled = True
                End If

                For s = 1 To UBound(PlayerData)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(s).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If ObjectID(1) = PlayerData(s).CharID Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        po3 = PlayerData(s).CharID
                        Exit For
                    End If
                Next s
                If ObjectID(1) = po3 Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(s).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).XPos < PlayerData(s).XPos Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceX = PlayerData(s).XPos - PlayerData(index).XPos
                    Else
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceX = PlayerData(s).XPos - PlayerData(index).XPos
                    End If

                    If DistanceX < 0 And DistanceX <> 0 Then
                        Distance = DistanceX * -1
                    Else
                        Distance = DistanceX
                    End If

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(s).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).YPos < PlayerData(s).YPos Then
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceY = PlayerData(s).YPos - PlayerData(index).YPos
                    Else
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        DistanceY = PlayerData(s).YPos - PlayerData(index).YPos
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
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & PlayerData(index).CharID
                        fData = fData & "01"
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & WordFromInteger(((PlayerData(index).XPos + DistanceX) - (PlayerData(index).XSection - 135) * 192) * 10) 'X
                        fData = fData & "0000" 'Z
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        fData = fData & WordFromInteger(((PlayerData(index).YPos + DistanceY) - ((PlayerData(index).YSection - 92)) * 192) * 10) 'Y
                        fData = fData & "00"
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).XPos = PlayerData(index).XPos + DistanceX
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).YPos = PlayerData(index).YPos + DistanceY
                        For i = 1 To UBound(PlayerData)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If PlayerData(i).Ingame = True Then
                                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
                            End If
                        Next i
                    End If

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).Attacking = True
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).AttackingID = ObjectID(1)
                    If DistanceX < 0 And DistanceX <> 0 Then DistanceX = DistanceX * -1
                    If DistanceY < 0 And DistanceY <> 0 Then DistanceY = DistanceY * -1 '50*((50*1.25)/100)+((15/50)*100))  100*((100*1.25)/100)+((20/100)*100))
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Timer-Eigenschaft WalkAttackDelay.Interval darf nicht den Wert 0 haben. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="169ECF4A-1968-402D-B243-16603CC08604"'
                    frmMain.WalkAttackDelay(index).Interval = (DistanceY + DistanceX) * 10000 / (((PlayerData(index).RunSpeed * (PlayerData(index).RunSpeed * 1.05)) / 100) + ((20 / PlayerData(index).RunSpeed) * 105))
                    frmMain.WalkAttackDelay(index).Enabled = True
                End If
        End Select

    End Function
    Public Function NormalAttack(ByRef index As Short) As Object
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(index).AttackingID = PlayerData(i).CharID Then
                'Call pNormalAttack(index)
            Else
                Call mNormalAttack(index)
            End If
        Next i
    End Function
    Public Function mNormalAttack(ByRef index As Short) As Object
        'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim MonsterDataBase As DAO.Recordset
        Dim BookMark As Object
        Dim ObjectID As String
        Dim SkillID As String
        Dim CastingID As String
        Dim Crit As String
        Dim AfterState As String
        Dim Damage As String
        Dim NumTargets As Short

        Dim XPGain As Integer
        Dim Exp As Short
        Dim ndmg As Short
        Dim dmg1 As Short
        Dim PMinA As Short
        Dim PMaxA As Short
        Dim attacktype As String
        Dim itemname As String
        Dim attackn As Short
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PMaxA = PlayerData(index).MaxPhyAtk
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PMinA = PlayerData(index).MinPhyAtk
        ndmg = Val(CStr(PMinA - PMaxA))
        dmg1 = ((Rnd() * ndmg) + PMinA)
        OpenSremuDataBase()
        MonsterDataBase = DataBases.OpenRecordset("Items", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table


        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(MonsterDataBase.Bookmark)

        MonsterDataBase.Index = "ID"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        MonsterDataBase.Seek(">=", CharItems(index, 6).ID)

        With MonsterDataBase
            itemname = .Fields("Name").Value
        End With
        If Left(itemname, 13) = "ITEM_CH_SWORD" Then
            attackn = 2
            attacktype = "02000000"
            dmg1 = dmg1 / 2
        ElseIf Left(itemname, 13) = "ITEM_CH_BLADE" Then
            attackn = 2
            attacktype = "02000000"
            dmg1 = dmg1 / 2
        ElseIf Left(itemname, 13) = "ITEM_CH_SPEAR" Then
            attackn = 1
            attacktype = "28000000"
            dmg1 = dmg1
        ElseIf Left(itemname, 13) = "ITEM_CH_TBLAD" Then
            attackn = 1
            attacktype = "28000000"
            dmg1 = dmg1
        ElseIf Left(itemname, 11) = "TEM_CH_BOW" Then
            attackn = 1
            attacktype = "46000000"
            dmg1 = dmg1
        End If


        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ObjectID = PlayerData(index).AttackingID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Attacking = True


        fData = "45B2"
        fData = fData & "0000"
        fData = fData & "0102"
        fData = fData & attacktype 'attack type blade/sword = 0200 glaive/spear = 2800
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & (Inverse(DecToHexLong(Int(Rnd() * 1048575) + 65536))) 'over id
        fData = fData & ObjectID
        fData = fData & "01"

        fData = fData & ByteFromInteger(attackn) 'Num Attacks
        fData = fData & "01" 'Num Mobs
        fData = fData & ObjectID
        Dim a As Short
        For a = 1 To attackn
            If ((Rnd() * 10) + 1) > 9 Then 'the 10 should be the crit of the weapon
                Crit = "02"
            Else
                Crit = "01"
            End If
            Damage = CStr((Rnd() * 30) + dmg1)
            If Crit = "02" Then Damage = CStr(CDbl(Damage) * 2)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Berserking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(index).Berserking = True Then Damage = CStr(CDbl(Damage) * 2)

            Damage = Inverse(DecToHex10Long(CInt(Damage)))

            For i = 1 To 200 ' number of mobs in world
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If ObjectID = Mobs(i).ID Then
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If Mobs(i).HP <= 0 Then Exit Function
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(i).HP = Mobs(i).HP - CDbl(HexToDec(Inverse(Damage)))
                    frmMain.MobWalkTimer.Enabled = False
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If Mobs(i).HP < 0 Then
                        AfterState = "80" 'dead
                        frmMain.mobattack.Enabled = False
                        deger = 0
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().SelectedID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        PlayerData(index).SelectedID = ""
                    Else
                        AfterState = "00"
                    End If
                    Exit For
                End If
            Next i

            fData = fData & AfterState
            fData = fData & Crit
            fData = fData & Damage
            fData = fData & "0000"
        Next a
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
            frmMain.DeathTimer.Load(i)
            frmMain.DeathTimer(i).Interval = 4000
            frmMain.DeathTimer(i).Enabled = True
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Attacking = False

            'attacking is over packet
            fData = "0200"
            fData = fData & "CDB2"
            fData = fData & "0000"
            fData = fData & "0200"
            modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

            Call GetXP(index, i)
            Call GetBerserk(index, i)
            Call CheckUnique(index, ObjectID)
        End If

        frmMain.AttackDelay(index).Interval = 1350
        frmMain.AttackDelay(index).Enabled = True

    End Function
    Public Function pNormalAttack(ByRef index As Short) As Object
        Dim ObjectID As String
        Dim SkillID As String
        Dim CastingID As String
        Dim Crit As String
        Dim AfterState As String
        Dim Damage As String
        Dim NumTargets As Short
        Dim BookMark As Object
        Dim XPGain As Integer
        Dim Exp As Short
        Dim ndmg As Short
        Dim dmg1 As Short
        Dim PMinA As Short
        Dim PMaxA As Short
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PMaxA = PlayerData(index).MaxPhyAtk
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PMinA = PlayerData(index).MinPhyAtk
        ndmg = Val(CStr(PMinA - PMaxA))
        dmg1 = ((Rnd() * ndmg) + PMinA)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ObjectID = PlayerData(index).AttackingID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Attacking = True

        'Build Attack Packet
        'Build Attack Packet
        fData = "45B2"
        fData = fData & "0000"
        fData = fData & "0102"
        fData = fData & "02000000" 'attack type blade/sword = 0200 glaive/spear = 2800
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & (Inverse(DecToHexLong(Int(Rnd() * 1048575) + 65536))) 'over id
        fData = fData & ObjectID
        fData = fData & "01"
        fData = fData & "01" 'Num Attacks
        fData = fData & "01" 'Num Mobs
        fData = fData & ObjectID

        If ((Rnd() * 10) + 1) > 9 Then 'the 10 should be the crit of the weapon
            Crit = "02"
        Else
            Crit = "01"
        End If
        Damage = CStr((Rnd() * 30) + dmg1)
        If Crit = "02" Then Damage = CStr(CDbl(Damage) * 2)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Berserking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).Berserking = True Then Damage = CStr(CDbl(Damage) * 2)

        Damage = Inverse(DecToHex10Long(CInt(Damage)))

        For i = 1 To UBound(PlayerData) ' number of mobs in world
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If ObjectID = PlayerData(i).CharID Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).ArtHP <= 0 Then Exit Function
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(i).ArtHP = PlayerData(i).ArtHP - CDbl(HexToDec(Inverse(Damage)))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If PlayerData(i).ArtHP < 0 Then
                    AfterState = "80" 'dead
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().SelectedID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PlayerData(index).SelectedID = ""
                Else
                    AfterState = "00"
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

            fData = "0200"
            fData = fData & "CDB2"
            fData = fData & "0000"
            fData = fData & "0200"
            modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
        End If

        frmMain.AttackDelay(index).Interval = 1350
        frmMain.AttackDelay(index).Enabled = True

    End Function
    Public Function AttackSkill4(ByRef index As Short) As Object
        'UPGRADE_WARNING: Die untere Begrenzung des Arrays ObjectID wurde von 1 in 0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        Dim ObjectID(9) As String
        Dim SkillID As String
        Dim CastingID As String
        Dim Crit As String
        Dim AfterState As String
        Dim Damage As String
        Dim NumTargets As Short
        Dim TimerVar As Short
        '-----------------------
        Dim Exp As Short

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ObjectID(1) = PlayerData(index).AttackingID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        SkillID = PlayerData(index).AttackSkillID
        NumTargets = 2

        fData = "45B2"
        fData = fData & "0000"
        fData = fData & "0102"
        fData = fData & SkillID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        CastingID = (Inverse(DecToHexLong(Int(Rnd() * 1548575) + 65536)))
        fData = fData & CastingID
        fData = fData & ObjectID(1)
        fData = fData & "00"

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'then you need to wait the casting time then

        fData = "05B5"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & CastingID
        fData = fData & ObjectID(1)
        fData = fData & "01"
        fData = fData & "01" 'Num Attacks
        fData = fData & "02"

        'For x = 1 To NumTargets

        fData = fData & ObjectID(1) 'was x
        If ((Rnd() * 50) + 10) > 35 Then 'the 10 should be the crit of the weapon
            Crit = "02"
        Else
            Crit = "01"
        End If
        Damage = CStr((Rnd() * 12000) + 800) 'was 2000
        If Crit = "02" Then Damage = CStr(CDbl(Damage) * 2)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Berserking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).Berserking = True Then Damage = CStr(CDbl(Damage) * 2)
        Damage = Inverse(DecToHex10Long(CInt(Damage)))

        For i = 1 To 500 'Number of mobs in area
            Exp = i 'ndex ' right index :)

            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If ObjectID(1) = Mobs(i).ID Then 'was ObjectID(x)
                frmMain.MobWalkTimer.Enabled = False
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Mobs(i).HP <= 0 Then Exit Function
                TimerVar = i
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Mobs(i).HP = Mobs(i).HP - CDbl(HexToDec(Inverse(Damage)))
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Mobs(i).HP < 0 Then
                    AfterState = "80" 'dead
                Else
                    AfterState = "00"
                End If
                Exit For
            End If
        Next i

        fData = fData & AfterState
        fData = fData & Crit
        fData = fData & Damage
        fData = fData & "0000"
        If NumTargets = 2 Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Mobs(dex).ID
            fData = fData & AfterState
            fData = fData & Crit
            fData = fData & Damage
            fData = fData & "0000"
        End If
        'Next x

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'Skill Over
        fData = "0200"
        fData = fData & "CDB2"
        fData = fData & "0000"
        fData = fData & "0200"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        'mp update
        'fData = "0B00"
        'fData = fData & "A633"
        'fData = fData & "0000"
        'fData = fData & PlayerData(index).CharID
        'fData = fData & "100002"
        'fData = fData & Inverse(DecToHexLong(PlayerData(index).MP - (SkillMp)))
        'PlayerData(index).MP = PlayerData(index).MP - (SkillMp) 'take away mp cost
        'modGlobal.GameSocket(index).SendData cv_StringFromHex(fData)

        'If mob is dead
        If AfterState = "80" Then
            frmMain.MobWalkTimer.Enabled = True
            Call CheckUnique(index, ObjectID(1))
            'remove dead mob after 4 seconds
            frmMain.DeathTimer.Load(TimerVar)
            frmMain.DeathTimer(TimerVar).Interval = 4000
            frmMain.DeathTimer(TimerVar).Enabled = True
            '------------------------------------------------------------
            Call GetXP(index, Exp) 'was i

            Exit Function ' fixed freezes skill after mob dead
        End If

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().UsingSkill konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).UsingSkill = True
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).AttackSkillID = ""
        frmMain.AttackDelay(index).Interval = 2200 'skill delay
        frmMain.AttackDelay(index).Enabled = True
        frmMain.MobWalkTimer.Enabled = True


    End Function


    Public Function CheckUnique(ByRef index As Short, ByRef ID As String) As Object
        'If the killed monster was a unique, we shell inform the players
        If ID = UniqueID(1) Then 'Tiger Girl
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(True, PlayerData(index).Charname, CShort("1954"))
        End If
        If ID = UniqueID(2) Then 'Tiger Girl
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(True, PlayerData(index).Charname, CShort("1982"))
        End If
        If ID = UniqueID(3) Then 'Tiger Girl
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(True, PlayerData(index).Charname, CShort("2002"))
        End If
        If ID = UniqueID(4) Then 'Tiger Girl
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(True, PlayerData(index).Charname, CShort("3810"))
        End If
        If ID = UniqueID(5) Then 'Tiger Girl
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(True, PlayerData(index).Charname, CShort("23638"))
        End If
        If ID = UniqueID(6) Then 'Captin Ivy
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            UniqueAnnounce(True, PlayerData(index).Charname, CShort("23639"))
        End If

    End Function
    'w huj do poprawy , oj w huj
    Public Function GetXP(ByRef index As Short, ByRef mobindex As Short) As Object
        Dim Mob As Integer
        Dim MobLvl As String
        Dim MobId As String
        'UPGRADE_WARNING: Arrays in Struktur MonsterDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim MonsterDataBase As DAO.Recordset
        'UPGRADE_WARNING: Arrays in Struktur ServerDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim ServerDataBase As DAO.Recordset
        Dim BookMark As Object
        Dim Xps As String
        Dim ServerXpRate As String
        Dim ServerSpRate As String
        Dim sp As Short
        sp = 200
        OpenSremuDataBase()
        MonsterDataBase = DataBases.OpenRecordset("Monsters", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table
        ServerDataBase = DataBases.OpenRecordset("Server", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table

        'For x = 1 To 500 was
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Xps konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Mob = Mobs(mobindex).Xps 'was x
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Type konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        MobId = Mobs(mobindex).Type
        'Exit For was
        'Next x was

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(MonsterDataBase.Bookmark)

        MonsterDataBase.Index = "Id"
        MonsterDataBase.Seek(">=", MobId)

        With MonsterDataBase
            MobLvl = .Fields("Lvl").Value
            Xps = .Fields("Xp").Value
        End With
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts BookMark konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        BookMark = VB6.CopyArray(ServerDataBase.Bookmark)

        With ServerDataBase
            ServerXpRate = .Fields("XpRate").Value
            'ServerSpRate = !SpRate
        End With

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).level < MobLvl Then 'Mob Higher Level
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Xps = CStr(((CDbl(MobLvl) - PlayerData(index).level) * CDbl(Xps)) * CDbl(ServerXpRate))
        End If
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).level > MobLvl Then 'Mob Lower Level
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Xps = CStr(((PlayerData(index).level - CDbl(MobLvl)) / CDbl(Xps)) * CDbl(ServerXpRate))
        End If
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).level konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).level = MobLvl Then 'Mob and Player Same Lvl
            Xps = CStr(CDbl(Xps) * CDbl(ServerXpRate))
        End If

        'sp/xp
        fData = "0D00"
        fData = fData & "D230"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Mobs(mobindex).ID
        fData = fData & Inverse(DecToHexLong(CInt(Xps)))
        fData = fData & Inverse(DecToHexLong(sp))
        fData = fData & "00"
        'PlayerData(index).SpBar = PlayerData(index).SpBar + sp
        'If PlayerData(index).SpBar > 400 Then
        'PlayerData(index).Skillpoints = PlayerData(index).Skillpoints + 1
        'End If
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        'fData = "B330"
        'fData = fData & "0000"
        'fData = fData & "01"    'Type
        'fData = fData & ByteFromInteger(70)
        'fData = fData & "2D16000000"
        'pLen = (Len(fData) - 8) / 2
        'fData = WordFromInteger(pLen) & fData
        'modGlobal.GameSocket(index).SendData cv_StringFromHex(fData)

    End Function

    Public Function GetBerserk(ByRef index As Short, ByRef mobindex As Short) As Object
        'Berserk orb
        If ((Rnd() * 10) + 0) > 7 Then 'not sure what the chance of getting a Berserk orb is 30% isnt bad
            fData = "0600"
            fData = fData & "B330"
            fData = fData & "0000"
            fData = fData & "04"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkBar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & ByteFromInteger(PlayerData(index).BerserkBar + 1)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Mobs(mobindex).ID
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).BerserkBar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(index).BerserkBar < 5 Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).BerserkBar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().BerserkBar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                PlayerData(index).BerserkBar = PlayerData(index).BerserkBar + 1
                modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
            End If
        End If
    End Function

    Public Function Attack(ByRef index As Short) As Object

        'UPGRADE_WARNING: Die untere Begrenzung des Arrays ObjectID wurde von 1 in 0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        Dim ObjectID(9) As String
        Dim SkillID As String
        Dim CastingID As String
        Dim Crit As String
        Dim AfterState As String
        Dim Damage As String
        Dim NumTargets As Short
        Dim TimerVar As Short
        Dim Exp As Short
        fData = "05B5"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & CastingID
        fData = fData & ObjectID(2)
        fData = fData & "01"
        fData = fData & "01" 'Num Attacks
        fData = fData & ByteFromInteger(NumTargets)

        For x = 1 To NumTargets

            fData = fData & ObjectID(2) 'was x
            If ((Rnd() * 50) + 10) > 52 Then 'the 10 should be the crit of the weapon
                Crit = "02"
            Else
                Crit = "01"
            End If
            Damage = CStr((Rnd() * 50) + 350) 'was 2000
            If Crit = "02" Then Damage = CStr(CDbl(Damage) * 2)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Berserking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(index).Berserking = True Then Damage = CStr(CDbl(Damage) * 2)
            Damage = Inverse(DecToHex10Long(CInt(Damage)))

            For i = 1 To 500 'Number of mobs in area
                Exp = i 'ndex ' right index :)

                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If ObjectID(1) = Mobs(i).ID Then 'was ObjectID(x)
                    frmMain.MobWalkTimer.Enabled = False
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If Mobs(i).HP <= 0 Then Exit Function
                    TimerVar = i
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Mobs(i).HP = Mobs(i).HP - CDbl(HexToDec(Inverse(Damage)))
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If Mobs(i).HP < 0 Then
                        AfterState = "80" 'dead
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().State konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        Mobs(index).State = "80"
                    Else
                        AfterState = "00"
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().State konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        Mobs(index).State = "00"
                    End If
                    Exit For
                End If
            Next i

            fData = fData & AfterState
            fData = fData & Crit
            fData = fData & Damage
            fData = fData & "0000"

        Next x

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'Skill Over
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

        'mp update
        'fData = "0B00"
        'fData = fData & "A633"
        'fData = fData & "0000"
        'fData = fData & PlayerData(index).CharID
        'fData = fData & "100002"
        'fData = fData & Inverse(DecToHexLong(PlayerData(index).MP - (SkillMp)))
        'PlayerData(index).MP = PlayerData(index).MP - (SkillMp) 'take away mp cost
        'modGlobal.GameSocket(index).SendData cv_StringFromHex(fData)

        'If mob is dead
        If AfterState = "80" Then
            'frmMain.MobWalkTimer.Enabled = True
            Call CheckUnique(index, ObjectID(1))
            'remove dead mob after 4 seconds
            frmMain.DeathTimer.Load(TimerVar)
            frmMain.DeathTimer(TimerVar).Interval = 4000
            frmMain.DeathTimer(TimerVar).Enabled = True
            '------------------------------------------------------------
            Call GetXP(index, Exp) 'was i

            Exit Function ' fixed freezes skill after mob dead
        End If
    End Function
    Public Function PlayerSpawn(ByRef index As Short) As Object
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().charstate konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).charstate = 1

        fData = "2231"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "0001"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        fData = "2231"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "0402"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "0B00"
        fData = fData & "A633"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "200001"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(DecToHexLong(PlayerData(index).ArtHP + PlayerData(index).HP))
        'PlayerData(index).MP = PlayerData(index).MP - (sMana) 'take away mp cost
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).ArtHP = PlayerData(index).HP
        'iniWrite CInt(mps), "ArtMP", "character", PlayerData(index).CharPath
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


    End Function
    Public Function ColdEffect(ByRef index As Short, ByRef oid As String, ByRef skillid1 As String) As Object
        For i = 1 To UBound(Mobs)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If oid = Mobs(i).ID Then
                frmMain.mobattack.Interval = 5000
            End If
        Next i

        fData = "2231"
        fData = fData & "0000"
        fData = fData & oid
        fData = fData & "0801"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        fData = "2231"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "0801"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        fData = "5334"
        fData = fData & "0000"
        fData = fData & oid
        fData = fData & "00004843"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        fData = "19B4"
        fData = fData & "0000"
        fData = fData & oid
        fData = fData & skillid1 & "00000000"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "A633"
        fData = fData & "0000"
        fData = fData & oid
        fData = fData & "00010402000000"
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
        fData = fData & "0200"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function
    Public Function LowMana(ByRef index As Short) As Object
        fData = "45B2"
        fData = fData & "0000"
        fData = fData & "0204"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        fData = "CDB2"
        fData = fData & "0000"
        fData = fData & "030004"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

    End Function
    Public Function mAttackSkill(ByRef index As Short) As Object
        'UPGRADE_WARNING: Arrays in Struktur SkillDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim SkillDataBase As DAO.Recordset
        'UPGRADE_WARNING: Arrays in Struktur SkillDataBase2 müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim SkillDataBase2 As DAO.Recordset
        Dim BookMark As Object
        'UPGRADE_WARNING: Die untere Begrenzung des Arrays ObjectID wurde von 1 in 0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        Dim ObjectID(9) As String
        Dim SkillID As String
        'SkillID = DWordFromInteger(707)
        Dim CastingID As String
        Dim Crit As String
        Dim AfterState As String
        Dim Damage As String
        Dim Damage2 As String
        Dim Damage3 As String
        Dim NumTargets As Short
        Dim TimerVar As Short
        Dim dmg1 As Double
        Dim dmg2 As Double
        Dim tdmg As Double
        Dim tdmg1 As Double
        Dim PMinA As Short
        Dim PMaxA As Short
        Dim SMinA As Short
        Dim SMaxA As Short
        Dim SPer As Short
        Dim Exp As Short
        Dim ndmg As Double
        Dim sdmg As Short
        Dim smana As Short
        Dim SkillName As String
        Dim SkillName1 As String
        Dim attackn As Short
        Dim skill As Short
        frmMain.MobWalkTimer.Enabled = False
        'dmg1 = ((Rnd * 5000) + 1000)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ObjectID(1) = PlayerData(index).AttackingID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        SkillID = PlayerData(index).AttackSkillID
        skill = IntegerFromWord(Mid(SkillID, 1, 4))

        Dim db As ADODB.Connection
        Dim rs As ADODB.Recordset
        Dim rs3 As ADODB.Recordset
        db = New ADODB.Connection
        rs = New ADODB.Recordset
        rs3 = New ADODB.Recordset
        db.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & My.Application.Info.DirectoryPath & "\DataBase\SremuDatabase.mdb;")


        rs.Open("SELECT * FROM " & "Chinnese_Skills" & " WHERE Alan2 = '" & skill & "'", db, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rs.RecordCount > 0 Then
            SkillName1 = rs.Fields("Alan4").Value
            smana = rs.Fields("Mana").Value
            SMinA = rs.Fields("Min").Value
            SMaxA = rs.Fields("Max").Value
            SPer = rs.Fields("Per").Value
            attackn = rs.Fields("NumberAttack").Value
            'bulduk
        End If
        rs.Close()


        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).ArtMP < (smana) Then
            Call LowMana(index)
            Exit Function
        End If

        fData = "0B00"
        fData = fData & "A633"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "100002"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(DecToHexLong(PlayerData(index).ArtMP - (smana)))
        'PlayerData(index).MP = PlayerData(index).MP - (sMana) 'take away mp cost
        Dim mps As Short
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).ArtMP = PlayerData(index).ArtMP - (smana)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        mps = PlayerData(index).ArtMP

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ObjectID(1) = PlayerData(index).AttackingID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        SkillID = PlayerData(index).AttackSkillID
        NumTargets = 1

        fData = "45B2"
        fData = fData & "0000"
        fData = fData & "0102"
        fData = fData & SkillID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        CastingID = (Inverse(DecToHexLong(Int(Rnd() * 1548575) + 65536)))
        fData = fData & CastingID
        fData = fData & ObjectID(1)
        fData = fData & "00"

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        Select Case SkillName
            Case "Cold wave-Arrest"
                ColdEffect(index, ObjectID(1), SkillID)
                Exit Function
                'then you need to wait the casting time then
            Case "Cold wave-Binding"
                ColdEffect(index, ObjectID(1), SkillID)
                Exit Function
            Case "Cold wave-Shackle"
                ColdEffect(index, ObjectID(1), SkillID)
                Exit Function
            Case "Cold Wave - Freeze"
                ColdEffect(index, ObjectID(1), SkillID)
                Exit Function
        End Select
        'then you need to wait the casting time then

        fData = "05B5"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & CastingID
        fData = fData & ObjectID(1)
        fData = fData & "01"
        fData = fData & ByteFromInteger(attackn) 'Num Attacks
        fData = fData & ByteFromInteger(NumTargets)
        Dim Y As Short
        Dim s As String
        Dim skill2 As Object
        Dim d As String
        Dim ReSkillname As String
        Dim tdef As Short ' Combos Damage calc
        For x = 1 To NumTargets
            fData = fData & ObjectID(1) 'was x
            For Y = 1 To attackn
                If AfterState = "80" Then Exit For
                If ((Rnd() * 10) + 1) > 10 Then 'the 10 should be the crit of the weapon
                    Crit = "02"
                Else
                    Crit = "00"
                End If

                For i = 1 To 500 'Number of mobs in area
                    If Y > 1 Then

                        If Left(SkillName1, 20) = "SKILL_CH_SPEAR_CHAIN" Then

                            s = Right(SkillName1, 4)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            skill2 = Left(SkillName1, Len(SkillName1) - 5)
                            d = CStr(CShort(Y) & s)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ReSkillname = (skill2 & d)

                        ElseIf Left(SkillName1, 20) = "SKILL_CH_SWORD_CHAIN" Then

                            s = Right(SkillName1, 4)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            skill2 = Left(SkillName1, Len(SkillName1) - 5)
                            d = CStr(CShort(Y) & s)

                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ReSkillname = (skill2 & d)

                        Else
                            s = Right(SkillName1, 3)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            skill2 = Left(SkillName1, Len(SkillName1) - 3)
                            d = CStr(CShort(Y) & s)

                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ReSkillname = (skill2 & d)

                        End If
                        rs3.Open("SELECT * FROM " & "Chinnese_Skills" & " WHERE Alan4 = '" & ReSkillname & "'", db, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

                        If rs3.RecordCount > 0 Then
                            SMinA = rs3.Fields("Min").Value
                            SMaxA = rs3.Fields("Max").Value
                            SPer = rs3.Fields("Per").Value
                        End If
                        rs3.Close()
                        If SMinA = CDbl("0") Or SMaxA = CDbl("0") Then
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            SMinA = PlayerData(index).MaxPhyAtk
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            SMaxA = PlayerData(index).MinPhyAtk
                            SendNotice("Skill Error")
                        End If

                    End If

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PMaxA = PlayerData(index).MaxPhyAtk + SMaxA
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PMinA = PlayerData(index).MinPhyAtk + SMinA
                    dmg1 = CDbl(Val(CStr(PMaxA)) * Val(CStr(SPer)) / 100)
                    dmg2 = CDbl(Val(CStr(PMinA)) * Val(CStr(SPer)) / 100)
                    ndmg = CDbl(Val(CStr(dmg1)) - Val(CStr(dmg2)))
                    tdmg = CDbl(Val(CStr(dmg1 + ndmg)))
                    tdmg1 = ((Rnd() * 50) + tdmg)


                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().pDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tdef = Mobs(i).pDef
                    Damage = CStr(tdmg1 - (tdef / attackn)) 'was 2000
                    If CDbl(Damage) < 0 Then
                        Damage = CStr((Rnd() * 20) + 100)
                    End If

                    If Crit = "02" Then Damage = CStr(CDbl(Damage) * 2)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Berserking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).Berserking = True Then Damage = CStr(CDbl(Damage) * 2)
                    If frmMain.BerserkTimer(index).Enabled = True Then Damage = CStr(CDbl(Damage) * 2)

                    Damage = Inverse(DecToHex10Long(CInt(Damage)))


                    Exp = i 'ndex ' right index :)

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If ObjectID(1) = Mobs(i).ID Then 'was ObjectID(x)
                        frmMain.MobWalkTimer.Enabled = False
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If Mobs(i).HP <= 0 Then Exit Function
                        TimerVar = i
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        Mobs(i).HP = Mobs(i).HP - CDbl(HexToDec(Inverse(Damage)))
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If Mobs(i).HP < 0 Then
                            AfterState = "80"
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            Mobs(i).Attacking = False
                            frmMain.mobattack.Enabled = False
                            deger = 0
                            'Exit For
                            'PlayerData(index).SelectedID = ""
                        Else
                            AfterState = "00"
                        End If
                        Exit For
                    End If
                Next i

                fData = fData & AfterState
                fData = fData & Crit
                fData = fData & Damage
                fData = fData & "0000"

            Next Y
        Next x


        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        'MsgBox (fData)
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'Skill Over
        fData = "0200"
        fData = fData & "CDB2"
        fData = fData & "0000"
        fData = fData & "0200"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        If AfterState = "80" Then

            Call CheckUnique(index, ObjectID(1))
            frmMain.DeathTimer.Load(TimerVar)
            frmMain.DeathTimer(TimerVar).Interval = 4000
            frmMain.DeathTimer(TimerVar).Enabled = True
            'Call GetXP(index, Exp) 'was i

            Exit Function ' fixed freezes skill after mob dead
        End If

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().UsingSkill konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).UsingSkill = True
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).AttackSkillID = ""
        frmMain.AttackDelay(index).Interval = 2200 'skill delay
        frmMain.AttackDelay(index).Enabled = True
        'frmMain.MobWalkTimer.Enabled = True

        'If mob is dead

    End Function
    Public Function pAttackSkill(ByRef index As Short) As Object
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).charstate konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).charstate = 10 Then Exit Function
        'UPGRADE_WARNING: Arrays in Struktur SkillDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim SkillDataBase As DAO.Recordset
        'UPGRADE_WARNING: Arrays in Struktur SkillDataBase2 müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim SkillDataBase2 As DAO.Recordset
        Dim BookMark As Object
        'UPGRADE_WARNING: Die untere Begrenzung des Arrays ObjectID wurde von 1 in 0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        Dim ObjectID(9) As String
        Dim SkillID As String
        Dim CastingID As String
        Dim Crit As String
        Dim AfterState As String
        Dim Damage As String
        Dim Damage2 As String
        Dim Damage3 As String
        Dim NumTargets As Short
        Dim TimerVar As Short
        Dim dmg1 As Double
        Dim dmg2 As Double
        Dim tdmg As Double
        Dim tdmg1 As Double
        Dim PMinA As Short
        Dim PMaxA As Short
        Dim SMinA As Short
        Dim SMaxA As Short
        Dim SPer As Short
        Dim Exp As Short
        Dim ndmg As Double
        Dim sdmg As Short
        Dim smana As Short
        Dim SkillName As String
        Dim SkillName1 As String
        Dim attackn As Short
        Dim skill As Short
        OpenSremuDataBase()
        SkillDataBase = DataBases.OpenRecordset("Skills", DAO.RecordsetTypeEnum.dbOpenTable) 'DB Table

        'dmg1 = ((Rnd * 5000) + 1000)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ObjectID(1) = PlayerData(index).AttackingID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        SkillID = PlayerData(index).AttackSkillID
        SkillDataBase.Index = "Id"
        SkillDataBase.Seek(">=", SkillID)
        Dim db As ADODB.Connection
        Dim rs As ADODB.Recordset
        Dim rs3 As ADODB.Recordset
        db = New ADODB.Connection
        rs = New ADODB.Recordset
        rs3 = New ADODB.Recordset
        db.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & My.Application.Info.DirectoryPath & "\DataBase\SremuDatabase.mdb;")
        Dim tablolar As Object
        Dim tablo As Object
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts tablolar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        tablolar = "Skilldata_10000|Skilldata_15000|SkillData_5000|Skilldata_20000|Skilldata_25000|Skilldata_30000|Skilldata_35000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts tablolar konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts tablo konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        tablo = Split(tablolar, "|")

        Dim t As Short
        For t = 0 To UBound(tablo)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts tablo() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            rs.Open("SELECT * FROM " & tablo(t) & " WHERE Alan2 = '" & skill & "'", db, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
            If rs.RecordCount > 0 Then
                SkillName1 = rs.Fields("Alan4").Value
                smana = rs.Fields("Mana").Value
                SMinA = rs.Fields("Min").Value
                SMaxA = rs.Fields("Max").Value
                SPer = rs.Fields("Per").Value
                'bulduk
                Exit For
            End If
            rs.Close()
        Next t

        With SkillDataBase
            attackn = .Fields("NumberofAttacks").Value
        End With


        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).ArtMP < (smana) Then
            Call LowMana(index)
            Exit Function
        End If
        fData = "0B00"
        fData = fData & "A633"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "100002"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(DecToHexLong(PlayerData(index).ArtMP - (smana)))
        'PlayerData(index).MP = PlayerData(index).MP - (sMana) 'take away mp cost
        Dim mps As Short
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).ArtMP = PlayerData(index).ArtMP - (smana)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        mps = PlayerData(index).ArtMP

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        '-----------------------
        'Dim Exp As Integer

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        SkillID = PlayerData(index).AttackSkillID

        NumTargets = 1
        fData = "45B2"
        fData = fData & "0000"
        fData = fData & "0102"
        fData = fData & SkillID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        CastingID = (Inverse(DecToHexLong(Int(Rnd() * 1548575) + 65536)))
        fData = fData & CastingID
        fData = fData & ObjectID(1)
        fData = fData & "00"

        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        Select Case SkillName
            Case "Cold wave-Arrest"
                ColdEffect(index, ObjectID(1), SkillID)
                Exit Function
                'then you need to wait the casting time then
            Case "Cold wave-Binding"
                ColdEffect(index, ObjectID(1), SkillID)
                Exit Function
            Case "Cold wave-Shackle"
                ColdEffect(index, ObjectID(1), SkillID)
                Exit Function
            Case "Cold Wave - Freeze"
                ColdEffect(index, ObjectID(1), SkillID)
                Exit Function
        End Select
        'then you need to wait the casting time then

        fData = "05B5"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & CastingID
        fData = fData & ObjectID(1)
        fData = fData & "01"
        fData = fData & ByteFromInteger(attackn) 'Num Attacks
        fData = fData & ByteFromInteger(NumTargets)
        Dim Y As Short
        Dim s As String
        Dim skill2 As Object
        Dim d As String
        Dim ReSkillname As String
        Dim tdef As Short ' Combos Damage calc
        For x = 1 To NumTargets
            fData = fData & ObjectID(1) 'was x
            For Y = 1 To attackn
                If AfterState = "80" Then Exit For
                If ((Rnd() * 10) + 1) > 10 Then 'the 10 should be the crit of the weapon
                    Crit = "02"
                Else
                    Crit = "01"
                End If

                For i = 1 To 500 'Number of mobs in area
                    If Y > 1 Then

                        If Left(SkillName1, 20) = "SKILL_CH_SPEAR_CHAIN" Then

                            s = Right(SkillName1, 4)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            skill2 = Left(SkillName1, Len(SkillName1) - 5)
                            d = CStr(CShort(Y) & s)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ReSkillname = (skill2 & d)

                        ElseIf Left(SkillName1, 20) = "SKILL_CH_SWORD_CHAIN" Then

                            s = Right(SkillName1, 4)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            skill2 = Left(SkillName1, Len(SkillName1) - 5)
                            d = CStr(CShort(Y) & s)

                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ReSkillname = (skill2 & d)

                        Else
                            s = Right(SkillName1, 3)
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            skill2 = Left(SkillName1, Len(SkillName1) - 3)
                            d = CStr(CShort(Y) & s)

                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts skill2 konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            ReSkillname = (skill2 & d)

                        End If
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts tablo() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        rs3.Open("SELECT * FROM " & tablo(t) & " WHERE Alan4 = '" & ReSkillname & "'", db, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)

                        If rs3.RecordCount > 0 Then
                            SMinA = rs3.Fields("Min").Value
                            SMaxA = rs3.Fields("Max").Value
                            SPer = rs3.Fields("Per").Value
                        End If
                        rs3.Close()
                        If SMinA = CDbl("0") Or SMaxA = CDbl("0") Then
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            SMinA = PlayerData(index).MaxPhyAtk
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            SMaxA = PlayerData(index).MinPhyAtk
                            SendNotice("Skill Error")
                        End If

                    End If

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MaxPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PMaxA = PlayerData(index).MaxPhyAtk + SMaxA
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().MinPhyAtk konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    PMinA = PlayerData(index).MinPhyAtk + SMinA
                    dmg1 = CDbl(Val(CStr(PMaxA)) * Val(CStr(SPer)) / 100)
                    dmg2 = CDbl(Val(CStr(PMinA)) * Val(CStr(SPer)) / 100)
                    ndmg = CDbl(Val(CStr(dmg1)) - Val(CStr(dmg2)))
                    tdmg = CDbl(Val(CStr(dmg1 + ndmg)))
                    tdmg1 = ((Rnd() * 50) + tdmg)


                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().pDef konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    tdef = Mobs(i).pDef
                    Damage = CStr(tdmg1 - tdef) 'was 2000
                    If Crit = "02" Then Damage = CStr(CDbl(Damage) * 2)
                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Berserking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If PlayerData(index).Berserking = True Then Damage = CStr(CDbl(Damage) * 2)
                    If frmMain.BerserkTimer(index).Enabled = True Then Damage = CStr(CDbl(Damage) * 2)

                    Damage = Inverse(DecToHex10Long(CInt(Damage)))


                    Exp = i 'ndex ' right index :)

                    'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If ObjectID(1) = Mobs(i).ID Then 'was ObjectID(x)
                        frmMain.MobWalkTimer.Enabled = False
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If Mobs(i).HP <= 0 Then Exit Function
                        TimerVar = i
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        Mobs(i).HP = Mobs(i).HP - CDbl(HexToDec(Inverse(Damage)))
                        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(i).HP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        If Mobs(i).HP < 0 Then
                            AfterState = "80"
                            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs().Attacking konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            Mobs(i).Attacking = False
                            frmMain.mobattack.Enabled = False
                            deger = 0
                            'Exit For
                            'PlayerData(index).SelectedID = ""
                        Else
                            AfterState = "00"
                        End If
                        Exit For
                    End If
                Next i

                fData = fData & AfterState
                fData = fData & Crit
                fData = fData & Damage
                fData = fData & "0000"

            Next Y
        Next x


        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'Skill Over
        fData = "0200"
        fData = fData & "CDB2"
        fData = fData & "0000"
        fData = fData & "0200"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        If AfterState = "80" Then

            'SendNotice PlayerData(index).Charname & " KILLED " & Charname

            Exit Function

        End If

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().UsingSkill konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).UsingSkill = True
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackSkillID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).AttackSkillID = ""
        frmMain.AttackDelay(index).Interval = 2200 'skill delay
        frmMain.AttackDelay(index).Enabled = True
        'frmMain.MobWalkTimer.Enabled = True

    End Function
	Public Function AttackSkill(ByRef index As Short) As Object
		For i = 1 To UBound(PlayerData)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).AttackingID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			If PlayerData(index).AttackingID = PlayerData(i).CharID Then
				Call pAttackSkill(index)
			Else
				Call mAttackSkill(index)
			End If
		Next i
	End Function
End Module