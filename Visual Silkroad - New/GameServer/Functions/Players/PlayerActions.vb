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
        Public Sub OnPlayerAction(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim action As Byte = packet.Byte

            'UpdateState(1, action, Index_)


        End Sub

        Public Sub UpdateState(ByVal Type As Byte, ByVal State As Byte, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Action)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Byte(Type)
            writer.Byte(State)
            Server.SendToAllInRange(writer.GetBytes, Index_)
        End Sub
    End Module
End Namespace
