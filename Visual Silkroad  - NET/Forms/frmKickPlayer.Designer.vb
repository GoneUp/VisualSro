<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmKickPlayer
#Region "Vom Windows Form-Designer generierter Code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'Dieser Aufruf ist f�r den Windows Form-Designer erforderlich.
		InitializeComponent()
	End Sub
	'Das Formular �berschreibt den L�schvorgang, um die Komponentenliste zu bereinigen.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Wird vom Windows Form-Designer ben�tigt.
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents GMCheckBox As System.Windows.Forms.CheckBox
	Public WithEvents KickField As System.Windows.Forms.TextBox
	Public WithEvents CancelButton_Renamed As System.Windows.Forms.Button
	Public WithEvents OKButton As System.Windows.Forms.Button
	'Hinweis: Die folgende Prozedur ist f�r den Windows Form-Designer erforderlich.
	'Das Ver�ndern mit dem Windows Form-Designer ist nicht m�glich.
	'Das Bearbeiten mit dem Code-Editor ist nicht m�glich.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmKickPlayer))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GMCheckBox = New System.Windows.Forms.CheckBox
        Me.KickField = New System.Windows.Forms.TextBox
        Me.CancelButton_Renamed = New System.Windows.Forms.Button
        Me.OKButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'GMCheckBox
        '
        Me.GMCheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.GMCheckBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.GMCheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GMCheckBox.Location = New System.Drawing.Point(8, 192)
        Me.GMCheckBox.Name = "GMCheckBox"
        Me.GMCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GMCheckBox.Size = New System.Drawing.Size(297, 17)
        Me.GMCheckBox.TabIndex = 3
        Me.GMCheckBox.Text = "GM"
        Me.GMCheckBox.UseVisualStyleBackColor = False
        '
        'KickField
        '
        Me.KickField.AcceptsReturn = True
        Me.KickField.BackColor = System.Drawing.SystemColors.Window
        Me.KickField.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.KickField.ForeColor = System.Drawing.SystemColors.WindowText
        Me.KickField.Location = New System.Drawing.Point(8, 8)
        Me.KickField.MaxLength = 0
        Me.KickField.Name = "KickField"
        Me.KickField.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.KickField.Size = New System.Drawing.Size(297, 20)
        Me.KickField.TabIndex = 2
        '
        'CancelButton_Renamed
        '
        Me.CancelButton_Renamed.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton_Renamed.Cursor = System.Windows.Forms.Cursors.Default
        Me.CancelButton_Renamed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CancelButton_Renamed.Location = New System.Drawing.Point(312, 184)
        Me.CancelButton_Renamed.Name = "CancelButton_Renamed"
        Me.CancelButton_Renamed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CancelButton_Renamed.Size = New System.Drawing.Size(81, 25)
        Me.CancelButton_Renamed.TabIndex = 1
        Me.CancelButton_Renamed.Text = "Cancel"
        Me.CancelButton_Renamed.UseVisualStyleBackColor = False
        '
        'OKButton
        '
        Me.OKButton.BackColor = System.Drawing.SystemColors.Control
        Me.OKButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.OKButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OKButton.Location = New System.Drawing.Point(312, 8)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OKButton.Size = New System.Drawing.Size(81, 169)
        Me.OKButton.TabIndex = 0
        Me.OKButton.Text = "Kick!"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'frmKickPlayer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(402, 216)
        Me.Controls.Add(Me.GMCheckBox)
        Me.Controls.Add(Me.KickField)
        Me.Controls.Add(Me.CancelButton_Renamed)
        Me.Controls.Add(Me.OKButton)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(184, 250)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmKickPlayer"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Kick Player"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class