Option Strict Off
Option Explicit On
Imports MSWinsockLib

Imports System.Net.Sockets

Imports System
Imports System.IO

Imports System.Windows.Forms.Form

Module modGlobal
    'SRVB - SREmu VB Open-Source Project
    'Copyright (C) 2008 DarkInc Community
    '
    'This program is free software: you can redistribute it and/or modify
    'it under the terms of the GNU General Public License as published by
    'the Free Software Foundation, either version 3 of the License, or
    '(at your option) any later version.
    '
    'This program is distributed in the hope that it will be useful,
    'but WITHOUT ANY WARRANTY; without even the implied warranty of
    'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    'GNU General Public License for more details.
    '
    'You should have received a copy of the GNU General Public License
    'along with this program.  If not, see <http://www.gnu.org/licenses/>.


    'Sockets

    Dim Arrays As New ArrayList
    Dim i As Short
    Public WithEvents GameSocket As New AxWinsockArray
    Public WithEvents LoginSocket As New AxWinsockArray
    Dim ls As Short
    Dim gs As Short

    Public iMaxSockets As Short
    Public ItemAmount(500) As Short
    Public ItemData(500) As String
    Public ListItemAmount(500) As Short
    Public ListItemData(500) As String
    Public PartyCount As Short
    Public PartyExist(65535) As Boolean
    Public PartysFormed As Short
    Public UniqueID(6) As String



    Public CharItems(500, 44) As CharItemData
    Public AvatarItemList(500, 5) As AvatarItems
    Public Items(500) As ItemDataMain
    Public Mobs(500) As MobData
    Public PlayerData() As pData
    Public NPCList(100) As NPCData 'not sure how many npcs in total are in the game
    Public ServerStarted As Boolean



    Public Structure pData
        Dim Username As String
        Dim Password As String
        Dim Charname As String
        Dim AccountId As String
        Dim BerserkBar As Short
        Dim State As Short
        Dim charstate As Short
        Dim GrowthPet As Integer
        Dim GrowthPetID As String
        Dim GrowthPetXPos As Double
        Dim GrowthPetYPos As Double
        Dim WalkSpeed As Short
        Dim RunSpeed As Short
        Dim BerserkSpeed As Short
        Dim UsingSkill As Boolean
        Dim AttackSkillID As String
        Dim Berserking As Boolean
        Dim Attacking As Boolean
        Dim AttackingID As String
        Dim Race As Short
        Dim CastingID As String
        Dim CastingSkillID As String
        Dim Transport As String
        Dim TransportID As String
        Dim Riding As Boolean
        Dim Busy As Boolean
        Dim Walking As Boolean
        Dim ActiveBuffs As Short
        <VBFixedArray(20)> Dim BuffList() As String
        <VBFixedArray(7)> Dim MasteryLvl() As Short
        Dim NumSkills As Short
        <VBFixedArray(300)> Dim SkillList() As String
        Dim Ingame As Boolean
        Dim CharID As String
        Dim Position As String
        Dim Chartype As Short
        Dim AttributePoints As Short
        Dim Volume As Short
        Dim level As Short
        Dim HighLevel As Short
        Dim Strength As Short
        Dim Intelligence As Short
        Dim Skillpoints As Double
        Dim Exp As Double
        Dim HP As Double
        Dim MP As Double
        Dim ArtHP As Double
        Dim ArtMP As Double
        Dim MinPhyAtk As Double
        Dim MaxPhyAtk As Double
        Dim MinMagAtk As Double
        Dim MaxMagAtk As Double
        Dim PhyDef As Double
        Dim MagDef As Double
        Dim Hit As Double
        Dim Parry As Double
        Dim XSection As Short
        Dim YSection As Short
        Dim XPos As Double
        Dim YPos As Double
        Dim WalkingX As Short
        Dim WalkingY As Short
        Dim CharPath As String
        Dim GM As Boolean
        Dim SelectedID As String
        Dim ChatIndex As Short
        Dim NewStall As Boolean
        Dim StallName As String
        Dim StallWelcomeMessage As String
        Dim StallIsOpen As Boolean
        Dim StallItemCount As Short
        <VBFixedArray(9)> Dim StallItem() As Boolean
        <VBFixedArray(9)> Dim StallItemID() As String
        <VBFixedArray(9)> Dim StallItemInventorySlot() As String
        <VBFixedArray(9)> Dim StallItemAmount() As String
        <VBFixedArray(9)> Dim StallItemGold() As String
        Dim StallChatCount As Short
        Dim IsInExchange As Boolean
        Dim ExchangeTargetID As String
        Dim ExchangeTargetWinSock As Short
        Dim IsInPT As Boolean
        Dim PTTargetID As String
        Dim PTTargetWinSock As Short
        Dim PTTargetname As String
        <VBFixedArray(100)> Dim InventorySlot() As String
        Dim GoldInInventory As String
        Dim GoldInStorage As String
        Dim PartyNr As String
        Dim PartyOwner As Boolean
        Dim PartyType As Short
        Dim PickingItem As Short
        Dim TeleportToX As String
        Dim TeleportToY As String
        Dim TeleportToXs As String
        Dim TeleportToYs As String
        Dim Place As String
        Dim SpBar As Short


        Public Sub Initialize()

            ReDim BuffList(20)

            ReDim MasteryLvl(7)
            ReDim SkillList(300)
            ReDim StallItem(9)
            ReDim StallItemID(9)
            ReDim StallItemInventorySlot(9)
            ReDim StallItemAmount(9)
            ReDim StallItemGold(9)
            ReDim InventorySlot(100)
        End Sub
    End Structure

    Public Structure CharItemData
        Dim Type As String
        Dim amount As Short
        Dim ID As Short
        Dim PlusValue As Short
        Dim Durability As Short
        Dim PhyReinforce As Short
        Dim MagReinforce As Short
    End Structure

    Public Structure MobData
        Dim Type As String
        Dim ID As String
        Dim HP As Integer
        Dim Xps As Integer
        Dim XPos As Double
        Dim YPos As Double
        Dim XSector As Short
        Dim YSector As Short
        Dim mDef As Short
        Dim pDef As Short
        Dim Movestep As Short
        Dim break As Byte
        Dim Save As Byte
        Dim Special As Short
        Dim State As String
        Dim Speed As Short
        Dim Attacking As Boolean
        Dim level As String
        Dim Mobs As Integer
    End Structure

    Public Structure ItemDataMain
        Dim Type As String
        Dim amount As Short
        Dim TypeID As Short
        Dim ID As String
        Dim PlusValue As Short
        Dim Durability As Short
        Dim PhyReinforce As Short
        Dim MagReinforce As Short
        Dim XPos As Double
        Dim YPos As Double
        Dim XSector As Short
        Dim YSector As Short


    End Structure

    Public Structure AvatarItems
        Dim ID As Short

    End Structure

    Public Structure NPCData
        Dim ID As String
        Dim NPCID As String
        Dim ChatId As String
        Dim ids As String
        Dim ShopId As String
        Dim Shoptype As String
        Dim respawnnpc As String
        Dim xy As String
        Dim xx As String
        Dim zz As String
        Dim zone As String
    End Structure



    Public Function InitServer() As Object

        'Print Stuff
        Debug.Print("Server started at: " & TimeValue(CStr(Now)))

        frmLog.Log_Item_Add("Server started at: " & TimeValue(CStr(Now)))

        'The Max Sockets = Max Player on the Server
        iMaxSockets = 500


        'Create arrays and check if one exitis
        If LoginSocket.Count = 0 Then
            LoginSocket.SetIndex(frmMain._LoginSocket_0, ls)
            GameSocket.SetIndex(frmMain._GameSocket_0, gs)

        End If


        'Set RemoteHost (Todo: Read it from File/Database)
        LoginSocket(ls)._RemoteHost = "127.0.0.1"
        GameSocket(gs)._RemoteHost = "127.0.0.1"

        LoginSocket(ls).RemoteHost = "127.0.0.1"
        GameSocket(gs).RemoteHost = "127.0.0.1"

        'Set Port
        LoginSocket(ls).LocalPort = 15779
        GameSocket(gs).LocalPort = 15778


        'Update Labels
        frmMain.lblLoginserver.BackColor = System.Drawing.ColorTranslator.FromOle(&HFF00)
        frmMain.lblGameserver.BackColor = System.Drawing.ColorTranslator.FromOle(&HFF00)
        frmMain.lblLoginserver.Text = "ONLINE"
        frmMain.lblGameserver.Text = "ONLINE"

        frmMain.StartButton.Text = "Started!"
        frmMain.StartButton.Enabled = False

        frmMain.StopButton.Enabled = True

        'Prevents unwanted loop errors.
        ReDim Preserve PlayerData(0)

        ServerStarted = True

    End Function
    Public Function KillServer() As Object

        Debug.Print("Server stopped at: " & TimeValue(CStr(Now)))

        frmLog.Log_Item_Add("Server stopped at: " & TimeValue(CStr(Now)))

        modGlobal.LoginSocket(0).Close()
        modGlobal.GameSocket(0).Close()
        modGlobal.GameSocket(0).LocalPort = 0
        modGlobal.LoginSocket(0).LocalPort = 0
        iMaxSockets = 0

        frmMain.StartButton.Text = "Start"
        frmMain.StartButton.Enabled = True
        frmMain.StopButton.Enabled = False


        frmMain.lblLoginserver.BackColor = System.Drawing.ColorTranslator.FromOle(&HFF)
        frmMain.lblGameserver.BackColor = System.Drawing.ColorTranslator.FromOle(&HFF)
        frmMain.lblLoginserver.Text = "OFFLINE"
        frmMain.lblGameserver.Text = "OFFLINE"

        ServerStarted = False

    End Function

