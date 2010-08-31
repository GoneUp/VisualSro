Module Opcodes

    'All Opcodes for now

    'C --> S

    Enum ClientOpcodes As UInteger

        'Login
        Ping = &H2002
        Handshake = &H9000
        PatchReq = &H6100
        InfoReq = &H2001 'cleint whoami
        Login = &H6103
        Character = &H7007
        IngameReq = &H7001
        JoinWorldReq = &H3012

        'Ingame
        Movement = &H7021
        GameMaster = &H7010
        Chat = &H7025
        Action = &H704F
        Emotion = &H3091
        [Exit] = &H7005
        Target = &H7045
        ItemMove = &H7034
        Alchemy = &H7150
        Angle_Update = &H7024
        Teleport_Reply = &H34B6


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
        Weather = &H3809
        Teleport_Annonce = &H34B5

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
