<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.components = New System.ComponentModel.Container
        Me.cmdLoadPK2 = New System.Windows.Forms.Button
        Me.pnlControls = New System.Windows.Forms.Panel
        Me.cmdMoveUp = New System.Windows.Forms.Button
        Me.cmdMoveLeft = New System.Windows.Forms.Button
        Me.cmdMoveDown = New System.Windows.Forms.Button
        Me.cmdMoveRight = New System.Windows.Forms.Button
        Me.txtLog = New System.Windows.Forms.TextBox
        Me.cmdRedraw = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.checkDrawImage = New System.Windows.Forms.CheckBox
        Me.txtScaleFactor = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.checkDrawZone2 = New System.Windows.Forms.CheckBox
        Me.checkDrawObjects = New System.Windows.Forms.CheckBox
        Me.checkDrawZone3 = New System.Windows.Forms.CheckBox
        Me.checkDrawXY = New System.Windows.Forms.CheckBox
        Me.checkDrawZone1 = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtYSec = New System.Windows.Forms.TextBox
        Me.txtXSec = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.picMap = New System.Windows.Forms.PictureBox
        Me.tip = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlControls.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.picMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdLoadPK2
        '
        Me.cmdLoadPK2.Location = New System.Drawing.Point(12, 12)
        Me.cmdLoadPK2.Name = "cmdLoadPK2"
        Me.cmdLoadPK2.Size = New System.Drawing.Size(357, 23)
        Me.cmdLoadPK2.TabIndex = 1
        Me.cmdLoadPK2.TabStop = False
        Me.cmdLoadPK2.Text = "Load PK2"
        Me.cmdLoadPK2.UseVisualStyleBackColor = True
        '
        'pnlControls
        '
        Me.pnlControls.Controls.Add(Me.cmdMoveUp)
        Me.pnlControls.Controls.Add(Me.cmdMoveLeft)
        Me.pnlControls.Controls.Add(Me.cmdMoveDown)
        Me.pnlControls.Controls.Add(Me.cmdMoveRight)
        Me.pnlControls.Controls.Add(Me.txtLog)
        Me.pnlControls.Controls.Add(Me.cmdRedraw)
        Me.pnlControls.Controls.Add(Me.GroupBox1)
        Me.pnlControls.Controls.Add(Me.Label2)
        Me.pnlControls.Controls.Add(Me.Label1)
        Me.pnlControls.Controls.Add(Me.txtYSec)
        Me.pnlControls.Controls.Add(Me.txtXSec)
        Me.pnlControls.Controls.Add(Me.Label4)
        Me.pnlControls.Enabled = False
        Me.pnlControls.Location = New System.Drawing.Point(13, 42)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(356, 478)
        Me.pnlControls.TabIndex = 2
        '
        'cmdMoveUp
        '
        Me.cmdMoveUp.Location = New System.Drawing.Point(150, 47)
        Me.cmdMoveUp.Name = "cmdMoveUp"
        Me.cmdMoveUp.Size = New System.Drawing.Size(50, 20)
        Me.cmdMoveUp.TabIndex = 31
        Me.cmdMoveUp.Text = "˄"
        Me.cmdMoveUp.UseVisualStyleBackColor = True
        '
        'cmdMoveLeft
        '
        Me.cmdMoveLeft.Location = New System.Drawing.Point(150, 21)
        Me.cmdMoveLeft.Name = "cmdMoveLeft"
        Me.cmdMoveLeft.Size = New System.Drawing.Size(50, 20)
        Me.cmdMoveLeft.TabIndex = 30
        Me.cmdMoveLeft.Text = "<"
        Me.cmdMoveLeft.UseVisualStyleBackColor = True
        '
        'cmdMoveDown
        '
        Me.cmdMoveDown.Location = New System.Drawing.Point(206, 47)
        Me.cmdMoveDown.Name = "cmdMoveDown"
        Me.cmdMoveDown.Size = New System.Drawing.Size(50, 20)
        Me.cmdMoveDown.TabIndex = 29
        Me.cmdMoveDown.Text = "˅"
        Me.cmdMoveDown.UseVisualStyleBackColor = True
        '
        'cmdMoveRight
        '
        Me.cmdMoveRight.Location = New System.Drawing.Point(206, 21)
        Me.cmdMoveRight.Name = "cmdMoveRight"
        Me.cmdMoveRight.Size = New System.Drawing.Size(50, 20)
        Me.cmdMoveRight.TabIndex = 28
        Me.cmdMoveRight.Text = ">"
        Me.cmdMoveRight.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLog.Location = New System.Drawing.Point(-1, 328)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(357, 150)
        Me.txtLog.TabIndex = 26
        '
        'cmdRedraw
        '
        Me.cmdRedraw.Location = New System.Drawing.Point(79, 229)
        Me.cmdRedraw.Name = "cmdRedraw"
        Me.cmdRedraw.Size = New System.Drawing.Size(199, 23)
        Me.cmdRedraw.TabIndex = 25
        Me.cmdRedraw.Text = "Redraw"
        Me.cmdRedraw.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.checkDrawImage)
        Me.GroupBox1.Controls.Add(Me.txtScaleFactor)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.checkDrawZone2)
        Me.GroupBox1.Controls.Add(Me.checkDrawObjects)
        Me.GroupBox1.Controls.Add(Me.checkDrawZone3)
        Me.GroupBox1.Controls.Add(Me.checkDrawXY)
        Me.GroupBox1.Controls.Add(Me.checkDrawZone1)
        Me.GroupBox1.Location = New System.Drawing.Point(53, 105)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(250, 100)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Draw Settings"
        '
        'checkDrawImage
        '
        Me.checkDrawImage.AutoSize = True
        Me.checkDrawImage.Checked = True
        Me.checkDrawImage.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkDrawImage.Location = New System.Drawing.Point(20, 54)
        Me.checkDrawImage.Name = "checkDrawImage"
        Me.checkDrawImage.Size = New System.Drawing.Size(55, 17)
        Me.checkDrawImage.TabIndex = 26
        Me.checkDrawImage.TabStop = False
        Me.checkDrawImage.Text = "Image"
        Me.checkDrawImage.UseVisualStyleBackColor = True
        '
        'txtScaleFactor
        '
        Me.txtScaleFactor.Location = New System.Drawing.Point(147, 17)
        Me.txtScaleFactor.Name = "txtScaleFactor"
        Me.txtScaleFactor.Size = New System.Drawing.Size(29, 20)
        Me.txtScaleFactor.TabIndex = 25
        Me.txtScaleFactor.TabStop = False
        Me.txtScaleFactor.Text = "1"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(74, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "ScaleFactor:"
        '
        'checkDrawZone2
        '
        Me.checkDrawZone2.AutoSize = True
        Me.checkDrawZone2.Location = New System.Drawing.Point(97, 77)
        Me.checkDrawZone2.Name = "checkDrawZone2"
        Me.checkDrawZone2.Size = New System.Drawing.Size(57, 17)
        Me.checkDrawZone2.TabIndex = 22
        Me.checkDrawZone2.TabStop = False
        Me.checkDrawZone2.Text = "Zone2"
        Me.checkDrawZone2.UseVisualStyleBackColor = True
        '
        'checkDrawObjects
        '
        Me.checkDrawObjects.AutoSize = True
        Me.checkDrawObjects.Location = New System.Drawing.Point(168, 54)
        Me.checkDrawObjects.Name = "checkDrawObjects"
        Me.checkDrawObjects.Size = New System.Drawing.Size(62, 17)
        Me.checkDrawObjects.TabIndex = 20
        Me.checkDrawObjects.TabStop = False
        Me.checkDrawObjects.Text = "Objects"
        Me.checkDrawObjects.UseVisualStyleBackColor = True
        '
        'checkDrawZone3
        '
        Me.checkDrawZone3.AutoSize = True
        Me.checkDrawZone3.Location = New System.Drawing.Point(160, 77)
        Me.checkDrawZone3.Name = "checkDrawZone3"
        Me.checkDrawZone3.Size = New System.Drawing.Size(57, 17)
        Me.checkDrawZone3.TabIndex = 23
        Me.checkDrawZone3.TabStop = False
        Me.checkDrawZone3.Text = "Zone3"
        Me.checkDrawZone3.UseVisualStyleBackColor = True
        '
        'checkDrawXY
        '
        Me.checkDrawXY.AutoSize = True
        Me.checkDrawXY.Location = New System.Drawing.Point(81, 54)
        Me.checkDrawXY.Name = "checkDrawXY"
        Me.checkDrawXY.Size = New System.Drawing.Size(81, 17)
        Me.checkDrawXY.TabIndex = 19
        Me.checkDrawXY.TabStop = False
        Me.checkDrawXY.Text = "XSec,YSec"
        Me.checkDrawXY.UseVisualStyleBackColor = True
        '
        'checkDrawZone1
        '
        Me.checkDrawZone1.AutoSize = True
        Me.checkDrawZone1.Location = New System.Drawing.Point(34, 77)
        Me.checkDrawZone1.Name = "checkDrawZone1"
        Me.checkDrawZone1.Size = New System.Drawing.Size(57, 17)
        Me.checkDrawZone1.TabIndex = 21
        Me.checkDrawZone1.TabStop = False
        Me.checkDrawZone1.Text = "Zone1"
        Me.checkDrawZone1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(46, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "YSec:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(46, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(36, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "XSec:"
        '
        'txtYSec
        '
        Me.txtYSec.Location = New System.Drawing.Point(91, 47)
        Me.txtYSec.Name = "txtYSec"
        Me.txtYSec.Size = New System.Drawing.Size(53, 20)
        Me.txtYSec.TabIndex = 11
        Me.txtYSec.TabStop = False
        Me.txtYSec.Text = "96"
        '
        'txtXSec
        '
        Me.txtXSec.Location = New System.Drawing.Point(91, 21)
        Me.txtXSec.Name = "txtXSec"
        Me.txtXSec.Size = New System.Drawing.Size(53, 20)
        Me.txtXSec.TabIndex = 10
        Me.txtXSec.TabStop = False
        Me.txtXSec.Text = "168"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(133, 312)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 13)
        Me.Label4.TabIndex = 27
        Me.Label4.Text = "---------- Log ----------"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.picMap)
        Me.Panel2.Location = New System.Drawing.Point(375, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(512, 512)
        Me.Panel2.TabIndex = 3
        '
        'picMap
        '
        Me.picMap.BackColor = System.Drawing.Color.Red
        Me.picMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picMap.Cursor = System.Windows.Forms.Cursors.Cross
        Me.picMap.Location = New System.Drawing.Point(0, 0)
        Me.picMap.Margin = New System.Windows.Forms.Padding(0)
        Me.picMap.MaximumSize = New System.Drawing.Size(512, 512)
        Me.picMap.MinimumSize = New System.Drawing.Size(512, 512)
        Me.picMap.Name = "picMap"
        Me.picMap.Size = New System.Drawing.Size(512, 512)
        Me.picMap.TabIndex = 9
        Me.picMap.TabStop = False
        '
        'tip
        '
        Me.tip.AutomaticDelay = 100
        Me.tip.UseAnimation = False
        Me.tip.UseFading = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(894, 532)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.pnlControls)
        Me.Controls.Add(Me.cmdLoadPK2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmMain"
        Me.Text = "Navmesh Window"
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.picMap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdLoadPK2 As System.Windows.Forms.Button
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtYSec As System.Windows.Forms.TextBox
    Friend WithEvents txtXSec As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents checkDrawZone2 As System.Windows.Forms.CheckBox
    Friend WithEvents checkDrawZone3 As System.Windows.Forms.CheckBox
    Friend WithEvents checkDrawZone1 As System.Windows.Forms.CheckBox
    Friend WithEvents checkDrawObjects As System.Windows.Forms.CheckBox
    Friend WithEvents checkDrawXY As System.Windows.Forms.CheckBox
    Friend WithEvents txtScaleFactor As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents picMap As System.Windows.Forms.PictureBox
    Friend WithEvents cmdRedraw As System.Windows.Forms.Button
    Friend WithEvents checkDrawImage As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents cmdMoveUp As System.Windows.Forms.Button
    Friend WithEvents cmdMoveLeft As System.Windows.Forms.Button
    Friend WithEvents cmdMoveDown As System.Windows.Forms.Button
    Friend WithEvents cmdMoveRight As System.Windows.Forms.Button
    Friend WithEvents tip As System.Windows.Forms.ToolTip

End Class
