Namespace GameServer
    Module GameDb

        'Timer
        Public WithEvents GameDbUpdate As New Timers.Timer

        'User
        Public UserCount As Integer
        Public Users() As cCharListing.UserArray

        'Chars
        Public CharCount As Integer
        Public Chars() As [cChar]

        Private First As Boolean = False



        Public Sub UpdateData() Handles GameDbUpdate.Elapsed

            GameDbUpdate.Stop()
            GameDbUpdate.Interval = 60000 '1minute
            If First = False Then
                Console.WriteLine("Loading Data from Database. This can take some Time.")
            End If

            GetUserData()
            GetCharData()
            If First = False Then
                Console.WriteLine("Loading complete!")
                First = True
            End If

            GameDbUpdate.Start()

        End Sub

        Public Sub GetUserData()

            Dim tmp As DataSet = GameServer.db.GetDataSet("SELECT * From Users")
            UserCount = tmp.Tables(0).Rows.Count

            ReDim Users(UserCount - 1) '-1 machen

            For i = 0 To UserCount - 1
                Users(i).Id = CInt(tmp.Tables(0).Rows(i).ItemArray(0))
                Users(i).Name = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                Users(i).Pw = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
                Users(i).FailedLogins = CInt(tmp.Tables(0).Rows(i).ItemArray(3))
                Users(i).Banned = CBool(tmp.Tables(0).Rows(i).ItemArray(4))
            Next

        End Sub

        Public Sub GetCharData()

            Dim tmp As DataSet = GameServer.db.GetDataSet("SELECT * From characters")
            CharCount = tmp.Tables(0).Rows.Count

            ReDim Chars(CharCount - 1) '-1 machen

            For i = 0 To Chars.Length - 1
                Chars(i) = New [cChar]
                Chars(i).CharacterId = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                Chars(i).AccountID = CUInt(tmp.Tables(0).Rows(i).ItemArray(1))
                Chars(i).CharacterName = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
                Chars(i).Model = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
                Chars(i).Volume = CUInt(tmp.Tables(0).Rows(i).ItemArray(4))
                Chars(i).Level = CUInt(tmp.Tables(0).Rows(i).ItemArray(5))
                Chars(i).Experience = CULng(tmp.Tables(0).Rows(i).ItemArray(6))
                Chars(i).Strength = CUInt(tmp.Tables(0).Rows(i).ItemArray(7))
                Chars(i).Intelligence = CUInt(tmp.Tables(0).Rows(i).ItemArray(8))
                Chars(i).Gold = CUInt(tmp.Tables(0).Rows(i).ItemArray(11))
                Chars(i).SkillPoints = CUInt(tmp.Tables(0).Rows(i).ItemArray(12))
                Chars(i).GM = CUInt(tmp.Tables(0).Rows(i).ItemArray(13))
                Chars(i).XSector = CByte(tmp.Tables(0).Rows(i).ItemArray(14))
                Chars(i).YSector = CByte(tmp.Tables(0).Rows(i).ItemArray(15))
                Chars(i).X = CDbl(tmp.Tables(0).Rows(i).ItemArray(16))
                Chars(i).Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(17))
                Chars(i).Z = CDbl(tmp.Tables(0).Rows(i).ItemArray(18))
                Chars(i).CHP = CUInt(tmp.Tables(0).Rows(i).ItemArray(19))
                Chars(i).CMP = CUInt(tmp.Tables(0).Rows(i).ItemArray(20))
                Chars(i).MinPhy = CUInt(tmp.Tables(0).Rows(i).ItemArray(21))
                Chars(i).MaxPhy = CUInt(tmp.Tables(0).Rows(i).ItemArray(22))
                Chars(i).MinMag = CUInt(tmp.Tables(0).Rows(i).ItemArray(23))
                Chars(i).MaxMag = CUInt(tmp.Tables(0).Rows(i).ItemArray(24))
                Chars(i).PhyDef = CUInt(tmp.Tables(0).Rows(i).ItemArray(25))
                Chars(i).MagDef = CUInt(tmp.Tables(0).Rows(i).ItemArray(26))
                Chars(i).Hit = CUInt(tmp.Tables(0).Rows(i).ItemArray(27))
                Chars(i).Parry = CUInt(tmp.Tables(0).Rows(i).ItemArray(28))
                Chars(i).WalkSpeed = CUInt(tmp.Tables(0).Rows(i).ItemArray(29))
                Chars(i).RunSpeed = CUInt(tmp.Tables(0).Rows(i).ItemArray(30))
                Chars(i).BerserkSpeed = CUInt(tmp.Tables(0).Rows(i).ItemArray(31))
                Chars(i).BerserkBar = CUInt(tmp.Tables(0).Rows(i).ItemArray(32))
                Chars(i).PVP = CUInt(tmp.Tables(0).Rows(i).ItemArray(32))
                Chars(i).SetCharStats()
            Next



        End Sub

        Public Function GetUserWithID(ByVal id As String) As Integer

            Dim i As Integer = 0

            For i = 0 To Users.Length
                If Users(i).Name = id Then
                    Exit For
                End If
            Next

            If Users.Length = i Then
                Return -1
            End If

            Return i



        End Function


    End Module
End Namespace