Module PacketLog

    Public Sub LogPacket(ByVal Packet As GameServer.ReadPacket, ByVal FromServer As Boolean, ByVal data As Byte())
        Try
            If FromServer = False Then
                Console.WriteLine("C --> S (" & (Packet.opcode) & ")" & BitConverter.ToString(data, 0, data.Length))
            ElseIf FromServer = True Then
                Console.WriteLine("S --> C (" & (Packet.opcode) & ")" & BitConverter.ToString(data, 0, data.Length))
            End If
        Catch ex As Exception

        End Try


    End Sub
End Module
