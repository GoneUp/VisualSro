Module Commands

    Public Sub CheckCommand(ByVal msg As String)

        Select Case msg

            Case "/info"
                Console.WriteLine("This Emulator is from GoneUp. ")
                Console.WriteLine("Specical Thanks to:")
                Console.WriteLine("Drew Benton")
                Console.WriteLine("Windrius for the Framework.")
                Console.WriteLine("SREmu Team")
                Console.WriteLine("Dickernoob for CSREmu")
                Console.WriteLine("Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


            Case "/help"
                Console.WriteLine("Commands: ")
                Console.WriteLine("/info for the credits")
                Console.WriteLine("/packets to enable packetlog")
                Console.WriteLine("/clear")

            Case "/packets"

                LoginServer.Program.Logpackets = True
                Console.WriteLine("Log Packets started!")


            Case "/clear"
                Console.Clear()

        End Select



    End Sub




End Module
