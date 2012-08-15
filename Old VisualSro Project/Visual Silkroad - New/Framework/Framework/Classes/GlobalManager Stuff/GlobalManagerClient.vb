Imports SRFramework
Imports System.Net.Sockets, System.Net

Public Class GlobalManagerClient
    Public ManagerSocket As Socket
    Public ReceiveThread As Threading.Thread

    Public LastPingTime As DateTime
    Public LastInfoTime As DateTime
    Public UpdateInfoAllowed As Boolean = False

#Region "Events"
    Public Event OnGlobalManagerInit As dGlobalManagerInit
    Public Event OnGlobalManagerShutdown As dGlobalManagerInit
    Public Event OnError As dError
    Public Event OnLog As dLog
    Public Event OnPacketReceived As dReceive
    Public Event OnGatewayUserauthReply As dGatewayUserauthReply
    Public Event OnGameserverUserauthReply As dGameserverUserauthReply

    Public Delegate Sub dGlobalManagerInit()
    Public Delegate Sub dError(ByVal ex As Exception, ByVal index As Integer)
    Public Delegate Sub dLog(ByVal message As String)
    Public Delegate Sub dReceive(ByVal packet As PacketReader)
    Public Delegate Sub dGatewayUserauthReply(ByVal succeed As Byte, ByVal errortag As Byte, ByVal sessionID As UInteger, ByVal index_ As Integer)
    Public Delegate Sub dGameserverUserauthReply(ByVal succeed As Byte, ByVal errortag As Byte, ByVal index_ As Integer)
#End Region

#Region "Connect"
    Public Sub Connect(ByVal Ip As String, ByVal port As UShort)
        Try
            UpdateInfoAllowed = False

            ManagerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ManagerSocket.Connect(New IPEndPoint(IPAddress.Parse(Ip), port))

            ReceiveThread = New Threading.Thread(AddressOf ReceiveData)
            ReceiveThread.Start()

        Catch ex As Exception
            RaiseEvent OnError(ex, -3)
        Finally
            If ManagerSocket IsNot Nothing AndAlso ManagerSocket.Connected Then
                RaiseEvent OnLog("Connected to GlobalManager[" & ManagerSocket.RemoteEndPoint.ToString & "]")
            Else
                RaiseEvent OnLog("Connecting to GlobalManager failed! Maybe a wrong IP/Port?")
                RaiseEvent OnLog("Maybe your Server cannot start without a globalmanager connection.")
            End If
        End Try
    End Sub

    Public Sub Disconnect()
        Try
            If ManagerSocket IsNot Nothing Then
                ManagerSocket.Disconnect(False)
                ManagerSocket.Close(1)
                ReceiveThread.Abort()
                ManagerSocket = Nothing
            End If

            UpdateInfoAllowed = False
        Catch ex As Exception
            RaiseEvent OnError(ex, -3)
        Finally
            If ManagerSocket Is Nothing Then
                RaiseEvent OnLog("Connection to GlobalManager closed!")
            End If
        End Try
    End Sub
#End Region

#Region "Receive"
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
                    RaiseEvent OnLog("Connection to GlobalManager lost!")
                    RaiseEvent OnError(New Exception("ConLost"), -10)
                    Exit Do
                End If

            Catch sock_ex As SocketException
                RaiseEvent OnLog("Connection to GlobalManager lost!")
                If sock_ex.ErrorCode = &H2746 Then
                    RaiseEvent OnError(sock_ex, -10)
                End If
            Catch thread_ex As Threading.ThreadAbortException
            Catch ex As Exception
                RaiseEvent OnError(ex, -10)
                Array.Clear(buffer, 0, buffer.Length)
            End Try
        Loop
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
            RaiseEvent OnPacketReceived(packet)
        Loop
    End Sub
#End Region
#Region "Send"
    Public Sub Send(ByVal buff() As Byte)
        Try
            If ManagerSocket IsNot Nothing Then
                If ManagerSocket.Connected Then
                    ManagerSocket.Send(buff)
                    LastPingTime = Date.Now
                End If
            End If
        Catch ex As Exception
            RaiseEvent OnError(ex, -5)
        End Try
    End Sub
#End Region

#Region "Methods"
    Public Sub InitComplete()
        RaiseEvent OnGlobalManagerInit()
    End Sub
    Public Sub ShutdownComplete()
        RaiseEvent OnGlobalManagerShutdown()
    End Sub
    Public Sub GatewayUserAuthReply(ByVal succeed As Byte, ByVal errortag As Byte, ByVal sessionID As UInteger, ByVal index_ As Integer)
        RaiseEvent OnGatewayUserauthReply(succeed, errortag, sessionID, index_)
    End Sub
    Public Sub GameserverUserAuthReply(ByVal succeed As Byte, ByVal errortag As Byte, ByVal index_ As Integer)
        RaiseEvent OnGameserverUserauthReply(succeed, errortag, index_)
    End Sub
    Public Sub SendPing()
        Dim writer As New PacketWriter
        writer.Create(ClientOpcodes.PING)
        Me.Send(writer.GetBytes)
    End Sub
    Public Sub Log(ByVal message As String)
        RaiseEvent OnLog(message)
    End Sub
#End Region
End Class
