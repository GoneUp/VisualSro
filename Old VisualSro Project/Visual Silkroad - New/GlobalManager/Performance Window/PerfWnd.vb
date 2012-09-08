Imports SRFramework

Public Class PerfWnd

    Dim timer As New System.Timers.Timer

    Private Sub PerfWnd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AddHandler timer.Elapsed, AddressOf TimerElapsed

        timer.Interval = 1000
        timer.Start()
    End Sub

    Private Sub TimerElapsed()
        timer.Stop()

        Me.Invoke(Sub() lblDLMain_TB.Text = cByteCounter.FormatVolume(Server.DownloadCounter.TotalBytes))
        Me.Invoke(Sub() lblDLMain_TP.Text = (Server.DownloadCounter.TotalPackets))
        Me.Invoke(Sub() lblDLMain_BS.Text = cByteCounter.FormatBandwidth(Server.DownloadCounter.BytesPerS))
        Me.Invoke(Sub() lblDLMain_PS.Text = cByteCounter.FormatBandwidth(Server.DownloadCounter.PacketsPerS))

        Me.Invoke(Sub() lblULMain_Tb.Text = cByteCounter.FormatVolume(Server.UploadCounter.TotalBytes))
        Me.Invoke(Sub() lblULMain_TP.Text = (Server.UploadCounter.TotalPackets))
        Me.Invoke(Sub() lblULMain_BS.Text = cByteCounter.FormatBandwidth(Server.UploadCounter.BytesPerS))
        Me.Invoke(Sub() lblULMain_PS.Text = cByteCounter.FormatBandwidth(Server.UploadCounter.PacketsPerS))

        'Me.Invoke(Sub() lblGMCDLMain_TB.Text = cByteCounter.FormatVolume(GlobalManagerCon.DownloadCounter.TotalBytes))
        'Me.Invoke(Sub() lblGMCDLMain_TP.Text = (GlobalManagerCon.DownloadCounter.TotalPackets))
        'Me.Invoke(Sub() lblGMCDLMain_BS.Text = cByteCounter.FormatBandwidth(GlobalManagerCon.DownloadCounter.BytesPerS))
        'Me.Invoke(Sub() lblGMCDLMain_PS.Text = cByteCounter.FormatBandwidth(GlobalManagerCon.DownloadCounter.PacketsPerS))

        'Me.Invoke(Sub() lblGMCULMain_TB.Text = cByteCounter.FormatVolume(GlobalManagerCon.UploadCounter.TotalBytes))
        'Me.Invoke(Sub() lblGMCULMain_TP.Text = (GlobalManagerCon.UploadCounter.TotalPackets))
        'Me.Invoke(Sub() lblGMCULMain_BS.Text = cByteCounter.FormatBandwidth(GlobalManagerCon.UploadCounter.BytesPerS))
        'Me.Invoke(Sub() lblGMCULMain_PS.Text = cByteCounter.FormatBandwidth(GlobalManagerCon.UploadCounter.PacketsPerS))

        Me.Invoke(Sub() lblTime.Text = Date.Now)

        timer.Start()
    End Sub

    Private Sub PerfWnd_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        timer.Dispose()
    End Sub
End Class