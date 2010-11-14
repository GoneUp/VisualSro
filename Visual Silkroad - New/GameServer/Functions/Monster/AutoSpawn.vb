Namespace GameServer.Functions
    Module AutoSpawn
        Public Sub LoadAutoSpawn(ByVal path As String)
            Dim lines As String() = IO.File.ReadAllLines(path)
            For i As Integer = 0 To lines.Length - 1
                Dim line2 As String = lines(i).Replace(".", ",")
                Dim tmpString As String() = line2.Split(ControlChars.Tab)
                Dim Pk2ID As UInt32 = tmpString(0)
                Dim refobject As Object_ = GetObjectById(Pk2ID)
                Dim pos As New Position

                pos.X = (tmpString(2))
                pos.Z = (tmpString(3))
                pos.Y = (tmpString(4))
                pos.XSector = Byte.Parse(Convert.ToInt16(tmpString(1)).ToString("X").Substring(2, 2), System.Globalization.NumberStyles.HexNumber)
                pos.YSector = Byte.Parse(Convert.ToInt16(tmpString(1)).ToString("X").Substring(0, 2), System.Globalization.NumberStyles.HexNumber)


                Select Case refobject.Type
                    Case Object_.Type_.Mob_Normal
                        SpawnMob(Pk2ID, 0, pos)
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
    End Module
End Namespace