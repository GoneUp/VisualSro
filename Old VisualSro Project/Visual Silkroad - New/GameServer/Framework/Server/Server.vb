Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports GameServer.Functions

Public Class Server
    Private Shared _Ip As IPAddress
    Private Shared _MaxClients As Integer
    Private Shared _MaxNormalClients As Integer
    Private Shared _OnlineClients As Integer
    Private Shared _ServerPort As Integer
    Private Shared ServerSocket As Socket
    Public Shared RevTheard(1) As Thread

    Public Shared Event OnClientConnect As dConnection
    Public Shared Event OnClientDisconnect As dDisconnected
    Public Shared Event OnReceiveData As dReceive
    Public Shared Event OnServerError As dError
    Public Shared Event OnServerStarted As dServerStarted
    Public Shared Event OnServerStopped As dServerStopped

    Public Shared Sub Start()
        Dim localEP As New IPEndPoint(IPAddress.Any, _ServerPort)
        ServerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Try
            ServerSocket.Bind(localEP)
            ServerSocket.Listen(5)
            ServerSocket.BeginAccept(New AsyncCallback(AddressOf ClientConnect), Nothing)

            ReDim RevTheard(MaxClients + 1)
        Catch exception As Exception
            RaiseEvent OnServerError(exception, -2)
        Finally
            Dim time As String = DateTime.Now.ToString()
            RaiseEvent OnServerStarted(time)
        End Try
    End Sub

    Public Shared Sub [Stop]()

        Try
            ServerSocket.Shutdown(SocketShutdown.Both)
            ServerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            ReDim RevTheard(MaxClients + 1)
        Catch exception As Exception
            RaiseEvent OnServerError(exception, -3)
        Finally
            Dim time As String = DateTime.Now.ToString()
            RaiseEvent OnServerStopped(time)
        End Try
    End Sub


    Private Shared Sub ClientConnect(ByVal ar As IAsyncResult)
        Try
            Dim sock As Socket = ServerSocket.EndAccept(ar)
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
                Log.WriteSystemLog("Socket Stack Full!")
            End If

            ServerSocket.BeginAccept(New AsyncCallback(AddressOf ClientConnect), Nothing)

        Catch exception As Exception
            RaiseEvent OnServerError(exception, -1)
        End Try
    End Sub

    Public Shared Sub Disconnect(ByVal index As Integer)
        Try
            Dim socket As Socket = ClientList.GetSocket(index)
            ClientList.Delete(index)
            socket.Shutdown(SocketShutdown.Both)

            RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), index)
        Catch ex As Exception
        End Try
    End Sub

    Private Shared Sub ReceiveData(ByVal Index_ As Integer)
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
                        ClientList.Delete(Index_)
                        RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), Index_)
                        Exit Do
                    End If
                Else
                    'Is Nothing
                    Exit Do
                End If


            Catch exception As SocketException
                If exception.ErrorCode = &H2746 Then
                    ClientList.Delete(Index_)
                    RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), Index_)
                End If
                Exit Do
            Catch exception1 As ThreadAbortException
                ClientList.Delete(Index_)
                RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), Index_)
                Exit Do
            Catch exception2 As Exception
                RaiseEvent OnServerError(exception2, Index_)
                Array.Clear(buffer, 0, buffer.Length)
            End Try
        Loop
    End Sub


    Public Shared Sub Send(ByVal buff() As Byte, ByVal index As Integer)
        Try
            Dim socket = ClientList.GetSocket(index)
            If socket IsNot Nothing Then
                If socket.Connected Then
                    socket.Send(buff)
                Else
                    Disconnect(index)
                End If
            End If

            If Settings.Server_DebugMode = True Then
                Log.LogPacket(buff, True)
            End If

        Catch ex As Exception
            If Settings.Log_Detail Then
                RaiseEvent OnServerError(ex, index)
            End If
        End Try
    End Sub


    Public Shared Sub SendToAllIngame(ByVal buff() As Byte)
        For i As Integer = 0 To OnlineClients
            Dim socket As Socket = ClientList.GetSocket(i)
            Dim player As [cChar] = PlayerData(i)
            'Check if Player is ingame
            If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                If player.Ingame = True Then
                    Send(buff, i)
                End If
            End If
        Next i
    End Sub

    Public Shared Sub SendToAllIngameExpectMe(ByVal buff() As Byte, ByVal index As Integer)
        For i As Integer = 0 To MaxClients
            Dim socket As Socket = ClientList.GetSocket(i)
            Dim player As [cChar] = PlayerData(i)
            'Check if Player is ingame
            If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso (i <> index) _
                Then
                If player.Ingame = True Then
                    Send(buff, i)
                End If
            End If
        Next i
    End Sub

    Public Shared Sub SendToAllInRange(ByVal buff() As Byte, ByVal Position As Position)
        For i As Integer = 0 To OnlineClients
            Dim socket As Socket = ClientList.GetSocket(i)
            Dim player As [cChar] = PlayerData(i)
            'Check if Player is ingame
            If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                Dim distance As Long = CalculateDistance(Position, player.Position)
                'Calculate Distance
                If distance < Settings.Server_Range Then
                    'In Rage 
                    If player.Ingame = True Then
                        Send(buff, i)
                    End If
                End If
            End If
        Next i
    End Sub

    Public Shared Sub SendToAllInRangeExpectMe(ByVal buff() As Byte, ByVal Index As Integer)

        For i As Integer = 0 To OnlineClients
            Dim socket As Socket = ClientList.GetSocket(i)
            Dim player As [cChar] = PlayerData(i)
            'Check if Player is ingame
            If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso (i <> Index) _
                Then
                Dim distance As Long = CalculateDistance(PlayerData(Index).Position, player.Position)
                'Calculate Distance
                If distance < Settings.Server_Range Then
                    'In Rage 
                    If player.Ingame = True Then
                        Send(buff, i)
                    End If
                End If
            End If
        Next i
    End Sub

    Public Shared Sub SendIfPlayerIsSpawned(ByVal buff() As Byte, ByVal Index_ As Integer)
        For i = 0 To MaxClients
            If PlayerData(i) IsNot Nothing Then
                If PlayerData(i).SpawnedPlayers.Contains(Index_) Or Index_ = i Then
                    If PlayerData(i).Ingame = True Then
                        Send(buff, i)
                    End If
                End If
            End If
        Next
    End Sub

    Public Shared Sub SendIfMobIsSpawned(ByVal buff() As Byte, ByVal MobUniqueID As Integer)
        For i = 0 To MaxClients
            Dim socket As Socket = ClientList.GetSocket(i)
            Dim player As [cChar] = PlayerData(i)
            'Check if Player is ingame
            If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                If PlayerData(i).SpawnedMonsters.Contains(MobUniqueID) Then
                    Send(buff, i)
                End If
            End If
        Next
    End Sub

    Public Shared Sub SendIfItemIsSpawned(ByVal buff() As Byte, ByVal ItemUniqueID As Integer)
        For i = 0 To MaxClients
            If PlayerData(i) IsNot Nothing Then
                If PlayerData(i).SpawnedItems.Contains(ItemUniqueID) Then
                    Send(buff, i)
                End If
            End If
        Next
    End Sub


    Public Shared Sub SendToGuild(ByVal buff() As Byte, ByVal GuildID As UInteger)
        For i = 0 To MaxClients
            If PlayerData(i) IsNot Nothing Then
                If PlayerData(i).GuildID = GuildID Then
                    Send(buff, i)
                End If
            End If
        Next
    End Sub


    Public Shared Sub SendToStallSession(ByVal buff() As Byte, ByVal StallID As UInteger, ByVal Owner As Boolean)
        For i = 0 To Stalls.Count - 1
            If Stalls(i).StallID = StallID Then
                'Send to Visitors
                For v = 0 To Stalls(i).Visitors.Count - 1
                    Send(buff, Stalls(i).Visitors(v))
                Next

                'Send To Owner
                If Owner Then
                    Send(buff, Stalls(i).OwnerIndex)
                End If

                Exit For
            End If
        Next
    End Sub

