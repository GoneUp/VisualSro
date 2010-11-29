﻿Namespace GameServer.Functions
    Module PlayerAttack

        Dim Random As New Random

        Public Sub OnPlayerAttack(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim found As Boolean = False
            Dim type1 As Byte = packet.Byte

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Reply)

            Select Case type1
                Case 1
                    'Attacking
                    Dim type2 As Byte = packet.Byte '1= punch -- 4 = skill


                    If type1 = 1 And type2 = 1 And PlayerData(Index_).Attacking = False Then
                        packet.Byte()
                        Dim ObjectID As UInt32 = packet.DWord


                        For i = 0 To MobList.Count - 1
                            If MobList(i).UniqueID = ObjectID And MobList(i).Death = False Then
                                writer.Byte(1)
                                writer.Byte(1)
                                Server.Send(writer.GetBytes, Index_)

                                PlayerAttackNormal(Index_, ObjectID)
                                found = True
                                Exit For
                            End If
                        Next
                    End If

                    If found = False Then
                        writer.Byte(2)
                        writer.Byte(0)
                        Server.Send(writer.GetBytes, Index_)
                    End If
                Case 2
                    'Attack Abort
                    writer.Byte(2)
                    writer.Byte(0)
                    Server.Send(writer.GetBytes, Index_)

                    PlayerData(Index_).Attacking = False
                    PlayerData(Index_).Busy = False
                    PlayerData(Index_).AttackedMonsterID = 0
                    PlayerData(Index_).AttackSkill = 0
                    PlayerData(Index_).AttackType = AttackType_.Normal
                    PlayerAttackTimer(Index_).Stop()
            End Select

        End Sub

        Public Sub PlayerAttackNormal(ByVal Index_ As Integer, ByVal ObjectID As UInteger)
            Dim NumberAttack = 1, NumberVictims = 1, AttackType, MobListIndex, afterstate As UInteger
            Dim RefWeapon As cItem = GetItemByID(Inventorys(Index_).UserItems(6).Pk2Id)
            Dim AttObject As New Object_

            For i = 0 To MobList.Count - 1
                If MobList(i).UniqueID = ObjectID Then
                    AttObject = GetObjectById(MobList(i).Pk2ID)
                    MobListIndex = i
                End If
            Next

            If AttObject.Name Is Nothing Then
                Exit Sub
            End If


            Select Case RefWeapon.CLASS_C
                Case 0
                    NumberAttack = 2
                    AttackType = 2
                Case 2
                    NumberAttack = 2
                    AttackType = 2
                Case 3
                    NumberAttack = 2
                    AttackType = 2
                Case 4
                    AttackType = 40
                Case 5
                    AttackType = 53
                Case 6
                    AttackType = 70
            End Select

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Main)
            writer.Byte(1)
            writer.Byte(2)

            writer.DWord(AttackType)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.DWord(CUInt(Random.Next(1000, 10000) + PlayerData(Index_).UniqueId))
            writer.DWord(ObjectID)

            writer.Byte(1)
            writer.Byte(NumberAttack)
            writer.Byte(NumberVictims) '1 victim

            For d = 0 To NumberVictims - 1
                writer.DWord(ObjectID)

                For i = 0 To NumberAttack - 1
                    Dim Damage As UInteger = CalculateDamageMob(Index_, AttObject, AttackType)
                    Dim Crit As Byte = GetCritical()

                    If Crit = True Then
                        Damage = Damage * 2
                        Crit = 2
                    End If

                    If MobList(MobListIndex).HP_Cur - Damage > 0 Then
                        MobList(MobListIndex).HP_Cur -= Damage
                        AddDamageFromPlayer(Damage, Index_, MobListIndex)
                    ElseIf MobList(MobListIndex).HP_Cur - Damage <= 0 Then
                        'Dead
                        afterstate = &H80
                        AddDamageFromPlayer(Damage, Index_, MobListIndex)
                        MobList(MobListIndex).HP_Cur = 0
                    End If

                    writer.Byte(afterstate)
                    writer.Byte(Crit)
                    writer.DWord(Damage)
                    writer.Byte(0)
                    writer.Word(0)
                Next
            Next
            Server.SendToAllInRange(writer.GetBytes, PlayerData(Index_).Position)

            If afterstate = &H80 Then
                KillMob(MobListIndex)
                UpdateState(0, 2, Index_, MobListIndex)
                SendAttackEnd(Index_)
            Else
                PlayerData(Index_).Attacking = True
                PlayerData(Index_).Busy = True
                PlayerData(Index_).AttackedMonsterID = ObjectID
                PlayerData(Index_).AttackSkill = AttackType
                PlayerData(Index_).AttackType = AttackType_.Normal
                PlayerAttackTimer(Index_).Stop()
                PlayerAttackTimer(Index_).Interval = 2500
                PlayerAttackTimer(Index_).Start()
            End If

        End Sub

        Public Sub PlayerAttackSkill(ByVal SkillID As UInt32)



        End Sub


        Function CalculateDamageMob(ByVal Index_ As Integer, ByVal Mob As Object_, ByVal SkillID As UInt32) As UInteger
            Dim RefWeapon As cItem = GetItemByID(Inventorys(Index_).UserItems(6).Pk2Id)
            Dim RefSkill As Skill_ = GetSkillById(SkillID)
            Dim FinalDamage As UInteger
            Dim Balance As Double = (1 + (PlayerData(Index_).Level - Mob.Level))

            Dim DamageMin As Double
            Dim DamageMax As Double

            If RefSkill.Type = TypeTable.Phy Then
                DamageMin = ((PlayerData(Index_).MinPhy + RefSkill.PwrMin) * (1 + 0) / (1 + 0) - Mob.PhyDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
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

        Private Function SendAttackEnd(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Reply)
            writer.Byte(2)
            writer.Byte(0)
            Server.Send(writer.GetBytes, Index_)
        End Function

        Private Function GetCritical() As Boolean
            If Math.Round(Rnd() * 5) = 5 Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Sub AddDamageFromPlayer(ByVal Damage As UInt32, ByVal Index_ As Integer, ByVal MobListIndex As UInteger)
            Dim found As Boolean = False
            Dim d = MobList(MobListIndex)

            'Search for an exits Entery
            For i = 0 To MobList(MobListIndex).DamageFromPlayer.Count - 1
                If MobList(MobListIndex).DamageFromPlayer(i).PlayerIndex = Index_ Then
                    found = True
                    MobList(MobListIndex).DamageFromPlayer(i).Damage += Damage
                    Exit For
                End If
            Next

            If found = False Then
                'Then we must add
                Dim tmp As New cDamageDone
                tmp.PlayerIndex = Index_
                tmp.Damage = Damage
                MobList(MobListIndex).DamageFromPlayer.Add(tmp)
            End If
        End Sub
    End Module
End Namespace