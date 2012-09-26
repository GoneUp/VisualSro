Namespace Functions
    Public Class Stall
        Public StallID As UInteger
        Public OwnerIndex As Integer
        Public OwnerID As UInteger
        Public Items(10) As StallItem

        Public Open As Boolean = False

        Public StallName As String
        Public WelcomeMessage As String

        Public Visitors As New List(Of Integer)

        Structure StallItem
            Public Slot As Byte
            Public Data As UShort
            Public Gold As Long
        End Structure

        Sub Init()
            For i = 0 To Items.Count - 1
                Items(i) = New StallItem
            Next
        End Sub
    End Class
End Namespace
