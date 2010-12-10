Namespace GameServer.Custom
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
                        For i = 0 To DatabaseCore.Masterys.Length - 1
                            If DatabaseCore.Masterys(i).OwnerID = Functions.PlayerData(Index_).CharacterId Then
                                DatabaseCore.Masterys(i).Level = tmp(1)

                                writer.Create(ServerOpcodes.Mastery_Up)
                                writer.Byte(1)
                                writer.DWord(DatabaseCore.Masterys(i).MasteryID)
                                writer.Byte(DatabaseCore.Masterys(i).Level)
                                Server.Send(writer.GetBytes, Index_)
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
                        Dim UserIndex As Integer = DatabaseCore.GetUserWithAccID(Functions.PlayerData(Index_).AccountID)
                        Dim user = DatabaseCore.Users(UserIndex)
                        user.Silk += tmp(1)
                        DatabaseCore.Users(UserIndex) = user
                        DataBase.SaveQuery(String.Format("UPDATE users SET silk='{0}' where id='{1}'", DatabaseCore.Users(UserIndex).Silk, Functions.PlayerData(Index_).AccountID))
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

                Case "\\count"
                    Functions.SendPm(Index_, "===========COUNT============", "[SERVER]")
                    Functions.SendPm(Index_, "Players: " & Server.OnlineClient, "[SERVER]")
                    Functions.SendPm(Index_, "Mob: " & Functions.MobList.Count, "[SERVER]")
                    Functions.SendPm(Index_, "Npc: " & Functions.NpcList.Count, "[SERVER]")
                    Functions.SendPm(Index_, "Items: " & Functions.ItemList.Count, "[SERVER]")
                    Functions.SendPm(Index_, "== END ==", "[SERVER]")
            End Select


            Functions.OnStatsPacket(Index_)
        End Sub




    End Module
End Namespace
