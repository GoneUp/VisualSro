Imports SRFramework

Public Class PerfWnd

    Dim timer As New Timers.Timer

    Private Sub PerfWnd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AddHandler timer.Elapsed, AddressOf TimerElapsed

        timer.Interval = 1000
        timer.Start()
    End Sub

    Private Sub TimerElapsed()
        Me.Invoke(Sub() lblDLMain_TB.Text = cByteCounter.FormatVolume(Server.DownloadCounter.TotalBytes))
        Me.Invoke(Sub() lblDLMain_TP.Text = (Server.DownloadCounter.TotalPackets))
        Me.Invoke(Sub() lblDLMain_BS.Text = cByteCounter.FormatBandwidth(Server.DownloadCounter.BytesPerS))
        Me.Invoke(Sub() lblDLMain_PS.Text = cByteCounter.FormatBandwidth(Server.DownloadCounter.PacketsPerS))

        Me.Invoke(Sub() lblULMain_Tb.Text = cByteCounter.FormatVolume(Server.UploadCounter.TotalBytes))
        Me.Invoke(Sub() lblULMain_TP.Text = (Server.UploadCounter.TotalPackets))
        Me.Invoke(Sub() lblULMain_BS.Text = cByteCounter.FormatBandwidth(Server.UploadCounter.BytesPerS))
        Me.Invoke(Sub() lblULMain_PS.Text = cByteCounter.FormatBandwidth(Server.UploadCounter.PacketsPerS))

        Me.Invoke(Sub() lblGMCMain_TB.Text = cByteCounter.FormatVolume(GlobalManagerCon.DownloadCounter.TotalBytes + GlobalManagerCon.UploadCounter.TotalBytes))
        Me.Invoke(Sub() lblGMCMain_TP.Text = (GlobalManagerCon.DownloadCounter.TotalPackets + GlobalManagerCon.UploadCounter.TotalPackets))
        Me.Invoke(Sub() lblGMCMain_BS.Text = cByteCounter.FormatBandwidth(GlobalManagerCon.DownloadCounter.BytesPerS + GlobalManagerCon.UploadCounter.BytesPerS))
        Me.Invoke(Sub() lblGMCMain_PS.Text = cByteCounter.FormatBandwidth(GlobalManagerCon.DownloadCounter.PacketsPerS + GlobalManagerCon.UploadCounter.PacketsPerS))

        Me.Invoke(Sub() lblTime.Text = Date.Now)
    End Sub

    Private Sub PerfWnd_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        timer.Dispose()
    End Sub
End Class