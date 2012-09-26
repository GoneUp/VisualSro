Imports GameServer.Functions
Imports SRFramework

Namespace Settings
    Module Settings
        'Loading File
        Private ReadOnly File As New cINI(AppDomain.CurrentDomain.BaseDirectory & "settings_game\settings.ini")

        'Genral 
        Public ServerIp As String = "0.0.0.0"
        Public ServerPort As UShort = 15580
        Public ServerNormalSlots As UInteger = 100
        Public ServerMaxClients As UInteger = 105
        Public ServerId As UShort = 0
        Public ServerName As String = "UNKNOWN"

        Public DatabaseIp As String
        Public DatabasePort As UShort
        Public DatabaseUser As String
        Public DatabasePassword As String
        Public DatabaseDatabase As String

        Public GlobalMangerIp As String = "0.0.0.0"
        Public GlobalMangerPort As UShort = 32000
        Public Const GlobalManagerProtocolVersion As UInteger = 1

        'Here is Place for Settings like Xp Rates, etc...
        Public PlayerStartPosCh As New Position
        Public PlayerStartPosEu As New Position
        Public PlayerStartReturnPos As New Position

        Public PlayerStartLevel As Byte = 1
        Public PlayerStartGold As ULong = 0
        Public PlayerStartMasteryLevel As Byte = 0
        Public PlayerStartSP As UInteger = 0
        Public PlayerStartGM As Boolean = False
        Public PlayerStartItemsPlusMin As Byte = 0
        Public PlayerStartItemsPlusMax As Byte = 0

        Public ServerXPRate As Long = 1
        Public ServerSPRate As Long = 1
        Public ServerGoldRate As Long = 1
        Public ServerDropRate As Long = 1

        Private m_serverDebugMode As Boolean = True
        Public Property ServerDebugMode As Boolean  'Only for setting the PingDc on ClientList
            Get
                Return m_serverDebugMode
            End Get
            Set(ByVal value As Boolean)
                m_serverDebugMode = value
                Server.Server_DebugMode = value
            End Set
        End Property


        Public ServerLevelCap As Byte = 100
        Public ServerMasteryCap As UInteger = 300
        Public ServerRange As UInteger = 750
        Public ServerTaxRate As UInt16 = 0
        Public ServerSpawnsPerSec As UInteger = 50
        Public ServerSpawnRate As Integer = 2

        Public ServerWorldChannel As UInt32 = 1

        Public LogDetail As Boolean = False
        Public LogGM As Boolean = False
        Public LogMall As Boolean = False
        Public LogChat As Boolean = False

        'Enable or disable Mods
        Public ModGeneral As Boolean = True
        Public ModDamage As Boolean = False

        Public Sub LoadSettings()
            ServerIp = File.Read("SERVER_INTERNAL", "Ip", "0.0.0.0")
            ServerPort = File.Read("SERVER_INTERNAL", "Port", "15880")
            ServerNormalSlots = File.Read("SERVER_INTERNAL", "Max_NormalSlots", "1000")
            ServerMaxClients = File.Read("SERVER_INTERNAL", "Max_Clients", "1050")
            ServerId = File.Read("SERVER_INTERNAL", "Server_Id", "0")
            ServerName = File.Read("SERVER_INTERNAL", "Server_Name", "UNKNOWN")

            DatabaseIp = File.Read("DATABASE", "Ip", "127.0.0.1")
            DatabasePort = File.Read("DATABASE", "Port", "3306")
            DatabaseDatabase = File.Read("DATABASE", "Database", "visualsro")
            DatabaseUser = File.Read("DATABASE", "User", "root")
            DatabasePassword = File.Read("DATABASE", "Password", "")

            GlobalMangerIp = File.Read("GLOBALMANAGER", "Ip", "127.0.0.1")
            GlobalMangerPort = File.Read("GLOBALMANAGER", "Port", "32000")

            ServerXPRate = File.Read("SERVER", "XP_Rate", "1")
            ServerSPRate = File.Read("SERVER", "SP_Rate", "1")
            ServerGoldRate = File.Read("SERVER", "Gold_Rate", "1")
            ServerDropRate = File.Read("SERVER", "Drop_Rate", "1")
            ServerLevelCap = File.Read("SERVER", "Level_Cap", "100")
            ServerMasteryCap = File.Read("SERVER", "Mastery_Cap", "300")
            ServerRange = File.Read("SERVER", "Range", "100")
            ServerTaxRate = File.Read("SERVER", "Tax_Rate", "0")
            ServerSpawnsPerSec = File.Read("SERVER", "SpawnsPerSec", "50")
            ServerSpawnRate = File.Read("SERVER", "Spawn_Rate", "1")

            PlayerStartGold = File.Read("PLAYER_START", "Gold", "0")
            PlayerStartLevel = File.Read("PLAYER_START", "Level", "0")
            PlayerStartMasteryLevel = File.Read("PLAYER_START", "Mastery_Level", "0")
            PlayerStartSP = File.Read("PLAYER_START", "Sp", "0")
            PlayerStartGM = CBool(File.Read("PLAYER_START", "Gm", "0"))
            PlayerStartItemsPlusMin = File.Read("PLAYER_START", "ItemPlusMin", "0")
            PlayerStartItemsPlusMax = File.Read("PLAYER_START", "ItemPlusMax", "0")

            PlayerStartPosCh.XSector = File.Read("PLAYER_START_POS", "Ch_Xsec", "168")
            PlayerStartPosCh.YSector = File.Read("PLAYER_START_POS", "Ch_Ysec", "98")
            PlayerStartPosCh.X = File.Read("PLAYER_START_POS", "Ch_X", "978")
            PlayerStartPosCh.Z = File.Read("PLAYER_START_POS", "Ch_Z", "0")
            PlayerStartPosCh.Y = File.Read("PLAYER_START_POS", "Ch_Y", "40")

            PlayerStartPosEu.XSector = File.Read("PLAYER_START_POS", "EU_Xsec", "168")
            PlayerStartPosEu.YSector = File.Read("PLAYER_START_POS", "EU_Ysec", "98")
            PlayerStartPosEu.X = File.Read("PLAYER_START_POS", "EU_X", "978")
            PlayerStartPosEu.Z = File.Read("PLAYER_START_POS", "EU_Z", "0")
            PlayerStartPosEu.Y = File.Read("PLAYER_START_POS", "EU_Y", "40")

            PlayerStartReturnPos.XSector = File.Read("PLAYER_START_POS", "RETURN_Xsec", "168")
            PlayerStartReturnPos.YSector = File.Read("PLAYER_START_POS", "RETURN_Ysec", "97")
            PlayerStartReturnPos.X = File.Read("PLAYER_START_POS", "RETURN_X", "980")
            PlayerStartReturnPos.Z = File.Read("PLAYER_START_POS", "RETURN_X", "0")
            PlayerStartReturnPos.Y = File.Read("PLAYER_START_POS", "RETURN_X", "40")

            LogChat = CBool(File.Read("LOG", "Chat", "0"))
            LogGM = CBool(File.Read("LOG", "Gm", "0"))
            LogMall = CBool(File.Read("LOG", "Mall", "0"))
            LogDetail = CBool(File.Read("LOG", "Detail", "0"))

            ModGeneral = CBool(File.Read("MOD", "General", "0"))
            ModDamage = CBool(File.Read("MOD", "Damage", "0"))
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
