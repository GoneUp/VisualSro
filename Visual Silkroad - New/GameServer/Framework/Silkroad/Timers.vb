Imports System.Timers
Namespace GameServer.Functions
    Module Timers
        Public PlayerAttackTimer As Timer() = New Timer(14999) {}
        Public PlayerMoveTimer As Timer() = New Timer(14999) {}
        Public PlayerBerserkTimer As Timer() = New Timer(14999) {}
        Public PlayerAutoHeal As New Timer
        Public MonsterMovement As New Timer
        Public MonsterCheck As New Timer
        Public MonsterRespawn As New Timer
        Public MonsterAttack As Timer() = New Timer(14999) {}
        Public PickUpTimer As Timer() = New Timer(14999) {}
        Public UsingItemTimer As Timer() = New Timer(14999) {}
        Public SitUpTimer As Timer() = New Timer(14999) {}
        Public DatabaseTimer As New Timer

        Public Sub LoadTimers(ByVal TimerCount As Integer)
            Log.WriteSystemLog("Loading Timers...")

            Try
                ReDim PlayerAttackTimer(TimerCount), PlayerMoveTimer(TimerCount), PickUpTimer(TimerCount), MonsterAttack(TimerCount), PlayerBerserkTimer(TimerCount), UsingItemTimer(TimerCount), SitUpTimer(TimerCount)

                For i As Integer = 0 To TimerCount - 1
                    PlayerAttackTimer(i) = New Timer()
                    AddHandler PlayerAttackTimer(i).Elapsed, AddressOf AttackTimer_Elapsed
                    PlayerMoveTimer(i) = New Timer()
                    AddHandler PlayerMoveTimer(i).Elapsed, AddressOf PlayerMoveTimer_Elapsed
                    PlayerBerserkTimer(i) = New Timer()
                    AddHandler PlayerBerserkTimer(i).Elapsed, AddressOf PlayerBerserkTimer_Elapsed
                    UsingItemTimer(i) = New Timer()
                    AddHandler UsingItemTimer(i).Elapsed, AddressOf UseItemTimer_Elapsed
                    SitUpTimer(i) = New Timer()
                    AddHandler SitUpTimer(i).Elapsed, AddressOf SitUpTimer_Elapsed
                    PickUpTimer(i) = New Timer()
                    AddHandler PickUpTimer(i).Elapsed, AddressOf SitUpTimer_Elapsed
                Next

                AddHandler MonsterCheck.Elapsed, AddressOf MonsterCheck_Elapsed
                AddHandler MonsterRespawn.Elapsed, AddressOf MonsterRespawn_Elapsed
                AddHandler MonsterMovement.Elapsed, AddressOf MonsterMovement_Elapsed
                AddHandler PlayerAutoHeal.Elapsed, AddressOf PlayerAutoHeal_Elapsed
                AddHandler DatabaseTimer.Elapsed, AddressOf DatabaseTimer_Elapsed

                'Start Timers
                MonsterCheck.Interval = 5000
                MonsterCheck.Start()

                MonsterMovement.Interval = 3500
                MonsterMovement.Start()

                MonsterRespawn.Interval = 7500
                MonsterRespawn.Start()

                PlayerAutoHeal.Interval = 5000
                PlayerAutoHeal.Start()

                DatabaseTimer.Interval = 30000
                DatabaseTimer.Start()

            Catch ex As Exception

            End Try
            Log.WriteSystemLog("Timers loaded!")
        End Sub

        Public Sub AttackTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = Information.LBound(PlayerAttackTimer, 1) To Information.UBound(PlayerAttackTimer, 1)
                    If Object.ReferenceEquals(PlayerAttackTimer(i), objB) Then
                        Index = i
                        Exit For
                    End If
                Next

                PlayerAttackTimer(Index).Stop()
                If Index <> -1 And PlayerData(Index) IsNot Nothing Then
                    PlayerData(Index).Busy = False
                    PlayerData(Index).Attacking = False

                    If PlayerData(Index).AttackedId <> 0 And MobList.ContainsKey(PlayerData(Index).AttackedId) Then
                        Dim mob_ As cMonster = MobList(PlayerData(Index).AttackedId)

                        If mob_.Death = False Then
                            If PlayerData(Index).AttackType = AttackType_.Normal Then
                                PlayerAttackNormal(Index, mob_.UniqueID)
                            ElseIf PlayerData(Index).AttackType = AttackType_.Skill Then
                                PlayerAttackEndSkill(Index)
                            End If
                        End If

                    Else
                        If PlayerData(Index).AttackType = AttackType_.Buff Then
                            PlayerBuff_EndCasting(Index)
                        End If
                    End If
                End If



            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
            End Try

        End Sub

        Public Sub UseItemTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = Information.LBound(UsingItemTimer, 1) To Information.UBound(UsingItemTimer, 1)
                    If Object.ReferenceEquals(UsingItemTimer(i), objB) Then
                        Index = i
                        Exit For
                    End If
                Next

                UsingItemTimer(Index).Stop()
                If Index <> -1 Then
                    Select Case Functions.PlayerData(Index).UsedItem
                        Case UseItemTypes.Return_Scroll
                            PlayerData(Index).Position_Recall = Functions.PlayerData(Index).Position  'Save Pos
                            PlayerData(Index).Position = Functions.PlayerData(Index).Position_Return 'Set new Pos
                            'Save to DB
                            DataBase.SaveQuery(String.Format("UPDATE positions SET recall_xsect='{0}', recall_ysect='{1}', recall_xpos='{2}', recall_zpos='{3}', recall_ypos='{4}' where OwnerCharID='{5}'", PlayerData(Index).Position_Recall.XSector, PlayerData(Index).Position_Recall.YSector, Math.Round(PlayerData(Index).Position_Recall.X), Math.Round(PlayerData(Index).Position_Recall.Z), Math.Round(PlayerData(Index).Position_Recall.Y), PlayerData(Index).CharacterId))
                            DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index).Position.XSector, PlayerData(Index).Position.YSector, Math.Round(PlayerData(Index).Position.X), Math.Round(PlayerData(Index).Position.Z), Math.Round(PlayerData(Index).Position.Y), PlayerData(Index).CharacterId))

                            OnTeleportUser(Index, PlayerData(Index).Position.XSector, PlayerData(Index).Position.YSector)
                            PlayerData(Index).Busy = False
                            PlayerData(Index).UsedItem = UseItemTypes.None


                        Case UseItemTypes.Reverse_Scroll_Recall
                            PlayerData(Index).Position = PlayerData(Index).Position_Recall
                            DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index).Position.XSector, PlayerData(Index).Position.YSector, Math.Round(PlayerData(Index).Position.X), Math.Round(PlayerData(Index).Position.Z), Math.Round(PlayerData(Index).Position.Y), PlayerData(Index).CharacterId))

                            OnTeleportUser(Index, PlayerData(Index).Position.XSector, PlayerData(Index).Position.YSector)
                            PlayerData(Index).Busy = False
                            PlayerData(Index).UsedItem = UseItemTypes.None

                        Case UseItemTypes.Reverse_Scroll_Dead
                            PlayerData(Index).Position = PlayerData(Index).Position_Dead
                            DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index).Position.XSector, PlayerData(Index).Position.YSector, Math.Round(PlayerData(Index).Position.X), Math.Round(PlayerData(Index).Position.Z), Math.Round(PlayerData(Index).Position.Y), PlayerData(Index).CharacterId))

                            OnTeleportUser(Index, PlayerData(Index).Position.XSector, PlayerData(Index).Position.YSector)
                            PlayerData(Index).Busy = False
                            PlayerData(Index).UsedItem = UseItemTypes.None

                        Case UseItemTypes.Reverse_Scroll_Point
                            Dim point As ReversePoint_ = GetReversePointByID(PlayerData(Index).UsedItemParameter)
                            PlayerData(Index).Position = point.Position
                            DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index).Position.XSector, PlayerData(Index).Position.YSector, Math.Round(PlayerData(Index).Position.X), Math.Round(PlayerData(Index).Position.Z), Math.Round(PlayerData(Index).Position.Y), PlayerData(Index).CharacterId))

                            OnTeleportUser(Index, PlayerData(Index).Position.XSector, PlayerData(Index).Position.YSector)
                            PlayerData(Index).Busy = False
                            PlayerData(Index).UsedItem = UseItemTypes.None
                            PlayerData(Index).UsedItemParameter = 0

                    End Select
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
            End Try


        End Sub

        Public Sub SitUpTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = Information.LBound(SitUpTimer, 1) To Information.UBound(SitUpTimer, 1)
                    If Object.ReferenceEquals(SitUpTimer(i), objB) Then
                        Index = i
                        Exit For
                    End If
                Next

                If Index <> -1 Then
                    SitUpTimer(Index).Stop()

                    If PlayerData(Index).ActionFlag = 4 Then
                        PlayerData(Index).Busy = True
                    ElseIf PlayerData(Index).ActionFlag = 0 Then
                        PlayerData(Index).Busy = False
                    End If
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
            End Try
        End Sub

        Public Sub PickUpTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = Information.LBound(PickUpTimer, 1) To Information.UBound(PickUpTimer, 1)
                    If Object.ReferenceEquals(PickUpTimer(i), objB) Then
                        Index = i
                        Exit For
                    End If
                Next

                If Index <> -1 Then
                    PickUpTimer(Index).Stop()

                    If ItemList.ContainsKey(PlayerData(Index).PickUpId) Then
                        PickUp(PlayerData(Index).PickUpId, Index)
                    End If

                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
            End Try
        End Sub

        Public Sub MonsterCheck_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            MonsterCheck.Stop()

            Try
                Dim stopwatch As New Stopwatch
                stopwatch.Start()



                Dim tmplist As Array = MobList.Keys.ToArray
                For Each key In tmplist
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)

                        If Mob_.Death = True Or Mob_.HP_Cur <= 0 Then
                            Dim wert As Integer = Date.Compare(Mob_.DeathRemoveTime, Date.Now)
                            If wert = -1 Then
                                'Abgelaufen
                                RemoveMob(Mob_.UniqueID)
                            End If
                        ElseIf IsInSaveZone(Mob_.Position) Then
                            RemoveMob(Mob_.UniqueID)
                        ElseIf Mob_.GetsAttacked = True Then
                            'Attack back...
                            Mob_.AttackTimer_Start(10)
                        End If
                    End If
                Next

                stopwatch.Stop()
                Debug.Print("MC: " & stopwatch.ElapsedMilliseconds & "ms. Count:" & tmplist.Length)
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MC") '
            End Try

            MonsterCheck.Start() 'restart Timer
        End Sub


        Public Sub MonsterRespawn_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            MonsterRespawn.Stop()
            Try
                Dim stopwatch As New Stopwatch
                stopwatch.Start()

                CheckForRespawns()

                stopwatch.Stop()
                Debug.Print("MR: " & stopwatch.ElapsedMilliseconds & "ms")
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MR") '
            End Try

            MonsterRespawn.Start() 'restart Timer
        End Sub

        Public Sub MonsterMovement_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            MonsterMovement.Stop()

            Try
                Dim stopwatch As New Stopwatch
                stopwatch.Start()

                Dim tmplist As Array = MobList.Keys.ToArray
                For Each key In tmplist
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)
                        Dim obj As Object_ = GetObjectById(Mob_.Pk2ID)
                        If Rand.Next(0, 3) = 0 Then
                            If Mob_.Death = False And Mob_.Pos_Tracker.MoveState = cPositionTracker.enumMoveState.Standing And obj.WalkSpeed > 0 And Mob_.GetsAttacked = False And Mob_.IsAttacking = False Then
                                Dim Dist_FromSpawn As Single = CalculateDistance(Mob_.Position, Mob_.Position_Spawn)

                                If Dist_FromSpawn < Settings.Server_Range / 1.25 Then
                                    Dim ToX As Single = Mob_.Position.ToGameX + Rand.Next(-15, +15)
                                    Dim ToY As Single = Mob_.Position.ToGameY + Rand.Next(-15, +15)

                                    Dim ToPos As New Position
                                    ToPos.XSector = GetXSecFromGameX(ToX)
                                    ToPos.YSector = GetYSecFromGameY(ToY)
                                    ToPos.X = GetXOffset(ToX)
                                    ToPos.Z = Mob_.Position.Z
                                    ToPos.Y = GetYOffset(ToY)

                                    If ToPos.X = Mob_.Position.X And ToPos.Y = Mob_.Position.Y Then
                                        ToPos.X += 10
                                    End If

                                    MoveMob(Mob_.UniqueID, ToPos)
                                Else
                                    MoveMob(Mob_.UniqueID, Mob_.Position_Spawn)
                                End If
                            End If
                        End If
                    End If
                Next

                stopwatch.Stop()
                Debug.Print("MM: " & stopwatch.ElapsedMilliseconds & "ms")

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MM") '
            End Try

            MonsterMovement.Start()
        End Sub

        Public Sub PlayerMoveTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = Information.LBound(PlayerMoveTimer, 1) To Information.UBound(PickUpTimer, 1)
                    If Object.ReferenceEquals(PlayerMoveTimer(i), objB) Then
                        Index = i
                        Exit For
                    End If
                Next

                If Index <> -1 Then
                    PlayerMoveTimer(Index).Stop()

                    If PlayerData(Index) IsNot Nothing Then
                        If PlayerData(Index).Pos_Tracker.MoveState = cPositionTracker.enumMoveState.Walking Then
                            Dim new_pos As Position = PlayerData(Index).Pos_Tracker.GetCurPos()
                            ObjectSpawnCheck(Index)
                            CheckForCaveTeleporter(Index)

                            'SendPm(Index, "secx" & new_pos.XSector & "secy" & new_pos.YSector & "X: " & new_pos.X & "Y: " & new_pos.Y & " X:" & new_pos.ToGameX & " Y: " & new_pos.ToGameY, "hh")
                            PlayerMoveTimer(Index).Start()

                        ElseIf PlayerData(Index).Pos_Tracker.MoveState = cPositionTracker.enumMoveState.Standing Then
                            ObjectSpawnCheck(Index)
                            'SendPm(Index, "Walk End", "hh")

                            CheckForCaveTeleporter(Index)
                            PlayerMoveTimer(Index).Interval = 20 * 1000
                            PlayerMoveTimer(Index).Start()
                        End If
                    End If

                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
            End Try
        End Sub

        Public Sub PlayerAutoHeal_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            PlayerAutoHeal.Stop()

            Try
                For i = 0 To PlayerData.Length - 1
                    If PlayerData(i) IsNot Nothing Then
                        If PlayerData(i).Ingame And PlayerData(i).Alive Then
                            Dim Changed_HPMP As Boolean = False
                            Select Case PlayerData(i).ActionFlag
                                Case 0
                                    'Nomral
                                    If PlayerData(i).CHP < PlayerData(i).HP Then
                                        PlayerData(i).CHP += Math.Round(PlayerData(i).HP * 0.002, 0, MidpointRounding.AwayFromZero)
                                        Changed_HPMP = True
                                    End If
                                    If PlayerData(i).CMP < PlayerData(i).MP Then
                                        PlayerData(i).CMP += Math.Round(PlayerData(i).MP * 0.002, 0, MidpointRounding.AwayFromZero)
                                        Changed_HPMP = True
                                    End If

                                Case 4
                                    'Sitting
                                    If PlayerData(i).CHP < PlayerData(i).HP Then
                                        PlayerData(i).CHP += Math.Round(PlayerData(i).HP * 0.05, 0, MidpointRounding.AwayFromZero)
                                        Changed_HPMP = True
                                    End If
                                    If PlayerData(i).CMP < PlayerData(i).MP Then
                                        PlayerData(i).CMP += Math.Round(PlayerData(i).MP * 0.05, 0, MidpointRounding.AwayFromZero)
                                        Changed_HPMP = True
                                    End If
                            End Select

                            'Check and Correct Errors...
                            If PlayerData(i).CHP > PlayerData(i).HP Then
                                PlayerData(i).CHP = PlayerData(i).HP
                            End If
                            If PlayerData(i).CMP > PlayerData(i).MP Then
                                PlayerData(i).CMP = PlayerData(i).MP
                            End If


                            If Changed_HPMP = True Then
                                UpdateHP_MP(i)
                            End If
                        End If
                    End If
                Next

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: PAH") '
            End Try

            PlayerAutoHeal.Start()
        End Sub


        Public Sub PlayerBerserkTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)

            Dim Index As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = Information.LBound(PlayerBerserkTimer, 1) To Information.UBound(PlayerBerserkTimer, 1)
                    If Object.ReferenceEquals(PlayerBerserkTimer(i), objB) Then
                        Index = i
                        Exit For
                    End If
                Next

                PlayerBerserkTimer(Index).Stop()

                If Index <> -1 And PlayerData(Index) IsNot Nothing Then
                    PlayerData(Index).Berserk = False
                    PlayerData(Index).BerserkBar = 0
                    PlayerData(Index).Pos_Tracker.SpeedMode = cPositionTracker.enumSpeedMode.Running
                    UpdateSpeeds(Index)
                    UpdateState(4, 0, Index)
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
            End Try
        End Sub

        Public Sub DatabaseTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            DatabaseTimer.Stop()

            Try
                DataBase.ExecuteQuerys()

                'Check other Timers for Crashed
                If MonsterRespawn.Enabled = False Then
                    MonsterRespawn.Start()
                End If
                If MonsterCheck.Enabled = False Then
                    MonsterCheck.Start()
                End If
                If MonsterMovement.Enabled = False Then
                    MonsterMovement.Start()
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: DB") '
            End Try

            DatabaseTimer.Start()
        End Sub
    End Module
End Namespace