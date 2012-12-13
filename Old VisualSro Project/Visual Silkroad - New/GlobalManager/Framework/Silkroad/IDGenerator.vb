Namespace Id_Gen
    Module IDGenerator

        '=================Session Id
        Private m_sessionIdCounter As UInteger = 1

        Public Function GetSessionId() As UInteger
            Dim toreturn As UInteger = m_sessionIdCounter
            If m_sessionIdCounter < UInteger.MaxValue Then
                m_sessionIdCounter += 1
            ElseIf m_sessionIdCounter = UInteger.MaxValue Then
                m_sessionIdCounter = 0
                Log.WriteSystemLog("Reached SessionId Max")
            End If

            Return toreturn
        End Function

        '=================Account Id's
        Public Function GetNewAccountId() As UInteger
            For newID As UInteger = 1 To UInteger.MaxValue
                Dim free As Boolean = True
                For c = 0 To GlobalDB.Users.Count - 1
                    If GlobalDB.Users(c).AccountID = newID Then
                        free = False
                    End If
                Next

                If free = True Then
                    Return newID
                End If
            Next

            Throw New Exception("Cannot find a new Acc ID!")
        End Function

    End Module
End Namespace