Namespace LoginServer
    Module Settings
        Public AutoRegister As Boolean = False
        Public MaxFailedLogins As Integer = 5

        Public Sub LoadSettings()
            AutoRegister = True
            MaxFailedLogins = 5
        End Sub



    End Module
End Namespace
