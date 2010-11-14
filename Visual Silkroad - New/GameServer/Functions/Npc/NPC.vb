Namespace GameServer.Functions
    Module NPC
        Public NpcList As New List(Of cNPC)

        Public Function CreateNPCGroupSpawnPacket(ByVal NpcIndex As Integer) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GroupSpawnStart)
            writer.Byte(1) 'spawn
            writer.Word(1)
            Dim bytes1 As Byte() = writer.GetBytes
            Dim bytes2 As Byte() = CreateNPCSpawnPacket(NpcIndex)
            writer.Create(ServerOpcodes.GroupSpawnEnd)
            Dim bytes3 As Byte() = writer.GetBytes
            Dim finalbytes((bytes1.Length + bytes2.Length + bytes3.Length) - 1) As Byte
            Array.ConstrainedCopy(bytes1, 0, finalbytes, 0, bytes1.Length)
            Array.ConstrainedCopy(bytes2, 0, finalbytes, 9, bytes2.Length)
            Array.ConstrainedCopy(bytes3, 0, finalbytes, (bytes2.Length) + 9, bytes3.Length)
            Return finalbytes
        End Function




        Public Function CreateNPCSpawnPacket(ByVal NpcIndex As Integer) As Byte()
            Dim npc As cNPC = NpcList(NpcIndex)
            Dim obj As Object_ = GetObjectById(npc.Pk2ID)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GroupSpawnData)
            writer.DWord(npc.Pk2ID)
            writer.DWord(npc.UniqueID)
            writer.Byte(npc.Position.XSector)
            writer.Byte(npc.Position.YSector)
            writer.Float(npc.Position.X)
            writer.Float(npc.Position.Z)
            writer.Float(npc.Position.Y)

            Select Case obj.Type
                Case Object_.Type_.Npc
                    writer.Word(npc.Angle)
                    writer.Byte(0)
                    writer.Byte(1)
                    writer.Byte(0)
                    writer.Word(npc.Angle)
                    writer.Byte(1)
                    writer.Byte(0)
                    writer.Byte(0)
                    writer.DWord(0) 'speeds
                    writer.DWord(0)
                    writer.Float(100) 'berserker speed
                    writer.Byte(0)
                    writer.Byte(2)
                    writer.DWord(2)

                Case Object_.Type_.Teleport
                    writer.Word(0)

            End Select

            Return writer.GetBytes
        End Function

        Public Sub SpawnNPC(ByVal ItemId As UInteger, ByVal Position As Position)
            Dim npc_ As Object_ = GetObjectById(ItemId)
            Dim toadd As New cNPC
            toadd.UniqueID = DatabaseCore.GetUnqiueID
            toadd.Pk2ID = npc_.Id
            If npc_.Type = Object_.Type_.Npc Then
                toadd.Position = Position
            ElseIf npc_.Type = Object_.Type_.Teleport Then
                toadd.Position = npc_.T_Position
            End If

            NpcList.Add(toadd)
            Dim MyIndex As UInteger = NpcList.IndexOf(toadd)
        End Sub

        Public Sub OnNpcChat(ByVal NpcIndex As Integer, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            Dim obj As Object_ = GetObjectById(NpcList(NpcIndex).Pk2ID)
            Dim name As String() = obj.Name.Split("_")

            writer.Create(ServerOpcodes.Target)
            writer.Byte(1) 'Sucess
            writer.DWord(NpcList(NpcIndex).UniqueID)

            Select Case name(2)
                Case "SMITH"
                    writer.Byte(0)
                    writer.Byte(&HB)
                    writer.Word(0)
                    writer.Byte(&H80)
                    writer.Byte(0)
                Case "POTION"
                    writer.Byte(0)
                    writer.DWord(3)
                    writer.Byte(0)
                Case "ARMOR"
                    writer.Byte(0)
                    writer.DWord(9)
                    writer.Byte(0)
                Case "ACCESSORY"
                    writer.Byte(0)
                    writer.DWord(1)
                    writer.Byte(0)
                Case "HORSE"
                    writer.Byte(0)
                    writer.DWord(1025)
                    writer.Byte(0)
                Case "WAREHOUSE"
                    writer.Byte(0)
                    writer.DWord(5)
                    writer.Byte(0)
                Case "SPECIAL"
                    writer.Byte(0)
                    writer.DWord(2049)
                    writer.Byte(0)
                Case "FERRY"
                    writer.Byte(0)
                    writer.Byte(1)
                    writer.Byte(0)
                    writer.Byte(8)
                    writer.Word(0)
                Case "GATE"
                    writer.DWord(192)
                Case Else
                    writer.Byte(0)
                    writer.DWord(0)
                    writer.Byte(0)
            End Select

            CheckForTax(obj.Name, writer)
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = True
        End Sub

        Public Sub OnNpcChatSelect(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ObjectID As UInteger = packet.DWord
            Dim ChatID As UInteger = packet.DWord

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Npc_Chat)
            writer.Byte(1) 'Sucess
            writer.DWord(ChatID)
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = True
        End Sub

        Public Sub OnNpcChatLeft(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ObjectID As UInteger = packet.DWord

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Npc_Chat_Left)
            writer.Byte(1) 'Sucess
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = False
        End Sub


        Public Sub OnNpcTeleport(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ObjectID As UInteger = packet.DWord
            packet.Byte()
            Dim TeleportNumber As Integer = packet.DWord
            Dim Point_ As TeleportPoint_ = GetTeleportPoint(TeleportNumber)

            PlayerData(Index_).Busy = True
            PlayerData(Index_).Position = Point_.ToPos
            PlayerData(Index_).TeleportType = TeleportType_.Npc

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Npc_Teleport_Confirm)
            writer.Byte(1)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Private Function CheckForTax(ByVal Model_Name As String, ByVal writer As PacketWriter)
            Select Case Model_Name
                Case "NPC_CH_SMITH", "NPC_CH_ARMOR", "NPC_CH_POTION", "NPC_CH_ACCESSORY", "NPC_KT_SMITH", "NPC_KT_ARMOR", _
                 "NPC_KT_POTION", "NPC_KT_ACCESSORY", "STORE_CH_GATE", "STORE_KT_GATE", "NPC_CH_FERRY", "NPC_CH_FERRY2", _
                 "NPC_KT_FERRY", "NPC_KT_FERRY2", "NPC_KT_FERRY3"

                    writer.Word(ServerTaxRate)
            End Select
        End Function


    End Module
End Namespace
