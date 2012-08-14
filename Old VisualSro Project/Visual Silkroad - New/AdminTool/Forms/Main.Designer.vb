<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageGlobal = New System.Windows.Forms.TabPage()
        Me.TabPageGS = New System.Windows.Forms.TabPage()
        Me.TabPageGM = New System.Windows.Forms.TabPage()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPageGlobal)
        Me.TabControl1.Controls.Add(Me.TabPageGS)
        Me.TabControl1.Controls.Add(Me.TabPageGM)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(289, 943)
        Me.TabControl1.TabIndex = 0
        '
        'TabPageGlobal
        '
        Me.TabPageGlobal.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGlobal.Name = "TabPageGlobal"
        Me.TabPageGlobal.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageGlobal.Size = New System.Drawing.Size(281, 917)
        Me.TabPageGlobal.TabIndex = 0
        Me.TabPageGlobal.Text = "GlobalManager"
        Me.TabPageGlobal.UseVisualStyleBackColor = True
        '
        'TabPageGS
        '
        Me.TabPageGS.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGS.Name = "TabPageGS"
        Me.TabPageGS.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageGS.Size = New System.Drawing.Size(281, 917)
        Me.TabPageGS.TabIndex = 1
        Me.TabPageGS.Text = "GameServer"
        Me.TabPageGS.UseVisualStyleBackColor = True
        '
        'TabPageGM
        '
        Me.TabPageGM.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGM.Name = "TabPageGM"
        Me.TabPageGM.Size = New System.Drawing.Size(281, 917)
        Me.TabPageGM.TabIndex = 2
        Me.TabPageGM.Text = "GameMaster"
        Me.TabPageGM.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(289, 943)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Main"
        Me.Text = "Form1"
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPageGlobal As System.Windows.Forms.TabPage
    Friend WithEvents TabPageGS As System.Windows.Forms.TabPage
    Friend WithEvents TabPageGM As System.Windows.Forms.TabPage

End Class