#Region "LoginSocket"

    Private Sub LoginSocket_CloseEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles LoginSocket.CloseEvent
        Dim index As Short = LoginSocket.GetIndex(eventSender)

        LoginSocket.Unload(index)

    End Sub

    Private Sub LoginSocket_ConnectionRequest(ByVal eventSender As System.Object, ByVal eventArgs As AxMSWinsockLib.DMSWinsockControlEvents_ConnectionRequestEvent) Handles LoginSocket.ConnectionRequest
        Dim index As Short = LoginSocket.GetIndex(eventSender)
        Debug.Print("Incoming Connection")
        Dim socket As Short
        socket = FreeLoginSocket()
        LoginSocket.Load(socket)
        LoginSocket(socket).Accept(eventArgs.requestID)
        LoginSocket(socket).SendData(cv_StringFromHex("01000050000001"))


    End Sub

    Private Function FreeLoginSocket() As Short

        For i = 1 To iMaxSockets
            On Error GoTo foundsocket
            'UPGRADE_NOTE: State wurde aktualisiert auf CtlState. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
            If LoginSocket(i).CtlState = 7 Then
            End If
        Next i

foundsocket:
        FreeLoginSocket = i

        Debug.Print("Found free (login)socket: " & i)

        frmLog.Log_Item_Add("Found free (login)socket: " & i)
        Exit Function

    End Function


    Private Sub LoginSocket_DataArrival(ByVal eventSender As System.Object, ByVal eventArgs As AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEvent) Handles LoginSocket.DataArrival
        Dim index As Short = LoginSocket.GetIndex(eventSender)

        Dim sData As String
        Dim dData As String
        sData = "0"
        LoginSocket(index).GetData(sData) ', 'VariantType.String)
        Debug.Print("Punkt 1")
        dData = cv_HexFromString(sData)
        Dim iPacketSize As Short
        iPacketSize = CDbl("&H" & Mid(dData, 3, 2) & Mid(dData, 1, 2)) + 6
        Debug.Print("Punkt 2")
        Dim firstpacket As String
        Dim newPacket As String
        Dim iPackS As Short
        Dim newData As String
        If Len(sData) > iPacketSize Then
            Debug.Print("Punkt 3")
            firstpacket = Mid(sData, 1, iPacketSize)
            ParseLoginData(firstpacket, index)
            newPacket = Replace(sData, firstpacket, "")
            Do While Len(newPacket) > 0
                iPackS = CDbl("&H" & Mid(cv_HexFromString(newPacket), 3, 2) & Mid(cv_HexFromString(newPacket), 1, 2)) + 6
                newData = Mid(newPacket, 1, iPackS)
                ParseLoginData(newData, index)
                newPacket = Replace(newPacket, newData, "")
                Debug.Print("Punkt 4")
            Loop
        Else
            Debug.Print("Punkt 5")
            ParseLoginData(sData, index)
        End If

        Debug.Print("Punkt Fertig")

    End Sub

    Private Sub LoginSocket_Error(ByVal eventSender As System.Object, ByVal eventArgs As AxMSWinsockLib.DMSWinsockControlEvents_ErrorEvent) Handles LoginSocket.Error
        Dim index As Short = LoginSocket.GetIndex(eventSender)

        If eventArgs.Number = 10053 Then
            LoginSocket(index).Close()
            LoginSocket.Unload(index)
        End If


        Debug.Print("Loginsocket " & index & "is closed through an Error!")
        frmLog.Log_Item_Add("Loginsocket " & index & "is closed through an Error!")

    End Sub

