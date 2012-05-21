Imports Framework
Imports System.Net.Sockets, System.Net
Imports LoginServer.Framework

Namespace GlobalManager
    Module GlobalManager
        Public ManagerSocket As Socket
        Public ReceiveThread As New Threading.Thread(AddressOf ReceiveData)
        Public LastPingTime As DateTime


#Region "Events"
        Public Event OnServerError As dError
        Public Event OnServerStarted As dServerStarted
        Public Event OnServerReceive As dServerStarted

        Public Delegate Sub dError(ByVal ex As Exception, ByVal index As Integer)
        Public Delegate Sub dReceive(ByVal buffer() As Byte, ByVal index As Integer)
        Public Delegate Sub dServerStarted(ByVal time As String)
#End Region

        Public Function Connect() As Boolean
            Try
                ManagerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Unknown, ProtocolType.Tcp)
                ManagerSocket.Connect(New IPEndPoint(IPAddress.Parse(Settings.GlobalManger_Ip), Settings.GlobalManger_Port))
                ReceiveThread.Start()

            Catch ex As Exception
                RaiseEvent OnServerError(ex, -3)
            Finally
                Log.WriteSystemLog("Connected to GlobalManager[" & ManagerSocket.RemoteEndPoint.ToString & "]")
            End Try
        End Function


        Private Sub ReceiveData()
            Dim buffer(8192) As Byte

            Do While True

                Try
                    If ManagerSocket.Connected Then
                        If ManagerSocket.Available > 0 Then
                            ManagerSocket.Receive(buffer, ManagerSocket.Available, SocketFlags.None)
                            Manager_OnReceiveData(buffer)
                            Array.Clear(buffer, 0, buffer.Length)
                        Else
                            Threading.Thread.Sleep(10)
                        End If

                    Else
                        Log.WriteSystemLog("Connection to GlobalManager lost!")
                        RaiseEvent OnServerError(New Exception("LoopFail"), -10)
                        Server.Stop()
                        Exit Do
                    End If


                Catch exception As SocketException
                    Log.WriteSystemLog("Connection to GlobalManager lost!")
                    If exception.ErrorCode = &H2746 Then
                        RaiseEvent OnServerError(exception, -10)
                        Server.Stop()
                    End If
                Catch exception1 As Threading.ThreadAbortException
                    Log.WriteSystemLog("Connection to GlobalManager lost!")
                    RaiseEvent OnServerError(exception1, -10)
                    Server.Stop()
                Catch exception2 As Exception
                    RaiseEvent OnServerError(exception2, -10)
                    Array.Clear(buffer, 0, buffer.Length)
                End Try
            Loop
        End Sub


        Public Sub Send(ByVal buff() As Byte)
            ManagerSocket.Send(buff)

            If Settings.Log_Packets = True Then
                'Log.LogPacket(buff, True)
            End If
        End Sub

        Private Sub Manager_OnReceiveData(ByVal buffer() As Byte)
            Dim Pointer As Integer = 0
            Do While True
                Dim length As Integer = BitConverter.ToUInt16(buffer, Pointer)
                Dim opc As Integer = BitConverter.ToUInt16(buffer, Pointer + 2)

                If length = 0 And opc = 0 Then 'endless prevention
                    Exit Do
                End If

                Dim newbuff(length + 5) As Byte
                Array.ConstrainedCopy(buffer, Pointer, newbuff, 0, length + 6)
                Pointer = Pointer + length + 6

                Dim packet As New PacketReader(newbuff)
                If Settings.Log_Packets = True Then
                    'LoginServer.LogPacket(newbuff, False)
                End If

                Functions.Parser.ParseGlobalManager(packet)
            Loop
        End Sub



    End Module
End Namespace