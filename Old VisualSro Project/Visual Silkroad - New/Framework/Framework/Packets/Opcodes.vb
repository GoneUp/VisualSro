Public Module Opcodes

    'All Opcodes for now

    'C --> S

    Public Enum ClientOpcodes As UShort
        HANDSHAKE = &H5000
        HANDSHAKE_CONFIRM = &H9000
        PING = &H2002

        LOGIN_PATCH_REQ = &H6100
        LOGIN_WHO_AM_I = &H2001 'cleint whoami
        LOGIN_LAUNCHER_REQ = &H6104
        LOGIN_SERVER_LIST_REQ = &H6101
        LOGIN_LOGIN_REQ = &H6102

        'Agent
        GAME_AUTH = &H6103
        GAME_CHARACTER = &H7007
        GAME_INGAME_REQ = &H7001
        GAME_JOIN_WORLD_REQ = &H3012
        GAME_JOIN_WORLD_REQ2 = &H34C5

        'Ingame
        GAME_MOVEMENT = &H7021
        GAME_GAMEMASTER = &H7010
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

        'SR_ADMIN #### Only for comability, remove after globalmanager integration
        SR_Admin = &HAD01
    End Enum



    'S --> C 
    Public Enum ServerOpcodes As UShort
        HANDSHAKE = &H5000
        LOGIN_SERVER_INFO = &H2001 'Gateway
        LOGIN_MASSIVE_MESSAGE = &H600D 'Patch Info
        LOGIN_LAUNCHER_INFO = &HA104
        LOGIN_SERVER_LIST = &HA101
        LOGIN_AUTH = &HA102

        GAME_AUTH = &HA103
        GAME_CHARACTER = &HB007
        GAME_INGAME_REQ_REPLY = &HB001

        GAME_CAPATCHA = &H2322

        GAME_LOADING_START = &H34A5
        GAME_LOADING_END = &H34A6
        GAME_LOADING_START_2 = &H34B5
        GAME_LOADING_END_2 = &H34B6
        GAME_CHARACTER_INFO = &H3013
        GAME_CHARACTER_ID = &H3020
        GAME_CHARACTER_STATS = &H303D
        GAME_JOIN_WORLD_REPLY = &H3809
        GAME_MESSAGE_NOTICE = &H3B07
        GAME_CLIENT_STATUS = &HB0EA

        GAME_MOVEMENT = &HB021
        GAME_CHAT_ACCEPT = &HB025
        GAME_CHAT = &H3026
        GAME_ACTION = &H30BF
        GAME_ITEM_MOVE = &HB034
        GAME_UNIQUE_ANNONCE = &H300C
        GAME_ALCHEMY = &HB150
        GAME_ANGLE_UPDATE = &HB024
        GAME_TELEPORT_START = &HB05A
        GAME_WEATHER = &H3809
        GAME_TELEPORT_ANNONCE = &H34B5
        GAME_EMOTION = &H3091
        GAME_HELPER_ICON = &HB402
        GAME_SILK = &H3153

        GAME_NPC_CHAT = &HB046
        GAME_NPC_CHAT_LEFT = &HB04B
        GAME_NPC_TELEPORT_CONFIRM = &HB05A

        'Updates
        GAME_HP_MP_UPDATE = &H3057
        GAME_GOLD_UPDATE = &H304E
        GAME_EXP_UPDATE = &H3056
        GAME_LEVELUP_ANIMATION = &H3054
        GAME_SPEED_UPDATE = &H30D0

        'Guild
        GAME_GUILD_INFO = &H3101
        GAME_GUILD_LINK = &H30FF
        GAME_GUILD_LOGON = &H38F5

        'Die
        GAME_DIE_1 = &H3011
        GAME_DIE_2 = &H30D2

        'Items
        GAME_ITEM_USE = &HB04C
        GAME_ITEM_USE_OTHERPLAYER = &H305C
        GAME_EQUIP_ITEM = &H3038
        GAME_UNEQUIP_ITEM = &H3039
        GAME_PICKUP_ITEM = &H3036
        GAME_PICKUP_MOVE = &HB023
        GAME_ITEM_DELETE = &H304D

        'Spawns
        GAME_SINGLE_SPAWN = &H3015
        GAME_SINGLE_DESPAWN = &H3016
        GAME_GROUP_SPAWN_START = &H3017
        GAME_GROUP_SPAWN_DATA = &H3019
        GAME_GROUP_SPAWN_END = &H3018

        'Monster Stuff
        GAME_TARGET = &HB045
        GAME_ATTACK_REPLY = &HB074
        GAME_ATTACK_MAIN = &HB070

        GAME_BUFF_INFO = &HB071
        GAME_BUFF_ICON = &HB0BD
        GAME_BUFF_END = &HB072

        GAME_STR_UP = &HB050
        GAME_INT_UP = &HB051

        'Skills
        GAME_MASTERY_UP = &HB0A2
        GAME_SKILL_UP = &HB0A1

        'Exchange
        GAME_EXCHANGE_INVITE = &H3080
        GAME_EXCHANGE_INVITE_REPLY = &HB081
        GAME_EXCHANGE_START = &H3085
        GAME_EXCHANGE_UPDATE_ITEMS = &H308C
        GAME_EXCHANGE_CONFIRM_REPLY = &HB082
        GAME_EXCHANGE_CONFIRM_OTHER = &H3086
        GAME_EXCHANGE_APPROVE_REPLY = &HB083
        GAME_EXCHANGE_FINISH = &H3087
        GAME_EXCHANGE_ERROR = &H3088
        GAME_EXCHANGE_GOLD = &H3089
        GAME_EXCHANGE_ABORT_REPLY = &HB084

        'Stall
        GAME_STALL_OPEN_TO_OTHER = &H30B8
        GAME_STALL_REPLY = &HB0B1
        GAME_STALL_DATA = &HB0BA
        GAME_STALL_NAME = &H30BB
        GAME_STALL_ITEMS = &HB0B3
        GAME_STALL_BUY = &HB0B4
        GAME_STALL_MESSAGE = &H30B7
        GAME_STALL_CLOSE_OWNER_OTHER = &H30B9
        GAME_STALL_CLOSE_OWNER = &HB0B2
        GAME_STALL_CLOSE_VISITOR = &HB0B5

        'Exit
        GAME_EXIT_COUNTDOWN = &HB005
        GAME_EXIT_FINAL = &H300A

        'SR_ADMIN #### Only for comability, remove after globalmanager integration
        SR_Admin = &HBD01
    End Enum

    Enum InternalClientOpcodes As UShort
        SERVER_INIT = &H1000
        SERVER_SHUTDOWN = &H1001
        SERVER_INFO = &H1002
        SERVER_UPDATEREPLY = &H1003

        Admin_Auth = &H1004
        Admin_UpdateServer = &H1005
        Admin_GetInfo = &H1006
        Admin_UpdateInfo = &H1007

        GATEWAYSERVER_USERAUTH = &H1010
        GAMESERVER_CHECK_USERAUTH = &H1011
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
