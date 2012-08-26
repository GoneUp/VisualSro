Imports SRFramework

Namespace GlobalManager
    Module AgentService
        Public Sub OnGameserverSendUserAuth(ByVal sessionID As UInteger, ByVal username As String, ByVal password As String, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.AGNET_CHECK_USERAUTH)
            writer.DWord(Index_)
            writer.DWord(sessionID)
            writer.Word(username.Length)
            writer.String(username)
            writer.Word(password.Length)
            writer.String(password)
            GlobalManagerCon.Send(writer.GetBytes)
        End Sub

        Public Sub OnGameserverUserAuthReply(ByVal packet As PacketReader)
            Dim UserIndex As Integer = packet.DWord
            Dim succeed As Byte = packet.Byte

            If succeed = 1 Then
                GlobalManagerCon.GameserverUserAuthReply(succeed, 0, UserIndex)
            ElseIf succeed = 2 Then
                'Error...
                GlobalManagerCon.GameserverUserAuthReply(succeed, packet.Byte, UserIndex)
            End If
        End Sub
    End Module
End Namespace
