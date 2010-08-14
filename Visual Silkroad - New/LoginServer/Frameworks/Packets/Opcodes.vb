Module Opcodes

    'All Opcodes for now

    'C --> S

    Enum ClientOpcodes
        Handshake = 9000
        PatchReq = 6100
        InfoReq = 2001 'cleint whoami
        LauncherReq = 6104
        Ping = 2002

        ServerListReq = 6101
        Login = 6102

      End Enum

    'S --> C

    Enum ServerOpcodes

        Handshake = &H5000
        ServerInfo = &H2001 'Gateway
        PatchInfo = &H600D 'Patch Info
        LaucherInfo = &HA104
        ServerList = &HA101
        LoginAuthInfo = &HA102

    End Enum


End Module
