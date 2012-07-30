Imports SRFramework

Namespace Functions
    Module PlayerStats
        Public Sub OnStatsPacket(ByVal Index_ As Integer)
            PlayerData(Index_).SetCharGroundStats()
            PlayerData(Index_).AddItemsToStats(Index_)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.CharacterStats)
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
            Database.SaveQuery(
                String.Format(
                    "UPDATE characters SET min_phyatk='{0}', max_phyatk='{1}', min_magatk='{2}', max_magatk='{3}', phydef='{4}', magdef='{5}', hit='{6}', parry='{7}', hp='{8}', mp='{9}', strength='{10}', intelligence='{11}', attribute='{12}' where id='{13}'",
                    PlayerData(Index_).MinPhy, PlayerData(Index_).MaxPhy, PlayerData(Index_).MinMag,
                    PlayerData(Index_).MinMag, PlayerData(Index_).PhyDef, PlayerData(Index_).MagDef,
                    PlayerData(Index_).Hit, PlayerData(Index_).Parry, PlayerData(Index_).HP, PlayerData(Index_).MP,
                    PlayerData(Index_).Strength, PlayerData(Index_).Intelligence, PlayerData(Index_).Attributes,
                    PlayerData(Index_).CharacterId))
        End Sub

        Public Sub UpdateHP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HP_MP_Update)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Word(&H10)
            writer.Byte(1)
            'type
            writer.DWord(PlayerData(Index_).CHP)
            Server.Send(writer.GetBytes, Index_)

            Database.SaveQuery(String.Format("UPDATE characters SET cur_hp='{0}', hp='{1}' where id='{2}'",
                                             PlayerData(Index_).CHP, PlayerData(Index_).HP,
                                             PlayerData(Index_).CharacterId))
        End Sub

        Public Sub UpdateMP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HP_MP_Update)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Word(&H10)
            writer.Byte(2)
            'type
            writer.DWord(PlayerData(Index_).CMP)
            Server.Send(writer.GetBytes, Index_)

            Database.SaveQuery(String.Format("UPDATE characters SET cur_mp='{0}', mp='{1}' where id='{2}'",
                                             PlayerData(Index_).CMP, PlayerData(Index_).MP,
                                             PlayerData(Index_).CharacterId))
        End Sub

        Public Sub UpdateHP_MP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HP_MP_Update)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Word(&H10)
            writer.Byte(3)
            'type
            writer.DWord(PlayerData(Index_).CHP)
            writer.DWord(PlayerData(Index_).CMP)
            Server.Send(writer.GetBytes, Index_)

            Database.SaveQuery(
                String.Format("UPDATE characters SET cur_hp='{0}', hp='{1}', cur_mp='{2}', mp='{3}' where id='{4}'",
                              PlayerData(Index_).CHP, PlayerData(Index_).HP, PlayerData(Index_).CMP,
                              PlayerData(Index_).MP, PlayerData(Index_).CharacterId))
        End Sub

        Public Sub UpdateGold(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Gold_Update)
            writer.Byte(1)
            writer.QWord(PlayerData(Index_).Gold)
            writer.Byte(0)
            Server.Send(writer.GetBytes, Index_)

            Database.SaveQuery(String.Format("UPDATE characters SET gold='{0}' where id='{1}'", PlayerData(Index_).Gold,
                                             PlayerData(Index_).CharacterId))
        End Sub

        Public Sub UpdateSP(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Gold_Update)
            writer.Byte(2)
            writer.DWord(PlayerData(index_).SkillPoints)
            writer.Byte(0)
            Server.Send(writer.GetBytes, index_)

            Database.SaveQuery(String.Format("UPDATE characters SET sp='{0}' where id='{1}'",
                                             PlayerData(index_).SkillPoints, PlayerData(index_).CharacterId))
        End Sub

        Public Sub UpdateBerserk(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Gold_Update)
            writer.Byte(4)
            writer.Byte(PlayerData(index_).BerserkBar)
            writer.DWord(0)
            Server.Send(writer.GetBytes, index_)

            Database.SaveQuery(String.Format("UPDATE characters SET berserk='{0}' where id='{1}'",
                                             PlayerData(index_).BerserkBar, PlayerData(index_).CharacterId))
        End Sub

        Public Sub UpdateSpeeds(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Speed_Update)
            writer.DWord(PlayerData(index_).UniqueID)
            writer.Float(PlayerData(index_).WalkSpeed)
            writer.Float(PlayerData(index_).RunSpeed)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, index_)
        End Sub

        Public Sub UpdateSpeedsBerserk(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Speed_Update)
            writer.DWord(PlayerData(index_).UniqueID)
            writer.Float(PlayerData(index_).WalkSpeed)
            writer.Float(PlayerData(index_).BerserkSpeed)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, index_)
        End Sub


        Public Sub UpStrength(ByVal Index_ As Integer)
            If PlayerData(Index_).Attributes > 0 Then
                PlayerData(Index_).Attributes -= 1
                If PlayerData(Index_).Strength < UShort.MaxValue Then 'Prevent Errors
                    PlayerData(Index_).Strength += 1

                    OnStatsPacket(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Str_Up)
                    writer.Byte(1)
                    Server.Send(writer.GetBytes, Index_)
                End If
            End If
        End Sub


        Public Sub UpIntelligence(ByVal Index_ As Integer)
            If PlayerData(Index_).Attributes > 0 Then
                PlayerData(Index_).Attributes -= 1
                If PlayerData(Index_).Intelligence < UShort.MaxValue Then 'Prevent Errors
                    PlayerData(Index_).Intelligence += 1

                    OnStatsPacket(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Int_Up)
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
            writer.Create(ServerOpcodes.Exp_Update)
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
            writer.Create(ServerOpcodes.LevelUp_Animation)
            writer.DWord(PlayerData(Index_).UniqueID)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub

        Public Sub SavePlayerPositionToDB(ByVal Index_ As Integer)
            Database.SaveQuery(
                String.Format(
                    "UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'",
                    PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector,
                    Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z),
                    Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
        End Sub
    End Module
End Namespace
