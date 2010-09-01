Imports Microsoft.VisualBasic
Imports System
Namespace GameServer


    Friend Class Program

        Public Shared Logpackets As Boolean = False

        Private Shared Sub db_OnConnectedToDatabase()
            Commands.WriteLog("Connected to database at: " & DateTime.Now.ToString())

        End Sub

        Private Shared Sub db_OnDatabaseError(ByVal ex As Exception)
            Commands.WriteLog("Database error: " & ex.Message)
        End Sub

        Shared Sub Main()
            AddHandler Server.OnClientConnect, AddressOf Program.Server_OnClientConnect
            AddHandler Server.OnClientDisconnect, AddressOf Program.Server_OnClientDisconnect
            AddHandler Server.OnReceiveData, AddressOf Program.Server_OnReceiveData
            AddHandler Server.OnServerError, AddressOf Program.Server_OnServerError
            AddHandler Server.OnServerStarted, AddressOf Program.Server_OnServerStarted
            AddHandler DataBase.OnDatabaseError, AddressOf Program.db_OnDatabaseError
            AddHandler DataBase.OnConnectedToDatabase, AddressOf Program.db_OnConnectedToDatabase


            Console.WindowHeight = 10
            Console.BufferHeight = 30
            Console.WindowWidth = 60
            Console.BufferWidth = 60
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.DarkGreen
            Console.Clear()
            Console.Title = "GAMESERVER ALPHA"
            Commands.WriteLog("Starting Agent Server")
			DataBase.Connect("127.0.0.1", 3306, "visualsro", "root", "")
            Server.ip = "127.0.0.1"
            Server.port = 15780
            Server.MaxClients = 1500
            Server.OnlineClient = 0
            Server.Start()
            Commands.WriteLog("Started Server. Loading Data now.")

            GameServer.DatabaseCore.UpdateData()
            SilkroadData.DumpItemFiles()

            Commands.WriteLog("Inital Loding complete!")

read:
            Dim msg As String = Console.ReadLine()
            CheckCommand(msg)
            GoTo read


        End Sub

        Private Shared Sub Server_OnClientConnect(ByVal ip As String, ByVal index As Integer)
            Commands.WriteLog("Client Connected : " & ip)
            Server.OnlineClient += 1

            Dim pack As New PacketWriter
            pack.Create(ServerOpcodes.Handshake)
            pack.Byte(1)
            GameServer.Server.Send(pack.GetBytes, index)

        End Sub

        Private Shared Sub Server_OnReceiveData(ByVal buffer() As Byte, ByVal index As Integer)

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

                Dim rp As New ReadPacket(newbuff, index)
                If Logpackets = True Then
                    PacketLog.LogPacket(rp, False, newbuff)
                End If

                Parser.Parse(rp)
            Loop



        End Sub

        Private Shared Sub Server_OnServerError(ByVal ex As Exception, ByVal index As Integer)
            Commands.WriteLog("Server Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & index) '-1 = on client connect + -2 = on server start
        End Sub

        Private Shared Sub Server_OnServerStarted(ByVal time As String)
            Commands.WriteLog("Server Started: " & time)
        End Sub

        Private Shared Sub Server_OnClientDisconnect(ByVal ip As String, ByVal index As Integer)
            Server.OnlineClient -= 1
            Try
                If Functions.PlayerData(index) IsNot Nothing Then
                    Functions.DespawnPlayer(index)
                End If
                GameServer.ClientList.OnCharListing(index) = Nothing
                Functions.PlayerData(index) = Nothing
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace




