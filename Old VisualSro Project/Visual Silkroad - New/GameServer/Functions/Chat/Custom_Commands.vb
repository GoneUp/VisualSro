Imports GameServer.GameServer.Functions

Namespace GameServer.GameMod
    Module Costum_Commands
        Public Sub CheckForCoustum(ByVal Msg As String, ByVal Index_ As Integer)
            'This Function is for additional Access from a GM
            Dim writer As New PacketWriter
            Dim tmp As String() = Msg.Split(" ")

            Select Case tmp(0)
                Case "\\1"
                    SendPm(Index_, "Spawn", "")
                    Dim stopwatch As New Stopwatch
                    stopwatch.Start()
                    Server.Send(CreateSpawnPacket(1), Index_)
                    Server.Send(CreateDespawnPacket(PlayerData(1).UniqueId), Index_)
                    stopwatch.Stop()
                    SendPm(Index_, "DeSpawn -" & stopwatch.ElapsedMilliseconds & "ms-", "")


                Case "\\capatcha"
                    SendCaptcha(Index_)

                Case "\\gold"
                    If IsNumeric(tmp(1)) Then
                        PlayerData(Index_).Gold += CULng(tmp(1))
                        UpdateGold(Index_)
                    End If
                Case "\\level"
                    If IsNumeric(tmp(1)) Then
                        PlayerData(Index_).Level = tmp(1)
                        DataBase.SaveQuery(String.Format("UPDATE characters SET level='{0}' where id='{1}'",
                                                         PlayerData(Index_).Level, PlayerData(Index_).CharacterId))
                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                    End If
                Case "\\sp"
                    If IsNumeric(tmp(1)) Then
                        PlayerData(Index_).SkillPoints += CULng(tmp(1))
                        UpdateSP(Index_)
                    End If
                Case "\\stat"
                    If IsNumeric(tmp(1)) Then
                        PlayerData(Index_).Attributes += CULng(tmp(1))
                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                    End If

                Case "\\mastery"

                    If IsNumeric(tmp(1)) Then
                        For i = 0 To GameDB.Masterys.Length - 1
                            If GameDB.Masterys(i).OwnerID = PlayerData(Index_).CharacterId Then
                                GameDB.Masterys(i).Level = tmp(1)

                                writer.Create(ServerOpcodes.Mastery_Up)
                                writer.Byte(1)
                                writer.DWord(GameDB.Masterys(i).MasteryID)
                                writer.Byte(GameDB.Masterys(i).Level)
                                Server.Send(writer.GetBytes, Index_)

                                DataBase.SaveQuery(
                                    String.Format(
                                        "UPDATE masteries SET level='{0}' where owner='{1}' and mastery='{2}' ",
                                        GameDB.Masterys(i).Level, GameDB.Masterys(i).OwnerID,
                                        GameDB.Masterys(i).MasteryID))
                            End If
                        Next
                    End If
                Case "\\kick"

                    For i As Integer = 0 To Server.MaxClients
                        If PlayerData(i) IsNot Nothing Then
                            If PlayerData(i).CharacterName = tmp(1) Then
                                Server.Disconnect(i)
                            End If
                        End If
                    Next
                Case "\\npc"

                    If IsNumeric(tmp(1)) Then
                        SpawnNPC(tmp(1), PlayerData(Index_).Position, 0)
                    End If
                Case "\\silk"
                    If IsNumeric(tmp(1)) Then
                        Dim UserIndex As Integer = GameDB.GetUserWithAccID(PlayerData(Index_).AccountID)
                        Dim user = GameDB.Users(UserIndex)
                        user.Silk += tmp(1)
                        GameDB.Users(UserIndex) = user
                        DataBase.SaveQuery(String.Format("UPDATE users SET silk='{0}' where id='{1}'",
                                                         GameDB.Users(UserIndex).Silk, PlayerData(Index_).AccountID))
                        OnSendSilks(Index_)
                    End If

                Case "\\state"

                    If IsNumeric(tmp(1)) And IsNumeric(tmp(2)) Then
                        UpdateState(tmp(1), tmp(2), Index_)
                    End If

                Case "\\save"
                    SendPm(Index_, "Saving start!", "[SERVER]")
                    SaveAutoSpawn(AppDomain.CurrentDomain.BaseDirectory & "npcpos.txt")
                    SendPm(Index_, "Saving finsihed!", "[SERVER]")

                Case "\\turn"
                    If IsNumeric(tmp(1)) Then
                        For i = 0 To NpcList.Count - 1
                            If NpcList(i).UniqueID = PlayerData(Index_).LastSelected Then
                                NpcList(i).Angle = (tmp(1) * 65535) / 360
                                Exit For
                            End If
                        Next
                    End If

                Case "\\count_me"
                    SendPm(Index_, "===========COUNT============", "[SERVER]")
                    SendPm(Index_, "Players: " & PlayerData(Index_).SpawnedPlayers.Count, "[SERVER]")
                    SendPm(Index_, "Mob: " & PlayerData(Index_).SpawnedMonsters.Count, "[SERVER]")
                    SendPm(Index_, "Npc: " & PlayerData(Index_).SpawnedNPCs.Count, "[SERVER]")
                    SendPm(Index_, "Items: " & PlayerData(Index_).SpawnedItems.Count, "[SERVER]")
                    SendPm(Index_, "== END ==", "[SERVER]")

                Case "\\count_world"
                    SendPm(Index_, "===========COUNT============", "[SERVER]")
                    SendPm(Index_, "Players: " & Server.OnlineClient, "[SERVER]")
                    SendPm(Index_, "Mob: " & MobList.Count, "[SERVER]")
                    SendPm(Index_, "Npc: " & NpcList.Count, "[SERVER]")
                    SendPm(Index_, "Items: " & ItemList.Count, "[SERVER]")
                    SendPm(Index_, "== END ==", "[SERVER]")


                Case "\\name_me"
                    If tmp(1) <> "" Then
                        PlayerData(Index_).CharacterName = tmp(1)
                        DataBase.SaveQuery(String.Format("UPDATE characters SET name='{0}' where id='{1}'",
                                                         PlayerData(Index_).CharacterName,
                                                         PlayerData(Index_).CharacterId))
                    End If

                Case "\\name_world"
                    '\\name_world [Old_Name] [New_Name]
                    If tmp(1) <> "" And tmp(2) = "" Then
                        For i = 0 To Server.OnlineClient - 1
                            If PlayerData(i) IsNot Nothing Then
                                If PlayerData(i).CharacterName = tmp(1) Then
                                    PlayerData(i).CharacterName = tmp(2)
                                    DataBase.SaveQuery(String.Format("UPDATE characters SET name='{0}' where id='{1}'",
                                                                     PlayerData(i).CharacterName,
                                                                     PlayerData(i).CharacterId))
                                    Exit For
                                End If
                            End If
                        Next
                    End If

                Case "\\dmakeitem"
                    Dim pk2id As UInteger = tmp(1)
                    Dim plus As Byte = tmp(2)
                    'Or Count

                    For i = 13 To PlayerData(Index_).MaxSlots - 1
                        If Inventorys(Index_).UserItems(i).Pk2Id = 0 Then
                            Dim temp_item As cInvItem = Inventorys(Index_).UserItems(i)
                            temp_item.Pk2Id = pk2id
                            temp_item.OwnerCharID = PlayerData(Index_).CharacterId


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

                            Inventorys(Index_).UserItems(i) = temp_item
                            UpdateItem(Inventorys(Index_).UserItems(i))
                            'SAVE IT

                            writer.Create(ServerOpcodes.ItemMove)
                            writer.Byte(1)
                            writer.Byte(6)
                            'type = new item
                            writer.Byte(Inventorys(Index_).UserItems(i).Slot)

                            AddItemDataToPacket(Inventorys(Index_).UserItems(i), writer)

                            Server.Send(writer.GetBytes, Index_)

                            Debug.Print("[ITEM CREATE][Info][Slot:{0}][ID:{1}][Dura:{2}][Amout:{3}][Plus:{4}]",
                                        temp_item.Slot, temp_item.Pk2Id, temp_item.Durability, temp_item.Amount,
                                        temp_item.Plus)

                            If Settings.Log_GM Then
                                Log.WriteGameLog(Index_, "GM", "Item_Create",
                                                 String.Format("Slot:{0}, ID:{1}, Dura:{2}, Amout:{3}, Plus:{4}",
                                                               temp_item.Slot, temp_item.Pk2Id, temp_item.Durability,
                                                               temp_item.Amount, temp_item.Plus))
                                Exit For
                            End If
                        End If
                    Next


                Case "\\moba"
                    If tmp(1) <> "" And PlayerData(Index_).LastSelected <> 0 Then
                        For Each key In MobList.Keys.ToList
                            If MobList.ContainsKey(key) Then
                                Dim Mob_ As cMonster = MobList.Item(key)
                                If Mob_.UniqueID = PlayerData(Index_).LastSelected Then
                                    MonsterAttackPlayer(Mob_.UniqueID, Index_)
                                End If
                            End If
                        Next
                    End If

                Case "\\respawn"
                    If PlayerData(Index_).Alive = False Then
                        PlayerData(Index_).CHP = PlayerData(Index_).HP / 2
                        PlayerData(Index_).Alive = True
                        PlayerData(Index_).Busy = False
                        Player_Die2(Index_)
                        UpdateState(0, 1, Index_)
                        UpdateHP(Index_)
                    End If

                Case "\\dropgold"
                    '\\dropgold [gold_amout] [drop_amout] [range]
                    If IsNumeric(tmp(1)) And IsNumeric(tmp(2)) And IsNumeric(tmp(3)) Then
                        Dim tmpitem As New cInvItem
                        tmpitem.OwnerCharID = PlayerData(Index_).UniqueId
                        tmpitem.Amount = tmp(1)
                        tmpitem.Pk2Id = 1

                        Dim random As New Random
                        'Drop that shiat
                        For i = 1 To CInt(tmp(2))
                            Dim tmpPos As Position = PlayerData(Index_).Position
                            Dim tmpX As Single = tmpPos.ToGameX + random.Next(tmp(3) * -1, tmp(3))
                            Dim tmpY As Single = tmpPos.ToGameY + random.Next(tmp(3) * -1, tmp(3))
                            tmpPos.XSector = GetXSecFromGameX(tmpX)
                            tmpPos.YSector = GetYSecFromGameY(tmpY)
                            tmpPos.X = GetXOffset(tmpX)
                            tmpPos.Y = GetYOffset(tmpY)
                            DropItem(tmpitem, tmpPos)
                        Next

                    End If
                Case "\\berserkbar"
                    If IsNumeric(tmp(1)) Then
                        If tmp(1) < 6 Then
                            PlayerData(Index_).BerserkBar = tmp(1)
                            UpdateBerserk(Index_)
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

                Case "\\reload"
                    OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)


                Case "\\spawncheck"
                    ObjectSpawnCheck(Index_)
            End Select


            OnStatsPacket(Index_)
        End Sub
    End Module
End Namespace
