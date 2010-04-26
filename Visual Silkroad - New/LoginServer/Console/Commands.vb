Module Commands

    Public Sub CheckCommand(ByVal msg As String)

        Select Case msg

            Case "/info"
                Console.WriteLine("This Emulator is from GoneUp. " & vbNewLine & "Specical Thanks to:" & vbNewLine & "Windrius for the Framework." & vbNewLine & "SREmu Team" & _
                                    vbNewLine & "Dickernoob for CSREmu" & vbNewLine & "Cheat-Project Germany [cp-g.net] <-- Best Forum ever")


            Case "/help"
                Console.WriteLine("Commands: " & vbNewLine & "/info for the credits")


            Case "/clear"
                Console.Clear()

        End Select



    End Sub




End Module
