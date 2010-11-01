Imports GameServer
Module Log

    Public Sub CheckCommand(ByVal msg As String)

        If msg.StartsWith("/info") Then
            GameServer.Log.WriteSystemLog("This Emulator is from GoneUp.")
            GameServer.Log.WriteSystemLog("Specical Thanks to:")
            GameServer.Log.WriteSystemLog("Drew Benton")
            GameServer.Log.WriteSystemLog("manneke for the great help")
            GameServer.Log.WriteSystemLog("Windrius for the original Framework")
            GameServer.Log.WriteSystemLog("SREmu Team")
            GameServer.Log.WriteSystemLog("Dickernoob for CSREmu")
            GameServer.Log.WriteSystemLog("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


        ElseIf msg.StartsWith("/help") Then
            GameServer.Log.WriteSystemLog("Log: ")
            GameServer.Log.WriteSystemLog("/info for the credits")
            GameServer.Log.WriteSystemLog("/packets to enable packetlog")
            GameServer.Log.WriteSystemLog("/notice [Message] - To write a global Message ")
            GameServer.Log.WriteSystemLog("/clear")

        ElseIf msg.StartsWith("/packets") Then

            GameServer.Program.Logpackets = True
            GameServer.Log.WriteSystemLog("Log Packets started!")


        ElseIf msg.StartsWith("/clear") Then
            Console.Clear()


        ElseIf msg.StartsWith("/notice") Then
            Dim spit As String = msg.Substring(8)
            GameServer.Functions.SendNotice(spit(1))


        ElseIf msg.StartsWith("/weather") Then
            Dim spit As String = msg.Substring(9)
            Dim spit2 As String() = spit.Split(" ")
            GameServer.Functions.OnSetWeather(CByte(spit2(0)), CByte(spit2(1)), 1)

        ElseIf msg = "/normalweather" Then
            GameServer.Functions.OnSetWeather(1, 75, 1)

        ElseIf msg = "/rain" Then
            GameServer.Functions.OnSetWeather(2, 75, 1)

        ElseIf msg = "/snow" Then
            GameServer.Functions.OnSetWeather(3, 75, 1)

        End If


    End Sub
End Module
