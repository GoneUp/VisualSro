Imports Microsoft.VisualBasic
Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports GameServer.GameServer.Functions
Namespace GameServer

    Public Class Server
        Private Shared Buffer(&H10000 - 1) As Byte
        Private Shared IP_Renamed As IPAddress
        Private Shared maxClients_Renamed As Integer
        Private Shared onlineClient_Renamed As Integer
        Private Shared onlineMob_Renamed As Integer
        Private Shared ServerPort As Integer
        Private Shared ServerSocket As Socket

        Public Shared Event OnClientConnect As dConnection
        Public Shared Event OnClientDisconnect As dDisconnected
        Public Shared Event OnReceiveData As dReceive
        Public Shared Event OnServerError As dError
        Public Shared Event OnServerStarted As dServerStarted

        Public Shared Sub Start()
            Dim localEP As New IPEndPoint(IPAddress.Any, ServerPort)
            ServerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Try
                ServerSocket.Bind(localEP)
                ServerSocket.Listen(5)
                ServerSocket.BeginAccept(New AsyncCallback(AddressOf Server.ClientConnect), Nothing)

                ClientList.StartPingCheck()
            Catch exception As Exception
                RaiseEvent OnServerError(exception, -2)
            Finally
                Dim time As String = DateTime.Now.ToString()
                RaiseEvent OnServerStarted(time)
            End Try
        End Sub


        Private Shared Sub ClientConnect(ByVal ar As IAsyncResult)
            Try
                Dim sock As Socket = ServerSocket.EndAccept(ar)
                If OnlineClient + 1 <= 1500 Then
                    ClientList.Add(sock)
                    Dim index As Integer = ClientList.FindIndex(sock)
                    RaiseEvent OnClientConnect(sock.RemoteEndPoint.ToString(), index)
                    sock.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf Server.ReceiveData), sock)
                    ServerSocket.BeginAccept(New AsyncCallback(AddressOf Server.ClientConnect), Nothing)
                Else
                    'More then 1500 Sockets
                    sock.Disconnect(False)
                    Log.WriteSystemLog("Socket Stack Full!")
                End If

            Catch exception As Exception
                RaiseEvent OnServerError(exception, -1)
            End Try
        End Sub

        Public Shared Sub Dissconnect(ByVal index As Integer)
            Try
                Dim socket As Socket = ClientList.GetSocket(index)
                ClientList.Delete(index)
                socket.Shutdown(SocketShutdown.Both)
                OnlineClient -= 1

                Try
                    If Functions.PlayerData(index) IsNot Nothing Then
                        Functions.DespawnPlayer(index)
                    End If
                    GameServer.ClientList.OnCharListing(index) = Nothing
                    Functions.PlayerData(index) = Nothing
                Catch ex As Exception
                    Functions.PlayerData(index) = Nothing
                End Try
            Catch
            End Try
        End Sub

        Private Shared Sub ReceiveData(ByVal ar As IAsyncResult)
            Dim asyncState As Socket = CType(ar.AsyncState, Socket)
            Dim index As Integer = ClientList.FindIndex(asyncState)
            If asyncState.Connected Then
                Try
                    If asyncState.EndReceive(ar) > 0 Then
                        RaiseEvent OnReceiveData(Buffer, index)
                        Array.Clear(Buffer, 0, Buffer.Length)
                    End If
                    asyncState.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf Server.ReceiveData), asyncState)
                Catch exception As SocketException
                    If exception.ErrorCode = &H2746 Then
                        ClientList.Delete(index)
                        RaiseEvent OnClientDisconnect(asyncState.RemoteEndPoint.ToString(), index)
                    End If
                Catch exception2 As Exception
                    RaiseEvent OnServerError(exception2, index)
                    Array.Clear(Buffer, 0, Buffer.Length)
                    If asyncState.Connected = True Then
                        asyncState.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf Server.ReceiveData), asyncState)
                    End If
                End Try
            Else
                Log.WriteSystemLog(Buffer.ToString)
            End If
        End Sub


        Public Shared Sub Send(ByVal buff() As Byte, ByVal index As Integer)
            Try
                ClientList.GetSocket(index).Send(buff)

                If GameServer.Program.Logpackets = True Then
                    Log.LogPacket(buff, True)
                End If

            Catch ex As Exception
                RaiseEvent OnServerError(ex, index)
            End Try
        End Sub


        Public Shared Sub SendToAllIngame(ByVal buff() As Byte)
            For i As Integer = 0 To OnlineClient
                Dim socket As Socket = ClientList.GetSocket(i)
                Dim player As [cChar] = PlayerData(i) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                    If player.Ingame = True Then
                        socket.Send(buff)
                    End If
                End If
            Next i
        End Sub

        Public Shared Sub SendToAllIngameExpectMe(ByVal buff() As Byte, ByVal index As Integer)
            For i As Integer = 0 To MaxClients
                Dim socket As Socket = ClientList.GetSocket(i)
                Dim player As [cChar] = PlayerData(i) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso (i <> index) Then
                    If player.Ingame = True Then
                        socket.Send(buff)
                    End If
                End If
            Next i
        End Sub

        Public Shared Sub SendToAllInRange(ByVal buff() As Byte, ByVal Position As Position)
            Dim range As Integer = 750

            For i As Integer = 0 To OnlineClient
                Dim socket As Socket = ClientList.GetSocket(i)
                Dim player As [cChar] = PlayerData(i) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                    Dim distance As Long = CalculateDistance(Position, player.Position) 'Calculate Distance
                    If distance < ServerRange Then
                        'In Rage 
                        If player.Ingame = True Then
                            socket.Send(buff)
                        End If
                    End If
                End If
            Next i
        End Sub

        Public Shared Sub SendToAllInRangeExpectMe(ByVal buff() As Byte, ByVal Index As Integer)

            For i As Integer = 0 To OnlineClient
                Dim socket As Socket = ClientList.GetSocket(i)
                Dim player As [cChar] = PlayerData(i) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso (i <> Index) Then
                    Dim distance As Long = CalculateDistance(PlayerData(Index).Position, player.Position) 'Calculate Distance
                    If distance < ServerRange Then
                        'In Rage 
                        If player.Ingame = True Then
                            socket.Send(buff)
                        End If
                    End If
                End If
            Next i
        End Sub

        Public Shared Sub SendIfPlayerIsSpawned(ByVal buff() As Byte, ByVal Index_ As Integer)
            For i = 0 To MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).SpawnedPlayers.Contains(Index_) = True Or Index_ = i Then
                        Server.Send(buff, i)
                    End If
                End If
            Next
        End Sub

        Public Shared Sub SendIfMobIsSpawned(ByVal buff() As Byte, ByVal MobUniqueID As Integer)
            For i = 0 To MaxClients
                Dim socket As Socket = ClientList.GetSocket(i)
                Dim player As [cChar] = PlayerData(i) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                    If PlayerData(i).SpawnedMonsters.Contains(MobUniqueID) = True Then
                        Server.Send(buff, i)
                    End If
                End If
            Next
        End Sub

        Public Shared Sub SendIfItemIsSpawned(ByVal buff() As Byte, ByVal ItemUniqueID As Integer)
            For i = 0 To MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).SpawnedItems.Contains(ItemUniqueID) = True Then
                        Server.Send(buff, i)
                    End If
                End If
            Next
        End Sub

