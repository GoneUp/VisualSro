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
                Case Object_.Type_.Normal
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
            toadd.Position = Position
            NpcList.Add(toadd)
            Dim MyIndex As UInteger = NpcList.IndexOf(toadd)

            Dim range As Integer = ServerRange

            For refindex As Integer = 0 To Server.MaxClients
                Dim socket As Net.Sockets.Socket = ClientList.GetSocket(refindex)
                Dim player As [cChar] = PlayerData(refindex) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                    Dim distance As Long = Math.Round(Math.Sqrt((Position.X - player.Position.X) ^ 2 + (Position.Y - player.Position.Y) ^ 2)) 'Calculate Distance
                    If distance < range Then
                        If PlayerData(refindex).SpawnedNPCs.Contains(MyIndex) = False Then
                            Server.Send(CreateNPCGroupSpawnPacket(MyIndex), refindex)
                            PlayerData(refindex).SpawnedNPCs.Add(MyIndex)
                        End If
                    End If
                End If
            Next refindex
        End Sub

        Public Sub SpawnNPCRange(ByVal Index_ As Integer)
            Dim range As Integer = ServerRange
            For i = 0 To NpcList.Count - 1
                If CalculateDistance(PlayerData(Index_).Position, NpcList(i).Position) <= ServerRange Then
                    If PlayerData(Index_).SpawnedNPCs.Contains(i) = False Then
                        Server.Send(CreateNPCGroupSpawnPacket(i), Index_)
                        PlayerData(Index_).SpawnedNPCs.Add(i)
                    End If
                End If
            Next
        End Sub

        Public Sub DeSpawnNPCRange(ByVal Index_ As Integer)
            Try
                For i = 0 To NpcList.Count - 1
                    If PlayerData(Index_).SpawnedNPCs.Contains(i) = True Then
                        Dim _npc As cNPC = NpcList(i)
                        If CalculateDistance(PlayerData(Index_).Position, _npc.Position) > ServerRange Then
                            Server.Send(CreateDespawnPacket(_npc.UniqueID), Index_)
                            PlayerData(Index_).SpawnedNPCs.Remove(i)
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try
        End Sub


        Public Sub OnNpcChat(ByVal NpcIndex As Integer, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            Dim obj As Object_ = GetObjectById(NpcList(NpcIndex).Pk2ID)
            Dim name As String() = obj.Name.Split("_")

            writer.Create(ServerOpcodes.Target)
            writer.Byte(1) 'Sucess
            writer.DWord(NpcList(NpcIndex).UniqueID)
            writer.Byte(0)

            Select Case name(2)
                Case "SMITH"
                    writer.Byte(&HB)
                    writer.Word(0)
                    writer.Byte(&H80)
                    writer.Byte(0)
                Case "POTION"
                    writer.DWord(3)
                    writer.Byte(0)
                Case "ARMOR"
                    writer.DWord(9)
                    writer.Byte(0)
                Case "ACCESSORY"
                    writer.DWord(1)
                    writer.Byte(0)
                Case "HORSE"
                    writer.DWord(1025)
                    writer.Byte(0)
                Case "WAREHOUSE"
                    writer.DWord(5)
                    writer.Byte(0)
                Case "SPECIAL"
                    writer.DWord(2049)
                    writer.Byte(0)
                Case "FERRY"
                    writer.Byte(1)
                    writer.Byte(0)
                    writer.Byte(8)
                    writer.Word(0)
                Case Else
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
