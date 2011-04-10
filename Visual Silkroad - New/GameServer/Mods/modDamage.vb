Namespace GameServer.Mod
    Module modDamage
        Public Sub SendDamageInfo(ByVal MobIndex As Integer)
            Dim ref_ As Object_ = GetObjectById(Functions.MobList(MobIndex).Pk2ID)

            'Sort
            Dim DmgList As List(Of cDamageDone) = Functions.MobList(MobIndex).DamageFromPlayer
            Dim sort = From elem In DmgList Order By elem.Damage Descending Select elem Take 9

            'Build Info
            Dim sDmg As String = String.Format("== Damage Statistic for Mob: {0} ==   ", ref_.RealName)
            Dim rank As Integer = 1
            For i = 0 To sort.Count - 1
                sDmg += String.Format("Rank: {0}, Name: {1}, Damage: {2} ; ", rank, Functions.PlayerData(DmgList(i).PlayerIndex).CharacterName, DmgList(i).Damage)
                rank += 1
            Next

            'Send it
            For i = 0 To sort.Count - 1
                Functions.SendPm(DmgList(i).PlayerIndex, sDmg, "[DAMAGE_MOD]")
            Next


        End Sub


    End Module
End Namespace
