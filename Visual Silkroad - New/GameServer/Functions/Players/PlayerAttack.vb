Namespace GameServer.Functions
    Module PlayerAttack

        Dim Random As New Random

        Public Sub OnPlayerAttack(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim found As Boolean = False
            Dim type1 As Byte = packet.Byte
            Dim type2 As Byte = packet.Byte '1= punch -- 4 = skill

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Reply)

            If type1 = 1 And type2 = 1 Then
                packet.Byte()
                Dim ObjectID As UInt32 = packet.DWord


                For i = 0 To MobList.Count - 1
                    If MobList(i).UniqueID = ObjectID Then
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

            Select Case RefWeapon.CLASS_C
                Case 0
                Case 2
                Case 3
                    NumberAttack = 2
                    AttackType = 2

                Case 4
                Case 5
                    AttackType = 40
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
                PlayerData(Index_).AttackedDeathMonsterID = ObjectID
                MonsterDeath(Index_).Interval = 3000
                MonsterDeath(Index_).Start()

                UpdateState(0, 2, Index_, MobListIndex)
                SendAttackEnd(Index_)
            End If

        End Sub

        Public Sub PlayerAttackSkill(ByVal SkillID As UInt32)



        End Sub


        Function CalculateDamageMob(ByVal Index_ As Integer, ByVal Mob As Object_, ByVal SkillID As UInt32) As UInteger
            Dim RefWeapon As cItem = GetItemByID(Inventorys(Index_).UserItems(6).Pk2Id)
            Dim RefSkill As Skill_ = GetSkillById(SkillID)
            Dim FinalDamage As UInteger
            Dim Balance As Double = 1 + ((PlayerData(Index_).Level - Mob.Level) / 100)

            If RefSkill.Type = TypeTable.Phy Then
                Dim DamageMin As Double = ((PlayerData(Index_).MinPhy + RefSkill.PwrMin) * (1 + 0) / (1 + 0) - Mob.PhyDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)
                Dim DamageMax As Double = ((PlayerData(Index_).MaxPhy + RefSkill.PwrMax) * (1 + 0) / (1 + 0) - Mob.PhyDef) * Balance * (1 + 0) * (RefSkill.PwrPercent / 100)

                Dim Radmon As Integer = Rnd() * 100
                FinalDamage = DamageMin + (((DamageMax - DamageMin) / 100) * Radmon)

            ElseIf RefSkill.Type = TypeTable.Mag Then
                Dim DamageMin As Double = ((PlayerData(Index_).MinMag + RefSkill.PwrMin) * (1 + 0) / (1 + 0) - Mob.MagDef) * Balance * (1 + 0) * RefSkill.PwrPercent
                Dim DamageMax As Double = ((PlayerData(Index_).MaxMag + RefSkill.PwrMax) * (1 + 0) / (1 + 0) - Mob.MagDef) * Balance * (1 + 0) * RefSkill.PwrPercent

                Dim Radmon As Integer = Rnd() * 100
                FinalDamage = DamageMin + (((DamageMax - DamageMin) / 100) * Radmon)
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