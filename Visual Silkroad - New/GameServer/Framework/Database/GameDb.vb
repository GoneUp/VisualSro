Imports GameServer.GameServer.Functions

Namespace GameServer.GameDB
    Module GameDb

        'Timer
        Public WithEvents GameDbUpdate As New System.Timers.Timer

        'User
        Public Users() As cCharListing.UserArray

        'Chars
        Public Chars() As [cChar]
        Public Hotkeys As New List(Of cHotKey)

        'Itemcount
        Public AllItems() As cInvItem

        'Masterys
        Public Masterys() As cMastery

        'Skills
        Public Skills() As cSkill

        'Guilds
        Public Guilds As New List(Of cGuild)

        Private First As Boolean = False

        Public Sub UpdateData() Handles GameDbUpdate.Elapsed
            GameDbUpdate.Stop()
            GameDbUpdate.Interval = 20000 '20 secs

            Try
                If First = False Then
                    Log.WriteSystemLog("Loading Playerdata from DB now.")

                    GetUserData()
                    GetCharData() 'Only Update for the First Time! 
                    GetItemData()
                    GetMasteryData()
                    GetSkillData()
                    GetPositionData()
                    GetHotkeyData()
                    GetGuildData()
                    First = True
                Else
                    GetUserData()
                End If



            Catch ex As Exception
                Log.WriteSystemLog("[REFRESH ERROR][" & ex.Message & " Stack: " & ex.StackTrace & "]")
            End Try

            GameDbUpdate.Start()
        End Sub

