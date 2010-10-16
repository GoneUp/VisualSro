Namespace GameServer.Functions
    Module Formula
        Public Function CalculateDistance(ByVal Object_1 As Position, ByVal Object_2 As Position) As Double
            Dim distance_x As Double = Object_1.X - Object_2.X
            Dim distance_y As Double = Object_1.Y - Object_2.Y

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
            If _refitem.CLASS_A = 1 Then
                Dim selector As String = _refitem.ITEM_TYPE_NAME.Substring(8, 3)
                Select Case selector
                    '========CH
                    Case "SWO" 'Sword
                        Return GameServer.Functions.GetMasteryByID(257, Index_).Level
                    Case "BLA" 'Blade
                        Return GameServer.Functions.GetMasteryByID(257, Index_).Level
                    Case "SPE" 'Spear
                        Return GameServer.Functions.GetMasteryByID(258, Index_).Level
                    Case "TBL" 'Glavie
                        Return GameServer.Functions.GetMasteryByID(258, Index_).Level
                    Case "BOW" 'Bow
                        Return GameServer.Functions.GetMasteryByID(259, Index_).Level
                        '======EU
                    Case Else
                        Return 1

                End Select
            End If
        End Function

    End Module
End Namespace