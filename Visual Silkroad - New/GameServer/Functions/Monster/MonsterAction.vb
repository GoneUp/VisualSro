Namespace GameServer.Functions
    Module MonsterAction
        Public Sub KillMob(ByVal MobIndex As Integer)
            MobList(MobIndex).HP_Cur = 0
            MobList(MobIndex).Death = True

            Dim tmp_ As Integer = GetPlayerWithMostDamage(MobIndex)
            If tmp_ > 0 Then
                If MobList(MobIndex).Mob_Type = 3 Then
                    SendUniqueKill(MobList(MobIndex).Pk2ID, PlayerData(tmp_).CharacterName)
                End If

                If ModGeneral And ModDamage Then
                    [Mod].SendDamageInfo(MobIndex)
                End If
            End If
        End Sub

        Private Function GetPlayerWithMostDamage(ByVal MobIndex As Integer)
            Dim MostIndex As Integer = -1
            Dim MostDamage As UInteger
            For i = 0 To MobList(MobIndex).DamageFromPlayer.Count - 1
                If MobList(MobIndex).DamageFromPlayer(i).Damage > MostDamage Then
                    MostDamage = MobList(MobIndex).DamageFromPlayer(i).Damage
                    MostIndex = MobList(MobIndex).DamageFromPlayer(i).PlayerIndex
                End If
            Next
            Return MostIndex
        End Function
    End Module
End Namespace
