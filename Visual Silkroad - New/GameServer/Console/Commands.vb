Module Commands

    Public Sub CheckCommand(ByVal msg As String)

        If msg.StartsWith("/info") Then
            Commands.WriteLog("This Emulator is from GoneUp. ")
            Commands.WriteLog("Specical Thanks to:")
            Commands.WriteLog("Drew Benton")
            Commands.WriteLog("Windrius for the Framework.")
            Commands.WriteLog("SREmu Team")
            Commands.WriteLog("Dickernoob for CSREmu")
            Commands.WriteLog("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


        ElseIf msg.StartsWith("/help") Then
            Commands.WriteLog("Commands: ")
            Commands.WriteLog("/info for the credits")
            Commands.WriteLog("/packets to enable packetlog")
            Commands.WriteLog("/notice [Message] - To write a global Message ")
            Commands.WriteLog("/clear")

        ElseIf msg.StartsWith("/packets") Then

            GameServer.Program.Logpackets = True
            Commands.WriteLog("Log Packets started!")


        ElseIf msg.StartsWith("/clear") Then
            Console.Clear()


        ElseIf msg.StartsWith("/notice") Then
            Dim spit As String = msg.Substring(8)
            ' GameServer.Functions.WriteNotice(spit)


        End If



    End Sub

    Public Sub WriteLog(ByVal Message As String)
        Dim i = Date.Now
        Dim writer As New IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory & (String.Format("{0}-{1}-{2}_Log.txt", i.Day, i.Month, i.Year)), True)
        writer.WriteLine(String.Format("[{0}]       {1}", Date.Now.ToString, Message))
        writer.Close()

        Console.WriteLine(Message)
    End Sub



End Module
