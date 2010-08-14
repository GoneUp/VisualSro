﻿Imports Microsoft.VisualBasic
	Imports System
	Imports System.Net
	Imports System.Net.Sockets
	Imports System.Runtime.CompilerServices
Namespace GameServer

    Public Class Server
        Private Shared buffer(&H1000 - 1) As Byte
        Private Shared IP_Renamed As IPAddress
        Private Shared maxClients_Renamed As Integer
        Private Shared onlineClient_Renamed As Integer
        Private Shared onlineMob_Renamed As Integer
        Private Shared PORT_Renamed As Integer
        Private Shared s(199) As Socket
        Private Shared serverSocket As Socket

        Public Shared Event OnClientConnect As dConnection

        Public Shared Event OnClientDisconnect As dDisconnected

        Public Shared Event OnReceiveData As dReceive

        Public Shared Event OnServerError As dError

        Public Shared Event OnServerStarted As dServerStarted


        Private Shared Sub ClientConnect(ByVal ar As IAsyncResult)
            Try
                Dim sock As Socket = serverSocket.EndAccept(ar)
                ClientList.Add(sock)
                Dim index As Integer = ClientList.FindIndex(sock)
                RaiseEvent OnClientConnect(sock.RemoteEndPoint.ToString(), index)
                sock.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf Server.ReceiveData), sock)
                serverSocket.BeginAccept(New AsyncCallback(AddressOf Server.ClientConnect), Nothing)
            Catch exception As Exception
                RaiseEvent OnServerError(exception, -1)
            End Try
        End Sub

        Public Shared Sub Dissconnect(ByVal index As Integer)
            Dim socket As Socket = ClientList.GetSocket(index)
            ClientList.Delete(index)
            socket.Shutdown(SocketShutdown.Both)
        End Sub

        Private Shared Sub ReceiveData(ByVal ar As IAsyncResult)
newa:
            Dim asyncState As Socket = CType(ar.AsyncState, Socket)
            Dim index As Integer = ClientList.FindIndex(asyncState)
            If asyncState.Connected Then
                Try
                    If asyncState.EndReceive(ar) > 0 Then
                        RaiseEvent OnReceiveData(buffer, index)
                        Array.Clear(buffer, 0, buffer.Length)
                    End If
                    asyncState.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, New AsyncCallback(AddressOf Server.ReceiveData), asyncState)
                Catch exception As SocketException
                    If exception.ErrorCode = &H2746 Then
                        ClientList.Delete(index)
                        RaiseEvent OnClientDisconnect(asyncState.RemoteEndPoint.ToString(), index)
                    End If
                Catch exception2 As Exception
                    RaiseEvent OnServerError(exception2, index)
                    'GoTo newa
                End Try
            Else
                Console.WriteLine(buffer)
            End If
        End Sub


        Public Shared Sub Send(ByVal buff() As Byte, ByVal index As Integer)
            ClientList.GetSocket(index).Send(buff)

            If GameServer.Program.Logpackets = True Then
                Dim rp As New ReadPacket(buffer, index)
                PacketLog.LogPacket(rp, True)
            End If
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


        Public Shared Sub Start()
            Dim localEP As New IPEndPoint(IPAddress.Any, PORT_Renamed)
            serverSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Try
                serverSocket.Bind(localEP)
                serverSocket.Listen(5)
                serverSocket.BeginAccept(New AsyncCallback(AddressOf Server.ClientConnect), Nothing)
            Catch exception As Exception
                RaiseEvent OnServerError(exception, -2)
            Finally
                Dim time As String = DateTime.Now.ToString()
                RaiseEvent OnServerStarted(time)
            End Try
        End Sub

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
                Return PORT_Renamed
            End Get
            Set(ByVal value As Integer)
                PORT_Renamed = value
            End Set
        End Property

        Public Delegate Sub dConnection(ByVal ip As String, ByVal index As Integer)

        Public Delegate Sub dDisconnected(ByVal ip As String, ByVal index As Integer)

        Public Delegate Sub dError(ByVal ex As Exception, ByVal index As Integer)

        Public Delegate Sub dReceive(ByVal buffer() As Byte, ByVal index As Integer)

        Public Delegate Sub dServerStarted(ByVal time As String)
    End Class
End Namespace

