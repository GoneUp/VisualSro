Imports GameServer.GameServer.Functions

Namespace GameServer.Mod
    Module Costum_Commands
        Public Sub CheckForCoustum(ByVal Msg As String, ByVal Index_ As Integer)
            'This Function is for additional Log from a GM
            Dim writer As New PacketWriter
            Dim tmp As String() = Msg.Split(" ")

            Select Case tmp(0)
                Case "\\capatcha"
                    SendCaptcha(Index_)

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

                Case "\\dmakeitem"
                    Dim pk2id As UInteger = tmp(1)
                    Dim plus As Byte = tmp(2)  'Or Count

                    For i = 13 To Functions.PlayerData(Index_).MaxSlots - 1
                        If Functions.Inventorys(Index_).UserItems(i).Pk2Id = 0 Then
                            Dim temp_item As cInvItem = Functions.Inventorys(Index_).UserItems(i)
                            temp_item.Pk2Id = pk2id
                            temp_item.OwnerCharID = Functions.PlayerData(Index_).CharacterId


                            Dim refitem As cItem = GetItemByID(pk2id)
                            If refitem.CLASS_A = 1 Then
                                'Equip
                                temp_item.Plus = plus
                                temp_item.Durability = refitem.MAX_DURA
                                temp_item.Blues = New List(Of cInvItem.sBlue)

                                temp_item.PerDurability = tmp(3)
                                temp_item.PerPhyRef = tmp(4)
                                temp_item.PerMagRef = tmp(5)
                                temp_item.PerAttackRate = tmp(6)
                                temp_item.PerPhyAtk = tmp(7)
                                temp_item.PerMagAtk = tmp(8)
                                temp_item.PerCritical = tmp(9)
                            ElseIf refitem.CLASS_A = 2 Then
                                'Pet

                            ElseIf refitem.CLASS_A = 3 Then
                                'Etc
                                temp_item.Amount = plus
                            End If

                            Functions.Inventorys(Index_).UserItems(i) = temp_item
                            Functions.UpdateItem(Functions.Inventorys(Index_).UserItems(i)) 'SAVE IT

                            writer.Create(ServerOpcodes.ItemMove)
                            writer.Byte(1)
                            writer.Byte(6) 'type = new item
                            writer.Byte(Functions.Inventorys(Index_).UserItems(i).Slot)

                            Functions.AddItemDataToPacket(Functions.Inventorys(Index_).UserItems(i), writer)

                            Server.Send(writer.GetBytes, Index_)

                            Debug.Print("[ITEM CREATE][Info][Slot:{0}][ID:{1}][Dura:{2}][Amout:{3}][Plus:{4}]", temp_item.Slot, temp_item.Pk2Id, temp_item.Durability, temp_item.Amount, temp_item.Plus)

                            If Settings.Log_GM Then
                                Log.WriteGameLog(Index_, "GM", "Item_Create", String.Format("Slot:{0}, ID:{1}, Dura:{2}, Amout:{3}, Plus:{4}", temp_item.Slot, temp_item.Pk2Id, temp_item.Durability, temp_item.Amount, temp_item.Plus))
                                Exit For
                            End If
                        End If
                    Next


                Case "\\moba"

                    If tmp(1) <> "" And Functions.PlayerData(Index_).LastSelected <> 0 Then
                        For Each key In Functions.MobList.Keys.ToList
                            If Functions.MobList.ContainsKey(key) Then
                                Dim Mob_ As cMonster = Functions.MobList.Item(key)
                                If Mob_.UniqueID = Functions.PlayerData(Index_).LastSelected Then
                                    Functions.MonsterAttackPlayer(Mob_.UniqueID, Index_)
                                End If
                            End If
                        Next
                    End If
                Case "\\respawn"
                    If Functions.PlayerData(Index_).Alive = False Then
                        Functions.PlayerData(Index_).CHP = Functions.PlayerData(Index_).HP / 2
                        Functions.PlayerData(Index_).Alive = True
                        Functions.PlayerData(Index_).Busy = False
                        Functions.Player_Die2(Index_)
                        Functions.UpdateState(0, 1, Index_)
                        Functions.UpdateHP(Index_)
                    End If

                Case "\\dropgold"
                    '\\dropgold [amout] [how much drops]
                    If IsNumeric(tmp(1)) And IsNumeric(tmp(2)) Then
                        Dim side As Integer = Math.Sqrt(tmp(2))
                        Dim tmpitem As New cInvItem
                        tmpitem.OwnerCharID = Functions.PlayerData(Index_).UniqueId
                        tmpitem.Amount = tmp(1)
                        tmpitem.Pk2Id = 1

                        For x = 1 To side
                            For y = 1 To side
                                Dim tmp_pos As Position = Functions.PlayerData(Index_).Position
                                Dim tmpX As Single = tmp_pos.ToGameX + x
                                Dim tmpY As Single = tmp_pos.ToGameY + y
                                tmp_pos.XSector = Functions.GetXSecFromGameX(tmpX)
                                tmp_pos.YSector = Functions.GetYSecFromGameY(tmpY)
                                tmp_pos.X = Functions.GetXOffset(tmpX)
                                tmp_pos.Y = Functions.GetYOffset(tmpY)
                                Functions.DropItem(tmpitem, tmp_pos)
                            Next
                        Next
                    End If
                Case "\\berserkbar"
                    If IsNumeric(tmp(1)) Then
                        If tmp(1) < 6 Then
                            Functions.PlayerData(Index_).BerserkBar = tmp(1)
                            Functions.UpdateBerserk(Index_)
                        End If
                    End If
                Case "\\berserk"
                    If PlayerData(Index_).Berserk Then
                        PlayerData(Index_).Berserk = True
                        PlayerData(Index_).Pos_Tracker.SpeedMode = cPositionTracker.enumSpeedMode.Zerking

                        UpdateBerserk(Index_)
                        UpdateState(4, 1, 0, Index_)
                        UpdateSpeedsBerserk(Index_)

                        PlayerBerserkTimer(Index_).Interval = 10 * 60 * 1000
                        PlayerBerserkTimer(Index_).Start()

                    Else
                        PlayerData(Index_).Berserk = False
                        PlayerData(Index_).BerserkBar = 5
                        PlayerData(Index_).Pos_Tracker.SpeedMode = cPositionTracker.enumSpeedMode.Running
                        UpdateSpeeds(Index_)
                        UpdateState(4, 0, Index_)
                        PlayerBerserkTimer(Index_).Stop()
                    End If

            End Select


            Functions.OnStatsPacket(Index_)
        End Sub




    End Module
End Namespace
