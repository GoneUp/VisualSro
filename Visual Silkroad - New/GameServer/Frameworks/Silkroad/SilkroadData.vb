﻿Module SilkroadData

    Public RefItems As New List(Of cItem)
    Public RefGoldData As New List(Of cGoldData)
    Public RefLevelData As New List(Of cLevelData)

    Public RefTmpSkills As New List(Of tmpSkill_)
    Public RefSkills As New List(Of Skill_)

    Public Sub DumpDataFiles()

        Try
            DumpItemFiles()
            Commands.WriteLog("Loaded " & RefItems.Count & " Ref-Items.")

            DumpGoldData(System.AppDomain.CurrentDomain.BaseDirectory & "data\levelgold.txt")
            Commands.WriteLog("Loaded " & RefGoldData.Count & " Ref-Goldlevels.")

            DumpLevelData(System.AppDomain.CurrentDomain.BaseDirectory & "data\leveldata.txt")
            Commands.WriteLog("Loaded " & RefLevelData.Count & " Ref-Levels.")

            DumpSkillFiles()
            Commands.WriteLog("Loaded " & RefSkills.Count & " Ref-Skills.")

        Catch ex As Exception
            Commands.WriteLog("Error at Loading Data! Message: " & ex.Message)
        End Try



    End Sub

    Public Sub DumpItemFiles()
        Dim paths As String() = IO.File.ReadAllLines(System.AppDomain.CurrentDomain.BaseDirectory & "data\itemdata.txt")
        For i As Integer = 0 To paths.Length - 1
            DumpItemFile(System.AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
        Next
    End Sub

    Public Sub DumpItemFile(ByVal path As String)


        Dim lines As String() = IO.File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1

            lines(i) = lines(i).Replace(".", ",")
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)

            Dim tmp As New cItem
            tmp.ITEM_TYPE = Convert.ToUInt32(tmpString(1))
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
            tmp.MIN_ABSORB = Convert.ToDouble(tmpString(70))
            tmp.MAX_ABSORB = Convert.ToDouble(tmpString(71))
            tmp.ABSORB_INC = Convert.ToDouble(tmpString(72))
            tmp.MIN_BLOCK = Convert.ToSingle(tmpString(73))
            tmp.MAX_BLOCK = Convert.ToSingle(tmpString(74))
            tmp.MAGDEF_MIN = Convert.ToDouble(tmpString(75))
            tmp.MAGDEF_MAX = Convert.ToDouble(tmpString(76))
            tmp.MAGDEF_INC = Convert.ToDouble(tmpString(77))
            tmp.MIN_APHYS_REINFORCE = Convert.ToSingle(tmpString(78))
            tmp.MAX_APHYS_REINFORCE = Convert.ToSingle(tmpString(79))
            tmp.MIN_AMAG_REINFORCE = Convert.ToSingle(tmpString(80))
            tmp.MAX_AMAG_REINFORCE = Convert.ToSingle(tmpString(81))
            tmp.ATTACK_DISTANCE = Convert.ToSingle(tmpString(94))
            tmp.MIN_LPHYATK = Convert.ToDouble(tmpString(95))
            tmp.MAX_LPHYATK = Convert.ToDouble(tmpString(96))
            tmp.MIN_HPHYATK = Convert.ToDouble(tmpString(97))
            tmp.MAX_HPHYATK = Convert.ToDouble(tmpString(99))
            tmp.PHYATK_INC = Convert.ToDouble(tmpString(100))
            tmp.MIN_LMAGATK = Convert.ToDouble(tmpString(101))
            tmp.MAX_LMAGATK = Convert.ToDouble(tmpString(102))
            tmp.MIN_HMAGATK = Convert.ToDouble(tmpString(103))
            tmp.MAX_HMAGATK = Convert.ToDouble(tmpString(104))
            tmp.MAGATK_INC = Convert.ToDouble(tmpString(105))
            tmp.MIN_LPHYS_REINFORCE = Convert.ToSingle(tmpString(106))
            tmp.MAX_LPHYS_REINFORCE = Convert.ToSingle(tmpString(107))
            tmp.MIN_HPHYS_REINFORCE = Convert.ToSingle(tmpString(108))
            tmp.MAX_HPHYS_REINFORCE = Convert.ToSingle(tmpString(109))
            tmp.MIN_LMAG_REINFORCE = Convert.ToSingle(tmpString(110))
            tmp.MAX_LMAG_REINFORCE = Convert.ToSingle(tmpString(111))
            tmp.MIN_HMAG_REINFORCE = Convert.ToSingle(tmpString(112))
            tmp.MAX_HMAG_REINFORCE = Convert.ToSingle(tmpString(113))
            tmp.MIN_ATTACK_RATING = Convert.ToSingle(tmpString(114))
            tmp.MAX_ATTACK_RATING = Convert.ToSingle(tmpString(115))
            tmp.MIN_CRITICAL = Convert.ToSingle(tmpString(116))
            tmp.MAX_CRITICAL = Convert.ToSingle(tmpString(117))
            tmp.USE_TIME_HP = Convert.ToInt32(tmpString(118))
            tmp.USE_TIME_HP_PER = Convert.ToInt32(tmpString(120))
            tmp.USE_TIME_MP = Convert.ToInt32(tmpString(122))
            tmp.USE_TIME_MP_PER = Convert.ToInt32(tmpString(124))
            RefItems.Add(tmp)
        Next

    End Sub
    Public Function GetItemByID(ByVal id As UInteger) As cItem
        For Each e As cItem In RefItems
            If e.ITEM_TYPE = id Then
                Return e
            End If
        Next
        Throw New Exception("Item couldn't be found!")
    End Function


    Structure cGoldData
        Public Level As Byte
        Public MinGold As ULong
        Public MaxGold As ULong
    End Structure


    Public Sub DumpGoldData(ByVal path As String)

        Dim lines As String() = IO.File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim gold As New cGoldData
            gold.Level = CByte(tmpString(0))
            gold.MinGold = CULng(tmpString(1))
            gold.MaxGold = CULng(tmpString(2))
            RefGoldData.Add(gold)
        Next
    End Sub

    Public Function GetGoldDataByLevel(ByVal level As Byte)
        For i = 0 To RefGoldData.Count - 1
            If RefGoldData(i).Level = level Then
                Return RefGoldData(i)
            End If
        Next
    End Function

    Structure cLevelData
        Public Level As Byte
        Public Experience As ULong
        Public SkillPoints As ULong
    End Structure


    Public Sub DumpLevelData(ByVal path As String)

        Dim lines As String() = IO.File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1

            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim level As New cLevelData
            level.Level = CByte(tmpString(0))
            level.Experience = CULng(tmpString(1))
            If level.Level = 1 Then
                level.SkillPoints = 0
            Else
                level.SkillPoints = CULng(tmpString(2))
            End If

            RefLevelData.Add(level)
        Next
    End Sub

    Public Function GetLevelDataByLevel(ByVal level As Byte)
        For i = 0 To RefLevelData.Count - 1
            If RefLevelData(i).Level = level Then
                Return RefLevelData(i)
            End If
        Next
    End Function


    Public Structure Skill_
        Public Name As String
        Public Id As UInteger
        Public NextId As UInteger
        Public RequiredSp As ULong
        Public RequiredMp As UShort
        Public CastTime As Byte
        Public PwrPercent As Integer
        Public PwrMin As Integer
        Public PwrMax As Integer
        Public Distance As Integer
        Public NumberOfAttacks As Byte
        Public Type As Byte
        Public Type2 As Byte
    End Structure
    Public Structure tmpSkill_
        Public Id As UInteger
        Public NextId As UInteger
    End Structure


    Public Sub DumpSkillFiles()
        Dim paths As String() = IO.File.ReadAllLines(System.AppDomain.CurrentDomain.BaseDirectory & "data\skilldata.txt")
        For i As Integer = 0 To paths.Length - 1
            DumpTmpSkillFile(System.AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
            DumpSkillFile(System.AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
        Next
    End Sub

    Public Sub DumpTmpSkillFile(ByVal path As String)

        Dim lines As String() = IO.File.ReadAllLines(path)
        For i As Integer = 0 To lines.Length - 1
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim tmp As New tmpSkill_()
            tmp.Id = Convert.ToUInt32(tmpString(1))
            tmp.NextId = Convert.ToUInt32(tmpString(9))
            RefTmpSkills.Add(tmp)
        Next
    End Sub

    Private Sub DumpSkillFile(ByVal path As String)
        Dim lines As String() = IO.File.ReadAllLines(path)

        For i As Integer = 0 To lines.Length - 1
            Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
            Dim tmp As New Skill_()
            tmp.Id = Convert.ToUInt32(tmpString(1))
            tmp.Name = tmpString(3)
            tmp.NextId = Convert.ToUInt32(tmpString(9))
            tmp.RequiredSp = Convert.ToUInt64(tmpString(46))
            tmp.RequiredMp = Convert.ToUInt16(tmpString(53))
            tmp.CastTime = Convert.ToByte(tmpString(68))
            tmp.PwrPercent = Convert.ToInt32(tmpString(71))
            tmp.PwrMin = Convert.ToInt32(tmpString(72))
            tmp.PwrMax = Convert.ToInt32(tmpString(73))
            tmp.Distance = Convert.ToInt32(tmpString(78))
            If tmp.Distance = 0 Then
                tmp.Distance = 21
            End If

            tmp.NumberOfAttacks = GetNumberOfAttacks(GetTmpSkillById(tmp.Id))
            If tmpString(3).Contains("SWORD") Then
                tmp.Type = TypeTable.Phy
                tmp.Type2 = TypeTable.Bicheon
            End If
            If tmpString(3).Contains("SPEAR") Then
                tmp.Type = TypeTable.Phy
                tmp.Type2 = TypeTable.Heuksal
            End If
            If tmpString(3).Contains("BOW") Then
                tmp.Type = TypeTable.Phy
                tmp.Type2 = TypeTable.Bow
            End If
            If tmpString(3).Contains("FIRE") OrElse tmpString(3).Contains("LIGHTNING") OrElse tmpString(3).Contains("COLD") OrElse tmpString(3).Contains("WATER") Then
                tmp.Type = TypeTable.Mag
                tmp.Type2 = TypeTable.All
            End If
            If tmpString(3).Contains("PUNCH") Then
                tmp.Type = TypeTable.Phy
                tmp.Type2 = TypeTable.All
            End If
            If tmpString(3).Contains("ROG") OrElse tmpString(3).Contains("WARRIOR") Then
                tmp.Type = TypeTable.Phy
                tmp.Type2 = TypeTable.All
            End If

            If tmpString(3).Contains("WIZARD") OrElse tmpString(3).Contains("STAFF") OrElse tmpString(3).Contains("WARLOCK") OrElse tmpString(3).Contains("BARD") OrElse tmpString(3).Contains("HARP") OrElse tmpString(3).Contains("CLERIC") Then
                tmp.Type = TypeTable.Mag
                tmp.Type2 = TypeTable.All
            End If

            RefSkills.Add(tmp)
        Next
    End Sub

    Public Class TypeTable
        Public Const Phy As Byte = &H1, Mag As Byte = &H2, Bicheon As Byte = &H3, Heuksal As Byte = &H4, Bow As Byte = &H5, All As Byte = &H6
    End Class

    Private Function GetNumberOfAttacks(ByVal tmp As tmpSkill_) As Byte
        For i As Byte = 0 To 9
            If tmp.NextId <> 0 Then
                tmp = GetTmpSkillById(tmp.NextId)
            Else
                Return CByte(i + 1)
            End If
        Next
        Return 1
    End Function

    Public Function GetSkillById(ByVal ItemId As UInteger) As Skill_
        For i As Integer = 0 To RefSkills.Count - 1
            If RefSkills(i).Id = ItemId Then
                Return RefSkills(i)
            End If
        Next
        Return New Skill_()
    End Function

    Private Function GetTmpSkillById(ByVal NextId As UInteger) As tmpSkill_
        For i As Integer = 0 To RefTmpSkills.Count - 1
            If RefTmpSkills(i).Id = NextId Then
                Return RefTmpSkills(i)
            End If
        Next
        Return New tmpSkill_()
    End Function



End Module