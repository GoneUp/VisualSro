Imports System.Timers
Imports GameServer.Functions
Imports SRFramework

Namespace GameDB
    Module GameDb
        'Timer
        Private WithEvents m_gameDbUpdate As New Timer

        'Chars
        Public Chars() As cCharacter
        Public Hotkeys As New List(Of cHotKey)

        'Itemcount
        Public Items As New Dictionary(Of UInt64, cItem)

        Public InventoryItems As New List(Of cInventoryItem)
        Public AvatarInventoryItems As New List(Of cInventoryItem)
        Public COSInventoryItems As New List(Of cInventoryItem)

        Public StorageItems As New List(Of cInventoryItem)
        Public GuildStorageItems As New List(Of cInventoryItem)

        'Masterys
        Public Masterys() As cMastery

        'Skills
        Public Skills() As cSkill
        Public SkillSets As New Dictionary(Of UInt32, cSkillSet)

        'Guilds
        Public Guilds As New List(Of cGuild)

        Public InitalLoad As Boolean = True

        Public Function LoadData() As Boolean Handles m_gameDbUpdate.Elapsed
            m_gameDbUpdate.Stop()
            m_gameDbUpdate.Interval = 20000

            Try
                If InitalLoad Then
                    Log.WriteSystemLog("Loading Playerdata from DB now.")
                    
                    GetCharData()

                    GetItemData()
                    GetInventoryData()
                    GetAvatarInventoryData()
                    GetCOSInventoryData()
                    GetStorageData()
                    GetGuildStorageData()

                    GetMasteryData()
                    GetSkillData()
                    GetSkillSetData()

                    GetPositionData()
                    GetHotkeyData()

                    GetGuildData()

                    InitalLoad = False

                    Log.WriteSystemLog("Inital Playerdata from Database loaded!")
                Else
                    'Any data need to reload?
                End If


            Catch ex As Exception
                Log.WriteSystemLog("[REFRESH ERROR][" & ex.Message & " Stack: " & ex.StackTrace & "]")
                Return False
            End Try

            m_gameDbUpdate.Start()
            Return True
        End Function

#Region "Get from DB"
#Region "Character"

        Private Sub GetCharData()

            Dim tmp As DataSet = Database.GetDataSet("SELECT * From characters")
            Dim charCount = tmp.Tables(0).Rows.Count

            If charCount = 0 Then
                ReDim Chars(0)
                Exit Sub
            End If

            ReDim Chars(charCount - 1)

            For i = 0 To Chars.Length - 1
                Chars(i) = New cCharacter
                Chars(i).PosTracker = New cPositionTracker(New Position, 0, 0, 0)

                Chars(i).CharacterId = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                Chars(i).AccountID = CUInt(tmp.Tables(0).Rows(i).ItemArray(1))
                Chars(i).CharacterName = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
                Chars(i).Pk2ID = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
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
                Chars(i).BerserkBar = CByte(tmp.Tables(0).Rows(i).ItemArray(35))
                Chars(i).PVP = CByte(tmp.Tables(0).Rows(i).ItemArray(36))
                Chars(i).MaxInvSlots = CByte(tmp.Tables(0).Rows(i).ItemArray(37))
                Chars(i).HelperIcon = CByte(tmp.Tables(0).Rows(i).ItemArray(38))
                Chars(i).PotHp = CByte(tmp.Tables(0).Rows(i).ItemArray(39))
                Chars(i).PotMp = CByte(tmp.Tables(0).Rows(i).ItemArray(40))
                Chars(i).PotAbormal = CByte(tmp.Tables(0).Rows(i).ItemArray(41))
                Chars(i).PotDelay = CByte(tmp.Tables(0).Rows(i).ItemArray(42))

                If Chars(i).CHP = 0 Then
                    Chars(i).Alive = False
                End If
            Next
        End Sub
#End Region


