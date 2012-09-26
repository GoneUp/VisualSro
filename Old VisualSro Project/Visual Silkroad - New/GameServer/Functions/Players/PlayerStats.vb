Imports SRFramework

Namespace Functions
    Module PlayerStats
        Public Sub OnStatsPacket(ByVal Index_ As Integer)
            PlayerData(Index_).SetCharGroundStats()
            PlayerData(Index_).AddItemsToStats(Index_)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_CHARACTER_STATS)
            writer.DWord(PlayerData(Index_).MinPhy)
            writer.DWord(PlayerData(Index_).MaxPhy)
            writer.DWord(PlayerData(Index_).MinMag)
            writer.DWord(PlayerData(Index_).MaxMag)
            writer.Word(PlayerData(Index_).PhyDef)
            writer.Word(PlayerData(Index_).MagDef)
            writer.Word(PlayerData(Index_).Hit)
            writer.Word(PlayerData(Index_).Parry)
            writer.DWord(PlayerData(Index_).HP)
            writer.DWord(PlayerData(Index_).MP)
            writer.Word(PlayerData(Index_).Strength)
            writer.Word(PlayerData(Index_).Intelligence)
            Server.Send(writer.GetBytes, Index_)

            'Save all Data to DB
            GameDB.SaveAllStats(Index_)
        End Sub

        Public Sub UpdateHP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_HP_MP_UPDATE)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Word(&H10)
            writer.Byte(1)
            'type
            writer.DWord(PlayerData(Index_).CHP)
            Server.Send(writer.GetBytes, Index_)

            GameDB.SaveHP(Index_)
        End Sub

        Public Sub UpdateMP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_HP_MP_UPDATE)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Word(&H10)
            writer.Byte(2)
            'type
            writer.DWord(PlayerData(Index_).CMP)
            Server.Send(writer.GetBytes, Index_)

            GameDB.SaveMP(Index_)
        End Sub

        Public Sub UpdateHPMP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_HP_MP_UPDATE)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Word(&H10)
            writer.Byte(3)
            'type
            writer.DWord(PlayerData(Index_).CHP)
            writer.DWord(PlayerData(Index_).CMP)
            Server.Send(writer.GetBytes, Index_)

            GameDB.SaveHPMP(Index_)
        End Sub

        Public Sub UpdateGold(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_GOLD_UPDATE)
            writer.Byte(1)
            writer.QWord(PlayerData(Index_).Gold)
            writer.Byte(0)
            Server.Send(writer.GetBytes, Index_)

            GameDB.SaveGold(Index_)
        End Sub

        Public Sub UpdateSP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_GOLD_UPDATE)
            writer.Byte(2)
            writer.DWord(PlayerData(Index_).SkillPoints)
            writer.Byte(0)
            Server.Send(writer.GetBytes, Index_)

            GameDB.SaveSp(Index_)
        End Sub

        Public Sub UpdateBerserk(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_GOLD_UPDATE)
            writer.Byte(4)
            writer.Byte(PlayerData(Index_).BerserkBar)
            writer.DWord(0)
            Server.Send(writer.GetBytes, Index_)

            GameDB.SaveBerserk(Index_)
        End Sub

        Public Sub UpdateSpeeds(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_SPEED_UPDATE)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Float(PlayerData(Index_).WalkSpeed)
            writer.Float(PlayerData(Index_).RunSpeed)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub

        Public Sub UpdateSpeedsBerserk(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_SPEED_UPDATE)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Float(PlayerData(Index_).WalkSpeed)
            writer.Float(PlayerData(Index_).BerserkSpeed)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub


        Public Sub UpStrength(ByVal Index_ As Integer)
            If PlayerData(Index_).Attributes > 0 Then
                PlayerData(Index_).Attributes -= 1
                If PlayerData(Index_).Strength + 1 < UShort.MaxValue Then 'Prevent Errors
                    PlayerData(Index_).Strength += 1

                    OnStatsPacket(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_STR_UP)
                    writer.Byte(1)
                    Server.Send(writer.GetBytes, Index_)
                End If
            End If
        End Sub


        Public Sub UpIntelligence(ByVal Index_ As Integer)
            If PlayerData(Index_).Attributes > 0 Then
                PlayerData(Index_).Attributes -= 1
                If PlayerData(Index_).Intelligence + 1 < UShort.MaxValue Then 'Prevent Errors
                    PlayerData(Index_).Intelligence += 1

                    OnStatsPacket(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_INT_UP)
                    writer.Byte(1)
                    Server.Send(writer.GetBytes, Index_)
                End If
            End If
        End Sub


        Public Sub GetXP(ByRef exp As ULong, ByRef sp As ULong, ByVal Index_ As Integer, ByVal fromID As UInt32)
            Dim newLevel As Boolean = False
            Dim startLevel As Byte = PlayerData(Index_).Level
            Dim startXP As UInt64 = PlayerData(Index_).Experience
            Dim startSTR As UInt16 = PlayerData(Index_).Intelligence
            Dim startINT As UInt16 = PlayerData(Index_).Strength
            Dim startAttr As UInt16 = PlayerData(Index_).Attributes

            If PlayerData(Index_).Experience + exp >= ULong.MaxValue Then
                Log.WriteSystemLog("EXP OVERFLOW. Index: " & Index_)
                Exit Sub
            End If

            PlayerData(Index_).Experience += exp

            Do While True
                Dim lvldata As LevelData = GetLevelData(PlayerData(Index_).Level)

                If lvldata IsNot Nothing AndAlso PlayerData(Index_).Experience >= lvldata.Experience Then
                    'New Level   
                    If Settings.ServerLevelCap >= PlayerData(Index_).Level + 1 Then
                        PlayerData(Index_).Level += 1
                        PlayerData(Index_).Experience -= lvldata.Experience
                        newLevel = True

                        'Level Up Things
                        PlayerData(Index_).CHP = PlayerData(Index_).HP
                        PlayerData(Index_).CMP = PlayerData(Index_).MP
                        PlayerData(Index_).Strength += 1
                        PlayerData(Index_).Intelligence += 1
                        PlayerData(Index_).Attributes += 3
                    ElseIf lvldata IsNot Nothing Then
                        'Max Cap Reached
                        PlayerData(Index_).Experience -= exp
                        Dim toget As ULong = ((lvldata.Experience - PlayerData(Index_).Experience) - 1)
                        exp = toget
                        PlayerData(Index_).Experience = lvldata.Experience - 1
                        Exit Do
                    Else
                        'Lvldata is nothing, maybe a exploit?
                        'We reset all operations
                        PlayerData(Index_).Level = startLevel
                        PlayerData(Index_).Experience = startXP
                        PlayerData(Index_).Strength = startSTR
                        PlayerData(Index_).Intelligence = startINT
                        PlayerData(Index_).Attributes = startAttr

                        Throw New Exception("GetXP::Reached not available level! " & PlayerData(Index_).Level)
                    End If
                Else
                    Exit Do
                End If
            Loop

            Dim gainedSP As UInteger = Math.Truncate(sp / 400)
            Dim gainedSPX As UInteger = sp - (gainedSP * 400)

            'Error Prevention
            If _
                PlayerData(Index_).SkillPoints + gainedSP >= UInteger.MaxValue Or
                PlayerData(Index_).SkillPointBar + gainedSPX >= UInteger.MaxValue Then
                Log.WriteSystemLog("SP OVERFLOW. Index: " & Index_ & " Char:" & PlayerData(Index_).CharacterName)
                Exit Sub
            End If


            PlayerData(Index_).SkillPoints += gainedSP
            PlayerData(Index_).SkillPointBar += gainedSPX


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_EXP_UPDATE)
            writer.DWord(FromID)
            writer.QWord(exp)
            writer.QWord(sp)
            writer.Byte(0)
            If newLevel = True Then
                writer.Word(PlayerData(Index_).Attributes)

                SendLevelUpAnimation(Index_)
                OnStatsPacket(Index_)

                PlayerData(Index_).CHP = PlayerData(Index_).HP
                PlayerData(Index_).CMP = PlayerData(Index_).MP
                UpdateHPMP(Index_)
            End If

            Server.Send(writer.GetBytes, Index_)

            UpdateSP(Index_)
            Database.SaveQuery(
                String.Format("UPDATE characters SET level='{0}', experience='{1}', sp='{2}' where id='{3}'",
                              PlayerData(Index_).Level, PlayerData(Index_).Experience, PlayerData(Index_).SkillPoints,
                              PlayerData(Index_).CharacterId))
        End Sub

        Private Sub SendLevelUpAnimation(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_LEVELUP_ANIMATION)
            writer.DWord(PlayerData(Index_).UniqueID)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub
    End Module
End Namespace
