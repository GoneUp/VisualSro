Public Class cCharListing
   
    Public Structure UserArray
        Public Id As Integer
        Public Name As String
        Public Pw As String
        Public FailedLogins As String
        Public Banned As Boolean
    End Structure

    Public LoginInformation As UserArray
    Public Char1 As [cChar]
    Public Char2 As [cChar]
    Public Char3 As [cChar]
    Public Char4 As [cChar]
End Class
