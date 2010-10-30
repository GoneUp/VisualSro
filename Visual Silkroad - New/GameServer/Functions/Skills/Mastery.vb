Namespace GameServer.Functions
    Module Mastery
        Public Sub OnUpMastery(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim MasteryID As UInteger = packet.DWord
            Dim masterycount As UInteger = 0
            Dim writer As New PacketWriter

            For i = 0 To DatabaseCore.Masterys.Length - 1
                If DatabaseCore.Masterys(i).OwnerID = PlayerData(index_).UniqueId Then
                    masterycount += DatabaseCore.Masterys(i).Level
                End If
            Next
            If PlayerData(index_).Model >= 1907 And PlayerData(index_).Model <= 1932 Then
                'Chinese Char
                If masterycount < ServerMasteryCap Then
                    'Free mastery

                    For i = 0 To DatabaseCore.Masterys.Length - 1
                        If DatabaseCore.Masterys(i).OwnerID = PlayerData(index_).UniqueId And DatabaseCore.Masterys(i).MasteryID = MasteryID Then

                            Dim _lvldata As cLevelData = GetLevelDataByLevel(DatabaseCore.Masterys(i).Level)
                            If PlayerData(index_).SkillPoints - _lvldata.SkillPoints >= 0 Then
                                DatabaseCore.Masterys(i).Level += 1

                                PlayerData(index_).SkillPoints -= _lvldata.SkillPoints
                                UpdateSP(index_)

                                DataBase.SaveQuery(String.Format("UPDATE masteries SET level='{0}' where owner='{1}' and mastery='{2}' ", DatabaseCore.Masterys(i).Level, DatabaseCore.Masterys(i).OwnerID, DatabaseCore.Masterys(i).MasteryID))

                                writer.Create(ServerOpcodes.Mastery_Up)
                                writer.Byte(1)
                                writer.DWord(DatabaseCore.Masterys(i).MasteryID)
                                writer.Byte(DatabaseCore.Masterys(i).Level)
                                Server.Send(writer.GetBytes, index_)

                            Else
                                'Not enough SP's
                                writer.Create(ServerOpcodes.Mastery_Up)
                                writer.Byte(2)
                                writer.Byte(4)
                                Server.Send(writer.GetBytes, index_)
                            End If



                            Exit For
                        End If
                    Next
                ElseIf masterycount >= ServerMasteryCap Then
                    writer.Create(ServerOpcodes.Mastery_Up)
                    writer.Byte(2)
                    writer.Byte(4)
                    Server.Send(writer.GetBytes, index_)
                End If


            ElseIf PlayerData(index_).Model >= 14717 And PlayerData(index_).Model <= 14743 Then
                'Europe Char = Diffrent Mastery Max system
                Dim maxmastery As UInteger = PlayerData(index_).Level * 2

                If masterycount < maxmastery Then
                    'Free mastery

                    For i = 0 To DatabaseCore.Masterys.Length - 1
                        If DatabaseCore.Masterys(i).OwnerID = PlayerData(index_).UniqueId And DatabaseCore.Masterys(i).MasteryID = MasteryID Then

                            Dim _lvldata As cLevelData = GetLevelDataByLevel(DatabaseCore.Masterys(i).Level + 1)
                            If PlayerData(index_).SkillPoints - _lvldata.SkillPoints >= 0 Then
                                DatabaseCore.Masterys(i).Level += 1

                                PlayerData(index_).SkillPoints -= _lvldata.SkillPoints
                                UpdateSP(index_)

                                DataBase.SaveQuery(String.Format("UPDATE masteries SET level='{0}' where owner='{1}' and mastery='{2} ", DatabaseCore.Masterys(i).Level, DatabaseCore.Masterys(i).OwnerID, DatabaseCore.Masterys(i).MasteryID))

                                writer.Create(ServerOpcodes.Mastery_Up)
                                writer.Byte(1)
                                writer.DWord(DatabaseCore.Masterys(i).MasteryID)
                                writer.Byte(DatabaseCore.Masterys(i).Level)
                                Server.Send(writer.GetBytes, index_)

                            Else
                                'Not enough SP's
                                writer.Create(ServerOpcodes.Mastery_Up)
                                writer.Byte(2)
                                writer.Byte(4)
                                Server.Send(writer.GetBytes, index_)
                            End If



                            Exit For
                        End If
                    Next
                ElseIf masterycount >= maxmastery Then
                    writer.Create(ServerOpcodes.Mastery_Up)
                    writer.Byte(2)
                    writer.Byte(4)
                    Server.Send(writer.GetBytes, index_)
                End If




            End If
        End Sub

        Public Sub OnAddSkill(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Dim SkillID As UInteger = Packet.DWord

            Dim _skill As Skill_ = GetSkillById(SkillID)

            If PlayerData(Index_).SkillPoints - _skill.RequiredSp >= 0 Then
                Dim skill As New cSkill
                skill.OwnerID = PlayerData(Index_).UniqueId
                skill.SkillID = SkillID
                AddSkillToDB(skill)

                PlayerData(Index_).SkillPoints -= _skill.RequiredSp
                UpdateSP(Index_)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Skill_Up)
                writer.Byte(1)
                writer.DWord(SkillID)
                Server.Send(writer.GetBytes, Index_)
            Else
                'Not enough SP

            End If
        End Sub


        Private Sub AddSkillToDB(ByVal toadd As cSkill)
            Dim NewIndex As UInteger = DatabaseCore.Skills.Length + 1
            Array.Resize(DatabaseCore.Skills, NewIndex)
            DatabaseCore.Skills(NewIndex - 1) = toadd

            DataBase.SaveQuery(String.Format("INSERT INTO skills(owner, SkillID) VALUE ('{0}',{1})", toadd.OwnerID, toadd.SkillID))
        End Sub



        Public Function GetMasteryByID(ByVal MasteryID As UInteger, ByVal Index_ As Integer) As cMastery
            Dim ToReturn As New cMastery
            For i = 0 To DatabaseCore.Masterys.Length - 1
                If DatabaseCore.Masterys(i).OwnerID = PlayerData(Index_).UniqueId And DatabaseCore.Masterys(i).MasteryID = MasteryID Then
                    ToReturn = DatabaseCore.Masterys(i)
                End If
            Next
            Return ToReturn
        End Function

    End Module
End Namespace
