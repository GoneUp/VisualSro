Option Strict Off
Option Explicit On
Friend Class frmMain
	Inherits System.Windows.Forms.Form
	Dim i As Short
	Dim RunTime As Integer


    'new Region only for Buttons
#Region "Buttons"


    Private Sub MySqlButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'New MySQl Test
        modDataBase.TestConnection()

    End Sub
    Public Sub BanButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles BanButton.Click
        frmBanPlayer.Show()
    End Sub

    Private Sub AboutButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles AboutButton.Click
        Dim index As Short = AboutButton.GetIndex(eventSender)
        MsgBox("VisualSro is an edited version of SremuX and DSremu (by Lyzerk and manneke)." & vbNewLine & vbNewLine & "The Visual Silkroad Project is Open Scoure, which is developed form GoneUp and manneke." & vbNewLine & vbNewLine & "Enjoy this release!")
    End Sub


    Public Sub ExitMenu_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ExitMenu.Click
        If ServerStarted = True Then
            Debug.Print("Server stopped at: " & TimeValue(CStr(Now)))
            Debug.Print("Programm Closed at: " & TimeValue(CStr(Now)))
        End If
        End
    End Sub

    Public Sub accounts_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Accounts.Click

        frmAccounts.Show()

    End Sub

    Public Sub itembuilder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles itembuilder.Click

        frmItemBuilder.Show()

    End Sub

    Public Sub KickButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles KickButton.Click
        frmKickPlayer.Show()
    End Sub

    Public Sub sndnotice_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles sndnotice.Click
        frmNotice.Show()
    End Sub

    Private Sub StartButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles StartButton.Click
        If (ServerStarted = False) Then
            InitServer()
            LoginSocket(0).Listen()
            GameSocket(0).Listen()
        End If
    End Sub

    Private Sub StopButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles StopButton.Click
        If (ServerStarted = True) Then
            KillServer()
        Else
            MsgBox("Is the Server runned? :P", MsgBoxStyle.Critical)

        End If
    End Sub

    Public Sub UnBanButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles UnBanButton.Click
        frmUnBanPlayer.Show()
    End Sub

    Public Sub uniqlist_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles uniqlist.Click
        MsgBox("TigerGirl - 1954" & vbNewLine & "Cerberus - 23638" & vbNewLine & "Captain Ivy - 23639" & vbNewLine & "Uruchi - 1982" & vbNewLine & "Isyutaru - 2002" & vbNewLine & "Lord Yarkan - 3810" & vbNewLine & "Roc - *Unknown* (3877???)")
    End Sub

    Private Sub Log_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Log.Click

        frmLog.Show()

    End Sub

#End Region

    'Timer Stuff
