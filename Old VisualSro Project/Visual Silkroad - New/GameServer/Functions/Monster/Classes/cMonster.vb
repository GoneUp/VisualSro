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


#Region "Timer"

        Sub New()
            m_attackTimer = New Timer
            AddHandler m_attackTimer.Elapsed, AddressOf AttackTimer_Elapsed
        End Sub

        Public Sub AttackTimer_Start(ByVal Interval As Integer)
            m_attackTimer.Interval = Interval
            m_attackTimer.Start()
        End Sub

        Public Sub AttackTimer_Stop()
            m_attackTimer.Stop()
        End Sub

        Public Sub AttackTimer_Elapsed()
            m_attackTimer.Stop()

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
                                    PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Running
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

            m_attackTimer.Interval = 5000
            m_attackTimer.Start()
        End Sub

        Public Sub Disponse()
            m_attackTimer.Dispose()
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
