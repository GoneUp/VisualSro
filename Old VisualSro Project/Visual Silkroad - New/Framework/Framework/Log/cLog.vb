Public Class cLog

    Private m_fileQueryList As New List(Of String)

#Region "Events"
    Public Event OnDatabaseQuery As dOnDatabaseQuery
    Public Delegate Sub dOnDatabaseQuery(ByVal command As String)
#End Region

#Region "Packetlog"
    Public Sub LogPacket(ByVal buffer As Byte(), ByVal fromServer As Boolean)
        Try
            Dim length As UInteger = BitConverter.ToUInt16(buffer, 0)
            Dim op As String = Hex(BitConverter.ToUInt16(buffer, 2))

            If op = "2002" Then
                Exit Sub
            End If

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
            WriteSystemLog(ex.Message & " sdfsdfsadf " & ex.StackTrace)
        End Try
    End Sub
#End Region

#Region "SystemLog"
    Public Sub WriteSystemLog(ByVal Message As String)
        Try
            Console.WriteLine(String.Format("[{0}]       {1}", Date.Now.ToString, Message))
            m_fileQueryList.Add(String.Format("[{0}]       {1}", Date.Now.ToString, Message))
        Catch ex As Exception
        End Try
    End Sub

    Public Sub WriteSystemFileQuerys()
        Dim writer As IO.StreamWriter
        Try
            writer = New IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory & (String.Format("{0}-{1}-{2}_Log.txt", Date.Now.Day, Date.Now.Month, Date.Now.Year)), True)
            For i = 0 To m_fileQueryList.Count - 1
                writer.WriteLine(m_fileQueryList(i))
            Next
        Catch ex As Exception
        Finally
            If writer IsNot Nothing Then
                writer.Flush()
                writer.Close()
                writer.Dispose()
            End If

            m_fileQueryList.Clear()
        End Try
    End Sub
#End Region

#Region "Gamelog"
    Public Sub WriteGameLog(ByVal Index_ As Integer, ByVal IP As String, ByVal Action As String, ByVal Action2 As String, ByVal Message As String)
        Dim time As String = String.Format("{0}-{1}-{2} {3}:{4}:{5}", Date.Now.Year, Date.Now.Month, Date.Now.Day, Date.Now.Hour, Date.Now.Minute, Date.Now.Second)
        RaiseEvent OnDatabaseQuery(String.Format("INSERT INTO log(ip_adress, charname, action, action2, parameter, time) VALUE ('{0}','[UNKNWON]','{1}','{2}','{3}','{4}')", IP, Action, Action2, cDatabase.CheckForInjection(Message), time))
    End Sub
#End Region

End Class
