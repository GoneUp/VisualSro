Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class ServerBase
    Private m_ip As IPAddress = IPAddress.None
    Private m_maxClients As UShort = 1
    Private m_maxNormalClients As UShort = 1
    Private m_onlineClients As UShort = 0
    Private m_serverPort As UShort = 0
    Private m_serverDebugMode As Boolean = False
    Private m_clientList As cClientList
    Private m_online As Boolean = False

    Private m_downloadCounter As New cByteCounter
    Private m_uploadCounter As New cByteCounter

    Private m_serverSocket As Socket
    Private m_serverSocketV6 As Socket
    Private m_revTheard(1) As Thread



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
            Return m_ip.ToString()
        End Get
        Set(ByVal value As String)
            m_ip = IPAddress.Parse(value)
        End Set
    End Property

    Public Property MaxClients() As UShort
        Get
            Return m_maxClients
        End Get
        Set(ByVal value As UShort)
            m_maxClients = value
            ClientList.Resize(value)
        End Set
    End Property

    Public Property MaxNormalClients() As UShort
        Get
            Return m_maxNormalClients
        End Get
        Set(ByVal value As UShort)
            m_maxNormalClients = value
        End Set
    End Property

    Public Property OnlineClients() As UShort
        Get
            Return m_onlineClients
        End Get
        Set(ByVal value As UShort)
            m_onlineClients = value
        End Set
    End Property

    Public Property Port() As UShort
        Get
            Return m_serverPort
        End Get
        Set(ByVal value As UShort)
            m_serverPort = value
        End Set
    End Property

    Public Property Server_DebugMode() As Boolean
        Get
            Return m_serverDebugMode
        End Get
        Set(ByVal value As Boolean)
            m_serverDebugMode = value
        End Set
    End Property

    Public Property ClientList() As cClientList
        Get
            Return m_clientList
        End Get
        Set(ByVal value As cClientList)
            m_clientList = value
        End Set
    End Property

    Public Property Online As Boolean
        Get
            Return m_online
        End Get
        Set(ByVal value As Boolean)
            m_online = value
        End Set
    End Property

    Public Property DownloadCounter As cByteCounter
        Get
            Return m_downloadCounter
        End Get
        Set(ByVal value As cByteCounter)
            m_downloadCounter = value
        End Set
    End Property

    Public Property UploadCounter As cByteCounter
        Get
            Return m_uploadCounter
        End Get
        Set(ByVal value As cByteCounter)
            m_uploadCounter = value
        End Set
    End Property
#End Region

#Region "New"
    Public Sub New()
        m_clientList = New cClientList(1)
    End Sub
#End Region

#Region "Start/Stop"
    Public Sub Start()
        Dim localEP As New IPEndPoint(IPAddress.Any, m_serverPort)
        Dim localEpv6 As New IPEndPoint(IPAddress.IPv6Any, m_serverPort)

        m_serverSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        m_serverSocketV6 = New Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp)
        Try
            m_serverSocket.Bind(localEP)
            m_serverSocket.Listen(5)
            m_serverSocket.BeginAccept(New AsyncCallback(AddressOf ClientConnect), Nothing)

            m_serverSocketV6.Bind(localEpv6)
            m_serverSocketV6.Listen(5)
            m_serverSocketV6.BeginAccept(New AsyncCallback(AddressOf ClientConnect), Nothing)

            ReDim m_revTheard(MaxClients + 1)

        Catch sockEx As SocketException
            If sockEx.ErrorCode = 10048 Then
                'Endpoint already in use
                RaiseEvent OnServerLog("Cannout bind Server. Port is already in use!!!")
            Else
                'Other Socket Error
                RaiseEvent OnServerError(sockEx, -5)
            End If

        Catch ex As Exception
            RaiseEvent OnServerError(ex, -2)

        Finally
            If m_serverSocket IsNot Nothing AndAlso m_serverSocket.IsBound Then
                RaiseEvent OnServerStarted(DateTime.Now.ToString())
                Online = True
            Else
                RaiseEvent OnServerLog("Server NOT started! Check your configuration.")
            End If
        End Try
    End Sub

    Public Sub [Stop]()

        Try
            m_serverSocket.Close(1)
            m_serverSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ReDim m_revTheard(MaxClients + 1)

        Catch ex As Exception
            RaiseEvent OnServerError(ex, -3)
        Finally
            If m_serverSocket IsNot Nothing AndAlso m_serverSocket.IsBound = False Then
                RaiseEvent OnServerStopped(DateTime.Now.ToString())
                Online = False
            Else
                RaiseEvent OnServerLog("Server NOT stopped! Check your configuration.")
            End If
        End Try
    End Sub
#End Region

#Region "Connect/Disconnect"
    Private Sub ClientConnect(ByVal ar As IAsyncResult)
        Try
            Dim sock As Socket = m_serverSocket.EndAccept(ar)
            If OnlineClients + 1 <= MaxClients Then
                ClientList.Add(sock)
                Dim index As Integer = ClientList.FindIndex(sock)
                RaiseEvent OnClientConnect(sock.RemoteEndPoint.ToString(), index)
                m_revTheard(index) = New Thread(AddressOf ReceiveData)
                m_revTheard(index).Start(index)
            Else
                'Absolute maximum capaticy of the server, 
                'normally shouldn't apperar since there is a buffer zone between MaxNormalClients and the real maxClients variable.
                'Disconnect of users over MaxNormalClients is handeled in UserAuth Method
                'Could be a potential security fail, because clients can overflood server if they dont call the UserAuth Method
                'Solution: Add an Timeout (ca. 30 secounds) when the client doesen't call the UserAuth Method

                sock.Disconnect(False)
                RaiseEvent OnServerLog("Socket Stack Full!")
            End If

            m_serverSocket.BeginAccept(New AsyncCallback(AddressOf ClientConnect), Nothing)

        Catch argumentEx As ArgumentException
            'Server was stopped?
        Catch objEx As ObjectDisposedException
            'Server was stopped
        Catch ex As Exception
            RaiseEvent OnServerError(ex, -1)
        End Try
    End Sub

    Public Sub Disconnect(ByVal index As Integer)
        Try
            Dim socket As Socket = ClientList.GetSocket(index)
            If socket IsNot Nothing Then
                socket.Shutdown(SocketShutdown.Both)
                RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), index)
            End If

            ClientList.Delete(index)

        Catch threadEx As ThreadAbortException
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


            Catch sockEx As SocketException
                If sockEx.ErrorCode = &H2746 Then
                    If ClientList.GetSocket(Index_) IsNot Nothing Then
                        ClientList.Delete(Index_)
                        RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), Index_)
                    End If
                End If
                Exit Do
            Catch threadEx As ThreadAbortException
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

            UploadCounter.AddPacket(buff.Length, PacketSource.Server)

        Catch sockEx As SocketException
            If sockEx.ErrorCode = &H2746 Then
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

    Public Sub SendToAll(ByVal buff() As Byte, ByVal expectIndex As Integer)
        For i As Integer = 0 To MaxClients
            Dim socket As Socket = ClientList.GetSocket(i)
            If (socket IsNot Nothing) AndAlso (socket.Connected) AndAlso (i <> expectIndex) Then
                Send(buff, i)
            End If
        Next i
    End Sub
#End Region
End Class


