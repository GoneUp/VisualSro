Namespace GameServer
    Module SilkroadData

        Public RefItems As New List(Of cItem)
        Public RefGoldData As New List(Of cGoldData)
        Public RefLevelData As New List(Of cLevelData)

        Public RefTmpSkills As New List(Of tmpSkill_)
        Public RefSkills As New List(Of Skill_)
        Public RefObjects As New List(Of Object_)
        Public RefMallItems As New List(Of MallPackage_)
        Public RefReversePoints As New List(Of ReversePoint_)
        Public RefTeleportPoints As New List(Of TeleportPoint_)

        Public Sub DumpDataFiles()

            Try
                Dim base_path As String = System.AppDomain.CurrentDomain.BaseDirectory
                Dim time As Date = Date.Now

                DumpItemFiles()
                Log.WriteSystemLog("Loaded " & RefItems.Count & " Ref-Items.")

                DumpGoldData(base_path & "data\levelgold.txt")
                Log.WriteSystemLog("Loaded " & RefGoldData.Count & " Ref-Goldlevels.")

                DumpLevelData(base_path & "data\leveldata.txt")
                Log.WriteSystemLog("Loaded " & RefLevelData.Count & " Ref-Levels.")

                DumpSkillFiles()
                Log.WriteSystemLog("Loaded " & RefSkills.Count & " Ref-Skills.")

                DumpObjectFiles()
                Log.WriteSystemLog("Loaded " & RefObjects.Count & " Ref-Objects.")

                DumpReversePoints(base_path & "data\reverse_points.txt")
                Log.WriteSystemLog("Loaded " & RefReversePoints.Count & " Reverse-Points.")

                DumpItemMall(base_path & "data\refscrapofpackageitem.txt", base_path & "data\refpricepolicyofitem.txt")
                Log.WriteSystemLog("Loaded " & RefMallItems.Count & " ItemMall-Items.")

                DumpTeleportBuildings(base_path & "data\teleportbuilding.txt")
                DumpTeleportData(base_path & "data\teleportdata.txt", base_path & "data\teleportlink.txt")
                Log.WriteSystemLog("Loaded " & RefTeleportPoints.Count & " Teleport-Points.")

                Functions.LoadAutoSpawn(base_path & "data\npcpos.txt")
                Log.WriteSystemLog("Loaded " & Functions.MobList.Count & " Autospawn Monster.")
                Log.WriteSystemLog("Loaded " & Functions.NpcList.Count & " Autospawn Npc's.")

                Log.WriteSystemLog("Loading took " & DateDiff(DateInterval.Second, time, Date.Now) & " Seconds.")

            Catch ex As Exception
                Log.WriteSystemLog("Error at Loading Data! Message: " & ex.Message)
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


                If My.Computer.Info.OSFullName.Contains("x64") = True Then
                    lines(i) = lines(i).Replace(",", ".")
                End If
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
                tmp.MAX_HPHYATK = Convert.ToDouble(tmpString(98))
                tmp.PHYATK_INC = Convert.ToDouble(tmpString(99))
                tmp.MIN_LMAGATK = Convert.ToDouble(tmpString(100))
                tmp.MAX_LMAGATK = Convert.ToDouble(tmpString(101))
                tmp.MIN_HMAGATK = Convert.ToDouble(tmpString(102))
                tmp.MAX_HMAGATK = Convert.ToDouble(tmpString(103))
                tmp.MAGATK_INC = Convert.ToDouble(tmpString(104))
                tmp.MIN_LPHYS_REINFORCE = Convert.ToSingle(tmpString(105))
                tmp.MAX_LPHYS_REINFORCE = Convert.ToSingle(tmpString(106))
                tmp.MIN_HPHYS_REINFORCE = Convert.ToSingle(tmpString(107))
                tmp.MAX_HPHYS_REINFORCE = Convert.ToSingle(tmpString(108))
                tmp.MIN_LMAG_REINFORCE = Convert.ToSingle(tmpString(109))
                tmp.MAX_LMAG_REINFORCE = Convert.ToSingle(tmpString(110))
                tmp.MIN_HMAG_REINFORCE = Convert.ToSingle(tmpString(111))
                tmp.MAX_HMAG_REINFORCE = Convert.ToSingle(tmpString(112))
                tmp.MIN_ATTACK_RATING = Convert.ToSingle(tmpString(113))
                tmp.MAX_ATTACK_RATING = Convert.ToSingle(tmpString(114))
                tmp.MIN_CRITICAL = Convert.ToSingle(tmpString(115))
                tmp.MAX_CRITICAL = Convert.ToSingle(tmpString(116))
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

        Public Function GetItemByName(ByVal Name As String) As cItem
            For Each e As cItem In RefItems
                If e.ITEM_TYPE_NAME = Name Then
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

        Public Function GetLevelDataByLevel(ByVal level As Byte) As cLevelData
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

        Public Structure Object_
            Public Id As UInteger
            Public Name As String
            Public OtherName As String
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

            'These Fileds are for Teleports
            Public T_Position As Position


            Enum Type_
                Mob_Normal = 0
                Npc = 1
                Teleport = 2
                [Structure] = 3
                Mob_Cave = 4
                COS = 5
            End Enum
        End Structure




        Public Sub DumpObjectFiles()
            Dim paths As String() = IO.File.ReadAllLines(System.AppDomain.CurrentDomain.BaseDirectory & "data\characterdata.txt")
            For i As Integer = 0 To paths.Length - 1
                DumpObjectFile(System.AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
            Next
        End Sub

        Private Sub DumpObjectFile(ByVal path As String)
            Dim lines As String() = IO.File.ReadAllLines(path)
            For i As Integer = 0 To lines.Length - 1
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim tmp As New Object_()
                tmp.Id = Convert.ToUInt32(tmpString(1))
                tmp.Name = tmpString(2)
                tmp.OtherName = tmpString(3)
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

                Dim selector As String() = tmp.Name.Split("_")
                Select Case selector(0)
                    Case "MOB"
                        If selector(1) = "TQ" Or selector(1) = "DH" Then
                            tmp.Type = Object_.Type_.Mob_Cave
                        Else
                            tmp.Type = Object_.Type_.Mob_Normal
                        End If

                    Case "NPC"
                        tmp.Type = Object_.Type_.Npc
                    Case "STORE"
                        tmp.Type = Object_.Type_.Teleport
                    Case "STRUCTURE"
                        tmp.Type = Object_.Type_.Structure
                    Case "COS"
                        tmp.Type = Object_.Type_.COS
                End Select

                'Error Preventing
                If tmp.WalkSpeed = 0 Then
                    tmp.WalkSpeed = 1
                End If
                If tmp.RunSpeed = 0 Then
                    tmp.RunSpeed = 1
                End If
                If tmp.BerserkSpeed = 0 Then
                    tmp.BerserkSpeed = 1
                End If

                RefObjects.Add(tmp)
            Next
        End Sub

        Public Function GetObjectById(ByVal ItemId As UInteger) As Object_
            For i As Integer = 0 To RefObjects.Count - 1
                If RefObjects(i).Id = ItemId Then
                    Return RefObjects(i)
                End If
            Next
            Return New Object_()
        End Function

        Structure ReversePoint_
            Public TeleportID As UInteger
            Public Position As Position
        End Structure

        Public Sub DumpReversePoints(ByVal path As String)
            Dim lines As String() = IO.File.ReadAllLines(path)
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

        Public Function GetReversePointByID(ByVal id As UInteger)
            For i = 0 To RefReversePoints.Count - 1
                If RefReversePoints(i).TeleportID = id Then
                    Return RefReversePoints(i)
                End If
            Next
            Return New ReversePoint_
        End Function

        Structure MallPackage_
            Public Name_Normal As String
            Public Name_Package As String
            Public Amout As UInt16
            Public Price As UInteger
        End Structure

        Public Sub DumpItemMall(ByVal FileAmoutPath As String, ByVal FilePricePath As String)
            Dim lines As String() = IO.File.ReadAllLines(FileAmoutPath)
            For i As Integer = 0 To lines.Length - 1
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim tmp As New MallPackage_
                tmp.Name_Package = tmpString(2)
                tmp.Name_Normal = tmpString(3)
                tmp.Amout = tmpString(6)

                Dim priceFile As String() = IO.File.ReadAllLines(FilePricePath)
                For d As Integer = 0 To priceFile.Length - 1
                    Dim tmpString2 As String() = priceFile(d).Split(ControlChars.Tab)
                    If tmpString2(2) = tmp.Name_Package And tmpString2(3) = 2 Then
                        tmp.Price = tmpString2(5)
                        Exit For
                    End If
                Next

                RefMallItems.Add(tmp)
            Next
        End Sub

        Public Function GetItemMallItemByName(ByVal Name As String) As MallPackage_
            For i = 0 To RefMallItems.Count - 1
                If RefMallItems(i).Name_Package = Name Then
                    Return RefMallItems(i)
                End If
            Next
            Return New MallPackage_
        End Function


        Public Sub DumpTeleportBuildings(ByVal path As String)
            Dim lines As String() = IO.File.ReadAllLines(path)
            For i As Integer = 0 To lines.Length - 1
                Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                Dim obj As New Object_

                obj.Id = tmpString(1)
                obj.Name = tmpString(2)
                obj.Type = Object_.Type_.Teleport

                Dim area As Integer = tmpString(41)
                obj.T_Position = New Position
                obj.T_Position.XSector = Convert.ToByte((area).ToString("X4").Substring(2, 2), 16)
                obj.T_Position.YSector = Convert.ToByte((area).ToString("X4").Substring(0, 2), 16)

                obj.T_Position.X = tmpString(43)
                obj.T_Position.Z = tmpString(44)
                obj.T_Position.Y = tmpString(45)

                RefObjects.Add(obj)
            Next
        End Sub

        Structure TeleportPoint_
            Public Number As Integer
            Public Name As String
            Public Pk2ID As UInteger
            Public ToPos As Position
            Public Cost As UInteger
            Public MinLevel As UInteger
            Public MaxLevel As UInteger
        End Structure

        Public Sub DumpTeleportData(ByVal Path_Data As String, ByVal Path_Link As String)
            Dim lines As String() = IO.File.ReadAllLines(Path_Data)
            For i As Integer = 0 To lines.Length - 1

                Try
                    Dim tmpString As String() = lines(i).Split(ControlChars.Tab)
                    Dim obj As New TeleportPoint_

                    obj.Number = tmpString(1)
                    obj.Name = tmpString(2)

                    Dim area As Integer = tmpString(5)
                    obj.ToPos = New Position
                    obj.ToPos.XSector = Convert.ToByte((area).ToString("X4").Substring(2, 2), 16)
                    obj.ToPos.YSector = Convert.ToByte((area).ToString("X4").Substring(0, 2), 16)

                    obj.ToPos.X = tmpString(6)
                    obj.ToPos.Z = tmpString(7)
                    obj.ToPos.Y = tmpString(8)



                    Dim link_lines As String() = IO.File.ReadAllLines(Path_Link)
                    For b As Integer = 0 To link_lines.Length - 1
                        Dim tmpString2 As String() = link_lines(b).Split(ControlChars.Tab)
                        If tmpString2(1) = obj.Number Then
                            obj.Cost = tmpString2(3)
                            obj.MinLevel = tmpString2(7)
                            obj.MaxLevel = tmpString2(8)
                            Exit For
                        End If
                    Next


                    RefTeleportPoints.Add(obj)
                Catch ex As Exception

                End Try
            Next
        End Sub

        Public Function GetTeleportPoint(ByVal Number As UInteger)
            For i = 0 To RefTeleportPoints.Count - 1
                If RefTeleportPoints(i).Number = Number Then
                    Return RefTeleportPoints(i)
                End If
            Next
        End Function
    End Module
End Namespace