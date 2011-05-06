Imports System.Timers
Namespace GameServer.Functions
    Public Class cPositionTracker
        WithEvents tmrMovement As Timer

        'ReadOnly
        Dim pPosition As Position 'Player Position
        Dim wPosition As Position 'Target Position

        Dim pSpeedMode As enumSpeedMode
        Dim pMoveState As enumMoveState

        Dim Speed_Walk As Single
        Dim Speed_Run As Single
        Dim Speed_Zerk As Single

        Dim StartWalkTime As DateTime

        Public Sub New(ByVal p As Position, ByVal WalkSpeed As Single, ByVal RunSpeed As Single, ByVal ZerkSpeed As Single)
            pPosition = p 'Character Start Point
            wPosition = p 'Character Walk Point

            Speed_Walk = WalkSpeed
            Speed_Run = RunSpeed
            Speed_Zerk = ZerkSpeed

            pMoveState = enumMoveState.Standing
            pSpeedMode = enumSpeedMode.Running
        End Sub

        Public Function GetCurrentPosition() As Position
            Dim ToGoX As Single
            Dim ToGoY As Single
            Dim num4 As Double
            Dim num5 As Double
            Dim tmpPos As Position
            Select Case pMoveState
                Case enumMoveState.Standing
                    Return pPosition
                Case enumMoveState.Walking
                    Dim TimeLeft As Double = Date.Now.Subtract(StartWalkTime).TotalMilliseconds
                    ToGoX = (pPosition.X - wPosition.X)
                    ToGoY = (pPosition.Y - wPosition.Y)
                    num4 = Math.Sqrt((ToGoX * ToGoX) + (ToGoY * ToGoY))

                    Select Case pSpeedMode
                        Case enumSpeedMode.Walking
                            num5 = ((Speed_Walk / 10.0!) * TimeLeft) / 1000.0!
                        Case enumSpeedMode.Running
                            num5 = ((Speed_Run / 10.0!) * TimeLeft) / 1000.0!
                        Case enumSpeedMode.Zerking
                            num5 = ((Speed_Zerk / 10.0!) * TimeLeft) / 1000.0!
                    End Select
                Case enumMoveState.Spinning
                    Return pPosition
            End Select
            Dim num6 As Single = CSng((((100.0! / num4) * num5) * 0.01))
            ToGoX *= num6
            ToGoY *= num6

            tmpPos = pPosition
            tmpPos.X -= ToGoX
            tmpPos.Y -= ToGoY
            StartWalkTime = Date.Now
            Return tmpPos
        End Function

        Public Sub Move(ByVal ToPos As Position)
            'Navmesh checks later...
            If pMoveState = enumMoveState.Walking Then
                pPosition = GetCurrentPosition()
            End If

            If Double.IsNaN(pPosition.X) Then 'Error Correction
                pPosition.X = ToPos.X
            End If
            If Double.IsNaN(pPosition.Y) Then
                pPosition.Y = ToPos.Y
            End If

            wPosition = ToPos
            Dim WalkDistance As Double = Functions.CalculateDistance(pPosition, ToPos)
            Dim WalkTime As Double
            Select Case pSpeedMode
                Case enumSpeedMode.Walking
                    WalkTime = (WalkDistance / Speed_Walk) * 10000
                Case enumSpeedMode.Running
                    WalkTime = (WalkDistance / Speed_Run) * 10000
                Case enumSpeedMode.Zerking
                    WalkTime = (WalkDistance / Speed_Zerk) * 10000
            End Select
            pMoveState = enumMoveState.Walking

            StartWalkTime = Date.Now 'Move Start Zeit
            If Double.IsInfinity(WalkTime) = False And Double.IsNaN(WalkTime) = False Then
                tmrMovement = New Timer(WalkTime)
                tmrMovement.Start()
            End If

        End Sub
        Private Sub tmrMovement_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs) Handles tmrMovement.Elapsed
            pPosition = wPosition 'Neue Character Position merken
            pMoveState = enumMoveState.Standing
            tmrMovement.Dispose()
        End Sub

        'Properties
        Public Property WalkSpeed() As Single
            Get
                Return Speed_Walk
            End Get
            Set(ByVal value As Single)
                pPosition = GetCurrentPosition() 'Store Old
                Speed_Walk = value
                If pMoveState = enumMoveState.Walking Then
                    Move(wPosition) 'Recalculate
                End If
            End Set
        End Property
        Public Property RunSpeed() As Single
            Get
                Return Speed_Run
            End Get
            Set(ByVal value As Single)
                pPosition = GetCurrentPosition() 'Store Old
                Speed_Run = value
                If pMoveState = enumMoveState.Walking Then
                    Move(wPosition) 'Recalculate
                End If
            End Set
        End Property
        Public Property ZerkSpeed() As Single
            Get
                Return Speed_Zerk
            End Get
            Set(ByVal value As Single)
                pPosition = GetCurrentPosition() 'Store Old
                Speed_Zerk = value
                If pMoveState = enumMoveState.Walking Then
                    Move(wPosition) 'Recalculate
                End If
            End Set
        End Property

        Public ReadOnly Property LastPos() As Position
            Get
                Return pPosition
            End Get
        End Property
        Public ReadOnly Property WalkPos() As Position
            Get
                Return wPosition
            End Get
        End Property

        Public ReadOnly Property MoveState() As enumMoveState
            Get
                Return pMoveState
            End Get
        End Property
        Public Property SpeedMode() As enumSpeedMode
            Get
                Return pSpeedMode
            End Get
            Set(ByVal value As enumSpeedMode)
                pPosition = GetCurrentPosition() 'Store Old
                pSpeedMode = value
                If pMoveState = enumMoveState.Walking Then
                    Move(wPosition) 'Recalculate
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


