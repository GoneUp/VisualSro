Imports Microsoft.VisualBasic, System.Threading
Namespace GameServer


    Friend Class Program

        Public Shared Logpackets As Boolean = False

        Public Shared TheardDB As New Thread(AddressOf GameServer.GameDB.UpdateData)
        Public Shared TheardLoad As New Thread(AddressOf SilkroadData.DumpDataFiles)
        Public Shared TheardSettings As New Thread(AddressOf LoadSettings)
        Public Shared TheardTimer As New Thread(AddressOf LoadTimers)
        Public Shared TheardServer As New Thread(AddressOf Server.Start)


        Shared Sub Main()
            AddHandler Server.OnClientConnect, AddressOf Program.Server_OnClientConnect
            AddHandler Server.OnClientDisconnect, AddressOf Program.Server_OnClientDisconnect
            AddHandler Server.OnReceiveData, AddressOf Program.Server_OnReceiveData
            AddHandler Server.OnServerError, AddressOf Program.Server_OnServerError
            AddHandler Server.OnServerStarted, AddressOf Program.Server_OnServerStarted
            AddHandler DataBase.OnDatabaseError, AddressOf Program.db_OnDatabaseError
            AddHandler DataBase.OnConnectedToDatabase, AddressOf Program.db_OnConnectedToDatabase

            Console.WindowHeight = 20
            Console.BufferHeight = 50
            Console.WindowWidth = 70
            Console.BufferWidth = 70
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.DarkGreen
            Console.Clear()
            Console.Title = "GAMESERVER ALPHA"

            GameServer.Log.WriteSystemLog("Connecting Database.")

            DataBase.Connect("127.0.0.1", 3306, "visualsro", "root", "sremu")
            Server.ip = "78.111.78.27"
            Server.port = 15780
            Server.MaxClients = 1500
            Server.OnlineClient = 0
            GameServer.Log.WriteSystemLog("Connected. Loading Data now.")

            TheardDB.Start()
            TheardLoad.Start()
            TheardSettings.Start()
            TheardTimer.Start(1500)
            GameServer.Log.WriteSystemLog("Data Loaded. Starting Server.")

            TheardServer.Start()

            GameServer.Log.WriteSystemLog("Inital Loding complete!")


read:
            Dim msg As String = Console.ReadLine()
            CheckCommand(msg)
            GoTo read



        End Sub

        Private Shared Sub Server_OnClientConnect(ByVal ip As String, ByVal index As Integer)
            If Log_Detail Then
                GameServer.Log.WriteSystemLog("Client Connected : " & ip)
            End If

            Server.OnlineClient += 1

            Dim packet As New PacketWriter
            packet.Create(ServerOpcodes.Handshake)
            packet.Byte(1)
            GameServer.Server.Send(packet.GetBytes, index)
        End Sub

        Private Shared Sub Server_OnReceiveData(ByVal buffer() As Byte, ByVal index_ As Integer)

            Dim Position As Integer = 0

            Do While True
                Dim length As Integer = BitConverter.ToUInt16(buffer, Position)
                Dim opc As Integer = BitConverter.ToUInt16(buffer, Position + 2)

                If length = 0 And opc = 0 Then 'endless prevention
                    Exit Do
                End If

                Dim newbuff(length + 5) As Byte
                Array.ConstrainedCopy(buffer, Position, newbuff, 0, length + 6)
                Position = Position + length + 6

                Dim packet As New PacketReader(newbuff)
                If Logpackets = True Then
                    Log.LogPacket(newbuff, False)
                End If

                Parser.Parse(packet, index_)
            Loop



        End Sub

        Private Shared Sub Server_OnServerError(ByVal ex As Exception, ByVal index As Integer)
            GameServer.Log.WriteSystemLog("Server Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & index) '-1 = on client connect + -2 = on server start
        End Sub

        Private Shared Sub Server_OnServerStarted(ByVal time As String)
            GameServer.Log.WriteSystemLog("Server Started: " & time)
        End Sub

        Private Shared Sub Server_OnClientDisconnect(ByVal ip As String, ByVal index As Integer)
            Try
                Server.OnlineClient -= 1
                If Functions.PlayerData(index) IsNot Nothing Then
                    Functions.DespawnPlayer(index)
                    Functions.CleanUpPlayer(index)
                End If
                GameServer.ClientList.CharListing(index) = Nothing
                Functions.PlayerData(index) = Nothing

                'Server.RevTheard(index).Abort()
                'Server.RevTheard(index) = Nothing
            Catch ex As Exception
                Functions.PlayerData(index) = Nothing
            End Try
        End Sub

        Private Shared Sub db_OnConnectedToDatabase()
            GameServer.Log.WriteSystemLog("Connected to database at: " & DateTime.Now.ToString())

        End Sub

        Private Shared Sub db_OnDatabaseError(ByVal ex As Exception, ByVal command As String)
            GameServer.Log.WriteSystemLog("Database error: " & ex.Message & " Command: " & command)
        End Sub
    End Class
End Namespace




