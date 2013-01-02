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
                        Dim objectID As UInt32 = packet.DWord
                        If PlayerData(Index_).Attacking = False Then
                            If MobList.ContainsKey(objectID) And MobList(objectID).Death = False Then
                                PlayerAttackNormal(Index_, objectID)
                            End If
                            For i = 0 To Server.MaxClients - 1
                                If PlayerData(i) IsNot Nothing Then
                                    If PlayerData(i).UniqueID = objectID Then
                                        PlayerAttackNormal(Index_, objectID)
                                    End If
                                End If
                            Next

                        End If
                    Case 2
                        'Pickup
                        packet.Byte()
                        Dim objectID As UInt32 = packet.DWord

                        If ItemList.ContainsKey(objectID) Then
                            OnPickUp(objectID, Index_)
                        End If

                    Case 4
                        'Use a Skill
                        Dim skillid As UInteger = packet.DWord
                        Dim type As Byte = packet.Byte()
                        'Type = 1--> Monster Attack --- Type = 0 --> Buff
               
                        Select Case type
                            Case 0
                                PlayerBuffBeginnCasting(skillid, Index_)
                            Case 1
                                Dim objectID As UInt32 = packet.DWord

                                If PlayerData(Index_).Attacking = False Then
                                    If MobList.ContainsKey(objectID) And MobList(objectID).Death = False Then
                                        PlayerAttackBeginSkill(skillid, Index_, objectID)
                                        found = True
                                    End If

                                Else
                                    If PlayerData(Index_).AttackType = AttackTypes.Normal Then
                                        If MobList.ContainsKey(objectID) And MobList(objectID).Death = False Then
                                            'Cleanup the regular Attack before using a Skill
                                            PlayerAttackTimer(Index_).Stop()
                                            PlayerData(Index_).Attacking = False
                                            PlayerData(Index_).Busy = False
                                            PlayerData(Index_).AttackType = AttackTypes.Normal
                                            PlayerData(Index_).AttackedId = 0
                                            PlayerData(Index_).UsingSkillId = 0
                                            PlayerData(Index_).SkillOverId = 0

                                            PlayerAttackBeginSkill(skillid, Index_, objectID)
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

        Public Sub PlayerAttackNormal(ByVal Index_ As Integer, ByVal mobUniqueId As Integer)

            If PlayerData(Index_).Busy Or MobList.ContainsKey(mobUniqueId) = False Then
                Exit Sub
            End If

            Dim numberAttack = 1, numberVictims = 1, attackType, afterstate As UInteger
            Dim refWeapon As New cRefItem
            Dim attObject As SilkroadObject = GetObject(MobList(mobUniqueId).Pk2ID)
            Dim mob As cMonster = MobList(mobUniqueId)

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
                PlayerData(Index_).AttackedId = MobList(mobUniqueId).UniqueID

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
            writer.Byte(numberVictims)
            '1 victim

            For d = 0 To numberVictims - 1
                writer.DWord(MobList(mobUniqueId).UniqueID)

                For i = 0 To numberAttack - 1
                    Dim damage As UInteger = CalculateDamageMob(Index_, attObject, attackType)
                    Dim crit As Byte = AttackGetCritical()

                    If crit = True Then
                        damage = damage * 2
                        crit = 2
                    End If
                    If CLng(MobList(mobUniqueId).HPCur) - damage > 0 Then
                        MobList(mobUniqueId).HPCur -= damage
                        MobAddDamageFromPlayer(damage, Index_, mob.UniqueID, True)
                    ElseIf CLng(MobList(mobUniqueId).HPCur) - damage <= 0 Then
                        'Dead
                        afterstate = &H80
                        MobAddDamageFromPlayer(mob.HPCur, Index_, mob.UniqueID, False)
                        'Done the last Damage
                        MobList(mobUniqueId).HPCur = 0
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
                GetEXPFromMob(MobList(mobUniqueId))
                KillMob(MobList(mobUniqueId).UniqueID)
                AttackSendAttackEnd(Index_)

                PlayerData(Index_).Attacking = False
                PlayerData(Index_).Busy = False
                PlayerData(Index_).AttackType = AttackTypes.Normal
                PlayerData(Index_).AttackedId = 0
                PlayerData(Index_).UsingSkillId = 0
                PlayerData(Index_).SkillOverId = 0
            Else
                PlayerData(Index_).Attacking = True
                PlayerData(Index_).Busy = True
                PlayerData(Index_).AttackedId = MobList(mobUniqueId).UniqueID
                PlayerData(Index_).UsingSkillId = attackType
                PlayerData(Index_).AttackType = AttackTypes.Normal
                If PlayerAttackTimer(Index_).Enabled = False Then
                    PlayerAttackTimer(Index_).Interval = 2500
                    PlayerAttackTimer(Index_).Start()
                End If

                'Monster Attack back
                mob.AttackTimerStart(5)
            End If
        End Sub

        Private Sub PlayerAttackBeginSkill(ByVal skillID As UInt32, ByVal Index_ As Integer, ByVal mobUniqueId As Integer)
            Dim refSkill As RefSkill = GetSkill(skillID)
            Dim Mob_ As cMonster = MobList(mobUniqueId)

            If PlayerData(Index_).Busy Or CheckIfUserOwnSkill(skillID, Index_) = False Then
                Exit Sub
            End If


            If CalculateDistance(PlayerData(Index_).Position, Mob_.Position) >= refSkill.Distance Then
                Dim walktime As Single = MoveUserToObject(Index_, Mob_.Position, refSkill.Distance)
                PlayerData(Index_).AttackType = AttackTypes.Normal
                PlayerData(Index_).AttackedId = MobList(mobUniqueId).UniqueID

                PlayerAttackTimer(Index_).Interval = walktime
                PlayerAttackTimer(Index_).Start()

                Exit Sub
            End If

            If CInt(PlayerData(Index_).CMP) - refSkill.RequiredMp < 0 And PlayerData(Index_).Invincible = False Then
                'Not enough MP
                AttackSendNotEnoughMP(Index_)
                Exit Sub
            Else
                If PlayerData(Index_).Invincible = False Then
                    PlayerData(Index_).CMP -= refSkill.RequiredMp
                    UpdateMP(Index_)
                End If
            End If

            PlayerData(Index_).AttackedId = Mob_.UniqueID
            PlayerData(Index_).Attacking = True
            PlayerData(Index_).UsingSkillId = skillID
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

            If refSkill.CastTime > 0 Then
                PlayerAttackTimer(Index_).Interval = refSkill.CastTime
                PlayerAttackTimer(Index_).Start()
            Else
                PlayerAttackEndSkill(Index_)
            End If
        End Sub

        Public Sub PlayerAttackEndSkill(ByVal Index_ As Integer)
            Dim refSkill As RefSkill = GetSkill(PlayerData(Index_).UsingSkillId)
            Dim attObject As New SilkroadObject
            Dim Mob_ As cMonster = MobList(PlayerData(Index_).AttackedId)
            Dim afterstate, numberVictims As Integer
            numberVictims = 1

            If Inventorys(Index_).UserItems(6).ItemID = 0 Then
                'No Weapon
                Exit Sub
            End If


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_BUFF_INFO)
            writer.Byte(1)

            writer.DWord(PlayerData(Index_).SkillOverId)
            writer.DWord(PlayerData(Index_).AttackedId)

            writer.Byte(1)
            writer.Byte(refSkill.NumberOfAttacks)
            writer.Byte(numberVictims)
            '1 victim

            For d = 0 To numberVictims - 1
                writer.DWord(PlayerData(Index_).AttackedId)

                For i = 0 To refSkill.NumberOfAttacks - 1
                    Dim damage As UInteger = CalculateDamageMob(Index_, attObject, refSkill.Pk2Id)
                    Dim crit As Byte = AttackGetCritical()

                    If crit = True Then
                        damage = damage * 2
                        crit = 2
                    End If

                    If CLng(Mob_.HPCur) - damage > 0 Then
                        Mob_.HPCur -= damage
                        MobAddDamageFromPlayer(damage, Index_, Mob_.UniqueID, True)
                    ElseIf CLng(Mob_.HPCur) - damage <= 0 Then
                        'Dead
                        afterstate = &H80
                        MobAddDamageFromPlayer(Mob_.HPCur, Index_, Mob_.UniqueID, True)
                        Mob_.HPCur = 0
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
                AttackSendAttackEnd(Index_)

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
                Mob_.AttackTimerStart(5)
            End If
        End Sub


        Private Function CalculateDamageMob(ByVal Index_ As Integer, ByVal mob As SilkroadObject, ByVal skillID As UInt32) As UInteger
            Dim refSkill As RefSkill = GetSkill(skillID)
            Dim finalDamage As UInteger
            Dim balance As Double
            'If (CSng(PlayerData(Index_).Level) - Mob.Level) > -10 Then
            '    Balance = (1 + ((CSng(PlayerData(Index_).Level) - Mob.Level) / 100))
            'Else
            '    Balance = 0.01
            'End If
            balance = (1 + ((CSng(PlayerData(Index_).Level) - mob.Level) / 100))
            If balance < 0 Then
                balance = 0.01
            End If

            Dim damageMin As Double
            Dim damageMax As Double

            If refSkill.Type = SkillTypeTable.Phy Then
                damageMin = ((PlayerData(Index_).MinPhy + refSkill.PwrMin) * (1 + 0) / (1 + 0) - mob.PhyDef) * balance * (1 + 0) *
                            (refSkill.PwrPercent / 10)
                damageMax = ((PlayerData(Index_).MaxPhy + refSkill.PwrMax) * (1 + 0) / (1 + 0) - mob.PhyDef) * balance * (1 + 0) *
                            (refSkill.PwrPercent / 10)
            ElseIf refSkill.Type = SkillTypeTable.Mag Then
                damageMin = ((PlayerData(Index_).MinMag + refSkill.PwrMin) * (1 + 0) / (1 + 0) - mob.MagDef) * balance * (1 + 0) *
                            (refSkill.PwrPercent / 10)
                damageMax = ((PlayerData(Index_).MaxMag + refSkill.PwrMax) * (1 + 0) / (1 + 0) - mob.MagDef) * balance * (1 + 0) *
                            (refSkill.PwrPercent / 10)
            End If


            If damageMin <= 0 Or damageMin >= UInteger.MaxValue Then
                damageMin = 1
            End If
            If damageMax <= 0 Or damageMax >= UInteger.MaxValue Then
                damageMax = 4
            End If


            If damageMax < damageMin Then
                Log.WriteSystemLog(
                    String.Format("Ply Max Dmg over Min Dmg. Min {0} Max {1}. Char: {2}, Mob:{3}, Skill:{4}", damageMin,
                                  damageMax, PlayerData(Index_).CharacterName, mob.CodeName, skillID))
                damageMax = damageMin + 1
            End If


            Dim radmon As Integer = Rnd() * 100
            finalDamage = damageMin + (((damageMax - damageMin) / 100) * radmon)


            If PlayerData(Index_).Berserk = True Then
                finalDamage *= 2
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

            Return finalDamage
        End Function

        Public Sub AttackSendNotEnoughMP(ByVal Index_ As Integer)
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

        Public Sub AttackSendAttackEnd(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ATTACK_REPLY)
            writer.Byte(2)
            writer.Byte(0)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Function AttackGetCritical() As Boolean
            If Math.Round(Rnd() * 5) = 5 Then
                Return True
            Else
                Return False
            End If
        End Function
    End Module
End Namespace