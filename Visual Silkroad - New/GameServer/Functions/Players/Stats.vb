Namespace GameServer.Functions
    Module Stats
        Public Sub OnStatsPacket(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.CharacterStats)
            writer.DWord(PlayerData(index_).MinPhy)
            writer.DWord(PlayerData(index_).MaxPhy)
            writer.DWord(PlayerData(index_).MinMag)
            writer.DWord(PlayerData(index_).MaxMag)
            writer.Word(PlayerData(index_).PhyDef)
            writer.Word(PlayerData(index_).MagDef)
            writer.Word(PlayerData(index_).Hit)
            writer.Word(PlayerData(index_).Parry)
            writer.DWord(PlayerData(index_).HP)
            writer.DWord(PlayerData(index_).MP)
            writer.Word(PlayerData(index_).Strength)
            writer.Word(PlayerData(index_).Intelligence)
            Server.Send(writer.GetBytes, index_)

            'Save all Data to DB
            DataBase.SaveQuery(String.Format("UPDATE characters SET min_phyatk='{0}', max_phyatk='{1}', min_magatk='{2}', max_magatk='{3}', phydef='{4}', magdef='{5}', hit='{6}', parry='{7}', hp='{8}', mp='{9}', strength='{10}', intelligence='{11}', attribute='{12}' where id='{13}'", _
            PlayerData(index_).MinPhy, PlayerData(index_).MaxPhy, PlayerData(index_).MinMag, PlayerData(index_).MinMag, PlayerData(index_).PhyDef, PlayerData(index_).MagDef, PlayerData(index_).Hit, PlayerData(index_).Parry, PlayerData(index_).HP, PlayerData(index_).MP, PlayerData(index_).Strength, PlayerData(index_).Intelligence, PlayerData(index_).Attributes, PlayerData(index_).UniqueId))
        End Sub

        Public Sub UpdateHP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HP_MP_Update)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Word(&H10)
            writer.Byte(1) 'type
            writer.DWord(PlayerData(Index_).CHP)
            Server.Send(writer.GetBytes, Index_)

            DataBase.SaveQuery(String.Format("UPDATE characters SET cur_hp='{0}', hp='{1}' where id='{2}'", PlayerData(Index_).CHP, PlayerData(Index_).HP, PlayerData(Index_).UniqueId))
        End Sub

        Public Sub UpdateMP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HP_MP_Update)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Word(&H10)
            writer.Byte(2) 'type
            writer.DWord(PlayerData(Index_).CMP)
            Server.Send(writer.GetBytes, Index_)

            DataBase.SaveQuery(String.Format("UPDATE characters SET cur_mp='{0}', mp='{1}' where id='{2}'", PlayerData(Index_).CMP, PlayerData(Index_).MP, PlayerData(Index_).UniqueId))
        End Sub

        Public Sub UpdateHP_MP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HP_MP_Update)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Word(&H10)
            writer.Byte(3) 'type
            writer.DWord(PlayerData(Index_).CHP)
            writer.DWord(PlayerData(Index_).CMP)
            Server.Send(writer.GetBytes, Index_)

            DataBase.SaveQuery(String.Format("UPDATE characters SET cur_hp='{0}', hp='{1}', cur_mp='{2}', mp='{3}' where id='{4}'", PlayerData(Index_).CHP, PlayerData(Index_).HP, PlayerData(Index_).CMP, PlayerData(Index_).MP, PlayerData(Index_).UniqueId))
        End Sub

        Public Sub UpdateGold(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Gold_Update)
            writer.Byte(1)
            writer.QWord(PlayerData(Index_).Gold)
            writer.Byte(0)
            Server.Send(writer.GetBytes, Index_)

            DataBase.SaveQuery(String.Format("UPDATE characters SET gold='{0}' where id='{1}'", PlayerData(Index_).Gold, PlayerData(Index_).UniqueId))
        End Sub

        Public Sub UpdateSP(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Gold_Update)
            writer.Byte(2)
            writer.DWord(PlayerData(index_).SkillPoints)
            writer.Byte(0)
            Server.Send(writer.GetBytes, index_)

            DataBase.SaveQuery(String.Format("UPDATE characters SET sp='{0}' where id='{1}'", PlayerData(index_).SkillPoints, PlayerData(index_).UniqueId))
        End Sub

        Public Sub UpStrength(ByVal Index_ As Integer)
            If PlayerData(Index_).Attributes > 0 Then
                PlayerData(Index_).Attributes -= 1
                If PlayerData(Index_).Strength < UShort.MaxValue Then 'Prevent Errors
                    PlayerData(Index_).Strength += 1
                End If

                PlayerData(Index_).SetCharStats()
                OnStatsPacket(Index_)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Str_Up)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub


        Public Sub UpIntelligence(ByVal Index_ As Integer)
            If PlayerData(Index_).Attributes > 0 Then
                PlayerData(Index_).Attributes -= 1
                If PlayerData(Index_).Intelligence < UShort.MaxValue Then 'Prevent Errors
                    PlayerData(Index_).Intelligence += 1
                End If

                PlayerData(Index_).SetCharStats()
                OnStatsPacket(Index_)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Int_Up)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub


    End Module
End Namespace
