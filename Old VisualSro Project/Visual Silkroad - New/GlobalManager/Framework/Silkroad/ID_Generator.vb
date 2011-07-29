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


    End Module
End Namespace