Imports SRFramework
Imports LoginServer.Framework


Namespace GlobalManager
    Module AgentService
        Public Sub OnSendUserAuth(ByVal gameserverId As UShort, ByVal username As String, ByVal password As String, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.Gateway_SendUserAuth)
            writer.DWord(Index_)
            writer.Word(gameserverId)
            writer.Word(username.Length)
            writer.String(username)
            writer.Word(password.Length)
            writer.String(password)
            GlobalManagerCon.Send(writer.GetBytes)
        End Sub

        Public Sub OnUserAuthReply(ByVal packet As PacketReader)
            Dim succeed As Byte = packet.Byte

            If succeed = 1 Then
                Dim UserIndex As Integer = packet.DWord
                Dim sessionID As UInteger = packet.DWord
                GlobalManagerCon.GatewayUserAuthReply(sessionID, UserIndex)
            End If
        End Sub
    End Module
End Namespace
