Imports System.Timers
Imports SRFramework

Namespace Timers
    Module Timers
        Public GeneralTimer As New Timer
        Public PingTimer As New Timer
        Public LoginInfoTimer(10) As Timer


        Public Sub LoadTimers(ByVal timerCount As Integer)
            Try
                'Initalize
                ReDim LoginInfoTimer(timerCount)

                For i = 0 To timerCount - 1
                    LoginInfoTimer(i) = New Timer
                    AddHandler LoginInfoTimer(i).Elapsed, AddressOf LoginInfoTimerElapsed
                Next

                'Handlers
                AddHandler GeneralTimer.Elapsed, AddressOf GeneralTimer_Elapsed
                AddHandler PingTimer.Elapsed, AddressOf PingTimer_Elapsed

                'Starting
                GeneralTimer.Interval = 2500
                GeneralTimer.Start()

                PingTimer.Interval = 30000
                PingTimer.Start()

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
                Database.ExecuteQuerys()

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

        Public Sub LoginInfoTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim index_ As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = LBound(LoginInfoTimer, 1) To UBound(LoginInfoTimer, 1)
                    If ReferenceEquals(LoginInfoTimer(i), objB) Then
                        index_ = i
                        Exit For
                    End If
                Next

                LoginInfoTimer(index_).Stop()
                If index_ <> -1 AndAlso SessionInfo(index_) IsNot Nothing AndAlso SessionInfo(index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.LOGIN Then
                    If LoginDb.LoginInfoMessages.Count > 0 Then
                        If SessionInfo(index_).LoginTextIndex > LoginDb.LoginInfoMessages.Count Then
                            SessionInfo(index_).LoginTextIndex = 0
                        End If

                        Dim tmpMsg As LoginDb.LoginInfoMessage_ = LoginDb.LoginInfoMessages(SessionInfo(index_).LoginTextIndex)
                        Functions.LoginWriteSpecialText(tmpMsg.Text, index_)
                        SessionInfo(index_).LoginTextIndex += 1

                        LoginInfoTimer(index_).Interval = tmpMsg.Delay * 1000
                        LoginInfoTimer(index_).Start()
                    End If
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & index_)
            End Try
        End Sub

        Public Sub PingTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            PingTimer.Stop()

            'Excluded from ClientList 

            Try
                Dim Count As Integer = 0

                For i = 0 To Server.MaxClients - 1
                    Dim socket As Net.Sockets.Socket = Server.ClientList.GetSocket(i)
                    If socket IsNot Nothing AndAlso socket.Connected AndAlso SessionInfo(i) IsNot Nothing Then
                        If DateDiff(DateInterval.Second, Server.ClientList.LastPingTime(i), DateTime.Now) > 30 Then
                            Server.Disconnect(i)

                            'ElseIf SessionInfo(i).LoginAuthRequired And DateDiff(DateInterval.Second, SessionInfo(i).LoginAuthTimeout, DateTime.Now) > 0 Then
                            '    'Later for check for missing Handshake Packets, ClientInfo
                            '    Server.Disconnect(i)
                        Else
                            Count += 1
                        End If
                    End If
                Next

                'Server.OnlineClients = Count

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: PING")
                '
            End Try

            PingTimer.Start()
        End Sub
    End Module
End Namespace
