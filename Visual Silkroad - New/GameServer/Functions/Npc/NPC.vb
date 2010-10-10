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
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GroupSpawnData)
            writer.DWord(npc.Pk2ID)
            writer.DWord(npc.UniqueID)
            writer.Byte(npc.Position.XSector)
            writer.Byte(npc.Position.YSector)
            writer.Float(npc.Position.X)
            writer.Float(0)
            writer.Float(npc.Position.Y)
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
            'writer.Word(0) 'normal npc's = 0000 /// andere haben hier 0002 01042000
            writer.Byte(0)
            writer.Byte(2)
            writer.DWord(2)
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
            Dim range As Integer = ServerRange
            For i = 0 To PlayerData(Index_).SpawnedNPCs.Count - 1
                If CalculateDistance(PlayerData(Index_).Position, NpcList(PlayerData(Index_).SpawnedNPCs(i)).Position) > ServerRange Then
                    Server.Send(CreateDespawnPacket(NpcList(PlayerData(Index_).SpawnedNPCs(i)).UniqueID), Index_)
                    PlayerData(Index_).SpawnedNPCs.RemoveAt(i)
                End If
            Next
        End Sub
    End Module
End Namespace
