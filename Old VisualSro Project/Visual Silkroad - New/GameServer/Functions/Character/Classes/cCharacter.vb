Namespace Functions
    Friend Class cCharacter
        Inherits cGameObject

        Public AccountID As UInteger = 0
        Public CharacterName As String = ""
        Public CharacterId As UInteger = 0
        Public HP As UInteger = 0
        Public MP As UInteger = 0
        Public CHP As UInteger = 0
        Public CMP As UInteger = 0
        Public Volume As Byte = 0
        Public Level As Byte = 0
        Public Experience As ULong = 0
        Public Gold As ULong = 0
        Public SkillPoints As UInteger = 0
        Public SkillPointBar As ULong = 0
        Public Attributes As UShort = 0
        Public BerserkBar As Byte = 0
        Public Berserk As Boolean = False

        Public MinPhy As UInteger = 0
        Public MaxPhy As UInteger = 0

        Public MinMag As UInteger = 0
        Public MaxMag As UInteger = 0

        Public PhyDef As UShort = 0
        Public MagDef As UShort = 0
        Public PhyAbs As UShort = 0
        Public MagAbs As UShort = 0
        Public AccessoryAbs As UShort = 0

        Public Hit As UShort = 0
        Public Parry As UShort = 0

        Public Strength As UShort = 0
        Public Intelligence As UShort = 0

        'Job
        Public AliasName As String = ""
        Public JobType As Byte = 0
        Public JobLevel As Byte = 1
        Public JobExperience As ULong = 0

        'Flags
        Public GM As Boolean = False
        Public PVP As Byte = 0
        Public MaxInvSlots As Byte = 0
        Public MaxAvatarSlots As Byte = 5
        Public Angle As UInt16 = 0
        Public Deleted As Boolean = False
        Public DeletionTime As DateTime
        Public HelperIcon As Byte = 0
        Public ActionFlag As Byte = 0        '0x00 = standing, 0x02 = walking, 0x03 = running, 0x04 = sitting
        Public Busy As Boolean = False
        Public Alive As Boolean = True
        Public Skilling As Boolean = False

        Public UsedItem As UseItemTypes
        Public UsedItemParameter As Integer = 0

        Public PositionRecall As New Position
        Public PositionReturn As New Position
        Public PositionDead As New Position

        Public SpawnedPlayers As New List(Of UInt32)
        Public SpawnedMonsters As New List(Of UInt32)
        Public SpawnedItems As New List(Of UInt32)
        Public SpawnedNPCs As New List(Of UInt32)

        Public Ingame As Boolean = False
        Public Invisible As Boolean = False  'For Gms
        Public Invincible As Boolean = False

        Public InGuild As Boolean = False
        Public GuildID As UInteger = 0

        Public InExchange As Boolean = False
        Public InExchangeWith As UInteger = 0
        Public ExchangeID As UInteger = 0

        Public InStall As Boolean = False
        Public StallID As UInteger = 0
        Public StallOwner As Boolean = False

        Public InParty As Boolean = False
        Public PartyID As UInteger = 0

        Public PickUpId As UInteger = 0
        Public Attacking As Boolean = False
        Public AttackedId As UInt32 = 0
        Public UsingSkillId As UInt32 = 0
        Public AttackType As Functions.AttackTypes
        Public SkillOverId As UInt32 = 0
        Public CastingId As UInt32 = 0
        Public LastSelected As UInt32 = 0

        Public PotHp As UInt16 = 0
        Public PotMp As UInt16 = 0
        Public PotAbormal As UInt16 = 0
        Public PotDelay As UInt16 = 0

        Public Buffs As New Dictionary(Of UInteger, cBuff) 'Key = SkillOverId

        Public TeleportType As TeleportTypes
        Public ExitType As ExitTypes
        Public BuybackList As New List(Of cItem)
        Public TmpSilkAddValue As Int64

        Public ReadOnly Property Position() As Position
            Get
                Return PosTracker.GetCurPos
            End Get
        End Property

        Public WriteOnly Property SetPosition() As Position
            Set(ByVal value As Position)
                PosTracker.LastPos = value
            End Set
        End Property

        Public Property WalkSpeed As Single
            Get
                Return PosTracker.WalkSpeed
            End Get
            Set(ByVal value As Single)
                PosTracker.WalkSpeed = value
            End Set
        End Property

        Public Property RunSpeed As Single
            Get
                Return PosTracker.RunSpeed
            End Get
            Set(ByVal value As Single)
                PosTracker.RunSpeed = value
            End Set
        End Property

        Public Property BerserkSpeed As Single
            Get
                Return PosTracker.BerserkSpeed
            End Get
            Set(ByVal value As Single)
                PosTracker.BerserkSpeed = value
            End Set
        End Property

        Sub SetCharGroundStats()
            'HP = GetLevelData(Level).MobEXP + (Me.Strength - 20) * 10
            'MP = GetLevelData(Level).MobEXP + (Me.Intelligence - 20) * 10
            HP = (Math.Pow(1.02, Me.Level - 1) * Me.Strength * 10)
            MP = (Math.Pow(1.02, Me.Level - 1) * Me.Intelligence * 10)

            Hit = Math.Round(Me.Level + 1000)
            Parry = Math.Round(Me.Level + 10)

            '=================Really unsure of these Formulas=============
            WalkSpeed = 15
            RunSpeed = Level + 49
            BerserkSpeed = RunSpeed * 2

            'Set default.
            MinPhy = 0
            'GameServer.Functions.GetMinPhy(Me.Strength)
            MaxPhy = 0
            'GameServer.Functions.GetMaxPhy(Me.Strength, Level)
            MinMag = 0
            'GameServer.Functions.GetMinMag(Me.Intelligence, Level)
            MaxMag = 0
            'GameServer.Functions.GetMaxMag(Me.Intelligence, Level)
            PhyDef = 0
            'GameServer.Functions.GetPhyDef(Me.Strength, Level)
            MagDef = 0
            'GameServer.Functions.GetMagDef(Me.Intelligence)
            PhyAbs = 0
            MagAbs = 0
        End Sub

        Sub AddItemsToStats(ByVal Index_ As Integer)
            For i = 0 To 12
                Dim _invItem As cInventoryItem = Inventorys(Index_).UserItems(i)
                If _invItem.ItemID <> 0 Then
                    Dim _item As cItem = GameDB.Items(_invItem.ItemID)


                    Dim _refitem As cRefItem = GetItemByID(_item.ObjectID)

                    If _refitem.CLASS_A = 1 Then

                        Select Case _invItem.Slot

                            Case 0, 1, 2, 3, 4, 5 'Equipment
                                Dim _whitestats As cWhitestats = New cWhitestats(cWhitestats.Type.Equipment, _item.Variance)
                                Dim tmpDifference As Single

                                'PhyDef
                                'Formel:PerItem PhyDef = STR * (PhyRef / 100) + ItemPhyDef
                                tmpDifference = (_refitem.MAX_PHYS_REINFORCE / 10) - (_refitem.MIN_PHYS_REINFORCE / 10)
                                Dim PhyRef As Single = (_refitem.MIN_PHYS_REINFORCE / 10) +
                                                       (_whitestats.PerPhyRef * (tmpDifference / 100))

                                tmpDifference = _refitem.MAX_PHYSDEF - _refitem.MIN_PHYSDEF
                                Dim ItemPhyDef As Single = _refitem.MIN_PHYSDEF + (_whitestats.PerPhyDef * (tmpDifference / 100)) +
                                                           (_item.Plus * _refitem.PHYSDEF_INC)
                                PhyDef += Math.Round(Strength * (PhyRef / 100) + ItemPhyDef, 0, MidpointRounding.ToEven)

                                'MagDef
                                'Formel:PerItem PhyDef = STR * (MagRef / 100) + ItemMagDef
                                tmpDifference = (_refitem.MAX_MAG_REINFORCE / 19) - (_refitem.MIN_MAG_REINFORCE / 10)
                                Dim MagRef As Single = (_refitem.MIN_MAG_REINFORCE / 10) +
                                                       (_whitestats.PerMagRef * (tmpDifference / 100))

                                tmpDifference = _refitem.MAX_MAGDEF - _refitem.MIN_MAGDEF
                                Dim ItemMagDef As Single = _refitem.MIN_MAGDEF + (_whitestats.PerMagDef * (tmpDifference / 100)) +
                                                           (_item.Plus * _refitem.MAGDEF_INC)
                                MagDef += Math.Round(Intelligence * (MagRef / 100) + ItemMagDef, 0, MidpointRounding.ToEven)

                                'Parry Rate
                                tmpDifference = _refitem.MAX_PARRY - _refitem.MIN_PARRY
                                Parry += Math.Round(_refitem.MIN_PARRY + (_whitestats.PerParryRate * (tmpDifference / 100)))
                            Case 6 'Weapon
                                '################################################################################
                                '=============Unsure OLD
                                'MinPhy += _refitem.MIN_HPHYATK + ((Me.Strength * _refitem.MIN_LPHYS_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)
                                'MaxPhy += _refitem.MAX_HPHYATK + ((Me.Strength * _refitem.MAX_LPHYS_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)

                                'MinMag += _refitem.MIN_LMAGATK + ((Me.Intelligence * _refitem.MIN_LMAG_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)
                                'MaxMag += _refitem.MAX_LMAGATK + ((Me.Intelligence * _refitem.MAX_LMAG_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)
                                '################################################################################
                                Dim _whitestats As cWhitestats = New cWhitestats(cWhitestats.Type.Weapon, _item.Variance)
                                Dim tmpDifference As Single


                                'PhyATK
                                'Formel:PhyAtk = STR * (PhyRef / 100) + PhyDmg

                                'PhyRefMin
                                tmpDifference = (_refitem.MAX_FROM_PHYS_REINFORCE / 10) -
                                                (_refitem.MIN_FROM_PHYS_REINFORCE / 10)
                                Dim PhyRefMin As Single = (_refitem.MIN_FROM_PHYS_REINFORCE / 10) +
                                                          (_whitestats.PerPhyRef * (tmpDifference / 100))

                                'PhyRefMax
                                tmpDifference = (_refitem.MAX_TO_PHYS_REINFORCE / 10) -
                                                (_refitem.MIN_TO_PHYS_REINFORCE / 10)
                                Dim PhyRefMax As Single = (_refitem.MIN_TO_PHYS_REINFORCE / 10) +
                                                          (_whitestats.PerPhyRef * (tmpDifference / 100))

                                'MinPhyDmg
                                tmpDifference = _refitem.MAX_FROM_PHYATK - _refitem.MIN_FROM_PHYATK
                                Dim MinPhyDmg As Single = _refitem.MIN_FROM_PHYATK +
                                                          (_whitestats.PerPhyAtk * (tmpDifference / 100)) +
                                                          (_item.Plus * _refitem.PHYSDEF_INC)

                                'MaxPhyDmg
                                tmpDifference = _refitem.MAX_TO_PHYATK - _refitem.MIN_TO_PHYATK
                                Dim MaxPhyDmg As Single = _refitem.MIN_TO_PHYATK + (_whitestats.PerPhyAtk * (tmpDifference / 100)) +
                                                          (_item.Plus * _refitem.PHYSDEF_INC)

                                MinPhy = Math.Round(Strength * (PhyRefMin / 100) + MinPhyDmg, 0, MidpointRounding.ToEven)
                                MaxPhy = Math.Round(Strength * (PhyRefMax / 100) + MaxPhyDmg, 0, MidpointRounding.ToEven)
                                '--------------------------------------------------------------------------------
                                'MagATK
                                'Formel:MagAtk = INT * (MagRef / 100) + MagDmg

                                'MagRefMin
                                tmpDifference = (_refitem.MAX_FROM_MAG_REINFORCE / 10) -
                                                (_refitem.MIN_FROM_MAG_REINFORCE / 10)
                                Dim MagRefMin As Single = (_refitem.MIN_FROM_MAG_REINFORCE / 10) +
                                                          (_whitestats.PerMagRef * (tmpDifference / 100))

                                'MagRefMax
                                tmpDifference = (_refitem.MAX_TO_MAG_REINFORCE / 10) - (_refitem.MIN_TO_MAG_REINFORCE / 10)
                                Dim MagRefMax As Single = (_refitem.MIN_TO_MAG_REINFORCE / 10) +
                                                          (_whitestats.PerMagRef * (tmpDifference / 100))

                                'MinMagDmg
                                tmpDifference = _refitem.MAX_FROM_MAGATK - _refitem.MIN_FROM_MAGATK
                                Dim MinMagDmg As Single = _refitem.MIN_FROM_MAGATK +
                                                          (_whitestats.PerMagAtk * (tmpDifference / 100)) +
                                                          (_item.Plus * _refitem.MAGATK_INC)

                                'MaxMagDmg
                                tmpDifference = _refitem.MAX_TO_MAGATK - _refitem.MIN_TO_MAGATK
                                Dim MaxMagDmg As Single = _refitem.MIN_TO_MAGATK + (_whitestats.PerMagAtk * (tmpDifference / 100)) +
                                                          (_item.Plus * _refitem.MAGATK_INC)

                                MinMag = Math.Round(Intelligence * (MagRefMin / 100) + MinMagDmg, 0, MidpointRounding.ToEven)
                                MaxMag = Math.Round(Intelligence * (MagRefMax / 100) + MaxMagDmg, 0, MidpointRounding.ToEven)

                                'Parry Rate
                                tmpDifference = _refitem.MAX_ATTACK_RATING - _refitem.MIN_ATTACK_RATING
                                Hit += Math.Round(_refitem.MIN_ATTACK_RATING + (_whitestats.PerAttackRate * (tmpDifference / 100)))
                            Case 7 'Shield

                                Dim _whitestats As cWhitestats = New cWhitestats(cWhitestats.Type.Shield, _item.Variance)
                                Dim tmpDifference As Single

                                'PhyDef
                                'Formel:PerItem PhyDef = STR * (PhyRef / 100) + ItemPhyDef
                                tmpDifference = (_refitem.MAX_PHYS_REINFORCE / 10) - (_refitem.MIN_PHYS_REINFORCE / 10)
                                Dim PhyRef As Single = (_refitem.MIN_PHYS_REINFORCE / 10) +
                                                       (_whitestats.PerPhyRef * (tmpDifference / 100))

                                tmpDifference = _refitem.MAX_PHYSDEF - _refitem.MIN_PHYSDEF
                                Dim ItemPhyDef As Single = _refitem.MIN_PHYSDEF + (_whitestats.PerPhyDef * (tmpDifference / 100)) +
                                                           (_item.Plus * _refitem.PHYSDEF_INC)
                                PhyDef += Math.Round(Strength * (PhyRef / 100) + ItemPhyDef, 0, MidpointRounding.ToEven)

                                'MagDef
                                'Formel:PerItem PhyDef = STR * (MagRef / 100) + ItemMagDef
                                tmpDifference = (_refitem.MAX_MAG_REINFORCE / 10) - (_refitem.MIN_MAG_REINFORCE / 10)
                                Dim MagRef As Single = (_refitem.MIN_MAG_REINFORCE / 10) +
                                                       (_whitestats.PerMagRef * (tmpDifference / 100))

                                tmpDifference = _refitem.MAX_MAGDEF - _refitem.MIN_MAGDEF
                                Dim ItemMagDef As Single = _refitem.MIN_MAGDEF + (_whitestats.PerMagDef * (tmpDifference / 100)) +
                                                           (_item.Plus * _refitem.MAGDEF_INC)
                                MagDef += Math.Round(Intelligence * (MagRef / 100) + ItemMagDef, 0, MidpointRounding.ToEven)

                            Case 8 'Jobsuit/PVP Cape

                            Case 9, 10, 11, 12 'Accessory

                                Dim _whitestats As cWhitestats = New cWhitestats(cWhitestats.Type.Shield, _item.Variance)
                                Dim tmpDifference As Single

                                tmpDifference = _refitem.MAX_PHYS_ABSORB - _refitem.MIN_PHYS_ABSORB
                                Dim tmpPhyAbs As Single = _refitem.MIN_PHYS_ABSORB +
                                                          (_whitestats.PerPhyAbs * (tmpDifference / 100)) +
                                                          (_item.Plus * _refitem.PHYS_ABSORB_INC)
                                PhyAbs += tmpPhyAbs
                                'Math.Round(tmpPhyAbs, 0, MidpointRounding.ToEven)

                                tmpDifference = _refitem.MAX_MAG_ABSORB - _refitem.MIN_MAG_ABSORB
                                Dim tmpMagAbs As Single = _refitem.MIN_MAG_ABSORB +
                                                          (_whitestats.PerMagAbs * (tmpDifference / 100)) +
                                                          (_item.Plus * _refitem.MAG_ABSORB_INC)
                                MagAbs += tmpMagAbs
                                'Math.Round(tmpMagAbs, 0, MidpointRounding.ToEven)

                        End Select
                    End If
                Else
                    'Slot is empty.

                End If
            Next

            'Wenn slot leer ist klappt diese formel nicht mehr darum benutz ich vorerst die alten formeln als fix
            If MinPhy = 0 Then
                MinPhy = GetMinPhy(Me.Strength)
            End If
            If MaxPhy = 0 Then
                MaxPhy = GetMaxPhy(Me.Strength, Level)
            End If
            If MinMag = 0 Then
                MinMag = GetMinMag(Me.Intelligence, Level)
            End If
            If MaxMag = 0 Then
                MaxMag = GetMaxMag(Me.Intelligence, Level)
            End If
            If PhyDef = 0 Then
                PhyDef = GetPhyDef(Me.Strength, Level)
            End If
            If MagDef = 0 Then
                MagDef = GetMagDef(Me.Intelligence)
            End If
        End Sub

        Sub AddBuffsToStats()
            Dim tmplist As Array = Buffs.Keys.ToArray
            For Each key In tmplist
                If Buffs.ContainsKey(key) Then
                    Select Case Buffs(key).Type
                        Case BuffType.ItemBuff
                            Dim ref As cRefItem = GetItemByID(Buffs(key).ItemID)


                        Case BuffType.SkillBuff
                            Dim ref As RefSkill = GetSkill(Buffs(key).SkillID)

                            For Each effect In ref.EffectList
                                Select Case effect.EffectId
                                    Case "dura"
                                    Case "heal"
                                        If CHP + effect.EffectParams(0) > HP Then
                                            CHP = HP
                                        Else
                                            CHP += effect.EffectParams(0)
                                        End If

                                    Case "hste"
                                        PosTracker.WalkSpeed *= (effect.EffectParams(0) / 100)
                                        PosTracker.RunSpeed *= (effect.EffectParams(0) / 100)
                                        PosTracker.BerserkSpeed *= (effect.EffectParams(0) / 100)
                                        

                                    Case Else
                                        Log.WriteSystemLog("AddBuffsToStats:: Unkown Effect: " & effect.EffectId)
                                End Select
                            Next

                          
                    End Select
                End If
            Next
        End Sub
    End Class
End Namespace