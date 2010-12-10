Namespace GameServer.DatabaseCore
	Module GameDb

		'Timer
        Public WithEvents GameDbUpdate As New System.Timers.Timer

		'User
		Public UserCount As Integer
		Public Users() As cCharListing.UserArray

		'Chars
		Public CharCount As Integer
        Public Chars() As [cChar]
        Public Hotkeys As New List(Of cHotKey)

        'Itemcount
        Public ItemCount As Integer
        Public AllItems() As cInvItem

        'Masterys
        Public MasteryCount As Integer
        Public Masterys() As cMastery

        'Skills
        Public Skills() As cSkill

        'Guilds
        Public Guilds As New List(Of cGuild)

        Private First As Boolean = False
        Public UniqueIdCounter As UInteger = 1



        Public Sub UpdateData() Handles GameDbUpdate.Elapsed
            Try

                GameDbUpdate.Stop()
                GameDbUpdate.Interval = 20000 '20 secs

                If First = False Then
                    Log.WriteSystemLog("Execute all saved Querys. This can take some Time.")
                    ExecuteSavedQuerys()
                    Log.WriteSystemLog("Query saving done. Loading Playerdata from DB now.")

                    GetCharData() 'Only Update for the First Time! 
                    GetItemData()
                    GetMasteryData()
                    GetSkillData()
                    GetPositionData()
                    GetHotkeyData()
                    First = True
                End If

                GetUserData()
                GameDbUpdate.Start()

            Catch ex As Exception
                Log.WriteSystemLog("[REFRESH ERROR][" & ex.Message & " Stack: " & ex.StackTrace & "]")
            End Try
        End Sub

