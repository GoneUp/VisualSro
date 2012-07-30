Imports GlobalManager.Framework
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
        Public Server_Slots As UInteger = 100
        Public Server_Id As UShort = 0

        Public Const Server_ProtocolVersion As UInteger = 1

        Public Log_Connect As Boolean = False
        Public Log_Packets As Boolean = False

        Public Sub LoadSettings()
            Server_Ip = File.Read("SERVER_INTERNAL", "Ip", "0.0.0.0")
            Server_Port = File.Read("SERVER_INTERNAL", "Port", "15880")
            Server_Slots = File.Read("SERVER_INTERNAL", "Max_Slots", "1000")
            Server_Id = File.Read("SERVER_INTERNAL", "Server_Id", "0")

            Database_IP = File.Read("DATABASE", "Ip", "127.0.0.1")
            Database_Port = File.Read("DATABASE", "Port", "3306")
            Database_Database = File.Read("DATABASE", "Database", "visualsro")
            Database_User = File.Read("DATABASE", "User", "root")
            Database_Password = File.Read("DATABASE", "Password", "sremu")

            Log_Connect = CBool(File.Read("LOG", "Connect", "0"))
        End Sub

        Public Sub SetToServer()
            Database.DbIP = Database_IP
            Database.DbPort = Database_Port
            Database.DbDatabase = Database_Database
            Database.DbUsername = Database_User
            Database.DbPassword = Database_Password

            Server.IP = Server_Ip
            Server.Port = Server_Port
            Server.MaxClients = Server_Slots
        End Sub

    End Module
End Namespace
