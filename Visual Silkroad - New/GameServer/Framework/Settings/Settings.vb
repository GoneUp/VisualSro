Namespace GameServer.Settings
    Module Settings
        'Loading File
        Private File As New cINI(System.AppDomain.CurrentDomain.BaseDirectory & "settings_game\settings.ini")

        'Here is Place for Settings like Xp Rates, etc...
        Public Player_StartPos_Ch As New Position
        Public Player_StartPos_Eu As New Position
        Public Player_StartReturnPos As New Position
        Public Player_StartLevel As Byte
        Public Player_StartGold As ULong
        Public Player_StartMasteryLevel As Byte
        Public Player_StartSP As UInteger
        Public Player_StartGM As Boolean

        Public Database_IP As String
        Public Database_Port As UShort
        Public Database_User As String
        Public Database_Password As String
        Public Database_Database As String

        Public Server_XPRate As Long = 1
        Public Server_SPRate As Long = 1
        Public Server_GoldRate As Long = 1
        Public Server_DropRate As Long = 1
        Public Server_PingDc As Boolean = True

        Public Server_Ip As String = "0.0.0.0"
        Public Server_Port As UShort = 15580
        Public Server_Slots As UInteger = 100

        Public Server_LevelCap As Byte = 100
        Public Server_MasteryCap As UInteger = 300
        Public Server_Range As UInteger = 750
        Public Server_TaxRate As UInt16 = 0
        Public Server_SpawnsPerSec As UInteger = 50
        Public Server_SpawnRate As Integer = 2

        Public Log_Detail As Boolean = False
        Public Log_GM As Boolean = False
        Public Log_Mall As Boolean = False
        Public Log_Chat As Boolean = False

        'Enable or disable Mods
        Public ModGeneral As Boolean = True
        Public ModDamage As Boolean = False

        Public Sub LoadSettings()
            Server_Ip = File.Read("SERVER_INTERNAL", "Ip", "0.0.0.0")
            Server_Port = File.Read("SERVER_INTERNAL", "Port", "15880")
            Server_Slots = File.Read("SERVER_INTERNAL", "Max_Slots", "1000")

            Database_IP = File.Read("DATABASE", "Ip", "127.0.0.1")
            Database_Port = File.Read("DATABASE", "Port", "3306")
            Database_Database = File.Read("DATABASE", "Database", "visualsro")
            Database_User = File.Read("DATABASE", "User", "root")
            Database_Password = File.Read("DATABASE", "Password", "")

            Server_XPRate = File.Read("SERVER", "XP_Rate", "1")
            Server_SPRate = File.Read("SERVER", "SP_Rate", "1")
            Server_GoldRate = File.Read("SERVER", "Gold_Rate", "1")
            Server_DropRate = File.Read("SERVER", "Drop_Rate", "1")
            Server_LevelCap = File.Read("SERVER", "Level_Cap", "100")
            Server_MasteryCap = File.Read("SERVER", "Mastery_Cap", "300")
            Server_Range = File.Read("SERVER", "Range", "100")
            Server_TaxRate = File.Read("SERVER", "Tax_Rate", "0")
            Server_SpawnsPerSec = File.Read("SERVER", "SpawnsPerSec", "50")
            Server_SpawnRate = File.Read("SERVER", "Spawn_Rate", "1")

            Player_StartGold = File.Read("PLAYER_START", "Gold", "0")
            Player_StartLevel = File.Read("PLAYER_START", "Level", "0")
            Player_StartMasteryLevel = File.Read("PLAYER_START", "Mastery_Level", "0")
            Player_StartSP = File.Read("PLAYER_START", "Sp", "0")
            Player_StartGM = CBool(File.Read("PLAYER_START", "Gm", "0"))

            Player_StartPos_Ch.XSector = File.Read("PLAYER_START_POS", "Ch_Xsec", "168")
            Player_StartPos_Ch.YSector = File.Read("PLAYER_START_POS", "Ch_Ysec", "98")
            Player_StartPos_Ch.X = File.Read("PLAYER_START_POS", "Ch_X", "978")
            Player_StartPos_Ch.Z = File.Read("PLAYER_START_POS", "Ch_Z", "0")
            Player_StartPos_Ch.Y = File.Read("PLAYER_START_POS", "Ch_Y", "40")

            Player_StartPos_Eu.XSector = File.Read("PLAYER_START_POS", "EU_Xsec", "168")
            Player_StartPos_Eu.YSector = File.Read("PLAYER_START_POS", "EU_Ysec", "98")
            Player_StartPos_Eu.X = File.Read("PLAYER_START_POS", "EU_X", "978")
            Player_StartPos_Eu.Z = File.Read("PLAYER_START_POS", "EU_Z", "0")
            Player_StartPos_Eu.Y = File.Read("PLAYER_START_POS", "EU_Y", "40")

            Player_StartReturnPos.XSector = File.Read("PLAYER_START_POS", "RETURN_Xsec", "168")
            Player_StartReturnPos.YSector = File.Read("PLAYER_START_POS", "RETURN_Ysec", "97")
            Player_StartReturnPos.X = File.Read("PLAYER_START_POS", "RETURN_X", "980")
            Player_StartReturnPos.Z = File.Read("PLAYER_START_POS", "RETURN_X", "0")
            Player_StartReturnPos.Y = File.Read("PLAYER_START_POS", "RETURN_X", "40")

            Log_Chat = CBool(File.Read("LOG", "Chat", "0"))
            Log_GM = CBool(File.Read("LOG", "Gm", "0"))
            Log_Mall = CBool(File.Read("LOG", "Mall", "0"))
            Log_Detail = CBool(File.Read("LOG", "Detail", "0"))

            ModGeneral = CBool(File.Read("MOD", "General", "0"))
            ModDamage = CBool(File.Read("MOD", "Damage", "0"))

        End Sub


        Public Sub SetToServer()
            DataBase.DB_IP = Database_IP
            DataBase.DB_PORT = Database_Port
            DataBase.DB_DATABASE = Database_Database
            DataBase.DB_USERNAME = Database_User
            DataBase.DB_PASSWORD = Database_Password

            Server.Ip = Server_Ip
            Server.Port = Server_Port
            Server.MaxClients = Server_Slots
        End Sub
    End Module
End Namespace
