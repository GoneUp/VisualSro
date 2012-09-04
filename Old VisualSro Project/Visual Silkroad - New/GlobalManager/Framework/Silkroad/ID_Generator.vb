Namespace Id_Gen
    Module ID_Generator

        '=================Session Id
        Private SessionIdCounter As UInteger = 1

        Public Function GetSessionId() As UInteger
            Dim toreturn As UInteger = SessionIdCounter
            If SessionIdCounter < UInteger.MaxValue Then
                SessionIdCounter += 1
            ElseIf SessionIdCounter = UInteger.MaxValue Then
                SessionIdCounter = 0
                Log.WriteSystemLog("Reached SessionId Max")
            End If

            Return toreturn
        End Function

        '=================Account Id's
        Public Function GetNewAccountId() As UInteger
            For NewID As UInteger = 1 To UInteger.MaxValue
                Dim free As Boolean = True
                For c = 0 To GlobalDB.Users.Count - 1
                    If GlobalDB.Users(c).AccountId = NewID Then
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