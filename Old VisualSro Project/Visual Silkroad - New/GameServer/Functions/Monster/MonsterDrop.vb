Namespace GameServer.Functions
    Module MonsterDrop

        Public Sub DropMonsterItems(ByVal MobUniqueId As UInteger)
            If MobList.ContainsKey(MobUniqueId) = False Then
                Exit Sub
            End If

            'Gold + Normal items (arrow, Pots, Vigor) + Equip (Normal, SOX) + Alchemy

            Dim RefMonster As Object_ = GetObjectById(MobList(MobUniqueId).Pk2ID)
            Dim RefLevel As cGoldData = GetGoldData(RefMonster.Level)
            Dim tmp_item As New cInvItem
            tmp_item.OwnerCharID = MobUniqueId

            'GOLD
            Dim Difference As Long = RefLevel.MaxGold - RefLevel.MinGold
            Dim Gold As Long = (RefLevel.MinGold + (Rand.NextDouble() * Difference)) * Settings.Server_GoldRate * GetMobExpMultiplier(MobList(MobUniqueId).Mob_Type)
            If Gold > UInt32.MaxValue Then
                Gold = UInt32.MaxValue
            End If

            tmp_item.Pk2Id = 1
            tmp_item.Amount = Gold
            DropItem(tmp_item, GetRandomPosition(MobList(MobUniqueId).Position, 3))



        End Sub


    End Module
End Namespace