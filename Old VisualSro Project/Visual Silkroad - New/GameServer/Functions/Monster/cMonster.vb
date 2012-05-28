Imports System.Timers

Namespace GameServer.Functions
    Public Class cMonster
        Inherits cGameObject

        Public SpotID As Long

        Public Mob_Type As Byte
        Public Angle As UInt16 = 0
        Public HP_Cur As UInteger
        Public HP_Max As UInteger

        Public Position_Spawn As Position
        Public Pos_Tracker As cPositionTracker

        Public Death As Boolean = False
        Public DeathRemoveTime As Date

        Public DamageFromPlayer As New List(Of cDamageDone)
        Public AttackEndTime As New Date
        Public AttackingId As UInteger
        Public UsingSkillId As UInteger
        Public AttackTimer As Timer

        Public SpawnedGuard_80 As Boolean
        Public SpawnedGuard_60 As Boolean
        Public SpawnedGuard_40 As Boolean
        Public SpawnedGuard_20 As Boolean

        Public Property Position() As Position
            Get
                Return Me.Pos_Tracker.GetCurPos
            End Get
            Set(ByVal value As Position)
                Me.Pos_Tracker.LastPos = value
            End Set
        End Property

        Public Function IsAttacking() As Boolean
            If Date.Compare(Date.Now, Me.AttackEndTime) = -1 Then
                Return True
            Else
                Return False
            End If
        End Function


#Region "Timer"

        Sub New()
            AttackTimer = New Timer
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
                If Me.Death = True And IsAttacking() Then
                    Exit Sub
                End If


                Dim sort = From elem In DamageFromPlayer Order By elem.Damage Descending Select elem

                For i = 0 To sort.Count - 1
                    Dim Index As Integer = sort(i).PlayerIndex
                    If PlayerData(Index) IsNot Nothing Then
                        If PlayerData(Index).Ingame And PlayerData(sort(i).PlayerIndex).Alive = True Then
                            If _
                                CalculateDistance(PlayerData(sort(i).PlayerIndex).Position, Me.Position) < 100 And
                                sort(i).AttackingAllowed Then
                                If cPositionTracker.enumSpeedMode.Walking Then
                                    Pos_Tracker.SpeedMode = cPositionTracker.enumSpeedMode.Running
                                    UpdateState(1, 3, Me)
                                End If

                                MonsterAttackPlayer(Me.UniqueID, sort(i).PlayerIndex)
                                Exit For
                            End If
                        End If
                    Else
                        sort(i).AttackingAllowed = False
                    End If

                Next

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MAE")
                '
            End Try

            AttackTimer.Interval = 5000
            AttackTimer.Start()
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
