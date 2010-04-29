Public Class cInventory

    Sub New(ByVal slots As Integer)
        ReDim UserItems(slots)
        For i = 0 To UserItems.Length - 1
            UserItems(i) = New cInvItem
        Next
    End Sub

    Public UserItems(255) As cInvItem


End Class
