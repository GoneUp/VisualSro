Imports SRFramework

Namespace Functions
    Module PlayerActions
        Public Sub OnLogout(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim countdown As Byte = 5

            Dim tag As Byte = packet.Byte
            Select Case tag
                Case 1 'Normal Exit
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_EXIT_COUNTDOWN)
                    writer.Byte(1) 'succeed
                    writer.Byte(tag)
                    writer.Byte(countdown)
                    Server.Send(writer.GetBytes, Index_)

                    Timers.PlayerExitTimer(Index_).Interval = countdown * 1000
                Case 2 'Restart
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_EXIT_COUNTDOWN)
                    writer.Byte(1) 'succeed
                    writer.Byte(tag)
                    writer.Byte(countdown)
                    Server.Send(writer.GetBytes, Index_)

                    GameMod.Damage.OnPlayerLogoff(Index_)
                    Server.Disconnect(Index_)
            End Select
        End Sub

        Public Sub OnPlayerAction(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim action As Byte = packet.Byte

            Select Case action
                Case 2 'Run --> Walk
                    UpdateState(1, action, Index_)
                    PlayerData(Index_).PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Walking

                Case 3 'Walk --> Run
                    UpdateState(1, action, Index_)
                    PlayerData(Index_).PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Running

                Case 4 'sit down
                    If SitUpTimer(Index_).Enabled = True Then
                        Exit Sub
                    End If

                    If PlayerData(Index_).ActionFlag = action Then
                        'Stand up
                        PlayerData(Index_).ActionFlag = 0
                        UpdateState(1, 0, Index_)
                        SitUpTimer(Index_).Interval = 1500
                        SitUpTimer(Index_).Start()
                    Else
                        'Sit down
                        PlayerData(Index_).ActionFlag = 4
                        UpdateState(1, action, Index_)
                        SitUpTimer(Index_).Interval = 1500
                        SitUpTimer(Index_).Start()
                    End If

                Case Else
                    Log.WriteSystemLog("UNKNOWN ACTION ID: " & action)
            End Select
        End Sub

        Public Sub UpdateState(ByVal Type As Byte, ByVal State As Byte, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ACTION)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Byte(Type)
            writer.Byte(State)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub


        Public Sub UpdateState(ByVal Type As Byte, ByVal State As Byte, ByVal State2 As Byte, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ACTION)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Byte(Type)
            writer.Byte(State)
            writer.Byte(State2)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub

        Public Sub UpdateState(ByVal Type As Byte, ByVal State As Byte, ByVal Mob_ As cMonster)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ACTION)
            writer.DWord(Mob_.UniqueID)
            writer.Byte(Type)
            writer.Byte(State)
            Server.SendIfMobIsSpawned(writer.GetBytes, Mob_.UniqueID)
        End Sub

        Public Sub OnTeleportUser(ByVal Index_ As Integer, ByVal XSec As Byte, ByVal YSec As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_TELEPORT_ANNONCE)
            writer.Byte(XSec)
            writer.Byte(YSec)
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).TeleportType = TeleportType_.GM
        End Sub

        Public Sub OnTeleportRequest(ByVal Index_ As Integer)
            If PlayerData(Index_).TeleportType = TeleportType_.None Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            DespawnPlayerTeleport(Index_)
            PlayerData(Index_).Ingame = False
            SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.GOING_INGAME

            Dim writer As New PacketWriter
            If PlayerData(Index_).TeleportType = TeleportType_.Npc Then
                writer.Create(ServerOpcodes.GAME_LOADING_START_2)
                writer.Byte(PlayerData(Index_).Position.XSector)
                writer.Byte(PlayerData(Index_).Position.YSector)
                Server.Send(writer.GetBytes, Index_)

                OnCharacterInfo(Index_)

            ElseIf PlayerData(Index_).TeleportType = TeleportType_.GM Then
                writer.Create(ServerOpcodes.GAME_LOADING_START)
                Server.Send(writer.GetBytes, Index_)

                OnCharacterInfo(Index_)

                writer = New PacketWriter
                writer.Create(ServerOpcodes.GAME_LOADING_END)
                Server.Send(writer.GetBytes, Index_)
            End If


            writer = New PacketWriter
            writer.Create(ServerOpcodes.GAME_CHARACTER_ID)
            writer.DWord(PlayerData(Index_).UniqueID)  'charid
            writer.Word(Date.Now.Day)  'moon pos
            writer.Byte(Date.Now.Hour) 'hours
            writer.Byte(Date.Now.Minute) 'minute
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = False
            PlayerData(Index_).TeleportType = TeleportType_.None
        End Sub

        Public Sub OnAngleUpdate(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim new_angle As UInt16 = packet.Word
            PlayerData(Index_).Angle = new_angle

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ANGLE_UPDATE)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.Word(PlayerData(Index_).Angle)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub

        Public Sub OnEmotion(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_EMOTION)
            writer.DWord(PlayerData(index_).UniqueID)
            writer.Byte(packet.Byte)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, index_)
        End Sub

        Public Sub OnHelperIcon(ByVal packet As PacketReader, ByVal index_ As Integer)
            PlayerData(index_).HelperIcon = packet.Byte

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_HELPER_ICON)
            writer.DWord(PlayerData(index_).UniqueID)
            writer.Byte(PlayerData(index_).HelperIcon)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, index_)

            Database.SaveQuery(String.Format("UPDATE characters SET helpericon='{0}' where id='{1}'",
                                             PlayerData(index_).HelperIcon, PlayerData(index_).CharacterId))
        End Sub

        Public Sub OnHotkeyUpdate(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim tag As Byte = packet.Byte

            Select Case tag
                Case 1
                    'Hotkey Update 
                    Dim slot As Byte = packet.Byte
                    Dim type As Byte = packet.Byte
                    Dim IconID As UInteger = packet.DWord


                    If slot >= 0 And slot <= 50 Then 'Check Slots
                        Dim tmp_ As New cHotKey
                        tmp_.OwnerID = PlayerData(index_).CharacterId
                        tmp_.Slot = slot
                        tmp_.Type = type
                        tmp_.IconID = IconID
                        UpdateHotkey(tmp_)
                    End If
                Case 2
                    PlayerData(index_).PotHp = packet.Word
                    PlayerData(index_).PotMp = packet.Word
                    PlayerData(index_).PotAbormal = packet.Word
                    PlayerData(index_).PotDelay = packet.Word

                    Database.SaveQuery(
                        String.Format(
                            "UPDATE characters SET pot_hp='{0}', pot_mp='{1}', pot_abnormal='{2}', pot_delay='{3}' where id='{4}'",
                            PlayerData(index_).PotHp,
                            PlayerData(index_).PotMp,
                            PlayerData(index_).PotAbormal,
                            PlayerData(index_).PotDelay, PlayerData(index_).CharacterId))
            End Select
        End Sub

        Private Sub UpdateHotkey(ByVal hotkey As cHotKey)
            For i = 0 To GameDB.Hotkeys.Count - 1
                If GameDB.Hotkeys(i).OwnerID = hotkey.OwnerID And GameDB.Hotkeys(i).Slot = hotkey.Slot Then
                    GameDB.Hotkeys(i) = hotkey
                    Exit For
                End If
            Next

            Database.SaveQuery(String.Format("UPDATE hotkeys SET Type='{0}', IconID='{1}' WHERE OwnerID='{2}' AND Slot='{3}' ",
                              hotkey.Type, hotkey.IconID, hotkey.OwnerID, hotkey.Slot))
        End Sub


        Public Sub OnSelectObject(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ObjectID As UInteger = packet.DWord
            PlayerData(Index_).LastSelected = ObjectID

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_TARGET)

            If ObjectID = PlayerData(Index_).UniqueID Then
                writer.Byte(1)
                'Sucess
                writer.DWord(PlayerData(Index_).UniqueID)
                Server.Send(writer.GetBytes, Index_)
                Exit Sub
            End If

            For i = 0 To Server.MaxClients - 1
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).UniqueID = ObjectID Then
                        writer.Byte(1)
                        'Sucess
                        writer.DWord(PlayerData(i).UniqueID)
                        writer.Byte(1)
                        'unknown
                        writer.Byte(5)
                        'unknown
                        writer.Byte(4)
                        'unknown
                        Server.Send(writer.GetBytes, Index_)
                        Exit Sub
                    End If
                End If
            Next

            If MobList.ContainsKey(ObjectID) Then
                Dim Mob_ As cMonster = MobList(ObjectID)
                If Mob_.UniqueID = ObjectID Then
                    writer.Byte(1)
                    'Sucess
                    writer.DWord(Mob_.UniqueID)
                    writer.Byte(1)
                    'unknown
                    writer.DWord(Mob_.HP_Cur)
                    writer.Byte(1)
                    'unknown
                    writer.Byte(5)
                    'unknown
                    Server.Send(writer.GetBytes, Index_)
                    Exit Sub
                End If
            End If


            If NpcList.ContainsKey(ObjectID) Then
                Dim npc = NpcList(ObjectID)
                OnNpcChat(ObjectID, Index_)
                Exit Sub
            End If
        End Sub

        Public Sub OnClientStatusUpdate(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim status As ULong = packet.QWord

            Select Case status
                Case 1
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_CLIENT_STATUS)
                    writer.Byte(1)
                    writer.QWord(status)
                    Server.Send(writer.GetBytes, Index_)
                Case 2
                    'moving
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_CLIENT_STATUS)
                    writer.Byte(1)
                    writer.QWord(status)
                    Server.Send(writer.GetBytes, Index_)
            End Select
        End Sub


        Public Sub KillPlayer(ByVal Index_ As Integer)
            UpdateState(8, 0, Index_)
            UpdateState(0, 2, Index_)
            Player_Die1(Index_)
            Player_Die2(Index_)

            Attack_SendAttackEnd(Index_)
            PlayerAttackTimer(Index_).Stop()
            PlayerData(Index_).Attacking = False
            PlayerData(Index_).Busy = True
            PlayerData(Index_).AttackType = AttackType_.Normal
            PlayerData(Index_).AttackedId = 0
            PlayerData(Index_).UsingSkillId = 0
            PlayerData(Index_).SkillOverId = 0
            PlayerData(Index_).CHP = 0
            PlayerData(Index_).Alive = False
            PlayerData(Index_).PositionDead = PlayerData(Index_).Position
            UpdateHP(Index_)
            GetXP(GetLevelData(PlayerData(Index_).Level).Experience * 0.01, 0, Index_, PlayerData(Index_).UniqueID) 'Reduce Experience by 1 %

            GameDB.SaveDeathPosition(Index_)
        End Sub

        Public Sub Player_Die1(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_DIE_1)
            writer.Byte(4)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub Player_Die2(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_DIE_2)
            writer.DWord(0)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnPlayerRespawn(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If PlayerData(Index_).Alive = True Then
                Exit Sub
            End If


            Dim tag As Byte = packet.Byte

            PlayerData(Index_).CHP = PlayerData(Index_).HP / 2
            PlayerData(Index_).Alive = True
            PlayerData(Index_).Busy = False

            Select Case tag
                Case 1 'To Town  
                    PlayerData(Index_).SetPosition = PlayerData(Index_).PositionReturn 'Set new Pos
                    GameDB.SavePosition(Index_)
                    OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)

                Case 2 'At present Point
                    Player_Die2(Index_)
                    UpdateState(0, 1, Index_)
                    UpdateHP(Index_)
            End Select
        End Sub

        Public Sub OnSetReturnPoint(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ObjectId As UInteger = packet.DWord

            If NpcList.ContainsKey(ObjectId) = False Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim ref As SilkroadObject = GetObject(NpcList(ObjectId).Pk2ID)
            PlayerData(Index_).PositionReturn = GetTeleportPoint(ref.Pk2ID).ToPos

            Database.SaveQuery(String.Format(
                            "UPDATE char_pos SET return_xsect='{0}', return_ysect='{1}', return_xpos='{2}', return_zpos='{3}', return_ypos='{4}' where OwnerCharID='{5}'",
                            PlayerData(Index_).PositionReturn.XSector, PlayerData(Index_).PositionReturn.YSector,
                            Math.Round(PlayerData(Index_).PositionReturn.X),
                            Math.Round(PlayerData(Index_).PositionReturn.Z),
                            Math.Round(PlayerData(Index_).PositionReturn.Y), PlayerData(Index_).CharacterId))


        End Sub

        Public Sub OnUseBerserk(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim tag As Byte = packet.Byte

            If tag = 1 Then
                If _
                    PlayerData(Index_).Ingame And PlayerData(Index_).Berserk = False And
                    PlayerData(Index_).BerserkBar = 5 Then
                    PlayerData(Index_).BerserkBar = 0
                    PlayerData(Index_).Berserk = True
                    PlayerData(Index_).PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Zerking

                    UpdateBerserk(Index_)
                    UpdateState(4, 1, 0, Index_)
                    UpdateSpeedsBerserk(Index_)

                    PlayerBerserkTimer(Index_).Interval = 60 * 1000
                    PlayerBerserkTimer(Index_).Start()
                End If
            End If
        End Sub
    End Module
End Namespace
