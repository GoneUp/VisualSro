Namespace GameServer.Functions
    Module MonsterAttack

        Dim Random As New Random

        Public Sub MonsterAttack(ByVal MyUniqueID As UInteger, ByVal AttackedID As UInteger)
            Dim MobIndex As Integer = 0

            For i = 0 To MobList.Count - 1
                If MobList(i).UniqueID = MyUniqueID Then
                    MobIndex = i
                    Exit For
                End If
            Next

            'Find others Index
            For i = 0 To PlayerData.Count - 1
                If PlayerData(i).UniqueId = AttackedID Then
                    MonsterAttackPlayer(MobIndex, i)
                    Exit Sub
                End If
            Next
        End Sub


        Public Sub MonsterAttackPlayer(ByVal MobIndex As Integer, ByVal Index_ As Integer)
            MobList(MobIndex).AttackingId = PlayerData(Index_).UniqueId
            MobList(MobIndex).UsingSkillId = Monster_GetNextSkill(MobList(MobIndex).UsingSkillId, MobList(MobIndex).Pk2ID)

            Dim NumberAttack = 1, NumberVictims = 1, AttackType, afterstate As UInteger
            Dim RefWeapon As New cItem
            Dim AttObject As Object_ = GetObjectById(PlayerData(Index_).Model)
            Dim RefSkill As Skill_ = GetSkillById(MobList(MobIndex).UsingSkillId)

            Dim Distance As Double = CalculateDistance(PlayerData(Index_).Position, MobList(MobIndex).Position)
            If Distance >= (RefSkill.Distance) Then
                MoveMob(MobIndex, PlayerData(Index_).Position)
                Exit Sub
            End If


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Reply)
            writer.Byte(1)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)


            writer.Create(ServerOpcodes.Attack_Main)
            writer.Byte(1)
            writer.Byte(2)
            writer.Byte(&H30)

            writer.DWord(AttackType)
            writer.DWord(MobList(MobIndex).UniqueID)
            writer.DWord(CUInt(Random.Next(1000, 10000) + MobList(MobIndex).UniqueID))
            writer.DWord(PlayerData(Index_).UniqueId)

            writer.Byte(1)
            writer.Byte(NumberAttack)
            writer.Byte(NumberVictims) '1 victim

            For d = 0 To NumberVictims - 1
                writer.DWord(PlayerData(Index_).UniqueId)

                For i = 0 To NumberAttack - 1
                    Dim Damage As UInteger = CalculateDamageMob(Index_, AttObject, AttackType)
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
            Server.SendIfMobIsSpawned(writer.GetBytes, MobList(MobIndex).UniqueID)


            If afterstate = &H80 Then
                KillPlayer(Index_)
            End If
        End Sub

        Public Sub MonsterAttackObject(ByVal MobIndex As Integer, ByVal OtherUniqueID As Integer)


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
            Dim RefWeapon As New cItem
            Dim RefSkill As Skill_ = GetSkillById(SkillID)
            Dim FinalDamage As UInteger
            Dim Balance As Double
            If (CSng(Mob.Level) - PlayerData(Index_).Level) > -10 Then
                Balance = (1 + (CSng(Mob.Level) - PlayerData(Index_).Level) / 10)
            Else
                Balance = 0.01
            End If



            Dim DamageMin As Double
            Dim DamageMax As Double

            If RefSkill.Type = TypeTable.Phy Then
                DamageMin = ((RefSkill.PwrMin) * (1 + 0) / (1 + 0) - Mob.PhyDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
                DamageMax = ((PlayerData(Index_).MaxPhy + RefSkill.PwrMax) * (1 + 0) / (1 + 0) - Mob.PhyDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
            ElseIf RefSkill.Type = TypeTable.Mag Then
                DamageMin = ((PlayerData(Index_).MinMag + RefSkill.PwrMin) * (1 + 0) / (1 + 0) - Mob.MagDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
                DamageMax = ((PlayerData(Index_).MaxMag + RefSkill.PwrMax) * (1 + 0) / (1 + 0) - Mob.MagDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
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



        Public Sub MobAddDamageFromPlayer(ByVal Damage As UInt32, ByVal Index_ As Integer, ByVal MobListIndex As UInteger, ByVal PlayerIsAttacking As Boolean)
            Dim found As Boolean = False

            'Search for an exits Entery
            For i = 0 To MobList(MobListIndex).DamageFromPlayer.Count - 1
                If MobList(MobListIndex).DamageFromPlayer(i).PlayerIndex = Index_ Then
                    found = True
                    MobList(MobListIndex).DamageFromPlayer(i).Damage += Damage
                    MobList(MobListIndex).DamageFromPlayer(i).Attacking = PlayerIsAttacking
                    Exit For
                End If
            Next

            If found = False Then
                'Then we must add
                Dim tmp As New cDamageDone
                tmp.PlayerIndex = Index_
                tmp.Damage = Damage
                tmp.Attacking = PlayerIsAttacking
                MobList(MobListIndex).DamageFromPlayer.Add(tmp)
            End If
        End Sub

        Public Sub MobSetAttackingFromPlayer(ByVal Index_ As Integer, ByVal MobUniqueID As UInteger, ByVal PlayerIsAttacking As Boolean)
            Dim found As Boolean = False
            Dim MobListIndex As Integer = GetMobIndex(MobUniqueID)

            If MobListIndex <> -1 Then
                'Search for an exits Entery
                For i = 0 To MobList(MobListIndex).DamageFromPlayer.Count - 1
                    If MobList(MobListIndex).DamageFromPlayer(i).PlayerIndex = Index_ Then
                        found = True
                        MobList(MobListIndex).DamageFromPlayer(i).Attacking = PlayerIsAttacking
                        Exit For
                    End If
                Next

                If found = False Then
                    'Then we must add
                    Dim tmp As New cDamageDone
                    tmp.PlayerIndex = Index_
                    tmp.Damage = 0
                    tmp.Attacking = PlayerIsAttacking
                    MobList(MobListIndex).DamageFromPlayer.Add(tmp)
                End If
            End If
        End Sub
    End Module
End Namespace
