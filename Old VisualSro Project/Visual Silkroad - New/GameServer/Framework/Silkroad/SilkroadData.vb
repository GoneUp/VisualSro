Imports GameServer.Functions
Imports System.IO

Module SilkroadData
    Public RefItems As New Dictionary(Of UInteger, cRefItem)
    Public RefGoldData As New List(Of cGoldData)
    Public RefLevelData As New List(Of LevelData)
    Public RefTmpSkills As New Dictionary(Of UInteger, RefSkill.tmpSkill)
    Public RefSkills As New Dictionary(Of UInteger, RefSkill)
    Public RefSkillGroups As New Dictionary(Of String, SkillGroup)
    Public RefObjects As New Dictionary(Of UInteger, SilkroadObject)
    Public RefPackageItems As New List(Of PackageItem)
    Public RefReversePoints As New List(Of ReversePoint_)
    Public RefTeleportPoints As New List(Of TeleportPoint_)
    Public RefSpecialZones As New List(Of SpecialSector_)
    Public RefRespawns As New List(Of ReSpawn_)
    Public RefRespawnsUnique As New List(Of ReSpawnUnique_)
    Public RefUniques As New List(Of UInteger)
    Public RefAbuseList As New List(Of String)
    Public RefCaveTeleporter As New List(Of CaveTeleporter_)
    Public RefSilkroadNameEntys As New Dictionary(Of String, String)
    Public RefShops As New Dictionary(Of String, Shop)
    Public RefShopGroups As New Dictionary(Of String, ShopGroup)
    Public RefShopTabGroups As New Dictionary(Of String, ShopTabGroup)

    Public base_path As String = AppDomain.CurrentDomain.BaseDirectory

    Public Sub DumpDataFiles()

        Try

            Dim time As Date = Date.Now

            DumpItemFiles()
            Log.WriteSystemLog("Loaded " & RefItems.Count & " Ref-Items.")

            DumpGoldData(base_path & "data\levelgold.txt")
            Log.WriteSystemLog("Loaded " & RefGoldData.Count & " Ref-Goldlevels.")

            DumpLevelData(base_path & "data\leveldata.txt")
            Log.WriteSystemLog("Loaded " & RefLevelData.Count & " Ref-Levels.")

            DumpNameFiles()
            Log.WriteSystemLog("Loaded " & RefSilkroadNameEntys.Count & " Name-Entry's.")

            DumpSkillFiles()
            Log.WriteSystemLog("Loaded " & RefSkills.Count & " Ref-Skills.")

            DumpUniqueFile(base_path & "\data\unique_ids.txt")
            Log.WriteSystemLog("Loaded " & RefUniques.Count & " Unique-Id's.")

            DumpObjectFiles()
            Log.WriteSystemLog("Loaded " & RefObjects.Count & " Ref-Objects.")

            DumpReversePoints(base_path & "data\reverse_points.txt")
            Log.WriteSystemLog("Loaded " & RefReversePoints.Count & " Reverse-Points.")

            DumpItemMall(base_path & "data\refpackageitem.txt", base_path & "data\refscrapofpackageitem.txt", base_path & "data\refpricepolicyofitem.txt")
            DumpItemMallNames()
            Log.WriteSystemLog("Loaded " & RefPackageItems.Count & " ItemMall-Items.")

            DumpTeleportBuildings(base_path & "data\teleportbuilding.txt")
            DumpTeleportData(base_path & "data\teleportdata.txt", base_path & "data\teleportlink.txt")
            DumpTelportLink(base_path & "data\teleportlink.txt")
            Log.WriteSystemLog("Loaded " & RefTeleportPoints.Count & " Teleport-Points.")

            DumpSpecialSectorFile(base_path & "\data\special_sectors.txt")
            Log.WriteSystemLog("Loaded " & RefSpecialZones.Count & " Special_Sectors.")

            DumpNpcChatFile(base_path & "\data\npcchatid.txt")
            Log.WriteSystemLog("Loaded NpcChat data.")

            DumpAbuseListFile(base_path & "\data\abuselist.txt")
            Log.WriteSystemLog("Loaded " & RefAbuseList.Count & "  Abuselist-Entry's.")

            DumpShopDataFile()
            Log.WriteSystemLog("Loaded Shop data.")

            DumpCaveTeleporterFile(base_path & "\data\cave_teleport.txt")
            Log.WriteSystemLog("Loaded " & RefCaveTeleporter.Count & " Cave-Teleporters.")

            'LoadAutoSpawn(base_path & "data\npcpos.txt")
            Log.WriteSystemLog("Loaded " & MobList.Count & " Autospawn Monster.")
            Log.WriteSystemLog("Loaded " & NpcList.Count & " Autospawn Npc's.")

            Log.WriteSystemLog("Loading took " & DateDiff(DateInterval.Second, time, Date.Now) & " Seconds.")

        Catch ex As Exception
            Log.WriteSystemLog("Error at Loading Data! Message: " & ex.Message & " Stack: " & ex.StackTrace)
        End Try
    End Sub