#End Region


#Region "GameSocket"

    Private Sub GameSocket_ConnectionRequest(ByVal eventSender As System.Object, ByVal eventArgs As AxMSWinsockLib.DMSWinsockControlEvents_ConnectionRequestEvent) Handles GameSocket.ConnectionRequest
        Dim index As Short = GameSocket.GetIndex(eventSender)

        Dim socket As Short
        socket = FreeGameSocket
        GameSocket.Load(socket)
        GameSocket(socket).Accept(eventArgs.requestID)
        GameSocket(socket).SendData(cv_StringFromHex("01000050000001"))

    End Sub


    Private Function FreeGameSocket() As Short

        For i = 1 To iMaxSockets
            On Error GoTo foundsocket
            'UPGRADE_NOTE: State wurde aktualisiert auf CtlState. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
            If GameSocket(i).CtlState = 7 Then
            End If
        Next i
foundsocket:
        FreeGameSocket = i
        Debug.Print("Found free (game)socket: " & i)

        frmLog.Log_Item_Add("Found free (game)socket: " & i)

        Exit Function

    End Function



    Private Sub GameSocket_DataArrival(ByVal eventSender As System.Object, ByVal eventArgs As AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEvent) Handles GameSocket.DataArrival
        Dim index As Short = GameSocket.GetIndex(eventSender)

        Dim sData As String
        Dim dData As String
        GameSocket(index).GetData(sData)       ', VariantType.String)
        dData = cv_HexFromString(sData)
        Dim iPacketSize As Short
        iPacketSize = CDbl("&H" & Mid(dData, 3, 2) & Mid(dData, 1, 2)) + 6

        Dim firstpacket As String
        Dim newPacket As String
        Dim iPackS As Short
        Dim newData As String
        If Len(sData) > iPacketSize Then
            firstpacket = Mid(sData, 1, iPacketSize)
            ParseGameData(firstpacket, index)
            newPacket = Replace(sData, firstpacket, "")
            Do While Len(newPacket) > 0
                iPackS = CDbl("&H" & Mid(cv_HexFromString(newPacket), 3, 2) & Mid(cv_HexFromString(newPacket), 1, 2)) + 6
                newData = Mid(newPacket, 1, iPackS)
                ParseGameData(newData, index)
                newPacket = Replace(newPacket, newData, "")
            Loop
        Else
            ParseGameData(sData, index)
        End If

    End Sub

    Private Sub GameSocket_Error(ByVal eventSender As System.Object, ByVal eventArgs As AxMSWinsockLib.DMSWinsockControlEvents_ErrorEvent) Handles GameSocket.Error
        Dim index As Short = GameSocket.GetIndex(eventSender)
        Dim Slot As Object

        '10053  - Connection is aborted due to timeout or other failure
        Dim fData As String
        If eventArgs.number = 10053 Then

            'Despawn for all other users.

            fData = "0400AB360000" & PlayerData(index).CharID
            For i = 1 To UBound(PlayerData)

                If i <> index And PlayerData(i).Ingame = True Then
                    GameSocket(i).SendData(cv_StringFromHex(fData))
                End If
            Next i

            'Write our latest known location.

            iniWrite(CStr(PlayerData(index).XSection), "XSection", "character", PlayerData(index).CharPath)
            iniWrite(CStr(PlayerData(index).YSection), "YSection", "character", PlayerData(index).CharPath)
            iniWrite(CStr(PlayerData(index).XPos), "XPos", "character", PlayerData(index).CharPath)

            iniWrite(CStr(PlayerData(index).YPos), "YPos", "character", PlayerData(index).CharPath)
            'Clean up

            PlayerData(index).Ingame = False
            GameSocket(index).Close()
            modGlobal.GameSocket.Unload(index)

            'Should clear items here.

            CharItems(index, Slot).amount = 0
            CharItems(index, Slot).Durability = 0
            CharItems(index, Slot).ID = 0
            CharItems(index, Slot).MagReinforce = 0
            CharItems(index, Slot).PhyReinforce = 0
            CharItems(index, Slot).PlusValue = 0
            CharItems(index, Slot).Type = ""

            'Clean up timers.
            frmMain.CleanTimers(index)

        End If


        Debug.Print("Gamesocket " & index & "is closed through an Error! Character: " & PlayerData(i).Charname)
        frmLog.Log_Item_Add("Gamesocket " & index & "is closed through an Error! Character: " & PlayerData(i).Charname)
    End Sub

#End Region


End Module