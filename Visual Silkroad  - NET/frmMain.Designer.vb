<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmMain
#Region "Vom Windows Form-Designer generierter Code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()
	End Sub
	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			Static fTerminateCalled As Boolean
			If Not fTerminateCalled Then
				Form_Terminate_renamed()
				fTerminateCalled = True
			End If
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Wird vom Windows Form-Designer benötigt.
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents Accounts As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents itembuilder As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents uniqlist As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents sndnotice As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents BanButton As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents UnBanButton As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents KickButton As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents tools As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ExitMenu As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	Public WithEvents _GameSocket_0 As AxMSWinsockLib.AxWinsock
	Public WithEvents mobattack As System.Windows.Forms.Timer
	Public WithEvents MobWalkTimer As System.Windows.Forms.Timer
	Public WithEvents _PickupDelay_0 As System.Windows.Forms.Timer
	Public WithEvents _WalkAttackDelay_0 As System.Windows.Forms.Timer
	Public WithEvents _CastingTimer_0 As System.Windows.Forms.Timer
	Public WithEvents _AttackDelay_0 As System.Windows.Forms.Timer
	Public WithEvents _BerserkTimer_0 As System.Windows.Forms.Timer
	Public WithEvents _DeathTimer_0 As System.Windows.Forms.Timer
	Public WithEvents _tmrCleanup_0 As System.Windows.Forms.Timer
	Public WithEvents _tmrQuit_0 As System.Windows.Forms.Timer
	Public WithEvents _LoginSocket_0 As AxMSWinsockLib.AxWinsock
	Public WithEvents RunTimeTimer As System.Windows.Forms.Timer
	Public WithEvents StopButton As System.Windows.Forms.Button
	Public WithEvents _AboutButton_1 As System.Windows.Forms.Button
	Public WithEvents StartButton As System.Windows.Forms.Button
	Public WithEvents capetimer As System.Windows.Forms.Timer
	Public WithEvents MWalkAttackDelay As System.Windows.Forms.Timer
	Public WithEvents lblGameserver As System.Windows.Forms.Label
	Public WithEvents lblLoginserver As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents frmStatus As System.Windows.Forms.GroupBox
	Public WithEvents AboutButton As Microsoft.VisualBasic.Compatibility.VB6.ButtonArray
	Public WithEvents AttackDelay As Microsoft.VisualBasic.Compatibility.VB6.TimerArray
	Public WithEvents BerserkTimer As Microsoft.VisualBasic.Compatibility.VB6.TimerArray
	Public WithEvents CastingTimer As Microsoft.VisualBasic.Compatibility.VB6.TimerArray
    Public WithEvents DeathTimer As Microsoft.VisualBasic.Compatibility.VB6.TimerArray

    'The Sockets
 


	Public WithEvents PickupDelay As Microsoft.VisualBasic.Compatibility.VB6.TimerArray
	Public WithEvents WalkAttackDelay As Microsoft.VisualBasic.Compatibility.VB6.TimerArray
	Public WithEvents tmrCleanup As Microsoft.VisualBasic.Compatibility.VB6.TimerArray
	Public WithEvents tmrQuit As Microsoft.VisualBasic.Compatibility.VB6.TimerArray
	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip
        Me.tools = New System.Windows.Forms.ToolStripMenuItem
        Me.Accounts = New System.Windows.Forms.ToolStripMenuItem
        Me.itembuilder = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.uniqlist = New System.Windows.Forms.ToolStripMenuItem
        Me.sndnotice = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.BanButton = New System.Windows.Forms.ToolStripMenuItem
        Me.UnBanButton = New System.Windows.Forms.ToolStripMenuItem
        Me.KickButton = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.Log = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitMenu = New System.Windows.Forms.ToolStripMenuItem
        Me._GameSocket_0 = New AxMSWinsockLib.AxWinsock
        Me.mobattack = New System.Windows.Forms.Timer(Me.components)
        Me.MobWalkTimer = New System.Windows.Forms.Timer(Me.components)
        Me._PickupDelay_0 = New System.Windows.Forms.Timer(Me.components)
        Me._WalkAttackDelay_0 = New System.Windows.Forms.Timer(Me.components)
        Me._CastingTimer_0 = New System.Windows.Forms.Timer(Me.components)
        Me._AttackDelay_0 = New System.Windows.Forms.Timer(Me.components)
        Me._BerserkTimer_0 = New System.Windows.Forms.Timer(Me.components)
        Me._DeathTimer_0 = New System.Windows.Forms.Timer(Me.components)
        Me._tmrCleanup_0 = New System.Windows.Forms.Timer(Me.components)
        Me._tmrQuit_0 = New System.Windows.Forms.Timer(Me.components)
        Me._LoginSocket_0 = New AxMSWinsockLib.AxWinsock
        Me.frmStatus = New System.Windows.Forms.GroupBox
        Me.StopButton = New System.Windows.Forms.Button
        Me._AboutButton_1 = New System.Windows.Forms.Button
        Me.StartButton = New System.Windows.Forms.Button
        Me.lblGameserver = New System.Windows.Forms.Label
        Me.lblLoginserver = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.RunTimeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.capetimer = New System.Windows.Forms.Timer(Me.components)
        Me.MWalkAttackDelay = New System.Windows.Forms.Timer(Me.components)
        Me.AboutButton = New Microsoft.VisualBasic.Compatibility.VB6.ButtonArray(Me.components)
        Me.AttackDelay = New Microsoft.VisualBasic.Compatibility.VB6.TimerArray(Me.components)
        Me.BerserkTimer = New Microsoft.VisualBasic.Compatibility.VB6.TimerArray(Me.components)
        Me.CastingTimer = New Microsoft.VisualBasic.Compatibility.VB6.TimerArray(Me.components)
        Me.DeathTimer = New Microsoft.VisualBasic.Compatibility.VB6.TimerArray(Me.components)
        Me.PickupDelay = New Microsoft.VisualBasic.Compatibility.VB6.TimerArray(Me.components)
        Me.WalkAttackDelay = New Microsoft.VisualBasic.Compatibility.VB6.TimerArray(Me.components)
        Me.tmrCleanup = New Microsoft.VisualBasic.Compatibility.VB6.TimerArray(Me.components)
        Me.tmrQuit = New Microsoft.VisualBasic.Compatibility.VB6.TimerArray(Me.components)
        Me.AxWinsockArray1 = New VisualSro.AxWinsockArray(Me.components)
        Me.Button1 = New System.Windows.Forms.Button
        Me.MainMenu1.SuspendLayout()
        CType(Me._GameSocket_0, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._LoginSocket_0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.frmStatus.SuspendLayout()
        CType(Me.AboutButton, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AttackDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BerserkTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CastingTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DeathTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PickupDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WalkAttackDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tmrCleanup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tmrQuit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxWinsockArray1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tools, Me.ExitMenu})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(322, 24)
        Me.MainMenu1.TabIndex = 2
        '
        'tools
        '
        Me.tools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Accounts, Me.itembuilder, Me.ToolStripSeparator1, Me.uniqlist, Me.sndnotice, Me.ToolStripSeparator2, Me.BanButton, Me.UnBanButton, Me.KickButton, Me.ToolStripSeparator3, Me.Log})
        Me.tools.Name = "tools"
        Me.tools.Size = New System.Drawing.Size(48, 20)
        Me.tools.Text = "Tools"
        '
        'Accounts
        '
        Me.Accounts.Name = "Accounts"
        Me.Accounts.Size = New System.Drawing.Size(153, 22)
        Me.Accounts.Text = "Account Editor"
        '
        'itembuilder
        '
        Me.itembuilder.Name = "itembuilder"
        Me.itembuilder.Size = New System.Drawing.Size(153, 22)
        Me.itembuilder.Text = "Item Builder"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(150, 6)
        '
        'uniqlist
        '
        Me.uniqlist.Name = "uniqlist"
        Me.uniqlist.Size = New System.Drawing.Size(153, 22)
        Me.uniqlist.Text = "Uniques List"
        '
        'sndnotice
        '
        Me.sndnotice.Name = "sndnotice"
        Me.sndnotice.Size = New System.Drawing.Size(153, 22)
        Me.sndnotice.Text = "Send Notice"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(150, 6)
        '
        'BanButton
        '
        Me.BanButton.Name = "BanButton"
        Me.BanButton.Size = New System.Drawing.Size(153, 22)
        Me.BanButton.Text = "Ban Player"
        '
        'UnBanButton
        '
        Me.UnBanButton.Name = "UnBanButton"
        Me.UnBanButton.Size = New System.Drawing.Size(153, 22)
        Me.UnBanButton.Text = "UnBan Player"
        '
        'KickButton
        '
        Me.KickButton.Name = "KickButton"
        Me.KickButton.Size = New System.Drawing.Size(153, 22)
        Me.KickButton.Text = "Kick Player"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(150, 6)
        '
        'Log
        '
        Me.Log.Name = "Log"
        Me.Log.Size = New System.Drawing.Size(153, 22)
        Me.Log.Text = "Status Log"
        '
        'ExitMenu
        '
        Me.ExitMenu.Name = "ExitMenu"
        Me.ExitMenu.Size = New System.Drawing.Size(37, 20)
        Me.ExitMenu.Text = "Exit"
        '
        '_GameSocket_0
        '
        Me._GameSocket_0.Enabled = True
        Me._GameSocket_0.Location = New System.Drawing.Point(192, 24)
        Me._GameSocket_0.Name = "_GameSocket_0"
        Me._GameSocket_0.OcxState = CType(resources.GetObject("_GameSocket_0.OcxState"), System.Windows.Forms.AxHost.State)
        Me._GameSocket_0.Size = New System.Drawing.Size(28, 28)
        Me._GameSocket_0.TabIndex = 0
        '
        'mobattack
        '
        Me.mobattack.Interval = 2000
        '
        'MobWalkTimer
        '
        Me.MobWalkTimer.Interval = 5000
        '
        '_PickupDelay_0
        '
        Me.PickupDelay.SetIndex(Me._PickupDelay_0, CType(0, Short))
        Me._PickupDelay_0.Interval = 1
        '
        '_WalkAttackDelay_0
        '
        Me.WalkAttackDelay.SetIndex(Me._WalkAttackDelay_0, CType(0, Short))
        Me._WalkAttackDelay_0.Interval = 1
        '
        '_CastingTimer_0
        '
        Me.CastingTimer.SetIndex(Me._CastingTimer_0, CType(0, Short))
        Me._CastingTimer_0.Interval = 5000
        '
        '_AttackDelay_0
        '
        Me.AttackDelay.SetIndex(Me._AttackDelay_0, CType(0, Short))
        Me._AttackDelay_0.Interval = 5000
        '
        '_BerserkTimer_0
        '
        Me.BerserkTimer.SetIndex(Me._BerserkTimer_0, CType(0, Short))
        Me._BerserkTimer_0.Interval = 8000
        '
        '_DeathTimer_0
        '
        Me.DeathTimer.SetIndex(Me._DeathTimer_0, CType(0, Short))
        Me._DeathTimer_0.Interval = 5000
        '
        '_tmrCleanup_0
        '
        Me.tmrCleanup.SetIndex(Me._tmrCleanup_0, CType(0, Short))
        Me._tmrCleanup_0.Interval = 6000
        '
        '_tmrQuit_0
        '
        Me.tmrQuit.SetIndex(Me._tmrQuit_0, CType(0, Short))
        Me._tmrQuit_0.Interval = 5000
        '
        '_LoginSocket_0
        '
        Me._LoginSocket_0.Enabled = True
        Me._LoginSocket_0.Location = New System.Drawing.Point(168, 24)
        Me._LoginSocket_0.Name = "_LoginSocket_0"
        Me._LoginSocket_0.OcxState = CType(resources.GetObject("_LoginSocket_0.OcxState"), System.Windows.Forms.AxHost.State)
        Me._LoginSocket_0.Size = New System.Drawing.Size(28, 28)
        Me._LoginSocket_0.TabIndex = 1
        '
        'frmStatus
        '
        Me.frmStatus.BackColor = System.Drawing.SystemColors.Control
        Me.frmStatus.Controls.Add(Me.Button1)
        Me.frmStatus.Controls.Add(Me.StopButton)
        Me.frmStatus.Controls.Add(Me._AboutButton_1)
        Me.frmStatus.Controls.Add(Me.StartButton)
        Me.frmStatus.Controls.Add(Me.lblGameserver)
        Me.frmStatus.Controls.Add(Me.lblLoginserver)
        Me.frmStatus.Controls.Add(Me.Label2)
        Me.frmStatus.Controls.Add(Me.Label1)
        Me.frmStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frmStatus.Location = New System.Drawing.Point(8, 32)
        Me.frmStatus.Name = "frmStatus"
        Me.frmStatus.Padding = New System.Windows.Forms.Padding(0)
        Me.frmStatus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frmStatus.Size = New System.Drawing.Size(305, 171)
        Me.frmStatus.TabIndex = 0
        Me.frmStatus.TabStop = False
        Me.frmStatus.Text = "Server Status"
        '
        'StopButton
        '
        Me.StopButton.BackColor = System.Drawing.SystemColors.Window
        Me.StopButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.StopButton.Enabled = False
        Me.StopButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.StopButton.Location = New System.Drawing.Point(104, 104)
        Me.StopButton.Name = "StopButton"
        Me.StopButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StopButton.Size = New System.Drawing.Size(89, 33)
        Me.StopButton.TabIndex = 7
        Me.StopButton.Text = "Stop"
        Me.StopButton.UseVisualStyleBackColor = False
        '
        '_AboutButton_1
        '
        Me._AboutButton_1.BackColor = System.Drawing.SystemColors.Window
        Me._AboutButton_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._AboutButton_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AboutButton.SetIndex(Me._AboutButton_1, CType(1, Short))
        Me._AboutButton_1.Location = New System.Drawing.Point(200, 104)
        Me._AboutButton_1.Name = "_AboutButton_1"
        Me._AboutButton_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._AboutButton_1.Size = New System.Drawing.Size(89, 33)
        Me._AboutButton_1.TabIndex = 6
        Me._AboutButton_1.Text = "About"
        Me._AboutButton_1.UseVisualStyleBackColor = False
        '
        'StartButton
        '
        Me.StartButton.BackColor = System.Drawing.SystemColors.Window
        Me.StartButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.StartButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.StartButton.Location = New System.Drawing.Point(8, 104)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartButton.Size = New System.Drawing.Size(89, 33)
        Me.StartButton.TabIndex = 5
        Me.StartButton.Text = "Start"
        Me.StartButton.UseVisualStyleBackColor = False
        '
        'lblGameserver
        '
        Me.lblGameserver.BackColor = System.Drawing.Color.Red
        Me.lblGameserver.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGameserver.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGameserver.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGameserver.Location = New System.Drawing.Point(160, 56)
        Me.lblGameserver.Name = "lblGameserver"
        Me.lblGameserver.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblGameserver.Size = New System.Drawing.Size(121, 25)
        Me.lblGameserver.TabIndex = 4
        Me.lblGameserver.Text = "OFFLINE"
        Me.lblGameserver.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLoginserver
        '
        Me.lblLoginserver.BackColor = System.Drawing.Color.Red
        Me.lblLoginserver.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLoginserver.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoginserver.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLoginserver.Location = New System.Drawing.Point(160, 24)
        Me.lblLoginserver.Name = "lblLoginserver"
        Me.lblLoginserver.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLoginserver.Size = New System.Drawing.Size(121, 25)
        Me.lblLoginserver.TabIndex = 3
        Me.lblLoginserver.Text = "OFFLINE"
        Me.lblLoginserver.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(32, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(81, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Game Server:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(32, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(81, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Login Server:"
        '
        'RunTimeTimer
        '
        Me.RunTimeTimer.Enabled = True
        Me.RunTimeTimer.Interval = 60000
        '
        'capetimer
        '
        Me.capetimer.Interval = 1
        '
        'MWalkAttackDelay
        '
        Me.MWalkAttackDelay.Interval = 1
        '
        'AboutButton
        '
        '
        'AttackDelay
        '
        '
        'BerserkTimer
        '
        '
        'CastingTimer
        '
        '
        'DeathTimer
        '
        '
        'PickupDelay
        '
        '
        'WalkAttackDelay
        '
        '
        'tmrCleanup
        '
        '
        'tmrQuit
        '
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(104, 139)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(89, 32)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "MySQL Stuff"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(322, 217)
        Me.Controls.Add(Me._GameSocket_0)
        Me.Controls.Add(Me._LoginSocket_0)
        Me.Controls.Add(Me.frmStatus)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(677, 335)
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Visual Silkroad"
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        CType(Me._GameSocket_0, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._LoginSocket_0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.frmStatus.ResumeLayout(False)
        CType(Me.AboutButton, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AttackDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BerserkTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CastingTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DeathTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PickupDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WalkAttackDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tmrCleanup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tmrQuit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxWinsockArray1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AxWinsockArray1 As VisualSro.AxWinsockArray
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Log As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button1 As System.Windows.Forms.Button
#End Region
End Class