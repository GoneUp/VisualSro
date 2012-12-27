Imports SRFramework

Namespace Functions
    Module PlayerBuff
        Public Sub PlayerBuff_BeginnCasting(ByVal skillID As UInt32, ByVal Index_ As Integer)
            Dim refSkill As RefSkill = GetSkill(skillID)
            Dim refWeapon As New cRefItem

            If _
                PlayerData(Index_).Busy Or CheckIfUserOwnSkill(skillID, Index_) = False Or
                PlayerData(Index_).CastingId <> 0 Then
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

            Dim tmp As New cBuff
            tmp.OverID = Id_Gen.GetSkillOverId
            tmp.CastingId = Id_Gen.GetCastingId
            tmp.SkillID = skillID
            tmp.OwnerID = PlayerData(Index_).UniqueID
            tmp.DurationStart = Date.Now
            tmp.DurationEnd = Date.Now.AddSeconds(refSkill.UseDuration)
            tmp.Type = BuffType.SkillBuff

            PlayerData(Index_).Busy = True
            PlayerData(Index_).Attacking = False
            PlayerData(Index_).AttackType = AttackTypes.Buff
            PlayerData(Index_).AttackedId = 0
            PlayerData(Index_).UsingSkillId = skillID
            PlayerData(Index_).SkillOverId = tmp.OverID
            PlayerData(Index_).CastingId = tmp.CastingId

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ATTACK_REPLY)
            writer.Byte(1)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.GAME_ATTACK_MAIN)
            writer.Byte(1)
            writer.Byte(2) '0=end packet,2=direct end
            writer.Byte(&H30)

            writer.DWord(PlayerData(Index_).UsingSkillId)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.DWord(PlayerData(Index_).CastingId)
            writer.DWord(0)
            writer.Byte(0)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            AddBuffToList(refSkill, tmp, Index_)


            If refSkill.CastTime > 0 Then
                PlayerAttackTimer(Index_).Interval = refSkill.CastTime
                PlayerAttackTimer(Index_).Start()
            Else
                PlayerBuffEndCasting(Index_)
            End If
        End Sub

        Public Sub PlayerBuffEndCasting(ByVal Index_ As Integer)
            PlayerBuffInfo(Index_, 0)
            PlayerBuffIconpacket(Index_)
            AttackSendAttackEnd(Index_)


            'Clean Up
            PlayerData(Index_).Busy = False
            PlayerData(Index_).Attacking = False
            PlayerData(Index_).AttackType = AttackTypes.Normal
            PlayerData(Index_).AttackedId = 0
            PlayerData(Index_).UsingSkillId = 0
            PlayerData(Index_).SkillOverId = 0
            PlayerData(Index_).CastingId = 0
        End Sub


        Public Sub PlayerBuffInfo(ByVal Index_ As Integer, ByVal type As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_BUFF_INFO)
            writer.Byte(1)
            writer.DWord(PlayerData(Index_).CastingId)
            writer.Byte(type)
            If type = 0 Then
                writer.DWord(0)
            End If
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub

        Public Sub PlayerBuffIconpacket(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_BUFF_ICON)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.DWord(PlayerData(Index_).UsingSkillId)
            writer.DWord(PlayerData(Index_).SkillOverId)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub


        Public Sub PlayerBuffEnd(ByVal skillOverId As UInteger, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_BUFF_END)
            writer.Byte(1)
            writer.DWord(PlayerData(Index_).Buffs(skillOverId).OverID)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            RemoveBuffFromList(skillOverId, Index_)
        End Sub


        Private Sub AddBuffToList(ByVal refskill As RefSkill, ByVal buff As cBuff, ByVal Index_ As Integer)
            PlayerData(Index_).Buffs.Add(buff.OverID, buff)
            PlayerData(Index_).Buffs(buff.OverID).ElaspedTimerStart(refskill.UseDuration)

            PlayerData(Index_).SetCharGroundStats()
            PlayerData(Index_).AddItemsToStats(Index_)
            PlayerData(Index_).AddBuffsToStats()
            OnStatsPacket(Index_)
        End Sub

        Private Sub RemoveBuffFromList(ByVal skillOverId As UInteger, ByVal Index_ As Integer)
            PlayerData(Index_).Buffs(SkillOverId).Disponse()
            PlayerData(Index_).Buffs.Remove(SkillOverId)

            PlayerData(Index_).SetCharGroundStats()
            PlayerData(Index_).AddItemsToStats(Index_)
            PlayerData(Index_).AddBuffsToStats()
            OnStatsPacket(Index_)
        End Sub
    End Module
End Namespace
