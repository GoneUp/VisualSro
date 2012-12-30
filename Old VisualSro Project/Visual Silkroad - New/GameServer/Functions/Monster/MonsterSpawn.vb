Imports System.Net.Sockets
Imports SRFramework

Namespace Functions
    Module MonsterSpawn
        Public Sub CreateMonsterSpawnPacket(ByVal mob As cMonster, ByVal obj As SilkroadObject, ByVal writer As PacketWriter, ByVal includePacketHeader As Boolean)
            If includePacketHeader Then
                writer.Create(ServerOpcodes.GAME_SINGLE_SPAWN)
            End If

            writer.DWord(mob.Pk2ID)
            writer.DWord(mob.UniqueID)
            writer.Byte(mob.Position.XSector)
            writer.Byte(mob.Position.YSector)
            writer.Float(mob.Position.X)
            writer.Float(mob.Position.Z)
            writer.Float(mob.Position.Y)

            writer.Word(mob.Angle)
            writer.Byte(mob.PosTracker.MoveState)
            'dest
            If mob.PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Walking Then
                writer.Byte(0) 'Walking
            Else
                writer.Byte(1)  'Running + Zerk
            End If

            If mob.PosTracker.MoveState = cPositionTracker.enumMoveState.Standing Then
                writer.Byte(0)
                'dest
                writer.Word(mob.Angle)
            ElseIf mob.PosTracker.MoveState = cPositionTracker.enumMoveState.Walking Then
                writer.Byte(mob.PosTracker.WalkPos.XSector)
                writer.Byte(mob.PosTracker.WalkPos.YSector)
                writer.Byte(BitConverter.GetBytes(CShort(mob.PosTracker.WalkPos.X)))
                writer.Byte(BitConverter.GetBytes(CShort(mob.PosTracker.WalkPos.Z)))
                writer.Byte(BitConverter.GetBytes(CShort(mob.PosTracker.WalkPos.Y)))
            End If

            Dim alive As Boolean = True
            If mob.Death = True Then alive = False
            'writer.Byte(Convert.ToByte(alive)) ' alive flag
            writer.Byte(1)
            writer.Byte(0)   'action flag 
            writer.Byte(0) 'jobmode? 3=normal?
            writer.Byte(0)  'berserk activated
            writer.Byte(0)  'unknown

            writer.Float(obj.WalkSpeed)     'walkspeed
            writer.Float(obj.RunSpeed) 'runspeed
            writer.Float(obj.BerserkSpeed) 'berserkerspeed

            writer.Byte(0)  'unknwown  
            writer.Byte(2)  'unknwown  
            writer.Byte(1)
            writer.Byte(5) 'mhm
            writer.Byte(mob.MobType) '
        End Sub

        ''' <summary>
        ''' Spawns a new Mob
        ''' </summary>
        ''' <param name="mobID"></param>
        ''' <param name="type"></param>
        ''' <param name="position"></param>
        ''' <param name="angle"></param>
        ''' <param name="spotID"></param>
        ''' <returns>The new Mob Unique Id</returns>
        ''' <remarks></remarks>
        Public Function SpawnMob(ByVal mobID As UInteger, ByVal type As Byte, ByVal position As Position,
                                 ByVal angle As UInteger, ByVal spotID As Long, Optional ByVal channelID As UInt32 = UInt32.MaxValue) As UInteger
            Dim mob_ As SilkroadObject = GetObject(mobID)

            Dim tmp As New cMonster
            tmp.UniqueID = Id_Gen.GetUnqiueId
            tmp.Pk2ID = mob_.Pk2ID
            tmp.PosTracker = New cPositionTracker(position, mob_.WalkSpeed, mob_.RunSpeed, mob_.BerserkSpeed)
            tmp.PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Walking
            tmp.PositionSpawn = position
            tmp.SpotID = SpotID
            tmp.MobType = type
            tmp.HPCur = mob_.Hp

            If channelID = UInt32.MaxValue Then
                tmp.ChannelId = Settings.ServerWorldChannel
            Else
                tmp.ChannelId = channelID
            End If

            Try
                'Try Catch, due to several spawn errors from gm's
                Select Case type
                    Case 0
                        tmp.HPCur = mob_.Hp * MobMultiplierHP.Normal
                    Case 1
                        tmp.HPCur = mob_.Hp * MobMultiplierHP.Champion
                    Case 3
                        tmp.HPCur = mob_.Hp
                    Case 4
                        tmp.HPCur = mob_.Hp * MobMultiplierHP.Giant
                    Case 5
                        tmp.HPCur = mob_.Hp * MobMultiplierHP.Titan
                    Case 6
                        tmp.HPCur = mob_.Hp * MobMultiplierHP.Elite
                    Case 7
                        tmp.HPCur = mob_.Hp * MobMultiplierHP.Strong
                    Case 16
                        tmp.HPCur = mob_.Hp * MobMultiplierHP.Party_Normal
                    Case 17
                        tmp.HPCur = mob_.Hp * MobMultiplierHP.Party_Champ
                    Case 20
                        tmp.HPCur = mob_.Hp * MobMultiplierHP.Party_Giant
                End Select
            Catch ex As Exception
                tmp.HPCur = mob_.Hp
            End Try

            tmp.HPMax = tmp.HPCur
            MobList.Add(tmp.UniqueID, tmp)

            'Send Unique Spawn..
            If IsUnique(tmp.Pk2ID) Or tmp.MobType = 3 Then
                SendUniqueSpawn(mobID)
            End If

            'Add it to Respawn...
            If SpotID >= 0 Then
                GetRespawn(SpotID).SpawnCount += 1
            End If


            'For refindex As Integer = 0 To Server.MaxClients - 1
            '    Dim socket As Socket = Server.ClientList.GetSocket(refindex)
            '    Dim player As cCharacter = PlayerData(refindex)
            '    'Check if Player is ingame
            '    If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
            '        If CheckRange(player.Position, Position) Then
            '            If PlayerData(refindex).SpawnedMonsters.Contains(tmp.UniqueID) = False Then
            '                Dim tmpSpawn As New GroupSpawn
            '                tmpSpawn.AddObject(tmp.UniqueID)
            '                SendNotice(tmp.UniqueID & " type: " & mob_.TypeName)
            '                tmpSpawn.Send(refindex, GroupSpawn.GroupSpawnMode.SPAWN)
            '                PlayerData(refindex).SpawnedMonsters.Add(tmp.UniqueID)
            '            End If
            '        End If
            '    End If
            'Next refindex

            Return tmp.UniqueID
        End Function

        Public Sub RemoveMob(ByVal uniqueID As Integer)
            If MobList.ContainsKey(uniqueID) Then
                Dim mob As cMonster = MobList(uniqueID)
                Server.SendIfMobIsSpawned(CreateSingleDespawnPacket(mob.UniqueID), mob.UniqueID)
                MobList.Remove(uniqueID)

                If mob.SpotID >= 0 Then
                    GetRespawn(mob.SpotID).SpawnCount -= 1
                End If

                'Better double Check it, normally this is done at despawn
                For i = 0 To Server.MaxClients - 1
                    If PlayerData(i) IsNot Nothing Then
                        If PlayerData(i).SpawnedMonsters.Contains(mob.UniqueID) = True Then
                            PlayerData(i).SpawnedMonsters.Remove(mob.UniqueID)
                        End If
                    End If
                Next
            End If

        End Sub


        Public Sub RemoveMob(ByVal uniqueIDs() As UInteger)
            Dim spawnCollector As New GroupSpawn

            For i = 0 To Server.MaxClients - 1
                If PlayerData(i) IsNot Nothing Then
                    For j = 0 To uniqueIDs.Count - 1
                        If PlayerData(i).SpawnedMonsters.Contains(uniqueIDs(j)) = True Then
                            spawnCollector.AddObject(uniqueIDs(j))
                            PlayerData(i).SpawnedMonsters.Remove(uniqueIDs(j))
                        End If
                    Next

                    If spawnCollector.Count > 0 Then
                        spawnCollector.Send(i, GroupSpawn.GroupSpawnMode.DESPAWN)
                        spawnCollector.Clear()
                    End If
                End If
            Next

            For i = 0 To uniqueIDs.Count - 1
                'Single Despawn in case of GroupDespawn Failure   
                If MobList.ContainsKey(uniqueIDs(i)) Then
                    'Adjusting spawn mgmt
                    Dim mob As cMonster = MobList(uniqueIDs(i))
                    If mob.SpotID >= 0 Then
                        GetRespawn(mob.SpotID).SpawnCount -= 1
                    End If

                    MobList.Remove(uniqueIDs(i))
                End If
            Next


        End Sub


        Public Sub SendUniqueSpawn(ByVal pk2ID As UInteger)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_UNIQUE_ANNONCE)
            writer.Byte(5)
            'Spawn
            writer.Byte(&HC)
            writer.DWord(PK2ID)
            Server.SendToAllIngame(writer.GetBytes)
        End Sub

        Public Sub SendGlobalStatusInfo(ByVal mode1 As UShort, Optional ByVal mode2 As Byte = Nothing)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_UNIQUE_ANNONCE)
            writer.Word(&HC1D)
            writer.Byte(1)
            writer.Byte(1)
            Server.SendToAllIngame(writer.GetBytes)
        End Sub

        Public Sub SendUniqueKill(ByVal pk2ID As UInteger, ByVal killName As String)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_UNIQUE_ANNONCE)
            writer.Byte(6)'Kill
            writer.Byte(&HC)
            writer.DWord(pk2ID)
            writer.Word(KillName.Length)
            writer.String(KillName)
            Server.SendToAllIngame(writer.GetBytes)
        End Sub

        Enum MobMultiplierHP
            Normal = 1
            Champion = 2
            Unique = 2
            Giant = 20
            Titan = 100
            Elite = 10
            Strong = 30
            Party_Normal = 10
            Party_Champ = 20
            Party_Giant = 200
        End Enum

        Enum MobMultiplierExp
            Normal = 1
            Champion = 2
            Unique = 7
            Giant = 3
            Titan = 25
            Elite = 4
            Strong = 3
            Party_Normal = 5
            Party_Champ = 6
            Party_Giant = 10
        End Enum
    End Module
End Namespace
