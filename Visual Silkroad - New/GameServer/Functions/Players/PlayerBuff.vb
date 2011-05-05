Namespace GameServer.Functions
    Module PlayerBuff

        Public Sub PlayerBuff_BeginnCasting(ByVal SkillID As UInt32, ByVal Index_ As Integer)
            Dim RefSkill As Skill_ = GetSkillById(SkillID)
            Dim RefWeapon As New cItem

            If PlayerData(Index_).Busy Or CheckIfUserOwnSkill(SkillID, Index_) = False Or PlayerData(Index_).CastingId <> 0 Then
                Exit Sub
            End If


            If CInt(PlayerData(Index_).CMP) - RefSkill.RequiredMp < 0 Then
                'Not enough MP
                Attack_SendNotEnoughMP(Index_)
                Exit Sub
            Else
                PlayerData(Index_).CMP -= RefSkill.RequiredMp
                UpdateMP(Index_)
            End If

            Dim tmp As New cBuff
            tmp.OverID = Id_Gen.GetSkillOverId
            tmp.CastingId = Id_Gen.GetCastingId
            tmp.SkillID = SkillID
            tmp.OwnerID = PlayerData(Index_).UniqueId
            tmp.DurationStart = Date.Now
            tmp.DurationEnd = Date.Now.AddSeconds(RefSkill.UseDuration)
            tmp.Type = BuffType_.SkillBuff

            PlayerData(Index_).Busy = True
            PlayerData(Index_).Attacking = False
            PlayerData(Index_).AttackType = AttackType_.Buff
            PlayerData(Index_).AttackedId = 0
            PlayerData(Index_).UsingSkillId = SkillID
            PlayerData(Index_).SkillOverId = tmp.OverID
            PlayerData(Index_).CastingId = tmp.CastingId

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_Reply)
            writer.Byte(1)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.Attack_Main)
            writer.Byte(1)
            writer.Byte(0)
            writer.Byte(&H30)

            writer.DWord(PlayerData(Index_).UsingSkillId)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.DWord(PlayerData(Index_).CastingId)
            writer.DWord(0)
            writer.Byte(0)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            AddBuffToList(RefSkill, tmp, Index_)


            If RefSkill.CastTime > 0 Then
                PlayerAttackTimer(Index_).Interval = RefSkill.CastTime
                PlayerAttackTimer(Index_).Start()
            Else
                PlayerBuff_EndCasting(Index_)
            End If
        End Sub

        Public Sub PlayerBuff_EndCasting(ByVal Index_ As Integer)
            PlayerBuff_Info(Index_, 0)
            PlayerBuff_Iconpacket(Index_)
            Attack_SendAttackEnd(Index_)

           
            'Clean Up
            PlayerData(Index_).Busy = False
            PlayerData(Index_).Attacking = False
            PlayerData(Index_).AttackType = AttackType_.Normal
            PlayerData(Index_).AttackedId = 0
            PlayerData(Index_).UsingSkillId = 0
            PlayerData(Index_).SkillOverId = 0
            PlayerData(Index_).CastingId = 0
        End Sub


        Public Sub PlayerBuff_Info(ByVal Index_ As Integer, ByVal Type As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Buff_Info)
            writer.Byte(1)
            writer.DWord(PlayerData(Index_).CastingId)
            writer.Byte(Type)
            If Type = 0 Then
                writer.DWord(0)
            End If
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub

        Public Sub PlayerBuff_Iconpacket(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Buff_Icon)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.DWord(PlayerData(Index_).UsingSkillId)
            writer.DWord(PlayerData(Index_).SkillOverId)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub


        Public Sub PlayerBuff_End(ByVal SkillOverId As UInteger, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Buff_End)
            writer.Byte(1)
            writer.DWord(PlayerData(Index_).Buffs(SkillOverId).OverID)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            RemoveBuffFromList(SkillOverId, Index_)
        End Sub


        Private Sub AddBuffToList(ByVal refskill As Skill_, ByVal buff As cBuff, ByVal Index_ As Integer)
            PlayerData(Index_).Buffs.Add(buff.OverID, buff)
            PlayerData(Index_).Buffs(buff.OverID).ElaspedTimer_Start(refskill.UseDuration)

            PlayerData(Index_).SetCharGroundStats()
            PlayerData(Index_).AddItemsToStats(Index_)
            PlayerData(Index_).AddBuffsToStats()
            OnStatsPacket(Index_)
        End Sub

        Private Sub RemoveBuffFromList(ByVal SkillOverId As UInteger, ByVal Index_ As Integer)
            PlayerData(Index_).Buffs(SkillOverId).Disponse()
            PlayerData(Index_).Buffs.Remove(SkillOverId)

            PlayerData(Index_).SetCharGroundStats()
            PlayerData(Index_).AddItemsToStats(Index_)
            PlayerData(Index_).AddBuffsToStats()
            OnStatsPacket(Index_)
        End Sub
    End Module
End Namespace
