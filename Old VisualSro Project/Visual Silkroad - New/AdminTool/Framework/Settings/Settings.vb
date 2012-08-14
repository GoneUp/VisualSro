
Namespace Settings
    Module Settings
        'Loading File
        Private File As New SRFramework.cINI(AppDomain.CurrentDomain.BaseDirectory & "settings_admintool\settings.ini")

        Public Server_Id As UShort = 0

        Public GlobalManger_Ip As String = "0.0.0.0"
        Public GlobalManger_Port As UShort = 32000
        Public Const GlobalManager_ProtocolVersion As UInteger = 1

        Public Sub LoadSettings()
            Server_Id = File.Read("SERVER_INTERNAL", "Server_Id", "0")
            GlobalManger_Ip = File.Read("GLOBALMANAGER", "Ip", "127.0.0.1")
            GlobalManger_Port = File.Read("GLOBALMANAGER", "Port", "32000")
        End Sub

        Public Sub SetToServer()
    
        End Sub

    End Module
End Namespace
