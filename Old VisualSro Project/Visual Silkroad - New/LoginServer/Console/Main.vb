Imports SRFramework

Friend Class Program

    Shared Sub Main()
        AddHandler Server.OnClientConnect, AddressOf Program.Server_OnClientConnect
        AddHandler Server.OnClientDisconnect, AddressOf Program.Server_OnClientDisconnect
        AddHandler Server.OnReceiveData, AddressOf Program.Server_OnReceiveData
        AddHandler Server.OnServerError, AddressOf Program.Server_OnServerError
        AddHandler Server.OnServerStarted, AddressOf Program.Server_OnServerStarted
        AddHandler Server.OnServerStopped, AddressOf Program.Server_OnServerStopped
        AddHandler Server.OnServerLog, AddressOf Program.Server_OnServerLog
        AddHandler Server.OnServerPacketLog, AddressOf Program.Server_OnPacketLog

        AddHandler Database.OnDatabaseError, AddressOf Program.db_OnDatabaseError
        AddHandler Database.OnDatabaseConnected, AddressOf Program.db_OnConnectedToDatabase
        AddHandler Database.OnDatabaseLog, AddressOf Program.db_OnDatabaseLog

        AddHandler GlobalManagerCon.OnGlobalManagerInit, AddressOf Program.gmc_OnGlobalManagerInit
        AddHandler GlobalManagerCon.OnGlobalManagerShutdown, AddressOf Program.gmc_OnGlobalManagerShutdown
        AddHandler GlobalManagerCon.OnGlobalManageConLost, AddressOf Program.gmc_OnGlobalManagerConLost
        AddHandler GlobalManagerCon.OnError, AddressOf Program.gmc_OnGlobalManagerError
        AddHandler GlobalManagerCon.OnLog, AddressOf Program.gmc_OnGlobalManagerLog
        AddHandler GlobalManagerCon.OnPacketReceived, AddressOf Functions.ParseGlobalManager
        AddHandler GlobalManagerCon.OnGatewayUserauthReply, AddressOf Functions.LoginSendUserAuthSucceed

        Console.BackgroundColor = ConsoleColor.White
        Console.ForegroundColor = ConsoleColor.DarkGreen
        Console.Clear()
        Console.Title = "LOGINSERVER ALPHA"
        Log.WriteSystemLog("Starting Server")

        Log.WriteSystemLog("Loading Settings.")
        Settings.LoadSettings()
        Settings.SetToServer()

        Log.WriteSystemLog("Loaded Settings. Conneting Database.")
        Database.Connect()

        Log.WriteSystemLog("Connected Database. Starting Server now.")
        LoginDb.UpdateData()
        Timers.LoadTimers(Server.MaxClients)
        GlobalDef.Initalize(Server.MaxClients)

        Log.WriteSystemLog("Inital Loading complete! Waiting for Globalmanager...")
        Log.WriteSystemLog("Latest Version: " & Settings.Server_CurrectVersion)
        Log.WriteSystemLog("Slotcount: " & Settings.Server_NormalSlots & "/" & Settings.Server_MaxClients)

        GlobalManagerCon.Connect(Settings.GlobalManger_Ip, Settings.GlobalManger_Port)

        Do While True
            Dim msg As String = Console.ReadLine()
            CheckCommand(msg)
            Threading.Thread.Sleep(10)
        Loop
    End Sub

    Private Shared Sub Server_OnClientConnect(ByVal ip As String, ByVal index As Integer)
        If True Then
            Log.WriteSystemLog(String.Format("Client[{0}/{1}] Connected: {2}", Server.OnlineClients, Server.MaxNormalClients, ip))
        End If

        SessionInfo(index) = New cSessionInfo_LoginServer
        Server.OnlineClients += 1

        Dim writer As New PacketWriter
        writer.Create(ServerOpcodes.HANDSHAKE)
        writer.Byte(1)
        Server.Send(writer.GetBytes, index)
    End Sub

    Private Shared Sub Server_OnClientDisconnect(ByVal ip As String, ByVal index As Integer)
        If Timers.LoginInfoTimer(index).Enabled Then
            Timers.LoginInfoTimer(index).Stop()
        End If

        SessionInfo(index) = Nothing
        Server.OnlineClients -= 1

        If True Then
            Log.WriteSystemLog(String.Format("Client[{0}/{1}] Disconnected: {2}", Server.OnlineClients, Server.MaxNormalClients, ip))
        End If
    End Sub

    Private Shared Sub Server_OnReceiveData(ByVal buffer() As Byte, ByVal index_ As Integer)
        Dim read As Integer = 0

        Do While True
            Dim length As Integer = BitConverter.ToUInt16(buffer, read)
            Dim opc As Integer = BitConverter.ToUInt16(buffer, read + 2)

            If length = 0 And opc = 0 Then 'endless prevention
                Exit Do
            End If

            Dim newbuff(length + 5) As Byte
            Array.ConstrainedCopy(buffer, read, newbuff, 0, length + 6)
            read = read + length + 6

            Dim packet As New PacketReader(newbuff)
            If Settings.Server_DebugMode Then
                Log.LogPacket(newbuff, False)
            End If

            Functions.Parser.Parse(packet, index_)
        Loop
    End Sub

    Private Shared Sub Server_OnServerError(ByVal ex As Exception, ByVal index As Integer)
        Log.WriteSystemLog("Server Error: " & ex.Message & " Index: " & index & " Stacktrace: " & ex.StackTrace) '-1 = on client connect + -2 = on server start
    End Sub

    Private Shared Sub Server_OnServerStarted(ByVal time As String)
        Log.WriteSystemLog("Server Started: " & time)
    End Sub

    Private Shared Sub Server_OnServerStopped(ByVal time As String)
        Log.WriteSystemLog("Server Stopped: " & time)
    End Sub

    Private Shared Sub Server_OnServerLog(ByVal message As String)
        Log.WriteSystemLog("Server Log: " & message)
    End Sub

    Private Shared Sub Server_OnPacketLog(ByVal buff() As Byte, ByVal fromserver As Boolean, ByVal index As Integer)
        Log.LogPacket(buff, fromserver)
    End Sub

    Private Shared Sub db_OnConnectedToDatabase()
        Log.WriteSystemLog("Connected to database at: " & DateTime.Now.ToString())
    End Sub

    Private Shared Sub db_OnDatabaseLog(ByVal message As String)
        Log.WriteSystemLog("Database Log: " & message)
    End Sub

    Private Shared Sub db_OnDatabaseError(ByVal ex As Exception, ByVal command As String)
        Log.WriteSystemLog("Server Error: " & ex.Message & " Command: " & command & " Stacktrace: " & ex.StackTrace)
    End Sub

    Private Shared Sub gmc_OnGlobalManagerInit()
        If Server.Online = False Then
            Server.Start()
        End If

        Log.WriteSystemLog("GMC: We are ready!")
    End Sub

    Private Shared Sub gmc_OnGlobalManagerShutdown()
        For i = 0 To SessionInfo.Count - 1
            If SessionInfo(i) IsNot Nothing Then
                Server.Disconnect(i)
            End If
        Next
        Server.Stop()
        Database.ExecuteQuerys()

        Log.WriteSystemLog("GMC: Server stopped, Data is save. Feel free to close!")
    End Sub

    Private Shared Sub gmc_OnGlobalManagerConLost()
        Log.WriteSystemLog("GMC: Lost Connection, Cleanup!")
        Shard_Gateways.Clear()
        Shard_Gameservers.Clear()
        Shard_Downloads.Clear()
    End Sub

    Private Shared Sub gmc_OnGlobalManagerLog(ByVal message As String)
        Log.WriteSystemLog("GMC Log: " & message)
    End Sub

    Private Shared Sub gmc_OnGlobalManagerError(ByVal ex As Exception, ByVal index As String)
        Log.WriteSystemLog("GMC Error: " & ex.Message & " Index: " & index & " Stacktrace: " & ex.StackTrace)
    End Sub
End Class


