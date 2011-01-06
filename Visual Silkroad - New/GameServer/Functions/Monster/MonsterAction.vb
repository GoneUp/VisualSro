Namespace GameServer.Functions
    Module MonsterAction
        Public Sub KillMob(ByVal MobIndex As Integer)
            MobList(MobIndex).HP_Cur = 0
            MobList(MobIndex).Death = True
            MobList(MobIndex).DeathRemoveTime = Date.Now.AddSeconds(5)
            UpdateState(0, 2, 0, MobIndex)

            Dim tmp_ As Integer = GetPlayerWithMostDamage(MobIndex)
            If tmp_ >= 0 Then
                If MobList(MobIndex).Mob_Type = 3 Then
                    SendUniqueKill(MobList(MobIndex).Pk2ID, PlayerData(tmp_).CharacterName)
                End If

                If ModGeneral And ModDamage Then
                    [Mod].SendDamageInfo(MobIndex)
                End If
            End If
        End Sub

        Private Function GetPlayerWithMostDamage(ByVal MobIndex As Integer)
            Dim MostIndex As Integer = -1
            Dim MostDamage As UInteger
            For i = 0 To MobList(MobIndex).DamageFromPlayer.Count - 1
                If MobList(MobIndex).DamageFromPlayer(i).Damage > MostDamage Then
                    MostDamage = MobList(MobIndex).DamageFromPlayer(i).Damage
                    MostIndex = MobList(MobIndex).DamageFromPlayer(i).PlayerIndex
                End If
            Next
            Return MostIndex
        End Function

        Public Function GetMobIndex(ByVal UniqueID As UInt32) As Integer
            For i = 0 To MobList.Count - 1
                If MobList(i).UniqueID = UniqueID Then
                    Return i
                End If
            Next
            Return -1
        End Function


        Public Sub MoveMob(ByVal MobListIndex As Integer, ByVal ToPos As Position)
            Dim Obj As Object_ = GetObjectById(MobList(MobListIndex).Pk2ID)
            Dim Distance As Single = CalculateDistance(MobList(MobListIndex).Position, ToPos)
            Dim Time As Single = Distance / Obj.WalkSpeed

            If Time > 0 Then
                MobList(MobListIndex).Walking = True
                MobList(MobListIndex).Position_FromPos = MobList(MobListIndex).Position
                MobList(MobListIndex).Position_ToPos = ToPos
                MobList(MobListIndex).WalkStart = Date.Now
                MobList(MobListIndex).WalkEnd = Date.Now.AddSeconds(Time)
            Else
                Debug.Print(1)
            End If



            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Movement)
            writer.DWord(MobList(MobListIndex).UniqueID)
            writer.Byte(1) 'destination
            writer.Byte(ToPos.XSector)
            writer.Byte(ToPos.YSector)
            writer.Word(CUInt(ToPos.X))
            writer.Word(0)
            writer.Word(CUInt(ToPos.Y))
            writer.Byte(0) '1= source

            Server.SendIfMobIsSpawned(writer.GetBytes, MobList(MobListIndex).UniqueID)
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

                Dim EXP As Long = ((ref_.Exp * GetExpMultiplier(mob_.Mob_Type)) * ServerXPRate * Percent) * Balance
                Dim SP As Long = (ref_.Exp * ServerSPRate * Percent)

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
