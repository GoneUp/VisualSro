Option Strict Off
Option Explicit On
Module modUseItem
	Private fData As String
	Private pLen As Short
	Private i As Short
	Private x As Short
	Public Function TakeCape(ByRef index As Short) As Object
		
		fData = "3434"
		fData = fData & "0000"
		'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		fData = fData & PlayerData(index).CharID
		fData = fData & "02010A"
		fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function
    Public Function ReturnScrols(ByRef index As Short) As Object
        Dim ids As String
        Dim amounts As String
        Dim namounts As Short
        Dim newamount As String
        Dim itemid As String
        ids = HexToDec(Inverse(Mid(sData, 1, 2)))
        Dim Slot As String
        Dim amountsss As Short
        Slot = Mid(sData, 1, 2)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        amounts = CharItems(index, CInt(ids)).amount
        namounts = CDbl(amounts) - 1
        newamount = CStr(namounts)

        'Dim ss As String
        'ss = Right(sData, 4)
        'MsgBox (sData)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = "060022310000" & PlayerData(index).CharID & "0B01"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        itemid = CStr(iniGetStr("amount", "Item" & ids, PlayerData(index).CharPath))
        amountsss = CDbl(itemid) - 1
        itemid = CStr(CDbl(itemid) - 1)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iniWrite(itemid, "amount", "item" & ids, PlayerData(index).CharPath)
        fData = "BDB5"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & Slot
        fData = fData & ByteFromInteger(amountsss)
        'fData = fData & ByteFromInteger(namounts)
        fData = fData & "00EC09"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
        'MsgBox (fData)
        'efect
        fData = "4934"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "1A4B0000"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
        'MsgBox (fData)

    End Function
    Public Function UseItem(ByRef index As Short, ByRef data As String) As Object
        Dim typeu As String
        typeu = HexToDec(Inverse(Mid(sData, 1, 2)))
        Dim useids As String
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        useids = CharItems(index, CInt(typeu)).ID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems(index, typeu).ID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Select Case CharItems(index, CInt(typeu)).ID

            Case "61"
                ReturnScrols(index) ', useids
            Case "2198"
                ReturnScrols(index) ', useids
            Case "19226"
                ReturnScrols(index) ', useids

            Case "7098" 'speed pot
                SpeedPot(index)
            Case "7100"
                SpeedPot(index)
            Case "2137" 'horse
                UseTransport(index, useids, 2191)
            Case "2138"
                UseTransport(index, useids, 2192)
            Case "2139"
                UseTransport(index, useids, 2193)
            Case "3909"
                UseTransport(index, useids, 3918)
            Case "7488" 'wolf

            Case "4" ' pot
                HandlePot(index)
            Case "5"
                HandlePot(index)
            Case "6"
                HandlePot(index)
            Case "7"
                HandlePot(index)
            Case "8"
                HandlePot(index)
            Case "9"


            Case "11" ' pot
                HandlePot(index)
            Case "12"
                HandlePot(index)
            Case "13"
                HandlePot(index)
            Case "14"
                HandlePot(index)
            Case "15"
                HandlePot(index)
            Case "16"
                HandlePot(index)

            Case "18" ' pot
                HandlePot(index)
            Case "19"
                HandlePot(index)
            Case "20"
                HandlePot(index)
            Case "21"
                HandlePot(index)
            Case "22"
                HandlePot(index)
            Case "23"
                HandlePot(index)
            Case "55" ' pill
                HandlePot(index)
            Case "56"
                HandlePot(index)
            Case "57"
                HandlePot(index)
            Case "58"
                HandlePot(index)
            Case "10368" 'state
                HandlePot(index)
            Case "10369"
                HandlePot(index)
            Case "10370"
                HandlePot(index)
            Case "10371"
                HandlePot(index)
            Case "10372"
                HandlePot(index)
            Case "10373"
                HandlePot(index)
            Case "10374"
                HandlePot(index)
            Case "10375"
                HandlePot(index)

            Case "3851"
                GlobalMessage(index, data)
        End Select
    End Function
    Public Function HandlePot(ByRef index As Short) As Object
        Dim data As String
        data = sData
        Dim psize As String
        Dim tslot As String
        Dim pottype As Short
        Dim potcount As Short
        Dim newpot As Short
        Dim laenge As Short
        Dim potsave As String
        ' ==== Check if its HP Pots ====
        Dim shp As Short
        If Right(data, 4) = "EC08" Then
            tslot = CStr(CInt("&H" & Left(data, 2))) 'check wich slot in inventory is pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pottype = CShort(iniGetStr("ID", "Item" & tslot, PlayerData(index).CharPath)) 'check size of Pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            potcount = CShort(iniGetStr("amount", "Item" & tslot, PlayerData(index).CharPath)) 'read amount of pots
            shp = 2000
            fData = "0B00"
            fData = fData & "A633"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).CharID
            fData = fData & "200001"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(DecToHexLong(PlayerData(index).ArtHP + (shp)))
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtHP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).ArtHP = PlayerData(index).ArtHP + (shp)
            modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

            ' Set Pot size
            psize = Hex(pottype)
            laenge = Len(psize)
            If laenge = 2 Then
                psize = psize & "000000"
            Else
                psize = "0" & psize & "000000"
            End If
            ' =============
            newpot = potcount - 1 'calculate new pot amount
            potsave = CStr(newpot)
            If newpot = 0 Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(vbNullString, vbNullString, "item" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are less than 0
            Else
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(potsave, "amount", "ITEM" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are more than 0
            End If
        End If
        ' ==============================
        ' ==== Check if its MP Pots ====
        Dim smana As Short
        Dim mps As Short
        If Right(data, 4) = "EC10" Then
            tslot = CStr(CInt("&H" & Left(data, 2))) 'check wich slot in inventory is pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pottype = CShort(iniGetStr("ID", "Item" & tslot, PlayerData(index).CharPath)) 'check size of Pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            potcount = CShort(iniGetStr("amount", "Item" & tslot, PlayerData(index).CharPath)) 'read amount of pots
            smana = 1500
            fData = "0B00"
            fData = fData & "A633"
            fData = fData & "0000"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & PlayerData(index).CharID
            fData = fData & "100002"
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(DecToHexLong(PlayerData(index).ArtMP + (smana)))
            'PlayerData(index).MP = PlayerData(index).MP - (sMana) 'take away mp cost
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).ArtMP = PlayerData(index).ArtMP + (smana)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().ArtMP konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            mps = PlayerData(index).ArtMP
            'iniWrite CInt(mps), "ArtMP", "character", PlayerData(index).CharPath
            modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

            ' Set Pot size
            psize = Hex(pottype)
            laenge = Len(psize)
            If laenge = 2 Then
                psize = psize & "000000"
            Else
                psize = "0" & psize & "000000"
            End If
            ' =============
            newpot = potcount - 1 'calculate new pot amount
            potsave = CStr(newpot)
            If newpot = 0 Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(vbNullString, vbNullString, "item" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are less than 0
            Else
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(potsave, "amount", "ITEM" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are more than 0
            End If
        End If
        ' ==============================
        ' ==== Check if its Vigor Pots ====
        If Right(data, 4) = "EC18" Then
            tslot = CStr(CInt("&H" & Left(data, 2))) 'check wich slot in inventory is pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pottype = CShort(iniGetStr("ID", "Item" & tslot, PlayerData(index).CharPath)) 'check size of Pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            potcount = CShort(iniGetStr("amount", "Item" & tslot, PlayerData(index).CharPath)) 'read amount of pots
            ' Set Pot size
            psize = Hex(pottype)
            laenge = Len(psize)
            If laenge = 2 Then
                psize = psize & "000000"
            Else
                psize = "0" & psize & "000000"
            End If
            ' =============
            newpot = potcount - 1 'calculate new pot amount
            potsave = CStr(newpot)
            If newpot = 0 Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(vbNullString, vbNullString, "item" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are less than 0
            Else
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(potsave, "amount", "ITEM" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are more than 0
            End If
        End If
        ' ==============================
        ' ==== Check if its UniPil Pots ====
        If Right(data, 4) = "6C31" Then
            tslot = CStr(CInt("&H" & Left(data, 2))) 'check wich slot in inventory is pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pottype = CShort(iniGetStr("ID", "Item" & tslot, PlayerData(index).CharPath)) 'check size of Pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            potcount = CShort(iniGetStr("amount", "Item" & tslot, PlayerData(index).CharPath)) 'read amount of pots
            ' Set Pot size
            psize = Hex(pottype)
            laenge = Len(psize)
            If laenge = 2 Then
                psize = psize & "000000"
            Else
                psize = "0" & psize & "000000"
            End If
            ' =============
            newpot = potcount - 1 'calculate new pot amount
            potsave = CStr(newpot)
            If newpot = 0 Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(vbNullString, vbNullString, "item" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are less than 0
            Else
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(potsave, "amount", "ITEM" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are more than 0
            End If
        End If
        ' ==============================
        ' ==== Check if its AntiState Pots ====
        If Right(data, 4) = "6C09" Then
            tslot = CStr(CInt("&H" & Left(data, 2))) 'check wich slot in inventory is pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pottype = CShort(iniGetStr("ID", "Item" & tslot, PlayerData(index).CharPath)) 'check size of Pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            potcount = CShort(iniGetStr("amount", "Item" & tslot, PlayerData(index).CharPath)) 'read amount of pots
            ' Set Pot size
            psize = Hex(pottype)
            psize = Inverse(psize) & "0000"
            ' =============
            newpot = potcount - 1 'calculate new pot amount
            potsave = CStr(newpot)
            If newpot = 0 Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(vbNullString, vbNullString, "item" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are less than 0
            Else
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(potsave, "amount", "ITEM" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are more than 0
            End If
        End If
        ' ==============================
        ' ==== Check if its AntiStealth Pots ====
        If Right(data, 4) = "EC0E" Then
            tslot = CStr(CInt("&H" & Left(data, 2))) 'check wich slot in inventory is pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pottype = CShort(iniGetStr("ID", "Item" & tslot, PlayerData(index).CharPath)) 'check size of Pot
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            potcount = CShort(iniGetStr("amount", "Item" & tslot, PlayerData(index).CharPath)) 'read amount of pots
            ' Set Pot size
            psize = Hex(pottype)
            psize = Inverse(psize) & "0000"
            ' =============
            newpot = potcount - 1 'calculate new pot amount
            potsave = CStr(newpot)
            If newpot = 0 Then
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(vbNullString, vbNullString, "item" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are less than 0
            Else
                'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharPath konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iniWrite(potsave, "amount", "ITEM" & tslot, PlayerData(index).CharPath) 'set new pot amount if pots are more than 0
            End If

        End If
        ' ==============================

        fData = "4934"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & psize
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i


        Dim ids As String
        Dim amounts As String
        Dim namounts As Short
        Dim newamount As String
        Dim itemid As String
        ids = HexToDec(Inverse(Mid(sData, 1, 2)))
        Dim Slot As String
        Dim amountsss As Short
        Slot = Mid(sData, 1, 2)
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts CharItems().amount konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        amounts = CharItems(index, CInt(ids)).amount
        namounts = CDbl(amounts) - 1
        newamount = CStr(namounts)
        Dim type23 As String
        type23 = Left(sData, 4)
        'Dim ss As String
        'ss = Right(sData, 4)
        'MsgBox (sData)

        'itemid = CStr(iniGetStr("amount", "Item" & ids, PlayerData(index).CharPath))
        'itemid = itemid - 1
        'iniWrite itemid, "amount", "item" & ids, PlayerData(index).CharPath
        fData = "BDB5"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & Slot
        'amountsss = itemid
        fData = fData & ByteFromInteger(newpot)
        'fData = fData & ByteFromInteger(namounts)
        fData = fData & "00" & type23
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function
    Public Function UseTransport(ByRef index As Short, ByRef ids As String, ByRef Transport As Integer) As Object
        '   Transport = ids
        Dim TransportID As String
        Dim Walking As String
        'Transport = "2191"
        'Start summon
        'fData = "0400"
        'fData = fData & "4934"
        'fData = fData & "0000"
        'fData = fData & "59080000" 'item id
        fData = "4934"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & DWordFromInteger(ids) '"1A4B0000"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))
        'spawn transport
        fData = "D730"
        fData = fData & "0000"
        fData = fData & Inverse(DecToHexLong(Transport))
        TransportID = Inverse(DecToHexLong(Int(Rnd() * 2000000) + 1500000))
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().TransportID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).TransportID = TransportID
        fData = fData & TransportID
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((PlayerData(index).XPos - ((PlayerData(index).XSection) - 135) * 192) * 10)) 'X
        fData = fData & "00000000" 'Z
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & Inverse(Float2Hex((PlayerData(index).YPos - ((PlayerData(index).YSection) - 92) * 192) * 10)) 'Y
        fData = fData & "DC72"
        fData = fData & "00" '01 walking
        fData = fData & "01"

        If Walking = "01" Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).XSection)) 'X sector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(ByteFromInteger(PlayerData(index).YSection)) 'ySector
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).XSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().XPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(WordFromInteger((PlayerData(index).XPos) - ((PlayerData(index).XSection) - 135) * 192)) 'X
            fData = fData & "0000" 'Z
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).YSection konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().YPos konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            fData = fData & Inverse(WordFromInteger((PlayerData(index).YPos) - ((PlayerData(index).YSection) - 92) * 192)) 'Y
        End If

        fData = fData & "00"
        fData = fData & "DC72"
        fData = fData & "0100"
        fData = fData & "00" 'Berserker
        fData = fData & "00003442" 'Playerspeed while walking
        fData = fData & "0000B442" 'Playerspeed while running
        fData = fData & "0000C842" 'Playerspeed while berserk
        fData = fData & "0000"
        fData = fData & "01"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "1100"
        fData = fData & "5831"
        fData = fData & "0000"
        fData = fData & TransportID
        fData = fData & Inverse(DecToHexLong(Transport))
        fData = fData & "D7030000"
        fData = fData & "D7030000"
        fData = fData & "00"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "0C00"
        fData = fData & "6F37"
        fData = fData & "0000"
        fData = fData & TransportID
        fData = fData & Inverse(Float2Hex(30)) 'walking speed
        fData = fData & Inverse(Float2Hex(130)) 'running speed

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "0600"
        fData = fData & "2231"
        fData = fData & "0000"
        fData = fData & TransportID
        fData = fData & "0103"
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        fData = "0A00"
        fData = fData & "B5B4"
        fData = fData & "0000"
        fData = fData & "01"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "01"
        fData = fData & TransportID
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Transport konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Transport = Transport
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Riding konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Riding = True
    End Function
    Public Function SpeedPot(ByRef index As Short) As Object
        Dim SkillID As String
        Dim CastingID As String
        fData = "45B2"
        fData = fData & "0000"
        fData = fData & "0100"
        fData = fData & "870F0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        CastingID = (Inverse(WordFromInteger(Int(Rnd() * 1048575) + 65536))) & "000"
        fData = fData & CastingID
        fData = fData & "0000000000"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))


        fData = "05B5"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & CastingID
        fData = fData & "00"
        fData = fData & "00000000"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        fData = "19B4"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "870F0000"
        fData = fData & "5EB30600"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "9136"
        fData = fData & "0000"
        fData = fData & "005EB3060040771B00"
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i
        Dim Speed As Double
        Speed = 150
        fData = "0C00"
        fData = fData & "6F37"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "00008041" 'Playerspeed while walking
        fData = fData & Inverse(Float2Hex(Speed))

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().RunSpeed konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).RunSpeed = Speed
        Dim ids As String
        Dim amounts As String
        Dim namounts As Short
        Dim newamount As String
        Dim itemid As String
        ids = HexToDec(Inverse(Mid(sData, 1, 2)))
        Dim Slot As String
        Dim amountsss As Short
        Slot = Mid(sData, 1, 2)
        'amounts = CharItems(index, ids).amount
        'namounts = amounts - 1
        newamount = CStr(namounts)

        'Dim ss As String
        'ss = Right(sData, 4)
        'MsgBox (sData)
        'itemid = CStr(iniGetStr("amount", "Item" & ids, PlayerData(index).CharPath))
        'amountsss = itemid - 1
        'itemid = itemid - 1
        'iniWrite itemid, "amount", "item" & ids, PlayerData(index).CharPath

        fData = "BDB5"
        fData = fData & "0000"
        fData = fData & "01"
        fData = fData & Slot
        fData = fData & "00" 'ByteFromInteger(amountsss)
        'fData = fData & ByteFromInteger(namounts)
        fData = fData & "00EC0E"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

        'fData = "BDB5"
        'fData = fData & "0000"
        'fData = fData & "01100000EC0E"
        'pLen = (Len(fData) - 8) / 2
        'fData = WordFromInteger(pLen) & fData
        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i




    End Function
    Public Function GlobalMessage(ByRef index As Short, ByRef data As String) As Object
        'Dim lenmsg As String
        Dim ChatType As Short
        Dim MessageInUnicode As Boolean
        Dim Message As String
        Dim Name As String
        'lenmsg = Mid(sData, 7, 4)
        ChatType = 6
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Name = PlayerData(index).Charname
        Message = Mid(data, 23, Len(data) - 22)
        MessageInUnicode = True
        Dim HexMessage As String
        Dim HexWithOhs As String

        fData = "6736" & "0000"
        fData = fData & "0" & ChatType
        pLen = Len(Name)
        If Not ChatType = 7 Then
            fData = fData & WordFromInteger(pLen)
            fData = fData & cv_HexFromString(Name)
        End If
        If MessageInUnicode = False Then
            HexMessage = cv_HexFromString(Message)
            For i = 1 To Len(HexMessage) Step 2
                HexWithOhs = HexWithOhs & Mid(HexMessage, i, 2) & "00"
            Next i
            fData = fData & WordFromInteger(Len(Message))
            fData = fData & HexWithOhs
        Else
            fData = fData & (WordFromInteger(Len(Message) / 4))
            fData = fData & Message
        End If
        pLen = (Len(fData) - 8) / 2
        fData = WordFromInteger(pLen) & fData

        For i = 1 To UBound(PlayerData)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Ingame konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If PlayerData(i).Ingame = True Then
                modGlobal.GameSocket(i).SendData(cv_StringFromHex(fData))
            End If
        Next i

        fData = "4934"
        fData = fData & "0000"
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().CharID konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        fData = fData & PlayerData(index).CharID
        fData = fData & "1A4B0000"
        fData = WordFromInteger((Len(fData) - 8) / 2) & fData
        modGlobal.GameSocket(index).SendData(cv_StringFromHex(fData))

    End Function
End Module