Imports SRFramework

Friend Module Program

#Region "Fields"
#Region "GMC Shutdown"
    Public Event GMCUserSidedShutdown As dGMCUserSidedShutdown
    Public Delegate Sub dGMCUserSidedShutdown()
#End Region
#End Region

    Public Sub Main()
        'Event's
        AddHandler Server.OnClientConnect, AddressOf Server_OnClientConnect
        AddHandler Server.OnClientDisconnect, AddressOf Server_OnClientDisconnect
        AddHandler Server.OnReceiveData, AddressOf Server_OnReceiveData
        AddHandler Server.OnServerError, AddressOf Server_OnServerError
        AddHandler Server.OnServerStarted, AddressOf Server_OnServerStarted
        AddHandler Server.OnServerStopped, AddressOf Server_OnServerStopped
        AddHandler Server.OnServerLog, AddressOf Server_OnServerLog
        AddHandler Server.OnServerPacketLog, AddressOf Server_OnPacketLog

        AddHandler Database.OnDatabaseError, AddressOf db_OnDatabaseError
        AddHandler Database.OnDatabaseConnected, AddressOf db_OnConnectedToDatabase
        AddHandler Database.OnDatabaseLog, AddressOf db_OnDatabaseLog

        AddHandler GlobalManagerCon.OnGlobalManagerInit, AddressOf gmc_OnGlobalManagerInit
        AddHandler GlobalManagerCon.OnGlobalManagerShutdown, AddressOf gmc_OnGlobalManagerShutdown
        AddHandler GlobalManagerCon.OnGlobalManageConLost, AddressOf gmc_OnGlobalManagerConLost
        AddHandler GlobalManagerCon.OnError, AddressOf gmc_OnGlobalManagerError
        AddHandler GlobalManagerCon.OnLog, AddressOf gmc_OnGlobalManagerLog
        AddHandler GlobalManagerCon.OnPacketReceived, AddressOf Functions.ParseGlobalManager
        AddHandler GlobalManagerCon.OnGatewayUserauthReply, AddressOf Functions.LoginSendUserAuthSucceed
        AddHandler GMCUserSidedShutdown, AddressOf gmc_OnUserSidedShutdown
        
        AddHandler Log.OnDatabaseQuery, AddressOf log_OnDatabaseQuery

        'Console
        Console.BackgroundColor = ConsoleColor.White
        Console.ForegroundColor = ConsoleColor.DarkGreen
        Console.Clear()
        Console.Title = "LOGINSERVER ALPHA"
        Log.WriteSystemLog("Starting Server")

        'Settings
        Log.WriteSystemLog("Loading Settings.")
        Settings.LoadSettings()
        Settings.SetToServer()

        'Database
        Log.WriteSystemLog("Loaded Settings. Conneting Database.")
        Database.Connect()

        'Load Data, Init GlobalDef
        Log.WriteSystemLog("Connected Database. Loading Data.")

        Dim succeed As Boolean = True
        succeed = Timers.LoadTimers(Server.MaxClients)
        succeed = Initalize(Server.MaxClients)

        If succeed And Settings.ServerDebugMode = False Then
            Log.WriteSystemLog("Inital Loading complete! Waiting for Globalmanager...")
            Log.WriteSystemLog("Latest Version: " & Settings.ServerCurrectVersion)
            Log.WriteSystemLog("Slotcount: " & Settings.ServerNormalSlots & "/" & Settings.ServerMaxClients)

            GlobalManagerCon.Connect(Settings.GlobalMangerIp, Settings.GlobalMangerPort)

        Else
            'Startup failed
            Console.ForegroundColor = ConsoleColor.Red
            Log.WriteSystemLog("Failed to load Refernce Data, check your error log!")
        End If
        
        Do While True
            Dim msg As String = Console.ReadLine()
            CheckCommand(msg)
            Threading.Thread.Sleep(10)
        Loop

    End Sub

    Private Sub Server_OnClientConnect(ByVal ip As String, ByVal index As Integer)
        If Settings.LogConnect Then
            Log.WriteSystemLog(String.Format("Client[{0}/{1}] Connected: {2}", Server.OnlineClients, Server.MaxNormalClients, ip))
        End If

        SessionInfo(index) = New cSessionInfo_LoginServer
        Server.OnlineClients += 1

        Dim writer As New PacketWriter
        writer.Create(ServerOpcodes.HANDSHAKE)
        writer.Byte(1)
        Server.Send(writer.GetBytes, index)
    End Sub

    Private Sub Server_OnClientDisconnect(ByVal ip As String, ByVal index As Integer)
        If Timers.LoginInfoTimer(index).Enabled Then
            Timers.LoginInfoTimer(index).Stop()
        End If

        SessionInfo(index) = Nothing

        If Server.OnlineClients > 0 Then
            Server.OnlineClients -= 1
        End If


        If Settings.LogConnect Then
            Log.WriteSystemLog(String.Format("Client[{0}/{1}] Disconnected: {2}", Server.OnlineClients, Server.MaxNormalClients, ip))
        End If
    End Sub

    Private Sub Server_OnReceiveData(ByVal buffer() As Byte, ByVal index_ As Integer)
        Dim position As Integer = 0

        Do While True
            Dim length As Integer = BitConverter.ToUInt16(buffer, position)
            Dim opc As Integer = BitConverter.ToUInt16(buffer, position + 2)

            If length = 0 And opc = 0 Then 'endless prevention
                Exit Do
            End If

            Dim newbuff(length + 5) As Byte
            Array.ConstrainedCopy(buffer, position, newbuff, 0, length + 6)
            position = position + length + 6

            Dim packet As New PacketReader(newbuff)

            If Settings.ServerDebugMode Then
                Log.LogPacket(newbuff, False)
            End If

            Server.DownloadCounter.AddPacket(packet, PacketSource.Client)

            Functions.Parser.Parse(packet, index_)

            packet.Dispose()
        Loop
    End Sub

    Private Sub Server_OnServerError(ByVal ex As Exception, ByVal index As Integer)
        Log.WriteSystemLog("Server Error: " & ex.Message & " Index: " & index & " Stacktrace: " & ex.StackTrace) '-1 = on client connect + -2 = on server start
    End Sub

    Private Sub Server_OnServerStarted(ByVal time As String)
        Log.WriteSystemLog("Server Started: " & time)
    End Sub

    Private Sub Server_OnServerStopped(ByVal time As String)
        Log.WriteSystemLog("Server Stopped: " & time)
    End Sub

    Private Sub Server_OnServerLog(ByVal message As String)
        Log.WriteSystemLog("Server Log: " & message)
    End Sub

    Private Sub Server_OnPacketLog(ByVal buff() As Byte, ByVal fromserver As Boolean, ByVal index As Integer)
        Log.LogPacket(buff, fromserver)
    End Sub

    Private Sub db_OnConnectedToDatabase()
        Log.WriteSystemLog("Connected to database at: " & DateTime.Now.ToString())
    End Sub

    Private Sub db_OnDatabaseLog(ByVal message As String)
        Log.WriteSystemLog("Database Log: " & message)
    End Sub

    Private Sub db_OnDatabaseError(ByVal ex As Exception, ByVal command As String)
        Log.WriteSystemLog("Server Error: " & ex.Message & " Command: " & command & " Stacktrace: " & ex.StackTrace)
    End Sub

    Private Sub gmc_OnGlobalManagerInit()
        If Server.Online = False Then
            Server.Start()
        End If

        
        Log.WriteSystemLog("GMC: We are ready!")
    End Sub

    Private Sub gmc_OnGlobalManagerShutdown()
        If GlobalManagerCon.UserSidedShutdown = False Then
            For i = 0 To SessionInfo.Count - 1
                If SessionInfo(i) IsNot Nothing Then
                    Server.Disconnect(i)
                End If
            Next
            Server.Stop()
            Database.ExecuteQuerys()

            Log.WriteSystemLog("GMC: Server stopped [serversided], Data is save. Feel free to close!")
        Else
            'Usersided Shutdown
            RaiseEvent GMCUserSidedShutdown()
        End If
    End Sub

    Private Sub gmc_OnGlobalManagerConLost()
        Log.WriteSystemLog("GMC: Lost Connection, Cleanup!")
        ShardGateways.Clear()
        ShardGameservers.Clear()
        ShardDownloads.Clear()
    End Sub

    Private Sub gmc_OnGlobalManagerLog(ByVal message As String)
        Log.WriteSystemLog("GMC Log: " & message)
    End Sub

    Private Sub gmc_OnGlobalManagerError(ByVal ex As Exception, ByVal index As String)
        Log.WriteSystemLog("GMC Error: " & ex.Message & " Index: " & index & " Stacktrace: " & ex.StackTrace)
    End Sub

    Private Sub gmc_OnUserSidedShutdown()
        Select Case GlobalManagerCon.ShutdownReason
            Case GlobalManagerClient.GMCShutdownReason.Normal_Shutdown
                Log.WriteSystemLog("GMC: Shutdown confirmed!")

            Case GlobalManagerClient.GMCShutdownReason.Reconnect
                Log.WriteSystemLog("Reconnect GlobalManager...")
                GlobalManagerCon.Connect(Settings.GlobalMangerIp, Settings.GlobalMangerPort)

            Case GlobalManagerClient.GMCShutdownReason.Reinit
                Log.WriteSystemLog("Cleanup Server...")

                GlobalDef.Initalize(Server.MaxClients)
                Timers.LoadTimers(Server.MaxClients)

                Log.WriteSystemLog("Reconnect GlobalManager...")
                GlobalManagerCon.Connect(Settings.GlobalMangerIp, Settings.GlobalMangerPort)
        End Select
    End Sub

    Private Sub log_OnDatabaseQuery(ByVal command As String)
        Database.SaveQuery(command)
    End Sub
End Module


