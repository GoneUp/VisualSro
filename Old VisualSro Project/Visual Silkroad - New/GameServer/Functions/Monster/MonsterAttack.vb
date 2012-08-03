Imports SRFramework

Namespace Functions
    Module MonsterAttack
        Public Sub MonsterAttackPlayer(ByVal MobUniqueID As Integer, ByVal Index_ As Integer,
                                       Optional ByVal SkillID As UInteger = 0)
            If MobList.ContainsKey(MobUniqueID) = False Or Index_ = -1 Or PlayerData(Index_) Is Nothing Then
                Exit Sub
            End If
            If MobList(MobUniqueID).IsAttacking = True Then
                Exit Sub
            End If

            Dim Mob_ As cMonster = MobList(MobUniqueID)
            Dim RefSkill As Skill

            Mob_.AttackingId = PlayerData(Index_).UniqueID

            'Search for the right Skill
            If SkillID = 0 Then
                Mob_.UsingSkillId = Monster_GetNextSkill(Mob_.UsingSkillId, Mob_.Pk2ID)
            Else
                Mob_.UsingSkillId = SkillID
            End If

            RefSkill = GetSkill(Mob_.UsingSkillId)

            Do Until RefSkill.Effect_0 = "att"
                CheckForSkillEffects(MobUniqueID, RefSkill, Index_)
                Mob_.UsingSkillId = Monster_GetNextSkill(Mob_.UsingSkillId, Mob_.Pk2ID)
                RefSkill = GetSkill(Mob_.UsingSkillId)
            Loop


            Dim NumberAttack = 1, NumberVictims = 1, afterstate As UInteger
            Dim AttObject As SilkroadObject = GetObject(PlayerData(Index_).Pk2ID)
            Dim RefMonster As SilkroadObject = GetObject(Mob_.Pk2ID)


            Dim Distance As Double = CalculateDistance(PlayerData(Index_).Position, Mob_.Position)
            If Distance >= Math.Sqrt(RefSkill.Distance) And RefMonster.WalkSpeed > 0 Then
                MoveMobToUser(MobUniqueID, PlayerData(Index_).Position, Math.Sqrt(RefSkill.Distance))
                Exit Sub
            End If


            UpdateState(1, 3, Mob_)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ATTACK_MAIN)
            writer.Byte(1)
            writer.Byte(2)
            writer.Byte(&H30)

            writer.DWord(RefSkill.Pk2Id)
            writer.DWord(Mob_.UniqueID)
            writer.DWord(Id_Gen.GetSkillOverId)
            writer.DWord(PlayerData(Index_).UniqueID)

            writer.Byte(1)
            writer.Byte(NumberAttack)
            writer.Byte(NumberVictims)
            '1 victim

            For d = 0 To NumberVictims - 1
                writer.DWord(PlayerData(Index_).UniqueID)

                For i = 0 To NumberAttack - 1
                    Dim Damage As UInteger = CalculateDamagePlayer(Index_, RefMonster, RefSkill.Pk2Id)
                    Dim Crit As Byte = Attack_GetCritical()
                    If Crit = True Then
                        Damage = Damage * 2
                        Crit = 2
                    End If

                    If CLng(PlayerData(Index_).CHP) - Damage > 0 Then
                        If PlayerData(Index_).Invincible = False Then
                            PlayerData(Index_).CHP -= Damage
                        End If
                    ElseIf CLng(PlayerData(Index_).CHP) - Damage <= 0 Then
                        'Dead
                        If PlayerData(Index_).Invincible = False Then
                            afterstate = &H80
                            PlayerData(Index_).CHP = 0
                        End If
                    End If

                    writer.Byte(afterstate)
                    writer.Byte(Crit)
                    writer.DWord(Damage)
                    writer.Byte(0)
                    writer.Word(0)
                Next
            Next
            Server.SendIfMobIsSpawned(writer.GetBytes, Mob_.UniqueID)

            Mob_.AttackingId = 0

            If afterstate = &H80 Then
                KillPlayer(Index_)
                MobSetAttackingFromPlayer(Index_, Mob_.UniqueID, False)
            Else
                UpdateHP(Index_)
                Mob_.AttackTimer_Start(RefSkill.UseDuration * 250)
                Mob_.AttackEndTime = Date.Now.AddMilliseconds(RefSkill.UseDuration * 250)
            End If
        End Sub

        Public Sub CheckMonsterAggro()
            Dim tmplist As Array = MobList.Keys.ToArray
            For Each key In tmplist
                If MobList.ContainsKey(key) Then
                    Dim Mob_ As cMonster = MobList(key)
                    For i = 0 To Server.MaxClients - 1
                        If PlayerData(i) IsNot Nothing Then
                            If CalculateDistance(Mob_.Position, PlayerData(i).Position) < 20 Then
                                SendPm(i, "You are in Aggro Range", "Serv")
                            End If
                        End If
                    Next
                End If
            Next
        End Sub

        Public Sub MonsterAttackObject(ByVal MobIndex As Integer, ByVal OtherUniqueID As Integer)
            'is for horses and pet's
        End Sub

        ''' <summary>
        ''' Damage from Mob To Player
        ''' </summary>
        ''' <param name="Index_"></param>
        ''' <param name="Mob"></param>
        ''' <param name="SkillID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CalculateDamagePlayer(ByVal Index_ As Integer, ByVal Mob As SilkroadObject, ByVal SkillID As UInt32) _
            As UInteger
            Dim RefSkill As Skill = GetSkill(SkillID)
            Dim FinalDamage As UInteger
            Dim Balance As Double
            If (CSng(Mob.Level) - PlayerData(Index_).Level) > -99 Then
                Balance = (1 + (CSng(Mob.Level) - PlayerData(Index_).Level) / 100)
            Else
                Balance = 0.01
            End If

            Dim DamageMin As Double
            Dim DamageMax As Double

            If RefSkill.Type = SkillTypeTable.Phy Then
                DamageMin =
                    ((Mob.ParryRatio + RefSkill.PwrMin) * (1 + 0) / (1 + (PlayerData(Index_).PhyAbs / 500)) -
                     PlayerData(Index_).PhyDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
                DamageMax =
                    ((Mob.ParryRatio + RefSkill.PwrMax) * (1 + 0) / (1 + (PlayerData(Index_).PhyAbs / 510)) -
                     PlayerData(Index_).PhyDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
            ElseIf RefSkill.Type = SkillTypeTable.Mag Then
                'UNUSED FOR NOW
                '  DamageMin = ((PlayerData(Index_).MinMag + RefSkill.PwrMin) * (1 + 0) / (1 + 0) - Mob.MagDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
                ' DamageMax = ((PlayerData(Index_).MaxMag + RefSkill.PwrMax) * (1 + 0) / (1 + 0) - Mob.MagDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
            End If


            If DamageMin <= 0 Then
                DamageMin = 1
            End If
            If DamageMax <= 0 Then
                DamageMax = 2
            End If

            Dim Radmon As Integer = Rnd() * 100
            FinalDamage = DamageMin + (((DamageMax - DamageMin) / 100) * Radmon)

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

        Private Function Monster_GetNextSkill(ByVal SkillID As UInteger, ByVal ObjectID As UInteger)
            Dim Ref_ As SilkroadObject = GetObject(ObjectID)


            Select Case SkillID
                Case 0
                    Return Ref_.Skill1

                Case Ref_.Skill1
                    If Ref_.Skill2 <> 0 Then
                        Return Ref_.Skill2
                    Else
                        Return Ref_.Skill1
                    End If
                Case Ref_.Skill2
                    If Ref_.Skill3 <> 0 Then
                        Return Ref_.Skill3
                    Else
                        Return Ref_.Skill1
                    End If

                Case Ref_.Skill3
                    If Ref_.Skill4 <> 0 Then
                        Return Ref_.Skill4
                    Else
                        Return Ref_.Skill1
                    End If

                Case Ref_.Skill4
                    If Ref_.Skill5 <> 0 Then
                        Return Ref_.Skill5
                    Else
                        Return Ref_.Skill1
                    End If

                Case Ref_.Skill5
                    If Ref_.Skill6 <> 0 Then
                        Return Ref_.Skill6
                    Else
                        Return Ref_.Skill1
                    End If

                Case Ref_.Skill6
                    If Ref_.Skill7 <> 0 Then
                        Return Ref_.Skill7
                    Else
                        Return Ref_.Skill1
                    End If

                Case Ref_.Skill7
                    If Ref_.Skill8 <> 0 Then
                        Return Ref_.Skill8
                    Else
                        Return Ref_.Skill1
                    End If

                Case Ref_.Skill8
                    If Ref_.Skill9 <> 0 Then
                        Return Ref_.Skill9
                    Else
                        Return Ref_.Skill1
                    End If

                Case Ref_.Skill9
                    Return Ref_.Skill1
            End Select

            Return 0
        End Function

        Public Sub CheckForSkillEffects(ByVal MobUniqueId As UInteger, ByVal RefSkill As Skill, ByVal Index_ As Integer)
            Dim Mob_ As cMonster = MobList(MobUniqueId)

            Select Case RefSkill.Effect_0
                Case "ssou"
                    Dim Spawn_Mobs As Boolean = False
                    Dim Hp_Percent = ((Mob_.HP_Cur / Mob_.HP_Max) * 100)
                    Select Case RefSkill.SpawnPercent
                        Case 80
                            If Mob_.SpawnedGuard_80 = False And Hp_Percent < 80 And Hp_Percent > 60 Then
                                Spawn_Mobs = True
                                Mob_.SpawnedGuard_80 = True
                            End If
                        Case 60
                            If Mob_.SpawnedGuard_60 = False And Hp_Percent < 60 And Hp_Percent > 40 Then
                                Spawn_Mobs = True
                                Mob_.SpawnedGuard_60 = True
                            End If
                        Case 40
                            If Mob_.SpawnedGuard_40 = False And Hp_Percent < 40 And Hp_Percent > 20 Then
                                Spawn_Mobs = True
                                Mob_.SpawnedGuard_40 = True
                            End If
                        Case 0
                            If Mob_.SpawnedGuard_20 = False And Hp_Percent < 20 And Hp_Percent > 0 Then
                                Spawn_Mobs = True
                                Mob_.SpawnedGuard_20 = True
                            End If
                    End Select

                    If Spawn_Mobs = True Then
                        If RefSkill.Effect_1 <> 0 Then
                            For i = 1 To RefSkill.Effect_4 'Effect_4 = Count of Mobs, 2 = Type  
                                Dim NewUniqueId As UInteger = SpawnMob(RefSkill.Effect_1, RefSkill.Effect_2,
                                                                       GetRandomPosition(Mob_.Position, 1), 0, -1)
                                MobSetAttackingFromPlayer(Index_, NewUniqueId, True)
                            Next
                        End If

                        If RefSkill.Effect_5 <> 0 Then
                            For i = 1 To RefSkill.Effect_8
                                Dim NewUniqueId As UInteger = SpawnMob(RefSkill.Effect_5, RefSkill.Effect_6,
                                                                       GetRandomPosition(Mob_.Position, 1), 0, -1)
                                MobSetAttackingFromPlayer(Index_, NewUniqueId, True)
                            Next
                        End If

                        If RefSkill.Effect_9 <> 0 Then
                            For i = 1 To RefSkill.Effect_12
                                Dim NewUniqueId As UInteger = SpawnMob(RefSkill.Effect_9, RefSkill.Effect_10,
                                                                       GetRandomPosition(Mob_.Position, 1), 0, -1)
                                MobSetAttackingFromPlayer(Index_, NewUniqueId, True)
                            Next
                        End If

                        If RefSkill.Effect_13 <> 0 Then
                            For i = 1 To RefSkill.Effect_16
                                Dim NewUniqueId As UInteger = SpawnMob(RefSkill.Effect_13, RefSkill.Effect_14,
                                                                       GetRandomPosition(Mob_.Position, 1), 0, -1)
                                MobSetAttackingFromPlayer(Index_, NewUniqueId, True)
                            Next
                        End If
                    End If
            End Select
        End Sub

        Public Sub MobAddDamageFromPlayer(ByVal Damage As UInt32, ByVal Index_ As Integer, ByVal MobUniqueID As UInteger,
                                          ByVal AttackingAllowed As Boolean)
            Dim found As Boolean = False

            'Search for an exits Entery
            For i = 0 To MobList(MobUniqueID).DamageFromPlayer.Count - 1
                If MobList(MobUniqueID).DamageFromPlayer(i).PlayerIndex = Index_ Then
                    found = True
                    MobList(MobUniqueID).DamageFromPlayer(i).Damage += Damage
                    MobList(MobUniqueID).DamageFromPlayer(i).AttackingAllowed = AttackingAllowed
                    Exit For
                End If
            Next

            If found = False Then
                'Then we must add
                Dim tmp As New cDamageDone
                tmp.PlayerIndex = Index_
                tmp.Damage = Damage
                tmp.AttackingAllowed = AttackingAllowed
                MobList(MobUniqueID).DamageFromPlayer.Add(tmp)
            End If
        End Sub

        Public Sub MobSetAttackingFromPlayer(ByVal Index_ As Integer, ByVal MobUniqueID As UInteger,
                                             ByVal AttackingAllowed As Boolean)
            Dim found As Boolean = False

            If MobList.ContainsKey(MobUniqueID) Then
                'Search for an exits Entery
                For i = 0 To MobList(MobUniqueID).DamageFromPlayer.Count - 1
                    If MobList(MobUniqueID).DamageFromPlayer(i).PlayerIndex = Index_ Then
                        found = True
                        MobList(MobUniqueID).DamageFromPlayer(i).AttackingAllowed = AttackingAllowed
                        Exit For
                    End If
                Next

                If found = False Then
                    'Then we must add
                    Dim tmp As New cDamageDone
                    tmp.PlayerIndex = Index_
                    tmp.Damage = 0
                    tmp.AttackingAllowed = AttackingAllowed
                    MobList(MobUniqueID).DamageFromPlayer.Add(tmp)
                End If
            End If
        End Sub
    End Module
End Namespace
