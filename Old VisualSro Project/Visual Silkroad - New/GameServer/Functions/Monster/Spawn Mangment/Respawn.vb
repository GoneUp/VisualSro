Namespace Functions
    Module Respawn
        Dim Random As New Random

        Public Sub CheckForRespawns()
            Dim SpotIndex As Integer


            Try
                Dim Random As New Random

                For SpotIndex = 0 To RefRespawns.Count - 1
                    If RefRespawns(SpotIndex).SpawnCount < Settings.Server_SpawnRate Then
                        If Random.Next(0, 4) = 0 Then
                            ReSpawnMob(SpotIndex)
                        End If
                    End If
                Next


                For SpotIndex = 0 To RefRespawnsUnique.Count - 1
                    If IsUniqueSpawned(RefRespawnsUnique(SpotIndex).Pk2ID) = False Then
                        ReSpawnUnique(SpotIndex)
                    End If
                Next
            Catch ex As Exception

            End Try
        End Sub


        Public Sub ReSpawnMob(ByVal SpotIndex As Integer)
            Try
                Dim re = RefRespawns(SpotIndex)
                Dim obj_ As SilkroadObject = GetObject(RefRespawns(SpotIndex).Pk2ID)

                Select Case obj_.Type
                    Case SilkroadObject.Type_.Mob_Normal
                        SpawnMob(RefRespawns(SpotIndex).Pk2ID, GetRadomMobType, RefRespawns(SpotIndex).Position, 0,
                                 re.SpotID)
                    Case SilkroadObject.Type_.Mob_Cave
                        SpawnMob(RefRespawns(SpotIndex).Pk2ID, GetRadomMobType, RefRespawns(SpotIndex).Position, 0,
                                 re.SpotID)
                    Case SilkroadObject.Type_.Npc
                        SpawnNPC(RefRespawns(SpotIndex).Pk2ID, RefRespawns(SpotIndex).Position,
                                 RefRespawns(SpotIndex).Angle)
                End Select
            Catch ex As Exception

            End Try
        End Sub

        Public Sub ReSpawnUnique(ByVal UniqueListID As Integer)
            Dim tmp As ReSpawnUnique_ = RefRespawnsUnique(UniqueListID)
            Dim selector As Integer = Random.Next(0, tmp.Spots.Count - 1)

            SpawnMob(tmp.Pk2ID, 3, tmp.Spots(selector), 0, -2)
        End Sub


        Public Sub AddUnqiueRespawn(ByVal Spot As ReSpawn_)
            For i = 0 To RefRespawnsUnique.Count - 1
                If RefRespawnsUnique(i).Pk2ID = Spot.Pk2ID Then
                    RefRespawnsUnique(i).Spots.Add(Spot.Position)
                    Exit Sub
                End If
            Next

            'Only if no Respawn Colletor Exitis
            Dim tmp As New ReSpawnUnique_
            tmp.Pk2ID = Spot.Pk2ID
            tmp.Spots = New List(Of Position)
            RefRespawnsUnique.Add(tmp)
        End Sub

#Region "Helper Functions"

        Public Function GetRespawn(ByVal SpotId As Integer) As ReSpawn_
            For SpotIndex = 0 To RefRespawns.Count - 1
                If RefRespawns(SpotIndex).SpotID = SpotId Then
                    Return RefRespawns(SpotIndex)
                End If
            Next
            Return New ReSpawn_ With {.SpotID = -1}
        End Function

        Private Function GetSpawnCount(ByVal SpotID As Long) As Integer
            Dim Count As Integer = 0
            Dim tmplist As Array = MobList.Keys.ToArray
            For Each key In tmplist
                If MobList.ContainsKey(key) Then
                    Dim Mob_ As cMonster = MobList.Item(key)
                    If Mob_.SpotID = SpotID Then
                        Count += 1
                    End If
                End If
            Next
            Return Count
        End Function

        Private Function IsUniqueSpawned(ByVal Pk2ID As UInteger) As Boolean

            Dim tmplist As Array = MobList.Keys.ToArray
            For Each key In tmplist
                If MobList.ContainsKey(key) Then
                    Dim Mob_ As cMonster = MobList.Item(key)
                    If Mob_.Pk2ID = Pk2ID And Mob_.SpotID <> -1 Then
                        Return True
                    End If
                End If
            Next
            Return False
        End Function

        Private Function GetCountPerSector(ByVal Xsec As Byte, ByVal Ysec As Byte) As Integer
            Dim Count As Integer

            Dim tmplist As Array = MobList.Keys.ToArray
            For Each key In tmplist
                If MobList.ContainsKey(key) Then
                    Dim Mob_ As cMonster = MobList.Item(key)
                    If Mob_.Position_Spawn.XSector = Xsec And Mob_.Position_Spawn.YSector = Ysec Then
                        Count += 1
                    End If
                End If
            Next
            Return Count
        End Function

#End Region

        Class ReSpawn_
            Public SpotID As Long
            Public Pk2ID As UInteger
            Public Position As Position
            Public Angle As UShort
            Public SpawnCount As Short
        End Class

        Class ReSpawnUnique_
            Public Pk2ID As UInteger
            Public Spots As List(Of Position)
        End Class
    End Module
End Namespace
