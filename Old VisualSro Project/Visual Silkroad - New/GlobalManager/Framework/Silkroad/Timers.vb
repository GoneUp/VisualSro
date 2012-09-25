Imports System.Timers

Namespace Timers
    Module Timers
        Private GenralTimer As New Timer
        Private CleanUpTimer As New Timer
        Private PingTimer As New Timer

        Friend Function LoadTimers() As Boolean
            Try
                'Handlers
                AddHandler GenralTimer.Elapsed, AddressOf GeneralTimerElapsed
                AddHandler CleanUpTimer.Elapsed, AddressOf CleanUpElapsed
                AddHandler PingTimer.Elapsed, AddressOf PingTimerElapsed

                'Starting
                GenralTimer.Interval = 10000
                GenralTimer.Start()

                CleanUpTimer.Interval = 2500
                CleanUpTimer.Start()

                PingTimer.Interval = 30000
                PingTimer.Start()
            Catch ex As Exception
                Log.WriteSystemLog("Timers loading failed! EX:" & ex.Message & " Stacktrace: " & ex.StackTrace)
                Return False
            Finally
                Log.WriteSystemLog("Timers loaded!")
            End Try

            Return True
        End Function


        Private Sub GeneralTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            GenralTimer.Stop()

            Try

                Database.ExecuteQuerys()
                Log.WriteSystemFileQuerys()


            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: QT")
            End Try

            GenralTimer.Start()
        End Sub


        Private Sub CleanUpElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            CleanUpTimer.Stop()

            Try

                Dim tmplist = Agent.UserAuthCache.Keys.ToList
                For i = 0 To tmplist.Count - 1
                    Dim key As UInt32 = tmplist(i)
                    If Agent.UserAuthCache.ContainsKey(key) Then
                        If Date.Compare(Date.Now, Agent.UserAuthCache(key).ExpireTime) = 1 Then '(-1,1)
                            Agent.UserAuthCache.Remove(key)
                        End If
                    End If
                Next


            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: CU")
            End Try

            CleanUpTimer.Start()
        End Sub

        Private Sub PingTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            PingTimer.Stop()

            'Excluded from ClientList 

            Try
                Dim count As Integer = 0

                For i = 0 To Server.MaxClients - 1
                    Dim socket As Net.Sockets.Socket = Server.ClientList.GetSocket(i)
                    If socket IsNot Nothing AndAlso socket.Connected AndAlso SessionInfo(i) IsNot Nothing Then
                        If DateDiff(DateInterval.Second, Server.ClientList.LastPingTime(i), DateTime.Now) > 30 Then
                            Server.Disconnect(i)

                            'ElseIf SessionInfo(i).LoginAuthRequired And DateDiff(DateInterval.Second, SessionInfo(i).LoginAuthTimeout, DateTime.Now) > 0 Then
                            '    'Later for check for missing Handshake Packets, ClientInfo
                            '    Server.Disconnect(i)
                        End If
                    End If
                Next


            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: PING")
            End Try

            PingTimer.Start()
        End Sub
    End Module
End Namespace
