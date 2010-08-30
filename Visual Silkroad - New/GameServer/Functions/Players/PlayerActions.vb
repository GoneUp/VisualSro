Namespace GameServer.Functions
    Module PlayerActions
        Public Sub OnLogout(ByVal packet As PacketReader, ByVal Index As Integer)

            Dim tag As Byte = packet.Byte
            Select Case tag
                Case 1 'Normal Exit
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Exit)
                    writer.Byte(1) 'sucees
                    writer.Byte(1) '1 sekunde
                    writer.Byte(tag) 'mode
                    Server.Send(writer.GetBytes, Index)

                    writer = New PacketWriter
                    writer.Create(ServerOpcodes.Exit2)
                    Server.Send(writer.GetBytes, Index)



                Case 2 'Restart
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Exit)
                    writer.Byte(1) 'sucees
                    writer.Byte(1) '1 sekunde
                    writer.Byte(tag) 'mode 
                    Server.Send(writer.GetBytes, Index)

                    writer = New PacketWriter
                    writer.Create(ServerOpcodes.Exit2)
                    Server.Send(writer.GetBytes, Index)



            End Select
        End Sub
    End Module
End Namespace
