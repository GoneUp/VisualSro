Namespace GameServer.Log
    Module SystemLog
        Public Sub WriteSystemLog(ByVal Message As String)
            Dim writer As New IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory & (String.Format("{0}-{1}-{2}_Log.txt", Date.Now.Day, Date.Now.Month, Date.Now.Year)), True)
            writer.WriteLine(String.Format("[{0}]       {1}", Date.Now.ToString, Message))
            writer.Close()

            Console.WriteLine(String.Format("[{0}]       {1}", Date.Now.ToString, Message))
        End Sub

    End Module
End Namespace
