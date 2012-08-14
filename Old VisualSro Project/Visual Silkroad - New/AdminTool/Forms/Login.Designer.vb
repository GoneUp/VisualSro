<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Login
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
        Me.lblUser = New System.Windows.Forms.Label()
        Me.boxUser = New System.Windows.Forms.TextBox()
        Me.boxPasswort = New System.Windows.Forms.TextBox()
        Me.lblPw = New System.Windows.Forms.Label()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblUser
        '
        Me.lblUser.AutoSize = True
        Me.lblUser.Location = New System.Drawing.Point(12, 9)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(55, 13)
        Me.lblUser.TabIndex = 0
        Me.lblUser.Text = "Username"
        '
        'boxUser
        '
        Me.boxUser.Location = New System.Drawing.Point(12, 25)
        Me.boxUser.Name = "boxUser"
        Me.boxUser.Size = New System.Drawing.Size(138, 20)
        Me.boxUser.TabIndex = 1
        '
        'boxPasswort
        '
        Me.boxPasswort.Location = New System.Drawing.Point(12, 64)
        Me.boxPasswort.Name = "boxPasswort"
        Me.boxPasswort.Size = New System.Drawing.Size(138, 20)
        Me.boxPasswort.TabIndex = 3
        '
        'lblPw
        '
        Me.lblPw.AutoSize = True
        Me.lblPw.Location = New System.Drawing.Point(12, 48)
        Me.lblPw.Name = "lblPw"
        Me.lblPw.Size = New System.Drawing.Size(50, 13)
        Me.lblPw.TabIndex = 2
        Me.lblPw.Text = "Passwort"
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(80, 90)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(70, 34)
        Me.btnLogin.TabIndex = 4
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'Login
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(162, 126)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.boxPasswort)
        Me.Controls.Add(Me.lblPw)
        Me.Controls.Add(Me.boxUser)
        Me.Controls.Add(Me.lblUser)
        Me.Name = "Login"
        Me.Text = "AdminTool"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents boxUser As System.Windows.Forms.TextBox
    Friend WithEvents boxPasswort As System.Windows.Forms.TextBox
    Friend WithEvents lblPw As System.Windows.Forms.Label
    Friend WithEvents btnLogin As System.Windows.Forms.Button
End Class