#Region "Propertys"
        Public Shared Property ip() As String
            Get
                Return IP_Renamed.ToString()
            End Get
            Set(ByVal value As String)
                IP_Renamed = IPAddress.Parse(value)
            End Set
        End Property

        Public Shared Property MaxClients() As Integer
            Get
                Return maxClients_Renamed
            End Get
            Set(ByVal value As Integer)
                maxClients_Renamed = value
            End Set
        End Property

        Public Shared Property OnlineClient() As Integer
            Get
                Return onlineClient_Renamed
            End Get
            Set(ByVal value As Integer)
                onlineClient_Renamed = value
            End Set
        End Property

        Public Shared Property OnlineMOB() As Integer
            Get
                Return onlineMob_Renamed
            End Get
            Set(ByVal value As Integer)
                onlineMob_Renamed = value
            End Set
        End Property

        Public Shared Property port() As Integer
            Get
                Return ServerPort
            End Get
            Set(ByVal value As Integer)
                ServerPort = value
            End Set
        End Property

        Public Delegate Sub dConnection(ByVal ip As String, ByVal index As Integer)
        Public Delegate Sub dDisconnected(ByVal ip As String, ByVal index As Integer)
        Public Delegate Sub dError(ByVal ex As Exception, ByVal index As Integer)
        Public Delegate Sub dReceive(ByVal buffer() As Byte, ByVal index As Integer)
        Public Delegate Sub dServerStarted(ByVal time As String)
#End Region
    End Class
End Namespace

