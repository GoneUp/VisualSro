﻿Namespace GameServer.Functions
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

                data.Create(ServerOpcodes.GroupSpawnData)
                For Each UniqueID In tmpUniqueIDs
                    If writtenBytes + 56 > 8192 Then
                        Exit For
                    End If

                    If MobList.ContainsKey(UniqueID) Then
                        Dim mob As cMonster = MobList(UniqueID)
                        Dim refobject As SilkroadObject = GetObject(mob.Pk2ID)
                        '49 bytes min
                        '56 bytes max

                        CreateMonsterSpawnPacket(mob, refobject, data, False)
                        writtenBytes += 56
                        tmpUniqueIDs.Remove(UniqueID)
                        Continue For
                    Else
                        tmpUniqueIDs.Remove(UniqueID)
                    End If

                    If NpcList.ContainsKey(UniqueID) Then
                        '47 max
                        CreateNPCSpawnPacket(UniqueID, data, False)
                        writtenBytes += 47
                        tmpUniqueIDs.Remove(UniqueID)
                        Continue For
                    Else
                        tmpUniqueIDs.Remove(UniqueID)
                    End If

                    If ItemList.ContainsKey(UniqueID) Then
                        '34 max
                        CreateItemSpawnPacket(ItemList(UniqueID), data, False)
                        writtenBytes += 34
                        tmpUniqueIDs.Remove(UniqueID)
                        Continue For
                    Else
                        tmpUniqueIDs.Remove(UniqueID)
                    End If
                Next



                start.Create(ServerOpcodes.GroupSpawnStart)
                start.Byte(mode)
                start.Word(writtenUniqueIds.Count)

                finsih.Create(ServerOpcodes.GroupSpawnEnd)


                If tmpUniqueIDs.Count = 0 Then
                    finsihed = True
                End If
                packets.Add(start)
                packets.Add(data)
                packets.Add(finsih)
            Loop While finsihed
     
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