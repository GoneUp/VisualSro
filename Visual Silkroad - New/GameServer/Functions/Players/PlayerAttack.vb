Namespace GameServer.Functions
    Module PlayerAttack

        Dim Random As New Random

        Public Sub OnPlayerAttack(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim found As Boolean = False
            Dim type1 As Byte = packet.Byte

            If type1 = 1 Then
                'Attacking
                Select Case packet.Byte
                    Case 1
                        'Normal Attack
                        packet.Skip(1)
                        Dim ObjectID As UInt32 = packet.DWord
                        If PlayerData(Index_).Attacking = False Then
                            For i = 0 To MobList.Count - 1
                                If MobList(i).UniqueID = ObjectID And MobList(i).Death = False Then
                                    PlayerAttackNormal(Index_, i)
                                    found = True
                                    Exit For
                                End If
                            Next
                        End If
                    Case 2
                        'Pickup
                        packet.Skip(1)
                        Dim ObjectID As UInt32 = packet.DWord

                        For i = 0 To ItemList.Count - 1
                            If ItemList(i).UniqueID = ObjectID Then
                                PickUp(i, Index_)
                            End If
                        Next
                    Case 4
                        Dim skillid As UInteger = packet.DWord
                        Dim type As Byte = packet.Byte() 'Type = 1--> Monster Attack --- Type = 0 --> Buff

                        If type = 1 Then
                            Dim ObjectID As UInt32 = packet.DWord

                            If PlayerData(Index_).Attacking = False Then
                                For i = 0 To MobList.Count - 1
                                    If MobList(i).UniqueID = ObjectID And MobList(i).Death = False Then
                                        PlayerAttackBeginSkill(skillid, Index_, i)
                                        found = True
                                        Exit For
                                    End If
                                Next
                            Else
                                If PlayerData(Index_).AttackType = AttackType_.Normal Then
                                    For i = 0 To MobList.Count - 1
                                        If MobList(i).UniqueID = ObjectID And MobList(i).Death = False Then
                                            PlayerAttackBeginSkill(skillid, Index_, i)
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        End If


                End Select


                If found = False Then
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Attack_Reply)
                    writer.Byte(2)
                    writer.Byte(0)
                    Server.Send(writer.GetBytes, Index_)
                End If
            Else
                'Attack Abort
                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Attack_Reply)
                writer.Byte(2)
                writer.Byte(0)
                Server.Send(writer.GetBytes, Index_)

                PlayerData(Index_).Attacking = False
                PlayerData(Index_).Busy = False
                PlayerData(Index_).AttackedId = 0
                PlayerData(Index_).UsingSkillId = 0
                PlayerData(Index_).AttackType = AttackType_.Normal
                PlayerAttackTimer(Index_).Stop()
            End If

        End Sub

        Public Sub PlayerAttackNormal(ByVal Index_ As Integer, ByVal MobListIndex As Integer)
            Dim NumberAttack = 1, NumberVictims = 1, AttackType, afterstate As UInteger
            Dim RefWeapon As New cItem
            Dim AttObject As Object_ = GetObjectById(MobList(MobListIndex).Pk2ID)



            If Inventorys(Index_).UserItems(6).Pk2Id <> 0 Then
                'Weapon
                RefWeapon = GetItemByID(Inventorys(Index_).UserItems(6).Pk2Id)
            Else
                'No Weapon
                RefWeapon.ATTACK_DISTANCE = 36
            End If

            If AttObject.Name Is Nothing Or PlayerData(Index_).Busy Then
                Exit Sub
            End If

            Dim Distance As Double = CalculateDistance(PlayerData(Index_).Position, MobList(MobListIndex).Position)
            If Distance >= (RefWeapon.ATTACK_DISTANCE) Then
                MoveUserToMonster(Index_, MobListIndex)
                Exit Sub
            End If


            Select Case RefWeapon.CLASS_C
                Case 0
                    AttackType = 1
                Case 2
                    NumberAttack = 2
                    AttackType = 2
                Case 3
                    NumberAttack = 2
                    AttackType = 2
                Case 4
                    AttackType = 40
                Case 5
                    AttackType = 40
                Case 6
                    AttackType = 70
                Case 7
                    AttackType = 7127
                Case 8
                    AttackType = 7128
                Case 9
                    NumberAttack = 2
                    AttackType = 7129
                Case 10
                    AttackType = 9069
                Case 11
                    AttackType = 8454
                Case 12
                    AttackType = 7909
                Case 13
                    NumberAttack = 2
                    AttackType = 7910
                Case 14
                    AttackType = 9606
                Case 15
                    AttackType = 9970
            End Select

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
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.DWord(CUInt(Random.Next(1000, 10000) + PlayerData(Index_).UniqueId))
            writer.DWord(MobList(MobListIndex).UniqueID)

            writer.Byte(1)
            writer.Byte(NumberAttack)
            writer.Byte(NumberVictims) '1 victim

            For d = 0 To NumberVictims - 1
                writer.DWord(MobList(MobListIndex).UniqueID)

                For i = 0 To NumberAttack - 1
                    Dim Damage As UInteger = CalculateDamageMob(Index_, AttObject, AttackType)
                    Dim Crit As Byte = GetCritical()

                    If Crit = True Then
                        Damage = Damage * 2
                        Crit = 2
                    End If

                    If CLng(MobList(MobListIndex).HP_Cur) - Damage > 0 Then
                        MobList(MobListIndex).HP_Cur -= Damage
                        AddDamageFromPlayer(Damage, Index_, MobListIndex)
                    ElseIf CLng(MobList(MobListIndex).HP_Cur) - Damage <= 0 Then
                        'Dead
                        afterstate = &H80
                        AddDamageFromPlayer(MobList(MobListIndex).HP_Cur, Index_, MobListIndex) 'Done the last Damage
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
                GetEXPFromMob(MobList(MobListIndex))

                KillMob(MobListIndex)
                SendAttackEnd(Index_)

                PlayerData(Index_).Attacking = False
                PlayerData(Index_).Busy = False
                PlayerData(Index_).AttackType = AttackType_.Normal
                PlayerData(Index_).AttackedId = 0
                PlayerData(Index_).UsingSkillId = 0
                PlayerData(Index_).SkillOverId = 0
            Else
                PlayerData(Index_).Attacking = True
                PlayerData(Index_).Busy = True
                PlayerData(Index_).AttackedId = MobList(MobListIndex).UniqueID
                PlayerData(Index_).UsingSkillId = AttackType
                PlayerData(Index_).AttackType = AttackType_.Normal
                If PlayerAttackTimer(Index_).Enabled = False Then
                    PlayerAttackTimer(Index_).Interval = 2500
                    PlayerAttackTimer(Index_).Start()
                End If
            End If

        End Sub

        Public Sub PlayerAttackBeginSkill(ByVal SkillID As UInt32, ByVal Index_ As Integer, ByVal MobIndex As Integer)
            Dim RefSkill As Skill_ = GetSkillById(SkillID)
            Dim RefWeapon As New cItem

            If PlayerData(Index_).Busy Or CheckIfUserOwnSkill(SkillID, Index_) = False Then
                Exit Sub
            End If


            If CalculateDistance(PlayerData(Index_).Position, MobList(MobIndex).Position) >= RefSkill.Distance Then
                'MoveUserToMonster(Index_, MobIndex)
                'Exit Sub
            End If

            If CInt(PlayerData(Index_).CMP) - RefSkill.RequiredMp < 0 Then
                'Not enough MP
                SendNotEnoughMP(Index_)
                Exit Sub
            Else
                PlayerData(Index_).CMP -= RefSkill.RequiredMp
                UpdateMP(Index_)
            End If

            PlayerData(Index_).AttackedId = MobList(MobIndex).UniqueID
            PlayerData(Index_).Attacking = True
            PlayerData(Index_).UsingSkillId = SkillID
            PlayerData(Index_).AttackType = AttackType_.Skill
            PlayerData(Index_).SkillOverId = Random.Next(1000, 10000) + PlayerData(Index_).UniqueId


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Reply)
            writer.Byte(1)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.Attack_Main)
            writer.Byte(1)
            writer.Byte(2)
            writer.Byte(&H30)

            writer.DWord(PlayerData(Index_).UsingSkillId)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.DWord(PlayerData(Index_).SkillOverId)
            writer.DWord(PlayerData(Index_).AttackedId)
            writer.Byte(0)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            If RefSkill.CastTime > 0 Then
                PlayerAttackTimer(Index_).Interval = RefSkill.CastTime * 1000
                PlayerAttackTimer(Index_).Start()
            Else
                PlayerAttackEndSkill(Index_)
            End If
        End Sub

        Public Sub PlayerAttackEndSkill(ByVal Index_ As Integer)
            Dim RefSkill As Skill_ = GetSkillById(PlayerData(Index_).UsingSkillId)
            Dim AttObject As Object
            Dim RefWeapon As New cItem
            Dim MobListIndex, afterstate, NumberVictims As Integer
            NumberVictims = 1

            If Inventorys(Index_).UserItems(6).Pk2Id <> 0 Then
                'Weapon
                RefWeapon = GetItemByID(Inventorys(Index_).UserItems(6).Pk2Id)
            Else
                'No Weapon
                Exit Sub
            End If

            For i = 0 To MobList.Count - 1
                If MobList(i).UniqueID = PlayerData(Index_).AttackedId Then
                    MobListIndex = i
                    AttObject = GetObjectById(MobList(i).Pk2ID)
                    Exit For
                End If
            Next

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_End)
            writer.Byte(1)

            writer.DWord(PlayerData(Index_).SkillOverId)
            writer.DWord(PlayerData(Index_).AttackedId)

            writer.Byte(1)
            writer.Byte(RefSkill.NumberOfAttacks)
            writer.Byte(NumberVictims) '1 victim

            For d = 0 To NumberVictims - 1
                writer.DWord(PlayerData(Index_).AttackedId)

                For i = 0 To RefSkill.NumberOfAttacks - 1
                    Dim Damage As UInteger = CalculateDamageMob(Index_, AttObject, RefSkill.Id)
                    Dim Crit As Byte = GetCritical()

                    If Crit = True Then
                        Damage = Damage * 2
                        Crit = 2
                    End If

                    If CLng(MobList(MobListIndex).HP_Cur) - Damage > 0 Then
                        MobList(MobListIndex).HP_Cur -= Damage
                        AddDamageFromPlayer(Damage, Index_, MobListIndex)
                    ElseIf CLng(MobList(MobListIndex).HP_Cur) - Damage <= 0 Then
                        'Dead
                        afterstate = &H80
                        AddDamageFromPlayer(MobList(MobListIndex).HP_Cur, Index_, MobListIndex)
                        MobList(MobListIndex).HP_Cur = 0
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
                GetEXPFromMob(MobList(MobListIndex))

                KillMob(MobListIndex)
                SendAttackEnd(Index_)

                PlayerData(Index_).Attacking = False
                PlayerData(Index_).Busy = False
                PlayerData(Index_).AttackType = AttackType_.Normal
                PlayerData(Index_).AttackedId = 0
                PlayerData(Index_).UsingSkillId = 0
                PlayerData(Index_).SkillOverId = 0
            Else
                PlayerData(Index_).Attacking = True
                PlayerData(Index_).Busy = True
                PlayerData(Index_).AttackType = AttackType_.Normal
                If PlayerAttackTimer(Index_).Enabled = False Then
                    PlayerAttackTimer(Index_).Interval = 2500
                    PlayerAttackTimer(Index_).Start()
                End If
            End If
        End Sub


        Function CalculateDamageMob(ByVal Index_ As Integer, ByVal Mob As Object_, ByVal SkillID As UInt32) As UInteger
            Dim RefWeapon As New cItem
            Dim RefSkill As Skill_ = GetSkillById(SkillID)
            Dim FinalDamage As UInteger
            Dim Balance As Double
            If (CSng(PlayerData(Index_).Level) - Mob.Level) > -10 Then
                Balance = (1 + ((CSng(PlayerData(Index_).Level) - Mob.Level) / 10))
            Else
                Balance = 0.01
            End If



            Dim DamageMin As Double
            Dim DamageMax As Double

            If Inventorys(Index_).UserItems(6).Pk2Id <> 0 Then
                'Weapon
                GetItemByID(Inventorys(Index_).UserItems(6).Pk2Id)
            Else
                'No Weapon
            End If

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

        Private Sub SendNotEnoughMP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Reply)
            writer.Byte(3)
            writer.Byte(0)
            writer.Byte(4)
            writer.Byte(&H40)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.Attack_Main)
            writer.Byte(2)
            writer.Byte(4)
            writer.Byte(&H30)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Private Sub SendAttackEnd(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Reply)
            writer.Byte(2)
            writer.Byte(0)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Private Function GetCritical() As Boolean
            If Math.Round(Rnd() * 5) = 5 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub AddDamageFromPlayer(ByVal Damage As UInt32, ByVal Index_ As Integer, ByVal MobListIndex As UInteger)
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