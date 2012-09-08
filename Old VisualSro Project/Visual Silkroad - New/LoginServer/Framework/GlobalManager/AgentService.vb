Imports SRFramework

Namespace GlobalManager
    Module AgentService
        Public Sub OnSendUserAuth(ByVal gameserverId As UShort, ByVal username As String, ByVal password As String, ip As String, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.AGENT_USERAUTH)
            writer.DWord(Index_)
            writer.Word(gameserverId)
            writer.Word(username.Length)
            writer.String(username)
            writer.Word(password.Length)
            writer.String(password)
            writer.Word(ip.Length)
            writer.String(ip)
            GlobalManagerCon.Send(writer.GetBytes)
        End Sub

        Public Sub OnUserAuthReply(ByVal packet As PacketReader)
            GlobalManagerCon.GatewayUserAuthReply(packet)
        End Sub
    End Module
End Namespace