#Region "Item Stuff"

        Private Sub GetItemData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From items")
            Dim itemCount = tmp.Tables(0).Rows.Count
            Dim initalID As UInt64 = 2

            Items.Clear()

            For i = 0 To (itemCount - 1)
                Dim tmpItem As New cItem
                tmpItem.ID = Convert.ToUInt64(tmp.Tables(0).Rows(i).ItemArray(0))
                tmpItem.ObjectID = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(1))
                tmpItem.Plus = Convert.ToByte(tmp.Tables(0).Rows(i).ItemArray(2))
                tmpItem.Variance = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(3))
                tmpItem.Data = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(4))
                tmpItem.CreatorName = Convert.ToString(tmp.Tables(0).Rows(i).ItemArray(5))

                Dim blue1 As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(6))
                Dim blue1_amout As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(7))
                Dim blue2 As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(8))
                Dim blue2_amout As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(9))
                Dim blue3 As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(10))
                Dim blue3_amout As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(11))
                Dim blue4 As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(12))
                Dim blue4_amout As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(13))
                Dim blue5 As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(14))
                Dim blue5_amout As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(15))
                Dim blue6 As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(16))
                Dim blue6_amout As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(17))
                Dim blue7 As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(18))
                Dim blue7_amout As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(19))
                Dim blue8 As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(20))
                Dim blue8_amout As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(21))

                If blue1 <> 0 Then
                    tmpItem.Blues.Add(New cBluestat With {.Type = blue1, .Amout = blue1_amout})
                End If
                If blue2 <> 0 Then
                    tmpItem.Blues.Add(New cBluestat With {.Type = blue2, .Amout = blue2_amout})
                End If
                If blue3 <> 0 Then
                    tmpItem.Blues.Add(New cBluestat With {.Type = blue3, .Amout = blue3_amout})
                End If
                If blue4 <> 0 Then
                    tmpItem.Blues.Add(New cBluestat With {.Type = blue4, .Amout = blue4_amout})
                End If
                If blue5 <> 0 Then
                    tmpItem.Blues.Add(New cBluestat With {.Type = blue5, .Amout = blue5_amout})
                End If
                If blue6 <> 0 Then
                    tmpItem.Blues.Add(New cBluestat With {.Type = blue6, .Amout = blue6_amout})
                End If
                If blue7 <> 0 Then
                    tmpItem.Blues.Add(New cBluestat With {.Type = blue7, .Amout = blue7_amout})
                End If
                If blue8 <> 0 Then
                    tmpItem.Blues.Add(New cBluestat With {.Type = blue8, .Amout = blue8_amout})
                End If

                Items.Add(tmpItem.ID, tmpItem)

                If tmpItem.ID > initalID Then
                    initalID = tmpItem.ID
                End If
            Next

            Id_Gen.SetItemInitalValue(initalID)
        End Sub

        Private Sub GetInventoryData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From inventory")
            Dim itemCount = tmp.Tables(0).Rows.Count

            InventoryItems.Clear()

            For i = 0 To (itemCount - 1)
                Dim tmpItem As New cInventoryItem
                tmpItem.OwnerID = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(0))
                tmpItem.Slot = Convert.ToByte(tmp.Tables(0).Rows(i).ItemArray(1))
                tmpItem.ItemID = Convert.ToUInt64(tmp.Tables(0).Rows(i).ItemArray(2))
                InventoryItems.Add(tmpItem)
            Next
        End Sub

        Private Sub GetAvatarInventoryData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From inventory_avatar")
            Dim itemCount = tmp.Tables(0).Rows.Count

            AvatarInventoryItems.Clear()

            For i = 0 To (itemCount - 1)
                Dim tmpItem As New cInventoryItem
                tmpItem.OwnerID = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(0))
                tmpItem.Slot = Convert.ToByte(tmp.Tables(0).Rows(i).ItemArray(1))
                tmpItem.ItemID = Convert.ToUInt64(tmp.Tables(0).Rows(i).ItemArray(2))
                AvatarInventoryItems.Add(tmpItem)
            Next
        End Sub

        Private Sub GetCOSInventoryData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From inventory_cos")
            Dim itemCount = tmp.Tables(0).Rows.Count

            COSInventoryItems.Clear()

            For i = 0 To (itemCount - 1)
                Dim tmpItem As New cInventoryItem
                tmpItem.OwnerID = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(0))
                tmpItem.Slot = Convert.ToByte(tmp.Tables(0).Rows(i).ItemArray(1))
                tmpItem.ItemID = Convert.ToUInt64(tmp.Tables(0).Rows(i).ItemArray(2))
                COSInventoryItems.Add(tmpItem)
            Next
        End Sub

        Private Sub GetStorageData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From storage")
            Dim itemCount = tmp.Tables(0).Rows.Count

            StorageItems.Clear()

            For i = 0 To (itemCount - 1)
                Dim tmpItem As New cInventoryItem
                tmpItem.OwnerID = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(0))
                tmpItem.Slot = Convert.ToByte(tmp.Tables(0).Rows(i).ItemArray(1))
                tmpItem.ItemID = Convert.ToUInt64(tmp.Tables(0).Rows(i).ItemArray(2))
                StorageItems.Add(tmpItem)
            Next
        End Sub

        Private Sub GetGuildStorageData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From storage_guild")
            Dim itemCount = tmp.Tables(0).Rows.Count

            GuildStorageItems.Clear()

            For i = 0 To (itemCount - 1)
                Dim tmpItem As New cInventoryItem
                tmpItem.OwnerID = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(0))
                tmpItem.Slot = Convert.ToByte(tmp.Tables(0).Rows(i).ItemArray(1))
                tmpItem.ItemID = Convert.ToUInt64(tmp.Tables(0).Rows(i).ItemArray(2))
                GuildStorageItems.Add(tmpItem)
            Next
        End Sub
