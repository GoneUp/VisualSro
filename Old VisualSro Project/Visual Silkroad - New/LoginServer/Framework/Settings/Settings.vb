
Namespace Settings
    Module Settings
        'Loading File
        Private ReadOnly File As New SRFramework.cINI(AppDomain.CurrentDomain.BaseDirectory & "settings_login\settings.ini")

        Private m_databaseIp As String
        Private m_databasePort As UShort
        Private m_databaseUser As String
        Private m_databasePassword As String
        Private m_databaseDatabase As String

        Public GlobalMangerIp As String = "0.0.0.0"
        Public GlobalMangerPort As UShort = 32000
        Public Const GlobalManagerProtocolVersion As UInteger = 1

        Public ServerIp As String = "0.0.0.0"
        Public ServerPort As UShort = 15780
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


        Public ServerCurrectVersion As UInteger = 0
        Public ServerLocal As Byte = 0

        Public LogConnect As Boolean = False
        Public LogLogin As Boolean = False

        Public Sub LoadSettings()
            ServerIp = File.Read("SERVER_INTERNAL", "Ip", "0.0.0.0")
            ServerPort = File.Read("SERVER_INTERNAL", "Port", "15880")
            ServerNormalSlots = File.Read("SERVER_INTERNAL", "Max_NormalSlots", "1000")
            ServerMaxClients = File.Read("SERVER_INTERNAL", "Max_Clients", "1050")
            ServerId = File.Read("SERVER_INTERNAL", "Server_Id", "0")

            m_databaseIp = File.Read("DATABASE", "Ip", "127.0.0.1")
            m_databasePort = File.Read("DATABASE", "Port", "3306")
            m_databaseDatabase = File.Read("DATABASE", "Database", "visualsro")
            m_databaseUser = File.Read("DATABASE", "User", "root")
            m_databasePassword = File.Read("DATABASE", "Password", "")

            GlobalMangerIp = File.Read("GLOBALMANAGER", "Ip", "127.0.0.1")
            GlobalMangerPort = File.Read("GLOBALMANAGER", "Port", "32000")

            ServerCurrectVersion = File.Read("SERVER", "Server_CurrectVersion", "0")
            ServerLocal = File.Read("SERVER", "Server_Local", "0")

            LogConnect = CBool(File.Read("LOG", "Connect", "0"))
            LogLogin = CBool(File.Read("LOG", "Login", "0"))
        End Sub

        Public Sub SetToServer()
            Database.DbIP = m_databaseIp
            Database.DbPort = m_databasePort
            Database.DbDatabase = m_databaseDatabase
            Database.DbUsername = m_databaseUser
            Database.DbPassword = m_databasePassword

            Server.Ip = ServerIp
            Server.Port = ServerPort
            Server.MaxNormalClients = ServerNormalSlots
            Server.MaxClients = ServerMaxClients
            Server.Server_DebugMode = ServerDebugMode
        End Sub

    End Module
End Namespace