#Region "Propertys"

    Public Shared Property Ip() As String
        Get
            Return _Ip.ToString()
        End Get
        Set(ByVal value As String)
            _Ip = IPAddress.Parse(value)
        End Set
    End Property

    Public Shared Property MaxClients() As Integer
        Get
            Return _MaxClients
        End Get
        Set(ByVal value As Integer)
            _MaxClients = value
        End Set
    End Property

    Public Shared Property MaxNormalClients() As Integer
        Get
            Return _MaxNormalClients
        End Get
        Set(ByVal value As Integer)
            _MaxNormalClients = value
        End Set
    End Property

    Public Shared Property OnlineClients() As Integer
        Get
            Return _OnlineClients
        End Get
        Set(ByVal value As Integer)
            _OnlineClients = value
        End Set
    End Property

    Public Shared Property Port() As Integer
        Get
            Return _ServerPort
        End Get
        Set(ByVal value As Integer)
            _ServerPort = value
        End Set
    End Property

    Public Delegate Sub dConnection(ByVal ip As String, ByVal index As Integer)
    Public Delegate Sub dDisconnected(ByVal ip As String, ByVal index As Integer)
    Public Delegate Sub dError(ByVal ex As Exception, ByVal index As Integer)
    Public Delegate Sub dReceive(ByVal buffer() As Byte, ByVal index As Integer)
    Public Delegate Sub dServerStarted(ByVal time As String)
    Public Delegate Sub dServerStopped(ByVal time As String)
#End Region
End Class


