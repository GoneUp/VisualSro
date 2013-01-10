Imports SRFramework

Namespace Functions
    Module CharacterStats
        Public Sub CharacterRecalculateStats(Index_ As Integer)
            SetCharGroundStats(Index_)
            AddItemsToStats(Index_)
            AddBuffsToStats(Index_)
        End Sub

        Private Sub SetCharGroundStats(Index_ As Integer)
            With PlayerData(Index_)
                'HP = Get.LevelData(.Level).MobEXP + (Me..Strength - 20) * 10
                'MP = Get.LevelData(.Level).MobEXP + (Me..Intelligence - 20) * 10
                .HP = (Math.Pow(1.02, .Level - 1) * .Strength * 10)
                .MP = (Math.Pow(1.02, .Level - 1) * .Intelligence * 10)

                .Hit = Math.Round(.Level + 1000)
                .Parry = Math.Round(.Level + 10)

                '=================Really unsure of these Formulas=============
                .PosTracker.WalkSpeed = 15
                .PosTracker.RunSpeed = .Level + 49
                .PosTracker.BerserkSpeed = .RunSpeed * 2

                'Set default.
                .MinPhy = 0
                .MaxPhy = 0
                .MinMag = 0
                .MaxMag = 0
                .PhyDef = 0
                .MagDef = 0
                .PhyAbs = 0
                .MagAbs = 0
            End With
        End Sub

        Private Sub AddItemsToStats(ByVal Index_ As Integer)
            With PlayerData(Index_)
                For i = 0 To 12
                    Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(i)
                    If invItem.ItemID <> 0 Then
                        Dim item As cItem = GameDB.Items(invItem.ItemID)
                        Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                        If refitem.CLASS_A = 1 Then

                            Select Case invItem.Slot

                                Case 0, 1, 2, 3, 4, 5 'Equipment
                                    Dim whitestats As cWhitestats = New cWhitestats(cWhitestats.Type.Equipment, item.Variance)
                                    Dim tmpDifference As Single

                                    '.PhyDef
                                    'Formel:PerItem .PhyDef = STR * (PhyRef / 100) + ItemPhyDef
                                    tmpDifference = (refitem.MAX_PHYS_REINFORCE / 10) - (refitem.MIN_PHYS_REINFORCE / 10)
                                    Dim phyRef As Single = (refitem.MIN_PHYS_REINFORCE / 10) +
                                                           (whitestats.PerPhyRef * (tmpDifference / 100))

                                    tmpDifference = refitem.MAX_PHYSDEF - refitem.MIN_PHYSDEF
                                    Dim itemPhyDef As Single = refitem.MIN_PHYSDEF + (whitestats.PerPhyDef * (tmpDifference / 100)) +
                                                               (item.Plus * refitem.PHYSDEF_INC)
                                    .PhyDef += Math.Round(.Strength * (phyRef / 100) + itemPhyDef, 0, MidpointRounding.ToEven)

                                    '.MagDef
                                    'Formel:PerItem .PhyDef = STR * (MagRef / 100) + Item.MagDef
                                    tmpDifference = (refitem.MAX_MAG_REINFORCE / 19) - (refitem.MIN_MAG_REINFORCE / 10)
                                    Dim magRef As Single = (refitem.MIN_MAG_REINFORCE / 10) +
                                                           (whitestats.PerMagRef * (tmpDifference / 100))

                                    tmpDifference = refitem.MAX_MAGDEF - refitem.MIN_MAGDEF
                                    Dim itemMagDef As Single = refitem.MIN_MAGDEF + (whitestats.PerMagDef * (tmpDifference / 100)) + (item.Plus * refitem.MAGDEF_INC)
                                    .MagDef += Math.Round(.Intelligence * (magRef / 100) + itemMagDef, 0, MidpointRounding.ToEven)

                                    'Parry Rate
                                    tmpDifference = refitem.MAX_PARRY - refitem.MIN_PARRY
                                    .Parry += Math.Round(refitem.MIN_PARRY + (whitestats.PerParryRate * (tmpDifference / 100)))
                                Case 6 'Weapon
                                    '################################################################################
                                    '=============Unsure OLD
                                    '.MinPhy += _refitem.MIN_HPHYATK + ((Me.Strength * _refitem.MIN_LPHYS_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMastery.Level(Index_) / 100)
                                    '.MaxPhy += _refitem.MAX_HPHYATK + ((Me.Strength * _refitem.MAX_LPHYS_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMastery.Level(Index_) / 100)

                                    '.MinMag += _refitem.MIN_LMAGATK + ((Me.Intelligence * _refitem.MIN_LMAG_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMastery.Level(Index_) / 100)
                                    '.MaxMag += _refitem.MAX_LMAGATK + ((Me.Intelligence * _refitem.MAX_LMAG_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMastery.Level(Index_) / 100)
                                    '################################################################################
                                    Dim whitestats As cWhitestats = New cWhitestats(cWhitestats.Type.Weapon, item.Variance)
                                    Dim tmpDifference As Single


                                    'PhyATK
                                    'Formel:PhyAtk = STR * (PhyRef / 100) + PhyDmg

                                    'PhyRefMin
                                    tmpDifference = (refitem.MAX_FROM_PHYS_REINFORCE / 10) -
                                                    (refitem.MIN_FROM_PHYS_REINFORCE / 10)
                                    Dim phyRefMin As Single = (refitem.MIN_FROM_PHYS_REINFORCE / 10) +
                                                              (whitestats.PerPhyRef * (tmpDifference / 100))

                                    'PhyRefMax
                                    tmpDifference = (refitem.MAX_TO_PHYS_REINFORCE / 10) -
                                                    (refitem.MIN_TO_PHYS_REINFORCE / 10)
                                    Dim phyRefMax As Single = (refitem.MIN_TO_PHYS_REINFORCE / 10) +
                                                              (whitestats.PerPhyRef * (tmpDifference / 100))

                                    'MinPhyDmg
                                    tmpDifference = refitem.MAX_FROM_PHYATK - refitem.MIN_FROM_PHYATK
                                    Dim minPhyDmg As Single = refitem.MIN_FROM_PHYATK +
                                                          (whitestats.PerPhyAtk * (tmpDifference / 100)) +
                                                          (item.Plus * refitem.PHYSDEF_INC)

                                    'MaxPhyDmg
                                    tmpDifference = refitem.MAX_TO_PHYATK - refitem.MIN_TO_PHYATK
                                    Dim maxPhyDmg As Single = refitem.MIN_TO_PHYATK + (whitestats.PerPhyAtk * (tmpDifference / 100)) +
                                                          (item.Plus * refitem.PHYSDEF_INC)

                                    .MinPhy = Math.Round(.Strength * (phyRefMin / 100) + minPhyDmg, 0, MidpointRounding.ToEven)
                                    .MaxPhy = Math.Round(.Strength * (phyRefMax / 100) + maxPhyDmg, 0, MidpointRounding.ToEven)
                                    '--------------------------------------------------------------------------------
                                    'MagATK
                                    'Formel:MagAtk = INT * (MagRef / 100) + MagDmg

                                    'MagRefMin
                                    tmpDifference = (refitem.MAX_FROM_MAG_REINFORCE / 10) -
                                                    (refitem.MIN_FROM_MAG_REINFORCE / 10)
                                    Dim magRefMin As Single = (refitem.MIN_FROM_MAG_REINFORCE / 10) +
                                                              (whitestats.PerMagRef * (tmpDifference / 100))

                                    'MagRefMax
                                    tmpDifference = (refitem.MAX_TO_MAG_REINFORCE / 10) - (refitem.MIN_TO_MAG_REINFORCE / 10)
                                    Dim magRefMax As Single = (refitem.MIN_TO_MAG_REINFORCE / 10) +
                                                              (whitestats.PerMagRef * (tmpDifference / 100))

                                    'MinMagDmg
                                    tmpDifference = refitem.MAX_FROM_MAGATK - refitem.MIN_FROM_MAGATK
                                    Dim minMagDmg As Single = refitem.MIN_FROM_MAGATK +
                                                              (whitestats.PerMagAtk * (tmpDifference / 100)) +
                                                              (item.Plus * refitem.MAGATK_INC)

                                    'MaxMagDmg
                                    tmpDifference = refitem.MAX_TO_MAGATK - refitem.MIN_TO_MAGATK
                                    Dim maxMagDmg As Single = refitem.MIN_TO_MAGATK + (whitestats.PerMagAtk * (tmpDifference / 100)) +
                                                              (item.Plus * refitem.MAGATK_INC)

                                    .MinMag = Math.Round(.Intelligence * (magRefMin / 100) + minMagDmg, 0, MidpointRounding.ToEven)
                                    .MaxMag = Math.Round(.Intelligence * (magRefMax / 100) + maxMagDmg, 0, MidpointRounding.ToEven)

                                    'Parry Rate
                                    tmpDifference = refitem.MAX_ATTACK_RATING - refitem.MIN_ATTACK_RATING
                                    .Hit += Math.Round(refitem.MIN_ATTACK_RATING + (whitestats.PerAttackRate * (tmpDifference / 100)))
                                Case 7 'Shield

                                    Dim whitestats As cWhitestats = New cWhitestats(cWhitestats.Type.Shield, item.Variance)
                                    Dim tmpDifference As Single

                                    '.PhyDef
                                    'Formel:PerItem .PhyDef = STR * (PhyRef / 100) + Item.PhyDef
                                    tmpDifference = (refitem.MAX_PHYS_REINFORCE / 10) - (refitem.MIN_PHYS_REINFORCE / 10)
                                    Dim phyRef As Single = (refitem.MIN_PHYS_REINFORCE / 10) +
                                                           (whitestats.PerPhyRef * (tmpDifference / 100))

                                    tmpDifference = refitem.MAX_PHYSDEF - refitem.MIN_PHYSDEF
                                    Dim itemPhyDef As Single = refitem.MIN_PHYSDEF + (whitestats.PerPhyDef * (tmpDifference / 100)) +
                                                               (item.Plus * refitem.PHYSDEF_INC)
                                    .PhyDef += Math.Round(.Strength * (phyRef / 100) + itemPhyDef, 0, MidpointRounding.ToEven)

                                    '.MagDef
                                    'Formel:PerItem .PhyDef = STR * (MagRef / 100) + Item.MagDef
                                    tmpDifference = (refitem.MAX_MAG_REINFORCE / 10) - (refitem.MIN_MAG_REINFORCE / 10)
                                    Dim magRef As Single = (refitem.MIN_MAG_REINFORCE / 10) +
                                                           (whitestats.PerMagRef * (tmpDifference / 100))

                                    tmpDifference = refitem.MAX_MAGDEF - refitem.MIN_MAGDEF
                                    Dim itemMagDef As Single = refitem.MIN_MAGDEF + (whitestats.PerMagDef * (tmpDifference / 100)) +
                                                               (item.Plus * refitem.MAGDEF_INC)
                                    .MagDef += Math.Round(.Intelligence * (magRef / 100) + itemMagDef, 0, MidpointRounding.ToEven)

                                Case 8 'Jobsuit/PVP Cape

                                Case 9, 10, 11, 12 'Accessory

                                    Dim whitestats As cWhitestats = New cWhitestats(cWhitestats.Type.Shield, item.Variance)
                                    Dim tmpDifference As Single

                                    tmpDifference = refitem.MAX_PHYS_ABSORB - refitem.MIN_PHYS_ABSORB
                                    Dim tmpPhyAbs As Single = refitem.MIN_PHYS_ABSORB +
                                                              (whitestats.PerPhyAbs * (tmpDifference / 100)) +
                                                              (item.Plus * refitem.PHYS_ABSORB_INC)
                                    .PhyAbs += tmpPhyAbs
                                    'Math.Round(tmpPhyAbs, 0, MidpointRounding.ToEven)

                                    tmpDifference = refitem.MAX_MAG_ABSORB - refitem.MIN_MAG_ABSORB
                                    Dim tmpMagAbs As Single = refitem.MIN_MAG_ABSORB +
                                                              (whitestats.PerMagAbs * (tmpDifference / 100)) +
                                                              (item.Plus * refitem.MAG_ABSORB_INC)
                                    .MagAbs += tmpMagAbs
                                    'Math.Round(tmpMagAbs, 0, MidpointRounding.ToEven)

                            End Select
                        End If
                    Else
                        'Slot is empty.

                    End If
                Next

                'Wenn slot leer ist klappt diese formel nicht mehr darum benutz ich vorerst die alten formeln als fix
                If .MinPhy = 0 Then
                    .MinPhy = GetMinPhy(.Strength)
                End If
                If .MaxPhy = 0 Then
                    .MaxPhy = GetMaxPhy(.Strength, .Level)
                End If
                If .MinMag = 0 Then
                    .MinMag = GetMinMag(.Intelligence, .Level)
                End If
                If .MaxMag = 0 Then
                    .MaxMag = GetMaxMag(.Intelligence, .Level)
                End If
                If .PhyDef = 0 Then
                    .PhyDef = GetPhyDef(.Strength, .Level)
                End If
                If .MagDef = 0 Then
                    .MagDef = GetMagDef(.Intelligence)
                End If
            End With
        End Sub

        Private Sub AddBuffsToStats(Index_ As Integer)
            With PlayerData(Index_)
                Dim tmplist As Array = .Buffs.Keys.ToArray
                For Each key In tmplist
                    If .Buffs.ContainsKey(key) Then
                        Select Case .Buffs(key).Type
                            Case BuffType.ItemBuff
                                Dim ref As cRefItem = GetItemByID(.Buffs(key).ItemID)


                            Case BuffType.SkillBuff
                                Dim ref As RefSkill = GetSkill(.Buffs(key).SkillID)

                                For Each effect In ref.EffectList
                                    Select Case effect.EffectId
                                        Case "dura"
                                        Case "heal"
                                            If .CHP + effect.EffectParams(0) > .HP Then
                                                .CHP = .HP
                                            Else
                                                .CHP += effect.EffectParams(0)
                                            End If

                                        Case "hste"
                                            .PosTracker.WalkSpeed *= (effect.EffectParams(0) / 100)
                                            .PosTracker.RunSpeed *= (effect.EffectParams(0) / 100)
                                            .PosTracker.BerserkSpeed *= (effect.EffectParams(0) / 100)


                                        Case Else
                                            Log.WriteSystemLog("AddBuffsToStats:: Unkown Effect: " & effect.EffectId)
                                    End Select
                                Next


                        End Select
                    End If
                Next
            End With
        End Sub
    End Module
End Namespace
