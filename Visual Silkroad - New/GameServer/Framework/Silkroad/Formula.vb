Namespace GameServer.Functions
    Module Formula
        Public Function CalculateDistance(ByVal Pos_1 As Position, ByVal Pos_2 As Position) As Double
            'Get Real Cords
            Dim Pos1X As Double = Pos_1.ToGameX
            Dim Pos1Y As Double = Pos_1.ToGameY
            Dim Pos2X As Double = Pos_2.ToGameX
            Dim Pos2Y As Double = Pos_2.ToGameY

            Dim distance_x As Double = Pos1X - Pos2X
            Dim distance_y As Double = Pos1Y - Pos2Y

            Dim x As Double = 0
            If distance_x < 0 And distance_x <> 0 Then
                x = distance_x * -1
            Else
                x = distance_x
            End If
            If distance_y < 0 And distance_y <> 0 Then
                Return ((distance_y * -1) + x)
            End If

            Return x + distance_y
        End Function
        Public Function CalculateDistance2(ByVal Pos_1 As Position, ByVal Pos_2 As Position) As Double
            Dim distance_x As Double = Pos_1.ToGameX - Pos_2.ToGameX
            Dim distance_y As Double = Pos_1.ToGameY - Pos_2.ToGameY

            Return Math.Sqrt((distance_x * distance_x) + (distance_y * distance_y))
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

        Public Function GetXOffset(ByVal X As Single) As Integer
            Return CInt(Math.Round(CDbl((((((X / 192.0!) - GetXSecFromGameX(X)) + 135.0!) * 192.0!) * 10.0!))))
        End Function

        Public Function GetYOffset(ByVal Y As Single) As Integer
            Return CInt(Math.Round(CDbl((((((Y / 192.0!) - GetYSecFromGameY(Y)) + 92.0!) * 192.0!) * 10.0!))))
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

        Public Function GetWeaponMasteryLevel(ByVal Index_) As Byte
            Dim _item As cInvItem = GameServer.Functions.Inventorys(Index_).UserItems(6)
            Dim _refitem As cItem = GetItemByID(_item.Pk2Id)
            '8-11
            If _refitem.CLASS_A = 1 And _refitem.CLASS_B = 6 Then
                Select Case _refitem.CLASS_C
                    '=============CH
                    Case 2 'Sword
                        Return GameServer.Functions.GetMasteryByID(257, Index_).Level
                    Case 3 'Blade
                        Return GameServer.Functions.GetMasteryByID(257, Index_).Level
                    Case 4 'Spear
                        Return GameServer.Functions.GetMasteryByID(258, Index_).Level
                    Case 5 'Glavie
                        Return GameServer.Functions.GetMasteryByID(258, Index_).Level
                    Case 6 'Bow
                        Return GameServer.Functions.GetMasteryByID(259, Index_).Level

                        '=========EU (6 diffs)
                    Case 7
                        'Long Sword (1 hand) == Warrior
                        Return GameServer.Functions.GetMasteryByID(513, Index_).Level
                    Case 8
                        'War Sword (2 Hand)  == Warrior
                        Return GameServer.Functions.GetMasteryByID(513, Index_).Level
                    Case 9
                        'Axe == Warrior
                        Return GameServer.Functions.GetMasteryByID(513, Index_).Level
                    Case 10
                        'Warlock == Warlock
                        Return GameServer.Functions.GetMasteryByID(516, Index_).Level
                    Case 11
                        'Long Staff (Wizard) == Wizard
                        Return GameServer.Functions.GetMasteryByID(514, Index_).Level
                    Case 12
                        'Crossbow == Rouge
                        Return GameServer.Functions.GetMasteryByID(515, Index_).Level
                    Case 13
                        'Dagger == ROuge
                        Return GameServer.Functions.GetMasteryByID(515, Index_).Level
                    Case 14
                        'Harp  == Bard
                        Return GameServer.Functions.GetMasteryByID(517, Index_).Level
                    Case 15
                        'Cleric --> Clweric
                        Return GameServer.Functions.GetMasteryByID(518, Index_).Level
                End Select
            End If
        End Function

    End Module
End Namespace