Module Commands

    Private perfWnd As New PerfWnd

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
                Log.WriteSystemLog("/packets to enable Log")
                Log.WriteSystemLog("/clear")

            Case "/packets"

                Settings.ServerDebugMode = True
                Log.WriteSystemLog("PacketLog started!")

            Case "/debug"
                If Settings.ServerDebugMode Then
                    Settings.ServerDebugMode = False
                    Log.WriteSystemLog("Turned off DebugMode")

                ElseIf Settings.ServerDebugMode = False Then
                    Settings.ServerDebugMode = True
                    Log.WriteSystemLog("Turned on DebugMode")
                End If

            Case "/clear"
                Console.Clear()

            Case "/wnd"
                perfWnd.Text = "GM: PefWnd"
                perfWnd.ShowDialog()

            Case "/end"
                For i = 0 To SessionInfo.Count - 1
                    If SessionInfo(i) IsNot Nothing Then
                        Server.Disconnect(i)
                    End If
                Next
                Server.Stop()
                DataBase.ExecuteQuerys()
                End

        End Select



    End Sub
End Module
