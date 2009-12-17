<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class Converter
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
	Public WithEvents Command1 As System.Windows.Forms.Button
	Public WithEvents Text1 As System.Windows.Forms.TextBox
	Public WithEvents y As System.Windows.Forms.TextBox
	Public WithEvents x As System.Windows.Forms.TextBox
	Public WithEvents xsec As System.Windows.Forms.TextBox
	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Converter))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.Command1 = New System.Windows.Forms.Button
		Me.Text1 = New System.Windows.Forms.TextBox
		Me.y = New System.Windows.Forms.TextBox
		Me.x = New System.Windows.Forms.TextBox
		Me.xsec = New System.Windows.Forms.TextBox
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.Text = "Form1"
		Me.ClientSize = New System.Drawing.Size(466, 184)
		Me.Location = New System.Drawing.Point(4, 30)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "Converter"
		Me.Command1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.Command1.Text = "Command1"
		Me.Command1.Size = New System.Drawing.Size(81, 89)
		Me.Command1.Location = New System.Drawing.Point(352, 32)
		Me.Command1.TabIndex = 4
		Me.Command1.BackColor = System.Drawing.SystemColors.Control
		Me.Command1.CausesValidation = True
		Me.Command1.Enabled = True
		Me.Command1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Command1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Command1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Command1.TabStop = True
		Me.Command1.Name = "Command1"
		Me.Text1.AutoSize = False
		Me.Text1.Size = New System.Drawing.Size(313, 25)
		Me.Text1.Location = New System.Drawing.Point(16, 96)
		Me.Text1.TabIndex = 3
		Me.Text1.Text = "Text1"
		Me.Text1.AcceptsReturn = True
		Me.Text1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.Text1.BackColor = System.Drawing.SystemColors.Window
		Me.Text1.CausesValidation = True
		Me.Text1.Enabled = True
		Me.Text1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.Text1.HideSelection = True
		Me.Text1.ReadOnly = False
		Me.Text1.Maxlength = 0
		Me.Text1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.Text1.MultiLine = False
		Me.Text1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Text1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.Text1.TabStop = True
		Me.Text1.Visible = True
		Me.Text1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Text1.Name = "Text1"
		Me.y.AutoSize = False
		Me.y.Size = New System.Drawing.Size(81, 25)
		Me.y.Location = New System.Drawing.Point(248, 32)
		Me.y.TabIndex = 2
		Me.y.Text = "Text1"
		Me.y.AcceptsReturn = True
		Me.y.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.y.BackColor = System.Drawing.SystemColors.Window
		Me.y.CausesValidation = True
		Me.y.Enabled = True
		Me.y.ForeColor = System.Drawing.SystemColors.WindowText
		Me.y.HideSelection = True
		Me.y.ReadOnly = False
		Me.y.Maxlength = 0
		Me.y.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.y.MultiLine = False
		Me.y.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.y.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.y.TabStop = True
		Me.y.Visible = True
		Me.y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.y.Name = "y"
		Me.x.AutoSize = False
		Me.x.Size = New System.Drawing.Size(89, 25)
		Me.x.Location = New System.Drawing.Point(136, 32)
		Me.x.TabIndex = 1
		Me.x.Text = "Text1"
		Me.x.AcceptsReturn = True
		Me.x.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.x.BackColor = System.Drawing.SystemColors.Window
		Me.x.CausesValidation = True
		Me.x.Enabled = True
		Me.x.ForeColor = System.Drawing.SystemColors.WindowText
		Me.x.HideSelection = True
		Me.x.ReadOnly = False
		Me.x.Maxlength = 0
		Me.x.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.x.MultiLine = False
		Me.x.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.x.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.x.TabStop = True
		Me.x.Visible = True
		Me.x.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.x.Name = "x"
		Me.xsec.AutoSize = False
		Me.xsec.Size = New System.Drawing.Size(97, 25)
		Me.xsec.Location = New System.Drawing.Point(16, 32)
		Me.xsec.TabIndex = 0
		Me.xsec.Text = "Text1"
		Me.xsec.AcceptsReturn = True
		Me.xsec.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.xsec.BackColor = System.Drawing.SystemColors.Window
		Me.xsec.CausesValidation = True
		Me.xsec.Enabled = True
		Me.xsec.ForeColor = System.Drawing.SystemColors.WindowText
		Me.xsec.HideSelection = True
		Me.xsec.ReadOnly = False
		Me.xsec.Maxlength = 0
		Me.xsec.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.xsec.MultiLine = False
		Me.xsec.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.xsec.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.xsec.TabStop = True
		Me.xsec.Visible = True
		Me.xsec.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.xsec.Name = "xsec"
		Me.Controls.Add(Command1)
		Me.Controls.Add(Text1)
		Me.Controls.Add(y)
		Me.Controls.Add(x)
		Me.Controls.Add(xsec)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class