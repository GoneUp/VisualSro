Public Class Main
#Region "Load"
    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AddHandler GlobalManagerCon.OnGlobalManagerInit, AddressOf Me.gmc_OnGlobalManagerInit
        AddHandler GlobalManagerCon.OnGlobalManagerShutdown, AddressOf Me.gmc_OnGlobalManagerInit
        AddHandler GlobalManagerCon.OnError, AddressOf Me.gmc_OnGlobalManagerError
        AddHandler GlobalManagerCon.OnLog, AddressOf Me.gmc_OnGlobalManagerLog
        'AddHandler GlobalManagerCon.OnPacketReceived, AddressOf Functions.Parser.ParseGlobalManager
        'AddHandler GlobalManagerCon.OnGameserverUserauthReply, AddressOf Functions.Check_GlobalManagerUserAuthReply

        Log.WriteSystemLog("Loading Settings.")
        Settings.LoadSettings()
        Settings.SetToServer()

        Timers.LoadTimers(1)

        Log.WriteSystemLog("Inital Loading complete! Waiting for Globalmanager...")

        GlobalManagerCon.Connect(Settings.GlobalManger_Ip, Settings.GlobalManger_Port)
    End Sub
#End Region

#Region "GlobalManager"
    Private Sub gmc_OnGlobalManagerInit()
        Log.WriteSystemLog("GMC: We are ready!")
    End Sub

    Private Sub gmc_OnGlobalManagerShutdown()
        Log.WriteSystemLog("Server stopped, Data is save. Feel free to close!")
        Me.Enabled = False
    End Sub

    Private Sub gmc_OnGlobalManagerLog(ByVal message As String)
        Log.WriteSystemLog("GMC Log: " & message)
    End Sub

    Private Sub gmc_OnGlobalManagerError(ByVal ex As Exception, ByVal command As String)
        Log.WriteSystemLog("GMC error: " & ex.Message & " Command: " & command)
    End Sub
#End Region
End Class
