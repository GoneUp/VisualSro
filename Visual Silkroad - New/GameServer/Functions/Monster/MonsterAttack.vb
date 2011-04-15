Namespace GameServer.Functions
    Module MonsterAttack

        Dim Random As New Random

        Public Sub MonsterAttackPlayer(ByVal MobUniqueID As Integer, ByVal Index_ As Integer, Optional ByVal SkillID As UInteger = 0)
            If MobList1.ContainsKey(MobUniqueID) = False Then
                Exit Sub
            End If

            Dim Mob_ As cMonster = MobList1(MobUniqueID)

            Mob_.AttackingId = PlayerData(Index_).UniqueId
            If SkillID = 0 Then
                Mob_.UsingSkillId = Monster_GetNextSkill(Mob_.UsingSkillId, Mob_.Pk2ID)
            Else
                Mob_.UsingSkillId = SkillID
            End If

            Dim NumberAttack = 1, NumberVictims = 1, afterstate As UInteger
            Dim AttObject As Object_ = GetObjectById(PlayerData(Index_).Model)
            Dim RefMonster As Object_ = GetObjectById(Mob_.Pk2ID)
            Dim RefSkill As Skill_ = GetSkillById(Mob_.UsingSkillId)

            Dim Distance As Double = CalculateDistance(PlayerData(Index_).Position, Mob_.Position)
            If Distance >= (RefSkill.Distance) Then
                MoveMob(MobUniqueID, PlayerData(Index_).Position)
                Exit Sub
            End If


            UpdateState(1, 3, Index_, MobUniqueID)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Main)
            writer.Byte(1)
            writer.Byte(2)
            writer.Byte(&H30)

            writer.DWord(RefSkill.Id)
            writer.DWord(Mob_.UniqueID)
            writer.DWord(Id_Gen.GetSkillOverId)
            writer.DWord(PlayerData(Index_).UniqueId)

            writer.Byte(1)
            writer.Byte(NumberAttack)
            writer.Byte(NumberVictims) '1 victim

            For d = 0 To NumberVictims - 1
                writer.DWord(PlayerData(Index_).UniqueId)

                For i = 0 To NumberAttack - 1
                    Dim Damage As UInteger = CalculateDamagePlayer(Index_, RefMonster, RefSkill.Id)
                    Dim Crit As Byte = Attack_GetCritical()

                    If Crit = True Then
                        Damage = Damage * 2
                        Crit = 2
                    End If

                    If CLng(PlayerData(Index_).CHP) - Damage > 0 Then
                        PlayerData(Index_).CHP -= Damage
                    ElseIf CLng(PlayerData(Index_).CHP) - Damage <= 0 Then
                        'Dead
                        afterstate = &H80
                        PlayerData(Index_).CHP = 0
                    End If

                    writer.Byte(afterstate)
                    writer.Byte(Crit)
                    writer.DWord(Damage)
                    writer.Byte(0)
                    writer.Word(0)
                Next
            Next
            Server.SendIfMobIsSpawned(writer.GetBytes, Mob_.UniqueID)


            If afterstate = &H80 Then
                KillPlayer(Index_)
                MobSetAttackingFromPlayer(Index_, Mob_.UniqueID, False)
            Else
                UpdateHP(Index_)

                If MobList1(MobUniqueID).GetsAttacked Then
                    MonsterAttackPlayer(MobUniqueID, MobGetPlayerWithMostDamage(MobUniqueID, True))
                End If
            End If
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
        Function CalculateDamagePlayer(ByVal Index_ As Integer, ByVal Mob As Object_, ByVal SkillID As UInt32) As UInteger
            Dim RefSkill As Skill_ = GetSkillById(SkillID)
            Dim FinalDamage As UInteger
            Dim Balance As Double
            If (CSng(Mob.Level) - PlayerData(Index_).Level) > -10 Then
                Balance = (1 + (CSng(Mob.Level) - PlayerData(Index_).Level) / 100)
            Else
                Balance = 0.01
            End If

            Dim DamageMin As Double
            Dim DamageMax As Double

            If RefSkill.Type = TypeTable.Phy Then
                DamageMin = ((Mob.ParryRatio + RefSkill.PwrMin) * (1 + 0) / (1 + (PlayerData(Index_).PhyAbs / 1000)) - PlayerData(Index_).PhyDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 10)
                DamageMax = ((Mob.ParryRatio + RefSkill.PwrMax) * (1 + 0) / (1 + (PlayerData(Index_).PhyAbs / 1000)) - PlayerData(Index_).PhyDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 10)
            ElseIf RefSkill.Type = TypeTable.Mag Then
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
            Dim Ref_ As Object_ = GetObjectById(ObjectID)


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
                        Return Ref_.Skill3
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



        Public Sub MobAddDamageFromPlayer(ByVal Damage As UInt32, ByVal Index_ As Integer, ByVal MobUniqueID As UInteger, ByVal PlayerIsAttacking As Boolean)
            Dim found As Boolean = False

            'Search for an exits Entery
            For i = 0 To MobList1(MobUniqueID).DamageFromPlayer.Count - 1
                If MobList1(MobUniqueID).DamageFromPlayer(i).PlayerIndex = Index_ Then
                    found = True
                    MobList1(MobUniqueID).DamageFromPlayer(i).Damage += Damage
                    MobList1(MobUniqueID).DamageFromPlayer(i).Attacking = PlayerIsAttacking
                    Exit For
                End If
            Next

            If found = False Then
                'Then we must add
                Dim tmp As New cDamageDone
                tmp.PlayerIndex = Index_
                tmp.Damage = Damage
                tmp.Attacking = PlayerIsAttacking
                MobList1(MobUniqueID).DamageFromPlayer.Add(tmp)
            End If
        End Sub

        Public Sub MobSetAttackingFromPlayer(ByVal Index_ As Integer, ByVal MobUniqueID As UInteger, ByVal PlayerIsAttacking As Boolean)
            Dim found As Boolean = False

            If MobList1.ContainsKey(MobUniqueID) Then
                'Search for an exits Entery
                For i = 0 To MobList1(MobUniqueID).DamageFromPlayer.Count - 1
                    If MobList1(MobUniqueID).DamageFromPlayer(i).PlayerIndex = Index_ Then
                        found = True
                        MobList1(MobUniqueID).DamageFromPlayer(i).Attacking = PlayerIsAttacking
                        Exit For
                    End If
                Next

                If found = False Then
                    'Then we must add
                    Dim tmp As New cDamageDone
                    tmp.PlayerIndex = Index_
                    tmp.Damage = 0
                    tmp.Attacking = PlayerIsAttacking
                    MobList1(MobUniqueID).DamageFromPlayer.Add(tmp)
                End If
            End If
        End Sub
    End Module
End Namespace
