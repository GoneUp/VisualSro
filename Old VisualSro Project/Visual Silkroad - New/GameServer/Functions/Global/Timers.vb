Imports System.Timers
Imports SRFramework

Namespace Functions
    Module Timers
        Public PlayerAttackTimer As Timer() = New Timer(1) {}
        Public PlayerMoveTimer As Timer() = New Timer(1) {}
        Public PlayerBerserkTimer As Timer() = New Timer(1) {}
        Public PlayerExitTimer As Timer() = New Timer(1) {}
        Public MonsterAttack As Timer() = New Timer(1) {}
        Public PickUpTimer As Timer() = New Timer(1) {}
        Public UsingItemTimer As Timer() = New Timer(1) {}
        Public SitUpTimer As Timer() = New Timer(1) {}

        Private ReadOnly GeneralTimer As New Timer
        Private ReadOnly PingTimer As New Timer
        Private ReadOnly PlayerAutoHeal As New Timer
        Private ReadOnly WorldCheck As New Timer
        Private ReadOnly MonsterMovement As New Timer
        Private ReadOnly MonsterRespawn As New Timer

        Public Function LoadTimers(ByVal timerCount As Integer) As Boolean
            Log.WriteSystemLog("Loading Timers...")

            Try
                ReDim PlayerAttackTimer(timerCount), PlayerMoveTimer(timerCount), PickUpTimer(timerCount),
                    MonsterAttack(timerCount), PlayerBerserkTimer(timerCount), UsingItemTimer(timerCount),
                    SitUpTimer(timerCount), PlayerExitTimer(timerCount)

                For i As Integer = 0 To timerCount - 1
                    PlayerAttackTimer(i) = New Timer()
                    AddHandler PlayerAttackTimer(i).Elapsed, AddressOf AttackTimerElapsed
                    PlayerMoveTimer(i) = New Timer()
                    AddHandler PlayerMoveTimer(i).Elapsed, AddressOf PlayerMoveTimerElapsed
                    PlayerBerserkTimer(i) = New Timer()
                    AddHandler PlayerBerserkTimer(i).Elapsed, AddressOf PlayerBerserkTimerElapsed
                    PlayerExitTimer(i) = New Timer()
                    AddHandler PlayerExitTimer(i).Elapsed, AddressOf PlayerExitTimerElapsed
                    UsingItemTimer(i) = New Timer()
                    AddHandler UsingItemTimer(i).Elapsed, AddressOf UseItemTimerElapsed
                    SitUpTimer(i) = New Timer()
                    AddHandler SitUpTimer(i).Elapsed, AddressOf SitUpTimerElapsed
                    PickUpTimer(i) = New Timer()
                    AddHandler PickUpTimer(i).Elapsed, AddressOf SitUpTimerElapsed
                Next

                AddHandler WorldCheck.Elapsed, AddressOf WorldCheckElapsed
                AddHandler MonsterRespawn.Elapsed, AddressOf MonsterRespawnElapsed
                AddHandler MonsterMovement.Elapsed, AddressOf MonsterMovementElapsed
                AddHandler PlayerAutoHeal.Elapsed, AddressOf PlayerAutoHealElapsed
                AddHandler GeneralTimer.Elapsed, AddressOf GeneralTimerElapsed
                AddHandler PingTimer.Elapsed, AddressOf PingTimerElapsed

                'Start Timers
                WorldCheck.Interval = 5000
                WorldCheck.Start()

                MonsterMovement.Interval = 3500
                MonsterMovement.Start()

                MonsterRespawn.Interval = 7500
                MonsterRespawn.Start()

                PlayerAutoHeal.Interval = 5000
                PlayerAutoHeal.Start()

                GeneralTimer.Interval = 2500
                GeneralTimer.Start()

                PingTimer.Interval = 30000
                PingTimer.Start()

            Catch ex As Exception
                Log.WriteSystemLog("Timers loading failed! EX:" & ex.Message & " Stacktrace: " & ex.StackTrace)
                Return False
            Finally
                Log.WriteSystemLog("Timers loaded!")
            End Try

            Return True
        End Function

        Private Sub AttackTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index_ As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = LBound(PlayerAttackTimer, 1) To UBound(PlayerAttackTimer, 1)
                    If ReferenceEquals(PlayerAttackTimer(i), objB) Then
                        Index_ = i
                        Exit For
                    End If
                Next

                PlayerAttackTimer(Index_).Stop()
                If Index_ <> -1 And PlayerData(Index_) IsNot Nothing Then
                    PlayerData(Index_).Busy = False
                    PlayerData(Index_).Attacking = False

                    If PlayerData(Index_).AttackedId <> 0 And MobList.ContainsKey(PlayerData(Index_).AttackedId) Then
                        Dim mob_ As cMonster = MobList(PlayerData(Index_).AttackedId)

                        If mob_.Death = False Then
                            If PlayerData(Index_).AttackType = AttackTypes.Normal Then
                                PlayerAttackNormal(Index_, mob_.UniqueID)
                            ElseIf PlayerData(Index_).AttackType = AttackTypes.Skill Then
                                PlayerAttackEndSkill(Index_)
                            End If
                        End If

                    Else
                        If PlayerData(Index_).AttackType = AttackTypes.Buff Then
                            PlayerBuffEndCasting(Index_)
                        End If
                    End If
                End If


            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index_)
            End Try
        End Sub

        Private Sub UseItemTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index_ As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = LBound(UsingItemTimer, 1) To UBound(UsingItemTimer, 1)
                    If ReferenceEquals(UsingItemTimer(i), objB) Then
                        Index_ = i
                        Exit For
                    End If
                Next

                UsingItemTimer(Index_).Stop()
                If Index_ <> -1 Then
                    Select Case PlayerData(Index_).UsedItem
                        Case UseItemTypes.ReturnScroll
                            PlayerData(Index_).PositionRecall = PlayerData(Index_).Position  'Save Pos
                            PlayerData(Index_).SetPosition = PlayerData(Index_).PositionReturn     'Set new Pos
                            'Save to DB
                            GameDB.SaveRecallPosition(Index_)
                            GameDB.SavePosition(Index_)

                            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                            PlayerData(Index_).Busy = False
                            PlayerData(Index_).UsedItem = UseItemTypes.None


                        Case UseItemTypes.ReverseScrollRecall
                            PlayerData(Index_).SetPosition = PlayerData(Index_).PositionRecall
                            GameDB.SavePosition(Index_)

                            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                            PlayerData(Index_).Busy = False
                            PlayerData(Index_).UsedItem = UseItemTypes.None

                        Case UseItemTypes.ReverseScrollDead
                            PlayerData(Index_).SetPosition = PlayerData(Index_).PositionDead
                            GameDB.SavePosition(Index_)

                            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                            PlayerData(Index_).Busy = False
                            PlayerData(Index_).UsedItem = UseItemTypes.None

                        Case UseItemTypes.ReverseScrollPoint
                            Dim point As ReversePoint = GetReversePoint(PlayerData(Index_).UsedItemParameter)
                            PlayerData(Index_).SetPosition = point.Position
                            GameDB.SavePosition(Index_)

                            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                            PlayerData(Index_).Busy = False
                            PlayerData(Index_).UsedItem = UseItemTypes.None
                            PlayerData(Index_).UsedItemParameter = 0

                    End Select
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index_)
                '
            End Try
        End Sub

        Private Sub SitUpTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index_ As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = LBound(SitUpTimer, 1) To UBound(SitUpTimer, 1)
                    If ReferenceEquals(SitUpTimer(i), objB) Then
                        Index_ = i
                        Exit For
                    End If
                Next

                If Index_ <> -1 Then
                    SitUpTimer(Index_).Stop()

                    If PlayerData(Index_).ActionFlag = 4 Then
                        PlayerData(Index_).Busy = True
                    ElseIf PlayerData(Index_).ActionFlag = 0 Then
                        PlayerData(Index_).Busy = False
                    End If
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index_)
                '
            End Try
        End Sub

        Public Sub PickUpTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index_ As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = LBound(PickUpTimer, 1) To UBound(PickUpTimer, 1)
                    If ReferenceEquals(PickUpTimer(i), objB) Then
                        Index_ = i
                        Exit For
                    End If
                Next

                If Index_ <> -1 Then
                    PickUpTimer(Index_).Stop()

                    If ItemList.ContainsKey(PlayerData(Index_).PickUpId) Then
                        OnPickUp(PlayerData(Index_).PickUpId, Index_)
                    End If

                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index_)
                '
            End Try
        End Sub

        Private Sub WorldCheckElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            WorldCheck.Stop()

            Try
                Dim stopwatch As New Stopwatch
                stopwatch.Start()

                Dim tmplist = MobList.Keys.ToArray
                Dim monsterRemoveList As New List(Of UInt32)

                For Each key In tmplist
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)

                        If Mob_.Death = True Or Mob_.HPCur <= 0 Then
                            If Date.Compare(Date.Now, Mob_.DeathRemoveTime) = 1 Then
                                'Abgelaufen
                                monsterRemoveList.Add(Mob_.UniqueID)
                            End If
                        ElseIf IsInSaveZone(Mob_.Position) Then
                            monsterRemoveList.Add(Mob_.UniqueID)
                        ElseIf Mob_.GetsAttacked = True Then
                            'Attack back...
                            Mob_.AttackTimerStart(10)
                        End If
                    End If
                Next

                'Send Removelist
                tmplist = monsterRemoveList.ToArray
                RemoveMob(tmplist)

                tmplist = ItemList.Keys.ToArray
                For Each key In tmplist
                    If ItemList.ContainsKey(key) Then
                        If Date.Compare(Date.Now, ItemList(key).DespawnTime) = 1 Then
                            RemoveItem(key)
                        End If
                    End If
                Next


                stopwatch.Stop()
                Debug.Print("WC: " & stopwatch.ElapsedMilliseconds & "ms. Count:" & MobList.Count)
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: WC")
            End Try

            WorldCheck.Start()
            'restart Timer
        End Sub


        Private Sub MonsterRespawnElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            MonsterRespawn.Stop()
            Try
                Dim stopwatch As New Stopwatch
                stopwatch.Start()

                CheckForRespawns()

                stopwatch.Stop()
                Debug.Print("MR: " & stopwatch.ElapsedMilliseconds & "ms")
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MR")
                '
            End Try

            MonsterRespawn.Start()
            'restart Timer
        End Sub

        Private Sub MonsterMovementElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            MonsterMovement.Stop()

            Try
                Dim stopwatch As New Stopwatch
                stopwatch.Start()

                Dim tmplist As Array = MobList.Keys.ToArray
                For Each key In tmplist
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)
                        Dim obj As SilkroadObject = GetObject(Mob_.Pk2ID)
                        If Rand.Next(0, 3) = 0 Then
                            If _
                                Mob_.Death = False And
                                Mob_.PosTracker.MoveState = cPositionTracker.enumMoveState.Standing And
                                obj.WalkSpeed > 0 And Mob_.GetsAttacked = False And Mob_.IsAttacking = False Then

                                Dim distFromSpawn As Single = CalculateDistance(Mob_.Position, Mob_.PositionSpawn)

                                If distFromSpawn < Settings.ServerRange / 1.25 Then
                                    Dim toX As Single = Mob_.Position.ToGameX + Rand.Next(-30, +30)
                                    Dim toY As Single = Mob_.Position.ToGameY + Rand.Next(-30, +30)
                                    Dim vaildCords As Boolean = False
                                    Dim validCordTrys As Integer = 0

                                    Do
                                        Dim tmpXsec As Int16 = GetXSecFromGameX(toX)
                                        Dim tmpYsec As Int16 = GetYSecFromGameY(toY)
                                        If tmpXsec > 0 And tmpXsec < 255 And tmpYsec > 0 And tmpYsec < 255 Then
                                            vaildCords = True
                                        ElseIf validCordTrys > 5 Then
                                            Continue For
                                        Else
                                            toX = Mob_.Position.ToGameX + Rand.Next(-15, +15)
                                            toY = Mob_.Position.ToGameY + Rand.Next(-15, +15)
                                            validCordTrys += 1
                                        End If
                                    Loop While vaildCords = False
                                    
                                    Dim toPos As New Position
                                    toPos.XSector = GetXSecFromGameX(toX)
                                    toPos.YSector = GetYSecFromGameY(toY)
                                    toPos.X = GetXOffset(toX)
                                    toPos.Z = Mob_.Position.Z
                                    toPos.Y = GetYOffset(toY)

                                    If Mob_.PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Walking And Rand.Next(0, 2) = 2 Then
                                        'Chance to get into run mode 1/3
                                        SetMobRunning(Mob_.UniqueID)

                                    ElseIf Mob_.PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Running And Rand.Next(0, 1) = 1 Then
                                        'Chanche to get out 1/2
                                        SetMobWalking(Mob_.UniqueID)
                                    End If

                                    MoveMob(Mob_.UniqueID, toPos)
                                Else
                                    SetMobRunning(Mob_.UniqueID)

                                    MoveMob(Mob_.UniqueID, Mob_.PositionSpawn)
                                End If
                            End If
                        End If
                    End If
                Next

                stopwatch.Stop()
                Debug.Print("MM: " & stopwatch.ElapsedMilliseconds & "ms")

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MM")
                '
            End Try

            MonsterMovement.Start()
        End Sub



        Private Sub PlayerMoveTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim Index_ As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = LBound(PlayerMoveTimer, 1) To UBound(PickUpTimer, 1)
                    If ReferenceEquals(PlayerMoveTimer(i), objB) Then
                        Index_ = i
                        Exit For
                    End If
                Next

                If Index_ <> -1 Then
                    PlayerMoveTimer(Index_).Stop()

                    If PlayerData(Index_) IsNot Nothing Then
                        If PlayerData(Index_).PosTracker.MoveState = cPositionTracker.enumMoveState.Walking Then
                            Dim newPos As Position = PlayerData(Index_).PosTracker.GetCurPos()
                            ObjectSpawnCheck(Index_)
                            CheckForCaveTeleporter(Index_)

                            'SendPm(Index_, "secx" & newPos.XSector & "secy" & newPos.YSector & "X: " & newPos.X & "Y: " & newPos.Y & " X:" & newPos.ToGameX & " Y: " & newPos.ToGameY & " Z: " & newPos.Z, "hh")
                            PlayerMoveTimer(Index_).Start()

                        ElseIf PlayerData(Index_).PosTracker.MoveState = cPositionTracker.enumMoveState.Standing Then
                            ObjectSpawnCheck(Index_)
                            SendPm(Index_, "Walk End", "hh")

                            GameDB.SavePosition(Index_)
                            CheckForCaveTeleporter(Index_)
                        End If
                    End If

                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index_)
                '
            End Try
        End Sub

        Private Sub PlayerAutoHealElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            PlayerAutoHeal.Stop()

            Try
                For i = 0 To PlayerData.Length - 1
                    If PlayerData(i) IsNot Nothing Then
                        If PlayerData(i).Ingame And PlayerData(i).Alive Then
                            Dim changedHPMP As Boolean = False
                            Select Case PlayerData(i).ActionFlag
                                Case 0
                                    'Nomral
                                    If PlayerData(i).CHP < PlayerData(i).HP Then
                                        PlayerData(i).CHP += Math.Round(PlayerData(i).HP * 0.01, 0,
                                                                        MidpointRounding.AwayFromZero)
                                        changedHPMP = True
                                    End If
                                    If PlayerData(i).CMP < PlayerData(i).MP Then
                                        PlayerData(i).CMP += Math.Round(PlayerData(i).MP * 0.01, 0,
                                                                        MidpointRounding.AwayFromZero)
                                        changedHPMP = True
                                    End If

                                Case 4
                                    'Sitting
                                    If PlayerData(i).CHP < PlayerData(i).HP Then
                                        PlayerData(i).CHP += Math.Round(PlayerData(i).HP * 0.05, 0,
                                                                        MidpointRounding.AwayFromZero)
                                        changedHPMP = True
                                    End If
                                    If PlayerData(i).CMP < PlayerData(i).MP Then
                                        PlayerData(i).CMP += Math.Round(PlayerData(i).MP * 0.05, 0,
                                                                        MidpointRounding.AwayFromZero)
                                        changedHPMP = True
                                    End If
                            End Select

                            'Check and Correct Errors...
                            If PlayerData(i).CHP > PlayerData(i).HP Then
                                PlayerData(i).CHP = PlayerData(i).HP
                            End If
                            If PlayerData(i).CMP > PlayerData(i).MP Then
                                PlayerData(i).CMP = PlayerData(i).MP
                            End If


                            If changedHPMP = True Then
                                UpdateHPMP(i)
                            End If
                        End If
                    End If
                Next

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: PAH")
                '
            End Try

            PlayerAutoHeal.Start()
        End Sub


        Private Sub PlayerBerserkTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)

            Dim Index_ As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = LBound(PlayerBerserkTimer, 1) To UBound(PlayerBerserkTimer, 1)
                    If ReferenceEquals(PlayerBerserkTimer(i), objB) Then
                        Index_ = i
                        Exit For
                    End If
                Next

                PlayerBerserkTimer(Index_).Stop()

                If Index_ <> -1 And PlayerData(Index_) IsNot Nothing Then
                    PlayerData(Index_).Berserk = False
                    PlayerData(Index_).BerserkBar = 0
                    PlayerData(Index_).PosTracker.SpeedMode = cPositionTracker.enumSpeedMode.Running
                    UpdateSpeeds(Index_)
                    UpdateState(4, 0, Index_)
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index_)
                '
            End Try
        End Sub

        Private Sub GeneralTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            GeneralTimer.Stop()

            'For Database Execute Querys, GlobalManager Ping, GlobalManager Update, check other Timers for a Crash

            Try
                Database.ExecuteQuerys()
                Log.WriteSystemFileQuerys()

                'GlobalManager..
                If DateDiff(DateInterval.Second, GlobalManagerCon.LastPingTime, Date.Now) > 5 Then
                    If GlobalManagerCon.ManagerSocket IsNot Nothing AndAlso GlobalManagerCon.ManagerSocket.Connected Then
                        GlobalManagerCon.SendPing()
                    End If
                End If

                If DateDiff(DateInterval.Second, GlobalManagerCon.LastInfoTime, Date.Now) > 10 And GlobalManagerCon.UpdateInfoAllowed Then
                    If GlobalManagerCon.ManagerSocket IsNot Nothing AndAlso GlobalManagerCon.ManagerSocket.Connected Then
                        GlobalManager.OnSendMyInfo()
                    End If
                End If

                'Check other Timers for Crashed
                If MonsterRespawn.Enabled = False Then
                    MonsterRespawn.Start()
                End If
                If WorldCheck.Enabled = False Then
                    WorldCheck.Start()
                End If
                If MonsterMovement.Enabled = False Then
                    MonsterMovement.Start()
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: DB/GENRAL")
            End Try

            GeneralTimer.Start()
        End Sub

        Private Sub PingTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            PingTimer.Stop()

            'Excluded from ClientList 

            Try
                For i = 0 To Server.MaxClients - 1
                    Dim socket As Net.Sockets.Socket = Server.ClientList.GetSocket(i)
                    If socket IsNot Nothing AndAlso socket.Connected AndAlso SessionInfo(i) IsNot Nothing Then
                        If Settings.ServerDebugMode = False AndAlso DateDiff(DateInterval.Second, Server.ClientList.LastPingTime(i), DateTime.Now) > 30 Then
                            Server.Disconnect(i)
                        ElseIf SessionInfo(i).LoginAuthRequired And DateDiff(DateInterval.Second, SessionInfo(i).LoginAuthTimeout, DateTime.Now) > 0 Then
                            'LoginAuth is missing..
                            Server.Disconnect(i)
                        End If
                    End If
                Next

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: PING")
            End Try

            PingTimer.Start()
        End Sub

        Private Sub PlayerExitTimerElapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)

            Dim Index_ As Integer = -1
            Try
                Dim objB As Timer = DirectCast(sender, Timer)
                For i As Integer = LBound(PlayerExitTimer, 1) To UBound(PlayerExitTimer, 1)
                    If ReferenceEquals(PlayerExitTimer(i), objB) Then
                        Index_ = i
                        Exit For
                    End If
                Next

                PlayerExitTimer(Index_).Stop()

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_EXIT_FINAL)
                Server.Send(writer.GetBytes, Index_)
                GameMod.Damage.OnPlayerLogoff(Index_)
                Server.Disconnect(Index_)
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index_)
                '
            End Try
        End Sub
    End Module
End Namespace