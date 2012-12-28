Imports System.Net.Sockets
Imports SRFramework

Namespace Functions
    Module NPC
        Public Sub CreateNPCSpawnPacket(ByVal npcUniqueId As UInteger, ByVal writer As PacketWriter, ByVal includePacketHeader As Boolean)
            Dim npc As cNPC = NpcList(NpcUniqueId)
            Dim obj As SilkroadObject = GetObject(npc.Pk2ID)

            If includePacketHeader Then
                writer.Create(ServerOpcodes.GAME_GROUP_SPAWN_DATA)
            End If

            writer.DWord(npc.Pk2ID)
            writer.DWord(npc.UniqueID)
            writer.Byte(npc.Position.XSector)
            writer.Byte(npc.Position.YSector)
            writer.Float(npc.Position.X)
            writer.Float(npc.Position.Z)
            writer.Float(npc.Position.Y)
            writer.Word(npc.Angle)

            Select Case obj.Type
                Case SilkroadObjectTypes.Npc
                    writer.Byte(0)
                    writer.Byte(1)
                    writer.Byte(0)
                    writer.Word(npc.Angle)
                    writer.Byte(1)
                    writer.Byte(0)
                    writer.Byte(0)
                    writer.Byte(0)
                    writer.Byte(0)  'unknown

                    'speeds
                    writer.DWord(0)
                    writer.DWord(0)
                    writer.Float(100)     'berserker speed

                    writer.Word(0)


                Case SilkroadObjectTypes.Teleport
                    writer.Byte(1)
                    writer.Byte(0)
                    writer.Byte(0)
                    writer.Byte(1)
                    writer.DWord(0)
                    writer.DWord(0)

            End Select
        End Sub

        Public Sub SpawnNPC(ByVal Pk2Id As UInteger, ByVal Position As Position, ByVal Angle As UInt16, Optional ByVal channelID As UInt32 = UInt32.MaxValue)
            Dim npc_ As SilkroadObject = GetObject(Pk2Id)

            If npc_ Is Nothing Then
                Throw New Exception("Npc Spawn failed, id: " & Pk2Id)
            End If

            Dim tmp As New cNPC
            tmp.UniqueID = Id_Gen.GetUnqiueId
            tmp.Pk2ID = npc_.Pk2ID
            tmp.Angle = Angle

            If channelID = UInt32.MaxValue Then
                tmp.ChannelId = Settings.ServerWorldChannel
            Else
                tmp.ChannelId = channelID
            End If

            If npc_.Type = SilkroadObjectTypes.Npc Then
                tmp.Position = Position
            ElseIf npc_.Type = SilkroadObjectTypes.Teleport Then
                tmp.Position = npc_.T_Position
            End If


            NpcList.Add(tmp.UniqueID, tmp)


            For refindex As Integer = 0 To Server.MaxClients - 1
                Dim socket As Socket = Server.ClientList.GetSocket(refindex)
                Dim player As cCharacter = PlayerData(refindex)
                'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                    If CheckRange(player.Position, Position) Then
                        If PlayerData(refindex).SpawnedNPCs.Contains(tmp.UniqueID) = False Then
                            Dim tmpSpawn As New GroupSpawn
                            tmpSpawn.AddObject(tmp.UniqueID)
                            tmpSpawn.Send(refindex, GroupSpawn.GroupSpawnMode.SPAWN)
                            PlayerData(refindex).SpawnedNPCs.Add(tmp.UniqueID)
                        End If
                    End If
                End If
            Next refindex
        End Sub

        Public Sub OnNpcChat(ByVal npcUniqueId As UInteger, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            Dim obj As SilkroadObject = GetObject(NpcList(NpcUniqueId).Pk2ID)
           
            writer.Create(ServerOpcodes.GAME_TARGET)
            writer.Byte(1)
            'Sucess
            writer.DWord(NpcList(NpcUniqueId).UniqueID)

            writer.Byte(0)

            If obj.ItemShop IsNot Nothing Then
                writer.Byte(2) 'Option Count
                writer.Byte(1)
                writer.Byte(4)
            Else
                writer.Byte(1) 'Option Count
                writer.Byte(4) 'Only Exit
            End If

            writer.Byte(0)

            'If obj.ChatBytes IsNot Nothing Then
            '    For i = 0 To obj.ChatBytes.Length - 1
            '        writer.Byte(obj.ChatBytes(i))
            '    Next
            'Else
            '    'Unsupoorted npc
            '    SendNotice("Unsupported NPC, ID: " & obj.Pk2ID & " TypeName: " & obj.CodeName)
            '    Exit Sub
            'End If

            If obj.FortessAssoc <> "" Then
                writer.Word(Settings.ServerTaxRate)
            End If

            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = True
        End Sub

        Public Sub OnNpcChatSelect(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim uniqueID As UInteger = packet.DWord
            Dim chatId As Byte = packet.Byte
            Dim refObj As New SilkroadObject

            If NpcList.ContainsKey(uniqueID) Then
                refObj = GetObject(NpcList(uniqueID).Pk2ID)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_NPC_CHAT)
                writer.Byte(1)
                'Sucess
                If refObj.CodeName <> "NPC_CH_GACHA_MACHINE" Then
                    writer.Byte(chatId)
                    'Type
                Else
                    writer.Byte(17)
                End If

                Server.Send(writer.GetBytes, Index_)

                PlayerData(Index_).Busy = True
            End If
        End Sub

        Public Sub OnNpcChatLeft(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim objectID As UInteger = packet.DWord

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_NPC_CHAT_LEFT)
            writer.Byte(1) 'Sucess
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = False
        End Sub


        Public Sub OnNpcTeleport(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim objectID As UInteger = packet.DWord
            Dim type As Byte = packet.Byte()
            '2=normal teleport; 5=special point; 

            Select Case type
                Case 2
                    Dim teleportNumber As Integer = packet.DWord
                    Dim Point_ As TeleportPoint = GetTeleportPointByNumber(teleportNumber)

                    If Point_ Is Nothing Then
                        Server.Disconnect(Index_)
                        Exit Sub
                    ElseIf Point_.Links.ContainsKey(teleportNumber) = False Then
                        Server.Disconnect(Index_)
                        Exit Sub
                    End If

                    Dim Link As TeleportLink = Point_.Links(teleportNumber)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_NPC_TELEPORT_CONFIRM)

                    If CSng(PlayerData(Index_).Gold) - Link.Cost < 0 Then
                        'Not enough Gold...
                        writer.Byte(2)
                        writer.Byte(7)
                        'Server.Send(writer.GetBytes, Index_)

                    ElseIf PlayerData(Index_).Level < Link.MinLevel And Link.MinLevel > 0 Then
                        'Level too low
                        writer.Byte(2)
                        writer.Byte(&H15)
                        writer.Byte(&H1C)
                        Server.Send(writer.GetBytes, Index_)

                    ElseIf PlayerData(Index_).Level > Link.MaxLevel And Link.MaxLevel > 0 Then
                        'Level too high
                        writer.Byte(2)
                        writer.Byte(&H16)
                        writer.Byte(&H1C)
                        Server.Send(writer.GetBytes, Index_)

                    Else
                        PlayerData(Index_).Busy = True
                        PlayerData(Index_).SetPosition = Point_.ToPos
                        PlayerData(Index_).TeleportType = TeleportTypes.Npc

                        PlayerData(Index_).Gold -= Link.Cost
                        UpdateGold(Index_)

                        writer.Create(ServerOpcodes.GAME_NPC_TELEPORT_CONFIRM)
                        writer.Byte(1)
                        Server.Send(writer.GetBytes, Index_)

                        GameDB.SavePosition(Index_)
                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                    End If


                Case 5
                    Dim TeleportNumber As Integer = packet.Byte
                    '0x02=recall point, 0x03=move to dead point
            End Select
        End Sub


        Private Sub CheckForTax(ByVal Model_Name As String, ByVal writer As PacketWriter)
            Select Case Model_Name
                Case "NPC_CH_SMITH", "NPC_CH_ARMOR", "NPC_CH_POTION", "NPC_CH_ACCESSORY",
                    "STORE_CH_GATE", "NPC_CH_FERRY", "NPC_CH_FERRY2"
                    writer.Word(Settings.ServerTaxRate)
            End Select
        End Sub
    End Module
End Namespace
