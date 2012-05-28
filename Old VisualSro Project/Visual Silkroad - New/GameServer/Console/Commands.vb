Imports GameServer.GameServer

Module Log
    Public Sub CheckCommand(ByVal FullMessage As String)

        Dim msg() As String = FullMessage.Split(" ")

        Select Case msg(0)

            Case "/info"
                GameServer.Log.WriteSystemLog("This Emulator is from GoneUp.")
                GameServer.Log.WriteSystemLog("Specical Thanks to:")
                GameServer.Log.WriteSystemLog("Drew Benton")
                GameServer.Log.WriteSystemLog("manneke for the great help")
                GameServer.Log.WriteSystemLog("Windrius for the original Framework")
                GameServer.Log.WriteSystemLog("SREmu Team")
                GameServer.Log.WriteSystemLog("Dickernoob for CSREmu")
                GameServer.Log.WriteSystemLog("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


            Case "/help"
                GameServer.Log.WriteSystemLog("Log: ")
                GameServer.Log.WriteSystemLog("/info for the credits")
                GameServer.Log.WriteSystemLog("/packets to enable packetlog")
                GameServer.Log.WriteSystemLog("/notice [Message] - To write a global Message ")
                GameServer.Log.WriteSystemLog("/clear")

            Case "/packets"

                Program.Logpackets = True
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
                GameServer.Log.WriteSystemLog(String.Format("Count Player:{0}!", Server.OnlineClient))
                GameServer.Log.WriteSystemLog(String.Format("Count Mob:{0}!", Functions.MobList.Count))

            Case "/cleanup"
                Dim mem As Long = Process.GetCurrentProcess.PrivateMemorySize64
                GC.Collect()
                GameServer.Log.WriteSystemLog(
                    "Cleanup Memory in mb: " & (mem - Process.GetCurrentProcess.PrivateMemorySize64)/1024/1024)
            Case "/end"

                GameServer.Log.WriteSystemLog("Ending Server....")
                For i = 0 To Functions.PlayerData.Count - 1
                    If Functions.PlayerData(i) IsNot Nothing Then
                        Server.Disconnect(i)
                    End If
                Next
                Server.Stop()
                DataBase.ExecuteQuerys()
                End

            Case "/dcoff"
                Settings.Server_PingDc = False
                GameServer.Log.WriteSystemLog("Turned off PingCheck")
            Case "/dcon"
                Settings.Server_PingDc = True
                GameServer.Log.WriteSystemLog("Turned on PingCheck")
            Case "/killmobs"
                GameServer.Log.WriteSystemLog("In Progress, Count: " & Functions.MobList.Count)
                For Each key In Functions.MobList.Keys.ToList
                    If Functions.MobList.ContainsKey(key) Then
                        Functions.RemoveMob(key)
                    End If
                Next
                GameServer.Log.WriteSystemLog("Finished!")

            Case "/respawnoff"
                Settings.Server_SpawnRate = 0
                GameServer.Log.WriteSystemLog("Turned off Respawn")
            Case "/save"
                Dim path = AppDomain.CurrentDomain.BaseDirectory & "npcpos_saved.txt"
                Dim cloack As New Stopwatch
                cloack.Start()
                GameServer.Log.WriteSystemLog("Save start to: " & path)
                Functions.SaveAutoSpawn(path)
                cloack.Stop()
                GameServer.Log.WriteSystemLog("Save Fin! S:" & cloack.Elapsed().TotalSeconds)
        End Select
    End Sub
End Module
