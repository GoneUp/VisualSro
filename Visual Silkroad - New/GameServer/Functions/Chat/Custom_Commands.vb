Namespace GameServer.Custom
    Module Costum_Commands
        Public Sub CheckForCoustum(ByVal Msg As String, ByVal Index_ As Integer)
            'This Function is for additional Commands from a GM
            Dim writer As New PacketWriter

            If Msg.StartsWith("\\gold") Then
                Dim tmp As String() = Msg.Split(" ")
                If IsNumeric(tmp(1)) Then
                    Functions.PlayerData(Index_).Gold += CULng(tmp(1))
                    Functions.UpdateGold(Index_)
                End If

            ElseIf Msg.StartsWith("\\sp") Then
                Dim tmp As String() = Msg.Split(" ")
                If IsNumeric(tmp(1)) Then
                    Functions.PlayerData(Index_).SkillPoints += CULng(tmp(1))
                    Functions.UpdateSP(Index_)
                End If

            ElseIf Msg.StartsWith("\\stat") Then
                Dim tmp As String() = Msg.Split(" ")
                If IsNumeric(tmp(1)) Then
                    Functions.PlayerData(Index_).Attributes += CULng(tmp(1))
                    Functions.OnStatsPacket(Index_)
                End If

            ElseIf Msg.StartsWith("\\mastery") Then
                Dim tmp As String() = Msg.Split(" ")
                If IsNumeric(tmp(1)) Then
                    For i = 0 To DatabaseCore.Masterys.Length - 1
                        If DatabaseCore.Masterys(i).OwnerID = Functions.PlayerData(Index_).UniqueId Then
                            DatabaseCore.Masterys(i).Level = tmp(1)

                            writer.Create(ServerOpcodes.Mastery_Up)
                            writer.Byte(1)
                            writer.DWord(DatabaseCore.Masterys(i).MasteryID)
                            writer.Byte(DatabaseCore.Masterys(i).Level)
                            Server.Send(writer.GetBytes, Index_)
                        End If
                    Next
                End If


            ElseIf Msg.StartsWith("\\kick") Then
                Dim tmp As String() = Msg.Split(" ")
                For i As Integer = 0 To Server.MaxClients
                    If Functions.PlayerData(i) IsNot Nothing Then
                        If Functions.PlayerData(i).CharacterName = tmp(1) Then
                            Server.Dissconnect(i)
                        End If
                    End If
                Next

            ElseIf Msg.StartsWith("\\npc") Then
                Dim tmp As String() = Msg.Split(" ")
                If IsNumeric(tmp(1)) Then
                    Functions.SpawnNPC(tmp(1), Functions.PlayerData(Index_).Position)
                End If
            End If
        End Sub




    End Module
End Namespace
