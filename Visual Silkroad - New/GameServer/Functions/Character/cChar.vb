Namespace GameServer
    Public Class [cChar]

        Public AccountID As UInteger
        Public CharacterName As String
        Public CharacterId As UInteger
        Public UniqueId As UInteger
        Public HP As UInteger
        Public MP As UInteger
        Public CHP As Integer
        Public CMP As Integer
        Public Model As UInteger
        Public Volume As Byte
        Public Level As Byte
        Public Experience As ULong
        Public Gold As ULong
        Public SkillPoints As UInteger
        Public SkillPointBar As ULong
        Public Attributes As UShort
        Public BerserkBar As Byte
        Public Berserk As Boolean
        Public WalkSpeed As Single
        Public RunSpeed As Single
        Public BerserkSpeed As Single

        Public MinPhy As UInteger
        Public MaxPhy As UInteger

        Public MinMag As UInteger
        Public MaxMag As UInteger

        Public PhyDef As UShort
        Public MagDef As UShort
        Public PhyAbs As UShort
        Public MagAbs As UShort
        Public AccessoryAbs As UShort

        Public Hit As UShort
        Public Parry As UShort

        Public Strength As UShort
        Public Intelligence As UShort

        'Flags
        Public GM As Boolean
        Public PVP As Byte
        Public MaxSlots As Byte
        Public Angle As UInt16
        Public Deleted As Boolean
        Public DeletionTime As DateTime
        Public HelperIcon As Byte
        Public ActionFlag As Byte '0x00 = standing, 0x02 = walking, 0x03 = running, 0x04 = sitting
        Public Busy As Boolean
        Public Alive As Boolean = True
        Public Skilling As Boolean = False

        Public UsedItem As UseItemTypes
        Public UsedItemParameter As Integer

        Public Position_Recall As New Position
        Public Position_Return As New Position
        Public Position_Dead As New Position
        Public Pos_Tracker As Functions.cPositionTracker

        Public SpawnedPlayers As New List(Of Integer)
        Public SpawnedMonsters As New List(Of Integer)
        Public SpawnedItems As New List(Of Integer)
        Public SpawnedNPCs As New List(Of Integer)

        Public Ingame As Boolean = False
        Public Invisible As Boolean = False 'For Gms
        Public Invincible As Boolean = False

        Public InGuild As Boolean = False
        Public GuildID As Long = -1

        Public InExchange As Boolean = False
        Public InExchangeWith As Integer = -1
        Public ExchangeID As Integer = -1

        Public InStall As Boolean
        Public StallID As UInteger = 0
        Public StallOwner As Boolean

        Public PickUpId As UInteger = 0
        Public Attacking As Boolean = False
        Public AttackedId As UInt32 = 0
        Public UsingSkillId As UInt32 = 0
        Public AttackType As AttackType_
        Public SkillOverId As UInt32 = 0
        Public CastingId As UInt32 = 0
        Public LastSelected As UInt32 = 0

        Public Pot_HP_Slot As Byte = 0
        Public Pot_HP_Value As Byte = 0
        Public Pot_MP_Slot As Byte = 0
        Public Pot_MP_Value As Byte = 0
        Public Pot_Abormal_Slot As Byte = 0
        Public Pot_Abormal_Value As Byte = 0
        Public Pot_Delay As Byte = 0

        Public Buffs As New Dictionary(Of UInteger, cBuff) 'Uint = SkillOverId

        Public TeleportType As TeleportType_

        Public Property Position() As Position
            Get
                Return Me.Pos_Tracker.GetCurPos
            End Get
            Set(ByVal value As Position)
                Me.Pos_Tracker.LastPos = value
            End Set
        End Property


        Sub SetCharGroundStats()
            'HP = GameServer.GetLevelDataByLevel(Level).Base + (Me.Strength - 20) * 10
            'MP = GameServer.GetLevelDataByLevel(Level).Base + (Me.Intelligence - 20) * 10
            HP = (Math.Pow(1.02, Me.Level - 1) * Me.Strength * 10)
            MP = (Math.Pow(1.02, Me.Level - 1) * Me.Intelligence * 10)

            Hit = Math.Round(Me.Level + 1000)
            Parry = Math.Round(Me.Level + 10)

            '=================Really unsure of these Formulas=============
            Me.WalkSpeed = 15
            Me.RunSpeed = Me.Level + 49
            Me.BerserkSpeed = Me.RunSpeed * 2
            Me.Pos_Tracker.WalkSpeed = Me.WalkSpeed
            Me.Pos_Tracker.RunSpeed = Me.RunSpeed
            Me.Pos_Tracker.ZerkSpeed = Me.BerserkSpeed


            'Set default.
            MinPhy = 0 'GameServer.Functions.GetMinPhy(Me.Strength)
            MaxPhy = 0 'GameServer.Functions.GetMaxPhy(Me.Strength, Level)
            MinMag = 0 'GameServer.Functions.GetMinMag(Me.Intelligence, Level)
            MaxMag = 0 'GameServer.Functions.GetMaxMag(Me.Intelligence, Level)
            PhyDef = 0 'GameServer.Functions.GetPhyDef(Me.Strength, Level)
            MagDef = 0 'GameServer.Functions.GetMagDef(Me.Intelligence)
            PhyAbs = 0
            MagAbs = 0
        End Sub

        Sub AddItemsToStats(ByVal Index_ As Integer)
            For i = 0 To 12
                Dim _item As cInvItem = GameServer.Functions.Inventorys(Index_).UserItems(i)
                If _item.Pk2Id <> 0 Then

                    Dim _refitem As cItem = GameServer.GetItemByID(_item.Pk2Id)

                    If _refitem.CLASS_A = 1 Then

                        Select Case _item.Slot

                            Case 0, 1, 2, 3, 4, 5 'Equipment
                                Dim tmpDifference As Single

                                'PhyDef
                                'Formel:PerItem PhyDef = STR * (PhyRef / 100) + ItemPhyDef
                                tmpDifference = (_refitem.MAX_PHYS_REINFORCE / 10) - (_refitem.MIN_PHYS_REINFORCE / 10)
                                Dim PhyRef As Single = (_refitem.MIN_PHYS_REINFORCE / 10) + (_item.PerPhyRef * (tmpDifference / 100))

                                tmpDifference = _refitem.MAX_PHYSDEF - _refitem.MIN_PHYSDEF
                                Dim ItemPhyDef As Single = _refitem.MIN_PHYSDEF + (_item.PerPhyDef * (tmpDifference / 100)) + (_item.Plus * _refitem.PHYSDEF_INC)
                                PhyDef += Math.Round(Strength * (PhyRef / 100) + ItemPhyDef, 0, MidpointRounding.ToEven)

                                'MagDef
                                'Formel:PerItem PhyDef = STR * (MagRef / 100) + ItemMagDef
                                tmpDifference = (_refitem.MAX_MAG_REINFORCE / 19) - (_refitem.MIN_MAG_REINFORCE / 10)
                                Dim MagRef As Single = (_refitem.MIN_MAG_REINFORCE / 10) + (_item.PerMagRef * (tmpDifference / 100))

                                tmpDifference = _refitem.MAX_MAGDEF - _refitem.MIN_MAGDEF
                                Dim ItemMagDef As Single = _refitem.MIN_MAGDEF + (_item.PerMagDef * (tmpDifference / 100)) + (_item.Plus * _refitem.MAGDEF_INC)
                                MagDef += Math.Round(Intelligence * (MagRef / 100) + ItemMagDef, 0, MidpointRounding.ToEven)

                                'Parry Rate
                                tmpDifference = _refitem.MAX_PARRY - _refitem.MIN_PARRY
                                Parry += Math.Round(_refitem.MIN_PARRY + (_item.PerParryRate * (tmpDifference / 100)))
                            Case 6 'Weapon
                                '################################################################################
                                '=============Unsure OLD
                                'MinPhy += _refitem.MIN_HPHYATK + ((Me.Strength * _refitem.MIN_LPHYS_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)
                                'MaxPhy += _refitem.MAX_HPHYATK + ((Me.Strength * _refitem.MAX_LPHYS_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)

                                'MinMag += _refitem.MIN_LMAGATK + ((Me.Intelligence * _refitem.MIN_LMAG_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)
                                'MaxMag += _refitem.MAX_LMAGATK + ((Me.Intelligence * _refitem.MAX_LMAG_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)
                                '################################################################################
                                Dim tmpDifference As Single

                                'PhyATK
                                'Formel:PhyAtk = STR * (PhyRef / 100) + PhyDmg

                                'PhyRefMin
                                tmpDifference = (_refitem.MAX_FROM_PHYS_REINFORCE / 10) - (_refitem.MIN_FROM_PHYS_REINFORCE / 10)
                                Dim PhyRefMin As Single = (_refitem.MIN_FROM_PHYS_REINFORCE / 10) + (_item.PerPhyRef * (tmpDifference / 100))

                                'PhyRefMax
                                tmpDifference = (_refitem.MAX_TO_PHYS_REINFORCE / 10) - (_refitem.MIN_TO_PHYS_REINFORCE / 10)
                                Dim PhyRefMax As Single = (_refitem.MIN_TO_PHYS_REINFORCE / 10) + (_item.PerPhyRef * (tmpDifference / 100))

                                'MinPhyDmg
                                tmpDifference = _refitem.MAX_FROM_PHYATK - _refitem.MIN_FROM_PHYATK
                                Dim MinPhyDmg As Single = _refitem.MIN_FROM_PHYATK + (_item.PerPhyAtk * (tmpDifference / 100)) + (_item.Plus * _refitem.PHYSDEF_INC)

                                'MaxPhyDmg
                                tmpDifference = _refitem.MAX_TO_PHYATK - _refitem.MIN_TO_PHYATK
                                Dim MaxPhyDmg As Single = _refitem.MIN_TO_PHYATK + (_item.PerPhyAtk * (tmpDifference / 100)) + (_item.Plus * _refitem.PHYSDEF_INC)

                                MinPhy = Math.Round(Strength * (PhyRefMin / 100) + MinPhyDmg, 0, MidpointRounding.ToEven)
                                MaxPhy = Math.Round(Strength * (PhyRefMax / 100) + MaxPhyDmg, 0, MidpointRounding.ToEven)
                                '--------------------------------------------------------------------------------
                                'MagATK
                                'Formel:MagAtk = INT * (MagRef / 100) + MagDmg

                                'MagRefMin
                                tmpDifference = (_refitem.MAX_FROM_MAG_REINFORCE / 10) - (_refitem.MIN_FROM_MAG_REINFORCE / 10)
                                Dim MagRefMin As Single = (_refitem.MIN_FROM_MAG_REINFORCE / 10) + (_item.PerMagRef * (tmpDifference / 100))

                                'MagRefMax
                                tmpDifference = (_refitem.MAX_TO_MAG_REINFORCE / 10) - (_refitem.MIN_TO_MAG_REINFORCE / 10)
                                Dim MagRefMax As Single = (_refitem.MIN_TO_MAG_REINFORCE / 10) + (_item.PerMagRef * (tmpDifference / 100))

                                'MinMagDmg
                                tmpDifference = _refitem.MAX_FROM_MAGATK - _refitem.MIN_FROM_MAGATK
                                Dim MinMagDmg As Single = _refitem.MIN_FROM_MAGATK + (_item.PerMagAtk * (tmpDifference / 100)) + (_item.Plus * _refitem.MAGATK_INC)

                                'MaxMagDmg
                                tmpDifference = _refitem.MAX_TO_MAGATK - _refitem.MIN_TO_MAGATK
                                Dim MaxMagDmg As Single = _refitem.MIN_TO_MAGATK + (_item.PerMagAtk * (tmpDifference / 100)) + (_item.Plus * _refitem.MAGATK_INC)

                                MinMag = Math.Round(Intelligence * (MagRefMin / 100) + MinMagDmg, 0, MidpointRounding.ToEven)
                                MaxMag = Math.Round(Intelligence * (MagRefMax / 100) + MaxMagDmg, 0, MidpointRounding.ToEven)

                                'Parry Rate
                                tmpDifference = _refitem.MAX_ATTACK_RATING - _refitem.MIN_ATTACK_RATING
                                Hit += Math.Round(_refitem.MIN_ATTACK_RATING + (_item.PerAttackRate * (tmpDifference / 100)))
                            Case 7 'Shield

                                Dim tmpDifference As Single

                                'PhyDef
                                'Formel:PerItem PhyDef = STR * (PhyRef / 100) + ItemPhyDef
                                tmpDifference = (_refitem.MAX_PHYS_REINFORCE / 10) - (_refitem.MIN_PHYS_REINFORCE / 10)
                                Dim PhyRef As Single = (_refitem.MIN_PHYS_REINFORCE / 10) + (_item.PerPhyRef * (tmpDifference / 100))

                                tmpDifference = _refitem.MAX_PHYSDEF - _refitem.MIN_PHYSDEF
                                Dim ItemPhyDef As Single = _refitem.MIN_PHYSDEF + (_item.PerPhyDef * (tmpDifference / 100)) + (_item.Plus * _refitem.PHYSDEF_INC)
                                PhyDef += Math.Round(Strength * (PhyRef / 100) + ItemPhyDef, 0, MidpointRounding.ToEven)

                                'MagDef
                                'Formel:PerItem PhyDef = STR * (MagRef / 100) + ItemMagDef
                                tmpDifference = (_refitem.MAX_MAG_REINFORCE / 10) - (_refitem.MIN_MAG_REINFORCE / 10)
                                Dim MagRef As Single = (_refitem.MIN_MAG_REINFORCE / 10) + (_item.PerMagRef * (tmpDifference / 100))

                                tmpDifference = _refitem.MAX_MAGDEF - _refitem.MIN_MAGDEF
                                Dim ItemMagDef As Single = _refitem.MIN_MAGDEF + (_item.PerMagDef * (tmpDifference / 100)) + (_item.Plus * _refitem.MAGDEF_INC)
                                MagDef += Math.Round(Intelligence * (MagRef / 100) + ItemMagDef, 0, MidpointRounding.ToEven)

                            Case 8 'Jobsuit/PVP Cape

                            Case 9, 10, 11, 12 'Accessory
                                Dim tmpDifference As Single

                                tmpDifference = _refitem.MAX_PHYS_ABSORB - _refitem.MIN_PHYS_ABSORB
                                Dim tmpPhyAbs As Single = _refitem.MIN_PHYS_ABSORB + (_item.PerPhyAbs * (tmpDifference / 100)) + (_item.Plus * _refitem.PHYS_ABSORB_INC)
                                PhyAbs += tmpPhyAbs  'Math.Round(tmpPhyAbs, 0, MidpointRounding.ToEven)

                                tmpDifference = _refitem.MAX_MAG_ABSORB - _refitem.MIN_MAG_ABSORB
                                Dim tmpMagAbs As Single = _refitem.MIN_MAG_ABSORB + (_item.PerMagAbs * (tmpDifference / 100)) + (_item.Plus * _refitem.MAG_ABSORB_INC)
                                MagAbs += tmpMagAbs 'Math.Round(tmpMagAbs, 0, MidpointRounding.ToEven)

                        End Select
                    End If
                Else
                    'Slot is empty.

                End If
            Next

            'Wenn slot leer ist klappt diese formel nicht mehr darum benutz ich vorerst die alten formeln als fix
            If MinPhy = 0 Then
                MinPhy = GameServer.Functions.GetMinPhy(Me.Strength)
            End If
            If MaxPhy = 0 Then
                MaxPhy = GameServer.Functions.GetMaxPhy(Me.Strength, Level)
            End If
            If MinMag = 0 Then
                MinMag = GameServer.Functions.GetMinMag(Me.Intelligence, Level)
            End If
            If MaxMag = 0 Then
                MaxMag = GameServer.Functions.GetMaxMag(Me.Intelligence, Level)
            End If
            If PhyDef = 0 Then
                PhyDef = GameServer.Functions.GetPhyDef(Me.Strength, Level)
            End If
            If MagDef = 0 Then
                MagDef = GameServer.Functions.GetMagDef(Me.Intelligence)
            End If


        End Sub

        Sub AddBuffsToStats()
            Dim tmplist As Array = Buffs.Keys.ToArray
            For Each key In tmplist
                If Buffs.ContainsKey(key) Then
                    Select Case Buffs(key).Type
                        Case BuffType_.ItemBuff
                            ' Dim Ref As cItem = GameServer.GetItemByID(Buffs(key).ItemID)


                        Case BuffType_.SkillBuff
                            'Dim Ref As GameServer.Skill_ = GameServer.GetSkillById(Buffs(key).SkillID)


                    End Select
                End If
            Next
        End Sub
    End Class

    Public Class Position
        Public XSector As Byte
        Public YSector As Byte
        Public X As Single
        Public Z As Single
        Public Y As Single

        Public Function ToGameX() As Single
            Return ((XSector - 135) * 192 + (X / 10))
        End Function
        Public Function ToGameY() As Single
            Return ((YSector - 92) * 192 + (Y / 10))
        End Function
    End Class

    Public Enum UseItemTypes
        None = 0
        Pot = 1
        Return_Scroll = 2
        Reverse_Scroll_Dead = 3
        Reverse_Scroll_Recall = 4
        Reverse_Scroll_Point = 5
    End Enum

    Public Structure cHotKey
        Public OwnerID As UInteger
        Public Type As UInteger
        Public Slot As UInteger
        Public IconID As UInteger
    End Structure

    Public Enum TeleportType_
        Npc = 0
        GM = 1
    End Enum


    Public Enum AttackType_
        Normal = 0
        Skill = 1
        Buff = 2
    End Enum

    Public Enum MoveType_
        Standing = 0
        Walking = 1
        Runing = 2
        Berserk = 3
    End Enum

End Namespace