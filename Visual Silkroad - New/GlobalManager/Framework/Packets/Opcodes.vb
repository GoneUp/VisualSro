Namespace Framework

    Module Opcodes

        'All Opcodes for now

        'C --> S

        Enum ClientOpcodes As UShort
            Handshake = &H5000
            Ping = &H2002
            WhoAmI = &H2001 'cleint whoami

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

        'S --> C

        Enum ServerOpcodes As UShort
            Handshake = &H5000
            ServerInfo = &H2001 'Gateway

            Server_Init = &HC000
            Server_Shutdown = &HC001
            ShardInfo = &HC002
            Server_Update = &HC003

            Admin_Auth = &HC004
            Admin_UpdateServer = &HC005
            Admin_GetInfo = &HC006
            Admin_UpdateInfo = &HC007

            GateWay_SendUserAuth = &HC010
            GameServer_CheckUserAuth = &HC011
        End Enum


    End Module
End Namespace
