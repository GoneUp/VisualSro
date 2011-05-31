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

                    Select Case obj_.Type
                        Case Object_.Type_.Mob_Normal
                            For s = 1 To Settings.Server_SpawnRate
                                SpawnMob(Pk2ID, GetRadomMobType, pos, 0, tmp.SpotID)
                            Next
                            RefRespawns.Add(tmp)
                        Case Object_.Type_.Mob_Cave
                            For s = 1 To Settings.Server_SpawnRate
                                SpawnMob(Pk2ID, GetRadomMobType, pos, 0, tmp.SpotID)
                            Next
                            RefRespawns.Add(tmp)
                        Case Object_.Type_.Mob_Unique
                            AddUnqiueRespawn(tmp)
                        Case Object_.Type_.Npc
                            SpawnNPC(Pk2ID, pos, Angle)
                    End Select
                End If
            Next
        End Sub

        Public Function GetRadomMobType() As Byte
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


            For Each key In MobList.Keys.ToList
                If MobList.ContainsKey(key) Then
                    Dim Mob_ As cMonster = MobList.Item(key)
                    str += CStr(Mob_.Pk2ID) & ControlChars.Tab
                    str += CStr(Integer.Parse(ByteFromInteger(Mob_.Position.YSector) & ByteFromInteger(Mob_.Position.XSector), Globalization.NumberStyles.HexNumber)) & ControlChars.Tab
                    str += CStr(Mob_.Position.X) & ControlChars.Tab
                    str += CStr(Mob_.Position.Z) & ControlChars.Tab
                    str += CStr(Mob_.Position.Y) & ControlChars.Tab
                    str += CStr(Math.Round((Mob_.Angle * 360) / 65535))
                    str += ControlChars.NewLine
                End If
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
    End Module
End Namespace