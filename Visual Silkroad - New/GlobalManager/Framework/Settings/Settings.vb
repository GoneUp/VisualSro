Namespace GlobalManger
    Module Settings
        'Loading File
        Private File As New cINI(System.AppDomain.CurrentDomain.BaseDirectory & "settings_login\settings.ini")

        Public Database_IP As String
        Public Database_Port As UShort
        Public Database_User As String
        Public Database_Password As String
        Public Database_Database As String

        Public Server_Ip As String = "0.0.0.0"
        Public Server_Port As UShort = 15580
        Public Server_Slots As UInteger = 100

        Public Auto_Register As Boolean = False
        Public Max_FailedLogins As Integer = 5
        Public Max_RegistersPerDay As Integer = 3
        Public Server_CurrectVersion As UInteger = 0
        Public Server_Local As Byte = 0

        Public Log_Connect As Boolean = False
        Public Log_Register As Boolean = False
        Public Log_Login As Boolean = False

        Public Sub LoadSettings()
            Server_Ip = File.Read("SERVER_INTERNAL", "Ip", "0.0.0.0")
            Server_Port = File.Read("SERVER_INTERNAL", "Port", "15880")
            Server_Slots = File.Read("SERVER_INTERNAL", "Max_Slots", "1000")

            Database_IP = File.Read("DATABASE", "Ip", "127.0.0.1")
            Database_Port = File.Read("DATABASE", "Port", "3306")
            Database_Database = File.Read("DATABASE", "Database", "visualsro")
            Database_User = File.Read("DATABASE", "User", "root")
            Database_Password = File.Read("DATABASE", "Password", "")

            Auto_Register = CBool(File.Read("SERVER", "Auto_Register", "0"))
            Max_FailedLogins = File.Read("SERVER", "Max_FailedLogins", "5")
            Max_RegistersPerDay = File.Read("SERVER", "Max_RegistersPerDay", "3")
            Server_CurrectVersion = File.Read("SERVER", "Server_CurrectVersion", "0")
            Server_Local = File.Read("SERVER", "Server_Local", "0")

            Log_Connect = CBool(File.Read("LOG", "Connect", "0"))
            Log_Login = CBool(File.Read("LOG", "Login", "0"))
            Log_Register = CBool(File.Read("LOG", "Register", "0"))
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
