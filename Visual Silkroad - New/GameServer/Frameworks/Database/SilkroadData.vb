Module SilkroadData

    Public RefItems As New List(Of cItem)

    Public Sub DumpItemFiles()

        Try
            Dim paths As String() = IO.File.ReadAllLines(System.AppDomain.CurrentDomain.BaseDirectory & "data\itemdata.txt")
            For i As Integer = 0 To paths.Length - 1
                DumpItemFile(System.AppDomain.CurrentDomain.BaseDirectory & "data\" & paths(i))
            Next
            Commands.WriteLog("Loaded " & RefItems.Count & " Ref-Items.")


        Catch ex As Exception
            Commands.WriteLog("Error at Loading Data! Message: " & ex.Message)
        End Try



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

End Module
