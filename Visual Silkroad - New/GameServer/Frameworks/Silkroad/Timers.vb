Imports System.Timers, GameServer.GameServer.Functions
Namespace GameServer
    Module Timers
        Public PlayerAttackTimer As Timer() = New Timer(14999) {}
        Public MonsterMovement As Timer() = New Timer(14999) {}
        Public MonsterDeath As New Timer
        Public MonsterAttack As Timer() = New Timer(14999) {}
        Public PickUpTimer As Timer() = New Timer(14999) {}
        Public CastAttackTimer As Timer() = New Timer(14999) {}
        Public CastBuffTimer As Timer() = New Timer(14999) {}
        Public UsingItemTimer As Timer() = New Timer(14999) {}
        Public SitUpTimer As Timer() = New Timer(14999) {}

        Public Sub LoadTimers(ByVal TimerCount As Integer)
            Log.WriteSystemLog("Loading Timers...")

            Try
                ReDim PlayerAttackTimer(TimerCount), PickUpTimer(TimerCount), MonsterMovement(TimerCount), MonsterAttack(TimerCount), CastAttackTimer(TimerCount), CastBuffTimer(TimerCount), UsingItemTimer(TimerCount), SitUpTimer(TimerCount)

                For i As Integer = 0 To TimerCount - 1
                    PlayerAttackTimer(i) = New Timer()
                    AddHandler PlayerAttackTimer(i).Elapsed, AddressOf AttackTimer_Elapsed
                    UsingItemTimer(i) = New Timer()
                    AddHandler UsingItemTimer(i).Elapsed, AddressOf UseItemTimer_Elapsed
                    SitUpTimer(i) = New Timer()
                    AddHandler SitUpTimer(i).Elapsed, AddressOf SitUpTimer_Elapsed
                    PickUpTimer(i) = New Timer()
                    AddHandler PickUpTimer(i).Elapsed, AddressOf SitUpTimer_Elapsed

                    AddHandler MonsterDeath.Elapsed, AddressOf MonsterDeath_Elapsed
                Next

                'Start Timers
                MonsterDeath.Interval = 4000
                MonsterDeath.Start()



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

                If PlayerData(Index).AttackedMonsterID <> 0 Then
                    If GetMobIndex(PlayerData(Index).AttackedMonsterID) <> -1 Then
                        Dim mob_ As cMonster = MobList(GetMobIndex(PlayerData(Index).AttackedMonsterID))
                        If mob_.Death = False Then
                            If PlayerData(Index).AttackType = AttackType_.Normal Then
                                PlayerAttackNormal(Index, mob_.UniqueID)
                            End If
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

        Public Sub MonsterDeath_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Try
                MonsterDeath.Stop()
                For i = 0 To MobList.Count - 1
                    If MobList(i).Death = True Then
                        RemoveMob(i)
                    End If
                Next
                MonsterDeath.Start()
            Catch ex As Exception
                Log.WriteSystemLog("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: MD") '
            End Try

        End Sub

    End Module
End Namespace