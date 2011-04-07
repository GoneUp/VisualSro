Namespace GameServer.Functions
    Module PlayerBuff

        Public Sub PlayerBuff_Beginn(ByVal SkillID As UInt32, ByVal Index_ As Integer)
            Dim RefSkill As Skill_ = GetSkillById(SkillID)
            Dim RefWeapon As New cItem

            If PlayerData(Index_).Busy Or CheckIfUserOwnSkill(SkillID, Index_) = False Then
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
            tmp.SkillID = SkillID
            tmp.OwnerID = PlayerData(Index_).UniqueId
            tmp.DurationStart = Date.Now
            tmp.DurationEnd = Date.Now.AddSeconds(RefSkill.Time)

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
            writer.DWord(0)
            writer.Byte(0)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = False
            PlayerData(Index_).Attacking = False
            PlayerData(Index_).AttackType = AttackType_.Buff
            PlayerData(Index_).AttackedId = 0
            PlayerData(Index_).UsingSkillId = SkillID
            PlayerData(Index_).SkillOverId = Id_Gen.GetSkillOverId
            AddBuffToList(tmp, Index_)


            If RefSkill.CastTime > 0 Then
                PlayerAttackTimer(Index_).Interval = RefSkill.CastTime * 1000
                PlayerAttackTimer(Index_).Start()
            Else
                PlayerBuff_End(Index_)
            End If
        End Sub

        Public Sub PlayerBuff_End(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Attack_End)
            writer.Byte(1)
            writer.DWord(PlayerData(Index_).SkillOverId)
            writer.DWord(0)
            writer.Byte(0)

            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

            Attack_SendAttackEnd(Index_)
            RemoveBuffToList(PlayerData(Index_).SkillOverId, Index_)
        End Sub

        Private Sub AddBuffToList(ByVal buff As cBuff, ByVal Index_ As Integer)
            PlayerData(Index_).Buffs.Add(buff)
        End Sub

        Private Sub RemoveBuffToList(ByVal SkillOverId As UInteger, ByVal Index_ As Integer)
            For i = 0 To PlayerData(Index_).Buffs.Count - 1
                If PlayerData(Index_).Buffs(i).OverID = SkillOverId Then
                    PlayerData(Index_).Buffs.Remove(PlayerData(Index_).Buffs(i))
                End If
            Next
        End Sub
    End Module
End Namespace
