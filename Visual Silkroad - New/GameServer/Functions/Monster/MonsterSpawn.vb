Namespace GameServer.Functions
    Module MonsterSpawn

        Public MobList As New List(Of cMonster)

        Public Function CreateMonsterSpawnPacket(ByVal _mob As cMonster) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleSpawn)
            writer.DWord(_mob.Pk2ID)
            writer.DWord(_mob.UniqueID)
            writer.Byte(_mob.Position.XSector)
            writer.Byte(_mob.Position.YSector)
            writer.Float(_mob.Position.X)
            writer.Float(_mob.Position.Z)
            writer.Float(_mob.Position.Y)

            writer.Word(_mob.Angle)
            writer.Byte(0) 'dest
            writer.Byte(1) 'walk run flag
            writer.Byte(0) 'dest
            writer.Word(_mob.Angle)
            writer.Byte(0) 'unknown
            writer.Byte(Convert.ToByte(_mob.Death)) 'death flag
            writer.Byte(0) 'berserker

            writer.Float(20) 'walkspeed
            writer.Float(50) 'runspeed
            writer.Float(100) 'berserkerspeed

            writer.Word(0) 'unknwown  
            writer.Byte(_mob.Mob_Type)
            writer.Byte(0) 'mhm

            Return writer.GetBytes
        End Function

        Public Sub SpawnMob(ByVal MobID As UInteger, ByVal Type As Byte, ByVal Position As Position)
            Dim mob_ As Object_ = GetObjectById(MobID)
            Dim tmp As New cMonster
            tmp.UniqueID = DatabaseCore.GetUnqiueID
            tmp.Pk2ID = mob_.Id
            tmp.Position = Position
            tmp.Mob_Type = Type
            tmp.HP_Cur = mob_.Hp

            Select Case Type
                Case 1
                    tmp.HP_Cur = mob_.Hp * 2
                Case 3
                    tmp.HP_Cur = mob_.Hp * 2
                    SendUniqueSpawn(MobID)
                Case 4
                    tmp.HP_Cur = mob_.Hp * 4
            End Select

            MobList.Add(tmp)

            Dim MyIndex As UInteger = MobList.IndexOf(tmp)
            Dim range As Integer = ServerRange

            For refindex As Integer = 0 To Server.MaxClients
                Dim socket As Net.Sockets.Socket = ClientList.GetSocket(refindex)
                Dim player As [cChar] = PlayerData(refindex) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                    If CheckRange(player.Position, Position) Then
                        If PlayerData(refindex).SpawnedNPCs.Contains(MyIndex) = False Then
                            Server.Send(CreateMonsterSpawnPacket(tmp), refindex)
                            PlayerData(refindex).SpawnedMonsters.Add(tmp.UniqueID)
                        End If
                    End If
                End If
            Next refindex
        End Sub

        Public Sub RemoveMob(ByVal MobIndex As Integer)
            Dim mob_ As cMonster = MobList(MobIndex)
            Server.SendIfMobIsSpawned(CreateDespawnPacket(mob_.UniqueID), mob_.UniqueID)
            MobList.RemoveAt(MobIndex)


            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).SpawnedMonsters.Contains(mob_.UniqueID) = True Then
                        PlayerData(i).SpawnedMonsters.Remove(mob_.UniqueID)
                    End If
                End If
            Next

        End Sub

        Public Sub SendUniqueSpawn(ByVal PK2ID As UInteger)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.UniqueAnnonce)
            writer.Byte(5) 'Spawn
            writer.DWord(PK2ID)
            Server.SendToAllIngame(writer.GetBytes)
        End Sub

        Public Sub SendUniqueKill(ByVal PK2ID As UInteger, ByVal KillName As String)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.UniqueAnnonce)
            writer.Byte(6) 'Spawn
            writer.DWord(PK2ID)
            writer.Word(KillName.Length)
            writer.String(KillName)
            Server.SendToAllIngame(writer.GetBytes)
        End Sub
    End Module
End Namespace
