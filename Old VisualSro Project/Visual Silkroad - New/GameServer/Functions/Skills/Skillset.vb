Namespace Functions
    Module Skillset
        Public Sub CreateSkillSet(ByVal Name As String)
            Dim tmp As New cSkillSet
            tmp.SetID = Id_Gen.GetSkillsetId
            tmp.Name = Name
            GameDB.SkillSets.Add(tmp.SetID, tmp)
        End Sub

        Public Sub Skillset_AddSkill(ByVal name As String, ByVal skill As UInt32)
            For i = 0 To GameDB.SkillSets.Keys.Count - 1
                Dim key As UInt32 = GameDB.SkillSets.Keys(i)
                If GameDB.SkillSets(key).Name = name Then
                    GameDB.SkillSets(key).Skills.Add(skill)
                    GameDB.SaveSkillsetNewSkill(GameDB.SkillSets(key).SetID, skill)
                    Exit For
                End If
            Next
        End Sub

        Public Sub Skillset_AddSkill(ByVal setID As UInt32, ByVal skill As UInt32)
            For i = 0 To GameDB.SkillSets.Keys.Count - 1
                Dim key As UInt32 = GameDB.SkillSets.Keys(i)
                If GameDB.SkillSets(key).SetID = setID Then
                    GameDB.SkillSets(key).Skills.Add(skill)
                    GameDB.SaveSkillsetNewSkill(GameDB.SkillSets(key).SetID, skill)
                    Exit For
                End If
            Next
        End Sub

        Public Sub Skillset_Clear(ByVal name As String)
            For i = 0 To GameDB.SkillSets.Keys.Count - 1
                Dim key As UInt32 = GameDB.SkillSets.Keys(i)
                If GameDB.SkillSets(key).Name = name Then
                    GameDB.SkillSets(key).Skills.Clear()
                    GameDB.SaveSkillsetClear(GameDB.SkillSets(key).SetID)
                    Exit For
                End If
            Next
        End Sub

        Public Sub Skillset_Clear(ByVal setID As UInt32)
            For i = 0 To GameDB.SkillSets.Keys.Count - 1
                Dim key As UInt32 = GameDB.SkillSets.Keys(i)
                If GameDB.SkillSets(key).SetID = setID Then
                    GameDB.SkillSets(key).Skills.Clear()
                    GameDB.SaveSkillsetClear(setID)
                    Exit For
                End If
            Next
        End Sub


        Public Sub Skillset_Load(ByVal name As String, ByVal Index_ As Integer)
            Dim skillset As cSkillSet
            For i = 0 To GameDB.SkillSets.Keys.Count - 1
                Dim key As UInt32 = GameDB.SkillSets.Keys(i)
                If GameDB.SkillSets(key).Name = name Then
                    skillset = GameDB.SkillSets(key)
                    Exit For
                End If
            Next

            If skillset Is Nothing Then
                Exit Sub
            End If

            For i = 0 To GameDB.Skills.Count - 1
                If GameDB.Skills(i) IsNot Nothing AndAlso GameDB.Skills(i).OwnerID = PlayerData(Index_).CharacterId Then

                End If
            Next
        End Sub
    End Module
End Namespace
