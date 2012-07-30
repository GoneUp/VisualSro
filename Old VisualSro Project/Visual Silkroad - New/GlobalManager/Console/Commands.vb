Module Commands

    Public Sub CheckCommand(ByVal msg As String)

        Select Case msg

            Case "/info"
                GlobalManager.Log.WriteSystemLog("This Emulator is from GoneUp. ")
                GlobalManager.Log.WriteSystemLog("Specical Thanks to:")
                GlobalManager.Log.WriteSystemLog("Drew Benton")
                GlobalManager.Log.WriteSystemLog("manneke for the great help")
                GlobalManager.Log.WriteSystemLog("Windrius for the Framework.")
                GlobalManager.Log.WriteSystemLog("SREmu Team")
                GlobalManager.Log.WriteSystemLog("Dickernoob for CSREmu")
                GlobalManager.Log.WriteSystemLog("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


            Case "/help"
                GlobalManager.Log.WriteSystemLog("Commands: ")
                GlobalManager.Log.WriteSystemLog("/info for the credits")
                GlobalManager.Log.WriteSystemLog("/packets to enable packetGlobalManager.Log")
                GlobalManager.Log.WriteSystemLog("/clear")

            Case "/packets"

                GlobalManager.Settings.Log_Packets = True
                GlobalManager.Log.WriteSystemLog("GlobalManager.Log Packets started!")


            Case "/clear"
                Console.Clear()


            Case "/end"
                For i = 0 To Framework.ClientList.SessionInfo.Count - 1
                    If Framework.ClientList.SessionInfo(i) IsNot Nothing Then
                        Framework.Server.Disconnect(i)
                    End If
                Next
                ' GameServer.Server.stop()
                Framework.DataBase.ExecuteQuerys()
                End

        End Select



    End Sub
End Module
