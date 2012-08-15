Imports System.Net.Sockets
Imports System.Timers

Public Class cClientList

#Region "Propertys"
    Private m_SocketList As Socket()
    Public Property SocketList As Socket()
        Get
            Return m_SocketList
        End Get
        Set(ByVal value As Socket())
            m_SocketList = value
        End Set
    End Property

    Public m_LastPingTime As DateTime()
    Public Property LastPingTime As DateTime()
        Get
            Return m_LastPingTime
        End Get
        Set(ByVal value As DateTime())
            m_LastPingTime = value
        End Set
    End Property
#End Region

    Public Sub New(ByVal MaxUser As Integer)
        ReDim SocketList(MaxUser), LastPingTime(MaxUser)
    End Sub

    Public Sub Resize(ByVal MaxUser As Integer)
        Array.Resize(SocketList, MaxUser)
        Array.Resize(LastPingTime, MaxUser)
    End Sub

    Public Sub Add(ByVal sock As Socket)
        For i As Integer = 0 To SocketList.Length - 1
            If SocketList(i) Is Nothing Then
                SocketList(i) = sock
                Return
            End If
        Next i
    End Sub

    Public Sub Delete(ByVal index As Integer)
        If SocketList(index) IsNot Nothing Then
            SocketList(index) = Nothing
        End If
    End Sub

    Public Function FindIndex(ByVal sock As Socket) As Integer
        For i As Integer = 0 To SocketList.Length - 1
            If sock Is SocketList(i) Then
                Return i
            End If
        Next i
        Return -1
    End Function

    Public Function GetSocket(ByVal index As Integer) As Socket
        Dim socket As Socket = Nothing
        If (SocketList(index) IsNot Nothing) Then
            socket = SocketList(index)
        End If
        Return socket
    End Function

    Public Function GetIP(ByVal index As Integer) As String
        Return GetSocket(index).RemoteEndPoint.ToString
    End Function
End Class
