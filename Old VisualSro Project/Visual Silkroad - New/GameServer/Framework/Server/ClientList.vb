Imports System.Net.Sockets
Imports GameServer.Functions
Imports System.Timers

Public Class ClientList
    Public Shared SocketList(1500) As Socket
    Public Shared SessionInfo(1500) As _SessionInfo
    Public Shared WithEvents PingTimer As New Timer

    Public Shared Sub Add(ByVal sock As Socket)
        For i As Integer = 0 To SocketList.Length - 1
            If SocketList(i) Is Nothing Then
                SocketList(i) = sock
                SessionInfo(i) = New _SessionInfo
                Return
            End If
        Next i
    End Sub

    Public Shared Sub Delete(ByVal index As Integer)
        If SocketList(index) IsNot Nothing Then
            SocketList(index) = Nothing
            SessionInfo(index) = Nothing
        End If
    End Sub

    Public Shared Function FindIndex(ByVal sock As Socket) As Integer
        For i As Integer = 0 To SocketList.Length - 1
            If sock Is SocketList(i) Then
                Return i
            End If
        Next i
        Return -1
    End Function

    Public Shared Function GetSocket(ByVal index As Integer) As Socket
        Dim socket As Socket = Nothing
        If (SocketList(index) IsNot Nothing) AndAlso SocketList(index).Connected Then
            socket = SocketList(index)
        End If
        Return socket
    End Function

    Public Shared Sub SetupClientList(ByVal MaxUser As Integer)
        ReDim SocketList(MaxUser)

        PingTimer.Interval = 30000
        PingTimer.Start()
    End Sub

    Public Shared Sub CheckUserPings() Handles PingTimer.Elapsed
        PingTimer.Stop()
        Dim Count As Integer = 0

        For i = 0 To Server.MaxClients
            Dim socket As Socket = GetSocket(i)
            If socket IsNot Nothing AndAlso socket.Connected AndAlso SessionInfo(i) IsNot Nothing Then
                If Settings.Server_DebugMode AndAlso DateDiff(DateInterval.Second, SessionInfo(i).LastPingTime, DateTime.Now) > 30 Then
                    Server.Disconnect(i)
                ElseIf SessionInfo(i).LoginAuthRequired And DateDiff(DateInterval.Second, SessionInfo(i).LoginAuthTimeout, DateTime.Now) > 0 Then
                    'LoginAuth is missing..
                    Server.Disconnect(i)
                Else
                    Count += 1
                End If
            End If
        Next

        Server.OnlineClients = Count

        PingTimer.Interval = 10000
        PingTimer.Start()
    End Sub
End Class

Public Class _SessionInfo
    Public Ip As String = ""
    Public ClientName As String = ""
    Public ClientType As ConnectionType = ConnectionType.SR_Client

    Public LastPingTime As New DateTime

    Public LoginAuthRequired As Boolean = False
    Public LoginAuthTimeout As New DateTime

    Public Username As String = ""
    Public Charname As String = ""

    Public Authorized As Boolean = False

    Enum ConnectionType
        SR_Client = 0
        SR_Admin = 1
    End Enum
End Class


