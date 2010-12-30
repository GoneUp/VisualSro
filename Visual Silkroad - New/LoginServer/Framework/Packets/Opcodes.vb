Module Opcodes

    'All Opcodes for now

    'C --> S

    Enum ClientOpcodes
        Handshake = &H9000
        PatchReq = &H6100
        InfoReq = &H2001 'cleint whoami
        LauncherReq = &H6104
        Ping = &H2002

        ServerListReq = &H6101
        Login = &H6102

      End Enum

    'S --> C

    Enum ServerOpcodes

        Handshake = &H5000
        ServerInfo = &H2001 'Gateway
        MassiveMessage = &H600D 'Patch Info
        LaucherInfo = &HA104
        ServerList = &HA101
        LoginAuthInfo = &HA102

    End Enum


End Module
