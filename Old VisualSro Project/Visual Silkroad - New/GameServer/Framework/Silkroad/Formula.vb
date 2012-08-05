Namespace Functions
    Module Formula
        Public Function CalculateDistance(ByVal Pos_1 As Position, ByVal Pos_2 As Position) As Double
            Dim distance_x As Double = Pos_1.ToGameX - Pos_2.ToGameX
            Dim distance_y As Double = Pos_1.ToGameY - Pos_2.ToGameY
            Return (Math.Sqrt(distance_x * distance_x) + Math.Sqrt(distance_y * distance_y))
        End Function

        Public Function CalculateDamage(ByVal BasicAP As Double, ByVal SkillAP As Double, ByVal AttackPowerInc As Double,
                                        ByVal EnemyAccAbsorbation As Double, ByVal EnemyDef As Double,
                                        ByVal Balance As Double, ByVal DamageInc As Double, ByVal SkillAPRate As Double) As Long
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
                ((BasicAP + SkillAP) * (1 + AttackPowerInc) / (1 + EnemyAccAbsorbation) - EnemyDef) * Balance * (1 + DamageInc) *
                SkillAPRate
        End Function

        Public Function GetRandomPosition(ByVal BasePosition As Position, ByVal Range As Integer) As Position
            If BasePosition Is Nothing Then
                Return Nothing
            End If

            Dim tmp_pos As Position = BasePosition
            Dim tmp_x As Single = BasePosition.ToGameX + Rand.Next(0 - Range, 0 + Range)
            Dim tmp_y As Single = BasePosition.ToGameY + Rand.Next(0 - Range, 0 + Range)
            tmp_pos.X = GetXOffset(tmp_x)
            tmp_pos.Y = GetYOffset(tmp_y)

            Dim tmpXsec As Int16 = GetXSecFromGameX(tmp_x)
            Dim tmpYsec As Int16 = GetYSecFromGameY(tmp_y)
            If tmpXsec > 0 And tmpXsec < 255 And tmpYsec > 0 And tmpYsec < 255 Then
                tmp_pos.XSector = tmpXsec
                tmp_pos.YSector = tmpYsec
            Else
                Return BasePosition
            End If

            Return tmp_pos
        End Function

#Region "Pos Help Functions"

        Public Function ToPacketX(ByVal XSec As Byte, ByVal XPos As Single) As Single
            Return (XSec - 135) * 192 + XPos / 10
        End Function

        Public Function ToPacketY(ByVal YSec As Byte, ByVal YPos As Single) As Single
            Return (YSec - 92) * 192 + YPos / 10
        End Function

        Public Function GetXSecFromGameX(ByVal X As Single) As Single
            Return CSng(Math.Floor(CDbl(((X / 192.0!) + 135.0!))))
        End Function

        Public Function GetYSecFromGameY(ByVal Y As Single) As Single
            Return CSng(Math.Floor(CDbl(((Y / 192.0!) + 92.0!))))
        End Function

        Public Function GetXOffset(ByVal X As Single) As Double
            Return CInt(Math.Round(CDbl((((((X / 192.0!) - GetXSecFromGameX(X)) + 135.0!) * 192.0!) * 10.0!))))
        End Function

        Public Function GetYOffset(ByVal Y As Single) As Double
            Return CInt(Math.Round(CDbl((((((Y / 192.0!) - GetYSecFromGameY(Y)) + 92.0!) * 192.0!) * 10.0!))))
        End Function

#End Region

#Region "Angle"

        Public Function GetAngle(ByVal Pos_From As Position, ByVal Pos_To As Position) As UShort
            Dim Grad As Single
            Dim AK As Double = Pos_From.ToGameX - Pos_To.ToGameX
            'distance_x
            Dim GK As Double = Pos_From.ToGameY - Pos_To.ToGameY
            'distance_y

            If GK > 0 And AK > 0 Then
                Grad = Math.Atan(DegreesToRadians(GK / AK))
            ElseIf GK > 0 And AK < 0 Then
                Grad = (Math.Atan(DegreesToRadians(GK / AK))) * -1 + 90
            ElseIf GK < 0 And AK < 0 Then
                Grad = (Math.Atan(DegreesToRadians(GK / AK))) * -1 + 180
            ElseIf GK < 0 And AK > 0 Then
                Grad = (Math.Atan(DegreesToRadians(GK / AK))) * -1 + 270
            End If


            'Dim i As Microsoft .
        End Function

        ' convert from degrees to radians
        Function DegreesToRadians(ByVal degrees As Single) As Single
            DegreesToRadians = degrees * 10
            '/ 57.29578
        End Function

        ' convert from radians to degrees
        Function RadiansToDegrees(ByVal radians As Single) As Single
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
            Dim _item As cInvItem = Inventorys(Index_).UserItems(6)
            Dim _refitem As cItem = GetItemByID(_item.Pk2Id)
            '8-11
            If _refitem.CLASS_A = 1 And _refitem.CLASS_B = 6 Then
                Select Case _refitem.CLASS_C
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
        End Function


        Public Function HexToString(ByVal ToConvert As String) As String
            Dim tmp As String = ""
            Try
                Dim IstGerade As Boolean = (ToConvert.Count / 2 - Math.Truncate(ToConvert.Count / 2)) = 0
                If ToConvert.Count >= 2 And IstGerade Then
                    For i = 0 To ToConvert.Count - 1 Step 2
                        tmp += Chr("&H" & (ToConvert.Substring(i, 2)))
                    Next
                End If
            Catch ex As Exception

            End Try

            Return tmp
        End Function
    End Module
End Namespace