#Region "Timer Stuff"

    Private Sub Form_Terminate_Renamed()
        If ServerStarted = True Then
            Debug.Print("Server stopped at: " & TimeValue(CStr(Now)))
        End If
    End Sub

    Private Sub AttackDelay_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles AttackDelay.Tick
        Dim index As Short = AttackDelay.GetIndex(eventSender)

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Busy konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Busy = False
        AttackDelay(index).Enabled = False

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).UsingSkill konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).UsingSkill = True Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().UsingSkill konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).UsingSkill = False
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Attacking konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).Attacking = False
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().AttackingID konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PlayerData(index).AttackingID = ""
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Attacking konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf PlayerData(index).Attacking = True Then
            Call NormalAttack(index)
        End If

    End Sub

    Private Sub BerserkTimer_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles BerserkTimer.Tick
        Dim index As Short = BerserkTimer.GetIndex(eventSender)

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts modGlobal.PlayerData(index).BerserkBar konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts modGlobal.PlayerData().BerserkBar konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        modGlobal.PlayerData(index).BerserkBar = modGlobal.PlayerData(index).BerserkBar - 1

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts modGlobal.PlayerData(index).BerserkBar konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If modGlobal.PlayerData(index).BerserkBar <= 0 Then
            Call HandleBerserk(index, False)
        End If

    End Sub

    Private Sub CastingTimer_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CastingTimer.Tick
        Dim index As Short = CastingTimer.GetIndex(eventSender)

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Busy konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Busy = False
        CastingTimer(index).Enabled = False
        Call StartBuff(index)

    End Sub

    Private Sub DeathTimer_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles DeathTimer.Tick
        Dim index As Short = DeathTimer.GetIndex(eventSender)

        Call modMonsters.RemoveDeadMob(index)

    End Sub

    Private Sub mobattack_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mobattack.Tick
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts Mobs(dex).Attacking konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Mobs(dex).Attacking = True Then
            Call MobSendAttack(dex)
        End If
        If mobattack.Enabled = False Then
            MobWalkTimer.Enabled = True
            deger = 0
        End If
    End Sub

    Private Sub MobWalkTimer_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MobWalkTimer.Tick
        For i = 1 To 50
            If MobWalkTimer.Enabled = True Then
                Call MobWalk(i, i, True)
                MobWalkTimer.Interval = 10000
            End If
            If MobWalkTimer.Enabled = False Then
                Call MobWalk(i, i, False)
            End If
        Next i
    End Sub

    Private Sub PickupDelay_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles PickupDelay.Tick
        Dim index As Short = PickupDelay.GetIndex(eventSender)

        Call PickupItem(index)

    End Sub

    ' Private Sub RunTimeTimer_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles RunTimeTimer.Tick
    '    If ServerStarted = True Then
    '       RunTime = RunTime + 60
    '      Debug.Print("Server running: " & TimeString_Renamed(RunTime))
    ' Else
    '    RunTime = 0
    'End If
    'End Sub

    Private Sub tmrCleanup_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrCleanup.Tick
        Dim index As Short = tmrCleanup.GetIndex(eventSender)

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData().Ingame konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PlayerData(index).Ingame = False
        GameSocket(index).Close()
        modGlobal.GameSocket.Unload(index)

        tmrCleanup(index).Enabled = False
        tmrCleanup.Unload(index)

    End Sub

    Private Sub tmrQuit_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrQuit.Tick
        Dim index As Short = tmrQuit.GetIndex(eventSender)

        Dim fData As String
        fData = "00005A310000"
        GameSocket(index).SendData(cv_StringFromHex(fData))
        tmrQuit(index).Enabled = False
        'unload all the players timers
        tmrQuit.Unload(index)
        AttackDelay.Unload(index)
        WalkAttackDelay.Unload(index)
        PickupDelay.Unload(index)
        BerserkTimer.Unload(index)
        CastingTimer.Unload(index)

    End Sub

    Private Sub WalkAttackDelay_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles WalkAttackDelay.Tick
        Dim index As Short = WalkAttackDelay.GetIndex(eventSender)

        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).Attacking konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If PlayerData(index).Attacking = True Then
            Call NormalAttack(index)
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(index).AttackSkillID konnte nicht aufgel?t werden. Klicken Sie hier f? weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ElseIf PlayerData(index).AttackSkillID <> "" Then
            Call AttackSkill(index)
        End If

        WalkAttackDelay(index).Enabled = False

    End Sub

    Public Function CleanTimers(ByRef index As Short) As Object

        On Error Resume Next
        BerserkTimer.Unload(index)
        PickupDelay.Unload(index)
        AttackDelay.Unload(index)
        CastingTimer.Unload(index)
        WalkAttackDelay.Unload(index)
        tmrCleanup.Unload(index)
        tmrQuit.Unload(index)
        DeathTimer.Unload(index)
        'Unload MobWalkTimer(index)
    End Function


#End Region



    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        frmMySQLTestForm.Show()

    End Sub

    Private Sub _AboutButton_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _AboutButton_1.Click

    End Sub
End Class