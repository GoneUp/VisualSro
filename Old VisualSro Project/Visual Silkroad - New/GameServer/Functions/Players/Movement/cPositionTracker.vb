Imports System.Timers

Namespace Functions
    Public Class cPositionTracker
        WithEvents m_tmrMovement As Timer

        'ReadOnly
        Dim m_pPosition As Position 'Player Position
        Dim m_wPosition As Position 'Target Position


        Dim m_pSpeedMode As enumSpeedMode
        Dim m_pMoveState As enumMoveState

        Dim m_speedWalk As Single
        Dim m_speedRun As Single
        Dim m_speedZerk As Single

        Dim m_startWalkTime As DateTime

        Public Sub New(ByVal p As Position, ByVal walkSpeed As Single, ByVal runSpeed As Single, ByVal zerkSpeed As Single)

            m_pPosition = p 'Character Start Point
            m_wPosition = p 'Character Walk Point

            m_speedWalk = walkSpeed
            m_speedRun = runSpeed
            m_speedZerk = zerkSpeed

            m_pMoveState = enumMoveState.Standing
            m_pSpeedMode = enumSpeedMode.Running
        End Sub

        Public Function GetCurPos() As Position
            Dim toGoX As Single
            Dim toGoY As Single
            Dim distance As Double
            Dim speed As Double

            Try
                Select Case m_pMoveState
                    Case enumMoveState.Standing
                        Return m_pPosition
                    Case enumMoveState.Walking
                        Dim timeLeft As Double = Date.Now.Subtract(m_startWalkTime).TotalMilliseconds
                        toGoX = (m_pPosition.ToGameX - m_wPosition.ToGameX)
                        toGoY = (m_pPosition.ToGameY - m_wPosition.ToGameY)
                        distance = Math.Sqrt((toGoX * toGoX) + (toGoY * toGoY))
                        Select Case m_pSpeedMode
                            Case enumSpeedMode.Walking
                                speed = ((m_speedWalk / 10.0!) * timeLeft) / 1000.0!
                            Case enumSpeedMode.Running
                                speed = ((m_speedRun / 10.0!) * timeLeft) / 1000.0!
                            Case enumSpeedMode.Zerking
                                speed = ((m_speedZerk / 10.0!) * timeLeft) / 1000.0!
                        End Select

                        If distance = 0.0 Then
                            'Nothing to do more
                            Return m_pPosition
                        End If

                        Dim num6 As Single = CSng((((100.0! / distance) * speed) * 0.01))
                        toGoX *= num6
                        toGoY *= num6


                        Dim tmpX As Single = (m_pPosition.ToGameX)
                        Dim tmpY As Single = (m_pPosition.ToGameY)
                        tmpX -= toGoX
                        tmpY -= toGoY
                        m_pPosition.XSector = GetXSecFromGameX(tmpX)
                        m_pPosition.YSector = GetYSecFromGameY(tmpY)
                        m_pPosition.X = GetXOffset(tmpX)
                        m_pPosition.Y = GetYOffset(tmpY)
                        m_startWalkTime = Date.Now
                        Return m_pPosition
                    Case enumMoveState.Spinning
                        Return m_pPosition
                End Select


            Catch ex As Exception
                Log.WriteSystemLog("Pos Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: POSCALC")
            End Try
        End Function

        Public Sub Move(ByVal toPos As Position)
            'Navmesh checks later...
            If m_pMoveState = enumMoveState.Walking Then
                m_pPosition = GetCurPos()
            End If

            If Double.IsNaN(m_pPosition.X) Then 'Error Correction
                m_pPosition.X = toPos.X
            End If
            If Double.IsNaN(m_pPosition.Y) Then
                m_pPosition.Y = toPos.Y
            End If

            m_wPosition = toPos
            Dim walkDistance As Double = CalculateDistance(m_pPosition, toPos)
            Dim walkTime As Integer
            Select Case m_pSpeedMode
                Case enumSpeedMode.Walking
                    walkTime = (walkDistance / m_speedWalk) * 10000
                Case enumSpeedMode.Running
                    walkTime = (walkDistance / m_speedRun) * 10000
                Case enumSpeedMode.Zerking
                    walkTime = (walkDistance / m_speedZerk) * 10000
            End Select
            m_pMoveState = enumMoveState.Walking
            m_startWalkTime = Date.Now
            'Move Start Zeit

            If walkTime = 0 Then
                walkTime = 1
            End If
            If Double.IsInfinity(walkTime) = False And Double.IsNaN(walkTime) = False Then
                m_tmrMovement = New Timer
                m_tmrMovement.Interval = walkTime
                m_tmrMovement.Start()
            End If
        End Sub

        Private Sub MovementTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs) Handles m_tmrMovement.Elapsed
            m_pPosition = m_wPosition
            'Neue Character Position merken
            m_pMoveState = enumMoveState.Standing
            m_tmrMovement.Dispose()
        End Sub

        Public Sub StopMove()
            If m_tmrMovement IsNot Nothing Then
                m_tmrMovement.Stop()
                m_tmrMovement.Dispose()
            End If

            m_pMoveState = enumMoveState.Standing
            m_pPosition = GetCurPos()
        End Sub

        'Properties
        Public Property WalkSpeed() As Single
            Get
                Return m_speedWalk
            End Get
            Set(ByVal value As Single)
                m_pPosition = GetCurPos()
                'Store Old
                m_speedWalk = value
                If m_pMoveState = enumMoveState.Walking Then
                    Move(m_wPosition)
                    'Recalculate
                End If
            End Set
        End Property

        Public Property RunSpeed() As Single
            Get
                Return m_speedRun
            End Get
            Set(ByVal value As Single)
                m_pPosition = GetCurPos()
                'Store Old
                m_speedRun = value
                If m_pMoveState = enumMoveState.Walking Then
                    Move(m_wPosition)
                    'Recalculate
                End If
            End Set
        End Property

        Public Property BerserkSpeed() As Single
            Get
                Return m_speedZerk
            End Get
            Set(ByVal value As Single)
                m_pPosition = GetCurPos()
                'Store Old
                m_speedZerk = value
                If m_pMoveState = enumMoveState.Walking Then
                    Move(m_wPosition)
                    'Recalculate
                End If
            End Set
        End Property

        Public Property LastPos() As Position
            Get
                Return m_pPosition
            End Get
            Set(ByVal value As Position)
                If m_tmrMovement IsNot Nothing Then
                    'tmrMovement.Dispose()
                End If
                m_pPosition = value
            End Set
        End Property

        Public ReadOnly Property WalkPos() As Position
            Get
                Return m_wPosition
            End Get
        End Property

        Public ReadOnly Property MoveState() As enumMoveState
            Get
                Return m_pMoveState
            End Get
        End Property

        Public Property SpeedMode() As enumSpeedMode
            Get
                Return m_pSpeedMode
            End Get
            Set(ByVal value As enumSpeedMode)
                m_pPosition = GetCurPos()
                'Store Old
                m_pSpeedMode = value
                If m_pMoveState = enumMoveState.Walking Then
                    Move(m_wPosition)
                    'Recalculate
                End If
            End Set
        End Property

        'Enum
        Public Enum enumMoveState As Byte
            Standing = 0
            Walking = 1
            Spinning = 0 'Move Angle
        End Enum

        Public Enum enumSpeedMode As Byte
            Walking = 0
            Running = 1
            Zerking = 2
        End Enum
    End Class
End Namespace


