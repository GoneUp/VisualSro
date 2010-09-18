Public Class cExchange
    Public ExchangeID As Integer
    Public Player1Index As Integer
    Public Player2Index As Integer

    'Link to Inventory ;; -1 = free slot
    Public Items1(11) As Integer
    Public Items2(11) As Integer

    Public Player1Gold As UInt64
    Public Player2Gold As UInt64


    'Security
    Public ConfirmPlyr1 As Boolean = False
    Public ConfirmPlyr2 As Boolean = False

    Public ApprovePlyr1 As Boolean = False
    Public ApprovePlyr2 As Boolean = False

    Public Aborted As Boolean = False
    Public AbortedFrom As Integer = -1

    Sub New()
        For i = 0 To Items1.Length - 1
            Items1(i) = -1
        Next

        For i = 0 To Items2.Length - 1
            Items2(i) = -1
        Next
    End Sub

End Class
