Class SystemLog

    Public Event OnLog As DelgateLog
    Public Delegate Sub DelgateLog(ByVal message As String)

    Public Sub WriteSystemLog(ByVal message As String)
        Try
            Dim writer As New IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory & (String.Format("{0}-{1}-{2}_Log.txt", Date.Now.Day, Date.Now.Month, Date.Now.Year)), True)
            writer.WriteLine(String.Format("[{0}]       {1}", Date.Now.ToString, message))
            writer.Close()

            Console.WriteLine(String.Format("[{0}]       {1}", Date.Now.ToString, message))
        Catch ex As Exception
        End Try
    End Sub
End Class

