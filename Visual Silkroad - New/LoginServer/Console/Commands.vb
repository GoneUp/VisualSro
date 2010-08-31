Module Commands

    Public Sub CheckCommand(ByVal msg As String)

        Select Case msg

            Case "/info"
                Commands.WriteLog("This Emulator is from GoneUp. ")
                Commands.WriteLog("Specical Thanks to:")
                Commands.WriteLog("Drew Benton")
                Commands.WriteLog("manneke for the great help")
                Commands.WriteLog("Windrius for the Framework.")
                Commands.WriteLog("SREmu Team")
                Commands.WriteLog("Dickernoob for CSREmu")
                Commands.WriteLog("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


            Case "/help"
                Commands.WriteLog("Commands: ")
                Commands.WriteLog("/info for the credits")
                Commands.WriteLog("/packets to enable packetlog")
                Commands.WriteLog("/clear")

            Case "/packets"

                LoginServer.Program.Logpackets = True
                Commands.WriteLog("Log Packets started!")


            Case "/clear"
                Console.Clear()

        End Select



    End Sub

    Public Sub WriteLog(ByVal Message As String)
        Dim i = Date.Now
        Dim writer As New IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory & (String.Format("{0}-{1}-{2}_Log.txt", i.Day, i.Month, i.Year)), True)
        writer.WriteLine(String.Format("[{0}]       {1}", Date.Now.ToString, Message))
        writer.Close()

        Console.WriteLine(Message)
    End Sub



End Module
