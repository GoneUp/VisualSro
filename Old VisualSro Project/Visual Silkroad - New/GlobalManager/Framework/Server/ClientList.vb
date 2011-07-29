Imports System.Net.Sockets, System.Timers
Namespace Framework

    Friend Class ClientList
        Public Shared List(1500) As Socket
        Public Shared LastPingTime(1500) As DateTime
        Public Shared SessionInfo(1500) As _SessionInfo
        Public Shared WithEvents PingTimer As New Timer

        Public Shared Sub Add(ByVal sock As Socket)
            For i As Integer = 0 To List.Length - 1
                If List(i) Is Nothing Then
                    List(i) = sock
                    LastPingTime(i) = New DateTime
                    SessionInfo(i) = New _SessionInfo
                    Return
                End If
            Next i
        End Sub

        Public Shared Sub Delete(ByVal index As Integer)
            If List(index) IsNot Nothing Then
                List(index) = Nothing
                LastPingTime(index) = Nothing
                SessionInfo(index) = Nothing
            End If
        End Sub

        Public Shared Function FindIndex(ByVal sock As Socket) As Integer
            For i As Integer = 0 To List.Length - 1
                If sock Is List(i) Then
                    Return i
                End If
            Next i
            Return -1
        End Function

        Public Shared Function GetSocket(ByVal index As Integer) As Socket
            Dim socket As Socket = Nothing
            If (List(index) IsNot Nothing) AndAlso List(index).Connected Then
                socket = List(index)
            End If
            Return socket
        End Function

        Public Shared Function GetIP(ByVal index As Integer) As String
            Return GetSocket(index).RemoteEndPoint.ToString
        End Function

        Public Shared Sub SetupClientList(ByVal MaxUser As Integer)
            ReDim List(MaxUser), ClientList.LastPingTime(MaxUser), SessionInfo(MaxUser)

            PingTimer.Interval = 60000
            PingTimer.Start()
        End Sub


        Public Shared Sub CheckUserPings() Handles PingTimer.Elapsed
            PingTimer.Stop()
            Dim Count As Integer = 0

            For i = 0 To Server.MaxClients
                Dim socket As Socket = GetSocket(i)
                If socket IsNot Nothing Then
                    If DateDiff(DateInterval.Second, LastPingTime(i), DateTime.Now) > 30 Then
                        If socket.Connected = True Then
                            Server.Dissconnect(i)
                        End If
                    Else
                        Count += 1
                    End If
                End If
            Next

            Server.OnlineClient = Count

            PingTimer.Interval = 10000
            PingTimer.Start()
        End Sub

    End Class

    Public Class _SessionInfo
        Public ClientName As String
        Public ProtocolVersion As UInt32
        Public BaseKey As UInt16
        Public ServerId As UInt16

        Public HandshakeComplete As Boolean
        Public Authorized As Boolean

        Public Type As _ServerTypes

        Enum _ServerTypes
            Unknown = 0
            GlobalManager = 1
            GateWayServer = 2
            GameServer = 3
            DownloadServer = 4
            AdminTool = 5
        End Enum
    End Class
End Namespace

