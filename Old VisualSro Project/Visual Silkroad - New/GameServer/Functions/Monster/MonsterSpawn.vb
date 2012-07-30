Imports System.Net.Sockets
Imports SRFramework

Namespace Functions
    Module MonsterSpawn
        Public Sub CreateMonsterSpawnPacket(ByVal mob As cMonster, ByVal obj As SilkroadObject, ByVal writer As PacketWriter, ByVal includePacketHeader As Boolean)
            If includePacketHeader Then
                writer.Create(ServerOpcodes.SingleSpawn)
            End If

            writer.DWord(mob.Pk2ID)
            writer.DWord(mob.UniqueID)
            writer.Byte(mob.Position.XSector)
            writer.Byte(mob.Position.YSector)
            writer.Float(mob.Position.X)
            writer.Float(mob.Position.Z)
            writer.Float(mob.Position.Y)

            writer.Word(mob.Angle)
            writer.Byte(mob.Pos_Tracker.MoveState)
            'dest
            If mob.Pos_Tracker.SpeedMode = cPositionTracker.enumSpeedMode.Walking Then
                writer.Byte(0) 'Walking
            Else
                writer.Byte(1)  'Running + Zerk
            End If

            If mob.Pos_Tracker.MoveState = cPositionTracker.enumMoveState.Standing Then
                writer.Byte(0)
                'dest
                writer.Word(mob.Angle)
            ElseIf mob.Pos_Tracker.MoveState = cPositionTracker.enumMoveState.Walking Then
                writer.Byte(mob.Pos_Tracker.WalkPos.XSector)
                writer.Byte(mob.Pos_Tracker.WalkPos.YSector)
                writer.Byte(BitConverter.GetBytes(CShort(mob.Pos_Tracker.WalkPos.X)))
                writer.Byte(BitConverter.GetBytes(CShort(mob.Pos_Tracker.WalkPos.Z)))
                writer.Byte(BitConverter.GetBytes(CShort(mob.Pos_Tracker.WalkPos.Y)))
            End If

            Dim alive As Boolean = True
            If mob.Death = True Then alive = False
            'writer.Byte(Convert.ToByte(alive)) ' alive flag
            writer.Byte(1)
            writer.Byte(0)   'action flag 
            writer.Byte(0) 'jobmode? 3=normal?
            writer.Byte(0)  'berserk activated


            writer.Float(obj.WalkSpeed)     'walkspeed
            writer.Float(obj.RunSpeed) 'runspeed
            writer.Float(obj.BerserkSpeed) 'berserkerspeed

            writer.Byte(0)  'unknwown  
            writer.Byte(2)  'unknwown  
            writer.Byte(1)
            writer.Byte(5) 'mhm
            writer.Byte(mob.Mob_Type) '
        End Sub

        ''' <summary>
        ''' Spawns a new Mob
        ''' </summary>
        ''' <param name="MobID"></param>
        ''' <param name="Type"></param>
        ''' <param name="Position"></param>
        ''' <param name="Angle"></param>
        ''' <param name="SpotID"></param>
        ''' <returns>The new Mob Unique Id</returns>
        ''' <remarks></remarks>
        Public Function SpawnMob(ByVal MobID As UInteger, ByVal Type As Byte, ByVal Position As Position,
                                 ByVal Angle As UInteger, ByVal SpotID As Long) As UInteger
            Dim mob_ As SilkroadObject = GetObject(MobID)
            Dim tmp As New cMonster
            tmp.UniqueID = Id_Gen.GetUnqiueId
            tmp.Pk2ID = mob_.Pk2ID
            tmp.Pos_Tracker = New cPositionTracker(Position, mob_.WalkSpeed, mob_.RunSpeed, mob_.BerserkSpeed)
            tmp.Pos_Tracker.SpeedMode = cPositionTracker.enumSpeedMode.Walking
            tmp.Position_Spawn = Position
            tmp.SpotID = SpotID
            tmp.Mob_Type = Type
            tmp.HP_Cur = mob_.Hp

            Try
                'Try Catch, due to several spawn errors from gm's
                Select Case Type
                    Case 0
                        tmp.HP_Cur = mob_.Hp * MobMultiplierHP.Normal
                    Case 1
                        tmp.HP_Cur = mob_.Hp * MobMultiplierHP.Champion
                    Case 3
                        tmp.HP_Cur = mob_.Hp
                    Case 4
                        tmp.HP_Cur = mob_.Hp * MobMultiplierHP.Giant
                    Case 5
                        tmp.HP_Cur = mob_.Hp * MobMultiplierHP.Titan
                    Case 6
                        tmp.HP_Cur = mob_.Hp * MobMultiplierHP.Elite
                    Case 7
                        tmp.HP_Cur = mob_.Hp * MobMultiplierHP.Strong
                    Case 16
                        tmp.HP_Cur = mob_.Hp * MobMultiplierHP.Party_Normal
                    Case 17
                        tmp.HP_Cur = mob_.Hp * MobMultiplierHP.Party_Champ
                    Case 20
                        tmp.HP_Cur = mob_.Hp * MobMultiplierHP.Party_Giant
                End Select
            Catch ex As Exception
                tmp.HP_Cur = mob_.Hp
            End Try

            tmp.HP_Max = tmp.HP_Cur
            MobList.Add(tmp.UniqueID, tmp)

            'Send Unique Spawn..
            If IsUnique(tmp.Pk2ID) Or tmp.Mob_Type = 3 Then
                SendUniqueSpawn(MobID)
            End If

            'Add it to Respawn...
            If SpotID >= 0 Then
                GetRespawn(SpotID).SpawnCount += 1
            End If


            For refindex As Integer = 0 To Server.MaxClients
                Dim socket As Socket = ClientList.GetSocket(refindex)
                Dim player As [cChar] = PlayerData(refindex)
                'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                    If CheckRange(player.Position, Position) Then
                        If PlayerData(refindex).SpawnedMonsters.Contains(tmp.UniqueID) = False Then
                            Dim tmpSpawn As New GroupSpawn
                            tmpSpawn.AddObject(tmp.UniqueID)
                            SendNotice(tmp.UniqueID & " type: " & mob_.TypeName)
                            tmpSpawn.Send(refindex, GroupSpawn.GroupSpawnMode.SPAWN)
                            PlayerData(refindex).SpawnedMonsters.Add(tmp.UniqueID)
                        End If
                    End If
                End If
            Next refindex

            Return tmp.UniqueID
        End Function

        Public Sub RemoveMob(ByVal UniqueID As Integer)
            Dim _mob As cMonster = MobList(UniqueID)
            Server.SendIfMobIsSpawned(CreateDespawnPacket(_mob.UniqueID), _mob.UniqueID)
            MobList.Remove(UniqueID)

            If _mob.SpotID >= 0 Then
                GetRespawn(_mob.SpotID).SpawnCount -= 1
            End If


            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).SpawnedMonsters.Contains(_mob.UniqueID) = True Then
                        PlayerData(i).SpawnedMonsters.Remove(_mob.UniqueID)
                    End If
                End If
            Next
        End Sub

        Public Sub SendUniqueSpawn(ByVal PK2ID As UInteger)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.UniqueAnnonce)
            writer.Byte(5)
            'Spawn
            writer.Byte(&HC)
            writer.DWord(PK2ID)
            Server.SendToAllIngame(writer.GetBytes)
        End Sub

        Public Sub SendGlobalStatusInfo(ByVal mode1 As UShort, Optional ByVal mode2 As Byte = Nothing)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.UniqueAnnonce)
            writer.Word(&HC1D)
            writer.Byte(1)
            writer.Byte(1)
            Server.SendToAllIngame(writer.GetBytes)
        End Sub

        Public Sub SendUniqueKill(ByVal PK2ID As UInteger, ByVal KillName As String)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.UniqueAnnonce)
            writer.Byte(6)
            'Kill
            writer.Byte(&HC)
            writer.DWord(PK2ID)
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
