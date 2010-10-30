Public Class cCharListing
   
    Public Structure UserArray
        Public Id As Integer
        Public Name As String
        Public Pw As String
        Public FailedLogins As String
        Public Banned As Boolean
        Public LoggedIn As Boolean
    End Structure

    Public LoginInformation As UserArray
    Public Chars As New List(Of [cChar])
    Public NumberOfChars As Byte
End Class
