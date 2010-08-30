Module PacketLog

    Public Sub LogPacket(ByVal Packet As LoginServer.ReadPacket, ByVal FromServer As Boolean)
        Try
            If FromServer = False Then
                Commands.WriteLog("C --> S (" & (Packet.opcode) & ")" & BitConverter.ToString(Packet.buffer, 0, BitConverter.ToUInt16(Packet.buffer, 0)))
            ElseIf FromServer = True Then
                Commands.WriteLog("S --> C (" & (Packet.opcode) & ")" & BitConverter.ToString(Packet.buffer, 0, BitConverter.ToUInt16(Packet.buffer, 0)))
            End If
        Catch ex As Exception

        End Try



    End Sub
End Module
