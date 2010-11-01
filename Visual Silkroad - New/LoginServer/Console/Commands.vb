Module Commands

    Public Sub CheckCommand(ByVal msg As String)

        Select Case msg

            Case "/info"
                LoginServer.Log.WriteSystemLog("This Emulator is from GoneUp. ")
                LoginServer.Log.WriteSystemLog("Specical Thanks to:")
                LoginServer.Log.WriteSystemLog("Drew Benton")
                LoginServer.Log.WriteSystemLog("manneke for the great help")
                LoginServer.Log.WriteSystemLog("Windrius for the Framework.")
                LoginServer.Log.WriteSystemLog("SREmu Team")
                LoginServer.Log.WriteSystemLog("Dickernoob for CSREmu")
                LoginServer.Log.WriteSystemLog("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


            Case "/help"
                LoginServer.Log.WriteSystemLog("Commands: ")
                LoginServer.Log.WriteSystemLog("/info for the credits")
                LoginServer.Log.WriteSystemLog("/packets to enable packetLoginServer.Log")
                LoginServer.Log.WriteSystemLog("/clear")

            Case "/packets"

                LoginServer.Program.Logpackets = True
                LoginServer.Log.WriteSystemLog("LoginServer.Log Packets started!")


            Case "/clear"
                Console.Clear()

        End Select



    End Sub
End Module
