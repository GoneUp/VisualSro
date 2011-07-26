Imports System.Timers
Namespace Timers
    Module Timers


        Friend QueryTimer As New Timer
        Friend CleanUpTimer As New Timer

        Friend Sub LoadTimers()
            'Handlers

            AddHandler QueryTimer.Elapsed, AddressOf QueryTimer_Elapsed
            AddHandler CleanUpTimer.Elapsed, AddressOf CleanUp_Elapsed

            'Starting
            QueryTimer.Interval = 2500
            QueryTimer.Start()

            CleanUpTimer.Interval = 2500
            CleanUpTimer.Start()
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


        Friend Sub CleanUp_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            CleanUpTimer.Stop()

            Try

                Dim tmplist = Agent.UserAuthCache.Keys.ToList
                For i = 0 To tmplist.Count - 1
                    If Agent.UserAuthCache.ContainsKey(tmplist(i)) Then
                        If Date.Compare(Agent.UserAuthCache(i).ExpireTime, Date.Now) = 1 Then
                            Agent.UserAuthCache.Remove(tmplist(i))
                        End If
                    End If
                Next


            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: CU")
            End Try

            CleanUpTimer.Start()
        End Sub

    End Module
End Namespace
