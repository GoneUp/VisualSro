Module Commands

    Private perfWnd As New PerfWnd

    Public Sub CheckCommand(ByVal FullMessage As String)

        Dim msg() As String = FullMessage.Split(" ")

        Select Case msg(0)

            Case "/info"
                Log.WriteSystemLog("This Emulator is from GoneUp.")
                Log.WriteSystemLog("Specical Thanks to:")
                Log.WriteSystemLog("Drew Benton")
                Log.WriteSystemLog("manneke for the great help")
                Log.WriteSystemLog("Windrius for the original Framework")
                Log.WriteSystemLog("SREmu Team")
                Log.WriteSystemLog("Dickernoob for CSREmu")
                Log.WriteSystemLog("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


            Case "/help"
                Log.WriteSystemLog("Log: ")
                Log.WriteSystemLog("/info for the credits")
                Log.WriteSystemLog("/packets to enable packetlog")
                Log.WriteSystemLog("/notice [Message] - To write a global Message ")
                Log.WriteSystemLog("/clear")

            Case "/packets"
                Settings.ServerDebugMode = True
                GameServer.Log.WriteSystemLog("Log Packets started!")


            Case "/clear"
                Console.Clear()


            Case "/notice"
                Functions.SendNotice(msg(1))


            Case "/weather"
                Functions.OnSetWeather(CByte(msg(1)), CByte(msg(2)))

            Case "/normalweather"
                Functions.OnSetWeather(1, 75)

            Case "/rain"
                Functions.OnSetWeather(2, 75)

            Case "/snow"
                Functions.OnSetWeather(3, 75)

            Case "/count"
                Log.WriteSystemLog(String.Format("Count Player:{0}!", Server.OnlineClients))
                Log.WriteSystemLog(String.Format("Count Mob:{0}!", Functions.MobList.Count))

            Case "/cleanup"
                Dim mem As Long = Process.GetCurrentProcess.PrivateMemorySize64
                GC.Collect()
                Log.WriteSystemLog(
                    "Cleanup Memory in mb: " & (mem - Process.GetCurrentProcess.PrivateMemorySize64) / 1024 / 1024)

            Case "/end"
                Log.WriteSystemLog("Ending Server....")
                GlobalManager.OnSendServerShutdown()


            Case "/debug"
                If Settings.ServerDebugMode Then
                    Settings.ServerDebugMode = False
                    Log.WriteSystemLog("Turned off DebugMode")
                    Functions.SendNotice("DEBUG Mode off!")

                ElseIf Settings.ServerDebugMode = False Then
                    Settings.ServerDebugMode = True
                    Log.WriteSystemLog("Turned on DebugMode")
                    Functions.SendNotice("DEBUG Mode on!")
                End If


            Case "/killmobs"
                Log.WriteSystemLog("In Progress, Count: " & Functions.MobList.Count)
                For Each key In Functions.MobList.Keys.ToList
                    If Functions.MobList.ContainsKey(key) Then
                        Functions.RemoveMob(key)
                    End If
                Next
                Log.WriteSystemLog("Finished!")

            Case "/respawnoff"
                Settings.ServerSpawnRate = 0
                Log.WriteSystemLog("Turned off Respawn")

            Case "/save"
                Dim path = AppDomain.CurrentDomain.BaseDirectory & "npcpos_saved.txt"
                Dim cloack As New Stopwatch
                cloack.Start()
                Log.WriteSystemLog("Save start to: " & path)
                Functions.SaveAutoSpawn(path)
                cloack.Stop()
                Log.WriteSystemLog("Save Fin! S:" & cloack.Elapsed().TotalSeconds)

            Case "/online"
                If Server.Online Then
                    Log.WriteSystemLog("Sure ;)")
                Else
                    Log.WriteSystemLog("nope...")
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

                Database.ExecuteQuerys()

                If GlobalManagerCon.ManagerSocket IsNot Nothing AndAlso GlobalManagerCon.ManagerSocket.Connected Then
                    GlobalManagerCon.UserSidedShutdown = True
                    GlobalManagerCon.ShutdownReason = SRFramework.GlobalManagerClient.GMCShutdownReason.Reinit
                    GlobalManager.OnSendServerShutdown()
                Else
                    Log.WriteSystemLog("GMC: Already disconnected!")
                    GlobalManagerCon.Connect(Settings.GlobalMangerIp, Settings.GlobalMangerPort)
                End If




            Case "/gmre"
                'GlobalManagerReConnect
                Log.WriteSystemLog("GMC: Started disconnect...")
                If GlobalManagerCon.ManagerSocket IsNot Nothing AndAlso GlobalManagerCon.ManagerSocket.Connected Then
                    GlobalManagerCon.UserSidedShutdown = True
                    GlobalManagerCon.ShutdownReason = SRFramework.GlobalManagerClient.GMCShutdownReason.Reconnect
                    GlobalManager.OnSendServerShutdown()
                Else
                    Log.WriteSystemLog("GMC: Already disconnected!")
                    GlobalManagerCon.Connect(Settings.GlobalMangerIp, Settings.GlobalMangerPort)
                End If

            Case "/wnd"
                perfWnd.Text = "GS: PefWnd"
                Dim thread As New Threading.Thread(AddressOf perfWnd.ShowDialog)
                thread.Start()
            Case "/writeimtable"
                GameEdit.WriteItemMallTables()
        End Select
    End Sub
End Module
