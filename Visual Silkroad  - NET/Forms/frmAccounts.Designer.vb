<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmAccounts
#Region "Vom Windows Form-Designer generierter Code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()
	End Sub
	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Wird vom Windows Form-Designer benötigt.
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents txtAttributePoints As System.Windows.Forms.TextBox
	Public WithEvents txtRunSpeed As System.Windows.Forms.TextBox
	Public WithEvents txtBerserkSpeed As System.Windows.Forms.TextBox
	Public WithEvents txtWalkSpeed As System.Windows.Forms.TextBox
	Public WithEvents txtBerserkBar As System.Windows.Forms.TextBox
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents chkGM As System.Windows.Forms.CheckBox
	Public WithEvents txtParry As System.Windows.Forms.TextBox
	Public WithEvents cmbChartype As System.Windows.Forms.ComboBox
	Public WithEvents txtHit As System.Windows.Forms.TextBox
	Public WithEvents txtMagDef As System.Windows.Forms.TextBox
	Public WithEvents txtPhyDef As System.Windows.Forms.TextBox
	Public WithEvents txtMaxMagAtk As System.Windows.Forms.TextBox
	Public WithEvents txtMinMagAtk As System.Windows.Forms.TextBox
	Public WithEvents txtMaxPhyAtk As System.Windows.Forms.TextBox
	Public WithEvents txtMinPhyAtk As System.Windows.Forms.TextBox
	Public WithEvents txtMP As System.Windows.Forms.TextBox
	Public WithEvents txtHP As System.Windows.Forms.TextBox
	Public WithEvents txtSP As System.Windows.Forms.TextBox
	Public WithEvents txtIntelligence As System.Windows.Forms.TextBox
	Public WithEvents txtStrength As System.Windows.Forms.TextBox
	Public WithEvents txtLevel As System.Windows.Forms.TextBox
	Public WithEvents txtVolume As System.Windows.Forms.TextBox
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents txtCharname As System.Windows.Forms.TextBox
	Public WithEvents cmdGo As System.Windows.Forms.Button
	Public WithEvents txtAccount As System.Windows.Forms.TextBox
	Public WithEvents Label22 As System.Windows.Forms.Label
	Public WithEvents Label21 As System.Windows.Forms.Label
	Public WithEvents Label20 As System.Windows.Forms.Label
	Public WithEvents Label19 As System.Windows.Forms.Label
	Public WithEvents Label18 As System.Windows.Forms.Label
	Public WithEvents lblParry As System.Windows.Forms.Label
	Public WithEvents Label17 As System.Windows.Forms.Label
	Public WithEvents Label16 As System.Windows.Forms.Label
	Public WithEvents Label15 As System.Windows.Forms.Label
	Public WithEvents Label14 As System.Windows.Forms.Label
	Public WithEvents Label13 As System.Windows.Forms.Label
	Public WithEvents Label12 As System.Windows.Forms.Label
	Public WithEvents Label11 As System.Windows.Forms.Label
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents Label9 As System.Windows.Forms.Label
	Public WithEvents Label8 As System.Windows.Forms.Label
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblUsername As System.Windows.Forms.Label
	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAccounts))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtAttributePoints = New System.Windows.Forms.TextBox
        Me.txtRunSpeed = New System.Windows.Forms.TextBox
        Me.txtBerserkSpeed = New System.Windows.Forms.TextBox
        Me.txtWalkSpeed = New System.Windows.Forms.TextBox
        Me.txtBerserkBar = New System.Windows.Forms.TextBox
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.chkGM = New System.Windows.Forms.CheckBox
        Me.txtParry = New System.Windows.Forms.TextBox
        Me.cmbChartype = New System.Windows.Forms.ComboBox
        Me.txtHit = New System.Windows.Forms.TextBox
        Me.txtMagDef = New System.Windows.Forms.TextBox
        Me.txtPhyDef = New System.Windows.Forms.TextBox
        Me.txtMaxMagAtk = New System.Windows.Forms.TextBox
        Me.txtMinMagAtk = New System.Windows.Forms.TextBox
        Me.txtMaxPhyAtk = New System.Windows.Forms.TextBox
        Me.txtMinPhyAtk = New System.Windows.Forms.TextBox
        Me.txtMP = New System.Windows.Forms.TextBox
        Me.txtHP = New System.Windows.Forms.TextBox
        Me.txtSP = New System.Windows.Forms.TextBox
        Me.txtIntelligence = New System.Windows.Forms.TextBox
        Me.txtStrength = New System.Windows.Forms.TextBox
        Me.txtLevel = New System.Windows.Forms.TextBox
        Me.txtVolume = New System.Windows.Forms.TextBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtCharname = New System.Windows.Forms.TextBox
        Me.cmdGo = New System.Windows.Forms.Button
        Me.txtAccount = New System.Windows.Forms.TextBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.lblParry = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblUsername = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtAttributePoints
        '
        Me.txtAttributePoints.AcceptsReturn = True
        Me.txtAttributePoints.BackColor = System.Drawing.SystemColors.Window
        Me.txtAttributePoints.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAttributePoints.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAttributePoints.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAttributePoints.Location = New System.Drawing.Point(312, 272)
        Me.txtAttributePoints.MaxLength = 0
        Me.txtAttributePoints.Name = "txtAttributePoints"
        Me.txtAttributePoints.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAttributePoints.Size = New System.Drawing.Size(129, 21)
        Me.txtAttributePoints.TabIndex = 45
        Me.txtAttributePoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtRunSpeed
        '
        Me.txtRunSpeed.AcceptsReturn = True
        Me.txtRunSpeed.BackColor = System.Drawing.SystemColors.Window
        Me.txtRunSpeed.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRunSpeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRunSpeed.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRunSpeed.Location = New System.Drawing.Point(88, 272)
        Me.txtRunSpeed.MaxLength = 0
        Me.txtRunSpeed.Name = "txtRunSpeed"
        Me.txtRunSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRunSpeed.Size = New System.Drawing.Size(129, 21)
        Me.txtRunSpeed.TabIndex = 44
        Me.txtRunSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtBerserkSpeed
        '
        Me.txtBerserkSpeed.AcceptsReturn = True
        Me.txtBerserkSpeed.BackColor = System.Drawing.SystemColors.Window
        Me.txtBerserkSpeed.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBerserkSpeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBerserkSpeed.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBerserkSpeed.Location = New System.Drawing.Point(312, 248)
        Me.txtBerserkSpeed.MaxLength = 0
        Me.txtBerserkSpeed.Name = "txtBerserkSpeed"
        Me.txtBerserkSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBerserkSpeed.Size = New System.Drawing.Size(129, 21)
        Me.txtBerserkSpeed.TabIndex = 43
        Me.txtBerserkSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtWalkSpeed
        '
        Me.txtWalkSpeed.AcceptsReturn = True
        Me.txtWalkSpeed.BackColor = System.Drawing.SystemColors.Window
        Me.txtWalkSpeed.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWalkSpeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWalkSpeed.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWalkSpeed.Location = New System.Drawing.Point(88, 248)
        Me.txtWalkSpeed.MaxLength = 0
        Me.txtWalkSpeed.Name = "txtWalkSpeed"
        Me.txtWalkSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWalkSpeed.Size = New System.Drawing.Size(129, 21)
        Me.txtWalkSpeed.TabIndex = 42
        Me.txtWalkSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtBerserkBar
        '
        Me.txtBerserkBar.AcceptsReturn = True
        Me.txtBerserkBar.BackColor = System.Drawing.SystemColors.Window
        Me.txtBerserkBar.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBerserkBar.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBerserkBar.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBerserkBar.Location = New System.Drawing.Point(88, 224)
        Me.txtBerserkBar.MaxLength = 0
        Me.txtBerserkBar.Name = "txtBerserkBar"
        Me.txtBerserkBar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBerserkBar.Size = New System.Drawing.Size(129, 21)
        Me.txtBerserkBar.TabIndex = 41
        Me.txtBerserkBar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = False
        Me.cmdDelete.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(8, 320)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(209, 25)
        Me.cmdDelete.TabIndex = 40
        Me.cmdDelete.Text = "Delete account"
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'chkGM
        '
        Me.chkGM.BackColor = System.Drawing.SystemColors.Control
        Me.chkGM.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkGM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkGM.Location = New System.Drawing.Point(8, 296)
        Me.chkGM.Name = "chkGM"
        Me.chkGM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkGM.Size = New System.Drawing.Size(441, 13)
        Me.chkGM.TabIndex = 39
        Me.chkGM.Text = "Check to enable GM Mode"
        Me.chkGM.UseVisualStyleBackColor = False
        '
        'txtParry
        '
        Me.txtParry.AcceptsReturn = True
        Me.txtParry.BackColor = System.Drawing.SystemColors.Window
        Me.txtParry.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtParry.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtParry.Location = New System.Drawing.Point(312, 224)
        Me.txtParry.MaxLength = 0
        Me.txtParry.Name = "txtParry"
        Me.txtParry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtParry.Size = New System.Drawing.Size(129, 21)
        Me.txtParry.TabIndex = 37
        Me.txtParry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbChartype
        '
        Me.cmbChartype.BackColor = System.Drawing.SystemColors.Window
        Me.cmbChartype.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbChartype.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbChartype.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbChartype.Items.AddRange(New Object() {"1907", "1908", "1909", "1910", "1911", "1912", "1913", "1914", "1915", "1916", "1917", "1918", "1919", "1920", "1921", "1922", "1923", "1924", "1925", "1926", "1927", "1928", "1929", "1930", "1931", "1932", "14717", "14718", "14719", "14720", "14721", "14722", "14723", "14724", "14725", "14726", "14727", "14728", "14729", "14730", "14731", "14732", "14733", "14734", "14735", "14736", "14737", "14738", "14739", "14740", "14741", "14742"})
        Me.cmbChartype.Location = New System.Drawing.Point(88, 80)
        Me.cmbChartype.Name = "cmbChartype"
        Me.cmbChartype.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbChartype.Size = New System.Drawing.Size(129, 21)
        Me.cmbChartype.TabIndex = 36
        '
        'txtHit
        '
        Me.txtHit.AcceptsReturn = True
        Me.txtHit.BackColor = System.Drawing.SystemColors.Window
        Me.txtHit.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHit.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHit.Location = New System.Drawing.Point(312, 200)
        Me.txtHit.MaxLength = 0
        Me.txtHit.Name = "txtHit"
        Me.txtHit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHit.Size = New System.Drawing.Size(129, 21)
        Me.txtHit.TabIndex = 18
        Me.txtHit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMagDef
        '
        Me.txtMagDef.AcceptsReturn = True
        Me.txtMagDef.BackColor = System.Drawing.SystemColors.Window
        Me.txtMagDef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMagDef.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMagDef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMagDef.Location = New System.Drawing.Point(312, 176)
        Me.txtMagDef.MaxLength = 0
        Me.txtMagDef.Name = "txtMagDef"
        Me.txtMagDef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMagDef.Size = New System.Drawing.Size(129, 21)
        Me.txtMagDef.TabIndex = 17
        Me.txtMagDef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPhyDef
        '
        Me.txtPhyDef.AcceptsReturn = True
        Me.txtPhyDef.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhyDef.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhyDef.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhyDef.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhyDef.Location = New System.Drawing.Point(312, 152)
        Me.txtPhyDef.MaxLength = 0
        Me.txtPhyDef.Name = "txtPhyDef"
        Me.txtPhyDef.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhyDef.Size = New System.Drawing.Size(129, 21)
        Me.txtPhyDef.TabIndex = 16
        Me.txtPhyDef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMaxMagAtk
        '
        Me.txtMaxMagAtk.AcceptsReturn = True
        Me.txtMaxMagAtk.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaxMagAtk.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxMagAtk.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxMagAtk.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxMagAtk.Location = New System.Drawing.Point(312, 128)
        Me.txtMaxMagAtk.MaxLength = 0
        Me.txtMaxMagAtk.Name = "txtMaxMagAtk"
        Me.txtMaxMagAtk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxMagAtk.Size = New System.Drawing.Size(129, 21)
        Me.txtMaxMagAtk.TabIndex = 15
        Me.txtMaxMagAtk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMinMagAtk
        '
        Me.txtMinMagAtk.AcceptsReturn = True
        Me.txtMinMagAtk.BackColor = System.Drawing.SystemColors.Window
        Me.txtMinMagAtk.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinMagAtk.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinMagAtk.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMinMagAtk.Location = New System.Drawing.Point(312, 104)
        Me.txtMinMagAtk.MaxLength = 0
        Me.txtMinMagAtk.Name = "txtMinMagAtk"
        Me.txtMinMagAtk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinMagAtk.Size = New System.Drawing.Size(129, 21)
        Me.txtMinMagAtk.TabIndex = 14
        Me.txtMinMagAtk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMaxPhyAtk
        '
        Me.txtMaxPhyAtk.AcceptsReturn = True
        Me.txtMaxPhyAtk.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaxPhyAtk.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxPhyAtk.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxPhyAtk.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxPhyAtk.Location = New System.Drawing.Point(312, 80)
        Me.txtMaxPhyAtk.MaxLength = 0
        Me.txtMaxPhyAtk.Name = "txtMaxPhyAtk"
        Me.txtMaxPhyAtk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxPhyAtk.Size = New System.Drawing.Size(129, 21)
        Me.txtMaxPhyAtk.TabIndex = 13
        Me.txtMaxPhyAtk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMinPhyAtk
        '
        Me.txtMinPhyAtk.AcceptsReturn = True
        Me.txtMinPhyAtk.BackColor = System.Drawing.SystemColors.Window
        Me.txtMinPhyAtk.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinPhyAtk.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinPhyAtk.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMinPhyAtk.Location = New System.Drawing.Point(312, 56)
        Me.txtMinPhyAtk.MaxLength = 0
        Me.txtMinPhyAtk.Name = "txtMinPhyAtk"
        Me.txtMinPhyAtk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinPhyAtk.Size = New System.Drawing.Size(129, 21)
        Me.txtMinPhyAtk.TabIndex = 12
        Me.txtMinPhyAtk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMP
        '
        Me.txtMP.AcceptsReturn = True
        Me.txtMP.BackColor = System.Drawing.SystemColors.Window
        Me.txtMP.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMP.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMP.Location = New System.Drawing.Point(312, 32)
        Me.txtMP.MaxLength = 0
        Me.txtMP.Name = "txtMP"
        Me.txtMP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMP.Size = New System.Drawing.Size(129, 21)
        Me.txtMP.TabIndex = 11
        Me.txtMP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtHP
        '
        Me.txtHP.AcceptsReturn = True
        Me.txtHP.BackColor = System.Drawing.SystemColors.Window
        Me.txtHP.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHP.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHP.Location = New System.Drawing.Point(312, 8)
        Me.txtHP.MaxLength = 0
        Me.txtHP.Name = "txtHP"
        Me.txtHP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHP.Size = New System.Drawing.Size(129, 21)
        Me.txtHP.TabIndex = 10
        Me.txtHP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSP
        '
        Me.txtSP.AcceptsReturn = True
        Me.txtSP.BackColor = System.Drawing.SystemColors.Window
        Me.txtSP.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSP.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSP.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSP.Location = New System.Drawing.Point(88, 200)
        Me.txtSP.MaxLength = 0
        Me.txtSP.Name = "txtSP"
        Me.txtSP.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSP.Size = New System.Drawing.Size(129, 21)
        Me.txtSP.TabIndex = 9
        Me.txtSP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtIntelligence
        '
        Me.txtIntelligence.AcceptsReturn = True
        Me.txtIntelligence.BackColor = System.Drawing.SystemColors.Window
        Me.txtIntelligence.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtIntelligence.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIntelligence.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIntelligence.Location = New System.Drawing.Point(88, 176)
        Me.txtIntelligence.MaxLength = 0
        Me.txtIntelligence.Name = "txtIntelligence"
        Me.txtIntelligence.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtIntelligence.Size = New System.Drawing.Size(129, 21)
        Me.txtIntelligence.TabIndex = 8
        Me.txtIntelligence.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtStrength
        '
        Me.txtStrength.AcceptsReturn = True
        Me.txtStrength.BackColor = System.Drawing.SystemColors.Window
        Me.txtStrength.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStrength.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStrength.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStrength.Location = New System.Drawing.Point(88, 152)
        Me.txtStrength.MaxLength = 0
        Me.txtStrength.Name = "txtStrength"
        Me.txtStrength.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStrength.Size = New System.Drawing.Size(129, 21)
        Me.txtStrength.TabIndex = 7
        Me.txtStrength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtLevel
        '
        Me.txtLevel.AcceptsReturn = True
        Me.txtLevel.BackColor = System.Drawing.SystemColors.Window
        Me.txtLevel.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLevel.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLevel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLevel.Location = New System.Drawing.Point(88, 128)
        Me.txtLevel.MaxLength = 0
        Me.txtLevel.Name = "txtLevel"
        Me.txtLevel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLevel.Size = New System.Drawing.Size(129, 21)
        Me.txtLevel.TabIndex = 6
        Me.txtLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtVolume
        '
        Me.txtVolume.AcceptsReturn = True
        Me.txtVolume.BackColor = System.Drawing.SystemColors.Window
        Me.txtVolume.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVolume.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVolume.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVolume.Location = New System.Drawing.Point(88, 104)
        Me.txtVolume.MaxLength = 0
        Me.txtVolume.Name = "txtVolume"
        Me.txtVolume.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVolume.Size = New System.Drawing.Size(129, 21)
        Me.txtVolume.TabIndex = 5
        Me.txtVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPassword
        '
        Me.txtPassword.AcceptsReturn = True
        Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPassword.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPassword.Location = New System.Drawing.Point(88, 56)
        Me.txtPassword.MaxLength = 0
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPassword.Size = New System.Drawing.Size(129, 21)
        Me.txtPassword.TabIndex = 4
        Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtCharname
        '
        Me.txtCharname.AcceptsReturn = True
        Me.txtCharname.BackColor = System.Drawing.SystemColors.Window
        Me.txtCharname.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCharname.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCharname.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCharname.Location = New System.Drawing.Point(88, 32)
        Me.txtCharname.MaxLength = 0
        Me.txtCharname.Name = "txtCharname"
        Me.txtCharname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCharname.Size = New System.Drawing.Size(129, 21)
        Me.txtCharname.TabIndex = 3
        Me.txtCharname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdGo
        '
        Me.cmdGo.BackColor = System.Drawing.SystemColors.Control
        Me.cmdGo.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdGo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdGo.Location = New System.Drawing.Point(232, 320)
        Me.cmdGo.Name = "cmdGo"
        Me.cmdGo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdGo.Size = New System.Drawing.Size(209, 25)
        Me.cmdGo.TabIndex = 2
        Me.cmdGo.Text = "Create account"
        Me.cmdGo.UseVisualStyleBackColor = False
        '
        'txtAccount
        '
        Me.txtAccount.AcceptsReturn = True
        Me.txtAccount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccount.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccount.Location = New System.Drawing.Point(88, 8)
        Me.txtAccount.MaxLength = 0
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccount.Size = New System.Drawing.Size(129, 21)
        Me.txtAccount.TabIndex = 0
        Me.txtAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label22
        '
        Me.Label22.BackColor = System.Drawing.SystemColors.Control
        Me.Label22.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label22.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label22.Location = New System.Drawing.Point(224, 272)
        Me.Label22.Name = "Label22"
        Me.Label22.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label22.Size = New System.Drawing.Size(81, 17)
        Me.Label22.TabIndex = 50
        Me.Label22.Text = "Attr. points:"
        '
        'Label21
        '
        Me.Label21.BackColor = System.Drawing.SystemColors.Control
        Me.Label21.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label21.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label21.Location = New System.Drawing.Point(224, 248)
        Me.Label21.Name = "Label21"
        Me.Label21.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label21.Size = New System.Drawing.Size(89, 17)
        Me.Label21.TabIndex = 49
        Me.Label21.Text = "Berserk speed:"
        '
        'Label20
        '
        Me.Label20.BackColor = System.Drawing.SystemColors.Control
        Me.Label20.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label20.Location = New System.Drawing.Point(8, 272)
        Me.Label20.Name = "Label20"
        Me.Label20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label20.Size = New System.Drawing.Size(73, 17)
        Me.Label20.TabIndex = 48
        Me.Label20.Text = "Run speed:"
        '
        'Label19
        '
        Me.Label19.BackColor = System.Drawing.SystemColors.Control
        Me.Label19.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label19.Location = New System.Drawing.Point(8, 248)
        Me.Label19.Name = "Label19"
        Me.Label19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label19.Size = New System.Drawing.Size(81, 17)
        Me.Label19.TabIndex = 47
        Me.Label19.Text = "Walk speed:"
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.SystemColors.Control
        Me.Label18.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label18.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label18.Location = New System.Drawing.Point(8, 224)
        Me.Label18.Name = "Label18"
        Me.Label18.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label18.Size = New System.Drawing.Size(81, 17)
        Me.Label18.TabIndex = 46
        Me.Label18.Text = "Berserk #:"
        '
        'lblParry
        '
        Me.lblParry.BackColor = System.Drawing.SystemColors.Control
        Me.lblParry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblParry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParry.Location = New System.Drawing.Point(224, 224)
        Me.lblParry.Name = "lblParry"
        Me.lblParry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblParry.Size = New System.Drawing.Size(81, 17)
        Me.lblParry.TabIndex = 38
        Me.lblParry.Text = "Parry rating:"
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.SystemColors.Control
        Me.Label17.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label17.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label17.Location = New System.Drawing.Point(224, 200)
        Me.Label17.Name = "Label17"
        Me.Label17.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label17.Size = New System.Drawing.Size(73, 17)
        Me.Label17.TabIndex = 35
        Me.Label17.Text = "Hit rating:"
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.SystemColors.Control
        Me.Label16.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label16.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label16.Location = New System.Drawing.Point(224, 176)
        Me.Label16.Name = "Label16"
        Me.Label16.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label16.Size = New System.Drawing.Size(81, 17)
        Me.Label16.TabIndex = 34
        Me.Label16.Text = "Mag. Defense:"
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.SystemColors.Control
        Me.Label15.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label15.Location = New System.Drawing.Point(224, 152)
        Me.Label15.Name = "Label15"
        Me.Label15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label15.Size = New System.Drawing.Size(81, 17)
        Me.Label15.TabIndex = 33
        Me.Label15.Text = "Phy. Defense:"
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.SystemColors.Control
        Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label14.Location = New System.Drawing.Point(224, 128)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(89, 17)
        Me.Label14.TabIndex = 32
        Me.Label14.Text = "Max. Mag. Atk."
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.SystemColors.Control
        Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(224, 104)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(81, 17)
        Me.Label13.TabIndex = 31
        Me.Label13.Text = "Min. Mag. Atk."
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(224, 80)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(81, 17)
        Me.Label12.TabIndex = 30
        Me.Label12.Text = "Max. Phy. Atk."
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(224, 56)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(81, 17)
        Me.Label11.TabIndex = 29
        Me.Label11.Text = "Min. Phy. Atk."
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(224, 32)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(81, 17)
        Me.Label10.TabIndex = 28
        Me.Label10.Text = "Mana points:"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(224, 8)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(81, 17)
        Me.Label9.TabIndex = 27
        Me.Label9.Text = "Health points:"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(8, 200)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(73, 17)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "Skillpoints:"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(8, 176)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(81, 17)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Intelligence:"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(8, 152)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(81, 17)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "Strength:"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(8, 128)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(81, 17)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "Level:"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(8, 104)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(81, 25)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Volume:"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(8, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(81, 17)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "Char. Type:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(81, 17)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "Password:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(73, 17)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Char. name:"
        '
        'lblUsername
        '
        Me.lblUsername.BackColor = System.Drawing.SystemColors.Control
        Me.lblUsername.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblUsername.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUsername.Location = New System.Drawing.Point(8, 10)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUsername.Size = New System.Drawing.Size(73, 17)
        Me.lblUsername.TabIndex = 1
        Me.lblUsername.Text = "Username:"
        '
        'frmAccounts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(449, 355)
        Me.Controls.Add(Me.txtAttributePoints)
        Me.Controls.Add(Me.txtRunSpeed)
        Me.Controls.Add(Me.txtBerserkSpeed)
        Me.Controls.Add(Me.txtWalkSpeed)
        Me.Controls.Add(Me.txtBerserkBar)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.chkGM)
        Me.Controls.Add(Me.txtParry)
        Me.Controls.Add(Me.cmbChartype)
        Me.Controls.Add(Me.txtHit)
        Me.Controls.Add(Me.txtMagDef)
        Me.Controls.Add(Me.txtPhyDef)
        Me.Controls.Add(Me.txtMaxMagAtk)
        Me.Controls.Add(Me.txtMinMagAtk)
        Me.Controls.Add(Me.txtMaxPhyAtk)
        Me.Controls.Add(Me.txtMinPhyAtk)
        Me.Controls.Add(Me.txtMP)
        Me.Controls.Add(Me.txtHP)
        Me.Controls.Add(Me.txtSP)
        Me.Controls.Add(Me.txtIntelligence)
        Me.Controls.Add(Me.txtStrength)
        Me.Controls.Add(Me.txtLevel)
        Me.Controls.Add(Me.txtVolume)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtCharname)
        Me.Controls.Add(Me.cmdGo)
        Me.Controls.Add(Me.txtAccount)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.lblParry)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblUsername)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.Name = "frmAccounts"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Account Editor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class