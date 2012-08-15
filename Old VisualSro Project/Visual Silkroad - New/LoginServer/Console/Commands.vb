Module Commands

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
                If Settings.Server_DebugMode Then
                    Settings.Server_DebugMode = False
                    Log.WriteSystemLog("Turned off DebugMode")
                ElseIf Settings.Server_DebugMode = False Then
                    Settings.Server_DebugMode = True
                    Log.WriteSystemLog("Turned on DebugMode")
                End If

            Case "/count"
                Log.WriteSystemLog("Sockets online: " & Server.OnlineClients)

            Case "/clear"
                Console.Clear()

            Case "/end"
                'For i = 0 To ClientList.SessionInfo.Count - 1
                '    If ClientList.SessionInfo(i) IsNot Nothing Then
                '        Server.Dissconnect(i)
                '    End If
                'Next
                'Server.Stop()
                'Database.ExecuteQuerys()
                'End
                GlobalManager.OnSendServerShutdown()

            Case "/reinit"
                Log.WriteSystemLog("Ending Server....")
                For i = 0 To Server.MaxClients - 1
                    If Server.ClientList.GetSocket(i) IsNot Nothing Then
                        Server.Disconnect(i)
                    End If
                Next

                If Server.Online Then
                    Server.Stop()
                End If

                Database.ExecuteQuerys()
                GlobalManagerCon.Disconnect()

                Log.WriteSystemLog("Cleanup Server...")

                GlobalDef.Initalize(Server.MaxClients)
                LoginDb.InitalLoad = True
                LoginDb.UpdateData()
                Timers.LoadTimers(Server.MaxClients)


                Log.WriteSystemLog("Reconnect GlobalManager...")
                GlobalManagerCon.Connect(Settings.GlobalManger_Ip, Settings.GlobalManger_Port)

            Case "/gmre"
                'GlobalManagerReConnect
                GlobalManagerCon.Disconnect()
                Log.WriteSystemLog("Reconnect GlobalManager...")
                GlobalManagerCon.Connect(Settings.GlobalManger_Ip, Settings.GlobalManger_Port)

        End Select



    End Sub
End Module
