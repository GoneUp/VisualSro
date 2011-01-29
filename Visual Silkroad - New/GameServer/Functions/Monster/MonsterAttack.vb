Namespace GameServer.Functions
    Module MonsterAttack
        Public Sub MonsterAttack(ByVal MyUniqueID As UInteger, ByVal AttackedID As UInteger)
            Dim MobIndex As Integer = 0

            For i = 0 To MobList.Count - 1
                If MobList(i).UniqueID = MyUniqueID Then
                    MobIndex = i
                    Exit For
                End If
            Next

            'Find others Index
            For i = 0 To PlayerData.Count - 1
                If PlayerData(i).UniqueId = AttackedID Then
                    MonsterAttackPlayer(MobIndex, i)
                    Exit Sub
                End If
            Next



        End Sub


        Public Sub MonsterAttackPlayer(ByVal MobIndex As Integer, ByVal Index_ As Integer)
            MobList(MobIndex).AttackingId = PlayerData(Index_).UniqueId
            MobList(MobIndex).UsingSkillId = 0


        End Sub

        Public Sub MonsterAttackHorse(ByVal MobIndex As Integer, ByVal Index_ As Integer)


        End Sub

        Private Function Monster_GetNextSkill(ByVal SkillID As UInteger)
            Dim Skill As Skill_ = GetSkillById(SkillID)



        End Function
    End Module
End Namespace
