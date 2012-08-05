Namespace Functions
    Public Class cCharListing


        Public LoginInformation As cUser
        Public Chars As New List(Of cCharacter)

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

    Public Class cUser
        Public Id As Integer = 0
        Public Name As String = ""
        Public Pw As String = ""
        Public FailedLogins As Byte = 0
        Public Banned As Boolean = False
        Public LoggedIn As Boolean = False
        Public Permission As Byte = 0 '0x00 = normal user, 0x01 = prefered access to the server (premium), 0x02 = gm, 0x03 = admin

        Public Silk As UInteger = 0
        Public Silk_Bonus As UInteger = 0
        Public Silk_Points As UInteger = 0
    End Class
End Namespace
