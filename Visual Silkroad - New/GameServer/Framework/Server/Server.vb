Imports Microsoft.VisualBasic
Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports GameServer.GameServer.Functions
Namespace GameServer

    Public Class Server
        Private Shared IP_ As IPAddress
        Private Shared MaxClients_ As Integer
        Private Shared OnlineClient_ As Integer
        Private Shared ServerPort As Integer
        Private Shared ServerSocket As Socket
        Public Shared RevTheard(1) As Threading.Thread

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

                ClientList.SetupClientList(MaxClients)

                ReDim RevTheard(MaxClients + 1)
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
                If OnlineClient + 1 <= MaxClients Then
                    ClientList.Add(sock)
                    Dim index As Integer = ClientList.FindIndex(sock)
                    RaiseEvent OnClientConnect(sock.RemoteEndPoint.ToString(), index)
                    RevTheard(index) = New Threading.Thread(AddressOf ReceiveData)
                    RevTheard(index).Start(index)
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
                                Threading.Thread.Sleep(10)
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
                Catch exception1 As Threading.ThreadAbortException
                    ClientList.Delete(Index_)
                    RaiseEvent OnClientDisconnect(socket.RemoteEndPoint.ToString(), Index_)
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
                    socket.Send(buff)
                End If


                If GameServer.Program.Logpackets = True Then
                    Log.LogPacket(buff, True)
                End If

            Catch ex As Exception
                If Settings.Log_Detail Then
                    RaiseEvent OnServerError(ex, index)
                End If
            End Try
        End Sub


        Public Shared Sub SendToAllIngame(ByVal buff() As Byte)
            For i As Integer = 0 To OnlineClient
                Dim socket As Socket = ClientList.GetSocket(i)
                Dim player As [cChar] = PlayerData(i) 'Check if Player is ingame
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
                Dim player As [cChar] = PlayerData(i) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso (i <> index) Then
                    If player.Ingame = True Then
                        Send(buff, i)
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
                    If distance < Settings.ServerRange Then
                        'In Rage 
                        If player.Ingame = True Then
                            Send(buff, i)
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
                    If distance < Settings.ServerRange Then
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
                    If PlayerData(i).SpawnedPlayers.Contains(Index_) = True Or Index_ = i Then
                        Send(buff, i)
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
                        Send(buff, i)
                    End If
                End If
            Next
        End Sub

        Public Shared Sub SendIfItemIsSpawned(ByVal buff() As Byte, ByVal ItemUniqueID As Integer)
            For i = 0 To MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).SpawnedItems.Contains(ItemUniqueID) = True Then
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
                        Server.Send(buff, Stalls(i).Visitors(v))
                    Next

                    'Send To Owner
                    If Owner Then
                        Server.Send(buff, Stalls(i).OwnerIndex)
                    End If

                    Exit For
                End If
            Next
        End Sub

#Region "Propertys"
        Public Shared Property Ip() As String
            Get
                Return IP_.ToString()
            End Get
            Set(ByVal value As String)
                IP_ = IPAddress.Parse(value)
            End Set
        End Property

        Public Shared Property MaxClients() As Integer
            Get
                Return MaxClients_
            End Get
            Set(ByVal value As Integer)
                MaxClients_ = value
            End Set
        End Property

        Public Shared Property OnlineClient() As Integer
            Get
                Return OnlineClient_
            End Get
            Set(ByVal value As Integer)
                OnlineClient_ = value
            End Set
        End Property

        Public Shared Property Port() As Integer
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

