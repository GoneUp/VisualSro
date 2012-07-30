Imports SRFramework
Imports System.Net.Sockets, System.Net

Public Class GlobalManagerClient
    Public ManagerSocket As Socket
    Public ReceiveThread As New Threading.Thread(AddressOf ReceiveData)

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

    Public Delegate Sub dGlobalManagerInit()
    Public Delegate Sub dError(ByVal ex As Exception, ByVal index As Integer)
    Public Delegate Sub dLog(ByVal message As String)
    Public Delegate Sub dReceive(ByVal packet As PacketReader)
    Public Delegate Sub dGatewayUserauthReply(ByVal sessionID As UInteger, ByVal index_ As Integer)
#End Region

#Region "Connect"
    Public Sub Connect(ByVal Ip As String, ByVal port As UShort)
        Try
            ManagerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ManagerSocket.Connect(New IPEndPoint(IPAddress.Parse(Ip), port))
            ReceiveThread.Start()

        Catch ex As Exception
            RaiseEvent OnError(ex, -3)
        Finally
            RaiseEvent OnLog("Connected to GlobalManager[" & ManagerSocket.RemoteEndPoint.ToString & "]")
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

            Catch exception As SocketException
                RaiseEvent OnLog("Connection to GlobalManager lost!")
                If exception.ErrorCode = &H2746 Then
                    RaiseEvent OnError(exception, -10)
                End If
            Catch exception1 As Threading.ThreadAbortException
                RaiseEvent OnLog("Connection to GlobalManager lost!")
                RaiseEvent OnError(exception1, -10)
            Catch exception2 As Exception
                RaiseEvent OnError(exception2, -10)
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
    Public Sub SendPing()
        Dim writer As New PacketWriter
        writer.Create(ClientOpcodes.PING)
        Me.Send(writer.GetBytes)
    End Sub
#End Region

End Class
