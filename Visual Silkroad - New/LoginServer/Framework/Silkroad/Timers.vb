Imports System.Timers
Imports LoginServer.Framework

Namespace Timers
    Module Timers


        Public QueryTimer As New Timer


        Public Sub LoadTimers()
            'Handlers

            AddHandler QueryTimer.Elapsed, AddressOf QueryTimer_Elapsed


            'Starting
            QueryTimer.Interval = 2500
            QueryTimer.Start()

        End Sub


        Public Sub QueryTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            QueryTimer.Stop()

            Try

                DataBase.ExecuteQuerys()


            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: QT")
            End Try

            QueryTimer.Start()
        End Sub

    End Module
End Namespace
