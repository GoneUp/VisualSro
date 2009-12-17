<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLog
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLog))
        Me.logBox = New System.Windows.Forms.ListBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ButClose = New System.Windows.Forms.Button
        Me.ButDelete = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'logBox
        '
        Me.logBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.logBox.Location = New System.Drawing.Point(0, 0)
        Me.logBox.Name = "logBox"
        Me.logBox.Size = New System.Drawing.Size(284, 264)
        Me.logBox.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.Panel1.Controls.Add(Me.ButClose)
        Me.Panel1.Controls.Add(Me.ButDelete)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 236)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(284, 28)
        Me.Panel1.TabIndex = 1
        '
        'ButClose
        '
        Me.ButClose.Location = New System.Drawing.Point(168, 3)
        Me.ButClose.Name = "ButClose"
        Me.ButClose.Size = New System.Drawing.Size(95, 20)
        Me.ButClose.TabIndex = 1
        Me.ButClose.Text = "Close"
        Me.ButClose.UseVisualStyleBackColor = True
        '
        'ButDelete
        '
        Me.ButDelete.Location = New System.Drawing.Point(20, 3)
        Me.ButDelete.Name = "ButDelete"
        Me.ButDelete.Size = New System.Drawing.Size(95, 20)
        Me.ButDelete.TabIndex = 0
        Me.ButDelete.Text = "Delete Log"
        Me.ButDelete.UseVisualStyleBackColor = True
        '
        'frmLog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 264)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.logBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLog"
        Me.Text = "Status Log"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents logBox As System.Windows.Forms.ListBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButClose As System.Windows.Forms.Button
    Friend WithEvents ButDelete As System.Windows.Forms.Button
End Class
