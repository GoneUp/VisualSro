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
        Character = 7007
        IngameReq = 7001
        JoinWorldReq = 3012

    End Enum

    'S --> C

    Enum ServerOpcodes

        Handshake = &H5000
        ServerInfo = &H2001 'Gateway
        PatchInfo = &H600D 'Patch Info
        LaucherInfo = &HA104
        ServerList = &HA101
        LoginAuthInfo = &HA103
        Character = &HB007
        IngameReqRepley = &HB001

        LoadingStart = &H34A5
        CharacterInfo = &H3013
        LoadingEnd = &H34A6
        CharacterID = &H3020
        CharacterStats = &H303D
        JoinWorldReply = &H3809


    End Enum


End Module
