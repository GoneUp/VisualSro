Imports SRFramework
Imports System.Threading

Namespace Functions
    Friend Class GroupSpawnSender
        Private m_thread As Thread
        
        Public Sub StartThread()
            m_thread = New Thread(AddressOf SendPlayerGroupspawns)
            m_thread.IsBackground = True
            m_thread.Start()
        End Sub

        Public Sub StopThread()
            If m_thread IsNot Nothing Then
                m_thread.Abort()
                m_thread = Nothing
            End If
        End Sub
        

        Public Sub SendPlayerGroupspawns()
            Do
                Try
                    For i = 0 To Server.MaxClients - 1
                        If PlayerData(i) IsNot Nothing Then
                            If PlayerData(i).GroupSpawnPacketsToSend.Count > 0 Then
                                Try
                                    Dim tmpPacket = PlayerData(i).GroupSpawnPacketsToSend.Dequeue()
                                    While (tmpPacket IsNot Nothing)
                                        Server.Send(tmpPacket.GetBytes, i)
                                        tmpPacket = PlayerData(i).GroupSpawnPacketsToSend.Dequeue()
                                    End While
                                Catch invOp As InvalidOperationException
                                    'Harmless, generated on a empty list
                                End Try
                            End If
                        End If
                    Next

                Catch threadEx As ThreadAbortException
                Catch objEx As ObjectDisposedException
                    'Generated on disconnect, dont worry
                Catch ex As Exception
                    Log.WriteSystemLog("GroupSpawnSender:: Message:" & ex.Message & " Stack: " & ex.StackTrace)
                End Try

                Thread.Sleep(1)
            Loop While True
        End Sub
    End Class
End Namespace
