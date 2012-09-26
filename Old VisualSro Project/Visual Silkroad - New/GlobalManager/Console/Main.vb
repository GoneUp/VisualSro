Imports System
Imports SRFramework

Friend Module Program
    Friend Sub Main()
        'Events
        AddHandler Server.OnClientConnect, AddressOf Server_OnClientConnect
        AddHandler Server.OnClientDisconnect, AddressOf Server_OnClientDisconnect
        AddHandler Server.OnReceiveData, AddressOf Server_OnReceiveData
        AddHandler Server.OnServerError, AddressOf Server_OnServerError
        AddHandler Server.OnServerStarted, AddressOf Server_OnServerStarted
        AddHandler Server.OnServerStopped, AddressOf Server_OnServerStopped
        AddHandler Server.OnServerLog, AddressOf Server_OnServerLog
        AddHandler Server.OnServerPacketLog, AddressOf Server_OnPacketLog

        AddHandler Database.OnDatabaseError, AddressOf db_OnDatabaseError
        AddHandler Database.OnDatabaseConnected, AddressOf db_OnDatabaseConnected
        AddHandler Database.OnDatabaseLog, AddressOf db_OnDatabaseLog

        AddHandler Log.OnDatabaseQuery, AddressOf log_OnDatabaseQuery

        'Console
        Console.BackgroundColor = ConsoleColor.White
        Console.ForegroundColor = ConsoleColor.DarkGreen
        Console.Clear()
        Console.Title = "GlobalManager"
        Log.WriteSystemLog("Starting Server")

        'Settings
        Log.WriteSystemLog("Loading Settings.")
        Settings.LoadSettings()
        Settings.SetToServer()

        Log.WriteSystemLog("Loaded Settings. Conneting Database.")
        Database.Connect()

        'Init Data
        Log.WriteSystemLog("Connected Database. Starting Server now.")

        Dim succeed As Boolean = True
        succeed = Initalize(Server.MaxClients) 'GlobalDef -> SessionInfo
        succeed = GlobalDB.LoadData()
        succeed = Timers.LoadTimers()

        If succeed And Settings.ServerDebugMode = False Then
            'Ready!
            Server.Start()
            Log.WriteSystemLog("Inital Loading complete!")

        Else
            'Startup failed
            Console.ForegroundColor = ConsoleColor.Red
            Log.WriteSystemLog("Failed to load Refernce Data, check your error log!")
        End If



        Do While True
            Dim msg As String = Console.ReadLine()
            CheckCommand(msg)
        Loop
    End Sub

    Private Sub Server_OnClientConnect(ByVal ip As String, ByVal index As Integer)
        Log.WriteSystemLog("Client Connected : " & ip)
        Server.OnlineClients += 1

        SessionInfo(index) = New cSessionInfo_GlobalManager
        SessionInfo(index).BaseKey = Auth.GenarateKey

        Dim writer As New PacketWriter
        writer.Create(ServerOpcodes.HANDSHAKE)
        writer.Word(SessionInfo(index).BaseKey)
        Server.Send(writer.GetBytes, index)
    End Sub

    Private Sub Server_OnClientDisconnect(ByVal ip As String, ByVal index As Integer)
        Log.WriteSystemLog("Client Disconnected : " & ip)

        If SessionInfo(index) IsNot Nothing Then
            Shard.RemoveServer(SessionInfo(index).ServerId, SessionInfo(index).Type)
        End If

        SessionInfo(index) = Nothing

        If Server.OnlineClients > 0 Then
            Server.OnlineClients -= 1
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

            If Settings.ServerDebugMode = True Then
                Log.LogPacket(newbuff, False)
            End If

            Server.DownloadCounter.AddPacket(packet, PacketSource.Client)

            Functions.Parse(packet, index_)
        Loop
    End Sub

    Private Sub Server_OnServerError(ByVal ex As Exception, ByVal index As Integer)
        Log.WriteSystemLog("Server Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & index) '-1 = on client connect + -2 = on server start
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

    Private Sub db_OnDatabaseConnected()
        Log.WriteSystemLog("Connected to database at: " & DateTime.Now.ToString())
    End Sub

    Private Sub db_OnDatabaseLog(ByVal message As String)
        Log.WriteSystemLog("Database Log: " & message)
    End Sub

    Private Sub db_OnDatabaseError(ByVal ex As Exception, ByVal command As String)
        Log.WriteSystemLog("Database error: " & ex.Message & " Command: " & command)
    End Sub

    Private Sub log_OnDatabaseQuery(ByVal command As String)
        Database.SaveQuery(command)
    End Sub
End Module


