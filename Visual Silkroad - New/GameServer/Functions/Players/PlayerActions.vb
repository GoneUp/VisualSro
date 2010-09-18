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
                    writer.Byte(tag) 'mode
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
                    If PlayerData(Index_).ActionFlag = action Then
                        PlayerData(Index_).ActionFlag = 0
                        UpdateState(1, 0, Index_)

                    Else
                        UpdateState(1, action, Index_)
                    End If

                Case Else
                    WriteLog("UNKNOWN ACTION ID: " & action)


            End Select


        End Sub

        Public Sub UpdateState(ByVal Type As Byte, ByVal State As Byte, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Action)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Byte(Type)
            writer.Byte(State)
            Server.SendToAllInRange(writer.GetBytes, Index_)
            PlayerData(Index_).ActionFlag = State
        End Sub

        Public Sub OnTeleportRequest(ByVal Index_ As Integer)
			DespawnPlayerTeleport(Index_)
            PlayerData(Index_).Ingame = False

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.LoadingStart)
            Server.Send(writer.GetBytes, Index_)

            OnCharacterInfo(Index_)

            writer = New PacketWriter
            writer.Create(ServerOpcodes.LoadingEnd)
            Server.Send(writer.GetBytes, Index_)


            writer = New PacketWriter
            writer.Create(ServerOpcodes.CharacterID)
            writer.DWord(PlayerData(Index_).UniqueId) 'charid
            writer.Word(13) 'moon pos
            writer.Byte(9) 'hours
            writer.Byte(28) 'minute
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnAngleUpdate(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim new_angle As UInt16 = packet.Word
            PlayerData(Index_).Angle = new_angle

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Angle_Update)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Word(PlayerData(Index_).Angle)
            Server.SendToAllInRange(writer.GetBytes, Index_)
		End Sub

		Public Sub OnEmotion(ByVal packet As PacketReader, ByVal index_ As Integer)
			Dim writer As New PacketWriter
			writer.Create(ServerOpcodes.Emotion)
			writer.DWord(PlayerData(index_).UniqueId)
			writer.Byte(packet.Byte)
			Server.SendToAllInRange(writer.GetBytes, index_)
		End Sub

		Public Sub OnHelperIcon(ByVal packet As PacketReader, ByVal index_ As Integer)
			PlayerData(index_).HelperIcon = packet.Byte

			Dim writer As New PacketWriter
			writer.Create(ServerOpcodes.HelperIcon)
			writer.DWord(PlayerData(index_).UniqueId)
			writer.Byte(PlayerData(index_).HelperIcon)
			Server.SendToAllInRange(writer.GetBytes, index_)

			DataBase.SaveQuery(String.Format("UPDATE characters SET helpericon='{0}' where id='{1}'", PlayerData(index_).HelperIcon, PlayerData(index_).UniqueId))
        End Sub

        Public Sub UpdateHP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HP_MP_Update)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Word(&H10)
            writer.Byte(1) 'type
            writer.DWord(PlayerData(Index_).CHP)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub UpdateMP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HP_MP_Update)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Word(&H10)
            writer.Byte(2) 'type
            writer.DWord(PlayerData(Index_).CMP)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub UpdateHP_MP(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HP_MP_Update)
            writer.DWord(PlayerData(Index_).UniqueId)
            writer.Word(&H10)
            writer.Byte(3) 'type
            writer.DWord(PlayerData(Index_).CHP)
            writer.DWord(PlayerData(Index_).CMP)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub UpdateGold(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Gold_Update)
            writer.Byte(1)
            writer.QWord(PlayerData(Index_).Gold)
            writer.Byte(0)
            Server.Send(writer.GetBytes, Index_)
        End Sub
    End Module
End Namespace
