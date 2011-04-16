Imports System

Namespace GameServer

    Public Class cMonster
        Public Pk2ID As UInt32
        Public UniqueID As UInt32
        Public SpotID As Long

        Public Mob_Type As Byte
        Public Angle As UInt16 = 0
        Public HP_Cur As UInteger
        Public HP_Max As UInteger

        Public Position As Position
        Public Position_FromPos As Position
        Public Position_ToPos As Position
        Public Position_Spawn As Position

        Public WalkStart As New Date
        Public WalkEnd As New Date
        Public Walking As Boolean = False

        Public Death As Boolean = False
        Public DeathRemoveTime As Date

        Public DamageFromPlayer As New List(Of cDamageDone)

        Public IsAttacking As Boolean = False
        Public AttackingId As UInteger
        Public UsingSkillId As UInteger
        Public AttackTimer As System.Timers.Timer

#Region "Timer"
        Sub New()
            AttackTimer = New System.Timers.Timer
            AddHandler AttackTimer.Elapsed, AddressOf AttackTimer_Elapsed
        End Sub

        Public Sub AttackTimer_Start(ByVal Interval As Integer)
            AttackTimer.Interval = Interval
            AttackTimer.Start()
        End Sub

        Public Sub AttackTimer_Stop()
            AttackTimer.Stop()
        End Sub

        Public Sub AttackTimer_Elapsed()
            AttackTimer.Stop()
            Dim sort = From elem In DamageFromPlayer Order By elem.Damage Descending Select elem

            For i = 0 To sort.Count - 1
                If Functions.PlayerData(sort(i).PlayerIndex) IsNot Nothing Then
                    If Functions.CalculateDistance(Functions.PlayerData(sort(i).PlayerIndex).Position, Me.Position) < 100 And Functions.PlayerData(sort(i).PlayerIndex).Alive = True Then
                        GameServer.Functions.MonsterAttackPlayer(Me.UniqueID, sort(i).PlayerIndex)
                    End If
                End If
            Next

        End Sub

        Public Sub Disponse()
            AttackTimer.Dispose()
        End Sub
#End Region

        Function GetsAttacked() As Boolean
            For i = 0 To Me.DamageFromPlayer.Count - 1
                If Me.DamageFromPlayer(i).Attacking = True Then
                    Return True
                End If
            Next
            Return False
        End Function



    End Class

    ''' <summary>
    ''' Is for the Damage done from a Player to a Monster.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class cDamageDone
        Public PlayerIndex As Integer
        Public Damage As ULong
        Public Attacking As Boolean = False
    End Class
End Namespace
