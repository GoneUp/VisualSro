Namespace GameServer.Functions
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
            Dim packets(3) As PacketWriter

            packets(0) = New PacketWriter()
            packets(0).Create(ServerOpcodes.GroupSpawnStart)
            packets(0).Byte(mode)
            packets(0).Word(m_UniqueIDs.Count)

            
            packets(1) = New PacketWriter()
            packets(1).Create(ServerOpcodes.GroupSpawnData)

            For i As Integer = 0 To m_UniqueIDs.Count - 1
                If MobList.ContainsKey(m_UniqueIDs(i)) Then
                    Dim mob As cMonster = MobList(m_UniqueIDs(i))
                    Dim refobject As SilkroadObject = GetObject(mob.Pk2ID)
                    CreateMonsterSpawnPacket(mob, refobject, packets(1), False)
                    Continue For
                End If

                If NpcList.ContainsKey(m_UniqueIDs(i)) Then
                    CreateNPCSpawnPacket(m_UniqueIDs(i), packets(1), False)
                    Continue For
                End If

                If ItemList.ContainsKey(m_UniqueIDs(i)) Then
                    CreateItemSpawnPacket(ItemList(m_UniqueIDs(i)), packets(1), False)
                    Continue For
                End If
            Next
            
            packets(2) = New PacketWriter()
            packets(2).Create(ServerOpcodes.GroupSpawnEnd)
   
            Return packets
        End Function

        Public Function GetBytes(ByVal mode As GroupSpawnMode) As Byte()
            Dim bytes(8192) As Byte
            Dim ms As New IO.MemoryStream(bytes)
            Dim br As New IO.BinaryWriter(ms)
            Dim packets() As PacketWriter = GetPackets(mode)

            br.Write(packets(0).GetBytes)
            br.Write(packets(1).GetBytes)
            br.Write(packets(2).GetBytes)
            Dim written As Integer = br.BaseStream.Position

            br.Close()
            ms.Close()

            Array.Resize(bytes, written)

            Return bytes
        End Function

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
