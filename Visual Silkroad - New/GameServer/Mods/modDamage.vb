Namespace GameServer.Mod
    Module modDamage
        Public Sub SendDamageInfo(ByVal MobIndex As Integer)
            Dim ref_ As Object_ = GetObjectById(Functions.MobList(MobIndex).Pk2ID)

            'Sort
            Dim DmgList As List(Of cDamageDone) = Functions.MobList(MobIndex).DamageFromPlayer
            Dim sort = From elem In DmgList Order By elem.Damage Select elem

            'Build Info
            Dim sDmg As String = String.Format("== Damage Statistic for Mob: {0} ==   ", ref_.OtherName)
            Dim rank As Integer = 1
            For i = sort.Count - 1 To 0 Step -1
                sDmg += String.Format(ControlChars.NewLine & "Rank: {0}, Name: {1}, Damage: {2} == ", rank, Functions.PlayerData(DmgList(i).PlayerIndex).CharacterName, DmgList(i).Damage)
                rank += 1
            Next

            'Send it
            For i = sort.Count - 1 To 0 Step -1
                Functions.SendPm(DmgList(i).PlayerIndex, sDmg, "[DAMAGE_MOD]")
            Next


        End Sub


    End Module
End Namespace
