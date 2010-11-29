Namespace GameServer.Functions
    Module AutoSpawn
        Public Sub LoadAutoSpawn(ByVal path As String)
            Dim lines As String() = IO.File.ReadAllLines(path)
            For i As Integer = 0 To lines.Length - 1
                'Dim line2 As String = lines(i).Replace(".", ",")
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim Pk2ID As UInt32 = tmpString(0)
                Dim refobject As Object_ = GetObjectById(Pk2ID)
                Dim pos As New Position

                pos.X = CSng(tmpString(2))
                pos.Z = CSng(tmpString(3))
                pos.Y = CSng(tmpString(4))
                pos.XSector = Byte.Parse(Convert.ToInt16(tmpString(1)).ToString("X").Substring(2, 2), System.Globalization.NumberStyles.HexNumber)
                pos.YSector = Byte.Parse(Convert.ToInt16(tmpString(1)).ToString("X").Substring(0, 2), System.Globalization.NumberStyles.HexNumber)


                Select Case refobject.Type
                    Case Object_.Type_.Mob_Normal
                        SpawnMob(Pk2ID, GetRadomMobType, pos)
                    Case Object_.Type_.Npc
                        SpawnNPC(Pk2ID, pos)
                End Select

nexti:
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
                str += MobList(i).Pk2ID
                str += Integer.Parse(ByteFromInteger(MobList(i).Position.YSector) & ByteFromInteger(MobList(i).Position.XSector), Globalization.NumberStyles.HexNumber)
                str += MobList(i).Position.X
                str += MobList(i).Position.Z
                str += MobList(i).Position.Y
                str += ControlChars.NewLine
            Next


            For i = 0 To NpcList.Count - 1
                str += NpcList(i).Pk2ID
                str += Integer.Parse(ByteFromInteger(NpcList(i).Position.YSector) & ByteFromInteger(NpcList(i).Position.XSector), Globalization.NumberStyles.HexNumber)
                str += NpcList(i).Position.X
                str += NpcList(i).Position.Z
                str += NpcList(i).Position.Y
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