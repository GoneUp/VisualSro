Namespace GameServer.Functions
    Module PlayerActions
        Public Sub OnLogout(ByVal packet As PacketReader, ByVal Index As Integer)
            Dim tag As Byte = packet.Byte
            Select Case tag
                Case 1 'Normal Exit
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Exit)
                    writer.Byte(1) 'sucees
                    writer.Byte(1) '1 sekunde
                    writer.Byte(tag) 'modew
                    Server.Send(writer.GetBytes, Index)

                    writer = New PacketWriter
                    writer.Create(ServerOpcodes.Exit2)
                    Server.Send(writer.GetBytes, Index)
                Case 2 'Restart
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Exit)
                    writer.Byte(1) 'sucees
                    writer.Byte(1) '1 sekunde
                    writer.Byte(tag) 'mode 
                    Server.Send(writer.GetBytes, Index)

                    writer = New PacketWriter
                    writer.Create(ServerOpcodes.Exit2)
                    Server.Send(writer.GetBytes, Index)
            End Select
        End Sub
        Public Sub OnPlayerAction(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim action As Byte = packet.Byte

            Select Case action
                Case 2 'Run --> Walk
                    UpdateState(1, action, Index_)

                Case 3 'Walk --> Run
                    UpdateState(1, action, Index_)

                Case 4 'sit down
                    If SitUpTimer(Index_).Enabled = True Then
                        Exit Sub
                    End If

                    If PlayerData(Index_).ActionFlag = action Then
                        PlayerData(Index_).ActionFlag = 0
                        UpdateState(1, 0, Index_)
                        SitUpTimer(Index_).Interval = 1500
                        SitUpTimer(Index_).Start()
                    Else
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
            PlayerData(Index_).ActionFlag = State
        End Sub

        Public Sub UpdateState(ByVal Type As Byte, ByVal State As Byte, ByVal Index_ As Integer, ByVal MobListIndex As UInteger)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Action)
            writer.DWord(MobList(MobListIndex).UniqueID)
            writer.Byte(Type)
            writer.Byte(State)
            Server.SendIfMobIsSpawned(writer.GetBytes, MobList(MobListIndex).UniqueID)
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

            If tag = 1 Then
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
            End If
        End Sub

        Private Sub UpdateHotkey(ByVal hotkey As cHotKey)
            For i = 0 To DatabaseCore.Hotkeys.Count - 1
                If DatabaseCore.Hotkeys(i).OwnerID = hotkey.OwnerID And DatabaseCore.Hotkeys(i).Slot = hotkey.Slot Then
                    DatabaseCore.Hotkeys(i) = hotkey
                    Exit For
                End If
            Next

            DataBase.SaveQuery(String.Format("UPDATE hotkeys SET Type='{0}', IconID='{1}' WHERE OwnerID='{2}' AND Slot='{3}' ", hotkey.Type, hotkey.IconID, hotkey.OwnerID, hotkey.Slot))
        End Sub


        Public Sub OnSelectObject(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ObjectID As UInteger = packet.DWord
            Dim writer As New PacketWriter

            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).UniqueId = ObjectID Then
                        writer.Create(ServerOpcodes.Target)
                        writer.Byte(1) 'Sucess
                        writer.DWord(PlayerData(i).UniqueId)
                        writer.Byte(10) 'unknown
                        writer.DWord(&H4000000) '0x04 00 00 00
                        Server.Send(writer.GetBytes, Index_)
                        Exit Sub
                    End If
                End If
            Next

            For i = 0 To MobList.Count - 1
                If MobList(i).UniqueID = ObjectID Then
                    Dim mob = MobList(i)
                    writer.Create(ServerOpcodes.Target)
                    writer.Byte(1) 'Sucess
                    writer.DWord(MobList(i).UniqueID)
                    writer.Byte(1) 'unknown
                    writer.DWord(MobList(i).HP_Cur)
                    writer.DWord(16) 'unknown
                    Server.Send(writer.GetBytes, Index_)
                    Exit Sub
                End If
            Next

            For i = 0 To NpcList.Count - 1
                If NpcList(i).UniqueID = ObjectID Then
                    Dim npc = NpcList(i)
                    OnNpcChat(i, Index_)
                    PlayerData(Index_).LastSelected = ObjectID
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
    End Module
End Namespace
