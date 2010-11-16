Imports System.Timers, GameServer.GameServer.Functions
Namespace GameServer
    Module Timers
        Public PlayerAttack As Timer() = New Timer(14999) {}
        Public MonsterMovement As Timer() = New Timer(14999) {}
        Public MonsterDeath As Timer() = New Timer(14999) {}
        Public MonsterAttack As Timer() = New Timer(14999) {}
        Public CastAttackTimer As Timer() = New Timer(14999) {}
        Public CastBuffTimer As Timer() = New Timer(14999) {}
        Public UsingItemTimer As Timer() = New Timer(14999) {}
        Public SitUpTimer As Timer() = New Timer(14999) {}

        Public Sub LoadTimers(ByVal TimerCount As Integer)
            Log.WriteSystemLog("Loading Timers...")

            Try
                ReDim PlayerAttack(TimerCount), MonsterMovement(TimerCount), MonsterDeath(TimerCount), MonsterAttack(TimerCount), CastAttackTimer(TimerCount), CastBuffTimer(TimerCount), UsingItemTimer(TimerCount), SitUpTimer(TimerCount)

                For i As Integer = 0 To TimerCount - 1
                    PlayerAttack(i) = New Timer()
                    AddHandler PlayerAttack(i).Elapsed, AddressOf AttackTimer_Elapsed
                    UsingItemTimer(i) = New Timer()
                    AddHandler UsingItemTimer(i).Elapsed, AddressOf UseItemTimer_Elapsed
                    SitUpTimer(i) = New Timer()
                    AddHandler SitUpTimer(i).Elapsed, AddressOf SitUpTimer_Elapsed
                    MonsterDeath(i) = New Timer()
                    AddHandler MonsterDeath(i).Elapsed, AddressOf MonsterDeath_Elapsed
                Next

            Catch ex As Exception

            End Try
            Log.WriteSystemLog("Timers loaded!")
        End Sub

        Public Sub AttackTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim objB As Timer = DirectCast(sender, Timer)
            Dim Index As Integer = -1
            For i As Integer = Information.LBound(PlayerAttack, 1) To Information.UBound(PlayerAttack, 1)
                If Object.ReferenceEquals(PlayerAttack(i), objB) Then
                    Index = i
                    Exit For
                End If
            Next

        End Sub

        Public Sub UseItemTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)

            Dim objB As Timer = DirectCast(sender, Timer)
            Dim Index_ As Integer = -1
            For i As Integer = Information.LBound(PlayerAttack, 1) To Information.UBound(PlayerAttack, 1)
                If Object.ReferenceEquals(UsingItemTimer(i), objB) Then
                    Index_ = i
                    Exit For
                End If
            Next

            UsingItemTimer(Index_).Stop()
            If Index_ <> -1 Then
                Select Case Functions.PlayerData(Index_).UsedItem
                    Case UseItemTypes.Return_Scroll
                        PlayerData(Index_).Position_Recall = Functions.PlayerData(Index_).Position 'Save Pos
                        PlayerData(Index_).Position = Functions.PlayerData(Index_).Position_Return 'Set new Pos
                        'Save to DB
                        DataBase.SaveQuery(String.Format("UPDATE positions SET recall_xsect='{0}', recall_ysect='{1}', recall_xpos='{2}', recall_zpos='{3}', recall_ypos='{4}' where OwnerCharID='{5}'", PlayerData(Index_).Position_Recall.XSector, PlayerData(Index_).Position_Recall.YSector, Math.Round(PlayerData(Index_).Position_Recall.X), Math.Round(PlayerData(Index_).Position_Recall.Z), Math.Round(PlayerData(Index_).Position_Recall.Y), PlayerData(Index_).UniqueId))
                        DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).UniqueId))

                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                        PlayerData(Index_).Busy = False
                        PlayerData(Index_).UsedItem = UseItemTypes.None


                    Case UseItemTypes.Reverse_Scroll_Recall
                        PlayerData(Index_).Position = PlayerData(Index_).Position_Recall
                        DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).UniqueId))

                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                        PlayerData(Index_).Busy = False
                        PlayerData(Index_).UsedItem = UseItemTypes.None

                    Case UseItemTypes.Reverse_Scroll_Dead
                        PlayerData(Index_).Position = PlayerData(Index_).Position_Dead
                        DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).UniqueId))

                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                        PlayerData(Index_).Busy = False
                        PlayerData(Index_).UsedItem = UseItemTypes.None

                    Case UseItemTypes.Reverse_Scroll_Point
                        Dim point As ReversePoint_ = GetReversePointByID(PlayerData(Index_).UsedItemParameter)
                        PlayerData(Index_).Position = point.Position
                        DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).UniqueId))

                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                        PlayerData(Index_).Busy = False
                        PlayerData(Index_).UsedItem = UseItemTypes.None
                        PlayerData(Index_).UsedItemParameter = 0

                End Select
            End If
        End Sub

        Public Sub SitUpTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim objB As Timer = DirectCast(sender, Timer)
            Dim Index As Integer = -1
            For i As Integer = Information.LBound(PlayerAttack, 1) To Information.UBound(PlayerAttack, 1)
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


        End Sub

        Public Sub MonsterDeath_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            Dim objB As Timer = DirectCast(sender, Timer)
            Dim Index As Integer = -1
            For i As Integer = Information.LBound(PlayerAttack, 1) To Information.UBound(PlayerAttack, 1)
                If Object.ReferenceEquals(SitUpTimer(i), objB) Then
                    Index = i
                    Exit For
                End If
            Next

            If Index <> -1 Then
                MonsterDeath(Index).Stop()

                For i = 0 To MobList.Count - 1
                    If MobList(i).UniqueID = PlayerData(Index).AttackedDeathMonsterID Then
                        RemoveMob(i)
                    End If
                Next
                PlayerData(Index).AttackedDeathMonsterID = 0
            End If
        End Sub

    End Module
End Namespace