Imports GameServer.Functions

Namespace GameDB
    Module GameSave
        Public Sub SavePosition(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'",
                           PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector,
                           Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z),
                           Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveRecallPosition(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE char_pos SET recall_xsect='{0}', recall_ysect='{1}', recall_xpos='{2}', recall_zpos='{3}', recall_ypos='{4}' where OwnerCharID='{5}'",
                                    PlayerData(Index_).PositionRecall.XSector, PlayerData(Index_).PositionRecall.YSector,
                                    Math.Round(PlayerData(Index_).PositionRecall.X),
                                    Math.Round(PlayerData(Index_).PositionRecall.Z),
                                    Math.Round(PlayerData(Index_).PositionRecall.Y), PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveDeathPosition(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE char_pos SET dead_xsect='{0}', dead_ysect='{1}', dead_xpos='{2}', dead_zpos='{3}', dead_ypos='{4}' where OwnerCharID='{5}'",
                    PlayerData(Index_).PositionDead.XSector, PlayerData(Index_).PositionDead.YSector,
                    Math.Round(PlayerData(Index_).PositionDead.X), Math.Round(PlayerData(Index_).PositionDead.Z),
                    Math.Round(PlayerData(Index_).PositionDead.Y), PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveHP(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE characters SET cur_hp='{0}', hp='{1}' where id='{2}'", PlayerData(Index_).CHP, PlayerData(Index_).HP, PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveMP(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE characters SET cur_mp='{0}', mp='{1}' where id='{2}'",PlayerData(Index_).CMP, PlayerData(Index_).MP, PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveHP_MP(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE characters SET cur_hp='{0}', hp='{1}', cur_mp='{2}', mp='{3}' where id='{4}'",
                              PlayerData(Index_).CHP, PlayerData(Index_).HP,
                              PlayerData(Index_).CMP, PlayerData(Index_).MP,
                              PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveNameUpdate(ByVal CharID As Integer, ByVal name As String)
            Database.SaveQuery(String.Format("UPDATE characters SET name='{0} where id='{1}'", name, CharID))
        End Sub

        Public Sub SaveMastery(ByVal CharID As UInt32, ByVal MasteryID As UInt32, ByVal Level As Byte)
            Database.SaveQuery(String.Format("UPDATE char_mastery SET level='{0}' where owner='{1}' and mastery='{2}'", Level, CharID, MasteryID))
        End Sub

        Public Sub SaveGold(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE characters SET gold='{0}' where id='{1}'", PlayerData(Index_).Gold, PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveExp(ByVal Index_ As Integer)

        End Sub

        Public Sub SaveSp(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE characters SET sp='{0}' where id='{1}'", PlayerData(Index_).SkillPoints, PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveBerserk(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE characters SET berserk='{0}' where id='{1}'",PlayerData(index_).BerserkBar, PlayerData(index_).CharacterId))
        End Sub

        Public Sub SaveAllStats(ByVal Index_ As Integer)
            Database.SaveQuery(
               String.Format(
                   "UPDATE characters SET min_phyatk='{0}', max_phyatk='{1}', min_magatk='{2}', max_magatk='{3}', phydef='{4}', magdef='{5}', hit='{6}', parry='{7}', hp='{8}', mp='{9}', strength='{10}', intelligence='{11}', attribute='{12}' where id='{13}'",
                   PlayerData(Index_).MinPhy, PlayerData(Index_).MaxPhy, PlayerData(Index_).MinMag,
                   PlayerData(Index_).MinMag, PlayerData(Index_).PhyDef, PlayerData(Index_).MagDef,
                   PlayerData(Index_).Hit, PlayerData(Index_).Parry, PlayerData(Index_).HP, PlayerData(Index_).MP,
                   PlayerData(Index_).Strength, PlayerData(Index_).Intelligence, PlayerData(Index_).Attributes,
                   PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveSilk(ByVal accountID As UInt32, ByVal silk As UInt32, ByVal silk_Bonus As UInt32, ByVal silk_Points As UInt32)
            Database.SaveQuery(String.Format("UPDATE users SET silk='{0}', silk_bonus='{1}', silk_points='{2}' where id='{3}'", silk, silk_Bonus, silk_Points, accountID))
        End Sub

        Public Sub SaveCharType_Volume(ByVal Index_ As Integer)
            Database.SaveQuery(String.Format("UPDATE characters SET chartype='{0}', volume='{1}' where id='{2}'",
                                                     PlayerData(Index_).Pk2ID, PlayerData(Index_).Volume,
                                                     PlayerData(Index_).CharacterId))
        End Sub

        Public Sub SaveSkillsetCreate(ByVal setID As UInt32, ByVal name As String)
            Database.SaveQuery(String.Format("INSERT into skillset_name(setID, name) VALUES ('{0}', '{1}')", setID, name))
        End Sub

        Public Sub SaveSkillsetNewSkill(ByVal setID As UInt32, ByVal skill As UInt32)
            Database.SaveQuery(String.Format("INSERT into skillset(setID, skillID) VALUES ('{0}', '{1}')", setID, skill))
        End Sub

        Public Sub SaveSkillsetRename(ByVal setID As UInt32, ByVal name As String)
            Database.SaveQuery(String.Format("UPDATE skillset_name SET name='{0}' where setID='{1}'", name, setID))
        End Sub

        Public Sub SaveSkillsetClear(ByVal setID As UInt32)
            Database.SaveQuery(String.Format("DELETE FROM skillset where setID='{0}'", setID))
        End Sub

        Public Sub SaveSkillsetDelete(ByVal setID As UInt32)
            Database.SaveQuery(String.Format("DELETE FROM skillset_name where setID='{0}'", setID))
        End Sub

        Public Sub SaveSkillAdd(ByVal ownerID As UInt32, ByVal skillID As UInt32)
            Database.SaveQuery(String.Format("INSERT INTO char_skill(owner, SkillID) VALUE ('{0}',{1})", ownerID, skillID))
        End Sub

        Public Sub SaveSkillDelete(ByVal ownerID As UInt32, ByVal skillID As UInt32)
            Database.SaveQuery(String.Format("DELETE FROM char_skill where owner='{0}' AND SkillID='{1}'", ownerID, skillID))
        End Sub
    End Module
End Namespace
