Imports System.Timers

Namespace Functions
    Public Class cSkill
        Public SkillID As UInteger
        Public OwnerID As UInteger
    End Class

    Public Class cBuff
        Public OwnerID As UInteger
        Public OverID As UInteger
        Public CastingId As UInteger
        Public DurationStart As DateTime
        Public DurationEnd As DateTime
        Public Type As BuffType_

        'If Skill
        Public SkillID As UInteger
        'If Item
        Public ItemID As UInteger

        'Timer
        Public ElaspedTimer As Timer

        Sub New()
            ElaspedTimer = New Timer
            AddHandler ElaspedTimer.Elapsed, AddressOf ElapsedTimerElapsed
        End Sub

        Public Sub ElaspedTimer_Start(ByVal Interval As Integer)
            ElaspedTimer.Interval = Interval
            ElaspedTimer.Start()
        End Sub

        Public Sub ElaspedTimer_Stop()
            ElaspedTimer.Stop()
        End Sub

        Public Sub ElapsedTimerElapsed()
            ElaspedTimer.Stop()

            Try
                For i As Integer = 0 To Server.MaxClients
                    If Functions.PlayerData(i) IsNot Nothing Then
                        If Functions.PlayerData(i).UniqueID = OwnerID Then
                            Functions.PlayerBuff_End(OverID, i)
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


    Public Enum BuffType_
        ItemBuff = 0
        SkillBuff = 1
    End Enum
End Namespace

