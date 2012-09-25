Imports SRFramework

Namespace Settings
    Module Settings
        'Loading File
        Private ReadOnly File As New cINI(System.AppDomain.CurrentDomain.BaseDirectory & "settings_manager\settings.ini")

        Public DatabaseIp As String
        Public DatabasePort As UShort
        Public DatabaseUser As String
        Public DatabasePassword As String
        Public DatabaseDatabase As String

        Public ServerIp As String = "0.0.0.0"
        Public ServerPort As UShort = 15580
        Public ServerNormalSlots As UInteger = 100
        Public ServerMaxClients As UInteger = 105
        Public ServerId As UShort = 0

        Private m_serverDebugMode As Boolean = False
        Public Property ServerDebugMode 'Only for setting the PingDc on ClientList
            Get
                Return m_serverDebugMode
            End Get
            Set(ByVal value)
                m_serverDebugMode = value
                Server.Server_DebugMode = value
            End Set
        End Property

        Public AgentAutoRegister As Boolean = False
        Public AgentMaxFailedLogins As Integer = 5
        Public AgentMaxRegistersPerDay As Integer = 3

        Public Const ServerProtocolVersion As UInteger = 1

        Public LogConnect As Boolean = False
        Public LogRegister As Boolean = False

        Public Sub LoadSettings()
            ServerIp = File.Read("SERVER_INTERNAL", "Ip", "0.0.0.0")
            ServerPort = File.Read("SERVER_INTERNAL", "Port", "15880")
            ServerNormalSlots = File.Read("SERVER_INTERNAL", "Max_NormalSlots", "1000")
            ServerMaxClients = File.Read("SERVER_INTERNAL", "Max_Clients", "1050")
            ServerId = File.Read("SERVER_INTERNAL", "Server_Id", "0")

            DatabaseIp = File.Read("DATABASE", "Ip", "127.0.0.1")
            DatabasePort = File.Read("DATABASE", "Port", "3306")
            DatabaseDatabase = File.Read("DATABASE", "Database", "visualsro")
            DatabaseUser = File.Read("DATABASE", "User", "root")
            DatabasePassword = File.Read("DATABASE", "Password", "sremu")

            AgentAutoRegister = CBool(File.Read("AGENT", "Auto_Register", "0"))
            AgentMaxFailedLogins = File.Read("AGENT", "Max_FailedLogins", "5")
            AgentMaxRegistersPerDay = File.Read("AGENT", "Max_RegistersPerDay", "3")

            LogConnect = CBool(File.Read("LOG", "Connect", "0"))
            LogRegister = CBool(File.Read("LOG", "Register", "0"))
        End Sub

        Public Sub SetToServer()
            Database.DbIP = DatabaseIp
            Database.DbPort = DatabasePort
            Database.DbDatabase = DatabaseDatabase
            Database.DbUsername = DatabaseUser
            Database.DbPassword = DatabasePassword

            Server.Ip = ServerIp
            Server.Port = ServerPort
            Server.MaxNormalClients = ServerNormalSlots
            Server.MaxClients = ServerMaxClients
            Server.Server_DebugMode = ServerDebugMode
        End Sub
    End Module
End Namespace
