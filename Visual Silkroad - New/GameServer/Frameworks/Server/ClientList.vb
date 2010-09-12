Imports Microsoft.VisualBasic
	Imports System
	Imports System.Net.Sockets
Namespace GameServer

    Public Class ClientList
        Public Shared List(1500) As Socket
        Public Shared OnCharListing(1500) As cCharListing
        Public Shared PlayerData(1500) As [cChar]

        Public Shared Sub Add(ByVal sock As Socket)
            For i As Integer = 0 To List.Length - 1
                If List(i) Is Nothing Then
                    List(i) = sock
                    Return
                End If
            Next i
        End Sub

        Public Shared Sub Delete(ByVal index As Integer)
            If List(index) IsNot Nothing Then
                List(index) = Nothing
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
    End Class
End Namespace

