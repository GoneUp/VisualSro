Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class cServer_Base
    Private _Ip As IPAddress = IPAddress.None
    Private _MaxClients As UShort = 1
    Private _MaxNormalClients As UShort = 1
    Private _OnlineClients As UShort = 0
    Private _ServerPort As UShort = 0
    Private _Server_DebugMode As Boolean = False
    Private _ClientList As cClientList
    Private _Online As Boolean = False

    Private _ServerSocket As Socket
    Public RevTheard(1) As Thread


#Region "Events"
    Public Event OnClientConnect As dConnection
    Public Event OnClientDisconnect As dDisconnected
    Public Event OnReceiveData As dReceive
    Public Event OnServerError As dError
    Public Event OnServerStarted As dServerStarted
    Public Event OnServerStopped As dServerStopped
    Public Event OnServerLog As dServerLog
    Public Event OnServerPacketLog As dServerPacketLog

    Public Delegate Sub dConnection(ByVal ip As String, ByVal index As Integer)
    Public Delegate Sub dDisconnected(ByVal ip As String, ByVal index As Integer)
    Public Delegate Sub dError(ByVal ex As Exception, ByVal index As Integer)
    Public Delegate Sub dReceive(ByVal buffer() As Byte, ByVal index As Integer)
    Public Delegate Sub dServerStarted(ByVal time As String)
    Public Delegate Sub dServerStopped(ByVal time As String)
    Public Delegate Sub dServerLog(ByVal message As String)
    Public Delegate Sub dServerPacketLog(ByVal buff() As Byte, ByVal fromserver As Boolean, ByVal index As Integer)
#End Region

#Region "Propertys"
    Public Property Ip() As String
        Get
            Return _Ip.ToString()
        End Get
        Set(ByVal value As String)
            _Ip = IPAddress.Parse(value)
        End Set
    End Property

    Public Property MaxClients() As UShort
        Get
            Return _MaxClients
        End Get
        Set(ByVal value As UShort)
            _MaxClients = value
            ClientList.Resize(value)
        End Set
    End Property

    Public Property MaxNormalClients() As UShort
        Get
            Return _MaxNormalClients
        End Get
        Set(ByVal value As UShort)
            _MaxNormalClients = value
        End Set
    End Property

    Public Property OnlineClients() As UShort
        Get
            Return _OnlineClients
        End Get
        Set(ByVal value As UShort)
            _OnlineClients = value
        End Set
    End Property

    Public Property Port() As UShort
        Get
            Return _ServerPort
        End Get
        Set(ByVal value As UShort)
            _ServerPort = value
        End Set
    End Property

    Public Property Server_DebugMode() As Boolean
        Get
            Return _Server_DebugMode
        End Get
        Set(ByVal value As Boolean)
            _Server_DebugMode = value
        End Set
    End Property

    Public Property ClientList() As cClientList
        Get
            Return _ClientList
        End Get
        Set(ByVal value As cClientList)
            _ClientList = value
        End Set
    End Property

    Public Property Online As Boolean
        Get
            Return _Online
        End Get
        Set(ByVal value As Boolean)
            _Online = value
        End Set
    End Property
#End Region

#Region "New"
    Public Sub New()
        _ClientList = New cClientList(1)
    End Sub
#End Region

#Region "Start/Stop"
    Public Sub Start()
        Dim localEP As New IPEndPoint(IPAddress.Any, _ServerPort)
        _ServerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Try
            _ServerSocket.Bind(localEP)
            _ServerSocket.Listen(5)
            _ServerSocket.BeginAccept(New AsyncCallback(AddressOf ClientConnect), Nothing)

            ReDim RevTheard(MaxClients + 1)
        Catch exception As Exception
            RaiseEvent OnServerError(exception, -2)
        Finally
            Dim time As String = DateTime.Now.ToString()
            RaiseEvent OnServerStarted(time)
            Online = True
        End Try
    End Sub

    Public Sub [Stop]()

        Try
            _ServerSocket.Close(1)
            _ServerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ReDim RevTheard(MaxClients + 1)
        Catch exception As Exception
            RaiseEvent OnServerError(exception, -3)
        Finally
            Dim time As String = DateTime.Now.ToString()
            RaiseEvent OnServerStopped(time)
            Online = False
        End Try
    End Sub
#End Region

