Namespace GameServer.Id_Gen
    Module ID_Generator

        '====================Unique ID's
        Public UniqueIdCounter As UInteger = 1

        Public Function GetUnqiueId() As UInteger
            Dim toreturn As UInteger = UniqueIdCounter
            If UniqueIdCounter < UInteger.MaxValue Then
                UniqueIdCounter += 1
            ElseIf UniqueIdCounter = UInteger.MaxValue Then
                UniqueIdCounter = 0
            End If

            Return toreturn
        End Function

        '=================Charcater Id's
        Public Function GetNewCharId() As UInteger
            For NewID As UInteger = 1 To UInteger.MaxValue
                Dim free As Boolean = True
                For c = 0 To GameDB.Chars.Count - 1
                    If GameDB.Chars(c).CharacterId = NewID Then
                        free = False
                    End If
                Next

                If free = True Then
                    Return NewID
                End If
            Next

            Throw New Exception("Cannot find a new Char ID!")
        End Function

        '======================Skill Over ID's
        Public SkillOverIdCounter As UInteger = 10

        Public Function GetSkillOverId() As UInteger
            Dim toreturn As UInteger = SkillOverIdCounter
            If SkillOverIdCounter < UInteger.MaxValue Then
                SkillOverIdCounter += 1
            ElseIf SkillOverIdCounter = UInteger.MaxValue Then
                SkillOverIdCounter = 0
                Log.WriteSystemLog("Reached SkillOverId Max")
            End If

            Return toreturn
        End Function

    End Module
End Namespace