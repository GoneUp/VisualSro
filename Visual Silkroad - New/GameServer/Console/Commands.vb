Imports GameServer
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

                GameServer.Program.Logpackets = True
                GameServer.Log.WriteSystemLog("Log Packets started!")


            Case "/clear"
                Console.Clear()


            Case "/notice"
                GameServer.Functions.SendNotice(msg(1))


            Case "/weather"
                GameServer.Functions.OnSetWeather(CByte(msg(1)), CByte(msg(2)))

            Case "/normalweather"
                GameServer.Functions.OnSetWeather(1, 75)

            Case "/rain"
                GameServer.Functions.OnSetWeather(2, 75)

            Case "/snow"
                GameServer.Functions.OnSetWeather(3, 75)

            Case "/count"
                GameServer.Log.WriteSystemLog(String.Format("Count:{0}!", GameServer.Server.OnlineClient))

            Case "/cleanup"
                System.GC.Collect()

            Case "/end"
                For i = 0 To GameServer.Functions.PlayerData.Count - 1
                    If GameServer.Functions.PlayerData(i) IsNot Nothing Then
                        GameServer.Server.Dissconnect(i)
                    End If
                Next
                GameServer.Server.Stop()
                GameServer.DataBase.ExecuteQuerys()
                End
        End Select

    End Sub
End Module
