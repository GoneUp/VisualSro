Imports System
Imports SRFramework

Class Program


    Shared Sub Main()
        AddHandler Server.OnClientConnect, AddressOf Program.Server_OnClientConnect
        AddHandler Server.OnClientDisconnect, AddressOf Program.Server_OnClientDisconnect
        AddHandler Server.OnReceiveData, AddressOf Program.Server_OnReceiveData
        AddHandler Server.OnServerError, AddressOf Program.Server_OnServerError
        AddHandler Server.OnServerStarted, AddressOf Program.Server_OnServerStarted
        AddHandler Server.OnServerStopped, AddressOf Program.Server_OnServerStopped
        AddHandler Server.OnServerLog, AddressOf Program.Server_OnServerLog
        AddHandler Server.OnServerPacketLog, AddressOf Program.Server_OnPacketLog

        AddHandler DataBase.OnDatabaseError, AddressOf Program.db_OnDatabaseError
        AddHandler Database.OnDatabaseConnected, AddressOf Program.db_OnDatabaseConnected
        AddHandler Database.OnDatabaseLog, AddressOf Program.db_OnDatabaseLog

        Console.WindowHeight = 10
        Console.BufferHeight = 300
        Console.WindowWidth = 60
        Console.BufferWidth = 60
        Console.BackgroundColor = ConsoleColor.White
        Console.ForegroundColor = ConsoleColor.DarkGreen
        Console.Clear()
        Console.Title = "GlobalManager ALPHA"
        Log.WriteSystemLog("Starting Server")

        Log.WriteSystemLog("Loading Settings.")
        Settings.LoadSettings()
        Settings.SetToServer()

        Log.WriteSystemLog("Loaded Settings. Conneting Database.")
        DataBase.Connect()

        Log.WriteSystemLog("Connected Database. Starting Server now.")
        GlobalDef.Initalize(Server.MaxClients)
        GlobalDb.UpdateData()
        Timers.LoadTimers()

        Server.Start()
        Log.WriteSystemLog("Inital Loading complete!")


        Do While True
            Dim msg As String = Console.ReadLine()
            CheckCommand(msg)
        Loop
    End Sub

    Private Shared Sub Server_OnClientConnect(ByVal ip As String, ByVal index As Integer)
        Log.WriteSystemLog("Client Connected : " & ip)
        Server.OnlineClients += 1

        SessionInfo(index) = New cSessionInfo_GlobalManager
        SessionInfo(index).BaseKey = Auth.GenarateKey

        Dim writer As New PacketWriter
        writer.Create(ServerOpcodes.HANDSHAKE)
        writer.Word(SessionInfo(index).BaseKey)
        Server.Send(writer.GetBytes, index)
    End Sub

    Private Shared Sub Server_OnClientDisconnect(ByVal ip As String, ByVal index As Integer)
        Log.WriteSystemLog("Client Disconnected : " & ip)

        SessionInfo(index) = Nothing

        If Server.OnlineClients > 0 Then
            Server.OnlineClients -= 1
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
            If Settings.Server_DebugMode = True Then
                Log.LogPacket(newbuff, False)
            End If

            Functions.Parse(packet, index_)
        Loop
    End Sub

    Private Shared Sub Server_OnServerError(ByVal ex As Exception, ByVal index As Integer)
        Log.WriteSystemLog("Server Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & index) '-1 = on client connect + -2 = on server start
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

    Private Shared Sub db_OnDatabaseConnected()
        Log.WriteSystemLog("Connected to database at: " & DateTime.Now.ToString())
    End Sub

    Private Shared Sub db_OnDatabaseLog(ByVal message As String)
        Log.WriteSystemLog("Database Log: " & message)
    End Sub

    Private Shared Sub db_OnDatabaseError(ByVal ex As Exception, ByVal command As String)
        Log.WriteSystemLog("Database error: " & ex.Message & " Command: " & command)
    End Sub
End Class


