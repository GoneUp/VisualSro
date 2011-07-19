Module Commands

    Public Sub CheckCommand(ByVal msg As String)

        Select Case msg

            Case "/info"
                GlobalManger.Log.WriteSystemLog("This Emulator is from GoneUp. ")
                GlobalManger.Log.WriteSystemLog("Specical Thanks to:")
                GlobalManger.Log.WriteSystemLog("Drew Benton")
                GlobalManger.Log.WriteSystemLog("manneke for the great help")
                GlobalManger.Log.WriteSystemLog("Windrius for the Framework.")
                GlobalManger.Log.WriteSystemLog("SREmu Team")
                GlobalManger.Log.WriteSystemLog("Dickernoob for CSREmu")
                GlobalManger.Log.WriteSystemLog("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


            Case "/help"
                GlobalManger.Log.WriteSystemLog("Commands: ")
                GlobalManger.Log.WriteSystemLog("/info for the credits")
                GlobalManger.Log.WriteSystemLog("/packets to enable packetGlobalManger.Log")
                GlobalManger.Log.WriteSystemLog("/clear")

            Case "/packets"

                GlobalManger.Program.Logpackets = True
                GlobalManger.Log.WriteSystemLog("GlobalManger.Log Packets started!")


            Case "/clear"
                Console.Clear()


            Case "/end"
                For i = 0 To GlobalManger.ClientList.SessionInfo.Count - 1
                    If GlobalManger.ClientList.SessionInfo(i) IsNot Nothing Then
                        GlobalManger.Server.Dissconnect(i)
                    End If
                Next
                ' GameServer.Server.stop()
                GlobalManger.DataBase.ExecuteQuerys()
                End

        End Select



    End Sub
End Module
