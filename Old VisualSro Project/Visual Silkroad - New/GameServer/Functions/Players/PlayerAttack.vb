Imports SRFramework

Namespace Functions
    Module PlayerAttack
        Public Rand As New Random

        Public Sub OnPlayerAttack(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim found As Boolean = False
            Dim type1 As Byte = packet.Byte

            If type1 = 1 Then
                'Attacking
                Select Case packet.Byte
                    Case 1
                        'Normal Attack
                        packet.Skip(2)
                        Dim ObjectID As UInt32 = packet.DWord
                        If PlayerData(Index_).Attacking = False Then
                            If MobList.ContainsKey(ObjectID) And MobList(ObjectID).Death = False Then
                                PlayerAttackNormal(Index_, ObjectID)
                            End If
                            For i = 0 To Server.MaxClients - 1
                                If PlayerData(i) IsNot Nothing Then
                                    If PlayerData(i).UniqueID = ObjectID Then
                                        PlayerAttackNormal(Index_, ObjectID)
                                    End If
                                End If
                            Next

                        End If
                    Case 2
                        'Pickup
                        packet.Byte()
                        Dim ObjectID As UInt32 = packet.DWord

                        If ItemList.ContainsKey(ObjectID) Then
                            OnPickUp(ObjectID, Index_)
                        End If

                    Case 4
                        Dim skillid As UInteger = packet.DWord
                        Dim type As Byte = packet.Byte()
                        'Type = 1--> Monster Attack --- Type = 0 --> Buff
                        Dim refskill As RefSkill = GetSkill(skillid)

                        Select Case type
                            Case 0
                                PlayerBuff_BeginnCasting(skillid, Index_)
                            Case 1
                                Dim ObjectID As UInt32 = packet.DWord

                                If PlayerData(Index_).Attacking = False Then
                                    If MobList.ContainsKey(ObjectID) And MobList(ObjectID).Death = False Then
                                        PlayerAttackBeginSkill(skillid, Index_, ObjectID)
                                        found = True
                                    End If

                                Else
                                    If PlayerData(Index_).AttackType = AttackTypes.Normal Then
                                        If MobList.ContainsKey(ObjectID) And MobList(ObjectID).Death = False Then
                                            'Cleanup the regular Attack before using a Skill
                                            PlayerAttackTimer(Index_).Stop()
                                            PlayerData(Index_).Attacking = False
                                            PlayerData(Index_).Busy = False
                                            PlayerData(Index_).AttackType = AttackTypes.Normal
                                            PlayerData(Index_).AttackedId = 0
                                            PlayerData(Index_).UsingSkillId = 0
                                            PlayerData(Index_).SkillOverId = 0

                                            PlayerAttackBeginSkill(skillid, Index_, ObjectID)
                                            found = True
                                        End If
                                    End If
                                End If
                        End Select
                End Select


                If found = False Then
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_ATTACK_REPLY)
                    writer.Byte(2)
                    writer.Byte(0)
                    Server.Send(writer.GetBytes, Index_)
                End If
            Else
                'Attack Abort
                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_ATTACK_REPLY)
                writer.Byte(2)
                writer.Byte(0)
                Server.Send(writer.GetBytes, Index_)

                PlayerData(Index_).Attacking = False
                PlayerData(Index_).Busy = False
                PlayerData(Index_).AttackedId = 0
                PlayerData(Index_).UsingSkillId = 0
                PlayerData(Index_).AttackType = AttackTypes.Normal
                PlayerAttackTimer(Index_).Stop()
            End If
        End Sub

        Public Sub PlayerAttackNormal(ByVal Index_ As Integer, ByVal MobUniqueId As Integer)

            If PlayerData(Index_).Busy Or MobList.ContainsKey(MobUniqueId) = False Then
                Exit Sub
            End If

            Dim numberAttack = 1, NumberVictims = 1, attackType, afterstate As UInteger
            Dim refWeapon As New cRefItem
            Dim attObject As SilkroadObject = GetObject(MobList(MobUniqueId).Pk2ID)
            Dim mob As cMonster = MobList(MobUniqueId)

            If Inventorys(Index_).UserItems(6).ItemID <> 0 Then
                'Weapon
                refWeapon = GetItemByID(GameDB.Items(Inventorys(Index_).UserItems(6).ItemID).ObjectID)
            Else
                'No Weapon
                refWeapon.ATTACK_DISTANCE = 6
            End If

            Dim distance As Double = CalculateDistance(PlayerData(Index_).Position, mob.Position)
            If distance >= (Math.Sqrt(refWeapon.ATTACK_DISTANCE)) Then
                Dim walktime As Single = MoveUserToObject(Index_, mob.Position, (Math.Sqrt(refWeapon.ATTACK_DISTANCE)))
                PlayerData(Index_).AttackType = AttackTypes.Normal
                PlayerData(Index_).AttackedId = MobList(MobUniqueId).UniqueID

                PlayerAttackTimer(Index_).Interval = walktime
                PlayerAttackTimer(Index_).Start()

                Exit Sub
            End If

            Select Case refWeapon.CLASS_C
                Case 0
                    attackType = 1
                Case 2
                    numberAttack = 2
                    attackType = 2
                Case 3
                    numberAttack = 2
                    attackType = 2
                Case 4
                    attackType = 40
                Case 5
                    attackType = 40
                Case 6
                    attackType = 70
                Case 7
                    attackType = 7127
                Case 8
                    attackType = 7128
                Case 9
                    numberAttack = 2
                    attackType = 7129
                Case 10
                    attackType = 9069
                Case 11
                    attackType = 8454
                Case 12
                    attackType = 7909
                Case 13
                    numberAttack = 2
                    attackType = 7910
                Case 14
                    attackType = 9606
                Case 15
                    attackType = 9970
            End Select

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ATTACK_REPLY)
            writer.Byte(1)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)


            writer.Create(ServerOpcodes.GAME_ATTACK_MAIN)
            writer.Byte(1)
            writer.Byte(2)
            writer.Byte(&H30)

            writer.DWord(attackType)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.DWord(Id_Gen.GetSkillOverId)
            writer.DWord(mob.UniqueID)

            writer.Byte(1)
            writer.Byte(numberAttack)
            writer.Byte(NumberVictims)
            '1 victim

            For d = 0 To NumberVictims - 1
                writer.DWord(MobList(MobUniqueId).UniqueID)

                For i = 0 To numberAttack - 1
                    Dim Damage As UInteger = CalculateDamageMob(Index_, attObject, attackType)
                    Dim Crit As Byte = Attack_GetCritical()

                    If Crit = True Then
                        Damage = Damage * 2
                        Crit = 2
                    End If
                    If CLng(MobList(MobUniqueId).HP_Cur) - Damage > 0 Then
                        MobList(MobUniqueId).HP_Cur -= Damage
                        MobAddDamageFromPlayer(Damage, Index_, mob.UniqueID, True)
                    ElseIf CLng(MobList(MobUniqueId).HP_Cur) - Damage <= 0 Then
                        'Dead
                        afterstate = &H80
                        MobAddDamageFromPlayer(mob.HP_Cur, Index_, mob.UniqueID, False)
                        'Done the last Damage
                        MobList(MobUniqueId).HP_Cur = 0
                    End If

                    writer.Byte(afterstate)
                    writer.Byte(Crit)
                    writer.DWord(Damage)
                    writer.Byte(0)
                    writer.Word(0)
                Next
            Next
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            If afterstate = &H80 Then
                GetEXPFromMob(MobList(MobUniqueId))
                KillMob(MobList(MobUniqueId).UniqueID)
                Attack_SendAttackEnd(Index_)

                PlayerData(Index_).Attacking = False
                PlayerData(Index_).Busy = False
                PlayerData(Index_).AttackType = AttackTypes.Normal
                PlayerData(Index_).AttackedId = 0
                PlayerData(Index_).UsingSkillId = 0
                PlayerData(Index_).SkillOverId = 0
            Else
                PlayerData(Index_).Attacking = True
                PlayerData(Index_).Busy = True
                PlayerData(Index_).AttackedId = MobList(MobUniqueId).UniqueID
                PlayerData(Index_).UsingSkillId = attackType
                PlayerData(Index_).AttackType = AttackTypes.Normal
                If PlayerAttackTimer(Index_).Enabled = False Then
                    PlayerAttackTimer(Index_).Interval = 2500
                    PlayerAttackTimer(Index_).Start()
                End If

                'Monster Attack back
                mob.AttackTimer_Start(5)
            End If
        End Sub

        Public Sub PlayerAttackBeginSkill(ByVal SkillID As UInt32, ByVal Index_ As Integer, ByVal MobUniqueId As Integer)
            Dim RefSkill As RefSkill = GetSkill(SkillID)
            Dim Mob_ As cMonster = MobList(MobUniqueId)

            If PlayerData(Index_).Busy Or CheckIfUserOwnSkill(SkillID, Index_) = False Then
                Exit Sub
            End If


            If CalculateDistance(PlayerData(Index_).Position, Mob_.Position) >= RefSkill.Distance Then
                Dim Walktime As Single = MoveUserToObject(Index_, Mob_.Position, RefSkill.Distance)
                PlayerData(Index_).AttackType = AttackTypes.Normal
                PlayerData(Index_).AttackedId = MobList(MobUniqueId).UniqueID

                PlayerAttackTimer(Index_).Interval = Walktime
                PlayerAttackTimer(Index_).Start()

                Exit Sub
            End If

            If CInt(PlayerData(Index_).CMP) - RefSkill.RequiredMp < 0 And PlayerData(Index_).Invincible = False Then
                'Not enough MP
                Attack_SendNotEnoughMP(Index_)
                Exit Sub
            Else
                If PlayerData(Index_).Invincible = False Then
                    PlayerData(Index_).CMP -= RefSkill.RequiredMp
                    UpdateMP(Index_)
                End If
            End If

            PlayerData(Index_).AttackedId = Mob_.UniqueID
            PlayerData(Index_).Attacking = True
            PlayerData(Index_).UsingSkillId = SkillID
            PlayerData(Index_).AttackType = AttackTypes.Skill
            PlayerData(Index_).SkillOverId = Id_Gen.GetSkillOverId
            MobSetAttackingFromPlayer(Index_, Mob_.UniqueID, True)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ATTACK_REPLY)
            writer.Byte(1)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.GAME_ATTACK_MAIN)
            writer.Byte(1)
            writer.Byte(2)
            writer.Byte(&H30)

            writer.DWord(PlayerData(Index_).UsingSkillId)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.DWord(PlayerData(Index_).SkillOverId)
            writer.DWord(PlayerData(Index_).AttackedId)
            writer.Byte(0)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            If RefSkill.CastTime > 0 Then
                PlayerAttackTimer(Index_).Interval = RefSkill.CastTime
                PlayerAttackTimer(Index_).Start()
            Else
                PlayerAttackEndSkill(Index_)
            End If
        End Sub

        Public Sub PlayerAttackEndSkill(ByVal Index_ As Integer)
            Dim RefSkill As RefSkill = GetSkill(PlayerData(Index_).UsingSkillId)
            Dim AttObject As New SilkroadObject
            Dim RefWeapon As New cRefItem
            Dim Mob_ As cMonster = MobList(PlayerData(Index_).AttackedId)
            Dim afterstate, NumberVictims As Integer
            NumberVictims = 1

            If Inventorys(Index_).UserItems(6).ItemID <> 0 Then
                'Weapon
                RefWeapon = GetItemByID(GameDB.Items(Inventorys(Index_).UserItems(6).ItemID).ObjectID)
            Else
                'No Weapon
                Exit Sub
            End If


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_BUFF_INFO)
            writer.Byte(1)

            writer.DWord(PlayerData(Index_).SkillOverId)
            writer.DWord(PlayerData(Index_).AttackedId)

            writer.Byte(1)
            writer.Byte(RefSkill.NumberOfAttacks)
            writer.Byte(NumberVictims)
            '1 victim

            For d = 0 To NumberVictims - 1
                writer.DWord(PlayerData(Index_).AttackedId)

                For i = 0 To RefSkill.NumberOfAttacks - 1
                    Dim damage As UInteger = CalculateDamageMob(Index_, AttObject, RefSkill.Pk2Id)
                    Dim crit As Byte = Attack_GetCritical()

                    If crit = True Then
                        damage = damage * 2
                        crit = 2
                    End If

                    If CLng(Mob_.HP_Cur) - damage > 0 Then
                        Mob_.HP_Cur -= damage
                        MobAddDamageFromPlayer(damage, Index_, Mob_.UniqueID, True)
                    ElseIf CLng(Mob_.HP_Cur) - damage <= 0 Then
                        'Dead
                        afterstate = &H80
                        MobAddDamageFromPlayer(Mob_.HP_Cur, Index_, Mob_.UniqueID, True)
                        Mob_.HP_Cur = 0
                    End If

                    writer.Byte(afterstate)
                    writer.Byte(crit)
                    writer.DWord(damage)
                    writer.Byte(0)
                    writer.Word(0)
                Next
            Next
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            If afterstate = &H80 Then
                GetEXPFromMob(Mob_)
                KillMob(Mob_.UniqueID)
                Attack_SendAttackEnd(Index_)

                PlayerData(Index_).Attacking = False
                PlayerData(Index_).Busy = False
                PlayerData(Index_).AttackType = AttackTypes.Normal
                PlayerData(Index_).AttackedId = 0
                PlayerData(Index_).UsingSkillId = 0
                PlayerData(Index_).SkillOverId = 0
            Else
                PlayerData(Index_).Attacking = True
                PlayerData(Index_).Busy = True
                PlayerData(Index_).AttackType = AttackTypes.Normal
                If PlayerAttackTimer(Index_).Enabled = False Then
                    PlayerAttackTimer(Index_).Interval = 2500
                    PlayerAttackTimer(Index_).Start()
                End If

                'Monster Attack back
                Mob_.AttackTimer_Start(5)
            End If
        End Sub


        Function CalculateDamageMob(ByVal Index_ As Integer, ByVal Mob As SilkroadObject, ByVal SkillID As UInt32) As UInteger
            Dim RefSkill As RefSkill = GetSkill(SkillID)
            Dim FinalDamage As UInteger
            Dim Balance As Double
            'If (CSng(PlayerData(Index_).Level) - Mob.Level) > -10 Then
            '    Balance = (1 + ((CSng(PlayerData(Index_).Level) - Mob.Level) / 100))
            'Else
            '    Balance = 0.01
            'End If
            Balance = (1 + ((CSng(PlayerData(Index_).Level) - Mob.Level) / 100))
            If Balance < 0 Then
                Balance = 0.01
            End If

            Dim DamageMin As Double
            Dim DamageMax As Double

            If RefSkill.Type = SkillTypeTable.Phy Then
                DamageMin = ((PlayerData(Index_).MinPhy + RefSkill.PwrMin) * (1 + 0) / (1 + 0) - Mob.PhyDef) * Balance * (1 + 0) *
                            (RefSkill.PwrPercent / 10)
                DamageMax = ((PlayerData(Index_).MaxPhy + RefSkill.PwrMax) * (1 + 0) / (1 + 0) - Mob.PhyDef) * Balance * (1 + 0) *
                            (RefSkill.PwrPercent / 10)
            ElseIf RefSkill.Type = SkillTypeTable.Mag Then
                DamageMin = ((PlayerData(Index_).MinMag + RefSkill.PwrMin) * (1 + 0) / (1 + 0) - Mob.MagDef) * Balance * (1 + 0) *
                            (RefSkill.PwrPercent / 10)
                DamageMax = ((PlayerData(Index_).MaxMag + RefSkill.PwrMax) * (1 + 0) / (1 + 0) - Mob.MagDef) * Balance * (1 + 0) *
                            (RefSkill.PwrPercent / 10)
            End If


            If DamageMin <= 0 Or DamageMin >= UInteger.MaxValue Then
                DamageMin = 1
            End If
            If DamageMax <= 0 Or DamageMax >= UInteger.MaxValue Then
                DamageMax = 4
            End If


            If DamageMax < DamageMin Then
                Log.WriteSystemLog(
                    String.Format("Ply Max Dmg over Min Dmg. Min {0} Max {1}. Char: {2}, Mob:{3}, Skill:{4}", DamageMin,
                                  DamageMax, PlayerData(Index_).CharacterName, Mob.TypeName, SkillID))
                DamageMax = DamageMin + 1
            End If


            Dim Radmon As Integer = Rnd() * 100
            FinalDamage = DamageMin + (((DamageMax - DamageMin) / 100) * Radmon)


            If PlayerData(Index_).Berserk = True Then
                FinalDamage *= 2
            End If
            'A = Basic Attack Power
            'B = Skill Attack Power
            'C = Attack Power Increasing rate
            'D = Enemy 's total accessories Absorption rate
            'E = Enemy 's Defence Power
            'F = Balance rate
            'G = Total Damage Increasing rate
            'H = Skill Attack Power rate
            'A final damage formula:

            'Damage = ((A + B) * (1 + C) / (1 + D) - E) * F * (1 + G) * H

            Return FinalDamage
        End Function

        Public Sub Attack_SendNotEnoughMP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ATTACK_REPLY)
            writer.Byte(3)
            writer.Byte(0)
            writer.Byte(4)
            writer.Byte(&H40)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.GAME_ATTACK_MAIN)
            writer.Byte(2)
            writer.Byte(4)
            writer.Byte(&H30)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub Attack_SendAttackEnd(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ATTACK_REPLY)
            writer.Byte(2)
            writer.Byte(0)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Function Attack_GetCritical() As Boolean
            If Math.Round(Rnd() * 5) = 5 Then
                Return True
            Else
                Return False
            End If
        End Function
    End Module
End Namespace