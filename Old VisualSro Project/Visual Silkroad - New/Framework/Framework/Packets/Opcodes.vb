Public Module Opcodes

    'All Opcodes for now

    'C --> S

    Public Enum ClientOpcodes As UInteger
        HANDSHAKE = &H5000
        HANDSHAKE_CONFIRM = &H9000
        PING = &H2002

        LOGIN_PATCH_REQ = &H6100
        LOGIN_WHO_AM_I = &H2001 'cleint whoami
        LOGIN_LAUNCHER_REQ = &H6104
        LOGIN_SERVER_LIST_REQ = &H6101
        LOGIN_LOGIN_REQ = &H6102

        'Agent
        GAME_CHARACTER = &H7007
        GAME_INGAME_REQ = &H7001
        GAME_JOIN_WORLD_REQ = &H3012
        GAME_JOIN_WORLD_REQ2 = &H34C5

        'Ingame
        GAME_MOVEMENT = &H7021
        GAME_GAME_MASTER = &H7010
        GAME_CHAT = &H7025
        GAME_ACTION = &H704F
        GAME_EMOTION = &H3091
        GAME_EXIT = &H7005
        GAME_TARGET = &H7045
        GAME_DEATH_RESPAWN = &H3053
        GAME_SET_RETURN_POINT = &H7059
        GAME_BERSERK_ACTIVATE = &H70A7

        GAME_ITEM_MOVE = &H7034
        GAME_ALCHEMY = &H7150
        GAME_ITEM_USE = &H704C
        GAME_SCROLL_CANCEL = &H705B
        GAME_BUY_BACK = &H7168

        GAME_ANGLE_UPDATE = &H7024
        GAME_TELEPORT_REPLY = &H34B6
        GAME_HELPER_ICON = &H7402
        GAME_HOTKEY_UPDATE = &H7158
        GAME_ATTACK = &H7074
        GAME_CLIENT_STATUS = &H70EA

        GAME_CTF_REGISTER = &H74B2

        GAME_NPC_CHAT = &H7046
        GAME_NPC_CHAT_LEFT = &H704B
        GAME_NPC_TELEPORT = &H705A

        GAME_EXCHANGE_INVITE = &H7081
        GAME_EXCHANGE_INVITE_ACCEPT = &H3080
        GAME_EXCHANGE_CONFIRM = &H7082
        GAME_EXCHANGE_APPROVE = &H7083
        GAME_EXCHANGE_ABORT = &H7084

        GAME_STR_UP = &H7050
        GAME_INT_UP = &H7051

        'Skills
        GAME_MASTERY_UP = &H70A2
        GAME_SKILL_UP = &H70A1

        'Stall
        GAME_STALL_OPEN = &H70B1
        GAME_STALL_CLOSE_OWN = &H70B2
        GAME_STALL_SELECT = &H70B3
        GAME_STALL_BUY = &H70B4
        GAME_STALL_CLOSE_VISITOR = &H70B5
        GAME_STALL_DATA = &H70BA
    End Enum



    'S --> C 
    Public Enum ServerOpcodes
        Handshake = &H5000
        ServerInfo = &H2001 'Gateway
        MassiveMessage = &H600D 'Patch Info
        LaucherInfo = &HA104
        ServerList = &HA101
        LoginAuthInfo = &HA102

        Character = &HB007
        IngameReqRepley = &HB001

        Capatcha = &H2322

        LoadingStart = &H34A5
        LoadingEnd = &H34A6
        LoadingStart2 = &H34B5
        LoadingEnd2 = &H34B6
        CharacterInfo = &H3013
        CharacterID = &H3020
        CharacterStats = &H303D
        JoinWorldReply = &H3809
        JoinWorldUnknown = &H3077
        JoinWorldUnknown2 = &H3305
        MessageNotice = &H3B07
        ClientStatus = &HB0EA

        Movement = &HB021
        Chat_Accept = &HB025
        Chat = &H3026
        Action = &H30BF
        ItemMove = &HB034
        UniqueAnnonce = &H300C
        Alchemy = &HB150
        Angle_Update = &HB024
        Teleport_Start = &HB05A
        Weather = &H3809
        Teleport_Annonce = &H34B5
        Emotion = &H3091
        HelperIcon = &HB402
        Silk = &H3153

        Npc_Chat = &HB046
        Npc_Chat_Left = &HB04B
        Npc_Teleport_Confirm = &HB05A

        'Updates
        HP_MP_Update = &H3057
        Gold_Update = &H304E
        Exp_Update = &H3056
        LevelUp_Animation = &H3054
        Speed_Update = &H30D0

        'Guild
        Guild_Info = &H3101
        Guild_Link = &H30FF
        Guild_Logon = &H38F5

        'Die
        Die_1 = &H3011
        Die_2 = &H30D2

        'Items
        ItemUse = &HB04C
        ItemUseOtherPlayer = &H305C
        EquipItem = &H3038
        UnEquipItem = &H3039
        PickUp_Item = &H3036
        PickUp_Move = &HB023
        ItemDelete = &H304D

        'Spawns
        SingleSpawn = &H3015
        SingleDespawn = &H3016
        GroupSpawnStart = &H3017
        GroupSpawnData = &H3019
        GroupSpawnEnd = &H3018

        'Monster Stuff
        Target = &HB045
        Attack_Reply = &HB074
        Attack_Main = &HB070

        Buff_Info = &HB071
        Buff_Icon = &HB0BD
        Buff_End = &HB072

        Str_Up = &HB050
        Int_Up = &HB051

        'Skills
        Mastery_Up = &HB0A2
        Skill_Up = &HB0A1

        'Exchange
        Exchange_Invite = &H3080
        Exchange_Invite_Reply = &HB081
        Exchange_Start = &H3085
        Exchange_UpdateItems = &H308C
        Exchange_Confirm_Reply = &HB082
        Exchange_Confirm_Other = &H3086
        Exchange_Approve_Reply = &HB083
        Exchange_Finsih = &H3087
        Exchange_Error = &H3088
        Exchange_Gold = &H3089
        Exchange_Abort_Reply = &HB084

        'Stall
        Stall_Open_ToOther = &H30B8
        Stall_Open_Reply = &HB0B1
        Stall_Data = &HB0BA
        Stall_Name = &H30BB
        Stall_Items = &HB0B3
        Stall_Buy = &HB0B4
        Stall_Message = &H30B7
        Stall_Close_Owner_Other = &H30B9
        Stall_Close_Owner = &HB0B2
        Stall_Close_Visitor = &HB0B5

        'Exit
        [Exit] = &HB005
        Exit2 = &H300A
    End Enum

    Enum InternalClientOpcodes
        Server_Init = &H1000
        Server_Shutdown = &H1001
        Server_Info = &H1002
        Server_UpdateReply = &H1003

        Admin_Auth = &H1004
        Admin_UpdateServer = &H1005
        Admin_GetInfo = &H1006
        Admin_UpdateInfo = &H1007

        GateWay_SendUserAuth = &H1010
        GameServer_CheckUserAuth = &H1011
    End Enum

    Enum InternalServerOpcodes As UShort
        SERVER_INIT = &HC000
        SERVER_SHUTDOWN = &HC001
        GLOBAL_INFO = &HC002
        SERVER_UPDATE = &HC003

        Admin_Auth = &HC004
        Admin_UpdateServer = &HC005
        Admin_GetInfo = &HC006
        Admin_UpdateInfo = &HC007

        GATEWAY_SEND_USERAUTH = &HC010
        GAMESERVER_CHECK_USERAUTH = &HC011
    End Enum


End Module
