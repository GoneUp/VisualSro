Imports SRFramework

Public Class PerfWnd

    Dim timer As New Timers.Timer

    Private Sub PerfWnd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AddHandler timer.Elapsed, AddressOf TimerElapsed

        timer.Interval = 1000
        timer.Start()
    End Sub

    Private Sub TimerElapsed()
        lblDLMain_TB.Text = cByteCounter.FormatVolume(Server.DownloadCounter.TotalBytes)
        lblDLMain_TP.Text = cByteCounter.FormatVolume(Server.DownloadCounter.TotalPackets)
        lblDLMain_BS.Text = cByteCounter.FormatBandwidth(Server.DownloadCounter.BytesPerS)
        lblDLMain_PS.Text = cByteCounter.FormatBandwidth(Server.DownloadCounter.PacketsPerS)

        lblULMain_Tb.Text = cByteCounter.FormatVolume(Server.UploadCounter.TotalBytes)
        lblULMain_TP.Text = cByteCounter.FormatVolume(Server.UploadCounter.TotalPackets)
        lblULMain_BS.Text = cByteCounter.FormatBandwidth(Server.UploadCounter.BytesPerS)
        lblULMain_PS.Text = cByteCounter.FormatBandwidth(Server.UploadCounter.PacketsPerS)

        lblGMCMain_TB.Text = cByteCounter.FormatVolume(GlobalManagerCon.DownloadCounter.TotalBytes + GlobalManagerCon.UploadCounter.TotalBytes)
        lblGMCMain_TP.Text = cByteCounter.FormatVolume(GlobalManagerCon.DownloadCounter.TotalPackets + GlobalManagerCon.UploadCounter.TotalPackets)
        lblGMCMain_BS.Text = cByteCounter.FormatBandwidth(GlobalManagerCon.DownloadCounter.BytesPerS + GlobalManagerCon.UploadCounter.BytesPerS)
        lblGMCMain_PS.Text = cByteCounter.FormatBandwidth(GlobalManagerCon.DownloadCounter.PacketsPerS + GlobalManagerCon.UploadCounter.PacketsPerS)

        lblTime.Text = Date.Now
    End Sub

    Private Sub PerfWnd_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        timer.Dispose()
    End Sub
End Class