Namespace GameServer.Functions
    Module PlayerActions
        Public Sub OnLogout(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim tag As Byte = packet.Byte
            Select Case tag
                Case 1 'Normal Exit
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Exit)
                    writer.Byte(1) 'sucees
                    writer.Byte(5) '1 sekunde
                    writer.Byte(1) 'modew
                    Server.Send(writer.GetBytes, Index_)

                    writer = New PacketWriter
                    writer.Create(ServerOpcodes.Exit2)
                    Server.Send(writer.GetBytes, Index_)
                Case 2 'Restart
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Exit)
                    writer.Byte(1) 'sucees
                    writer.Byte(5) '1 sekunde
                    writer.Byte(2) 'mode 
                    Server.Send(writer.GetBytes, Index_)

                    writer = New PacketWriter
                    writer.Create(ServerOpcodes.Exit2)
                    Server.Send(writer.GetBytes, Index_)
            End Select

            [Mod].Damage.OnPlayerLogoff(Index_)
            Server.Dissconnect(Index_)
        End Sub
        Public Sub OnPlayerAction(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim action As Byte = packet.Byte

            Select Case action
                Case 2 'Run --> Walk
                    UpdateState(1, action, Index_)
                    PlayerData(Index_).MovementType = MoveType_.Walk

                Case 3 'Walk --> Run
                    UpdateState(1, action, Index_)
                    PlayerData(Index_).MovementType = MoveType_.Run

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
            writer.Create(ServerOpcodes.Action)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Byte(Type)
            writer.Byte(State)
            Server.SendToAllInRange(writer.GetBytes, PlayerData(Index_).Position)
        End Sub

        Public Sub UpdateState(ByVal Type As Byte, ByVal State As Byte, ByVal Index_ As Integer, ByVal MobUniqueID As UInteger)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Action)
            writer.DWord(MobUniqueID)
            writer.Byte(Type)
            writer.Byte(State)
            Server.SendIfMobIsSpawned(writer.GetBytes, MobUniqueID)
        End Sub

        Public Sub OnTeleportUser(ByVal Index_ As Integer, ByVal XSec As Byte, ByVal YSec As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Teleport_Annonce)
            writer.Byte(XSec)
            writer.Byte(YSec)
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).TeleportType = TeleportType_.GM
        End Sub

        Public Sub OnTeleportRequest(ByVal Index_ As Integer)
            DespawnPlayerTeleport(Index_)
            PlayerData(Index_).Ingame = False
            Dim writer As New PacketWriter


            If PlayerData(Index_).TeleportType = TeleportType_.Npc Then
                writer.Create(ServerOpcodes.LoadingStart2)
                writer.Byte(PlayerData(Index_).Position.XSector)
                writer.Byte(PlayerData(Index_).Position.YSector)
                Server.Send(writer.GetBytes, Index_)

                OnCharacterInfo(Index_)

            Else
                writer.Create(ServerOpcodes.LoadingStart)
                Server.Send(writer.GetBytes, Index_)

                OnCharacterInfo(Index_)

                writer = New PacketWriter
                writer.Create(ServerOpcodes.LoadingEnd)
                Server.Send(writer.GetBytes, Index_)
            End If


            writer = New PacketWriter
            writer.Create(ServerOpcodes.CharacterID)
            writer.DWord(PlayerData(Index_).UniqueId) 'charid
            writer.Word(13) 'moon pos
            writer.Byte(9) 'hours
            writer.Byte(28) 'minute
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = False
        End Sub

        Public Sub OnAngleUpdate(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim new_angle As UInt16 = packet.Word
            PlayerData(Index_).Angle = new_angle

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Angle_Update)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Word(PlayerData(Index_).Angle)
            Server.SendToAllInRange(writer.GetBytes, PlayerData(Index_).Position)
        End Sub

        Public Sub OnEmotion(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Emotion)
            writer.DWord(PlayerData(index_).UniqueId)
            writer.Byte(packet.Byte)
            Server.SendToAllInRange(writer.GetBytes, PlayerData(index_).Position)
        End Sub

        Public Sub OnHelperIcon(ByVal packet As PacketReader, ByVal index_ As Integer)
            PlayerData(index_).HelperIcon = packet.Byte

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HelperIcon)
            writer.DWord(PlayerData(index_).UniqueId)
            writer.Byte(PlayerData(index_).HelperIcon)
            Server.SendToAllInRange(writer.GetBytes, PlayerData(index_).Position)

            DataBase.SaveQuery(String.Format("UPDATE characters SET helpericon='{0}' where id='{1}'", PlayerData(index_).HelperIcon, PlayerData(index_).CharacterId))
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
                    PlayerData(index_).Pot_HP_Slot = packet.Byte
                    PlayerData(index_).Pot_HP_Value = packet.Byte
                    PlayerData(index_).Pot_MP_Slot = packet.Byte
                    PlayerData(index_).Pot_MP_Value = packet.Byte
                    PlayerData(index_).Pot_Abormal_Slot = packet.Byte
                    PlayerData(index_).Pot_Abormal_Value = packet.Byte
                    PlayerData(index_).Pot_Delay = packet.Byte

                    DataBase.SaveQuery(String.Format("UPDATE characters SET pot_hp_slot='{0}', pot_hp_value='{1}', pot_mp_slot='{2}', pot_mp_value='{3}', pot_abnormal_slot='{4}', pot_abnormal_value='{5}', , pot_delay='{6}' where id='{7}'", _
                                                     PlayerData(index_).Pot_HP_Slot, PlayerData(index_).Pot_HP_Value, PlayerData(index_).Pot_MP_Slot, PlayerData(index_).Pot_MP_Value, PlayerData(index_).Pot_Abormal_Slot, PlayerData(index_).Pot_Abormal_Value, PlayerData(index_).Pot_Delay, PlayerData(index_).CharacterId))
            End Select

        End Sub

        Private Sub UpdateHotkey(ByVal hotkey As cHotKey)
            For i = 0 To GameDB.Hotkeys.Count - 1
                If GameDB.Hotkeys(i).OwnerID = hotkey.OwnerID And GameDB.Hotkeys(i).Slot = hotkey.Slot Then
                    GameDB.Hotkeys(i) = hotkey
                    Exit For
                End If
            Next

            DataBase.SaveQuery(String.Format("UPDATE hotkeys SET Type='{0}', IconID='{1}' WHERE OwnerID='{2}' AND Slot='{3}' ", hotkey.Type, hotkey.IconID, hotkey.OwnerID, hotkey.Slot))
        End Sub


        Public Sub OnSelectObject(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ObjectID As UInteger = packet.DWord
            PlayerData(Index_).LastSelected = ObjectID

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Target)

            If ObjectID = PlayerData(Index_).UniqueId Then
                writer.Byte(1) 'Sucess
                writer.DWord(PlayerData(Index_).UniqueId)
                Server.Send(writer.GetBytes, Index_)
                Exit Sub
            End If

            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).UniqueId = ObjectID Then
                        writer.Byte(1) 'Sucess
                        writer.DWord(PlayerData(i).UniqueId)
                        writer.Byte(1) 'unknown
                        writer.Byte(5) 'unknown
                        writer.Byte(4) 'unknown
                        Server.Send(writer.GetBytes, Index_)
                        Exit Sub
                    End If
                End If
            Next

            If MobList.ContainsKey(ObjectID) Then
                Dim Mob_ As cMonster = MobList(ObjectID)
                If Mob_.UniqueID = ObjectID Then
                    writer.Byte(1) 'Sucess
                    writer.DWord(Mob_.UniqueID)
                    writer.Byte(1) 'unknown
                    writer.DWord(Mob_.HP_Cur)
                    writer.Byte(1) 'unknown
                    writer.Byte(5) 'unknown
                    Server.Send(writer.GetBytes, Index_)
                    Exit Sub
                End If
            End If


            For i = 0 To NpcList.Count - 1
                If NpcList(i).UniqueID = ObjectID Then
                    Dim npc = NpcList(i)
                    OnNpcChat(i, Index_)
                    Exit Sub
                End If
            Next
        End Sub

        Public Sub OnClientStatusUpdate(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim status As ULong = packet.QWord
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.ClientStatus)
            writer.Byte(1)
            writer.QWord(status)
            Server.Send(writer.GetBytes, Index_)
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
            PlayerData(Index_).Position_Dead = PlayerData(Index_).Position
            UpdateHP(Index_)

            DataBase.SaveQuery(String.Format("UPDATE positions SET death_xsect='{0}', death_ysect='{1}', death_xpos='{2}', death_zpos='{3}', death_ypos='{4}' where OwnerCharID='{5}'", PlayerData(Index_).Position_Recall.XSector, PlayerData(Index_).Position_Recall.YSector, Math.Round(PlayerData(Index_).Position_Recall.X), Math.Round(PlayerData(Index_).Position_Recall.Z), Math.Round(PlayerData(Index_).Position_Recall.Y), PlayerData(Index_).CharacterId))
        End Sub

        Public Sub Player_Die1(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Die_1)
            writer.Byte(4)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub Player_Die2(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Die_2)
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
                    PlayerData(Index_).Position = Functions.PlayerData(Index_).Position_Return 'Set new Pos
                    DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                    OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)

                Case 2 'At present Point
                    Player_Die2(Index_)
                    UpdateState(0, 1, Index_)
                    UpdateHP(Index_)
            End Select
        End Sub
    End Module
End Namespace
