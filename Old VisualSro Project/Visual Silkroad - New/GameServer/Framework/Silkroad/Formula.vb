Namespace Functions
    Module Formula
        Public Function CalculateDistance(ByVal pos1 As Position, ByVal pos2 As Position) As Double
            Dim distanceX As Double = pos1.ToGameX - pos2.ToGameX
            Dim distanceY As Double = pos1.ToGameY - pos2.ToGameY
            Return (Math.Sqrt(distanceX * distanceX) + Math.Sqrt(distanceY * distanceY))
        End Function

        Public Function CalculateDamage(ByVal basicAP As Double, ByVal skillAP As Double, ByVal attackPowerInc As Double,
                                        ByVal enemyAccAbsorbation As Double, ByVal enemyDef As Double,
                                        ByVal balance As Double, ByVal damageInc As Double, ByVal skillAPRate As Double) As Long
            'A = Basic Attack Power
            'B = Skill Attack Power
            'C = Attack Power Increasing rate
            'D = Enemy 's total accessories Absorption rate
            'E = Enemy 's Defence Power
            'F = Balance rate
            'G = Total Damage Increasing rate
            'H = Skill Attack Power rate
            'A final damage formula:

            'Damage = ((A + B) * (1 + C) / (1 + D) - E) * F * (1 + G) * H
            Return _
                ((basicAP + skillAP) * (1 + attackPowerInc) / (1 + enemyAccAbsorbation) - enemyDef) * balance * (1 + damageInc) *
                SkillAPRate
        End Function

        Public Function GetRandomPosition(ByVal basePosition As Position, ByVal range As Integer) As Position
            If basePosition Is Nothing Then
                Return Nothing
            End If

            Dim tmpPos As Position = basePosition
            Dim tmpX As Single = basePosition.ToGameX + Rand.Next(0 - Range, 0 + Range)
            Dim tmpY As Single = basePosition.ToGameY + Rand.Next(0 - Range, 0 + Range)
            tmpPos.X = GetXOffset(tmpX)
            tmpPos.Y = GetYOffset(tmpY)

            Dim tmpXsec As Int16 = GetXSecFromGameX(tmpX)
            Dim tmpYsec As Int16 = GetYSecFromGameY(tmpY)
            If tmpXsec > 0 And tmpXsec < 255 And tmpYsec > 0 And tmpYsec < 255 Then
                tmpPos.XSector = tmpXsec
                tmpPos.YSector = tmpYsec
            Else
                Return basePosition
            End If

            Return tmpPos
        End Function

#Region "Pos Help Functions"

        Public Function ToPacketX(ByVal xSec As Byte, ByVal xPos As Single) As Single
            Return (xSec - 135) * 192 + XPos / 10
        End Function

        Public Function ToPacketY(ByVal ySec As Byte, ByVal yPos As Single) As Single
            Return (ySec - 92) * 192 + YPos / 10
        End Function

        Public Function GetXSecFromGameX(ByVal x As Single) As Single
            Return CSng(Math.Floor(CDbl(((X / 192.0!) + 135.0!))))
        End Function

        Public Function GetYSecFromGameY(ByVal y As Single) As Single
            Return CSng(Math.Floor(CDbl(((Y / 192.0!) + 92.0!))))
        End Function

        Public Function GetXOffset(ByVal x As Single) As Double
            Return CInt(Math.Round(CDbl((((((X / 192.0!) - GetXSecFromGameX(X)) + 135.0!) * 192.0!) * 10.0!))))
        End Function

        Public Function GetYOffset(ByVal y As Single) As Double
            Return CInt(Math.Round(CDbl((((((Y / 192.0!) - GetYSecFromGameY(Y)) + 92.0!) * 192.0!) * 10.0!))))
        End Function

#End Region

#Region "Angle"

        Public Function GetAngle(ByVal posFrom As Position, ByVal posTo As Position) As UShort
            Dim grad As Single
            Dim ak As Double = posFrom.ToGameX - posTo.ToGameX
            'distance_x
            Dim gk As Double = posFrom.ToGameY - posTo.ToGameY
            'distance_y

            If gk > 0 And ak > 0 Then
                grad = Math.Atan(DegreesToRadians(gk / ak))
            ElseIf gk > 0 And ak < 0 Then
                grad = (Math.Atan(DegreesToRadians(gk / ak))) * -1 + 90
            ElseIf gk < 0 And ak < 0 Then
                grad = (Math.Atan(DegreesToRadians(gk / ak))) * -1 + 180
            ElseIf gk < 0 And ak > 0 Then
                grad = (Math.Atan(DegreesToRadians(gk / ak))) * -1 + 270
            End If


            'Dim i As Microsoft .
            Return grad
        End Function

        ' convert from degrees to radians
        Private Function DegreesToRadians(ByVal degrees As Single) As Single
            DegreesToRadians = degrees * 10
            '/ 57.29578
        End Function

        ' convert from radians to degrees
        Private Function RadiansToDegrees(ByVal radians As Single) As Single
            RadiansToDegrees = radians * 57.29578
        End Function

