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

        Public Sub UpdateHP_MP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_HP_MP_UPDATE)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Word(&H10)
            writer.Byte(3)
            'type
            writer.DWord(PlayerData(Index_).CHP)
            writer.DWord(PlayerData(Index_).CMP)
            Server.Send(writer.GetBytes, Index_)

            GameDB.SaveHP_MP(Index_)
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

        Public Sub UpdateSP(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_GOLD_UPDATE)
            writer.Byte(2)
            writer.DWord(PlayerData(index_).SkillPoints)
            writer.Byte(0)
            Server.Send(writer.GetBytes, index_)

            GameDB.SaveSp(index_)
        End Sub

        Public Sub UpdateBerserk(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_GOLD_UPDATE)
            writer.Byte(4)
            writer.Byte(PlayerData(index_).BerserkBar)
            writer.DWord(0)
            Server.Send(writer.GetBytes, index_)

            GameDB.SaveBerserk(index_)
        End Sub

        Public Sub UpdateSpeeds(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_SPEED_UPDATE)
            writer.DWord(PlayerData(index_).UniqueID)
            writer.Float(PlayerData(index_).WalkSpeed)
            writer.Float(PlayerData(index_).RunSpeed)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, index_)
        End Sub

        Public Sub UpdateSpeedsBerserk(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_SPEED_UPDATE)
            writer.DWord(PlayerData(index_).UniqueID)
            writer.Float(PlayerData(index_).WalkSpeed)
            writer.Float(PlayerData(index_).BerserkSpeed)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, index_)
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


        Public Sub GetXP(ByRef exp As ULong, ByRef sp As ULong, ByVal index_ As Integer, ByVal FromID As UInt32)
            Dim NewLevel As Boolean = False

            If PlayerData(index_).Experience + exp >= ULong.MaxValue Then
                Log.WriteSystemLog("EXP OVERFLOW. Index: " & index_)
                Exit Sub
            End If

            PlayerData(index_).Experience += exp

            Do While True
                Dim lvldata As LevelData = GetLevelData(PlayerData(index_).Level)

                If PlayerData(index_).Experience >= lvldata.Experience Then
                    'New Level   
                    If Settings.Server_LevelCap >= PlayerData(index_).Level + 1 Then
                        PlayerData(index_).Level += 1
                        PlayerData(index_).Experience -= lvldata.Experience
                        NewLevel = True

                        'Level Up Things
                        PlayerData(index_).CHP = PlayerData(index_).HP
                        PlayerData(index_).CMP = PlayerData(index_).MP
                        PlayerData(index_).Strength += 1
                        PlayerData(index_).Intelligence += 1
                        PlayerData(index_).Attributes += 3
                    Else
                        'Max Cap Reached
                        PlayerData(index_).Experience -= exp
                        Dim toget As ULong = ((lvldata.Experience - PlayerData(index_).Experience) - 1)
                        exp = toget
                        PlayerData(index_).Experience = lvldata.Experience - 1
                        Exit Do
                    End If
                Else
                    Exit Do
                End If
            Loop

            Dim Gained_SP As UInteger = Math.Truncate(sp / 400)
            Dim Gained_SPX As UInteger = sp - (Gained_SP * 400)

            'Error Prevention
            If _
                PlayerData(index_).SkillPoints + Gained_SP >= UInteger.MaxValue Or
                PlayerData(index_).SkillPointBar + Gained_SPX >= UInteger.MaxValue Then
                Log.WriteSystemLog("SP OVERFLOW. Index: " & index_ & " Char:" & PlayerData(index_).CharacterName)
                Exit Sub
            End If


            PlayerData(index_).SkillPoints += Gained_SP
            PlayerData(index_).SkillPointBar += Gained_SPX


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_EXP_UPDATE)
            writer.DWord(FromID)
            writer.QWord(exp)
            writer.QWord(sp)
            writer.Byte(0)
            If NewLevel = True Then
                writer.Word(PlayerData(index_).Attributes)

                SendLevelUpAnimation(index_)
                OnStatsPacket(index_)

                PlayerData(index_).CHP = PlayerData(index_).HP
                PlayerData(index_).CMP = PlayerData(index_).MP
                UpdateHP_MP(index_)
            End If

            Server.Send(writer.GetBytes, index_)

            UpdateSP(index_)
            Database.SaveQuery(
                String.Format("UPDATE characters SET level='{0}', experience='{1}', sp='{2}' where id='{3}'",
                              PlayerData(index_).Level, PlayerData(index_).Experience, PlayerData(index_).SkillPoints,
                              PlayerData(index_).CharacterId))
        End Sub

        Public Sub SendLevelUpAnimation(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_LEVELUP_ANIMATION)
            writer.DWord(PlayerData(Index_).UniqueID)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub
    End Module
End Namespace
