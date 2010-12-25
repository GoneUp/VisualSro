Namespace LoginServer
    Module Settings
        Public AutoRegister As Boolean = False
        Public MaxFailedLogins As Integer = 5
        Public MaxRegistersPerDay As Integer = 3

        Public Log_Connect As Boolean = False
        Public Log_Register As Boolean = False
        Public Log_Login As Boolean = False

        Public Sub LoadSettings()
            AutoRegister = True
            MaxFailedLogins = 5
            MaxRegistersPerDay = 3

            Log_Connect = True
            Log_Login = True
            Log_Register = True
        End Sub



    End Module
End Namespace
