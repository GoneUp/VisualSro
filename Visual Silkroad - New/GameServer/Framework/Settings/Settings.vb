Namespace GameServer.Settings
    Module Settings
        'Here is Place for Settings like Xp Rates, etc...
        Public Player_StartPos As New Position
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
            'TODO: Load these Settings from a Config File
            Player_StartPos.XSector = 168
            Player_StartPos.YSector = 98
            Player_StartPos.X = 978
            Player_StartPos.Z = 0
            Player_StartPos.Y = 40

            Player_StartReturnPos.XSector = 168
            Player_StartReturnPos.YSector = 97
            Player_StartReturnPos.X = 980
            Player_StartReturnPos.Z = 0
            Player_StartReturnPos.Y = 1330

            Player_StartGold = 1000000 '1m
            Player_StartLevel = 1
            Player_StartMasteryLevel = 0
            Player_StartSP = 1000000 '1m
            Player_StartGM = True

            Server_XPRate = 50
            Server_SPRate = 50
            Server_GoldRate = 50
            Server_DropRate = 50
            Server_LevelCap = 100
            Server_MasteryCap = 300
            Server_Range = 100
            Server_TaxRate = 20
            Server_SpawnsPerSec = 50

            Log_Chat = True
            Log_GM = True
            Log_Mall = True

            ModGeneral = True
            ModDamage = True

            Database_IP = "127.0.0.1"
            Database_Database = "visualsro"
            Database_Port = 3306
            Database_User = "root"
            Database_Password = "sremu"
        End Sub


        Public Sub SetToServer()
            DataBase.DB_IP = Database_IP
            DataBase.DB_PORT = Database_Port
            DataBase.DB_DATABASE = Database_Database
            DataBase.DB_USERNAME = Database_User
            DataBase.DB_PASSWORD = Database_Password

        End Sub
    End Module
End Namespace
