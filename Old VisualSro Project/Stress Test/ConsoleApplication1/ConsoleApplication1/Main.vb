Imports System.Net, System.Net.Sockets
Module Main

    Public s(50) As Socket
    Public Rev(10000) As Threading.Thread
    Public GameServer(10000) As Boolean
    Public Key(10000) As UInt32
    Public Pos(10000) As Position

    Dim ip1 As String = "127.0.0.1"
    Dim ip2 As String = "78.111.78.27"
    Dim ip3 As String = "85.214.227.25"

    Sub Main()
        Dim b

        LoadTimers(s.Length)

        Try
            For b = 0 To s.Length - 1
                s(b) = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                Rev(b) = New Threading.Thread(AddressOf ReceiveData)
                s(b).Connect(New IPEndPoint(IPAddress.Parse(ip1), 15779))
                Rev(b).Start(b)
                Pos(b) = New Position
            Next
            b = 0


            Console.Read()

        Catch ex As Exception
            Console.Write("=========" & b)
        End Try
    End Sub

    Private Sub Server_OnReceiveData(ByVal buffer() As Byte, ByVal index_ As Integer)

        Dim read As Integer = 0

        Do While True
            Dim length As Integer = BitConverter.ToUInt16(buffer, read)
            Dim opc As Integer = BitConverter.ToUInt16(buffer, read + 2)

            If length = 0 And opc = 0 Then 'endless prevention
                Exit Do
            End If

            Dim newbuff(length + 5) As Byte
            Array.ConstrainedCopy(buffer, read, newbuff, 0, length + 6)
            read = read + length + 6

            Dim packet As New PacketReader(newbuff)

            Parser.Parse(packet, index_)
        Loop



    End Sub

    Sub ReceiveData(ByVal Index_ As Integer)
        Dim socket As Socket = s(Index_)
        Dim buffer(&H10000 - 1) As Byte


        Do While True

            Try
                If socket.Connected Then
                    If socket.Available > 0 Then
                        socket.Receive(buffer, socket.Available, SocketFlags.None)
                        Server_OnReceiveData(buffer, Index_)
                        Array.Clear(buffer, 0, buffer.Length)
                    Else
                        Threading.Thread.Sleep(10)
                    End If

                Else
                    Exit Do
                End If


            Catch exception As SocketException
                If exception.ErrorCode = &H2746 Then
                    Console.WriteLine("Socket Dissconnect: " & Index_)
                End If
            Catch exception2 As Exception
                Console.WriteLine(exception2.StackTrace)
                Array.Clear(buffer, 0, buffer.Length)
            End Try
        Loop



    End Sub


    Public Sub Send(ByVal Buff() As Byte, ByVal Index_ As Integer)
        s(Index_).Send(Buff, Buff.Length, SocketFlags.None)
    End Sub
End Module
