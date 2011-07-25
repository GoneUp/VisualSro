Imports System.Timers
Namespace Timers
    Module Timers


        Friend QueryTimer As New Timer


        Friend Sub LoadTimers()
            'Handlers

            AddHandler QueryTimer.Elapsed, AddressOf QueryTimer_Elapsed


            'Starting
            QueryTimer.Interval = 2500
            QueryTimer.Start()

        End Sub


        Friend Sub QueryTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            QueryTimer.Stop()

            Try

                Framework.DataBase.ExecuteQuerys()


            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: QT")
            End Try

            QueryTimer.Start()
        End Sub

    End Module
End Namespace
