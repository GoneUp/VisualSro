Namespace GameServer.Functions
    Module MonsterAction
        Public Sub KillMob(ByVal MobIndex As Integer)
            MobList(MobIndex).HP_Cur = 0
            MobList(MobIndex).Death = True

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

    End Module
End Namespace
