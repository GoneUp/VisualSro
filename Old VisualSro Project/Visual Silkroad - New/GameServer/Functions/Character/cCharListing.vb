Namespace GameServer.Functions
    Public Class cCharListing
        Public Structure UserArray
            Public Id As Integer
            Public Name As String
            Public Pw As String
            Public FailedLogins As String
            Public Banned As Boolean
            Public LoggedIn As Boolean
            Public Permission As Byte

            Public Silk As UInteger
            Public Silk_Bonus As UInteger
            Public Silk_Points As UInteger
        End Structure

        Public LoginInformation As UserArray
        Public Chars As New List(Of [cChar])

        Public ReadOnly Property NumberOfChars As Byte
            Get
                Return Chars.Count
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim accountString As String = String.Format("cCharListing; ID: {0}, Name: {1}", LoginInformation.Id, LoginInformation.Name)
            Dim charString As String = ""
            Dim silkString As String = String.Format("Silk: {0}", LoginInformation.Silk)

            For i = 0 To Chars.Count - 1
                If Chars(i) IsNot Nothing Then
                    charString += String.Format("- Char{0}; ID: {1}, Name: {2} ", i, Chars(i).CharacterId, Chars(i).CharacterName)
                End If
            Next

            Return accountString & " ## " & silkString & " ## " & charString
        End Function

    End Class
End Namespace
