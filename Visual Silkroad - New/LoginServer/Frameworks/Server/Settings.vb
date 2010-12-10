Namespace LoginServer
    Module Settings
        Public AutoRegister As Boolean = False
        Public MaxFailedLogins As Integer = 5
        Public MaxRegistersPerDay As Integer = 3

        Public Sub LoadSettings()
            AutoRegister = True
            MaxFailedLogins = 5
            MaxRegistersPerDay = 3
        End Sub



    End Module
End Namespace
