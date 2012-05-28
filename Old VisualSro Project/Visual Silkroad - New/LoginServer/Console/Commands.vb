Imports LoginServer.Framework

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
                Log.WriteSystemLog("/packets to enable packetLoginServer.Log")
                Log.WriteSystemLog("/clear")

            Case "/packets"

                Program.Logpackets = True
                Log.WriteSystemLog("LoginServer.Log Packets started!")

            Case "/count"
                Log.WriteSystemLog("Sockets online: " & Server.OnlineClient)

            Case "/clear"
                Console.Clear()

            Case "/end"
                For i = 0 To ClientList.SessionInfo.Count - 1
                    If ClientList.SessionInfo(i) IsNot Nothing Then
                        Server.Dissconnect(i)
                    End If
                Next
                ' GameServer.Server.stop()
                Database.ExecuteQuerys()
                End

        End Select



    End Sub
End Module
