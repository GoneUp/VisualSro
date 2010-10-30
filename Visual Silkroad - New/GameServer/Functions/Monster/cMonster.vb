﻿Public Class cMonster
    Public Pk2ID As UInt32
    Public UniqueID As UInt32
    Public Mob_Type As Byte
    Public Angle As UInt16 = 0
    Public HP_Cur As UInt32
    Public Position As Position
    Public Death As Boolean = False

    Public DamageFromPlayer As List(Of cDamageDone)
End Class

''' <summary>
''' Is for the Damage done from a Player to a Monster.
''' </summary>
''' <remarks></remarks>
Public Structure cDamageDone
    Public PlayerIndex As Integer
    Public Damage As ULong
End Structure
