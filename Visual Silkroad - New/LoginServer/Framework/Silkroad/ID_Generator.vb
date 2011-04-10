Namespace LoginServer.Id_Gen
    Module ID_Generator

        '=================Account Id's
        Public Function GetNewAccountId() As UInteger
            For NewID As UInteger = 1 To UInteger.MaxValue
                Dim free As Boolean = True
                For c = 0 To LoginDb.Users.Count - 1
                    If LoginDb.Users(c).AccountId = NewID Then
                        free = False
                    End If
                Next

                If free = True Then
                    Return NewID
                End If
            Next

            Throw New Exception("Cannot find a new Acc ID!")
        End Function


    End Module
End Namespace