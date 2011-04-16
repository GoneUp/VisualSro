Namespace GameServer.Functions
    Module MonsterAction
        Public Sub KillMob(ByVal UniqueID As Integer)
            MobList(UniqueID).HP_Cur = 0
            MobList(UniqueID).Death = True
            MobList(UniqueID).DeathRemoveTime = Date.Now.AddSeconds(5)
            UpdateState(0, 2, 0, UniqueID)

            Dim tmp_ As Integer = MobGetPlayerWithMostDamage(UniqueID)
            If tmp_ >= 0 Then
                If MobList(UniqueID).Mob_Type = 3 Then
                    SendUniqueKill(MobList(UniqueID).Pk2ID, PlayerData(tmp_).CharacterName)
                End If

                If Settings.ModGeneral And Settings.ModDamage Then
                    [Mod].Damage.SendDamageInfo(UniqueID)
                End If
            End If
        End Sub

        Public Function MobGetPlayerWithMostDamage(ByVal UniqueID As Integer)
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


        Public Function MobGetPlayerWithMostDamage(ByVal UniqueID As Integer, ByVal Attacking As Boolean)
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


        Public Sub MoveMob(ByVal UniqueID As Integer, ByVal ToPos As Position)
            Dim Obj As Object_ = GetObjectById(MobList(UniqueID).Pk2ID)
            Dim Distance As Single = CalculateDistance(MobList(UniqueID).Position, ToPos)
            Dim Time As Single = Distance / Obj.WalkSpeed

            If Time > 0 Then
                MobList(UniqueID).Walking = True
                MobList(UniqueID).Position_FromPos = MobList(UniqueID).Position
                MobList(UniqueID).Position_ToPos = ToPos
                MobList(UniqueID).WalkStart = Date.Now
                MobList(UniqueID).WalkEnd = Date.Now.AddSeconds(Time)
            Else
                Debug.Print(1)
            End If



            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Movement)
            writer.DWord(UniqueID)
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

            Server.SendIfMobIsSpawned(writer.GetBytes, MobList(UniqueID).UniqueID)

            If MobList(UniqueID).IsAttacking = True Then
                If Time > 0 Then
                    MobList(UniqueID).AttackTimer_Start(Time * 1000)
                End If
            End If
        End Sub


        Public Sub GetEXPFromMob(ByVal mob_ As cMonster)
            Dim ref_ As Object_ = GetObjectById(mob_.Pk2ID)
            For i = 0 To mob_.DamageFromPlayer.Count - 1
                Dim Index_ As Integer = mob_.DamageFromPlayer(i).PlayerIndex
                Dim Percent As Single = mob_.DamageFromPlayer(i).Damage / mob_.HP_Max

                Dim Balance As Double 'The Level factor...
                If CSng(ref_.Level) - PlayerData(Index_).Level > 0 Then
                    'Mob is higher then you
                    Balance = (1 + ((CSng(ref_.Level) - CSng(PlayerData(Index_).Level)) / 10))
                Else
                    'Mob is lower then you
                    Balance = (1 + ((CSng(ref_.Level) - CSng(PlayerData(Index_).Level)) / 100))
                End If

                Dim EXP As Long = ((ref_.Exp * GetExpMultiplier(mob_.Mob_Type)) * Settings.Server_XPRate * Percent) * Balance
                Dim SP As Long = (ref_.Exp * Settings.Server_SPRate * Percent)

                GetXP(EXP, SP, Index_, mob_.UniqueID)
            Next
        End Sub

        Private Function GetExpMultiplier(ByVal Type As Byte) As Byte
            Select Case Type
                Case 0
                    Return MobMultiplierExp.Normal
                Case 1
                    Return MobMultiplierExp.Champion
                Case 3
                    Return MobMultiplierExp.Unique
                Case 4
                    Return MobMultiplierExp.Giant
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
