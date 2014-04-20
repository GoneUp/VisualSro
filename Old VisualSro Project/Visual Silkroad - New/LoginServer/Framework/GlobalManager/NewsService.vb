Imports SRFramework

Namespace GlobalManager
    Module NewsService
        Public Sub RequestNews()
            Dim writer As New PacketWriter(InternalClientOpcodes.AGENT_NEWS)
            GlobalManagerCon.Send(writer.GetBytes)
        End Sub
        Public Sub OnNewsHandler(packet As PacketReader)
            LauncherMessages.Clear()

            Dim count As UInt16 = packet.Word

            For i = 0 To count - 1
                Dim tmpMsg As LauncherMessage = BinFormatter.Deserialize(packet.BaseStream)

                If tmpMsg IsNot Nothing Then
                    LauncherMessages.Add(tmpMsg)
                End If
            Next
        End Sub
    End Module
End Namespace