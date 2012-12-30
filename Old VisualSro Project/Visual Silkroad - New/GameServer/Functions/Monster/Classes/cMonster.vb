Imports System.Timers

Namespace Functions
    Public Class cMonster
        Inherits cGameObject

        Public SpotID As Long

        Public MobType As Byte
        Public Angle As UInt16 = 0
        Public HPCur As UInteger
        Public HPMax As UInteger

        Public PositionSpawn As Position

        Public Death As Boolean = False
        Public DeathRemoveTime As Date

        Private m_attackTimer As Timer
        Public DamageFromPlayer As New List(Of cDamageDone)
        Public AttackEndTime As New Date
        Public AttackingId As UInteger
        Public UsingSkillId As UInteger

        Public SpawnedGuard80 As Boolean
        Public SpawnedGuard60 As Boolean
        Public SpawnedGuard40 As Boolean
        Public SpawnedGuard20 As Boolean


        Public Function IsAttacking() As Boolean
            If Date.Compare(Date.Now, Me.AttackEndTime) = -1 Then
                Return True
            Else
                Return False
            End If
        End Function

        Function GetsAttacked() As Boolean
            For i = 0 To DamageFromPlayer.Count - 1
                If DamageFromPlayer(i).AttackingAllowed = True Then
                    Return True
                End If
            Next
            Return False
        End Function

#Region "Timer"
        Sub New()
            m_attackTimer = New Timer
            AddHandler m_attackTimer.Elapsed, AddressOf AttackTimerElapsed
            AddHandler PosTracker.TmrMovement.Elapsed, AddressOf AttackTimerElapsed
        End Sub

        Public Sub AttackTimerStart(ByVal interval As Integer)
            m_attackTimer.Interval = interval
            m_attackTimer.Start()
        End Sub

        Public Sub AttackTimerStop()
            m_attackTimer.Stop()
        End Sub

        Public Sub AttackTimerElapsed()
            m_attackTimer.Stop()

            Try
                If Death = True And IsAttacking() Then
                    Exit Sub
                End If


                Dim sort = From elem In DamageFromPlayer Order By elem.Damage Descending Select elem

                For i = 0 To sort.Count - 1
                    Dim index As Integer = sort(i).PlayerIndex
                    If PlayerData(index) IsNot Nothing Then
                        If PlayerData(index).Ingame And PlayerData(sort(i).PlayerIndex).Alive = True Then
                            If _
                                CalculateDistance(PlayerData(sort(i).PlayerIndex).Position, Me.Position) < 100 And
                                sort(i).AttackingAllowed Then
                                If cPositionTracker.enumSpeedMode.Walking Then
                                    SetMobRunning(UniqueID)
                                End If

                                MonsterAttackPlayer(UniqueID, sort(i).PlayerIndex)
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

            m_attackTimer.Interval = 5000
            m_attackTimer.Start()
        End Sub

        Public Sub Disponse()
            m_attackTimer.Dispose()
        End Sub

#End Region

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