#End Region


        Public Function GetMinPhy(ByVal stat As UShort) As Integer
            Return Convert.ToInt32(6 + ((stat - 20) \ 3))
        End Function

        Public Function GetMaxPhy(ByVal stat As UShort, ByVal level As Byte) As Integer
            Return Convert.ToInt32(9 + ((stat - 20) \ 3) + level)
        End Function

        Public Function GetMinMag(ByVal stat As UShort, ByVal level As Byte) As Integer
            If level = 1 Then
                Return Convert.ToInt32(6 + ((stat - 20) \ 3))
            Else
                Return Convert.ToInt32(7 + ((stat - 20) \ 3))
            End If
        End Function

        Public Function GetMaxMag(ByVal stat As UShort, ByVal level As Byte) As Integer
            If level = 1 Then
                Return Convert.ToInt32(10 + ((stat - 20) \ 2))
            Else
                Return Convert.ToInt32(11 + ((stat - 20) \ 2))
            End If
        End Function

        Public Function GetMagDef(ByVal stat As Integer) As Integer
            Return Convert.ToInt32(6 + ((stat - 20) \ 3))
        End Function

        Public Function GetPhyDef(ByVal stat As Integer, ByVal level As Integer) As Integer
            Return Convert.ToInt32(3 + ((stat - 20) \ 3))
        End Function

        Public Function GetWeaponMasteryLevel(ByVal Index_ As Integer) As Byte
            Dim item As cItem = GameDB.Items(Inventorys(Index_).UserItems(6).ItemID)
            Dim refitem As cRefItem = GetItemByID(item.ObjectID)
            '8-11
            If refitem.CLASS_A = 1 And refitem.CLASS_B = 6 Then
                Select Case refitem.CLASS_C
                    '=============CH
                    Case 2 'Sword
                        Return GetMasteryByID(257, Index_).Level
                    Case 3 'Blade
                        Return GetMasteryByID(257, Index_).Level
                    Case 4 'Spear
                        Return GetMasteryByID(258, Index_).Level
                    Case 5 'Glavie
                        Return GetMasteryByID(258, Index_).Level
                    Case 6 'Bow
                        Return GetMasteryByID(259, Index_).Level

                        '=========EU (6 diffs)
                    Case 7
                        'Long Sword (1 hand) == Warrior
                        Return GetMasteryByID(513, Index_).Level
                    Case 8
                        'War Sword (2 Hand)  == Warrior
                        Return GetMasteryByID(513, Index_).Level
                    Case 9
                        'Axe == Warrior
                        Return GetMasteryByID(513, Index_).Level
                    Case 10
                        'Warlock == Warlock
                        Return GetMasteryByID(516, Index_).Level
                    Case 11
                        'Long Staff (Wizard) == Wizard
                        Return GetMasteryByID(514, Index_).Level
                    Case 12
                        'Crossbow == Rouge
                        Return GetMasteryByID(515, Index_).Level
                    Case 13
                        'Dagger == ROuge
                        Return GetMasteryByID(515, Index_).Level
                    Case 14
                        'Harp  == Bard
                        Return GetMasteryByID(517, Index_).Level
                    Case 15
                        'Cleric --> Clweric
                        Return GetMasteryByID(518, Index_).Level
                End Select
            End If
            Return 0
        End Function


        Public Function HexToString(ByVal toConvert As String) As String
            Dim tmp As String = ""
            Dim istGerade As Boolean = ((toConvert.Count / 2 - Math.Truncate(toConvert.Count / 2)) = 0)
                If toConvert.Count >= 2 And istGerade Then
                    For i = 0 To toConvert.Count - 1 Step 2
                        tmp += Chr("&H" & (toConvert.Substring(i, 2)))
                    Next
                End If
            Return tmp
        End Function
    End Module
End Namespace