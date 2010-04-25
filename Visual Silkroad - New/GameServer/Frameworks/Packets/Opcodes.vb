Module Opcodes

    'All Opcodes for now

    'C --> S

    Enum ClientOpcodes

        'Login
        Ping = 2002
        Handshake = 9000
        PatchReq = 6100
        InfoReq = 2001 'cleint whoami
        Login = 6103
        CharList = 7007

    End Enum

    'S --> C

    Enum ServerOpcodes

        ServerInfo = &H2001 'Gateway
        PatchInfo = &H600D 'Patch Info
        LaucherInfo = &HA104
        ServerList = &HA101
        LoginAuthInfo = &HA103
        CharList = &HB007

    End Enum


End Module
