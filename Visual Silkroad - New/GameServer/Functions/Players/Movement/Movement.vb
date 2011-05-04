Imports GameServer.GameServer.cPositionTracker

Namespace GameServer.Functions
    Module Movement

        Public Sub OnPlayerMovement(ByVal Index_ As Integer, ByVal packet As PacketReader)

            If PlayerData(Index_).Busy = True Or PlayerData(Index_).Attacking = True Then
                Exit Sub
            End If

            Dim tag As Byte = packet.Byte
            If tag = 1 Then
                Dim to_pos As New Position
                to_pos.XSector = packet.Byte
                to_pos.YSector = packet.Byte

                If IsInCave(to_pos) = False Then
                    to_pos.X = packet.WordInt
                    to_pos.Z = packet.WordInt
                    to_pos.Y = packet.WordInt
                Else
                    'In Cave
                    to_pos.X = packet.DWordInt
                    to_pos.Z = packet.DWordInt
                    to_pos.Y = packet.DWordInt
                End If


                OnMoveUser(Index_, to_pos)
            End If
        End Sub

        Public Sub OnMoveUser(ByVal Index_ As Integer, ByVal ToPos As Position)
            Try
                Dim Distance As Single = CalculateDistance(PlayerData(Index_).Position, ToPos)
                Dim WalkTime As Single
                Select Case PlayerData(Index_).Position_Tracker.SpeedMode
                    Case cPositionTracker.enumSpeedMode.Walking
                        WalkTime = (Distance / PlayerData(Index_).WalkSpeed) * 10000
                    Case cPositionTracker.enumSpeedMode.Running
                        WalkTime = (Distance / PlayerData(Index_).RunSpeed) * 10000
                    Case cPositionTracker.enumSpeedMode.Zerking
                        WalkTime = (Distance / PlayerData(Index_).BerserkSpeed) * 10000
                End Select

                'If Distance < 10000 Then

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Movement)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Byte(1) 'destination
                writer.Byte(ToPos.XSector)
                writer.Byte(ToPos.YSector)

                If IsInCave(ToPos) = False Then
                    writer.Byte(BitConverter.GetBytes(CShort(ToPos.X)))
                    writer.Byte(BitConverter.GetBytes(CShort(ToPos.Z)))
                    writer.Byte(BitConverter.GetBytes(CShort(ToPos.Y)))
                Else
                    'In Cave
                    writer.Byte(BitConverter.GetBytes(CInt(ToPos.X)))
                    writer.Byte(BitConverter.GetBytes(CInt(ToPos.Z)))
                    writer.Byte(BitConverter.GetBytes(CInt(ToPos.Y)))
                End If

                writer.Byte(0) '1= source

                DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

                CheckForCaveTeleporter(Index_)

                PlayerData(Index_).Position_Tracker.Move(ToPos)
                PlayerMoveTimer(Index_).Interval = 500
                PlayerMoveTimer(Index_).Start()
                ' End If

            Catch ex As Exception
                Console.WriteLine("OnMoveUser::error...")
                Debug.Write(ex)
            End Try

        End Sub

        Public Sub MoveUserToMonster(ByVal Index_ As Integer, ByVal MobUniqueID As Integer)
            Dim RefWeapon As New cItem

            If Inventorys(Index_).UserItems(6).Pk2Id <> 0 Then 'Will be used for Weapon Distance
                'Weapon
                RefWeapon = GetItemByID(Inventorys(Index_).UserItems(6).Pk2Id)
            Else
                'No Weapon
                RefWeapon.ATTACK_DISTANCE = 6
            End If


            Dim ToX As Single = GetRealX(MobList(MobUniqueID).Position.XSector, MobList(MobUniqueID).Position.X) + (1)
            Dim ToY As Single = GetRealY(MobList(MobUniqueID).Position.YSector, MobList(MobUniqueID).Position.Y) + (1)

            Dim ToPos As New Position
            ToPos.XSector = GetXSec(ToX)
            ToPos.YSector = GetYSec(ToY)
            ToPos.X = GetXOffset(ToX)
            ToPos.Z = MobList(MobUniqueID).Position.Z
            ToPos.Y = GetYOffset(ToY)

            Dim Distance As Single = CalculateDistance(PlayerData(Index_).Position, ToPos)
            Dim WalkTime As Single
            Select Case PlayerData(Index_).Position_Tracker.SpeedMode
                Case cPositionTracker.enumSpeedMode.Walking
                    WalkTime = (Distance / PlayerData(Index_).WalkSpeed) * 10000
                Case cPositionTracker.enumSpeedMode.Running
                    WalkTime = (Distance / PlayerData(Index_).RunSpeed) * 10000
                Case cPositionTracker.enumSpeedMode.Zerking
                    WalkTime = (Distance / PlayerData(Index_).BerserkSpeed) * 10000
            End Select

            PlayerData(Index_).AttackType = AttackType_.Normal
            PlayerData(Index_).AttackedId = MobList(MobUniqueID).UniqueID

            PlayerAttackTimer(Index_).Interval = WalkTime
            PlayerAttackTimer(Index_).Start()


            OnMoveUser(Index_, ToPos)
        End Sub
        Public Sub ObjectSpawnCheck(ByVal Index_ As Integer)
            Try
                Dim range As Integer = Settings.Server_Range
                ObjectDeSpawnCheck(Index_)

                '=============Players============
                For refindex As Integer = 0 To Server.MaxClients
                    If (ClientList.GetSocket(refindex) IsNot Nothing) AndAlso (PlayerData(refindex) IsNot Nothing) AndAlso ClientList.GetSocket(refindex).Connected AndAlso Index_ <> refindex Then
                        If PlayerData(refindex).SpawnedPlayers.Contains(Index_) = False And PlayerData(Index_).Invisible = False Then
                            Server.Send(CreateSpawnPacket(Index_), refindex)
                            PlayerData(refindex).SpawnedPlayers.Add(Index_)
                        End If
                        If PlayerData(Index_).SpawnedPlayers.Contains(refindex) = False Then
                            Server.Send(CreateSpawnPacket(refindex), Index_)
                            PlayerData(Index_).SpawnedPlayers.Add(refindex)
                        End If
                    End If
                Next refindex
                '===========MOBS===================

                For Each key In MobList.Keys.ToList
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)
                        If CheckRange(PlayerData(Index_).Position, Mob_.Position) Then
                            Dim obj As Object = GetObjectById(Mob_.Pk2ID)
                            If PlayerData(Index_).SpawnedMonsters.Contains(Mob_.UniqueID) = False Then
                                Server.Send(CreateMonsterSpawnPacket(Mob_, obj), Index_)
                                PlayerData(Index_).SpawnedMonsters.Add(Mob_.UniqueID)
                            End If
                        End If
                    End If
                Next

                '===========NPCS===================
                For i = 0 To NpcList.Count - 1
                    Dim _npc As cNPC = NpcList(i)
                    If CheckRange(PlayerData(Index_).Position, NpcList(i).Position) Then
                        If PlayerData(Index_).SpawnedNPCs.Contains(_npc.UniqueID) = False Then
                            Server.Send(CreateNPCGroupSpawnPacket(i), Index_)
                            PlayerData(Index_).SpawnedNPCs.Add(_npc.UniqueID)
                        End If
                    End If
                Next


                '===========ITEMS===================
                For i = 0 To ItemList.Count - 1
                    Dim _item As cItemDrop = ItemList(i)
                    If CheckRange(PlayerData(Index_).Position, ItemList(i).Position) Then
                        If PlayerData(Index_).SpawnedItems.Contains(_item.UniqueID) = False Then
                            Server.Send(CreateItemSpawnPacket(_item), Index_)
                            PlayerData(Index_).SpawnedItems.Add(_item.UniqueID)
                        End If
                    End If
                Next


            Catch ex As Exception
                Log.WriteSystemLog("SpawnCheckError:: Message: " & ex.Message & " Stack:" & ex.StackTrace)
            End Try
        End Sub


        Public Sub ObjectDeSpawnCheck(ByVal Index_ As Integer)
            Try
                For Other_Index = 0 To Server.MaxClients
                    If PlayerData(Other_Index) IsNot Nothing And PlayerData(Index_).SpawnedPlayers.Contains(Other_Index) Then
                        If CheckRange(PlayerData(Index_).Position, PlayerData(Other_Index).Position) = False Then
                            'Despawn for both
                            Server.Send(CreateDespawnPacket(PlayerData(Index_).UniqueId), Other_Index)
                            PlayerData(Other_Index).SpawnedPlayers.Remove(Index_)
                            Server.Send(CreateDespawnPacket(PlayerData(Other_Index).UniqueId), Index_)
                            PlayerData(Index_).SpawnedPlayers.Remove(Other_Index)
                        End If
                    End If
                Next

                For Each key In MobList.Keys.ToList
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)
                        If PlayerData(Index_).SpawnedMonsters.Contains(Mob_.UniqueID) = True Then
                            If CheckRange(PlayerData(Index_).Position, Mob_.Position) = False Then
                                Server.Send(CreateDespawnPacket(Mob_.UniqueID), Index_)
                                PlayerData(Index_).SpawnedMonsters.Remove(Mob_.UniqueID)
                            End If
                        End If
                    End If
                Next

                For i = 0 To NpcList.Count - 1
                    Dim _npc As cNPC = NpcList(i)
                    If PlayerData(Index_).SpawnedNPCs.Contains(_npc.UniqueID) = True Then
                        If CheckRange(PlayerData(Index_).Position, _npc.Position) = False Then
                            Server.Send(CreateDespawnPacket(_npc.UniqueID), Index_)
                            PlayerData(Index_).SpawnedNPCs.Remove(_npc.UniqueID)
                        End If
                    End If
                Next


                For i = 0 To ItemList.Count - 1
                    Dim _item As cItemDrop = ItemList(i)
                    If PlayerData(Index_).SpawnedItems.Contains(_item.UniqueID) = True Then
                        If CheckRange(PlayerData(Index_).Position, _item.Position) = False Then
                            Server.Send(CreateDespawnPacket(_item.UniqueID), Index_)
                            PlayerData(Index_).SpawnedItems.Remove(_item.UniqueID)
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try
        End Sub

        Public Sub CheckForCaveTeleporter(ByVal Index_ As Integer)
            For i = 0 To RefCaveTeleporter.Count - 1
                If CheckSectors(PlayerData(Index_).Position, RefCaveTeleporter(i).FromPosition) Then
                    Debug.Print(CalculateDistance(PlayerData(Index_).Position, RefCaveTeleporter(i).FromPosition))
                    If CalculateDistance(PlayerData(Index_).Position, RefCaveTeleporter(i).FromPosition) <= RefCaveTeleporter(i).Range Then
                        'In Range --> Teleport
                        Dim Point_ As TeleportPoint_ = GetTeleportPoint(RefCaveTeleporter(i).ToTeleporterID)

                        If PlayerData(Index_).Level < Point_.MinLevel And Point_.MinLevel > 0 Then
                            'Level too low
                            Dim writer As New PacketWriter
                            writer.Create(ServerOpcodes.Teleport_Start)
                            writer.Byte(2)
                            writer.Byte(&H15)
                            writer.Byte(&H1C)
                            Server.Send(writer.GetBytes, Index_)
                        ElseIf PlayerData(Index_).Level > Point_.MaxLevel And Point_.MaxLevel > 0 Then
                            'Level too high
                            SendNotice("Cannot Teleport because your Level is too high.", Index_)
                        Else
                            PlayerData(Index_).Busy = True
                            PlayerData(Index_).Position = Point_.ToPos

                            DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                        End If
                    End If
                End If
            Next
        End Sub

        Public Function CheckRange(ByVal Pos_1 As Position, ByVal Pos_2 As Position) As Boolean
            If CalculateDistance(Pos_1, Pos_2) <= Settings.Server_Range Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Is for checking if Mob Spawn is possible
        ''' </summary>
        ''' <param name="Pos_1"></param>
        ''' <param name="Pos_2"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckSectors(ByVal Pos_1 As Position, ByVal Pos_2 As Position) As Boolean

            '############
            '# 1 # 2 # 3#
            '# 4 # 5 # 6#
            '# 7 # 8 # 9#
            '############
            Dim PossibleSectors As New List(Of Position)
            For x = -1 To 1
                For y = -1 To 1
                    Dim pos As New Position
                    pos.XSector = Pos_1.XSector + x
                    pos.YSector = Pos_1.YSector + y
                    PossibleSectors.Add(pos)
                Next
            Next

            For Each e As Position In PossibleSectors
                If e.XSector = Pos_2.XSector And e.YSector = Pos_2.YSector Then
                    Return True
                End If
            Next

            Return False
        End Function
    End Module
End Namespace