#Region "Get from DB"
        Public Sub ExecuteSavedQuerys()
            Try
                Dim lines() As String = IO.File.ReadAllLines(System.AppDomain.CurrentDomain.BaseDirectory & "save.txt")
                For Each commando In lines
                    DataBase.InsertData(commando)
                Next
                IO.File.Delete(System.AppDomain.CurrentDomain.BaseDirectory & "save.txt")
            Catch ex As Exception
                'Log .WriteLog ("Database Error: " & ex.Message)
            End Try

        End Sub

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
                Users(i).Silk = CUInt(tmp.Tables(0).Rows(i).ItemArray(7))
            Next

        End Sub

        Public Sub GetCharData()

            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From characters")
            CharCount = tmp.Tables(0).Rows.Count

            If CharCount >= 1 Then
                ReDim Chars(CharCount - 1)

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
                    Chars(i).Deleted = CByte(tmp.Tables(0).Rows(i).ItemArray(12))
                    Chars(i).DeletionTime = (tmp.Tables(0).Rows(i).ItemArray(13))
                    Chars(i).Gold = CULng(tmp.Tables(0).Rows(i).ItemArray(14))
                    Chars(i).SkillPoints = CUInt(tmp.Tables(0).Rows(i).ItemArray(15))
                    Chars(i).GM = CBool(tmp.Tables(0).Rows(i).ItemArray(16))
                    Chars(i).Position.XSector = CByte(tmp.Tables(0).Rows(i).ItemArray(17))
                    Chars(i).Position.YSector = CByte(tmp.Tables(0).Rows(i).ItemArray(18))
                    Chars(i).Position.X = CDbl(tmp.Tables(0).Rows(i).ItemArray(19))
                    Chars(i).Position.Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(20)) 'xsec
                    Chars(i).Position.Z = CDbl(tmp.Tables(0).Rows(i).ItemArray(21))
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
                    Chars(i).HelperIcon = CByte(tmp.Tables(0).Rows(i).ItemArray(38))
                    Chars(i).SetCharGroundStats()

                    Chars(i).UniqueId = GetUnqiueID()
                Next

            Else
                ReDim Chars(0)
            End If
        End Sub

        Public Sub GetItemData()
            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From items")
            ItemCount = tmp.Tables(0).Rows.Count

            If ItemCount >= 1 Then
                ReDim AllItems(ItemCount - 1)

                For i = 0 To (ItemCount - 1)
                    AllItems(i) = New cInvItem
                    AllItems(i).DatabaseID = CInt(tmp.Tables(0).Rows(i).ItemArray(0))
                    AllItems(i).Pk2Id = CInt(tmp.Tables(0).Rows(i).ItemArray(1))
                    AllItems(i).OwnerCharID = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                    AllItems(i).Plus = CByte(tmp.Tables(0).Rows(i).ItemArray(3))
                    AllItems(i).Slot = CByte(tmp.Tables(0).Rows(i).ItemArray(4))
                    AllItems(i).Amount = CUShort(tmp.Tables(0).Rows(i).ItemArray(5))
                    AllItems(i).Durability = CUInt(tmp.Tables(0).Rows(i).ItemArray(6))
                Next
            Else
                ReDim AllItems(0)
            End If

        End Sub

        Public Sub GetMasteryData()

            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From masteries")
            MasteryCount = tmp.Tables(0).Rows.Count

            If MasteryCount >= 1 Then

                ReDim Masterys(MasteryCount - 1)

                For i = 0 To MasteryCount - 1
                    Masterys(i) = New cMastery
                    Masterys(i).OwnerID = CUInt(tmp.Tables(0).Rows(i).ItemArray(1))
                    Masterys(i).MasteryID = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                    Masterys(i).Level = CByte(tmp.Tables(0).Rows(i).ItemArray(3))
                Next
            Else
                ReDim Masterys(0)
            End If
        End Sub

        Public Sub GetSkillData()
            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From skills")
            Dim Count As Integer = tmp.Tables(0).Rows.Count

            If Count >= 1 Then
                ReDim Skills(Count - 1)

                For i = 0 To Count - 1
                    Skills(i) = New cSkill
                    Skills(i).OwnerID = CUInt(tmp.Tables(0).Rows(i).ItemArray(1))
                    Skills(i).SkillID = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                Next

            Else
                ReDim Skills(0)
            End If
        End Sub
        Public Sub GetPositionData()
            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From positions")
            Dim Count As Integer = tmp.Tables(0).Rows.Count

            For i = 0 To Count - 1
                Dim CharID As UInteger = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                For c = 0 To Chars.Count - 1
                    If Chars(c).CharacterId = CharID Then
                        Chars(c).Position_Return.XSector = CByte(tmp.Tables(0).Rows(i).ItemArray(1))
                        Chars(c).Position_Return.YSector = CByte(tmp.Tables(0).Rows(i).ItemArray(2))
                        Chars(c).Position_Return.X = CDbl(tmp.Tables(0).Rows(i).ItemArray(3))
                        Chars(c).Position_Return.Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(4))
                        Chars(c).Position_Return.Z = CDbl(tmp.Tables(0).Rows(i).ItemArray(5))

                        Chars(c).Position_Recall.XSector = CByte(tmp.Tables(0).Rows(i).ItemArray(6))
                        Chars(c).Position_Recall.YSector = CByte(tmp.Tables(0).Rows(i).ItemArray(7))
                        Chars(c).Position_Recall.X = CDbl(tmp.Tables(0).Rows(i).ItemArray(8))
                        Chars(c).Position_Recall.Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(9))
                        Chars(c).Position_Recall.Z = CDbl(tmp.Tables(0).Rows(i).ItemArray(10))

                        Chars(c).Position_Dead.XSector = CByte(tmp.Tables(0).Rows(i).ItemArray(11))
                        Chars(c).Position_Dead.YSector = CByte(tmp.Tables(0).Rows(i).ItemArray(12))
                        Chars(c).Position_Dead.X = CDbl(tmp.Tables(0).Rows(i).ItemArray(13))
                        Chars(c).Position_Dead.Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(14))
                        Chars(c).Position_Dead.Z = CDbl(tmp.Tables(0).Rows(i).ItemArray(15))
                    End If
                Next
            Next
        End Sub

        Public Sub GetHotkeyData()
            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From hotkeys")
            Dim Count As Integer = tmp.Tables(0).Rows.Count

            For i = 0 To Count - 1
                Dim tmp_ As New cHotKey
                tmp_.OwnerID = CUInt(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_.Slot = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_.Type = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
                tmp_.IconID = CUInt(tmp.Tables(0).Rows(i).ItemArray(4))

                Hotkeys.Add(tmp_)
            Next
        End Sub

        Public Sub GetGuildData()
            Dim tmp As DataSet = DataBase.GetDataSet("SELECT * From guild_main")
            Dim Count As Integer = tmp.Tables(0).Rows.Count

            For i = 0 To Count - 1
                Dim tmp_ As New cGuild
                tmp_.GuildID = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                tmp_.GuildName = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_.GuildPoints = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_.GuildLevel = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
                tmp_.GuildNoticeTitle = CStr(tmp.Tables(0).Rows(i).ItemArray(4))
                tmp_.GuildNotice = CStr(tmp.Tables(0).Rows(i).ItemArray(5))

                Guilds.Add(tmp_)
            Next


            tmp = DataBase.GetDataSet("SELECT * From guild_member")
            Count = tmp.Tables(0).Rows.Count

            For i = 0 To Count - 1
                Dim tmp_ As New cGuild.GuildMember_
                tmp_.CharacterID = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                tmp_.GuildID = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_.DonantedGP = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_.Rights.Invite = CBool(tmp.Tables(0).Rows(i).ItemArray(3))
                tmp_.Rights.Kick = CBool(tmp.Tables(0).Rows(i).ItemArray(4))
                tmp_.Rights.Notice = CBool(tmp.Tables(0).Rows(i).ItemArray(5))
                tmp_.Rights.Union = CBool(tmp.Tables(0).Rows(i).ItemArray(6))
                tmp_.Rights.Strorage = CBool(tmp.Tables(0).Rows(i).ItemArray(7))

                For g = 0 To Guilds.Count - 1
                    If Guilds(g).GuildID = tmp_.GuildID Then
                        Guilds(g).Member.Add(tmp_)
                    End If
                Next

                For c = 0 To Chars.Count - 1
                    If Chars(i).CharacterId = tmp_.CharacterID Then
                        Chars(i).InGuild = True
                    End If
                Next
            Next

        End Sub

#End Region

#Region "Get Things from Array"
        Public Function GetUserWithAccName(ByVal id As String) As Integer
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

        Public Function GetUserWithAccID(ByVal id As String) As Integer
            Dim i As Integer = 0
            For i = 0 To Users.Length
                If Users(i).Id = id Then
                    Exit For
                End If
            Next
            If Users.Length = i Then
                Return -1
            End If
            Return i
        End Function
        Public Function FillCharList(ByVal CharArray As cCharListing) As cCharListing

            Dim CharCount As Integer = 0
            CharArray.Chars.Clear()

            For i = 0 To Chars.Length - 1
                If CharArray.LoginInformation.Id = Chars(i).AccountID Then
                    CharArray.Chars.Add(Chars(i))
                    CharCount += 1
                End If
            Next

            CharArray.NumberOfChars = CharCount
            Return CharArray
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
                If (Chars(i) IsNot Nothing) AndAlso (Chars(i).CharacterName = nick) Then
                    free = False
                End If
            Next
            Return free
        End Function

        Public Function GetUnqiueID() As UInteger
            Dim toreturn As UInteger = UniqueIdCounter
            If UniqueIdCounter < UInteger.MaxValue Then
                UniqueIdCounter += 1
            ElseIf UniqueIdCounter = UInteger.MaxValue Then
                UniqueIdCounter = 0
            End If

            Return toreturn
        End Function

        Public Function GetGuildWithGuildID(ByVal GuildID As UInteger) As cGuild
            For i = 0 To Guilds.Count - 1
                If Guilds(i).GuildID = GuildID Then
                    Return Guilds(i)
                End If
            Next
            Return Nothing
        End Function
#End Region

    End Module
End Namespace