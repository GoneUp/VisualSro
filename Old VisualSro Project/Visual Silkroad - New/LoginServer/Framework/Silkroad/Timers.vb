Imports System.Timers
Imports LoginServer.Framework

Namespace Timers
    Module Timers


        Public QueryTimer As New Timer
        Public LoginInfoTimer(10) As Timer


        Public Sub LoadTimers(ByVal timerCount As Integer)

            'Initalize
            ReDim LoginInfoTimer(timerCount)

            For i = 0 To timerCount - 1
                LoginInfoTimer(i) = New Timer
                AddHandler LoginInfoTimer(i).Elapsed, AddressOf LoginInfoTimerElapsed
            Next

            'Handlers
            AddHandler QueryTimer.Elapsed, AddressOf QueryTimerElapsed


            'Starting
            QueryTimer.Interval = 2500
            QueryTimer.Start()

        End Sub


        Public Sub QueryTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            QueryTimer.Stop()

            Try

                Database.ExecuteQuerys()


            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: QT")
            End Try

            QueryTimer.Start()
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
                If index_ <> -1 And ClientList.SessionInfo(index_).SRConnectionSetup = _SessionInfo.SRConnectionStatus.LOGIN Then
                    If LoginDb.LoginInfoMessages.Count > 0 Then
                        If ClientList.SessionInfo(index_).LoginTextIndex > LoginDb.LoginInfoMessages.Count Then
                            ClientList.SessionInfo(index_).LoginTextIndex = 0
                        End If

                        Dim tmpMsg As LoginDb.LoginInfoMessage_ = LoginDb.LoginInfoMessages(ClientList.SessionInfo(index_).LoginTextIndex)
                        Functions.LoginWriteSpecialText(tmpMsg.Text, index_)
                        ClientList.SessionInfo(index_).LoginTextIndex += 1

                        LoginInfoTimer(index_).Interval = tmpMsg.Delay * 1000
                        LoginInfoTimer(index_).Start()
                    End If
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & index_)
            End Try
        End Sub
    End Module
End Namespace
