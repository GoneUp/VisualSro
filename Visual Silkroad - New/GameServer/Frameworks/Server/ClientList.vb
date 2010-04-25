Imports Microsoft.VisualBasic
	Imports System
	Imports System.Net.Sockets
Namespace GameServer

    Public Class ClientList
        Public Shared lista(1500) As Socket
        Public Shared OnCharListing(1500) As cCharListing

        Public Shared Sub Add(ByVal sock As Socket)
            For i As Integer = 0 To lista.Length - 1
                If lista(i) Is Nothing Then
                    lista(i) = sock
                    Return
                End If
            Next i
        End Sub

        Public Shared Sub Delete(ByVal index As Integer)
            If lista(index) IsNot Nothing Then
                lista(index) = Nothing
            End If
        End Sub

        Public Shared Function FindIndex(ByVal sock As Socket) As Integer
            For i As Integer = 0 To lista.Length - 1
                If sock Is lista(i) Then
                    Return i
                End If
            Next i
            Return -1
        End Function

        Public Shared Function GetSocket(ByVal index As Integer) As Socket
            Dim socket As Socket = Nothing
            If (lista(index) IsNot Nothing) AndAlso lista(index).Connected Then
                socket = lista(index)
            End If
            Return socket
        End Function
    End Class
End Namespace

