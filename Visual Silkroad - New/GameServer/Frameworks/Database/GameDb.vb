Namespace GameServer.DatabaseCore
    Module GameDb
  

        'Timer
        Public WithEvents GameDbUpdate As New Timers.Timer

        'User
        Public UserCount As Integer
        Public Users() As cCharListing.UserArray

        'Chars
        Public CharCount As Integer
        Public Chars() As [cChar]

        'Itemcount
        Public ItemCount As Integer
        Public AllItems() As cInvItem

        'Masterys
        Public MasteryCount As Integer
        Public Masterys() As cMastery

        Private First As Boolean = False

        Public UniqueIdCounter As Integer = 1



        Public Sub UpdateData() Handles GameDbUpdate.Elapsed

            GameDbUpdate.Stop()
            GameDbUpdate.Interval = 60000 '1minute
            If First = False Then
                Console.WriteLine("Loading Data from Database. This can take some Time.")
            End If

            GetUserData()
            GetCharData()
            GetItemData()
            If First = False Then
                Console.WriteLine("Loading complete!")
                First = True
            End If

            GameDbUpdate.Start()

        End Sub

#Region "Get from DB"

        Public Sub GetUserData()

            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From Users")
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

            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From characters")
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
                Chars(i).Attributes = CUInt(tmp.Tables(0).Rows(i).ItemArray(9))
                Chars(i).HP = CUInt(tmp.Tables(0).Rows(i).ItemArray(10))
                Chars(i).MP = CUInt(tmp.Tables(0).Rows(i).ItemArray(11))
                Chars(i).Gold = CUInt(tmp.Tables(0).Rows(i).ItemArray(14))
                Chars(i).SkillPoints = CUInt(tmp.Tables(0).Rows(i).ItemArray(15))
                Chars(i).GM = CBool(tmp.Tables(0).Rows(i).ItemArray(16))
                Chars(i).XSector = CByte(tmp.Tables(0).Rows(i).ItemArray(17))
                Chars(i).YSector = CByte(tmp.Tables(0).Rows(i).ItemArray(18))
                Chars(i).X = CDbl(tmp.Tables(0).Rows(i).ItemArray(19))
                Chars(i).Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(20)) 'xsec
                Chars(i).Z = CDbl(tmp.Tables(0).Rows(i).ItemArray(21))
                Chars(i).CHP = CUInt(tmp.Tables(0).Rows(i).ItemArray(22))
                Chars(i).CMP = CUInt(tmp.Tables(0).Rows(i).ItemArray(23))
                Chars(i).MinPhy = CUInt(tmp.Tables(0).Rows(i).ItemArray(24))
                Chars(i).MaxPhy = CUInt(tmp.Tables(0).Rows(i).ItemArray(25))
                Chars(i).MinMag = CUInt(tmp.Tables(0).Rows(i).ItemArray(26))
                Chars(i).MaxMag = CUInt(tmp.Tables(0).Rows(i).ItemArray(27))
                Chars(i).PhyDef = CUInt(tmp.Tables(0).Rows(i).ItemArray(28))
                Chars(i).MagDef = CUInt(tmp.Tables(0).Rows(i).ItemArray(29))
                Chars(i).Hit = CUInt(tmp.Tables(0).Rows(i).ItemArray(30))
                Chars(i).Parry = CUInt(tmp.Tables(0).Rows(i).ItemArray(31))
                Chars(i).WalkSpeed = CUInt(tmp.Tables(0).Rows(i).ItemArray(32))
                Chars(i).RunSpeed = CUInt(tmp.Tables(0).Rows(i).ItemArray(33))
                Chars(i).BerserkSpeed = CUInt(tmp.Tables(0).Rows(i).ItemArray(34))
                Chars(i).BerserkBar = CUInt(tmp.Tables(0).Rows(i).ItemArray(35))
                Chars(i).PVP = CByte(tmp.Tables(0).Rows(i).ItemArray(36))
                Chars(i).MaxSlots = CByte(tmp.Tables(0).Rows(i).ItemArray(37))
                Chars(i).SetCharStats()

                Chars(i).UniqueId = UniqueIdCounter
                UniqueIdCounter += 1
            Next



        End Sub

        Public Sub GetItemData()
            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From items")
            ItemCount = tmp.Tables(0).Rows.Count

            ReDim AllItems(ItemCount - 1)

            For i = 0 To (ItemCount - 1)
                AllItems(i) = New cInvItem
                AllItems(i).Pk2Id = CInt(tmp.Tables(0).Rows(i).ItemArray(1))
                AllItems(i).OwnerCharID = CInt(tmp.Tables(0).Rows(i).ItemArray(2))
                AllItems(i).Plus = CByte(tmp.Tables(0).Rows(i).ItemArray(3))
                AllItems(i).Slot = CByte(tmp.Tables(0).Rows(i).ItemArray(4))
                AllItems(i).Amount = CByte(tmp.Tables(0).Rows(i).ItemArray(5))
                AllItems(i).Durability = CByte(tmp.Tables(0).Rows(i).ItemArray(6))
            Next


        End Sub

        Public Sub GetMasteryData()

            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From masteries")
            MasteryCount = tmp.Tables(0).Rows.Count

            ReDim Masterys(MasteryCount - 1)

            For i = 0 To MasteryCount - 1
                Masterys(i) = New cMastery
                Masterys(i).MasteryID = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                Masterys(i).Level = CByte(tmp.Tables(0).Rows(i).ItemArray(1))
                Masterys(i).OwnerID = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
            Next



        End Sub


#End Region

#Region "Get Things from Array"
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

        Public Function FillCharList(ByVal chararray As cCharListing) As cCharListing

            Dim playerCharCount As Integer = 0

            chararray.Chars(0) = New [cChar]
            chararray.Chars(1) = New [cChar]
            chararray.Chars(2) = New [cChar]
            chararray.Chars(3) = New [cChar]

            For i = 0 To (CharCount - 1)
                If chararray.LoginInformation.Id = Chars(i).AccountID Then
                    If chararray.Chars(0).CharacterId = 0 Then
                        chararray.Chars(0) = New [cChar]
                        chararray.Chars(0) = Chars(i)
                        playerCharCount = 1

                    ElseIf chararray.Chars(1).CharacterId = 0 Then
                        chararray.Chars(1) = New [cChar]
                        chararray.Chars(1) = Chars(i)
                        playerCharCount = 2

                    ElseIf chararray.Chars(2).CharacterId = 0 Then
                        chararray.Chars(2) = New [cChar]
                        chararray.Chars(2) = Chars(i)
                        playerCharCount = 3

                    ElseIf chararray.Chars(3).CharacterId = 0 Then
                        chararray.Chars(3) = New [cChar]
                        chararray.Chars(3) = Chars(i)
                        playerCharCount = 4
                    End If
                End If
            Next
            chararray.NumberOfChars = playerCharCount
            Return chararray
        End Function

        Public Function FillInventory(ByVal [char] As [cChar]) As cInventory

            Dim inventory As New cInventory([char].MaxSlots)


            For i = 0 To (AllItems.Length - 1)
                If AllItems(i).OwnerCharID = [char].CharacterId Then
                    inventory.UserItems(AllItems(i).Slot) = AllItems(i)
                End If
            Next

            Return inventory

        End Function

        Public Function CheckNick(ByVal nick As String) As Boolean


            Dim free As Boolean = True
            For i = 0 To Chars.Length - 1
                If Chars(i).CharacterName = nick Then
                    free = False
                End If
            Next


            Return free

        End Function

#End Region
    End Module


End Namespace