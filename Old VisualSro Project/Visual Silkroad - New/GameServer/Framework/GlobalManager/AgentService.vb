Imports SRFramework
Imports System.Runtime.Serialization

Namespace GlobalManager
    Module AgentService

        Private m_formatter As IFormatter = New Formatters.Binary.BinaryFormatter()

        Public Sub OnGameserverSendUserAuth(ByVal sessionID As UInteger, ByVal username As String, ByVal password As String, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.AGENT_CHECK_USERAUTH)
            writer.DWord(Index_)
            writer.DWord(sessionID)
            writer.Word(username.Length)
            writer.String(username)
            writer.Word(password.Length)
            writer.String(password)
            GlobalManagerCon.Send(writer.GetBytes)
        End Sub

        Public Sub OnGameserverUserAuthReply(ByVal packet As PacketReader)
            Dim socketIndex As Integer = packet.DWord 'SocketIndex
            Dim succeed As Byte = packet.Byte
            Dim user As cUser = m_formatter.Deserialize(packet.BaseStream)

            If succeed = 1 Then
                GlobalManagerCon.GameserverUserAuthReply(succeed, 0, user, socketIndex)
            ElseIf succeed = 2 Then
                'Error...
                GlobalManagerCon.GameserverUserAuthReply(succeed, packet.Byte, Nothing, socketIndex)
            End If
        End Sub
    End Module
End Namespace
