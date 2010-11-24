Namespace GameServer.Mod
    Module modDamage
        Public Sub SendDamageInfo(ByVal MobIndex As Integer)
            Dim ref_ As Object_ = GetObjectById(Functions.MobList(MobIndex).Pk2ID)

            'Sort
            Dim DmgList As List(Of cDamageDone) = Functions.MobList(MobIndex).DamageFromPlayer
            DmgList.Sort(New Comparison(Of cDamageDone)( _
            Function(o1 As cDamageDone, o2 As cDamageDone) _
            o1.Damage.CompareTo(o2.Damage)))

            'Build Info
            Dim sDmg As String = String.Format("== Damage Statistic for Mob: {0} ==   ", ref_.OtherName)
            For i = 0 To DmgList.Count - 1
                sDmg += String.Format(ControlChars.NewLine & "Rank: {0}, Name: {1}, Damage {2}", i + 1, Functions.PlayerData(DmgList(i).PlayerIndex).CharacterName, DmgList(i).Damage)
            Next

            'Send it
            For i = 0 To DmgList.Count - 1
                Functions.SendPm(DmgList(i).PlayerIndex, sDmg, "[DAMAGE_MOD]")
            Next


        End Sub


    End Module
End Namespace
