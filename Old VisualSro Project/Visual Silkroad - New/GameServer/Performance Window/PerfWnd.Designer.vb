<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PerfWnd
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblDLMain_BS = New System.Windows.Forms.Label()
        Me.lblDLMain_PS = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblDLMain_TP = New System.Windows.Forms.Label()
        Me.lblDLMain_TB = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblULMain_BS = New System.Windows.Forms.Label()
        Me.lblULMain_PS = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblULMain_TP = New System.Windows.Forms.Label()
        Me.lblULMain_Tb = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblGMCMain_BS = New System.Windows.Forms.Label()
        Me.lblGMCMain_PS = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblGMCMain_TP = New System.Windows.Forms.Label()
        Me.lblGMCMain_TB = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblDLMain_BS)
        Me.GroupBox1.Controls.Add(Me.lblDLMain_PS)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.lblDLMain_TP)
        Me.GroupBox1.Controls.Add(Me.lblDLMain_TB)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(204, 78)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Downlink Main"
        '
        'lblDLMain_BS
        '
        Me.lblDLMain_BS.AutoSize = True
        Me.lblDLMain_BS.Location = New System.Drawing.Point(88, 55)
        Me.lblDLMain_BS.Name = "lblDLMain_BS"
        Me.lblDLMain_BS.Size = New System.Drawing.Size(13, 13)
        Me.lblDLMain_BS.TabIndex = 7
        Me.lblDLMain_BS.Text = "0"
        '
        'lblDLMain_PS
        '
        Me.lblDLMain_PS.AutoSize = True
        Me.lblDLMain_PS.Location = New System.Drawing.Point(88, 42)
        Me.lblDLMain_PS.Name = "lblDLMain_PS"
        Me.lblDLMain_PS.Size = New System.Drawing.Size(13, 13)
        Me.lblDLMain_PS.TabIndex = 6
        Me.lblDLMain_PS.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 55)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(27, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "B/s:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 42)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(27, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "P/s:"
        '
        'lblDLMain_TP
        '
        Me.lblDLMain_TP.AutoSize = True
        Me.lblDLMain_TP.Location = New System.Drawing.Point(88, 29)
        Me.lblDLMain_TP.Name = "lblDLMain_TP"
        Me.lblDLMain_TP.Size = New System.Drawing.Size(13, 13)
        Me.lblDLMain_TP.TabIndex = 3
        Me.lblDLMain_TP.Text = "0"
        '
        'lblDLMain_TB
        '
        Me.lblDLMain_TB.AutoSize = True
        Me.lblDLMain_TB.Location = New System.Drawing.Point(88, 16)
        Me.lblDLMain_TB.Name = "lblDLMain_TB"
        Me.lblDLMain_TB.Size = New System.Drawing.Size(13, 13)
        Me.lblDLMain_TB.TabIndex = 2
        Me.lblDLMain_TB.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Total Packets:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total Bytes:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblULMain_BS)
        Me.GroupBox2.Controls.Add(Me.lblULMain_PS)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.lblULMain_TP)
        Me.GroupBox2.Controls.Add(Me.lblULMain_Tb)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Location = New System.Drawing.Point(9, 89)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(204, 78)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Uplink Main"
        '
        'lblULMain_BS
        '
        Me.lblULMain_BS.AutoSize = True
        Me.lblULMain_BS.Location = New System.Drawing.Point(88, 55)
        Me.lblULMain_BS.Name = "lblULMain_BS"
        Me.lblULMain_BS.Size = New System.Drawing.Size(13, 13)
        Me.lblULMain_BS.TabIndex = 7
        Me.lblULMain_BS.Text = "0"
        '
        'lblULMain_PS
        '
        Me.lblULMain_PS.AutoSize = True
        Me.lblULMain_PS.Location = New System.Drawing.Point(88, 42)
        Me.lblULMain_PS.Name = "lblULMain_PS"
        Me.lblULMain_PS.Size = New System.Drawing.Size(13, 13)
        Me.lblULMain_PS.TabIndex = 6
        Me.lblULMain_PS.Text = "0"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 55)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(27, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "B/s:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 42)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(27, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "P/s:"
        '
        'lblULMain_TP
        '
        Me.lblULMain_TP.AutoSize = True
        Me.lblULMain_TP.Location = New System.Drawing.Point(88, 29)
        Me.lblULMain_TP.Name = "lblULMain_TP"
        Me.lblULMain_TP.Size = New System.Drawing.Size(13, 13)
        Me.lblULMain_TP.TabIndex = 3
        Me.lblULMain_TP.Text = "0"
        '
        'lblULMain_Tb
        '
        Me.lblULMain_Tb.AutoSize = True
        Me.lblULMain_Tb.Location = New System.Drawing.Point(88, 16)
        Me.lblULMain_Tb.Name = "lblULMain_Tb"
        Me.lblULMain_Tb.Size = New System.Drawing.Size(13, 13)
        Me.lblULMain_Tb.TabIndex = 2
        Me.lblULMain_Tb.Text = "0"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 29)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(76, 13)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "Total Packets:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 16)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(63, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Total Bytes:"
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Location = New System.Drawing.Point(288, 118)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(10, 13)
        Me.lblTime.TabIndex = 9
        Me.lblTime.Text = "-"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lblGMCMain_BS)
        Me.GroupBox3.Controls.Add(Me.lblGMCMain_PS)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.lblGMCMain_TP)
        Me.GroupBox3.Controls.Add(Me.lblGMCMain_TB)
        Me.GroupBox3.Controls.Add(Me.Label15)
        Me.GroupBox3.Controls.Add(Me.Label16)
        Me.GroupBox3.Location = New System.Drawing.Point(219, 5)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(204, 78)
        Me.GroupBox3.TabIndex = 8
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "GMC Main"
        '
        'lblGMCMain_BS
        '
        Me.lblGMCMain_BS.AutoSize = True
        Me.lblGMCMain_BS.Location = New System.Drawing.Point(88, 55)
        Me.lblGMCMain_BS.Name = "lblGMCMain_BS"
        Me.lblGMCMain_BS.Size = New System.Drawing.Size(13, 13)
        Me.lblGMCMain_BS.TabIndex = 7
        Me.lblGMCMain_BS.Text = "0"
        '
        'lblGMCMain_PS
        '
        Me.lblGMCMain_PS.AutoSize = True
        Me.lblGMCMain_PS.Location = New System.Drawing.Point(88, 42)
        Me.lblGMCMain_PS.Name = "lblGMCMain_PS"
        Me.lblGMCMain_PS.Size = New System.Drawing.Size(13, 13)
        Me.lblGMCMain_PS.TabIndex = 6
        Me.lblGMCMain_PS.Text = "0"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 55)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(27, 13)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "B/s:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 42)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(27, 13)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "P/s:"
        '
        'lblGMCMain_TP
        '
        Me.lblGMCMain_TP.AutoSize = True
        Me.lblGMCMain_TP.Location = New System.Drawing.Point(88, 29)
        Me.lblGMCMain_TP.Name = "lblGMCMain_TP"
        Me.lblGMCMain_TP.Size = New System.Drawing.Size(13, 13)
        Me.lblGMCMain_TP.TabIndex = 3
        Me.lblGMCMain_TP.Text = "0"
        '
        'lblGMCMain_TB
        '
        Me.lblGMCMain_TB.AutoSize = True
        Me.lblGMCMain_TB.Location = New System.Drawing.Point(88, 16)
        Me.lblGMCMain_TB.Name = "lblGMCMain_TB"
        Me.lblGMCMain_TB.Size = New System.Drawing.Size(13, 13)
        Me.lblGMCMain_TB.TabIndex = 2
        Me.lblGMCMain_TB.Text = "0"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 29)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(76, 13)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "Total Packets:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(6, 16)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(63, 13)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Total Bytes:"
        '
        'PerfWnd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(454, 194)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "PerfWnd"
        Me.Text = "GS: PerfWind"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblDLMain_BS As System.Windows.Forms.Label
    Friend WithEvents lblDLMain_PS As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblDLMain_TP As System.Windows.Forms.Label
    Friend WithEvents lblDLMain_TB As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lblULMain_BS As System.Windows.Forms.Label
    Friend WithEvents lblULMain_PS As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblULMain_TP As System.Windows.Forms.Label
    Friend WithEvents lblULMain_Tb As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lblGMCMain_BS As System.Windows.Forms.Label
    Friend WithEvents lblGMCMain_PS As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblGMCMain_TP As System.Windows.Forms.Label
    Friend WithEvents lblGMCMain_TB As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
End Class
