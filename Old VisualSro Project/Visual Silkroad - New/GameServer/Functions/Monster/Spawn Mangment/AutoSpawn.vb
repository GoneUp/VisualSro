Imports System.IO
Imports System.Globalization
Imports System.Text

Namespace GameServer.Functions
    Module AutoSpawn
        Public Sub LoadAutoSpawn(ByVal path As String)
            Dim lines As String() = File.ReadAllLines(path)
            For i As Integer = 0 To lines.Length - 1
                If lines(i).StartsWith("//") = False Then
                    Dim pos As New Position
                    Dim tmp As New ReSpawn_
                    Dim Angle As UShort = 0

                    'If My.Computer.Info.OSFullName.Contains("x64") = True Then
                    '    lines(i) = lines(i).Replace(",", ".")
                    'End If
                    lines(i) = lines(i).Replace(".", ",")

                    Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

                    Dim Pk2ID As UInt32 = tmpString(0)
                    Dim obj_ As SilkroadObject = GetObject(Pk2ID)
                    If tmpString.Length = 6 Then
                        Angle = Math.Round((CInt(tmpString(5)) * 65535) / 360)
                    End If

                    Dim tmpSectors As String = Hex(tmpString(1))
                    If tmpSectors.Length = 8 Then
                        tmpSectors = tmpSectors.Substring(tmpSectors.Length - 4, 4)
                    End If
                    If tmpSectors.Length <> 4 Then
                        Debug.Print("Fuck")
                    End If

                    pos.XSector = Byte.Parse(tmpSectors.Substring(2, 2), NumberStyles.HexNumber)
                    pos.YSector = Byte.Parse(tmpSectors.Substring(0, 2), NumberStyles.HexNumber)
                    pos.X = CSng(tmpString(2))
                    pos.Z = CSng(tmpString(3))
                    pos.Y = CSng(tmpString(4))


                    tmp.Pk2ID = Pk2ID
                    tmp.SpotID = i + 10
                    tmp.Position = pos

                    Select Case obj_.Type
                        Case SilkroadObject.Type_.Mob_Normal
                            RefRespawns.Add(tmp)
                            For s = 1 To Settings.Server_SpawnRate
                                SpawnMob(Pk2ID, GetRadomMobType, pos, 0, tmp.SpotID)
                            Next
                        Case SilkroadObject.Type_.Mob_Cave
                            RefRespawns.Add(tmp)
                            For s = 1 To Settings.Server_SpawnRate
                                SpawnMob(Pk2ID, GetRadomMobType, pos, 0, tmp.SpotID)
                            Next
                        Case SilkroadObject.Type_.Mob_Unique
                            AddUnqiueRespawn(tmp)
                        Case SilkroadObject.Type_.Npc
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
            Dim str As New StringBuilder


            For Each key In MobList.Keys.ToList
                If MobList.ContainsKey(key) Then
                    Dim mob As cMonster = MobList.Item(key)
                    If mob.Mob_Type = 3 Then
                        Continue For
                    End If

                    str.Append(CStr(mob.Pk2ID) & ControlChars.Tab)
                    str.Append(
                        CStr(
                            Integer.Parse(
                                ByteFromInteger(mob.Position.YSector) & ByteFromInteger(mob.Position.XSector),
                                NumberStyles.HexNumber)) & ControlChars.Tab)
                    str.Append(CStr(mob.Position.X) & ControlChars.Tab)
                    str.Append(CStr(mob.Position.Z) & ControlChars.Tab)
                    str.Append(CStr(mob.Position.Y) & ControlChars.Tab)
                    str.Append(CStr(Math.Round((mob.Angle * 360) / 65535)))
                    str.Append(ControlChars.NewLine)
                End If
            Next

            For Each unique In RefRespawnsUnique
                For Each spot In unique.Spots
                    str.Append(CStr(unique.Pk2ID) & ControlChars.Tab)
                    str.Append(
                        CStr(
                            Integer.Parse(
                                ByteFromInteger(spot.YSector) & ByteFromInteger(spot.XSector),
                                NumberStyles.HexNumber)) & ControlChars.Tab)
                    str.Append(CStr(spot.X) & ControlChars.Tab)
                    str.Append(CStr(spot.Z) & ControlChars.Tab)
                    str.Append(CStr(spot.Y) & ControlChars.Tab)
                    str.Append(0)
                    str.Append(ControlChars.NewLine)
                Next
            Next


            For Each key In NpcList.Keys.ToList
                If NpcList.ContainsKey(key) Then
                    Dim npc As cNPC = NpcList.Item(key)
                    str.Append(CStr(npc.Pk2ID) & ControlChars.Tab)
                    str.Append(
                        CStr(
                            Integer.Parse(
                                ByteFromInteger(npc.Position.YSector) & ByteFromInteger(npc.Position.XSector),
                                NumberStyles.HexNumber)) & ControlChars.Tab)
                    str.Append(CStr(npc.Position.X) & ControlChars.Tab)
                    str.Append(CStr(npc.Position.Z) & ControlChars.Tab)
                    str.Append(CStr(npc.Position.Y) & ControlChars.Tab)
                    str.Append(CStr(Math.Round((npc.Angle * 360) / 65535)))
                    str.Append(ControlChars.NewLine)
                End If
            Next


            File.WriteAllText(path, str.ToString())
        End Sub

        Private Function ByteFromInteger(ByVal data As Integer) As String
            Dim str As String = Nothing
            str = Convert.ToString(data, &H10).ToUpper()
            If str.Length = 1 Then
                str = "0" & str
            End If
            Return str
        End Function
    End Module
End Namespace