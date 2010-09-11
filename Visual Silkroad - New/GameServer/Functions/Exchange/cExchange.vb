Public Class cExchange
    Public ExchangeID As Integer
    Public Player1Index As Integer
    Public Player2Index As Integer

    'Link to Inventory ;; -1 = free slot
    Public Items1(11) As Integer
    Public Items2(11) As Integer

    Sub New()
        For i = 0 To Items1.Length - 1
            Items1(i) = -1
        Next

        For i = 0 To Items2.Length - 1
            Items2(i) = -1
        Next
    End Sub

End Class
