Namespace UserService
    Module UserDb
        Public RegisterList As New List(Of cRegisteredUsed)
    End Module

    Friend Class cRegisteredUsed
        Public IP As String
        Public Name As String
        Public Password As String
        Public Time As Date
    End Class
End Namespace