#End Region

#Region "Skills"

        Private Sub GetMasteryData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From char_mastery")
            Dim masteryCount = tmp.Tables(0).Rows.Count

            If masteryCount = 0 Then
                ReDim Masterys(0)
            End If

            ReDim Masterys(masteryCount - 1)

            For i = 0 To masteryCount - 1
                Masterys(i) = New cMastery
                Masterys(i).OwnerID = CUInt(tmp.Tables(0).Rows(i).ItemArray(1))
                Masterys(i).MasteryID = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                Masterys(i).Level = CByte(tmp.Tables(0).Rows(i).ItemArray(3))
            Next
        End Sub

        Private Sub GetSkillData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From char_skill")
            Dim count As Integer = tmp.Tables(0).Rows.Count

            If count = 0 Then
                ReDim Skills(0)
            End If

            ReDim Skills(count - 1)

            For i = 0 To count - 1
                Skills(i) = New cSkill
                Skills(i).OwnerID = CUInt(tmp.Tables(0).Rows(i).ItemArray(1))
                Skills(i).SkillID = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
            Next
        End Sub

        Private Sub GetSkillSetData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From skillset_name")
            Dim count As Integer = tmp.Tables(0).Rows.Count

            SkillSets.Clear()

            For i = 0 To count - 1
                Dim tmpSet As New cSkillSet
                tmpSet.SetID = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(0))
                tmpSet.Name = Convert.ToString(tmp.Tables(0).Rows(i).ItemArray(1))
                SkillSets.Add(tmpSet.SetID, tmpSet)
            Next

            tmp = Database.GetDataSet("SELECT * From skillset")
            count = tmp.Tables(0).Rows.Count

            For i = 0 To count - 1
                Dim setID As UInt32 = Convert.ToUInt32(tmp.Tables(0).Rows(i).ItemArray(0))
                Dim skillID = Convert.ToString(tmp.Tables(0).Rows(i).ItemArray(1))

                If SkillSets.ContainsKey(setID) Then
                    SkillSets(setID).Skills.Add(skillID)
                Else
                    Log.WriteSystemLog(setID & " not found!")
                End If
            Next
        End Sub
#End Region

