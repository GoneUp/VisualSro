Imports System
Imports System.Net
Imports System.Net.Sockets

Namespace Framework

    Public Class Server
        Private Shared _Ip As IPAddress
        Private Shared _MaxClients As Integer
        Private Shared _OnlineClients As Integer
        Private Shared _ServerPort As Integer
        Private Shared ServerSocket As Socket
        Public Shared RevTheard(1) As Threading.Thread

        Public Shared Event OnClientConnect As dConnection
        Public Shared Event OnClientDisconnect As dDisconnected
        Public Shared Event OnReceiveData As dReceive
        Public Shared Event OnServerError As dError
        Public Shared Event OnServerStarted As dServerStarted
        Public Shared Event OnServerStopped As dServerStopped

#Region "Start/Stop"
        Public Shared Sub Start()
            Dim localEP As New IPEndPoint(IPAddress.Any, _ServerPort)
            ServerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Try
                ServerSocket.Bind(localEP)
                ServerSocket.Listen(5)
                ServerSocket.BeginAccept(New AsyncCallback(AddressOf Server.ClientConnect), Nothing)

                ReDim RevTheard(MaxClients + 1)
            Catch exception As Exception
                RaiseEvent OnServerError(exception, -2)
            Finally
                Dim time As String = DateTime.Now.ToString()
                RaiseEvent OnServerStarted(time)
            End Try
        End Sub
#End Region

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
                    ServerSocket.BeginAccept(New AsyncCallback(AddressOf Server.ClientConnect), Nothing)
                End If

            Catch exception As Exception
                RaiseEvent OnServerError(exception, -1)
            End Try
        End Sub

        Public Shared Sub Disconnect(ByVal index As Integer)
            Try
                Dim socket As Socket = ClientList.GetSocket(index)
                ClientList.Delete(index)
                socket.Shutdown(SocketShutdown.Both)
                Server.RevTheard(index).Abort()
                OnlineClient -= 1
            Catch ex As Exception
                RaiseEvent OnServerError(ex, -2)
            End Try
        End Sub

        Private Shared Sub ReceiveData(ByVal Index_ As Integer)
            Dim socket As Socket = ClientList.GetSocket(Index_)
            Dim buffer(8192) As Byte


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
                            Log.WriteSystemLog(buffer.ToString)
                            Exit Do
                        End If
                    Else
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
                    If socket.Connected Then
                        socket.Send(buff)
                    Else
                        Disconnect(index)
                    End If
                End If

            Catch ex As Exception
                RaiseEvent OnServerError(ex, index)
            End Try
        End Sub


        Public Shared Sub SendToAll(ByVal buff() As Byte)
            For i As Integer = 0 To MaxClients
                Dim socket As Socket = ClientList.GetSocket(i)
                If (socket IsNot Nothing) AndAlso socket.Connected Then
                    socket.Send(buff)
                End If
            Next i
        End Sub

        Public Shared Sub SendToAll(ByVal buff() As Byte, ByVal index As Integer)
            For i As Integer = 0 To MaxClients
                Dim socket As Socket = ClientList.GetSocket(i)
                If ((socket IsNot Nothing) AndAlso socket.Connected) AndAlso (i <> index) Then
                    socket.Send(buff)
                End If
            Next i
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

        Public Shared Property OnlineClient() As Integer
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
End Namespace

