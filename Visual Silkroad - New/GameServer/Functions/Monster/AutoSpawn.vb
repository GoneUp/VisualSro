Namespace GameServer.Functions
    Module AutoSpawn



        Public Sub LoadAutoSpawn(ByVal path As String)
            Dim lines As String() = IO.File.ReadAllLines(path)
            For i As Integer = 0 To lines.Length - 1
                If lines(i).StartsWith("//") = False Then

                    If My.Computer.Info.OSFullName.Contains("x64") = True Then
                        lines(i) = lines(i).Replace(",", ".")
                    End If
                    Dim r = My.Computer.Info.OSFullName
                    Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

                    Dim pos As New Position
                    Dim tmp As New ReSpawn_

                    Dim Pk2ID As UInt32 = tmpString(0)
                    Dim Angle As UShort = Math.Round((CInt(tmpString(5)) * 65535) / 360)
                    Dim obj_ As Object_ = GetObjectById(Pk2ID)
                    pos.X = CSng(tmpString(2))
                    pos.Z = CSng(tmpString(3))
                    pos.Y = CSng(tmpString(4))
                    pos.XSector = Byte.Parse(Convert.ToUInt16(tmpString(1)).ToString("X").Substring(2, 2), System.Globalization.NumberStyles.HexNumber)
                    pos.YSector = Byte.Parse(Convert.ToUInt16(tmpString(1)).ToString("X").Substring(0, 2), System.Globalization.NumberStyles.HexNumber)

                    tmp.Pk2ID = Pk2ID
                    tmp.SpotID = i
                    tmp.Position = pos
                    RefRespawns.Add(tmp)

                    Select Case obj_.Type
                        Case Object_.Type_.Mob_Normal
                            SpawnMob(Pk2ID, GetRadomMobType, pos, 0, i)
                        Case Object_.Type_.Mob_Cave
                            SpawnMob(Pk2ID, GetRadomMobType, pos, 0, i)
                        Case Object_.Type_.Npc
                            SpawnNPC(Pk2ID, pos, Angle)
                    End Select
                End If
            Next
        End Sub


        Private Function GetRadomMobType() As Byte
            Dim num As Integer = Rnd() * 10

            If num <= 5 Then
                Return 0
            ElseIf num >= 6 And num <= 9 Then
                Return 1
            ElseIf num = 10 Then
                Return 4
            End If

        End Function


        Public Sub SaveAutoSpawn(ByVal path As String)
            Dim str As String = ""

            For i = 0 To MobList.Count - 1
                str += CStr(MobList(i).Pk2ID) & ControlChars.Tab
                str += CStr(Integer.Parse(ByteFromInteger(MobList(i).Position.YSector) & ByteFromInteger(MobList(i).Position.XSector), Globalization.NumberStyles.HexNumber)) & ControlChars.Tab
                str += CStr(MobList(i).Position.X) & ControlChars.Tab
                str += CStr(MobList(i).Position.Z) & ControlChars.Tab
                str += CStr(MobList(i).Position.Y) & ControlChars.Tab
                str += CStr(Math.Round((MobList(i).Angle * 360) / 65535))
                str += ControlChars.NewLine
            Next


            For i = 0 To NpcList.Count - 1
                str += CStr(NpcList(i).Pk2ID) & ControlChars.Tab
                str += CStr(Integer.Parse(ByteFromInteger(NpcList(i).Position.YSector) & ByteFromInteger(NpcList(i).Position.XSector), Globalization.NumberStyles.HexNumber)) & ControlChars.Tab
                str += CStr(NpcList(i).Position.X) & ControlChars.Tab
                str += CStr(NpcList(i).Position.Z) & ControlChars.Tab
                str += CStr(NpcList(i).Position.Y) & ControlChars.Tab
                str += CStr(Math.Round((NpcList(i).Angle * 360) / 65535))
                str += ControlChars.NewLine
            Next


            IO.File.WriteAllText(path, str)
        End Sub

        Public Function ByteFromInteger(ByVal data As Integer) As String
            Dim str As String = Nothing
            str = System.Convert.ToString(data, &H10).ToUpper()
            If str.Length = 1 Then
                str = "0" & str
            End If
            Return str
        End Function

        Public Sub CheckForRespawns()
            For i = 0 To RefRespawns.Count - 1
                If IsSpawned(i) = False Then

                End If
            Next
        End Sub

        Private Function IsSpawned(ByVal SpotID As Long) As Boolean
            For i = 0 To MobList.Count - 1
                If MobList(i).SpotID = SpotID Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Sub ReSpawnMob(ByVal SpotIndex As Integer)
            Dim obj_ As Object_ = GetObjectById(RefRespawns(SpotIndex).Pk2ID)

            Select Case obj_.Type
                Case Object_.Type_.Mob_Normal
                    SpawnMob(RefRespawns(SpotIndex).Pk2ID, GetRadomMobType, RefRespawns(SpotIndex).Position, 0, SpotIndex)
                Case Object_.Type_.Mob_Cave
                    SpawnMob(RefRespawns(SpotIndex).Pk2ID, GetRadomMobType, RefRespawns(SpotIndex).Position, 0, SpotIndex)
                Case Object_.Type_.Npc
                    SpawnNPC(RefRespawns(SpotIndex).Pk2ID, RefRespawns(SpotIndex).Position, RefRespawns(SpotIndex).Angle)
            End Select
        End Sub

        Structure ReSpawn_
            Public SpotID As Long
            Public Pk2ID As UInteger
            Public Position As Position
            Public Angle As UShort
        End Structure
    End Module
End Namespace