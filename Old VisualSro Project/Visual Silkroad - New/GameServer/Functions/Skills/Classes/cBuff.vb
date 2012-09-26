Imports System.Timers

Namespace Functions
    Public Class cBuff
        Public OwnerID As UInteger
        Public OverID As UInteger
        Public CastingId As UInteger
        Public DurationStart As DateTime
        Public DurationEnd As DateTime
        Public Type As BuffType

        'If Skill
        Public SkillID As UInteger
        'If Item
        Public ItemID As UInteger

        'Timer
        Private ElaspedTimer As Timer

        Sub New()
            ElaspedTimer = New Timer
            AddHandler ElaspedTimer.Elapsed, AddressOf ElapsedTimerElapsed
        End Sub

        Public Sub ElaspedTimerStart(ByVal interval As Integer)
            ElaspedTimer.Interval = interval
            ElaspedTimer.Start()
        End Sub

        Public Sub ElaspedTimerStop()
            ElaspedTimer.Stop()
        End Sub

        Private Sub ElapsedTimerElapsed()
            ElaspedTimer.Stop()

            Try
                For i As Integer = 0 To Server.MaxClients
                    If PlayerData(i) IsNot Nothing Then
                        If PlayerData(i).UniqueID = OwnerID Then
                            PlayerBuff_End(OverID, i)
                        End If
                    End If
                Next

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MAE")
                '
            End Try
        End Sub

        Public Sub Disponse()
            ElaspedTimer.Dispose()
        End Sub
    End Class


    Public Enum BuffType
        ItemBuff = 0
        SkillBuff = 1
    End Enum
End Namespace
