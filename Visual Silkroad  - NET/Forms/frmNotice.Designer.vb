<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmNotice
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
	Public WithEvents sndnoticebutton As System.Windows.Forms.Button
	Public WithEvents sndnoticefield As System.Windows.Forms.TextBox
	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNotice))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.sndnoticebutton = New System.Windows.Forms.Button
        Me.sndnoticefield = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'sndnoticebutton
        '
        Me.sndnoticebutton.BackColor = System.Drawing.SystemColors.Control
        Me.sndnoticebutton.Cursor = System.Windows.Forms.Cursors.Default
        Me.sndnoticebutton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.sndnoticebutton.Location = New System.Drawing.Point(8, 120)
        Me.sndnoticebutton.Name = "sndnoticebutton"
        Me.sndnoticebutton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.sndnoticebutton.Size = New System.Drawing.Size(289, 73)
        Me.sndnoticebutton.TabIndex = 1
        Me.sndnoticebutton.Text = "Send Notice!"
        Me.sndnoticebutton.UseVisualStyleBackColor = False
        '
        'sndnoticefield
        '
        Me.sndnoticefield.AcceptsReturn = True
        Me.sndnoticefield.BackColor = System.Drawing.SystemColors.Window
        Me.sndnoticefield.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.sndnoticefield.ForeColor = System.Drawing.SystemColors.WindowText
        Me.sndnoticefield.Location = New System.Drawing.Point(8, 8)
        Me.sndnoticefield.MaxLength = 0
        Me.sndnoticefield.Multiline = True
        Me.sndnoticefield.Name = "sndnoticefield"
        Me.sndnoticefield.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.sndnoticefield.Size = New System.Drawing.Size(289, 97)
        Me.sndnoticefield.TabIndex = 0
        Me.sndnoticefield.Text = "Welcome to Visual Silkroad!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'frmNotice
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(304, 202)
        Me.Controls.Add(Me.sndnoticebutton)
        Me.Controls.Add(Me.sndnoticefield)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(8, 30)
        Me.Name = "frmNotice"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Notice"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class