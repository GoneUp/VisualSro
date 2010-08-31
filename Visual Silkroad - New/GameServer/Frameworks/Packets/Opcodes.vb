Module Opcodes

    'All Opcodes for now

    'C --> S

    Enum ClientOpcodes As UInteger

        'Login
        Ping = 2002
        Handshake = 9000
        PatchReq = 6100
        InfoReq = 2001 'cleint whoami
        Login = 6103
        Character = 7007
        IngameReq = 7001
        JoinWorldReq = 3012

        'Ingame
        Movement = 7021
        GameMaster = 7010
        Chat = 7025
        Action = 704F
        Emotion = 3091
        [Exit] = 7005
        Target = 7045
        ItemMove = 7034
        Alchemy = 7150
        Angle_Update = 7024

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

        Movement = &HB021
        Chat_Accept = &HB025
        Chat = &H3026
        Action = &HBF30
        ItemMove = &HB034
        UniqueAnnonce = &H300C
        Alchemy = &HB150
        Angle_Update = &HB024
        Teleport_Start = &HB05A

        EquipItem = &H3038
        UnEquipItem = &H3039

        SingleSpawn = &H3015
        SingleDespawn = &H3016
        GroupSpawnStart = &H3017
        GroupSpawnData = &H3019
        GroupSpawnEnd = &H3018
        Target = &HB045

        [Exit] = &HB005
        Exit2 = &H300A

    End Enum


End Module
