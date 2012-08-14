Namespace Log
    Module SystemLog
        Private FileQueryList As New List(Of String)

        Public Sub WriteSystemLog(ByVal Message As String)
            Try
                Console.WriteLine(String.Format("[{0}]       {1}", Date.Now.ToString, Message))
                FileQueryList.Add(String.Format("[{0}]       {1}", Date.Now.ToString, Message))
            Catch ex As Exception
            End Try
        End Sub

        Public Sub WriteSystemFileQuerys()
            Dim writer As IO.StreamWriter
            Try
                writer = New IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory & (String.Format("{0}-{1}-{2}_Log.txt", Date.Now.Day, Date.Now.Month, Date.Now.Year)), True)
                For i = 0 To FileQueryList.Count - 1
                    writer.WriteLine(FileQueryList(i))
                Next
            Catch ex As Exception
            Finally
                If writer IsNot Nothing Then
                    writer.Flush()
                    writer.Close()
                    writer.Dispose()
                End If

                FileQueryList.Clear()
            End Try
        End Sub
    End Module
End Namespace
