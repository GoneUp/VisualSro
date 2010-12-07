Public Class cMonster
    Public Pk2ID As UInt32
    Public UniqueID As UInt32
    Public Mob_Type As Byte
    Public Angle As UInt16 = 0
    Public HP_Cur As Int64

    Public Position As Position
    Public Position_Spawn As Position

    Public Death As Boolean = False

    Public DamageFromPlayer As New List(Of cDamageDone)
End Class

''' <summary>
''' Is for the Damage done from a Player to a Monster.
''' </summary>
''' <remarks></remarks>
Public Class cDamageDone
    Public PlayerIndex As Integer
    Public Damage As ULong
End Class
