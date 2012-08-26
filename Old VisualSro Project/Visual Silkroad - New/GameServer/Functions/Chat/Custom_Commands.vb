Imports GameServer.Functions
Imports SRFramework

Namespace GameMod
    Module Costum_Commands
        Public Sub CheckForCoustum(ByVal Msg As String, ByVal Index_ As Integer)
            'This Function is for additional Access from a GM
            Dim writer As New PacketWriter
            Dim tmp As String() = Msg.Split(" ")

            Select Case tmp(0)
                Case "\\1"
                    'SendPm(Index_, "Spawn", "")
                    'Dim stopwatch As New Stopwatch
                    'stopwatch.Start()
                    'Server.Send(CreateSpawnPacket(1), Index_)
                    'Server.Send(CreateDespawnPacket(PlayerData(1).UniqueId), Index_)
                    'stopwatch.Stop()
                    'SendPm(Index_, "DeSpawn -" & stopwatch.ElapsedMilliseconds & "ms-", "")
                    SendGlobalStatusInfo(0, 0)

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
                        Database.SaveQuery(String.Format("UPDATE characters SET level='{0}' where id='{1}'",
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

                                writer.Create(ServerOpcodes.GAME_MASTERY_UP)
                                writer.Byte(1)
                                writer.DWord(GameDB.Masterys(i).MasteryID)
                                writer.Byte(GameDB.Masterys(i).Level)
                                Server.Send(writer.GetBytes, Index_)

                                GameDB.SaveMastery(PlayerData(Index_).CharacterId, GameDB.Masterys(i).MasteryID, GameDB.Masterys(i).Level)
                            End If
                        Next
                    End If
                Case "\\kick"

                    For i As Integer = 0 To Server.MaxClients - 1
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
                        Dim userIndex As Integer = GameDB.GetUserIndex(PlayerData(Index_).AccountID)
                        Dim user As cUser = GameDB.GetUser(PlayerData(Index_).AccountID)
                        user.Silk += tmp(1)
                        GameDB.Users(UserIndex) = user
                        
                        OnSendSilks(Index_)
                    End If

                Case "\\state"

                    If IsNumeric(tmp(1)) And IsNumeric(tmp(2)) Then
                        UpdateState(tmp(1), tmp(2), Index_)
                    End If

                Case "\\save"
                    SendPm(Index_, "Saving start!", "[SERVER]")
                    SaveAutoSpawn(AppDomain.CurrentDomain.BaseDirectory & "npcpos_saved.txt")
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
                    SendPm(Index_, "Players: " & Server.OnlineClients, "[SERVER]")
                    SendPm(Index_, "Mob: " & MobList.Count, "[SERVER]")
                    SendPm(Index_, "Npc: " & NpcList.Count, "[SERVER]")
                    SendPm(Index_, "Items: " & ItemList.Count, "[SERVER]")
                    SendPm(Index_, "== END ==", "[SERVER]")


                Case "\\name_me"
                    If tmp(1) <> "" Then
                        PlayerData(Index_).CharacterName = tmp(1)
                        GameDB.SaveNameUpdate(PlayerData(Index_).CharacterId, tmp(1))
                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                    End If

                Case "\\name_world"
                    '\\name_world [Old_Name] [New_Name]
                    If tmp(1) <> "" And tmp(2) = "" Then
                        For i = 0 To Server.OnlineClients - 1
                            If PlayerData(i) IsNot Nothing Then
                                If PlayerData(i).CharacterName = tmp(1) Then
                                    PlayerData(i).CharacterName = tmp(2)
                                    GameDB.SaveNameUpdate(PlayerData(i).CharacterId, tmp(2))
                                    OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                                    Exit For
                                End If
                            End If
                        Next
                    End If

                Case "\\dmakeitem"
                    Dim pk2id As UInteger = tmp(1)
                    Dim plus As Byte = tmp(2)
                    'Or Count

                    Dim slot As Byte = GetFreeItemSlot(Index_)
                    If slot <> -1 AndAlso Inventorys(Index_).UserItems(slot).ItemID = 0 Then
                        Dim temp_item As New cItem
                        temp_item.ObjectID = pk2id

                        Dim refitem As cRefItem = GetItemByID(pk2id)
                        If refitem.CLASS_A = 1 Then
                            'Equip
                            temp_item.Plus = plus
                            temp_item.Data = refitem.MAX_DURA

                            Dim whitestats As New cWhitestats
                            whitestats.PerDurability = tmp(3)
                            whitestats.PerPhyRef = tmp(4)
                            whitestats.PerMagRef = tmp(5)
                            whitestats.PerAttackRate = tmp(6)
                            whitestats.PerPhyAtk = tmp(7)
                            whitestats.PerMagAtk = tmp(8)
                            whitestats.PerCritical = tmp(9)

                            temp_item.Variance = whitestats.GetWhiteStats(whitestats.GetItemType(refitem.CLASS_B))
                        ElseIf refitem.CLASS_A = 2 Then
                            'Pet

                        ElseIf refitem.CLASS_A = 3 Then
                            'Etc
                            temp_item.Data = plus
                        End If

                        temp_item.ID = ItemManager.AddItem(temp_item)

                        Dim invItem As New cInventoryItem
                        invItem.OwnerID = PlayerData(Index_).CharacterId
                        invItem.Slot = slot
                        invItem.ItemID = temp_item.ID
                        ItemManager.UpdateInvItem(invItem, cInventoryItem.Type.Inventory)

                        writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                        writer.Byte(1)
                        writer.Byte(6)  'type = new item
                        writer.Byte(slot)

                        AddItemDataToPacket(temp_item, writer)

                        Server.Send(writer.GetBytes, Index_)

                        Debug.Print("[ITEM CREATE][Info][Slot:{0}][ID:{1}][Dura:{2}][Amout:{3}][Plus:{4}]",
                                    slot, temp_item.ObjectID, temp_item.Data, temp_item.Data,
                                    temp_item.Plus)

                        If Settings.Log_GM Then
                            Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "GM", "Item_Create",
                                             String.Format("Slot:{0}, ID:{1}, Dura:{2}, Amout:{3}, Plus:{4}",
                                                           slot, temp_item.ObjectID, temp_item.Data,
                                                           temp_item.Data, temp_item.Plus))
                        End If
                    End If


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
                        Dim tmpitem As New cItem
                        tmpitem.Data = tmp(1)
                        tmpitem.ObjectID = 1

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
                            DropItem(New cInventoryItem, tmpitem, tmpPos)
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
                        PlayerData(Index_).PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Zerking

                        UpdateBerserk(Index_)
                        UpdateState(4, 1, 0, Index_)
                        UpdateSpeedsBerserk(Index_)

                        PlayerBerserkTimer(Index_).Interval = 10 * 60 * 1000
                        PlayerBerserkTimer(Index_).Start()

                    Else
                        PlayerData(Index_).Berserk = False
                        PlayerData(Index_).BerserkBar = 5
                        PlayerData(Index_).PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Running
                        UpdateSpeeds(Index_)
                        UpdateState(4, 0, Index_)
                        PlayerBerserkTimer(Index_).Stop()
                    End If

                Case "\\reload"
                    OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)

                Case "\\spawncheck"
                    ObjectSpawnCheck(Index_)

                Case "\\spawn"
                    If IsNumeric(tmp(1)) Then

                    End If

                Case "\\speed"
                    If IsNumeric(tmp(1)) And IsNumeric(tmp(2)) And IsNumeric(tmp(3)) Then
                        PlayerData(Index_).PosTracker.WalkSpeed = tmp(1)
                        PlayerData(Index_).PosTracker.RunSpeed = tmp(2)
                        PlayerData(Index_).PosTracker.BerserkSpeed = tmp(3)
                        UpdateSpeeds(Index_)
                    End If

                Case "\\z"
                    If IsNumeric(tmp(1)) Then
                        Dim pos = PlayerData(Index_).Position
                        pos.Z = tmp(1)
                        PlayerData(Index_).SetPosition = pos
                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                    End If

                Case "\\skillset"
                    Dim name As String = tmp(2)

                    Select Case tmp(1)
                        Case "create"
                            CreateSkillSet(name)
                            SendPm(Index_, "Skillset " & name & " created!", "[SERVER]")
                        Case "save"
                            For i = 0 To GameDB.Skills.Count - 1
                                If GameDB.Skills(i) IsNot Nothing AndAlso GameDB.Skills(i).OwnerID = PlayerData(Index_).CharacterId Then
                                    Skillset_AddSkill(name, GameDB.Skills(i).SkillID)
                                End If
                            Next
                            SendPm(Index_, "Skillset " & name & " saved!", "[SERVER]")
                        Case "clear"
                            Skillset_Clear(tmp(2)) 'name
                            SendPm(Index_, "Skillset " & name & " cleared!", "[SERVER]")
                        Case "list"
                            SendPm(Index_, "Skillset List: ", "SERVER")
                            For i = 0 To GameDB.SkillSets.Keys.Count - 1
                                Dim key As UInt32 = GameDB.SkillSets.Keys(i)
                                SendPm(Index_, GameDB.SkillSets(key).Name, "[SERVER]")
                            Next
                        Case "load"
                            Skillset_Load(name, Index_)
                            SendPm(Index_, "Skillset " & name & " loaded!", "[SERVER]")
                            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                    End Select

                Case "\\channel"
                    Select Case tmp(1)
                        Case "actual"
                            SendPm(Index_, "Actual Channel: " & PlayerData(Index_).ChannelId & ", AvoidChannel:" & PlayerData(Index_).AvoidChannels.ToString, "[SERVER]")
                        Case "switch"
                            Dim newchannel As UInt32 = Convert.ToUInt32(tmp(2))
                            PlayerData(Index_).ChannelId = newchannel
                            ObjectSpawnCheck(Index_)
                        Case "seeall"
                            If PlayerData(Index_).AvoidChannels Then
                                PlayerData(Index_).AvoidChannels = False
                            Else
                                PlayerData(Index_).AvoidChannels = True

                            End If
                            ObjectSpawnCheck(Index_)
                    End Select

                Case "\\kill"
                    'Monsters
                    Select Case tmp(1)
                        Case "view"
                            Dim mon_list = PlayerData(Index_).SpawnedMonsters
                            For i = 0 To mon_list.Count - 1
                                KillMob(mon_list(i))
                            Next

                            Dim player_list = PlayerData(Index_).SpawnedPlayers
                            For i = 0 To player_list.Count - 1
                                KillPlayer(player_list(i))
                            Next
                        Case "all"
                            Dim mon_list = MobList.Keys.ToList
                            For i = 0 To mon_list.Count - 1
                                KillMob(mon_list(i))
                            Next

                            For i = 0 To Functions.PlayerData.Count - 1
                                If Functions.PlayerData(i) IsNot Nothing AndAlso Functions.PlayerData(i).GM = False Then
                                    KillPlayer(i)
                                End If
                            Next
                    End Select


                Case "\\fill"
                    PlayerData(Index_).CHP = PlayerData(Index_).HP
                    PlayerData(Index_).CMP = PlayerData(Index_).MP
                    UpdateHP_MP(Index_)
            End Select


            OnStatsPacket(Index_)
        End Sub
    End Module
End Namespace
