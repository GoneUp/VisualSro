Imports System.Timers
Imports SRFramework

Namespace Timers
    Module Timers
        Public GeneralTimer As New Timer

        Public Sub LoadTimers(ByVal timerCount As Integer)
            Try
                'Initalize


                For i = 0 To timerCount - 1
                   
                Next

                'Handlers
                AddHandler GeneralTimer.Elapsed, AddressOf GeneralTimer_Elapsed

                'Starting
                GeneralTimer.Interval = 2500
                GeneralTimer.Start()
            Catch ex As Exception
                Log.WriteSystemLog("Timers loading failed! EX:" & ex.Message & " Stacktrace: " & ex.StackTrace)
            Finally
                Log.WriteSystemLog("Timers loaded!")
            End Try
        End Sub


        Public Sub GeneralTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            GeneralTimer.Stop()

            'For Database Execute Querys, GlobalManager Ping, GlobalManager Update

            Try
                Log.WriteSystemFileQuerys()

                If DateDiff(DateInterval.Second, GlobalManagerCon.LastPingTime, Date.Now) > 5 Then
                    If GlobalManagerCon.ManagerSocket.Connected Then
                        GlobalManagerCon.SendPing()
                    End If
                End If

                If DateDiff(DateInterval.Second, GlobalManagerCon.LastInfoTime, Date.Now) > 10 And GlobalManagerCon.UpdateInfoAllowed Then
                    If GlobalManagerCon.ManagerSocket.Connected Then
                        GlobalManager.OnSendMyInfo()
                    End If
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: QT")
            End Try

            GeneralTimer.Start()
        End Sub
    End Module
End Namespace