#Region "Items"
    Public Sub DumpItemFiles()
        RefItems.Clear()
        Dim paths As String() = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory & "data\itemdata.txt")
        For i As Integer = 0 To paths.Length - 1
            DumpItemFile(AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
        Next
    End Sub

    Public Sub DumpItemFile(ByVal path As String)


        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            If My.Computer.Info.OSFullName.Contains("x64") = False Then
                lines(i) = lines(i).Replace(".", ",")
            End If

            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

            Dim tmp As New cRefItem
            tmp.Pk2Id = Convert.ToUInt32(tmpString(1))
            tmp.ITEM_TYPE_NAME = tmpString(2)
            tmp.ITEM_MALL = Convert.ToByte(tmpString(7))
            tmp.CLASS_A = Convert.ToByte(tmpString(10))
            tmp.CLASS_B = Convert.ToByte(tmpString(11))
            tmp.CLASS_C = Convert.ToByte(tmpString(12))
            tmp.RACE = Convert.ToByte(tmpString(14))
            tmp.SHOP_PRICE = Convert.ToUInt64(tmpString(26))
            tmp.MIN_REPAIR = Convert.ToUInt16(tmpString(27))
            tmp.MAX_REPAIR = Convert.ToUInt16(tmpString(28))
            tmp.STORE_PRICE = Convert.ToUInt64(tmpString(30))
            tmp.SELL_PRICE = Convert.ToUInt64(tmpString(31))
            tmp.LV_REQ = Convert.ToByte(tmpString(33))
            tmp.REQ1 = Convert.ToInt32(tmpString(34))
            tmp.REQ1_LV = Convert.ToByte(tmpString(35))
            tmp.REQ2 = Convert.ToInt32(tmpString(36))
            tmp.REQ2_LV = Convert.ToByte(tmpString(37))
            tmp.REQ3 = Convert.ToInt32(tmpString(38))
            tmp.REQ3_LV = Convert.ToByte(tmpString(39))
            tmp.MAX_POSSES = Convert.ToInt32(tmpString(40))
            tmp.MAX_STACK = Convert.ToUInt16(tmpString(57))
            tmp.GENDER = Convert.ToByte(tmpString(58))

            tmp.MIN_DURA = Convert.ToSingle(tmpString(63))
            tmp.MAX_DURA = Convert.ToSingle(tmpString(64))

            tmp.MIN_PHYSDEF = Convert.ToDouble(tmpString(65))
            tmp.MAX_PHYSDEF = Convert.ToDouble(tmpString(66))
            tmp.PHYSDEF_INC = Convert.ToDouble(tmpString(67))

            tmp.MIN_PARRY = Convert.ToSingle(tmpString(68))
            tmp.MAX_PARRY = Convert.ToSingle(tmpString(69))

            tmp.MIN_PHYS_ABSORB = Convert.ToDouble(tmpString(71))
            tmp.MAX_PHYS_ABSORB = Convert.ToDouble(tmpString(72))
            tmp.PHYS_ABSORB_INC = Convert.ToDouble(tmpString(73))

            tmp.MIN_MAG_ABSORB = Convert.ToDouble(tmpString(79))
            tmp.MAX_MAG_ABSORB = Convert.ToDouble(tmpString(80))
            tmp.MAG_ABSORB_INC = Convert.ToDouble(tmpString(81))

            tmp.MIN_BLOCK = Convert.ToSingle(tmpString(74))
            tmp.MAX_BLOCK = Convert.ToSingle(tmpString(75))

            tmp.MIN_MAGDEF = Convert.ToDouble(tmpString(76))
            tmp.MAX_MAGDEF = Convert.ToDouble(tmpString(77))
            tmp.MAGDEF_INC = Convert.ToDouble(tmpString(78))

            tmp.MIN_PHYS_REINFORCE = Convert.ToSingle(tmpString(82))
            tmp.MAX_PHYS_REINFORCE = Convert.ToSingle(tmpString(83))
            tmp.MIN_MAG_REINFORCE = Convert.ToSingle(tmpString(84))
            tmp.MAX_MAG_REINFORCE = Convert.ToSingle(tmpString(85))

            tmp.ATTACK_DISTANCE = Convert.ToSingle(tmpString(94))

            tmp.MIN_FROM_PHYATK = Convert.ToSingle(tmpString(95))
            tmp.MAX_FROM_PHYATK = Convert.ToDouble(tmpString(96))
            tmp.MIN_TO_PHYATK = Convert.ToDouble(tmpString(97))
            tmp.MAX_TO_PHYATK = Convert.ToDouble(tmpString(98))
            tmp.PHYATK_INC = Convert.ToDouble(tmpString(99))

            tmp.MIN_FROM_MAGATK = Convert.ToDouble(tmpString(100))
            tmp.MAX_FROM_MAGATK = Convert.ToDouble(tmpString(101))
            tmp.MIN_TO_MAGATK = Convert.ToDouble(tmpString(102))
            tmp.MAX_TO_MAGATK = Convert.ToDouble(tmpString(103))
            tmp.MAGATK_INC = Convert.ToDouble(tmpString(104))

            tmp.MIN_FROM_PHYS_REINFORCE = Convert.ToSingle(tmpString(105))
            tmp.MAX_FROM_PHYS_REINFORCE = Convert.ToSingle(tmpString(106))
            tmp.MIN_TO_PHYS_REINFORCE = Convert.ToSingle(tmpString(107))
            tmp.MAX_TO_PHYS_REINFORCE = Convert.ToSingle(tmpString(108))
            tmp.MIN_FROM_MAG_REINFORCE = Convert.ToSingle(tmpString(109))
            tmp.MAX_FROM_MAG_REINFORCE = Convert.ToSingle(tmpString(110))
            tmp.MIN_TO_MAG_REINFORCE = Convert.ToSingle(tmpString(111))
            tmp.MAX_TO_MAG_REINFORCE = Convert.ToSingle(tmpString(112))

            tmp.MIN_ATTACK_RATING = Convert.ToSingle(tmpString(113))
            tmp.MAX_ATTACK_RATING = Convert.ToSingle(tmpString(114))

            tmp.MIN_CRITICAL = Convert.ToSingle(tmpString(116))
            tmp.MAX_CRITICAL = Convert.ToSingle(tmpString(117))

            tmp.USE_TIME_HP = Convert.ToInt32(tmpString(118))
            tmp.USE_TIME_HP_PER = Convert.ToInt32(tmpString(120))
            tmp.USE_TIME_MP = Convert.ToInt32(tmpString(122))
            tmp.USE_TIME_MP_PER = Convert.ToInt32(tmpString(124))
            RefItems.Add(tmp.Pk2Id, tmp)
        Next
    End Sub

    Public Function GetItemByID(ByVal id As UInteger) As cRefItem
        If RefItems.ContainsKey(id) Then
            Return RefItems(id)
        End If
        Throw New Exception("Item couldn't be found!")
    End Function

    Public Function GetItemByName(ByVal Name As String) As cRefItem
        For Each key In RefItems.Keys.ToList
            If RefItems.ContainsKey(key) Then
                If RefItems(key).ITEM_TYPE_NAME = Name Then
                    Return RefItems(key)
                End If
            End If
        Next
        Throw New Exception("Item couldn't be found!")
    End Function
#End Region

#Region "Gold"
    Structure cGoldData
        Public Level As Byte
        Public MinGold As ULong
        Public MaxGold As ULong
    End Structure


    Public Sub DumpGoldData(ByVal path As String)
        RefGoldData.Clear()

        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim gold As New cGoldData
            gold.Level = CByte(tmpString(0))
            gold.MinGold = CULng(tmpString(1))
            gold.MaxGold = CULng(tmpString(2))
            RefGoldData.Add(gold)
        Next
    End Sub

    Public Function GetGoldData(ByVal level As Byte) As cGoldData
        For i = 0 To RefGoldData.Count - 1
            If RefGoldData(i).Level = level Then
                Return RefGoldData(i)
            End If
        Next
        Throw New Exception("Level couldn't be found!")
    End Function
#End Region

#Region "LevelData"
    Structure LevelData
        Public Level As Byte
        Public MobEXP As ULong
        Public Experience As ULong
        Public SkillPoints As ULong
    End Structure


    Public Sub DumpLevelData(ByVal path As String)
        RefLevelData.Clear()

        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1

            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim level As New LevelData
            level.Level = tmpString(0)
            level.MobEXP = tmpString(1)
            level.Experience = tmpString(2)
            If level.Level = 1 Then
                level.SkillPoints = 0
            Else
                level.SkillPoints = tmpString(3)
            End If

            RefLevelData.Add(level)
        Next
    End Sub

    Public Function GetLevelData(ByVal level As Byte) As LevelData
        For i = 0 To RefLevelData.Count - 1
            If RefLevelData(i).Level = level Then
                Return RefLevelData(i)
            End If
        Next
    End Function
#End Region

#Region "Skill"
    Public Class RefSkill
        Public Name As String
        Public Pk2Id As UInteger
        Public PreviousId As UInteger
        Public NextId As UInteger
        Public MasteryID As UInteger
        Public MasteryLevel As Byte
        Public RequiredSp As ULong
        Public RequiredMp As UShort
        Public CastTime As Integer
        Public UseDuration As UInteger
        Public PwrPercent As Integer
        Public PwrMin As Integer
        Public PwrMax As Integer
        Public Distance As Integer
        Public NumberOfAttacks As Byte
        Public Type As Byte
        Public Type2 As Byte

        Public SkillGroupID As UInt32
        Public SkillGroupName As String
        Public SkillGroupLevel As Byte

        Public SpawnPercent As Integer

        Public Effect_0 As String
        Public Effect_1 As Long
        Public Effect_2 As Long
        Public Effect_3 As Long
        Public Effect_4 As Long
        Public Effect_5 As Long
        Public Effect_6 As Long
        Public Effect_7 As Long
        Public Effect_8 As Long
        Public Effect_9 As Long
        Public Effect_10 As Long
        Public Effect_11 As Long
        Public Effect_12 As Long
        Public Effect_13 As Long
        Public Effect_14 As Long
        Public Effect_15 As Long
        Public Effect_16 As Long

        Public Structure tmpSkill
            Public Pk2Id As UInteger
            Public NextId As UInteger
        End Structure
    End Class

    Public Class SkillGroup
        Private m_ID As UInt32 = 0
        Public Property ID As UInt32
            Get
                Return m_ID
            End Get
            Set(ByVal value As UInt32)
                m_ID = value
            End Set
        End Property

        Private m_Name As String = ""
        Public Property Name As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
            End Set
        End Property

        Private m_Skills As New Dictionary(Of Byte, UInt32) 'Key = Skill Series Level, Value = SkillID
        Public Property Skills As Dictionary(Of Byte, UInt32)
            Get
                Return m_Skills
            End Get
            Set(ByVal value As Dictionary(Of Byte, UInt32))
                m_Skills = value
            End Set
        End Property
    End Class


    Public Class SkillTypeTable
        Public Const Phy As Byte = &H1,
                     Mag As Byte = &H2,
                     Bicheon As Byte = &H3,
                     Heuksal As Byte = &H4,
                     Bow As Byte = &H5,
                     All As Byte = &H6
    End Class

    Public Structure SkillEffect
        Public EffectId As String

        Public Effect_1 As Long
        Public Effect_2 As Long
        Public Effect_3 As Long
        Public Effect_4 As Long
        Public Effect_5 As Long
        Public Effect_6 As Long
        Public Effect_7 As Long
        Public Effect_8 As Long
        Public Effect_9 As Long
        Public Effect_10 As Long
        Public Effect_11 As Long
        Public Effect_12 As Long
        Public Effect_13 As Long
        Public Effect_14 As Long
        Public Effect_15 As Long
        Public Effect_16 As Long
    End Structure

    Public Sub DumpSkillFiles()
        RefSkills.Clear()
        RefTmpSkills.Clear()

        Dim paths As String() = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory & "data\skilldata.txt")
        For i As Integer = 0 To paths.Length - 1
            DumpTmpSkillFile(AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
            DumpSkillFile(AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
        Next
    End Sub

    Public Sub DumpTmpSkillFile(ByVal path As String)

        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim tmp As New RefSkill.tmpSkill()
            tmp.Pk2Id = Convert.ToUInt32(tmpString(1))
            tmp.NextId = Convert.ToUInt32(tmpString(9))
            RefTmpSkills.Add(tmp.Pk2Id, tmp)
        Next
    End Sub

    Private Sub DumpSkillFile(ByVal path As String)
        Dim lines As String() = File.ReadAllLines(path)

        For i As Integer = 0 To lines.Length - 1
            If My.Computer.Info.OSFullName.Contains("x64") = False Then
                lines(i) = lines(i).Replace(".", ",")
            End If

            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim tmp As New RefSkill()
            tmp.Pk2Id = Convert.ToUInt32(tmpString(1))
            tmp.SkillGroupID = Convert.ToUInt32(tmpString(2))
            tmp.Name = tmpString(3)
            tmp.SkillGroupName = tmpString(5)
            tmp.SkillGroupLevel = Convert.ToByte(tmpString(7))
            tmp.NextId = Convert.ToUInt32(tmpString(9))
            tmp.CastTime = Convert.ToInt32(tmpString(13))
            tmp.MasteryID = Convert.ToUInt32(tmpString(34))
            tmp.MasteryLevel = Convert.ToUInt32(tmpString(36))
            tmp.RequiredSp = Convert.ToUInt64(tmpString(46))
            tmp.RequiredMp = Convert.ToUInt16(tmpString(53))
            tmp.SpawnPercent = Convert.ToInt32(tmpString(66))
            tmp.UseDuration = Convert.ToInt32(tmpString(70))
            tmp.PwrPercent = Convert.ToInt32(tmpString(71))
            tmp.PwrMin = Convert.ToInt32(tmpString(72))
            tmp.PwrMax = Convert.ToInt32(tmpString(73))
            tmp.Effect_0 = HexToString(Hex(tmpString(69)))
            tmp.Effect_1 = Convert.ToInt32(tmpString(70))
            tmp.Effect_2 = Convert.ToInt32(tmpString(71))
            tmp.Effect_3 = Convert.ToInt32(tmpString(72))
            tmp.Effect_4 = Convert.ToInt32(tmpString(73))
            tmp.Effect_5 = Convert.ToInt32(tmpString(74))
            tmp.Effect_6 = Convert.ToInt32(tmpString(75))
            tmp.Effect_7 = Convert.ToInt32(tmpString(76))
            tmp.Effect_8 = Convert.ToInt32(tmpString(77))
            tmp.Effect_9 = Convert.ToInt32(tmpString(78))
            tmp.Effect_10 = Convert.ToInt32(tmpString(79))
            tmp.Effect_11 = Convert.ToInt32(tmpString(80))
            tmp.Effect_12 = Convert.ToInt32(tmpString(81))
            tmp.Effect_13 = Convert.ToInt32(tmpString(82))
            tmp.Effect_14 = Convert.ToInt32(tmpString(83))
            tmp.Effect_15 = Convert.ToInt32(tmpString(84))
            tmp.Effect_16 = Convert.ToInt32(tmpString(85))


            If tmp.Effect_0 = "att" Then
                tmp.Distance = 21
            End If

            If tmpString(3).Contains("SWORD") Then
                tmp.Type = SkillTypeTable.Phy
                tmp.Type2 = SkillTypeTable.Bicheon
            End If
            If tmpString(3).Contains("SPEAR") Then
                tmp.Type = SkillTypeTable.Phy
                tmp.Type2 = SkillTypeTable.Heuksal
            End If
            If tmpString(3).Contains("BOW") Then
                tmp.Type = SkillTypeTable.Phy
                tmp.Type2 = SkillTypeTable.Bow
            End If
            If _
                tmpString(3).Contains("FIRE") OrElse tmpString(3).Contains("LIGHTNING") OrElse
                tmpString(3).Contains("COLD") OrElse tmpString(3).Contains("WATER") Then
                tmp.Type = SkillTypeTable.Mag
                tmp.Type2 = SkillTypeTable.All
            End If
            If tmpString(3).Contains("PUNCH") Then
                tmp.Type = SkillTypeTable.Phy
                tmp.Type2 = SkillTypeTable.All
            End If
            If _
                tmpString(3).Contains("ROG") OrElse tmpString(3).Contains("WARRIOR") OrElse
                tmpString(3).Contains("AXE") Then
                tmp.Type = SkillTypeTable.Phy
                tmp.Type2 = SkillTypeTable.All
            End If
            If _
                tmpString(3).Contains("WIZARD") OrElse tmpString(3).Contains("STAFF") OrElse
                tmpString(3).Contains("WARLOCK") OrElse tmpString(3).Contains("BARD") OrElse
                tmpString(3).Contains("HARP") OrElse tmpString(3).Contains("CLERIC") Then
                tmp.Type = SkillTypeTable.Mag
                tmp.Type2 = SkillTypeTable.All
            End If
            If tmpString(3).StartsWith("MSKILL") Then
                tmp.Type = SkillTypeTable.Phy
                tmp.Type2 = SkillTypeTable.All
            End If


            RefSkills.Add(tmp.Pk2Id, tmp)

            If RefSkillGroups.ContainsKey(tmp.SkillGroupName) Then
                If RefSkillGroups(tmp.SkillGroupName).Skills.ContainsKey(tmp.SkillGroupLevel) = False Then
                    RefSkillGroups(tmp.SkillGroupName).Skills.Add(tmp.SkillGroupLevel, tmp.Pk2Id)
                Else
                    Debug.Print(0)
                End If

            Else
                Dim tmpGroup As New SkillGroup
                tmpGroup.ID = tmp.SkillGroupID
                tmpGroup.Name = tmp.SkillGroupName
                tmpGroup.Skills.Add(tmp.SkillGroupLevel, tmp.Pk2Id)

                RefSkillGroups.Add(tmpGroup.Name, tmpGroup)
            End If
        Next

        For Each key In RefSkills.Keys.ToList
            If RefSkills.ContainsKey(key) Then
                RefSkills(key).NumberOfAttacks = GetSkillNumberOfAttacks(GetTmpSkill(RefSkills(key).Pk2Id))
                'RefSkills(key).PreviousId = GetSkillPreviosId(key)
            End If
        Next
    End Sub

    Private Function GetSkillNumberOfAttacks(ByVal tmp As RefSkill.tmpSkill) As Byte
        For i As Byte = 0 To 9
            If tmp.NextId <> 0 Then
                tmp = GetTmpSkill(tmp.NextId)
            Else
                Return CByte(i + 1)
            End If
        Next
        Return 1
    End Function

    Private Function GetTmpSkill(ByVal NextId As UInteger) As RefSkill.tmpSkill
        If RefTmpSkills.ContainsKey(NextId) Then
            Return RefTmpSkills(NextId)
        Else
            Return Nothing
        End If
    End Function

    Private Function GetSkillPreviosId(ByVal pk2id As UInteger) As UInteger
        For Each key In RefSkills.Keys.ToList
            If RefSkills(key).NextId = pk2id Then
                Return RefSkills(key).Pk2Id
            End If
        Next
        Return 0
    End Function

    Public Function GetSkill(ByVal Pk2Id As UInteger) As RefSkill
        If RefSkills.ContainsKey(Pk2Id) Then
            Return RefSkills(Pk2Id)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetSkillGroup(ByVal SkillGroupName As String) As SkillGroup
        If RefSkillGroups.ContainsKey(SkillGroupName) Then
            Return RefSkillGroups(SkillGroupName)
        End If
        Return Nothing
    End Function


    Private Function ParseSkillEffects(ByVal tmpEffectList() As Long) As List(Of SkillEffect)
        'Note: use the effect system, so any skill can use any effect (more coustumable, not so static)
        'case 1: use teh common system with Effect_1, you not knwo what is behind
        'case 2: create types (or smth similar) that represent each possible (under)effect


        Dim tmpList As New List(Of SkillEffect)
        Dim counter As Integer = 0

        Do While True
            Dim tmpEffect As New SkillEffect
            tmpEffect.EffectId = HexToString(Hex(tmpEffectList(counter)))

            Select Case tmpEffect.EffectId
                Case "att"

            End Select
        Loop
    End Function
#End Region

#Region "Objects"
    Public Class SilkroadObject
        Public Pk2ID As UInteger

        Public TypeName As String
        Public InternalName As String
        Public RealName As String

        Public Type As Type_
        Public WalkSpeed As Single
        Public RunSpeed As Single
        Public BerserkSpeed As Single
        Public Level As Byte
        Public Hp As UInteger
        Public InvSize As Byte

        Public PhyDef As UShort
        Public MagDef As UShort
        Public HitRatio As Byte
        Public ParryRatio As Byte

        Public Exp As ULong
        Public Skill1 As UInteger
        Public Skill2 As UInteger
        Public Skill3 As UInteger
        Public Skill4 As UInteger
        Public Skill5 As UInteger
        Public Skill6 As UInteger
        Public Skill7 As UInteger
        Public Skill8 As UInteger
        Public Skill9 As UInteger
        Public ChatBytes() As Byte
        'For the NPC Chat

        Public Shop As ShopData_

        'These Fileds are for Teleports
        Public T_Position As Position

        Enum Type_
            Mob_Normal = 0
            Npc = 1
            Teleport = 2
            [Structure] = 3
            Mob_Cave = 4
            COS = 5
            Mob_Unique = 6
            Mob_Quest = 7
            Character = 8
            Trade = 9
            MovePet = 10
        End Enum
    End Class

    Public Sub DumpObjectFiles()
        RefObjects.Clear()

        Dim paths As String() = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory & "data\characterdata.txt")
        For i As Integer = 0 To paths.Length - 1
            DumpObjectFile(AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
        Next
    End Sub

    Private Sub DumpObjectFile(ByVal path As String)
        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            If My.Computer.Info.OSFullName.Contains("x64") = False Then
                lines(i) = lines(i).Replace(".", ",")
            End If

            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim tmp As New SilkroadObject()
            tmp.Pk2ID = Convert.ToUInt32(tmpString(1))
            tmp.TypeName = tmpString(2)
            tmp.InternalName = tmpString(5)
            Dim tmpRarity As Int32 = tmpString(15)
            tmp.WalkSpeed = Convert.ToSingle(tmpString(46))
            tmp.RunSpeed = Convert.ToSingle(tmpString(47))
            tmp.BerserkSpeed = Convert.ToSingle(tmpString(48))
            tmp.Level = Convert.ToByte(tmpString(57))
            tmp.Hp = Convert.ToUInt32(tmpString(59))
            tmp.InvSize = 0
            tmp.PhyDef = Convert.ToUInt16(tmpString(71))
            tmp.MagDef = Convert.ToUInt16(tmpString(72))
            tmp.HitRatio = Convert.ToByte(tmpString(75))
            tmp.ParryRatio = Convert.ToByte(tmpString(77))
            tmp.Exp = Convert.ToUInt64(tmpString(79))
            tmp.Skill1 = Convert.ToUInt32(tmpString(83))
            tmp.Skill2 = Convert.ToUInt32(tmpString(85))
            tmp.Skill3 = Convert.ToUInt32(tmpString(86))
            tmp.Skill4 = Convert.ToUInt32(tmpString(87))
            tmp.Skill5 = Convert.ToUInt32(tmpString(88))
            tmp.Skill6 = Convert.ToUInt32(tmpString(89))
            tmp.Skill7 = Convert.ToUInt32(tmpString(90))
            tmp.Skill8 = Convert.ToUInt32(tmpString(91))
            tmp.Skill9 = Convert.ToUInt32(tmpString(92))


            Dim selector As String() = tmp.TypeName.Split("_")
            Select Case selector(0)
                Case "MOB"
                    If selector(1) = "TQ" Or selector(1) = "DH" Then
                        tmp.Type = SilkroadObject.Type_.Mob_Cave

                    ElseIf selector(1) = "QT" Then
                        tmp.Type = SilkroadObject.Type_.Mob_Quest

                    ElseIf IsUnique(tmp.Pk2ID) Or tmpRarity = 3 Then
                        tmp.Type = SilkroadObject.Type_.Mob_Unique
                    Else
                        tmp.Type = SilkroadObject.Type_.Mob_Normal
                    End If

                Case "NPC"
                    tmp.Type = SilkroadObject.Type_.Npc
                Case "STORE"
                    tmp.Type = SilkroadObject.Type_.Teleport
                Case "STRUCTURE"
                    tmp.Type = SilkroadObject.Type_.Structure
                Case "COS"
                    tmp.Type = SilkroadObject.Type_.COS
                Case "MOV"
                    tmp.Type = SilkroadObject.Type_.MovePet
                Case "CHAR"
                    tmp.Type = SilkroadObject.Type_.Character
                Case "TRADE"
                    tmp.Type = SilkroadObject.Type_.Trade
                Case Else
                    Log.WriteSystemLog("LOADOBJ::Case Else: " & tmp.TypeName)
            End Select

            RefObjects.Add(tmp.Pk2ID, tmp)
        Next
    End Sub

    Public Function GetObject(ByVal Pk2Id As UInteger) As SilkroadObject
        If RefObjects.ContainsKey(Pk2Id) Then
            Return RefObjects(Pk2Id)
        Else
            Return Nothing
        End If
    End Function
#End Region

#Region "Reverse Points"
    Public Class ReversePoint_
        Public TeleportID As UInteger
        Public Position As New Position
    End Class

    Public Sub DumpReversePoints(ByVal path As String)
        RefReversePoints.Clear()

        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim tmp As New ReversePoint_
            tmp.TeleportID = tmpString(0)
            tmp.Position.XSector = tmpString(1)
            tmp.Position.YSector = tmpString(2)
            tmp.Position.X = tmpString(3)
            tmp.Position.Z = tmpString(4)
            tmp.Position.Y = tmpString(5)
            RefReversePoints.Add(tmp)
        Next
    End Sub

    Public Function GetReversePoint(ByVal id As UInteger) As ReversePoint_
        For i = 0 To RefReversePoints.Count - 1
            If RefReversePoints(i).TeleportID = id Then
                Return RefReversePoints(i)
            End If
        Next
        Return Nothing
    End Function
#End Region

#Region "Mall"
    Public Class PackageItem
        Public Code_Name As String = ""
        Public Package_Name As String = ""

        Public Name_Real_Code As String = ""
        Public Name_Real As String = ""
        Public Description_Code As String = ""
        Public Description As String = ""

        Public Data As UInt32 = 0
        Public Variance As UInt64 = 0
        Public Payments As New Dictionary(Of UShort, MallPaymentEntry) 'Key = PaymentDevice

        Public InShop As Boolean = True
        Public Shop As String = ""
    End Class

    Public Class MallPaymentEntry
        Public PaymentDevice As PaymentDevices
        Public Price As UInt32

        Public Enum PaymentDevices As UShort
            Normal = 1
            Mall = 2
            Mall_Point = 4
            Mall_Bonus = 16
        End Enum
    End Class


    Public Sub DumpItemMall(ByVal filePackagePath As String, ByVal fileScrapPath As String, ByVal filePricePath As String)
        RefPackageItems.Clear()

        Dim lines As String() = File.ReadAllLines(filePackagePath)
        For i As Integer = 0 To lines.Length - 1
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim tmp As New PackageItem
            tmp.Package_Name = tmpString(3)
            tmp.Name_Real_Code = tmpString(6)
            tmp.Description_Code = tmpString(7)

            RefPackageItems.Add(tmp)
        Next

        Dim itemScrapFile As String() = File.ReadAllLines(fileScrapPath)
        For d As Integer = 0 To itemScrapFile.Length - 1
            Dim tmpString As String() = itemScrapFile(d).Split(ControlChars.Tab)
            For i = 0 To RefPackageItems.Count - 1
                If RefPackageItems(i).Package_Name = tmpString(2) Then
                    RefPackageItems(i).Code_Name = tmpString(3)
                    RefPackageItems(i).Data = tmpString(6)
                    RefPackageItems(i).Variance = tmpString(8)
                    Exit For
                End If
            Next
        Next


        Dim ItemPriceFile As String() = File.ReadAllLines(filePricePath)
        For d As Integer = 0 To ItemPriceFile.Length - 1
            Dim tmpString As String() = ItemPriceFile(d).Split(ControlChars.Tab)
            For i = 0 To RefPackageItems.Count - 1
                If RefPackageItems(i).Package_Name = tmpString(2) Then
                    Dim tmp As New MallPaymentEntry
                    tmp.PaymentDevice = tmpString(3)
                    tmp.Price = tmpString(5)
                    RefPackageItems(i).Payments.Add(tmp.PaymentDevice, tmp)
                    Exit For
                End If
            Next
        Next
    End Sub

    Public Sub DumpItemMallNames()
        For i = 0 To RefPackageItems.Count - 1
            If RefSilkroadNameEntys.ContainsKey(RefPackageItems(i).Name_Real_Code) Then
                RefPackageItems(i).Name_Real = RefSilkroadNameEntys(RefPackageItems(i).Name_Real_Code)
            End If

            If RefSilkroadNameEntys.ContainsKey(RefPackageItems(i).Description_Code) Then
                RefPackageItems(i).Description = RefSilkroadNameEntys(RefPackageItems(i).Description_Code)
            End If
        Next
    End Sub

    Public Function GetPackageItem(ByVal code_Name As String) As PackageItem
        For i = 0 To RefPackageItems.Count - 1
            If RefPackageItems(i).Package_Name = code_Name Then
                Return RefPackageItems(i)
            End If
        Next
        Return Nothing
    End Function
#End Region

#Region "Teleport"
    Public Sub DumpTeleportBuildings(ByVal path As String)
        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim obj As New SilkroadObject

            obj.Pk2ID = tmpString(1)
            obj.TypeName = tmpString(2)
            obj.Type = SilkroadObject.Type_.Teleport

            Dim area As Integer = tmpString(41)
            obj.T_Position = New Position
            obj.T_Position.XSector = Convert.ToByte((area).ToString("X4").Substring(2, 2), 16)
            obj.T_Position.YSector = Convert.ToByte((area).ToString("X4").Substring(0, 2), 16)

            obj.T_Position.X = tmpString(43)
            obj.T_Position.Z = tmpString(44)
            obj.T_Position.Y = tmpString(45)

            RefObjects.Add(obj.Pk2ID, obj)

            SpawnNPC(obj.Pk2ID, obj.T_Position, 0)
        Next
    End Sub

    Class TeleportPoint_
        Public TeleportNumber As UInt32
        Public TypeName As String
        Public Pk2ID As UInteger
        Public ToPos As Position
        Public Links As Dictionary(Of UInt32, TeleportLink)
    End Class

    Public Sub DumpTeleportData(ByVal Path_Data As String, ByVal Path_Link As String)
        RefTeleportPoints.Clear()

        Dim lines As String() = File.ReadAllLines(Path_Data)
        Try
            For i As Integer = 0 To lines.Length - 1
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim obj As New TeleportPoint_

                obj.TeleportNumber = tmpString(1)
                obj.TypeName = tmpString(2)
                obj.Pk2ID = tmpString(3)

                Dim area As Integer = tmpString(5)
                If area < 0 Then
                    area *= -1
                End If

                obj.ToPos = New Position
                obj.ToPos.XSector = Convert.ToByte((area).ToString("X4").Substring(2, 2), 16)
                obj.ToPos.YSector = Convert.ToByte((area).ToString("X4").Substring(0, 2), 16)

                obj.ToPos.X = tmpString(6)
                obj.ToPos.Z = tmpString(7)
                obj.ToPos.Y = tmpString(8)

                obj.Links = New Dictionary(Of UInteger, TeleportLink)

                RefTeleportPoints.Add(obj)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Structure TeleportLink
        Public FromPoint As UInt32
        Public ToPoint As UInt32
        Public Cost As UInteger
        Public MinLevel As UInteger
        Public MaxLevel As UInteger
    End Structure

    Public Sub DumpTelportLink(ByVal path As String)
        Dim lines As String() = File.ReadAllLines(path)
        For i = 0 To lines.Length - 1
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

            Dim fromPoint As TeleportPoint_ = GetTeleportPointByNumber(tmpString(1))

            If fromPoint IsNot Nothing Then
                Dim obj As New TeleportLink
                obj.FromPoint = tmpString(1)
                obj.ToPoint = tmpString(2)
                obj.Cost = tmpString(3)
                obj.MinLevel = tmpString(7)
                obj.MaxLevel = tmpString(8)

                fromPoint.Links.Add(obj.ToPoint, obj)
            End If
        Next
    End Sub

    Public Function GetTeleportPointByNumber(ByVal number As Integer) As TeleportPoint_
        For i = 0 To RefTeleportPoints.Count - 1
            If RefTeleportPoints(i).TeleportNumber = number Then
                Return RefTeleportPoints(i)
            End If
        Next
        Return Nothing
    End Function

    Public Function GetTeleportPoint(ByVal pk2Id As UInteger) As TeleportPoint_
        For i = 0 To RefTeleportPoints.Count - 1
            If RefTeleportPoints(i).Pk2ID = pk2Id Then
                Return RefTeleportPoints(i)
            End If
        Next
        Return Nothing
    End Function
#End Region

#Region "SpecialSectors"
    Public Structure SpecialSector_
        Public Type As SpecialSector_Types
        Public XSec As Byte
        Public YSec As Byte
    End Structure

    Enum SpecialSector_Types
        Safe_Zone = 1
        Cave_Zone = 2
    End Enum

    Public Sub DumpSpecialSectorFile(ByVal path As String)
        RefSpecialZones.Clear()

        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False Then
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim tmp As New SpecialSector_

                tmp.XSec = tmpString(0)
                tmp.YSec = tmpString(1)
                tmp.Type = tmpString(2)

                RefSpecialZones.Add(tmp)
            End If
        Next i
    End Sub

    Public Function IsInSaveZone(ByVal Pos As Position) As Boolean
        For i = 0 To RefSpecialZones.Count - 1
            If _
                RefSpecialZones(i).XSec = Pos.XSector And RefSpecialZones(i).YSec = Pos.YSector And
                RefSpecialZones(i).Type = SpecialSector_Types.Safe_Zone Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function IsInCave(ByVal Pos As Position) As Boolean
        For i = 0 To RefSpecialZones.Count - 1
            If _
                RefSpecialZones(i).XSec = Pos.XSector And RefSpecialZones(i).YSec = Pos.YSector And
                RefSpecialZones(i).Type = SpecialSector_Types.Cave_Zone Then
                Return True
            End If
        Next
        Return False
    End Function
#End Region

#Region "Unique"
    Public Sub DumpUniqueFile(ByVal path As String)
        RefUniques.Clear()

        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) = "" = False Then
                RefUniques.Add(lines(i))
            End If
        Next i
    End Sub

    Public Function IsUnique(ByVal Pk2ID As UInteger) As Boolean
        For i = 0 To RefUniques.Count - 1
            If RefUniques(i) = Pk2ID Then
                Return True
            End If
        Next
        Return False
    End Function
#End Region

#Region "NPC/Shop"
    Public Sub DumpNpcChatFile(ByVal path As String)
        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) = "" = False Then

                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim obj As SilkroadObject = GetObject(tmpString(1))
                If obj IsNot Nothing Then
                    ReDim obj.ChatBytes(tmpString.Length - 3)

                    For c = 0 To obj.ChatBytes.Count - 1
                        obj.ChatBytes(c) = tmpString(c + 2)
                    Next
                Else
                    'Obj not found
                End If
            End If
        Next
    End Sub



    Public Sub DumpShopDataFile()
        'refshop.txt --> refmappingshopwithtab.txt --> refshoptab.txt --> refshopgoods.txt
        'refshopgroup --> refmappingshopgroup.txt

        '1	19	1976	STORE_CH_SMITH
        Dim lines As String() = File.ReadAllLines(base_path & "data\refshop.txt")
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" Then
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim tmp As New Shop
                tmp.Name = tmpString(3)

                RefShops.Add(tmp.Name, tmp)
            End If
        Next
        Log.WriteSystemLog("refshop")


        '1	19	1898	GROUP_STORE_CH_SMITH	NPC_CH_SMITH
        lines = File.ReadAllLines(base_path & "data\refshopgroup.txt")
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" Then
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

                Dim tmp As New ShopGroup
                tmp.Group_Name = tmpString(3)
                tmp.Object_Code_Name = tmpString(4)

                RefShopGroups.Add(tmp.Group_Name, tmp)
            End If
        Next
        Log.WriteSystemLog("refshopgroup")


        'Map group with Stores
        '1	19	GROUP_STORE_CH_SMITH	STORE_CH_SMITH
        lines = File.ReadAllLines(base_path & "data\refmappingshopgroup.txt")
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" Then
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

                Dim groupName As String = tmpString(2)
                Dim storeName As String = tmpString(3)

                If RefShopGroups.ContainsKey(groupName) Then
                    RefShopGroups(groupName).Store_Name = storeName
                End If
            End If
        Next
        Log.WriteSystemLog("refmappingshopgroup")


        'Loading TabGroups
        '1	19	2063	STORE_CH_SMITH_GROUP1
        lines = File.ReadAllLines(base_path & "data\refshoptabgroup.txt")
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" Then
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

                Dim tmp As New ShopTabGroup
                tmp.Group_Name = tmpString(3)

                RefShopTabGroups.Add(tmp.Group_Name, tmp)
            End If
        Next
        Log.WriteSystemLog("refshoptabgroup")


        'Loading Tabs
        '1	19	4642	STORE_CH_SMITH_TAB1	STORE_CH_SMITH_GROUP1
        lines = File.ReadAllLines(base_path & "data\refshoptab.txt")
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" Then
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

                Dim tabName As String = tmpString(3)
                Dim tabGroup As String = tmpString(4)

                Dim tmp As New ShopTab
                tmp.Tab_Name = tabName

                If RefShopTabGroups.ContainsKey(tabGroup) Then
                    Dim tabIndex As Byte = GetShopTabIndex(tabName)
                    RefShopTabGroups(tabGroup).ShopTabs.Add(tabIndex, tmp)
                End If
            End If
        Next
        Log.WriteSystemLog("refshoptabgroup")


        'Dump Items
        '1	19	STORE_CH_SMITH_TAB1	PACKAGE_ITEM_CH_SWORD_01_A	0
        lines = File.ReadAllLines(base_path & "data\refshopgoods.txt")
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" Then
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim tabName As String = tmpString(2)
                Dim packageName As String = tmpString(3)
                Dim itemLine As Byte = tmpString(4)

                Dim list = RefShopTabGroups.Keys.ToList
                For Each key In list
                    If RefShopTabGroups.ContainsKey(key) Then
                        For j = 0 To RefShopTabGroups(key).ShopTabs.Count - 1
                            If RefShopTabGroups(key).ShopTabs(j).Tab_Name = tabName Then
                                RefShopTabGroups(key).ShopTabs(j).Items.Add(itemLine, packageName)

                                Dim tmpPackage As PackageItem = GetPackageItem(packageName)
                                For k = 0 To RefPackageItems.Count - 1
                                    If RefPackageItems(k).Package_Name = packageName Then
                                        RefPackageItems(k).InShop = True
                                        RefPackageItems(k).Shop = tabName
                                        Exit For
                                    End If
                                Next

                                Exit For
                            End If
                        Next

                        Exit For
                    End If
                Next
            End If
        Next
        Log.WriteSystemLog("refshopgoods")


        'Mapping Shop with Tabs
        '1	19	STORE_CH_SMITH	STORE_CH_SMITH_GROUP1
        lines = File.ReadAllLines(base_path & "data\refmappingshopwithtab.txt")
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" Then
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

                Dim storeName As String = tmpString(2)
                Dim groupName As String = tmpString(3)

                If RefShops.ContainsKey(storeName) And RefShopTabGroups.ContainsKey(groupName) Then
                    RefShops(storeName).TabGroups.Add(RefShopTabGroups(groupName))

                    For j = 0 To RefShopTabGroups(groupName).ShopTabs.Count - 1

                    Next
                End If
            End If
        Next
        Log.WriteSystemLog("refmappingshopwithtab")
    End Sub

    Public Sub CorrectHotanData()
        Dim obj As SilkroadObject = GetObject(2072)
        obj.Shop.Tabs(0).TabName = "STORE_KT_SMITH_EU_TAB1"
        obj.Shop.Tabs(1).TabName = "STORE_KT_SMITH_EU_TAB2"
        obj.Shop.Tabs(2).TabName = "STORE_KT_SMITH_EU_TAB3"
        obj.Shop.Tabs(3).TabName = "STORE_KT_SMITH_TAB1"
        obj.Shop.Tabs(4).TabName = "STORE_KT_SMITH_TAB2"
        obj.Shop.Tabs(5).TabName = "STORE_KT_SMITH_TAB3"

        obj = GetObject(2073)
        obj.Shop.Tabs(0).TabName = "STORE_KT_ARMOR_EU_TAB1"
        obj.Shop.Tabs(1).TabName = "STORE_KT_ARMOR_EU_TAB2"
        obj.Shop.Tabs(2).TabName = "STORE_KT_ARMOR_EU_TAB3"
        obj.Shop.Tabs(3).TabName = "STORE_KT_ARMOR_EU_TAB4"
        obj.Shop.Tabs(4).TabName = "STORE_KT_ARMOR_EU_TAB5"
        obj.Shop.Tabs(5).TabName = "STORE_KT_ARMOR_EU_TAB6"
        obj.Shop.Tabs(6).TabName = "STORE_KT_ARMOR_TAB1"
        obj.Shop.Tabs(7).TabName = "STORE_KT_ARMOR_TAB2"
        obj.Shop.Tabs(8).TabName = "STORE_KT_ARMOR_TAB3"
        obj.Shop.Tabs(9).TabName = "STORE_KT_ARMOR_TAB4"
        obj.Shop.Tabs(10).TabName = "STORE_KT_ARMOR_TAB5"
        obj.Shop.Tabs(11).TabName = "STORE_KT_ARMOR_TAB6"

        obj = GetObject(2075)
        obj.Shop.Tabs(0).TabName = "STORE_KT_ACCESSORY_EU_TAB1"
        obj.Shop.Tabs(1).TabName = "STORE_KT_ACCESSORY_EU_TAB2"
        obj.Shop.Tabs(2).TabName = "STORE_KT_ACCESSORY_EU_TAB3"
        obj.Shop.Tabs(3).TabName = "STORE_KT_ACCESSORY_TAB1"
        obj.Shop.Tabs(4).TabName = "STORE_KT_ACCESSORY_TAB2"
        obj.Shop.Tabs(5).TabName = "STORE_KT_ACCESSORY_TAB3"
    End Sub

    Public Function GetNpc(ByVal StoreName As String) As Integer
        Dim tmplist As Array = RefObjects.Keys.ToArray
        For Each key In tmplist
            If RefObjects.ContainsKey(key) Then
                If RefObjects(key).Shop IsNot Nothing Then
                    If RefObjects(key).Shop.StoreName = StoreName Then
                        Return key
                    End If
                End If
            End If
        Next
        Return -1
    End Function

    Public Function GetNpc2(ByVal TabName As String) As Integer
        Dim tmplist As Array = RefObjects.Keys.ToArray
        For Each key In tmplist
            If RefObjects.ContainsKey(key) Then
                If RefObjects(key).Shop IsNot Nothing Then
                    For r = 0 To RefObjects(key).Shop.Tabs.Count - 1
                        If RefObjects(key).Shop.Tabs(r) IsNot Nothing Then
                            If RefObjects(key).Shop.Tabs(r).TabName = TabName Then
                                Return key
                            End If
                        End If
                    Next
                End If
            End If
        Next
        Return -1
    End Function

    Private Function GetShopTabIndex(ByVal tabName As String) As Byte
        Return tabName.Substring(tabName.Length - 1)
    End Function
