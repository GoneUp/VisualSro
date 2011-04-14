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
            tmp.DurationEnd = Date.Now.AddSeconds(RefSkill.UseDuration)

            PlayerData(Index_).Busy = True
            PlayerData(Index_).Attacking = False
            PlayerData(Index_).AttackType = AttackType_.Buff
            PlayerData(Index_).AttackedId = 0
            PlayerData(Index_).UsingSkillId = SkillID
            PlayerData(Index_).SkillOverId = tmp.OverID
            PlayerData(Index_).CastingId = Id_Gen.GetCastingId

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


            AddBuffToList(tmp, Index_)


            If RefSkill.CastTime > 0 Then
                PlayerAttackTimer(Index_).Interval = RefSkill.CastTime * 1000
                PlayerAttackTimer(Index_).Start()
            Else
                PlayerBuff_Info(Index_, 0)
                PlayerBuff_Iconpacket(Index_)
                Attack_SendAttackEnd(Index_)
            End If
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

        Private Sub AddBuffToList(ByVal buff As cBuff, ByVal Index_ As Integer)
            PlayerData(Index_).Buffs.Add(buff)
        End Sub

        Private Sub RemoveBuffFromList(ByVal SkillOverId As UInteger, ByVal Index_ As Integer)
            For i = 0 To PlayerData(Index_).Buffs.Count - 1
                If PlayerData(Index_).Buffs(i).OverID = SkillOverId Then
                    PlayerData(Index_).Buffs.Remove(PlayerData(Index_).Buffs(i))
                End If
            Next
        End Sub
    End Module
End Namespace
