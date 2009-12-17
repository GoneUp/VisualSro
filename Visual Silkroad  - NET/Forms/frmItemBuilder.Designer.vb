<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemBuilder
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
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmbType As System.Windows.Forms.ComboBox
	Public WithEvents cmbSlot As System.Windows.Forms.ComboBox
	Public WithEvents txtUsername As System.Windows.Forms.TextBox
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents txtID As System.Windows.Forms.TextBox
	Public WithEvents txtDurability As System.Windows.Forms.TextBox
	Public WithEvents txtPlus As System.Windows.Forms.TextBox
	Public WithEvents txtPhysical As System.Windows.Forms.TextBox
	Public WithEvents txtMagical As System.Windows.Forms.TextBox
	Public WithEvents cmdModify As System.Windows.Forms.Button
	Public WithEvents Label11 As System.Windows.Forms.Label
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents Label9 As System.Windows.Forms.Label
	Public WithEvents Label8 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents Line1 As Microsoft.VisualBasic.PowerPacks.LineShape
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemBuilder))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer
        Me.Line1 = New Microsoft.VisualBasic.PowerPacks.LineShape
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.cmbSlot = New System.Windows.Forms.ComboBox
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.txtID = New System.Windows.Forms.TextBox
        Me.txtDurability = New System.Windows.Forms.TextBox
        Me.txtPlus = New System.Windows.Forms.TextBox
        Me.txtPhysical = New System.Windows.Forms.TextBox
        Me.txtMagical = New System.Windows.Forms.TextBox
        Me.cmdModify = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.Line1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(273, 321)
        Me.ShapeContainer1.TabIndex = 22
        Me.ShapeContainer1.TabStop = False
        '
        'Line1
        '
        Me.Line1.BorderColor = System.Drawing.SystemColors.GrayText
        Me.Line1.Name = "Line1"
        Me.Line1.X1 = 8
        Me.Line1.X2 = 264
        Me.Line1.Y1 = 72
        Me.Line1.Y2 = 72
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Enabled = False
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(8, 288)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(257, 25)
        Me.cmdDelete.TabIndex = 21
        Me.cmdDelete.Text = "Please enter a character name."
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmbType
        '
        Me.cmbType.BackColor = System.Drawing.SystemColors.Window
        Me.cmbType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbType.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbType.Items.AddRange(New Object() {"ETC", "CH"})
        Me.cmbType.Location = New System.Drawing.Point(160, 80)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbType.Size = New System.Drawing.Size(105, 21)
        Me.cmbType.TabIndex = 18
        '
        'cmbSlot
        '
        Me.cmbSlot.BackColor = System.Drawing.SystemColors.Window
        Me.cmbSlot.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmbSlot.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbSlot.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44"})
        Me.cmbSlot.Location = New System.Drawing.Point(200, 48)
        Me.cmbSlot.Name = "cmbSlot"
        Me.cmbSlot.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmbSlot.Size = New System.Drawing.Size(65, 21)
        Me.cmbSlot.TabIndex = 15
        '
        'txtUsername
        '
        Me.txtUsername.AcceptsReturn = True
        Me.txtUsername.BackColor = System.Drawing.SystemColors.Window
        Me.txtUsername.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtUsername.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsername.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUsername.Location = New System.Drawing.Point(72, 48)
        Me.txtUsername.MaxLength = 0
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtUsername.Size = New System.Drawing.Size(81, 21)
        Me.txtUsername.TabIndex = 14
        Me.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(160, 128)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(105, 21)
        Me.txtAmount.TabIndex = 12
        Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtID
        '
        Me.txtID.AcceptsReturn = True
        Me.txtID.BackColor = System.Drawing.SystemColors.Window
        Me.txtID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtID.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtID.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtID.Location = New System.Drawing.Point(160, 104)
        Me.txtID.MaxLength = 0
        Me.txtID.Name = "txtID"
        Me.txtID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtID.Size = New System.Drawing.Size(105, 21)
        Me.txtID.TabIndex = 11
        Me.txtID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDurability
        '
        Me.txtDurability.AcceptsReturn = True
        Me.txtDurability.BackColor = System.Drawing.SystemColors.Window
        Me.txtDurability.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDurability.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDurability.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDurability.Location = New System.Drawing.Point(160, 152)
        Me.txtDurability.MaxLength = 0
        Me.txtDurability.Name = "txtDurability"
        Me.txtDurability.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDurability.Size = New System.Drawing.Size(105, 21)
        Me.txtDurability.TabIndex = 10
        Me.txtDurability.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPlus
        '
        Me.txtPlus.AcceptsReturn = True
        Me.txtPlus.BackColor = System.Drawing.SystemColors.Window
        Me.txtPlus.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPlus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPlus.Location = New System.Drawing.Point(160, 176)
        Me.txtPlus.MaxLength = 0
        Me.txtPlus.Name = "txtPlus"
        Me.txtPlus.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPlus.Size = New System.Drawing.Size(105, 21)
        Me.txtPlus.TabIndex = 9
        Me.txtPlus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPhysical
        '
        Me.txtPhysical.AcceptsReturn = True
        Me.txtPhysical.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhysical.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPhysical.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhysical.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhysical.Location = New System.Drawing.Point(160, 200)
        Me.txtPhysical.MaxLength = 0
        Me.txtPhysical.Name = "txtPhysical"
        Me.txtPhysical.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPhysical.Size = New System.Drawing.Size(105, 21)
        Me.txtPhysical.TabIndex = 8
        Me.txtPhysical.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMagical
        '
        Me.txtMagical.AcceptsReturn = True
        Me.txtMagical.BackColor = System.Drawing.SystemColors.Window
        Me.txtMagical.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMagical.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMagical.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMagical.Location = New System.Drawing.Point(160, 224)
        Me.txtMagical.MaxLength = 0
        Me.txtMagical.Name = "txtMagical"
        Me.txtMagical.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMagical.Size = New System.Drawing.Size(105, 21)
        Me.txtMagical.TabIndex = 7
        Me.txtMagical.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdModify
        '
        Me.cmdModify.BackColor = System.Drawing.SystemColors.Control
        Me.cmdModify.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdModify.Enabled = False
        Me.cmdModify.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdModify.Location = New System.Drawing.Point(8, 256)
        Me.cmdModify.Name = "cmdModify"
        Me.cmdModify.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdModify.Size = New System.Drawing.Size(257, 25)
        Me.cmdModify.TabIndex = 0
        Me.cmdModify.Text = "Please enter a character name."
        Me.cmdModify.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(8, 24)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(257, 17)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "ETC Type: Requires ID/Amount"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(8, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(257, 17)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "CH Type: Requires ID/Durability/Plus Value/Phy/Mag"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(8, 84)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(137, 17)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Item type:"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(160, 50)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(41, 17)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Slot:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(65, 17)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Username:"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(8, 227)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(145, 17)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Magical reinforcement %:"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(8, 204)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(161, 17)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Physical reinforcement %:"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(8, 179)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(129, 17)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Plus value:"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(8, 155)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(121, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Durability:"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(8, 106)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(121, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Item ID:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 131)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(121, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Amount:"
        '
        'frmItemBuilder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(273, 321)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmbType)
        Me.Controls.Add(Me.cmbSlot)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.txtDurability)
        Me.Controls.Add(Me.txtPlus)
        Me.Controls.Add(Me.txtPhysical)
        Me.Controls.Add(Me.txtMagical)
        Me.Controls.Add(Me.cmdModify)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 29)
        Me.MaximizeBox = False
        Me.Name = "frmItemBuilder"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Itembuilder"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class