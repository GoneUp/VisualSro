Module Commands
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
                Settings.Server_DebugMode = True
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

                GameServer.Log.WriteSystemLog("Ending Server....")
                For i = 0 To Functions.PlayerData.Count - 1
                    If Functions.PlayerData(i) IsNot Nothing Then
                        Server.Disconnect(i)
                    End If
                Next
                Server.Stop()
                Database.ExecuteQuerys()


            Case "/debug"
                If Settings.Server_DebugMode Then
                    Settings.Server_DebugMode = False
                    Log.WriteSystemLog("Turned off DebugMode")
                    Functions.SendNotice("DEBUG Mode off!")

                ElseIf Settings.Server_DebugMode = False Then
                    Settings.Server_DebugMode = True
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
                Settings.Server_SpawnRate = 0
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
                GameServer.Log.WriteSystemLog("Ending Server....")
                For i = 0 To Functions.PlayerData.Count - 1
                    If Functions.PlayerData(i) IsNot Nothing Then
                        Server.Disconnect(i)
                    End If
                Next
                Server.Stop()
                If Server.Online Then
                    Server.Stop()
                End If
                Database.ExecuteQuerys()
                GlobalManagerCon.Disconnect()

                Log.WriteSystemLog("Cleanup Server...")

                Functions.GlobalGame.GlobalInit(Server.MaxClients)
                GlobalDef.Initalize(Server.MaxClients)
                SilkroadData.DumpDataFiles()
                GameDB.UpdateData()
                Functions.Timers.LoadTimers(Server.MaxClients)
                GameMod.Damage.OnServerStart(Server.MaxClients)

                Log.WriteSystemLog("Reconnect GlobalManager...")
                GlobalManagerCon.Connect(Settings.GlobalManger_Ip, Settings.GlobalManger_Port)
        End Select
    End Sub
End Module
