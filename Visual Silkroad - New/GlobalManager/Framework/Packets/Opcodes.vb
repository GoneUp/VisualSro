Namespace GlobalManager
    Module Opcodes

        'All Opcodes for now

        'C --> S

        Enum ClientOpcodes As UShort
            Handshake = &H5000
            PatchReq = &H6100
            WhoAmI = &H2001 'cleint whoami
            Ping = &H2002

            ServerListReq = &H6101
            Login = &H6102

        End Enum

        'S --> C

        Enum ServerOpcodes As UShort

            Handshake = &H5000
            ServerInfo = &H2001 'Gateway
            MassiveMessage = &H600D 'Patch Info
            LaucherInfo = &HA104
            ServerList = &HA101
            LoginAuthInfo = &HA102

        End Enum


    End Module
End Namespace
