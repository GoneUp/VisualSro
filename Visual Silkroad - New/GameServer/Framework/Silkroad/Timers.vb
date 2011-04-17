Imports System.Timers, GameServer.GameServer.Functions
Namespace GameServer
    Module Timers
        Public PlayerAttackTimer As Timer() = New Timer(14999) {}
        Public PlayerMoveTimer As Timer() = New Timer(14999) {}
        Public MonsterMovement As New Timer
        Public MonsterCheck As New Timer
        Public MonsterRespawn As New Timer
        Public MonsterAttack As Timer() = New Timer(14999) {}
        Public PickUpTimer As Timer() = New Timer(14999) {}
        Public CastAttackTimer As Timer() = New Timer(14999) {}
        Public CastBuffTimer As Timer() = New Timer(14999) {}
        Public UsingItemTimer As Timer() = New Timer(14999) {}
        Public SitUpTimer As Timer() = New Timer(14999) {}
        Public DatabaseTimer As New Timer

        Public Sub LoadTimers(ByVal TimerCount As Integer)
            Log.WriteSystemLog("Loading Timers...")

            Try
                ReDim PlayerAttackTimer(TimerCount), PlayerMoveTimer(TimerCount), PickUpTimer(TimerCount), MonsterAttack(TimerCount), CastAttackTimer(TimerCount), CastBuffTimer(TimerCount), UsingItemTimer(TimerCount), SitUpTimer(TimerCount)

                For i As Integer = 0 To TimerCount - 1
                    PlayerAttackTimer(i) = New Timer()
                    AddHandler PlayerAttackTimer(i).Elapsed, AddressOf AttackTimer_Elapsed
                    PlayerMoveTimer(i) = New Timer()
                    AddHandler PlayerMoveTimer(i).Elapsed, AddressOf PlayerMoveTimer_Elapsed
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
                AddHandler DatabaseTimer.Elapsed, AddressOf DatabaseTimer_Elapsed

                'Start Timers
                MonsterCheck.Interval = 5000
                MonsterCheck.Start()

                MonsterMovement.Interval = 5000
                MonsterMovement.Start()

                MonsterRespawn.Interval = 7500
                MonsterRespawn.Start()

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
                            PlayerBuff_Info(Index, 0)
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
                            PlayerData(Index).Position_Recall = Functions.PlayerData(Index).Position 'Save Pos
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

                    For i = 0 To ItemList.Count - 1
                        If ItemList(i).UniqueID = PlayerData(Index).PickUpId Then
                            PickUp(i, Index)
                        End If
                    Next
                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
            End Try
        End Sub

        Public Sub MonsterCheck_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            MonsterCheck.Stop()

            Try

                For Each key In MobList.Keys.ToList
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)

                        If Mob_.Death = True Then
                            Dim wert As Integer = Date.Compare(Mob_.DeathRemoveTime, Date.Now)
                            If wert = -1 Then
                                'Abgelaufen
                                RemoveMob(Mob_.UniqueID)
                            End If
                        ElseIf IsInSaveZone(Mob_.Position) Then
                            RemoveMob(Mob_.UniqueID)
                            'ElseIf Mob_.GetsAttacked = True Then
                            '    'Attack back...
                            '    If Mob_.IsAttacking = False Then
                            '        MonsterAttackPlayer(Mob_.UniqueID, MobGetPlayerWithMostDamage(Mob_.UniqueID, True))
                            '    End If
                        End If
                    End If
                Next


                Debug.Print("MC: " & DateDiff(DateInterval.Second, e.SignalTime, Date.Now))
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MC") '
            End Try

            MonsterCheck.Start() 'restart Timer
        End Sub


        Public Sub MonsterRespawn_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            MonsterRespawn.Stop()

            Try

                CheckForRespawns()
                Debug.Print("MR: " & DateDiff(DateInterval.Second, e.SignalTime, Date.Now))

            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MR") '
            End Try

            MonsterRespawn.Start() 'restart Timer
        End Sub

        Public Sub MonsterMovement_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            MonsterMovement.Stop()

            Try

                For Each key In MobList.Keys.ToList
                    If MobList.ContainsKey(key) Then
                        Dim Mob_ As cMonster = MobList.Item(key)
                        Dim obj As Object_ = GetObjectById(Mob_.Pk2ID)

                        If Mob_.Death = False And Mob_.Walking = False And obj.WalkSpeed > 0 And Mob_.GetsAttacked = False Then
                            Dim dist As Single = CalculateDistance(Mob_.Position, Mob_.Position_Spawn)

                            If dist < Settings.Server_Range Then
                                Dim ToX As Single = GetRealX(Mob_.Position.XSector, Mob_.Position.X) + Rand.Next(-15, +10)
                                Dim ToY As Single = GetRealY(Mob_.Position.YSector, Mob_.Position.Y) + Rand.Next(-10, +15)

                                Dim ToPos As New Position
                                ToPos.XSector = GetXSec(ToX)
                                ToPos.YSector = GetYSec(ToY)
                                ToPos.X = GetXOffset(ToX)
                                ToPos.Z = Mob_.Position.Z
                                ToPos.Y = GetYOffset(ToY)

                                If ToPos.X = Mob_.Position.X And ToPos.Y = Mob_.Position.Y Then
                                    ToPos.X += 1
                                End If

                                MoveMob(Mob_.UniqueID, ToPos)
                            Else
                                MoveMob(Mob_.UniqueID, Mob_.Position_Spawn)
                            End If


                        ElseIf Mob_.Walking = True And obj.WalkSpeed > 0 And Mob_.GetsAttacked = False Then
                            Dim wert As Integer = Date.Compare(Mob_.WalkEnd, Date.Now)
                            If wert = -1 Then
                                'Abgelaufen
                                Mob_.Walking = False
                                Mob_.Position = Mob_.Position_ToPos
                            Else
                                Dim Past As Single = DateDiff(DateInterval.Second, Date.Now, Mob_.WalkEnd)
                                Dim FullTime As Single = DateDiff(DateInterval.Second, Mob_.WalkStart, Mob_.WalkEnd)
                                Dim Verhältnis As Single = (Past / FullTime)

                                Dim Walked As Single = (CalculateDistance(Mob_.Position_FromPos, Mob_.Position_ToPos) * Verhältnis)

                                If Walked > 0 Then
                                    Dim OldX As Single = Mob_.Position_FromPos.X
                                    Dim OldY As Single = Mob_.Position_FromPos.Y
                                    Dim Full_X As Single = Mob_.Position_FromPos.X - Mob_.Position_ToPos.X
                                    Dim Full_Y As Single = Mob_.Position_FromPos.Y - Mob_.Position_ToPos.Y

                                    Dim Cur_X As Single = Full_X * Verhältnis
                                    Dim Cur_Y As Single = Full_Y * Verhältnis

                                    Mob_.Position.X = GetXOffset(OldX + Cur_X)
                                    Mob_.Position.Y = GetYOffset(OldY + Cur_Y)

                                    Mob_.Position.XSector = GetXSec(Mob_.Position.X)
                                    Mob_.Position.YSector = GetYSec(Mob_.Position.Y)
                                End If
                            End If
                        End If
                    End If
                Next

                Debug.Print("MM: " & DateDiff(DateInterval.Second, e.SignalTime, Date.Now))

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

                    If PlayerData(Index).Walking = True Then
                        Dim wert As Integer = Date.Compare(PlayerData(Index).WalkEnd, Date.Now)
                        If wert = -1 Then
                            'Abgelaufen
                            PlayerData(Index).Walking = False
                            PlayerData(Index).Position = PlayerData(Index).Position_ToPos
                            ObjectSpawnCheck(Index)
                        Else
                            Dim Past As Single = DateDiff(DateInterval.Second, PlayerData(Index).WalkStart, Date.Now)
                            Dim FullTime As Single = DateDiff(DateInterval.Second, PlayerData(Index).WalkStart, PlayerData(Index).WalkEnd)
                            Dim Verhältnis As Single = (Past / FullTime)

                            Dim Walked As Single = (CalculateDistance2(PlayerData(Index).Position_FromPos, PlayerData(Index).Position_ToPos) * Verhältnis)

                            If Walked > 0 Then
                                Dim OldX As Single = PlayerData(Index).Position_FromPos.X
                                Dim OldY As Single = PlayerData(Index).Position_FromPos.Y
                                Dim Full_X As Single = PlayerData(Index).Position_FromPos.X - PlayerData(Index).Position_ToPos.X
                                Dim Full_Y As Single = PlayerData(Index).Position_FromPos.Y - PlayerData(Index).Position_ToPos.Y

                                Dim Cur_X As Single = Full_X * Verhältnis
                                Dim Cur_Y As Single = Full_Y * Verhältnis

                                Dim New_X As Single = OldX + Cur_X
                                Dim New_Y As Single = OldY + Cur_Y

                                'PlayerData(Index).Position.XSector = GetXSec(PlayerData(Index).Position.X)
                                'PlayerData(Index).Position.YSector = GetYSec(PlayerData(Index).Position.Y)

                                'SendPm(Index, "X: " & CStr(GetRealX(PlayerData(Index).Position.XSector, PlayerData(Index).Position.X)) & " Y:" & GetRealY(PlayerData(Index).Position.YSector, PlayerData(Index).Position.Y), "[TMP]")
                                ObjectSpawnCheck(Index)
                            End If

                            PlayerMoveTimer(Index).Start()
                        End If
                    End If




                End If
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
            End Try
        End Sub


        Public Sub DatabaseTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            DatabaseTimer.Stop()
            DataBase.ExecuteQuerys()
            DatabaseTimer.Start()
        End Sub
    End Module
End Namespace