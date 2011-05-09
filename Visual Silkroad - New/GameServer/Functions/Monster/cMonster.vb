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

        Public Position_Spawn As Position
        Public Pos_Tracker As Functions.cPositionTracker

        Public Death As Boolean = False
        Public DeathRemoveTime As Date

        Public DamageFromPlayer As New List(Of cDamageDone)

        Public IsAttacking As Boolean = False
        Public AttackingId As UInteger
        Public UsingSkillId As UInteger
        Public AttackTimer As System.Timers.Timer


        Public Property Position() As Position
            Get
                Return Me.Pos_Tracker.GetCurPos
            End Get
            Set(ByVal value As Position)
                Me.Pos_Tracker.LastPos = value
            End Set
        End Property

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

            Try
                Dim sort = From elem In DamageFromPlayer Order By elem.Damage Descending Select elem

                For i = 0 To sort.Count - 1
                    Dim Index As Integer = sort(i).PlayerIndex
                    If Functions.PlayerData(Index) IsNot Nothing And Functions.PlayerData(Index).Ingame And Functions.PlayerData(sort(i).PlayerIndex).Alive = True Then
                        If Functions.CalculateDistance(Functions.PlayerData(sort(i).PlayerIndex).Position, Me.Position) < 100 And sort(i).AttackingAllowed Then
                            GameServer.Functions.MonsterAttackPlayer(Me.UniqueID, sort(i).PlayerIndex)
                        End If
                    End If
                Next

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MAE") '
            End Try
        End Sub

        Public Sub Disponse()
            AttackTimer.Dispose()
        End Sub
#End Region

        Function GetsAttacked() As Boolean
            For i = 0 To Me.DamageFromPlayer.Count - 1
                If Me.DamageFromPlayer(i).AttackingAllowed = True Then
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
        Public AttackingAllowed As Boolean = False
    End Class
End Namespace