#End Region

#Region "AbuseList"
    Public Sub DumpAbuseListFile(ByVal path As String)
        RefAbuseList.Clear()

        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" Then
                RefAbuseList.Add(lines(i))
            End If
        Next i
    End Sub
#End Region

#Region "Caveteleport"
    Public Class CaveTeleporter_
        Public FromPosition As New Position
        Public Range As Integer
        Public ToTeleporterID As Integer
    End Class


    Public Sub DumpCaveTeleporterFile(ByVal path As String)
        RefCaveTeleporter.Clear()


        Dim lines As String() = File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" Then
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim tmp As New CaveTeleporter_
                tmp.FromPosition.XSector = tmpString(0)
                tmp.FromPosition.YSector = tmpString(1)
                tmp.FromPosition.X = tmpString(2)
                tmp.FromPosition.Z = tmpString(3)
                tmp.FromPosition.Y = tmpString(4)
                tmp.Range = tmpString(5)
                tmp.ToTeleporterID = tmpString(6)
                RefCaveTeleporter.Add(tmp)
            End If
        Next i
    End Sub
#End Region

#Region "NameFiles"
    Public Class SilkroadNameEntry
        Public Code_Name As String
        Public Content As String
    End Class

    Public Sub DumpNameFiles()
        Dim paths As String() = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory & "data\textdataname.txt")
        For i As Integer = 0 To paths.Length - 1
            DumpNameFile(AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
        Next
    End Sub

    Public Sub DumpNameFile(ByVal path As String)
        RefSilkroadNameEntys.Clear()

        Dim LanguageTabIndex As Byte = 8
        Dim lines As String() = File.ReadAllLines(path)

        For i As Integer = 0 To lines.Length - 1
            If lines(i).StartsWith("//") = False And lines(i) <> "" And lines(i).StartsWith("1" & ControlChars.Tab) = True Then

                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                If tmpString.Length >= LanguageTabIndex AndAlso tmpString(1).StartsWith("SN") And RefSilkroadNameEntys.ContainsKey(tmpString(1)) = False Then
                    RefSilkroadNameEntys.Add(tmpString(1), tmpString(LanguageTabIndex))
                End If
            End If
        Next
    End Sub
#End Region
End Module
