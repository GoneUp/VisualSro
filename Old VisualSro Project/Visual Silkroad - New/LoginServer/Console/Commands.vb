Module Commands

    Private perfWnd As New PerfWnd

    Public Sub CheckCommand(ByVal msg As String)

        Select Case msg

            Case "/info"
                Log.WriteSystemLog("This Emulator is from GoneUp. ")
                Log.WriteSystemLog("Specical Thanks to:")
                Log.WriteSystemLog("Drew Benton")
                Log.WriteSystemLog("manneke for the great help")
                Log.WriteSystemLog("Windrius for the Framework.")
                Log.WriteSystemLog("SREmu Team")
                Log.WriteSystemLog("Dickernoob for CSREmu")
                Log.WriteSystemLog("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


            Case "/help"
                Log.WriteSystemLog("Commands: ")
                Log.WriteSystemLog("/info for the credits")
                Log.WriteSystemLog("/debug to enable packetLoginServer.Log")
                Log.WriteSystemLog("/clear")
                Log.WriteSystemLog("/count")
                Log.WriteSystemLog("/end")

            Case "/debug"
                If Settings.ServerDebugMode Then
                    Settings.ServerDebugMode = False
                    Log.WriteSystemLog("Turned off DebugMode")
                ElseIf Settings.ServerDebugMode = False Then
                    Settings.ServerDebugMode = True
                    Log.WriteSystemLog("Turned on DebugMode")
                End If

            Case "/count"
                Log.WriteSystemLog("Sockets online: " & Server.OnlineClients)

            Case "/clear"
                Console.Clear()

            Case "/end"
                GlobalManagerCon.UserSidedShutdown = False
                If GlobalManagerCon.ManagerSocket IsNot Nothing AndAlso GlobalManagerCon.ManagerSocket.Connected Then
                    GlobalManager.OnSendServerShutdown()
                Else
                    'No Connection, init the reconnection
                    GlobalManagerCon.ShutdownComplete()
                End If

            Case "/reinit"
                Log.WriteSystemLog("Ending Server....")
                For i = 0 To Server.MaxClients - 1
                    If SessionInfo(i) IsNot Nothing Then
                        Server.Disconnect(i)
                    End If
                Next

                If Server.Online Then
                    Server.Stop()
                End If

       
                GlobalManagerCon.UserSidedShutdown = True
                GlobalManagerCon.ShutdownReason = SRFramework.GlobalManagerClient.GMCShutdownReason.Reinit

                If GlobalManagerCon.ManagerSocket IsNot Nothing AndAlso GlobalManagerCon.ManagerSocket.Connected Then
                    GlobalManager.OnSendServerShutdown()
                Else
                    'No Connection, init the reconnection
                    GlobalManagerCon.ShutdownComplete()
                End If

            Case "/wnd"
                perfWnd.Text = "LS: PefWnd"
                perfWnd.ShowDialog()

            Case "/gmre"
                'GlobalManagerReConnect
                Log.WriteSystemLog("GMC: Started disconnect...")
                GlobalManagerCon.UserSidedShutdown = True
                GlobalManagerCon.ShutdownReason = SRFramework.GlobalManagerClient.GMCShutdownReason.Reconnect

                If GlobalManagerCon.ManagerSocket IsNot Nothing AndAlso GlobalManagerCon.ManagerSocket.Connected Then
                    GlobalManager.OnSendServerShutdown()
                Else
                    'No Connection, init the reconnection
                    GlobalManagerCon.ShutdownComplete()
                End If


        End Select



    End Sub
End Module
