Namespace Log
    Module PacketLog
        Public Sub LogPacket(ByVal buffer As Byte(), ByVal FromServer As Boolean)
            Try
                Dim length As UInteger = BitConverter.ToUInt16(buffer, 0)
                Dim op As String = Hex(BitConverter.ToUInt16(buffer, 2))


                If FromServer = False Then
                    If length > 0 Then
                        WriteSystemLog("C --> S (" & (op) & ")" & BitConverter.ToString(buffer, 6, length))
                    Else
                        WriteSystemLog("C --> S (" & (op) & ")")
                    End If

                ElseIf FromServer = True Then
                    If length > 0 Then
                        WriteSystemLog("S --> C (" & (op) & ")" & BitConverter.ToString(buffer, 6, length))
                    Else
                        WriteSystemLog("S --> C (" & (op) & ")")
                    End If

                End If
            Catch ex As Exception
                Log.WriteSystemLog(ex.Message & " sdfsdfsadf " & ex.StackTrace)
            End Try
        End Sub
    End Module
End Namespace
