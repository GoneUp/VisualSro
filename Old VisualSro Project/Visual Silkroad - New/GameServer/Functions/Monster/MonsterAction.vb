Imports SRFramework

Namespace Functions
    Module MonsterAction
        Public Sub KillMob(ByVal uniqueID As Integer)
            If MobList.ContainsKey(uniqueID) = False Then
                Exit Sub
            End If

            MobList(uniqueID).HPCur = 0
            MobList(uniqueID).Death = True
            MobList(uniqueID).DeathRemoveTime = Date.Now.AddSeconds(5)
            UpdateState(0, 2, MobList(uniqueID))

            DropMonsterItems(uniqueID)

            Dim tmp_ As Integer = MobGetPlayerWithMostDamage(uniqueID)
            If tmp_ >= 0 Then
                If MobList(uniqueID).MobType = 3 Then
                    SendUniqueKill(MobList(uniqueID).Pk2ID, PlayerData(tmp_).CharacterName)
                End If

                If Settings.ModGeneral And Settings.ModDamage Then
                    GameMod.Damage.SendDamageInfo(uniqueID)
                End If
            End If
        End Sub

        Public Function MobGetPlayerWithMostDamage(ByVal UniqueID As Integer) As Integer
            Dim MostIndex As Integer = -1
            Dim MostDamage As UInteger
            Dim Mob_ As cMonster = MobList(UniqueID)

            For i = 0 To Mob_.DamageFromPlayer.Count - 1
                If Mob_.DamageFromPlayer(i).Damage > MostDamage Then
                    MostDamage = Mob_.DamageFromPlayer(i).Damage
                    MostIndex = Mob_.DamageFromPlayer(i).PlayerIndex
                End If
            Next
            Return MostIndex
        End Function


        Public Function MobGetPlayerWithMostDamage(ByVal UniqueID As Integer, ByVal Attacking As Boolean) As Integer
            Dim MostIndex As Integer = -1
            Dim MostDamage As Long = -1
            Dim Mob_ As cMonster = MobList(UniqueID)

            For i = 0 To Mob_.DamageFromPlayer.Count - 1
                If Mob_.DamageFromPlayer(i).Damage > MostDamage Then
                    MostDamage = Mob_.DamageFromPlayer(i).Damage
                    MostIndex = Mob_.DamageFromPlayer(i).PlayerIndex
                End If
            Next
            Return MostIndex
        End Function

        Public Sub MoveMob(ByVal uniqueID As Integer, ByVal toPos As Position)
            Dim writer As New PacketWriter
            SendMoveObject(writer, uniqueID, toPos, MobList(uniqueID).Position, True)

            Server.SendIfMobIsSpawned(writer.GetBytes, MobList(uniqueID).UniqueID)
            MobList(uniqueID).PosTracker.Move(toPos)
        End Sub

        Public Sub MoveMobToUser(ByVal uniqueID As Integer, ByVal toPos As Position, ByVal range As Integer)
            Dim distanceX As Double = MobList(uniqueID).Position.ToGameX - toPos.ToGameX
            Dim distanceY As Double = MobList(uniqueID).Position.ToGameY - toPos.ToGameY
            Dim distance As Double = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY))

            If distance > range Then
                Dim cosinus As Double = Math.Cos(distanceX / distance)
                Dim sinus As Double = Math.Sin(distanceY / distance)

                Dim distanceXNew As Double = range * cosinus
                Dim distanceYNew As Double = range * sinus

                Dim newX As Single = toPos.ToGameX + distanceXNew
                Dim newY As Single = toPos.ToGameY + distanceYNew
                toPos.X = GetXOffset(newX)
                toPos.Y = GetYOffset(newY)
                toPos.XSector = GetXSecFromGameX(newX)
                toPos.YSector = GetYSecFromGameY(newY)
            End If

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_MOVEMENT)
            writer.DWord(uniqueID)
            writer.Byte(1)
            'destination
            writer.Byte(toPos.XSector)
            writer.Byte(toPos.YSector)

            If IsInCave(toPos) = False Then
                writer.Byte(BitConverter.GetBytes(CShort(toPos.X)))
                writer.Byte(BitConverter.GetBytes(CShort(toPos.Z)))
                writer.Byte(BitConverter.GetBytes(CShort(toPos.Y)))
            Else
                'In Cave
                writer.Byte(BitConverter.GetBytes(CInt(toPos.X)))
                writer.Byte(BitConverter.GetBytes(CInt(toPos.Z)))
                writer.Byte(BitConverter.GetBytes(CInt(toPos.Y)))
            End If

            writer.Byte(0)
            '1= source

            Server.SendIfMobIsSpawned(writer.GetBytes, MobList(uniqueID).UniqueID)
            MobList(uniqueID).PosTracker.Move(toPos)
        End Sub


        Public Sub GetEXPFromMob(ByVal mob As cMonster)
            Dim ref_ As SilkroadObject = GetObject(mob.Pk2ID)
            For i = 0 To mob.DamageFromPlayer.Count - 1
                Dim Index_ As Integer = mob.DamageFromPlayer(i).PlayerIndex
                Dim dmgPercent As Double = mob.DamageFromPlayer(i).Damage / mob.HPMax

                If PlayerData(Index_) IsNot Nothing Then
                    Dim balance As Double
                    'The Level factor...
                    If CSng(ref_.Level) - PlayerData(Index_).Level > 0 Then
                        'Mob is higher then you
                        balance = (1 + ((CSng(ref_.Level) - CSng(PlayerData(Index_).Level)) / 10))
                    Else
                        'Mob is lower then you
                        If PlayerData(Index_).Level - CSng(ref_.Level) < 100 Then
                            balance = (1 + ((CSng(ref_.Level) - CSng(PlayerData(Index_).Level)) / 100))
                        Else
                            balance = 0.01
                        End If
                    End If

                    Dim higehstMastery As cMastery = GetHighestPlayerMastery(Index_)
                    Dim gap As Integer = (CInt(PlayerData(Index_).Level) - CInt(higehstMastery.Level))
                    Dim gapFactorXP As Double
                    Dim gapFactorSP As Double
                    'Gap 0 = 100 EXP; 100 SP
                    'Gap 1 = 90 EXP; 110 SP
                    'Gap 2 = 80 EXP; 120 SP
                    'Gap 3 = 70 EXP; 130 SP
                    'Gap 4 = 60 EXP; 140 SP
                    'Gap 5 = 50 EXP; 150 SP
                    'Gap 6 = 40 EXP; 160 SP
                    'Gap 7 = 30 EXP; 170 SP
                    'Gap 8 = 20 EXP; 180 SP
                    'Gap 9 = 10 EXP; 190 SP

                    If gap > 9 Then
                        gapFactorXP = 0.1
                        gapFactorSP = 1.9
                    ElseIf gap <= 0 Then
                        gapFactorXP = 1
                        gapFactorSP = 1
                    Else
                        'Gap 1-9
                        gapFactorXP = 1 - (gap / 10)
                        gapFactorSP = 1 + (gap / 10)
                    End If

                    Dim exp As ULong = (((ref_.Exp * GetMobExpMultiplier(mob.MobType)) * dmgPercent * balance * gapFactorXP) * Settings.ServerXPRate)
                    Dim sp As ULong = ((ref_.Exp * dmgPercent * balance * gapFactorSP) * Settings.ServerSPRate)

                    GetXP(exp, sp, Index_, mob.UniqueID)
                End If
            Next
        End Sub

        Public Sub SetMobWalking(uniqueId As UInt32)
            If MobList.ContainsKey(uniqueId) Then
                MobList(uniqueId).PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Walking
                UpdateState(1, 2, MobList(uniqueId))
            End If
        End Sub


        Public Sub SetMobRunning(uniqueId As UInt32)
            If MobList.ContainsKey(uniqueId) Then
                MobList(uniqueId).PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Running
                UpdateState(1, 3, MobList(uniqueId))
            End If
        End Sub


        Public Function GetMobExpMultiplier(ByVal type As Byte) As Byte
            Select Case type
                Case 0
                    Return MobMultiplierExp.Normal
                Case 1
                    Return MobMultiplierExp.Champion
                Case 3
                    Return MobMultiplierExp.Unique
                Case 4
                    Return MobMultiplierExp.Giant
                Case 5
                    Return MobMultiplierExp.Titan
                Case 6
                    Return MobMultiplierExp.Elite
                Case 16
                    Return MobMultiplierExp.Party_Normal
                Case 17
                    Return MobMultiplierExp.Party_Champ
                Case 20
                    Return MobMultiplierExp.Party_Giant
                Case Else
                    Return 1
            End Select
        End Function
    End Module
End Namespace