#Region "Character MISC"

        Private Sub GetPositionData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From char_pos")
            Dim count As Integer = tmp.Tables(0).Rows.Count

            For i = 0 To count - 1
                Dim charID As UInteger = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                For c = 0 To Chars.Count - 1
                    If Chars(c).CharacterId = charID Then
                        Chars(c).PositionReturn.XSector = CByte(tmp.Tables(0).Rows(i).ItemArray(1))
                        Chars(c).PositionReturn.YSector = CByte(tmp.Tables(0).Rows(i).ItemArray(2))
                        Chars(c).PositionReturn.X = CDbl(tmp.Tables(0).Rows(i).ItemArray(3))
                        Chars(c).PositionReturn.Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(4))
                        Chars(c).PositionReturn.Z = CDbl(tmp.Tables(0).Rows(i).ItemArray(5))

                        Chars(c).PositionRecall.XSector = CByte(tmp.Tables(0).Rows(i).ItemArray(6))
                        Chars(c).PositionRecall.YSector = CByte(tmp.Tables(0).Rows(i).ItemArray(7))
                        Chars(c).PositionRecall.X = CDbl(tmp.Tables(0).Rows(i).ItemArray(8))
                        Chars(c).PositionRecall.Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(9))
                        Chars(c).PositionRecall.Z = CDbl(tmp.Tables(0).Rows(i).ItemArray(10))

                        Chars(c).PositionDead.XSector = CByte(tmp.Tables(0).Rows(i).ItemArray(11))
                        Chars(c).PositionDead.YSector = CByte(tmp.Tables(0).Rows(i).ItemArray(12))
                        Chars(c).PositionDead.X = CDbl(tmp.Tables(0).Rows(i).ItemArray(13))
                        Chars(c).PositionDead.Y = CDbl(tmp.Tables(0).Rows(i).ItemArray(14))
                        Chars(c).PositionDead.Z = CDbl(tmp.Tables(0).Rows(i).ItemArray(15))
                        Exit For
                    End If
                Next
            Next
        End Sub

        Private Sub GetHotkeyData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From hotkeys")
            Dim count As Integer = tmp.Tables(0).Rows.Count

            Hotkeys.Clear()

            For i = 0 To count - 1
                Dim tmp_ As New cHotKey
                tmp_.OwnerID = CUInt(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_.Slot = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_.Type = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
                tmp_.IconID = CUInt(tmp.Tables(0).Rows(i).ItemArray(4))

                Hotkeys.Add(tmp_)
            Next
        End Sub
#End Region

#Region "Guild"

        Private Sub GetGuildData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From guild")
            Dim count As Integer = tmp.Tables(0).Rows.Count

            Guilds.Clear()

            For i = 0 To count - 1
                Dim tmp_ As New cGuild
                tmp_.GuildID = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                tmp_.Name = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_.Points = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_.Level = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
                tmp_.NoticeTitle = CStr(tmp.Tables(0).Rows(i).ItemArray(4))
                tmp_.Notice = CStr(tmp.Tables(0).Rows(i).ItemArray(5))

                Guilds.Add(tmp_)
            Next


            tmp = Database.GetDataSet("SELECT * From guild_member")
            count = tmp.Tables(0).Rows.Count

            For i = 0 To count - 1
                Dim tmp_ As New cGuild.GuildMember
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

#End Region

#Region "Get Things from Array"
        Public Sub FillCharList(ByVal charArray As cCharListing)

            charArray.Chars.Clear()

            For i = 0 To Chars.Length - 1
                If Chars(i) IsNot Nothing Then
                    If charArray.LoginInformation.AccountID = Chars(i).AccountID Then
                        charArray.Chars.Add(Chars(i))
                    End If
                End If
            Next
        End Sub

        Public Sub UpdateChar(ByVal tmpChar As cCharacter)
            For i = 0 To Chars.Length - 1
                If Chars(i) IsNot Nothing Then
                    If Chars(i).CharacterId = tmpChar.CharacterId Then
                        Chars(i) = tmpChar
                    End If
                End If
            Next
        End Sub


        Public Function CheckNick(ByVal nick As String) As Boolean
            Dim free As Boolean = True
            For i = 0 To Chars.Length - 1
                If (Chars(i) IsNot Nothing) AndAlso (Chars(i).CharacterName = nick) Then
                    free = False
                End If
            Next
            Return free
        End Function

        Public Function GetGuild(ByVal guildID As UInteger) As cGuild
            For i = 0 To Guilds.Count - 1
                If Guilds(i).GuildID = GuildID Then
                    Return Guilds(i)
                End If
            Next
            Return Nothing
        End Function

        Public Function GetGuild(ByVal guildName As String) As cGuild
            For i = 0 To Guilds.Count - 1
                If Guilds(i).Name = GuildName Then
                    Return Guilds(i)
                End If
            Next
            Return Nothing
        End Function

        Public Function GetCharWithCharID(ByVal characterID As UInteger) As cCharacter
            For i = 0 To Chars.Length - 1
                If Chars(i).CharacterId = CharacterID Then
                    Return Chars(i)
                End If
            Next
            Return Nothing
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