Imports SRFramework

Namespace Settings
    Module Settings
        'Loading File
        Private File As New cINI(System.AppDomain.CurrentDomain.BaseDirectory & "settings_manager\settings.ini")

        Public Database_IP As String
        Public Database_Port As UShort
        Public Database_User As String
        Public Database_Password As String
        Public Database_Database As String

        Public Server_Ip As String = "0.0.0.0"
        Public Server_Port As UShort = 15580
        Public Server_NormalSlots As UInteger = 100
        Public Server_MaxClients As UInteger = 105
        Public Server_Id As UShort = 0
        Private _Server_DebugMode As Boolean = False
        Public Property Server_DebugMode 'Only for setting the PingDc on ClientList
            Get
                Return _Server_DebugMode
            End Get
            Set(ByVal value)
                _Server_DebugMode = value
                Server.Server_DebugMode = value
            End Set
        End Property

        Public Agent_Auto_Register As Boolean = False
        Public Agent_Max_FailedLogins As Integer = 5
        Public Agent_Max_RegistersPerDay As Integer = 3


        Public Const Server_ProtocolVersion As UInteger = 1

        Public Log_Connect As Boolean = False

        Public Sub LoadSettings()
            Server_Ip = File.Read("SERVER_INTERNAL", "Ip", "0.0.0.0")
            Server_Port = File.Read("SERVER_INTERNAL", "Port", "15880")
            Server_NormalSlots = File.Read("SERVER_INTERNAL", "Max_NormalSlots", "1000")
            Server_MaxClients = File.Read("SERVER_INTERNAL", "Max_Clients", "1050")
            Server_Id = File.Read("SERVER_INTERNAL", "Server_Id", "0")

            Database_IP = File.Read("DATABASE", "Ip", "127.0.0.1")
            Database_Port = File.Read("DATABASE", "Port", "3306")
            Database_Database = File.Read("DATABASE", "Database", "visualsro")
            Database_User = File.Read("DATABASE", "User", "root")
            Database_Password = File.Read("DATABASE", "Password", "sremu")

            Agent_Auto_Register = CBool(File.Read("AGENT", "Auto_Register", "0"))
            Agent_Max_FailedLogins = File.Read("AGENT", "Max_FailedLogins", "5")
            Agent_Max_RegistersPerDay = File.Read("AGENT", "Max_RegistersPerDay", "3")

            Log_Connect = CBool(File.Read("LOG", "Connect", "0"))
        End Sub

        Public Sub SetToServer()
            Database.DbIP = Database_IP
            Database.DbPort = Database_Port
            Database.DbDatabase = Database_Database
            Database.DbUsername = Database_User
            Database.DbPassword = Database_Password

            Server.Ip = Server_Ip
            Server.Port = Server_Port
            Server.MaxNormalClients = Server_NormalSlots
            Server.MaxClients = Server_MaxClients
            Server.Server_DebugMode = Server_DebugMode
        End Sub
    End Module
End Namespace
