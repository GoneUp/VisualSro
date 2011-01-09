Imports System.Timers, GameServer.GameServer.Functions
Namespace GameServer
    Module Timers
        Public PlayerAttackTimer As Timer() = New Timer(14999) {}
        Public PlayerMoveTimer As Timer() = New Timer(14999) {}
        Public MonsterMovement As New Timer
        Public MonsterCheck As New Timer
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
                AddHandler MonsterMovement.Elapsed, AddressOf MonsterMovement_Elapsed
                AddHandler DatabaseTimer.Elapsed, AddressOf DatabaseTimer_Elapsed

                'Start Timers
                MonsterCheck.Interval = 5000
                MonsterCheck.Start()

                MonsterMovement.Interval = 5000
                MonsterMovement.Start()

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
                PlayerData(Index).Busy = False
                PlayerData(Index).Attacking = False

                If PlayerData(Index).AttackedId <> 0 Then
                    Dim MobListIndex As Integer
                    For i = 0 To MobList.Count - 1
                        If MobList(i).UniqueID = PlayerData(Index).AttackedId Then
                            MobListIndex = i
                        End If
                    Next
                    Dim mob_ As cMonster = MobList(MobListIndex)
                    If mob_.Death = False Then
                        If PlayerData(Index).AttackType = AttackType_.Normal Then
                            PlayerAttackNormal(Index, MobListIndex)
                        ElseIf PlayerData(Index).AttackType = AttackType_.Skill Then
                            PlayerAttackEndSkill(Index)
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
            Try
                MonsterCheck.Stop()

                For i = 0 To MobList.Count - 1
                    If i < MobList.Count Then
                        Dim mob_ = MobList(i)
                        If MobList(i).Death = True Then
                            Dim wert As Integer = Date.Compare(MobList(i).DeathRemoveTime, Date.Now)
                            If wert = -1 Then
                                'Abgelaufen
                                RemoveMob(i)
                            End If
                        ElseIf IsInSaveZone(MobList(i).Position) Then
                            RemoveMob(i)
                        End If
                    End If
                Next

                CheckForRespawns()

                MonsterCheck.Start()
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MD") '
            End Try
        End Sub


        Public Sub MonsterMovement_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim random As New Random

            Try
                MonsterMovement.Stop()

                Dim time As Date = Date.Now
                For i = 0 To MobList.Count - 1
                    If i < MobList.Count Then
                        Dim obj As Object_ = GetObjectById(MobList(i).Pk2ID)
                        Dim mob = MobList(i)


                        If MobList(i).Death = False And MobList(i).Walking = False And obj.WalkSpeed > 0 Then
                            Dim dist As Single = CalculateDistance(MobList(i).Position, MobList(i).Position_Spawn)
                            Dim mob_ = MobList(i)

                            If dist < Settings.ServerRange Then
                                Dim ToX As Single = GetRealX(MobList(i).Position.XSector, MobList(i).Position.X) + random.Next(-15, +10)
                                Dim ToY As Single = GetRealY(MobList(i).Position.YSector, MobList(i).Position.Y) + random.Next(-10, +15)

                                Dim ToPos As New Position
                                ToPos.XSector = GetXSec(ToX)
                                ToPos.YSector = GetYSec(ToY)
                                ToPos.X = GetXOffset(ToX)
                                ToPos.Z = MobList(i).Position.Z
                                ToPos.Y = GetYOffset(ToY)

                                If ToPos.X = mob.Position.X And ToPos.Y = mob.Position.Y Then
                                    ToPos.X += 1
                                End If


                                MoveMob(i, ToPos)
                            Else
                                MoveMob(i, MobList(i).Position_Spawn)
                            End If


                        ElseIf MobList(i).Walking = True And obj.WalkSpeed > 0 Then
                            Dim wert As Integer = Date.Compare(MobList(i).WalkEnd, Date.Now)
                            If wert = -1 Then
                                'Abgelaufen
                                MobList(i).Walking = False
                                MobList(i).Position = MobList(i).Position_ToPos
                            Else
                                Dim Past As Single = DateDiff(DateInterval.Second, Date.Now, MobList(i).WalkEnd)
                                Dim FullTime As Single = DateDiff(DateInterval.Second, MobList(i).WalkStart, MobList(i).WalkEnd)
                                Dim Verhältnis As Single = (Past / FullTime)

                                Dim Walked As Single = (CalculateDistance(MobList(i).Position_FromPos, MobList(i).Position_ToPos) * Verhältnis)

                                If Walked > 0 Then
                                    Dim OldX As Single = GetRealX(MobList(i).Position_FromPos.XSector, MobList(i).Position_FromPos.X)
                                    Dim OldY As Single = GetRealY(MobList(i).Position_FromPos.YSector, MobList(i).Position_FromPos.Y)
                                    Dim Full_X As Single = GetRealX(MobList(i).Position_FromPos.XSector, MobList(i).Position_FromPos.X) - GetRealX(MobList(i).Position_ToPos.XSector, MobList(i).Position_ToPos.X)
                                    Dim Full_Y As Single = GetRealY(MobList(i).Position_FromPos.YSector, MobList(i).Position_FromPos.Y) - GetRealY(MobList(i).Position_ToPos.YSector, MobList(i).Position_ToPos.Y)

                                    Dim Cur_X As Single = Full_X * Verhältnis
                                    Dim Cur_Y As Single = Full_Y * Verhältnis

                                    MobList(i).Position.X = GetXOffset(OldX + Cur_X)
                                    MobList(i).Position.Y = GetYOffset(OldY + Cur_Y)

                                    MobList(i).Position.XSector = GetXSec(MobList(i).Position.X)
                                    MobList(i).Position.YSector = GetYSec(MobList(i).Position.Y)
                                End If
                            End If
                        End If
                    End If
                Next

                Debug.Print("MM: " & DateDiff(DateInterval.Second, time, Date.Now))

                MonsterMovement.Start()
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MM") '
            End Try

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
                                Dim OldX As Single = GetRealX(PlayerData(Index).Position_FromPos.XSector, PlayerData(Index).Position_FromPos.X)
                                Dim OldY As Single = GetRealY(PlayerData(Index).Position_FromPos.YSector, PlayerData(Index).Position_FromPos.Y)
                                Dim Full_X As Single = GetRealX(PlayerData(Index).Position_FromPos.XSector, PlayerData(Index).Position_FromPos.X) - GetRealX(PlayerData(Index).Position_ToPos.XSector, PlayerData(Index).Position_ToPos.X)
                                Dim Full_Y As Single = GetRealY(PlayerData(Index).Position_FromPos.YSector, PlayerData(Index).Position_FromPos.Y) - GetRealY(PlayerData(Index).Position_ToPos.YSector, PlayerData(Index).Position_ToPos.Y)

                                Dim Cur_X As Single = Full_X * Verhältnis
                                Dim Cur_Y As Single = Full_Y * Verhältnis

                                Dim New_X As Single = OldX + Cur_X
                                Dim New_Y As Single = OldY + Cur_Y

                                'PlayerData(Index).Position.X = GetXOffset(New_X)
                                'PlayerData(Index).Position.Y = GetYOffset(New_Y)

                                Dim test = PlayerData(Index).Position_FromPos.X
                                Dim test2 = PlayerData(Index).Position_FromPos.Y
                                Dim test3 = GetXOffset(New_X)
                                Dim test4 = GetYOffset(New_Y)

                                'PlayerData(Index).Position.XSector = GetXSec(PlayerData(Index).Position.X)
                                'PlayerData(Index).Position.YSector = GetYSec(PlayerData(Index).Position.Y)

                                Dim r = GetRealX(PlayerData(Index).Position.XSector, PlayerData(Index).Position.X)
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