#Region "Get from DB"
        Public Sub GetUserData()

            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From Users")
            Dim UserCount = tmp.Tables(0).Rows.Count

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
            Dim CharCount = tmp.Tables(0).Rows.Count

            If CharCount >= 1 Then
                ReDim Chars(CharCount - 1)

                For i = 0 To Chars.Length - 1
                    Chars(i) = New [cChar]
                    Chars(i).Pos_Tracker = New Functions.cPositionTracker(New Position, Chars(i).WalkSpeed, Chars(i).RunSpeed, Chars(i).BerserkSpeed)

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
                    Chars(i).Position.Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(20))
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
                    Chars(i).Pot_HP_Slot = CByte(tmp.Tables(0).Rows(i).ItemArray(39))
                    Chars(i).Pot_HP_Value = CByte(tmp.Tables(0).Rows(i).ItemArray(40))
                    Chars(i).Pot_MP_Slot = CByte(tmp.Tables(0).Rows(i).ItemArray(41))
                    Chars(i).Pot_MP_Value = CByte(tmp.Tables(0).Rows(i).ItemArray(42))
                    Chars(i).Pot_Abormal_Slot = CByte(tmp.Tables(0).Rows(i).ItemArray(43))
                    Chars(i).Pot_Abormal_Value = CByte(tmp.Tables(0).Rows(i).ItemArray(44))
                    Chars(i).Pot_Delay = CByte(tmp.Tables(0).Rows(i).ItemArray(45))

                    If Chars(i).CHP = 0 Then
                        Chars(i).Alive = False
                    End If


                    Chars(i).UniqueId = Id_Gen.GetUnqiueId()
                Next

            Else
                ReDim Chars(0)
            End If
        End Sub

        Public Sub GetItemData()
            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From items")
            Dim ItemCount = tmp.Tables(0).Rows.Count

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
                    AllItems(i).PerDurability = CByte(tmp.Tables(0).Rows(i).ItemArray(8))
                    AllItems(i).PerPhyRef = CByte(tmp.Tables(0).Rows(i).ItemArray(9))
                    AllItems(i).PerMagRef = CByte(tmp.Tables(0).Rows(i).ItemArray(10))
                    AllItems(i).PerPhyAtk = CByte(tmp.Tables(0).Rows(i).ItemArray(11))
                    AllItems(i).PerMagAtk = CByte(tmp.Tables(0).Rows(i).ItemArray(12))
                    AllItems(i).PerPhyDef = CByte(tmp.Tables(0).Rows(i).ItemArray(13))
                    AllItems(i).PerMagDef = CByte(tmp.Tables(0).Rows(i).ItemArray(14))
                    AllItems(i).PerBlock = CByte(tmp.Tables(0).Rows(i).ItemArray(15))
                    AllItems(i).PerCritical = CByte(tmp.Tables(0).Rows(i).ItemArray(16))
                    AllItems(i).PerAttackRate = CByte(tmp.Tables(0).Rows(i).ItemArray(17))
                    AllItems(i).PerParryRate = CByte(tmp.Tables(0).Rows(i).ItemArray(18))
                    AllItems(i).PerPhyAbs = CByte(tmp.Tables(0).Rows(i).ItemArray(19))
                    AllItems(i).PerMagAbs = CByte(tmp.Tables(0).Rows(i).ItemArray(20))

                    AllItems(i).ItemType = GetItemTypeFromDb((CStr(tmp.Tables(0).Rows(i).ItemArray(7))))
                Next
            Else
                ReDim AllItems(0)
            End If

        End Sub

        Public Sub GetMasteryData()

            Dim tmp As DataSet = GameServer.DataBase.GetDataSet("SELECT * From masteries")
            Dim MasteryCount = tmp.Tables(0).Rows.Count

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
                tmp_.Name = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_.Points = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_.Level = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
                tmp_.NoticeTitle = CStr(tmp.Tables(0).Rows(i).ItemArray(4))
                tmp_.Notice = CStr(tmp.Tables(0).Rows(i).ItemArray(5))

                Guilds.Add(tmp_)
            Next


            tmp = DataBase.GetDataSet("SELECT * From guild_member")
            Count = tmp.Tables(0).Rows.Count

            For i = 0 To Count - 1
                Dim tmp_ As New cGuild.GuildMember_
                tmp_.CharacterID = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                tmp_.GuildID = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_.DonantedGP = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_.GrantName = CStr(tmp.Tables(0).Rows(i).ItemArray(3))
                tmp_.Rights.Master = CBool(tmp.Tables(0).Rows(i).ItemArray(4))
                tmp_.Rights.Invite = CBool(tmp.Tables(0).Rows(i).ItemArray(5))
                tmp_.Rights.Kick = CBool(tmp.Tables(0).Rows(i).ItemArray(6))
                tmp_.Rights.Notice = CBool(tmp.Tables(0).Rows(i).ItemArray(7))
                tmp_.Rights.Union = CBool(tmp.Tables(0).Rows(i).ItemArray(8))
                tmp_.Rights.Storage = CBool(tmp.Tables(0).Rows(i).ItemArray(9))

                If tmp_.GuildID <> -1 Then
                    For g = 0 To Guilds.Count - 1
                        If Guilds(g).GuildID = tmp_.GuildID Then
                            Guilds(g).Member.Add(tmp_)
                            Exit For
                        End If
                    Next

                    For c = 0 To Chars.Count - 1
                        If Chars(c).CharacterId = tmp_.CharacterID Then
                            Chars(c).InGuild = True
                            Chars(c).GuildID = tmp_.GuildID
                            Exit For
                        End If
                    Next
                End If

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
        Public Function FillCharList(ByVal CharArray As cCharListing)

            Dim CharCount As Integer = 0
            CharArray.Chars.Clear()

            For i = 0 To Chars.Length - 1
                If Chars(i) IsNot Nothing Then
                    If CharArray.LoginInformation.Id = Chars(i).AccountID Then
                        CharArray.Chars.Add(Chars(i))
                        CharCount += 1
                    End If
                End If
            Next

            CharArray.NumberOfChars = CharCount
        End Function

        Public Function FillInventory(ByVal [char] As [cChar]) As cInventory
            Dim inventory As New cInventory([char].MaxSlots)
            For i = 0 To (AllItems.Length - 1)
                If i < AllItems.Length And AllItems(i) IsNot Nothing Then
                    If AllItems(i).OwnerCharID = [char].CharacterId Then
                        If AllItems(i).ItemType = cInvItem.sUserItemType.Inventory Then
                            If AllItems(i).Slot < [char].MaxSlots Then
                                inventory.UserItems(AllItems(i).Slot) = AllItems(i)
                            End If
                        ElseIf AllItems(i).ItemType = cInvItem.sUserItemType.Avatar Then
                            inventory.AvatarItems(AllItems(i).Slot) = AllItems(i)
                        End If
                    End If
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

        Public Function GetGuildWithGuildID(ByVal GuildID As UInteger) As cGuild
            For i = 0 To Guilds.Count - 1
                If Guilds(i).GuildID = GuildID Then
                    Return Guilds(i)
                End If
            Next
            Return Nothing
        End Function

        Public Function GetCharWithCharID(ByVal CharacterID As UInteger) As [cChar]
            For i = 0 To Chars.Length - 1
                If Chars(i).CharacterId = CharacterID Then
                    Return Chars(i)
                End If
            Next
            Return Nothing
        End Function

        Private Function GetItemTypeFromDb(ByVal itemstring As String) As cInvItem.sUserItemType
            If itemstring.StartsWith("item") Then
                Return cInvItem.sUserItemType.Inventory
            ElseIf itemstring.StartsWith("avatar") Then
                Return cInvItem.sUserItemType.Avatar
            End If
        End Function
#End Region

    End Module
End Namespace

'WhiteStat Indexes:
'PerDurability:8
'PerPhyRef:9
'PerMagRef:10
'PerPhyAtk:11
'PerMagAtk:12
'PerPhyDef:13
'PerMagDef:14
'PerBlock:15
'PerCritical:16
'PerAttackRate:17
'PerParryRate:18
'PerPhyAbs:19
'PerMagAbs:20