#Region "Connect/Disconnect"
    Private Sub ClientConnect(ByVal ar As IAsyncResult)
        Try
            Dim sock As Socket = _ServerSocket.EndAccept(ar)
            If OnlineClients + 1 <= MaxClients Then
                ClientList.Add(sock)
                Dim index As Integer = ClientList.FindIndex(sock)
                RaiseEvent OnClientConnect(sock.RemoteEndPoint.ToString(), index)
                RevTheard(index) = New Thread(AddressOf ReceiveData)
                RevTheard(index).Start(index)
            Else
                'Absolute maximum capaticy of the server, 
                'normally shouldn't apperar since there is a buffer zone between MaxNormalClients and the real maxClients variable.
                'Disconnect of users over MaxNormalClients is handeled in UserAuth Method
                'Could be a potential security fail, because clients can overflood server if they dont call the UserAuth Method
                'Solution: Add an Timeout (ca. 30 secounds) when the client doesen't call the UserAuth Method

                sock.Disconnect(False)
                RaiseEvent OnServerLog("Socket Stack Full!")
            End If

            _ServerSocket.BeginAccept(New AsyncCallback(AddressOf ClientConnect), Nothing)

        Catch argument_ex As ArgumentException
            'Server was stopped?
        Catch obj_ex As ObjectDisposedException
            'Server was stopped
        Catch exception As Exception
            RaiseEvent OnServerError(exception, -1)
        End Try
    End Sub

    Public Sub Disconnect(ByVal index As Integer)
        Try
            Dim socket As Socket = ClientList.GetSocket(index)
            If socket IsNot Nothing Then
                socket.Shutdown(SocketShutdown.Both)
            End If

            ClientList.Delete(index)
            RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), index)

        Catch thread_ex As ThreadAbortException
        Catch ex As Exception
            RaiseEvent OnServerError(ex, -4)
        End Try
    End Sub
#End Region

#Region "Receive"
    Private Sub ReceiveData(ByVal Index_ As Integer)
        Dim socket As Socket = ClientList.GetSocket(Index_)
        Dim buffer(&H10000 - 1) As Byte


        Do While True
            Try
                If socket IsNot Nothing Then
                    If socket.Connected Then
                        If socket.Available > 0 Then
                            socket.Receive(buffer, socket.Available, SocketFlags.None)
                            RaiseEvent OnReceiveData(buffer, Index_)
                            Array.Clear(buffer, 0, buffer.Length)
                        Else
                            Thread.Sleep(10)
                        End If

                    Else
                        'Not Connected..
                        socket = ClientList.GetSocket(Index_)
                        If socket IsNot Nothing Then
                            'Connection closed by the User.
                            ClientList.Delete(Index_)
                            RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), Index_)
                        Else
                            'Already Disconnected! 
                            'Connection Closed by us
                        End If
                        Exit Do
                    End If
                Else
                    'Is Nothing
                    Exit Do
                End If


            Catch sock_ex As SocketException
                If sock_ex.ErrorCode = &H2746 Then
                    If ClientList.GetSocket(Index_) IsNot Nothing Then
                        ClientList.Delete(Index_)
                        RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), Index_)
                    End If
                End If
                Exit Do
            Catch thread_ex As ThreadAbortException
                Exit Do
            Catch ex As Exception
                RaiseEvent OnServerError(ex, Index_)
                Array.Clear(buffer, 0, buffer.Length)
            End Try
        Loop
    End Sub
#End Region

#Region "Send"
    'Basic sending Class, other send methods must be impleted with "Children Classes"
    Public Sub Send(ByVal buff() As Byte, ByVal index As Integer)
        Dim socket = ClientList.GetSocket(index)
        Try
            If socket IsNot Nothing Then
                If socket.Connected Then
                    socket.Send(buff)
                Else
                    Disconnect(index)
                End If
            End If

            If Server_DebugMode = True Then
                RaiseEvent OnServerPacketLog(buff, True, index)
            End If

        Catch sock_ex As SocketException
            If sock_ex.ErrorCode = &H2746 Then
                If ClientList.GetSocket(index) IsNot Nothing Then
                    ClientList.Delete(index)
                    RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), index)
                End If
            End If
        Catch ex As Exception
            If Server_DebugMode Then
                RaiseEvent OnServerError(ex, index)
            End If
        End Try
    End Sub

    Public Sub SendToAll(ByVal buff() As Byte)
        For i As Integer = 0 To MaxClients - 1
            Dim socket As Socket = ClientList.GetSocket(i)
            If (socket IsNot Nothing) AndAlso (socket.Connected) Then
                Send(buff, i)
            End If
        Next i
    End Sub

    Public Sub SendToAll(ByVal buff() As Byte, ByVal expect_index As Integer)
        For i As Integer = 0 To MaxClients
            Dim socket As Socket = ClientList.GetSocket(i)
            If (socket IsNot Nothing) AndAlso (socket.Connected) AndAlso (i <> expect_index) Then
                Send(buff, i)
            End If
        Next i
    End Sub
#End Region
End Class


