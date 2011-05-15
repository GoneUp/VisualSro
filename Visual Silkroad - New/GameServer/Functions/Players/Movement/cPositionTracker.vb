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

        Public Function GetCurPos() As Position
            Dim ToGoX As Single
            Dim ToGoY As Single
            Dim num4 As Double
            Dim num5 As Double
            Dim tmpPos As New Position

            Try
                Select Case pMoveState
                    Case enumMoveState.Standing
                        Return pPosition
                    Case enumMoveState.Walking
                        Dim TimeLeft As Double = Date.Now.Subtract(StartWalkTime).TotalMilliseconds
                        ToGoX = (pPosition.ToGameX - wPosition.ToGameX)
                        ToGoY = (pPosition.ToGameY - wPosition.ToGameY)
                        num4 = Math.Sqrt((ToGoX * ToGoX) + (ToGoY * ToGoY))
                        Select Case pSpeedMode
                            Case enumSpeedMode.Walking
                                num5 = ((Speed_Walk / 10.0!) * TimeLeft) / 1000.0!
                            Case enumSpeedMode.Running
                                num5 = ((Speed_Run / 10.0!) * TimeLeft) / 1000.0!
                            Case enumSpeedMode.Zerking
                                num5 = ((Speed_Zerk / 10.0!) * TimeLeft) / 1000.0!
                        End Select

                        If num4 = 0.0 Then
                            'Nothing to do more
                            Return pPosition
                        End If

                        Dim num6 As Single = CSng((((100.0! / num4) * num5) * 0.01))
                        ToGoX *= num6
                        ToGoY *= num6


                        Dim tmpX As Single = (pPosition.ToGameX)
                        Dim tmpY As Single = (pPosition.ToGameY)
                        tmpX -= ToGoX
                        tmpY -= ToGoY
                        pPosition.X = GetXOffset(tmpX)
                        pPosition.Y = GetYOffset(tmpY)
                        StartWalkTime = Date.Now
                        Return pPosition
                    Case enumMoveState.Spinning
                        Return pPosition
                End Select



            Catch ex As Exception
                Log.WriteSystemLog("Pos Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: POSCALC") '
            End Try
        End Function

        Public Sub Move(ByVal ToPos As Position)
            'Navmesh checks later...
            If pMoveState = enumMoveState.Walking Then
                pPosition = GetCurPos()
            End If

            If Double.IsNaN(pPosition.X) Then 'Error Correction
                pPosition.X = ToPos.X
            End If
            If Double.IsNaN(pPosition.Y) Then
                pPosition.Y = ToPos.Y
            End If

            wPosition = ToPos
            Dim WalkDistance As Double = Functions.CalculateDistance(pPosition, ToPos)
            Dim WalkTime As Integer
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

            If WalkTime = 0 Then
                WalkTime = 1
            End If
            If Double.IsInfinity(WalkTime) = False And Double.IsNaN(WalkTime) = False Then
                tmrMovement = New Timer
                tmrMovement.Interval = WalkTime
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
                pPosition = GetCurPos() 'Store Old
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
                pPosition = GetCurPos() 'Store Old
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
                pPosition = GetCurPos() 'Store Old
                Speed_Zerk = value
                If pMoveState = enumMoveState.Walking Then
                    Move(wPosition) 'Recalculate
                End If
            End Set
        End Property

        Public Property LastPos() As Position
            Get
                Return pPosition
            End Get
            Set(ByVal value As Position)
                If tmrMovement IsNot Nothing Then
                    'tmrMovement.Dispose()
                End If
                pPosition = value
            End Set
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
                pPosition = GetCurPos() 'Store Old
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


