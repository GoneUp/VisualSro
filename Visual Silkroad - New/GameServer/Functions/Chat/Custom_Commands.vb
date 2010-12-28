Namespace GameServer.Mod
    Module Costum_Log
        Public Sub CheckForCoustum(ByVal Msg As String, ByVal Index_ As Integer)
            'This Function is for additional Log from a GM
            Dim writer As New PacketWriter
            Dim tmp As String() = Msg.Split(" ")

            Select Case tmp(0)
                Case "\\gold"
                    If IsNumeric(tmp(1)) Then
                        Functions.PlayerData(Index_).Gold += CULng(tmp(1))
                        Functions.UpdateGold(Index_)
                    End If
                Case "\\level"
                    If IsNumeric(tmp(1)) Then
                        Functions.PlayerData(Index_).Level = tmp(1)
                        DataBase.SaveQuery(String.Format("UPDATE characters SET level='{0}' where id='{1}'", Functions.PlayerData(Index_).Level, Functions.PlayerData(Index_).CharacterId))
                        Functions.OnTeleportUser(Index_, Functions.PlayerData(Index_).Position.XSector, Functions.PlayerData(Index_).Position.YSector)
                    End If
                Case "\\sp"
                    If IsNumeric(tmp(1)) Then
                        Functions.PlayerData(Index_).SkillPoints += CULng(tmp(1))
                        Functions.UpdateSP(Index_)
                    End If
                Case "\\stat"
                    If IsNumeric(tmp(1)) Then
                        Functions.PlayerData(Index_).Attributes += CULng(tmp(1))
                        Functions.OnTeleportUser(Index_, Functions.PlayerData(Index_).Position.XSector, Functions.PlayerData(Index_).Position.YSector)
                    End If

                Case "\\mastery"

                    If IsNumeric(tmp(1)) Then
                        For i = 0 To GameDB.Masterys.Length - 1
                            If GameDB.Masterys(i).OwnerID = Functions.PlayerData(Index_).CharacterId Then
                                GameDB.Masterys(i).Level = tmp(1)

                                writer.Create(ServerOpcodes.Mastery_Up)
                                writer.Byte(1)
                                writer.DWord(GameDB.Masterys(i).MasteryID)
                                writer.Byte(GameDB.Masterys(i).Level)
                                Server.Send(writer.GetBytes, Index_)

                                DataBase.SaveQuery(String.Format("UPDATE masteries SET level='{0}' where owner='{1}' and mastery='{2}' ", GameDB.Masterys(i).Level, GameDB.Masterys(i).OwnerID, GameDB.Masterys(i).MasteryID))
                            End If
                        Next
                    End If
                Case "\\kick"

                    For i As Integer = 0 To Server.MaxClients
                        If Functions.PlayerData(i) IsNot Nothing Then
                            If Functions.PlayerData(i).CharacterName = tmp(1) Then
                                Server.Dissconnect(i)
                            End If
                        End If
                    Next
                Case "\\npc"

                    If IsNumeric(tmp(1)) Then
                        Functions.SpawnNPC(tmp(1), Functions.PlayerData(Index_).Position, 0)
                    End If
                Case "\\silk"
                    If IsNumeric(tmp(1)) Then
                        Dim UserIndex As Integer = GameDB.GetUserWithAccID(Functions.PlayerData(Index_).AccountID)
                        Dim user = GameDB.Users(UserIndex)
                        user.Silk += tmp(1)
                        GameDB.Users(UserIndex) = user
                        DataBase.SaveQuery(String.Format("UPDATE users SET silk='{0}' where id='{1}'", GameDB.Users(UserIndex).Silk, Functions.PlayerData(Index_).AccountID))
                        Functions.OnSendSilks(Index_)
                    End If

                Case "\\state"

                    If IsNumeric(tmp(1)) And IsNumeric(tmp(2)) Then
                        Functions.UpdateState(tmp(1), tmp(2), Index_)
                    End If

                Case "\\save"
                    Functions.SendPm(Index_, "Saving start!", "[SERVER]")
                    Functions.SaveAutoSpawn(System.AppDomain.CurrentDomain.BaseDirectory & "npcpos.txt")
                    Functions.SendPm(Index_, "Saving finsihed!", "[SERVER]")

                Case "\\turn"
                    If IsNumeric(tmp(1)) Then
                        For i = 0 To Functions.NpcList.Count - 1
                            If Functions.NpcList(i).UniqueID = Functions.PlayerData(Index_).LastSelected Then
                                Functions.NpcList(i).Angle = (tmp(1) * 65535) / 360
                                Exit For
                            End If
                        Next
                    End If

                Case "\\count_me"
                    Functions.SendPm(Index_, "===========COUNT============", "[SERVER]")
                    Functions.SendPm(Index_, "Players: " & Functions.PlayerData(Index_).SpawnedPlayers.Count, "[SERVER]")
                    Functions.SendPm(Index_, "Mob: " & Functions.PlayerData(Index_).SpawnedMonsters.Count, "[SERVER]")
                    Functions.SendPm(Index_, "Npc: " & Functions.PlayerData(Index_).SpawnedNPCs.Count, "[SERVER]")
                    Functions.SendPm(Index_, "Items: " & Functions.PlayerData(Index_).SpawnedItems.Count, "[SERVER]")
                    Functions.SendPm(Index_, "== END ==", "[SERVER]")

                Case "\\count_world"
                    Functions.SendPm(Index_, "===========COUNT============", "[SERVER]")
                    Functions.SendPm(Index_, "Players: " & Server.OnlineClient, "[SERVER]")
                    Functions.SendPm(Index_, "Mob: " & Functions.MobList.Count, "[SERVER]")
                    Functions.SendPm(Index_, "Npc: " & Functions.NpcList.Count, "[SERVER]")
                    Functions.SendPm(Index_, "Items: " & Functions.ItemList.Count, "[SERVER]")
                    Functions.SendPm(Index_, "== END ==", "[SERVER]")


                Case "\\name_me"
                    If tmp(1) <> "" Then
                        Functions.PlayerData(Index_).CharacterName = tmp(1)
                        DataBase.SaveQuery(String.Format("UPDATE characters SET name='{0}' where id='{1}'", Functions.PlayerData(Index_).CharacterName, Functions.PlayerData(Index_).CharacterId))
                    End If

                Case "\\name_world"
                    '\\name_world [Old_Name] [New_Name]
                    If tmp(1) <> "" And tmp(2) = "" Then
                        For i = 0 To Server.OnlineClient - 1
                            If Functions.PlayerData(i) IsNot Nothing Then
                                If Functions.PlayerData(i).CharacterName = tmp(1) Then
                                    Functions.PlayerData(i).CharacterName = tmp(2)
                                    DataBase.SaveQuery(String.Format("UPDATE characters SET name='{0}' where id='{1}'", Functions.PlayerData(i).CharacterName, Functions.PlayerData(i).CharacterId))
                                    Exit For
                                End If
                            End If
                        Next
                    End If

            End Select


            Functions.OnStatsPacket(Index_)
        End Sub




    End Module
End Namespace
