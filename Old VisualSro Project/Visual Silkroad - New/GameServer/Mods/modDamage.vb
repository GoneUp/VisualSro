Namespace GameServer.GameMod.Damage
    Module modDamage

        Public Mod_Name As String = "mod_Damage"
        Public Settings(1500) As Settings_

        Public Sub SendDamageInfo(ByVal MobUniqueId As Integer)
            Dim Mob_ As Functions.cMonster = Functions.MobList(MobUniqueId)
            Dim ref_ As Object_ = GetObjectById(Mob_.Pk2ID)

            'Sort
            Dim DmgList As List(Of Functions.cDamageDone) = Functions.MobList(MobUniqueId).DamageFromPlayer
            Dim sort = From elem In DmgList Order By elem.Damage Descending Select elem Take 9

            'Build Info
            Dim sDmg As String = String.Format("== Damage Statistic for Mob: {0} ==   ", ref_.RealName)
            Dim rank As Integer = 1
            For i = 0 To sort.Count - 1
                sDmg += String.Format("Rank: {0}, Name: {1}, Damage: {2} ; ", rank, Functions.PlayerData(DmgList(i).PlayerIndex).CharacterName, DmgList(i).Damage)
                rank += 1
            Next

            'Send it
            For i = 0 To sort.Count - 1
                If IsSendingAllowed(DmgList(i).PlayerIndex, Mob_.Mob_Type) Then
                    Functions.SendPm(DmgList(i).PlayerIndex, sDmg, "[DAMAGE_MOD]")
                End If
            Next
        End Sub

        Public Sub OnServerStart(ByVal Slots As Integer)
            ReDim Settings(Slots)

            For i = 0 To Settings.Count - 1
                Settings(i) = New Settings_
            Next
        End Sub

        Public Sub OnPlayerCreate(ByVal CharId As UInteger, ByVal Index_ As Integer)
            Try
                DataBase.SaveQuery(String.Format("INSERT INTO coustum(ownerid, name, settings) VALUE ('{0}','damage','0,255,255')", CharId))
            Catch ex As Exception
                Log.WriteSystemLog(String.Format("[MOD_DAMAGE][CREATE][Index:{0},CHAR:{1}]", Index_, CharId))
            End Try
        End Sub

        Public Sub OnPlayerLogon(ByVal Index_)
            Try
                Dim tmpSet_ As DataSet = DataBase.GetDataSet(String.Format("SELECT * FROM coustum WHERE ownerid='{0}' AND name='damage'", Functions.PlayerData(Index_).CharacterId))
                Dim tmp As New Settings_
                tmp.CharacterId = Functions.PlayerData(Index_).CharacterId

                Dim I = tmpSet_.Tables(0).Rows.Count
                If tmpSet_.Tables(0).Rows.Count > 1 Or tmpSet_.Tables(0).Rows.Count = 0 Then
                    'Db Entry is not created
                    Settings(Index_) = tmp
                    Exit Sub
                End If


                Dim setting As String = tmpSet_.Tables(0).Rows(0).Item(3)
                Dim tmpString As String() = setting.Split(",")
                tmp.Send_Normal = CBool(tmpString(0))
                tmp.Send_Giant = CBool(tmpString(1))
                tmp.Send_Unique = CBool(tmpString(2))
                Settings(Index_) = tmp
            Catch ex As Exception
                Log.WriteSystemLog(String.Format("[MOD_DAMAGE][LOAD][Index:{0},CHAR:{1}]", Index_, Functions.PlayerData(Index_).CharacterName))
            End Try
        End Sub

        Public Sub OnPlayerLogoff(ByVal Index_)
            DataBase.SaveQuery(String.Format("UPDATE coustum SET settings='{0}' WHERE ownerid='{1}' AND name='damage'", GenerateSettings(Index_), Functions.PlayerData(Index_).CharacterId))
            Settings(Index_) = New Settings_
        End Sub

        Public Sub ParseMessage(ByVal Index_ As Integer, ByVal Message As String)
            Try
                If Message <> "" Then
                    Dim tmpString As String() = Message.Split(",")
                    Settings(Index_).Send_Normal = tmpString(0)
                    Settings(Index_).Send_Giant = tmpString(1)
                    Settings(Index_).Send_Unique = tmpString(2)
                    DataBase.SaveQuery(String.Format("UPDATE coustum SET settings='{0}' WHERE ownerid='{1}' AND name='damage'", GenerateSettings(Index_), Functions.PlayerData(Index_).CharacterId))
                    Functions.SendPm(Index_, "Updated Settings of Damage Mod. Normal, Champions is: " & Settings(Index_).Send_Normal & ",  Giant is: " & Settings(Index_).Send_Giant & ", Unique is: " & Settings(Index_).Send_Unique, "[DAMAGE_MOD]")
                End If
            Catch ex As Exception
                Log.WriteSystemLog(String.Format("[MOD_DAMAGE][PARSE: {0}][Index:{1}]", Message, Index_))
            End Try
        End Sub




        Private Function GenerateSettings(ByVal Index_ As Integer) As String
            '[Normal],[Giant],[Unique] (On = 1, Off=0)(Normal = Normal & Champion Mobs
            Return CByte(Settings(Index_).Send_Normal) & "," & CByte(Settings(Index_).Send_Giant) & "," & CByte(Settings(Index_).Send_Unique)
        End Function

        Private Function IsSendingAllowed(ByVal Index_ As Integer, ByVal Mob_Type As Integer) As Boolean
            Select Case Mob_Type
                Case 0, 1, 16, 17 'Normal, Champ, Party N, Party Champ
                    If Settings(Index_).Send_Normal Then
                        Return True
                    Else
                        Return False
                    End If
                Case 4, 20
                    If Settings(Index_).Send_Giant Then
                        Return True
                    Else
                        Return False
                    End If
                Case 3, 4, 5, 6
                    If Settings(Index_).Send_Unique Then
                        Return True
                    Else
                        Return False
                    End If
                Case Else
                    Return True
            End Select

        End Function
        Class Settings_
            Public CharacterId As UInteger
            Public Send_Normal As Boolean
            Public Send_Giant As Boolean
            Public Send_Unique As Boolean
        End Class

    End Module
End Namespace
