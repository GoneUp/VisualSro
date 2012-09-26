Imports SRFramework

Namespace Functions
    Public Class GroupSpawn

        Private m_UniqueIDs As New List(Of UInt32)

        Public Enum GroupSpawnMode
            SPAWN = 1
            DESPAWN = 2
        End Enum

        Public Sub AddObject(ByVal id As UInt32)
            m_UniqueIDs.Add(id)
        End Sub

        Public Function GetPackets(ByVal mode As GroupSpawnMode) As PacketWriter()
            Dim packets As New List(Of PacketWriter)
            Dim tmpUniqueIDs As List(Of UInt32) = m_UniqueIDs
            Dim finsihed As Boolean = False

            Do
                Dim start As New PacketWriter
                Dim data As New PacketWriter
                Dim finsih As New PacketWriter
                Dim writtenUniqueIds As New List(Of UInt32)
                Dim writtenBytes As UInt32 = 0

                data.Create(ServerOpcodes.GAME_GROUP_SPAWN_DATA)
                For Each UniqueID In tmpUniqueIDs.ToArray()
                    If writtenBytes + 300 > 8192 Then
                        '300 is a confotable saftey distance to the maximal packet size. We start a new Packet then.
                        Exit For
                    End If

                    'Mobs
                    If MobList.ContainsKey(UniqueID) Then
                        Dim mob As cMonster = MobList(UniqueID)
                        Dim refobject As SilkroadObject = GetObject(mob.Pk2ID)
                        '49 bytes min
                        '56 bytes max
                        Dim byteCount As UInt16 = data.Length
                        If mode = GroupSpawnMode.SPAWN Then
                            CreateMonsterSpawnPacket(mob, refobject, data, False)
                        ElseIf mode = GroupSpawnMode.DESPAWN Then
                            data.DWord(UniqueID)
                        End If

                        writtenBytes += (data.Length - byteCount)

                        writtenUniqueIds.Add(UniqueID)
                        tmpUniqueIDs.Remove(UniqueID)
                        Continue For
                    End If

                    'Npcs
                    If NpcList.ContainsKey(UniqueID) Then
                        '47 max
                        Dim byteCount As UInt16 = data.Length
                        If mode = GroupSpawnMode.SPAWN Then
                            CreateNPCSpawnPacket(UniqueID, data, False)
                        ElseIf mode = GroupSpawnMode.DESPAWN Then
                            data.DWord(UniqueID)
                        End If

                        writtenBytes += (data.Length - byteCount)

                        writtenUniqueIds.Add(UniqueID)
                        tmpUniqueIDs.Remove(UniqueID)
                        Continue For
                  End If

                    'Items
                    If ItemList.ContainsKey(UniqueID) Then
                        '34 max
                        Dim byteCount As UInt16 = data.Length

                        If mode = GroupSpawnMode.SPAWN Then
                            CreateItemSpawnPacket(ItemList(UniqueID), data, False)
                        ElseIf mode = GroupSpawnMode.DESPAWN Then
                            data.DWord(UniqueID)
                        End If
                        writtenBytes += (data.Length - byteCount)

                        writtenUniqueIds.Add(UniqueID)
                        tmpUniqueIDs.Remove(UniqueID)
                        Continue For
                    End If

                    'Players
                    For refindex As Integer = 0 To Server.MaxClients - 1
                        Dim othersock As Net.Sockets.Socket = Server.ClientList.GetSocket(refindex)
                        'Socket checks..
                        If (othersock IsNot Nothing) AndAlso (PlayerData(refindex) IsNot Nothing) AndAlso (othersock.Connected) Then
                            If PlayerData(refindex).UniqueID = UniqueID Then
                                Dim byteCount As UInt16 = data.Length

                                If mode = GroupSpawnMode.SPAWN Then
                                    CreatePlayerSpawnPacket(refindex, data, False)
                                ElseIf mode = GroupSpawnMode.DESPAWN Then
                                    data.DWord(UniqueID)
                                End If
                                writtenBytes += (data.Length - byteCount)

                                writtenUniqueIds.Add(UniqueID)
                                tmpUniqueIDs.Remove(UniqueID)
                            End If
                        End If
                    Next

                Next

                    If writtenUniqueIds.Count > 0 Then
                        start.Create(ServerOpcodes.GAME_GROUP_SPAWN_START)
                        start.Byte(mode)
                        start.Word(writtenUniqueIds.Count)

                        finsih.Create(ServerOpcodes.GAME_GROUP_SPAWN_END)

                        packets.Add(start)
                        packets.Add(data)
                        packets.Add(finsih)
                    End If

                    If tmpUniqueIDs.Count = 0 Then
                        finsihed = True
                    End If
            Loop While finsihed = False

            Return packets.ToArray
        End Function

        Public Function GetBytes(ByVal mode As GroupSpawnMode) As Byte()
            Dim bytes(8192) As Byte
            Dim ms As New IO.MemoryStream(bytes)
            Dim br As New IO.BinaryWriter(ms)
            Dim packets() As PacketWriter = GetPackets(mode)

            For i = 0 To packets.Count - 1
                br.Write(packets(i).GetBytes())
            Next
            Dim written As Integer = br.BaseStream.Position

            br.Close()
            ms.Close()

            Array.Resize(bytes, written)

            Return bytes
        End Function

        Public Sub Send(ByVal index_ As Integer, ByVal mode As GroupSpawnMode)
            Dim packets() As PacketWriter = GetPackets(mode)
            For Each writer As PacketWriter In packets
                Server.Send(writer.GetBytes, index_)
            Next
        End Sub


        Public ReadOnly Property Count() As Integer
            Get
                Return m_UniqueIDs.Count
            End Get
        End Property

        Public Sub Clear()
            m_UniqueIDs.Clear()
        End Sub
    End Class
End Namespace
