Imports SRFramework

Namespace Functions
    Module Mastery
        Public Sub OnUpMastery(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim MasteryID As UInteger = packet.DWord
            Dim masterycount As UInteger = 0
            Dim writer As New PacketWriter

            If PlayerData(index_).Skilling = True Then
                Exit Sub
            End If
            PlayerData(index_).Skilling = True


            For i = 0 To GameDB.Masterys.Length - 1
                If GameDB.Masterys(i) IsNot Nothing Then
                    If GameDB.Masterys(i).OwnerID = PlayerData(index_).CharacterId Then
                        masterycount += GameDB.Masterys(i).Level
                    End If
                End If
            Next
            If PlayerData(index_).Pk2ID >= 1907 And PlayerData(index_).Pk2ID <= 1932 Then  'Chinese Char
                If masterycount < Settings.Server_MasteryCap Then 'Free mastery


                    For i = 0 To GameDB.Masterys.Length - 1
                        If _
                            GameDB.Masterys(i).OwnerID = PlayerData(index_).CharacterId And
                            GameDB.Masterys(i).MasteryID = MasteryID Then

                            Dim _lvldata As LevelData = GetLevelData(GameDB.Masterys(i).Level)
                            If PlayerData(index_).SkillPoints - _lvldata.SkillPoints >= 0 Then
                                GameDB.Masterys(i).Level += 1

                                PlayerData(index_).SkillPoints -= _lvldata.SkillPoints
                                UpdateSP(index_)

                                GameDB.SaveMastery(PlayerData(index_).CharacterId, GameDB.Masterys(i).MasteryID, GameDB.Masterys(i).Level)

                                writer.Create(ServerOpcodes.GAME_MASTERY_UP)
                                writer.Byte(1)
                                writer.DWord(GameDB.Masterys(i).MasteryID)
                                writer.Byte(GameDB.Masterys(i).Level)
                                Server.Send(writer.GetBytes, index_)

                            Else
                                'Not enough SP's
                                writer.Create(ServerOpcodes.GAME_MASTERY_UP)
                                writer.Byte(2)
                                writer.Byte(4)
                                'Server.Send(writer.GetBytes, index_)
                            End If


                            Exit For
                        End If
                    Next
                ElseIf masterycount >= Settings.Server_MasteryCap Then
                    writer.Create(ServerOpcodes.GAME_MASTERY_UP)
                    writer.Byte(2)
                    writer.Byte(4)
                    'Server.Send(writer.GetBytes, index_)
                End If


            ElseIf PlayerData(index_).Pk2ID >= 14717 And PlayerData(index_).Pk2ID <= 14743 Then
                'Europe Char = Diffrent Mastery Max system
                Dim maxmastery As UInteger = PlayerData(index_).Level * 2

                If masterycount < maxmastery Then
                    'Free mastery

                    For i = 0 To GameDB.Masterys.Length - 1
                        If GameDB.Masterys(i) IsNot Nothing Then
                            If _
                                GameDB.Masterys(i).OwnerID = PlayerData(index_).CharacterId And
                                GameDB.Masterys(i).MasteryID = MasteryID Then
                                Dim _lvldata As LevelData = GetLevelData(GameDB.Masterys(i).Level + 1)
                                If PlayerData(index_).SkillPoints - _lvldata.SkillPoints >= 0 Then
                                    GameDB.Masterys(i).Level += 1

                                    PlayerData(index_).SkillPoints -= _lvldata.SkillPoints
                                    UpdateSP(index_)

                                    GameDB.SaveMastery(PlayerData(index_).CharacterId, GameDB.Masterys(i).MasteryID, GameDB.Masterys(i).Level)

                                    writer.Create(ServerOpcodes.GAME_MASTERY_UP)
                                    writer.Byte(1)
                                    writer.DWord(GameDB.Masterys(i).MasteryID)
                                    writer.Byte(GameDB.Masterys(i).Level)
                                    Server.Send(writer.GetBytes, index_)

                                Else
                                    'Not enough SP's
                                    writer.Create(ServerOpcodes.GAME_MASTERY_UP)
                                    writer.Byte(2)
                                    writer.Byte(4)
                                    'Server.Send(writer.GetBytes, index_) 
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                ElseIf masterycount >= maxmastery Then
                    writer.Create(ServerOpcodes.GAME_MASTERY_UP)
                    writer.Byte(2)
                    writer.Byte(4)
                    'Server.Send(writer.GetBytes, index_)
                End If
            End If

            PlayerData(index_).Skilling = False
        End Sub

        Public Sub OnAddSkill(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Dim SkillID As UInteger = Packet.DWord

            If PlayerData(Index_).Skilling = True Then
                Exit Sub
            End If
            PlayerData(Index_).Skilling = True

            Dim _skill As RefSkill = GetSkill(SkillID)
            Dim _refmastery As cMastery = GetMasteryByID(_skill.MasteryID, Index_)
            Dim _skillGroup As SkillGroup = GetSkillGroup(_skill.SkillGroupName)

            If _skill Is Nothing Or _refmastery Is Nothing Then
                PlayerData(Index_).Skilling = False
                Exit Sub
            End If


            'Check the previous skills in the SkillGroup
            Dim requirementsFilled As Boolean = True

            Dim list = _skillGroup.Skills.Keys.ToList
            For i = 0 To list.Count - 1
                Dim key As Byte = list(i)
                If key < _skill.SkillGroupLevel Then
                    If CheckIfUserOwnSkill(_skillGroup.Skills(key), Index_) = False Then
                       requirementsFilled = False
                    End If
                End If
            Next


            If PlayerData(Index_).SkillPoints - _skill.RequiredSp >= 0 And CheckIfUserOwnSkill(SkillID, Index_) = False And _refmastery.Level >= _skill.MasteryLevel Then
                If requirementsFilled And _skillGroup.ID <> 0 Then

                    Dim skill As New cSkill
                    skill.OwnerID = PlayerData(Index_).CharacterId
                    skill.SkillID = SkillID
                    AddSkillToDB(skill)

                    PlayerData(Index_).SkillPoints -= _skill.RequiredSp
                    UpdateSP(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_SKILL_UP)
                    writer.Byte(1)
                    writer.DWord(SkillID)
                    Server.Send(writer.GetBytes, Index_)

                Else
                    'SkillGroup error or previous Skills not skilled
                    'Kick him ;)
                    Server.Disconnect(Index_)
                    Exit Sub
                End If
            Else
                'Not enough SP or other Error

            End If

            PlayerData(Index_).Skilling = False
        End Sub

        Public Sub AddSkillToDB(ByVal toadd As cSkill)
            Dim NewIndex As UInteger = GameDB.Skills.Length + 1
            Array.Resize(GameDB.Skills, NewIndex)
            GameDB.Skills(NewIndex - 1) = toadd

            GameDB.SaveSkillAdd(toadd.OwnerID, toadd.SkillID)
        End Sub


        Public Function GetMasteryByID(ByVal MasteryID As UInteger, ByVal Index_ As Integer) As cMastery
            For i = 0 To GameDB.Masterys.Length - 1

                If GameDB.Masterys(i) IsNot Nothing Then
                    If GameDB.Masterys(i).OwnerID = PlayerData(Index_).CharacterId And GameDB.Masterys(i).MasteryID = MasteryID Then
                        Return GameDB.Masterys(i)
                    End If
                Else
                    Debug.Print("Mastery is notihing = " & Index_)
                End If
            Next
            Return Nothing
        End Function

        Public Function CheckIfUserOwnSkill(ByVal SkillID As UInteger, ByVal Index_ As Integer) As Boolean
            For i = 0 To GameDB.Skills.Length - 1
                If GameDB.Skills(i) IsNot Nothing Then
                    If GameDB.Skills(i).OwnerID = PlayerData(Index_).CharacterId And GameDB.Skills(i).SkillID = SkillID Then
                        Return True
                    End If
                End If
            Next
            Return False
        End Function


        Public Function GetHighestPlayerMastery(ByVal Index_ As Integer) As cMastery
            Dim tmpMastery As cMastery
            For i = 0 To GameDB.Masterys.Length - 1

                If GameDB.Masterys(i) IsNot Nothing Then
                    If GameDB.Masterys(i).OwnerID = PlayerData(Index_).CharacterId Then
                        If tmpMastery Is Nothing Then
                            tmpMastery = GameDB.Masterys(i)
                        ElseIf tmpMastery.Level < GameDB.Masterys(i).Level Then
                            tmpMastery = GameDB.Masterys(i)
                        End If

                    End If

                End If
            Next
            Return tmpMastery
        End Function
    End Module
End Namespace
