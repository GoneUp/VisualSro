Namespace Functions
    Module MonsterDrop
        Public Sub DropMonsterItems(ByVal MobUniqueId As UInteger)
            If MobList.ContainsKey(MobUniqueId) = False Then
                Exit Sub
            End If

            'Gold + Normal items (arrow, Pots, Vigor) + Equip (Normal, SOX) + Alchemy

            Dim RefMonster As SilkroadObject = GetObject(MobList(MobUniqueId).Pk2ID)
            Dim RefLevel As GoldData = GetGoldData(RefMonster.Level)
            Dim invItem As New cInventoryItem
            invItem.OwnerID = MobUniqueId


            'GOLD
            Dim item As cItem
            Dim Difference As Long = RefLevel.MaxGold - RefLevel.MinGold
            Dim Gold As Long = (RefLevel.MinGold + (Rand.NextDouble() * Difference)) * Settings.ServerGoldRate *
                               GetMobExpMultiplier(MobList(MobUniqueId).Mob_Type)
            If Gold > UInt32.MaxValue Then
                Gold = UInt32.MaxValue
            End If

            item.ObjectID = 1
            item.Data = Gold

            DropItem(invItem, item, GetRandomPosition(MobList(MobUniqueId).Position, 3))
        End Sub
    End Module
End Namespace