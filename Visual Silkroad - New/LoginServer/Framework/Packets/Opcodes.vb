Namespace Framework
    Module Opcodes

        'All Opcodes for now

        'C --> S

        Enum ClientOpcodes As UShort
            Handshake = &H5000
            Handshake_Confirm = &H9000
            Ping = &H2002

            Login_PatchReq = &H6100
            Login_InfoReq = &H2001 'cleint whoami
            Login_LauncherReq = &H6104
            Login_ServerListReq = &H6101
            Login_LoginReq = &H6102

            Server_WhoAmI = &H6100 'cleint whoami
            Server_Init = &H1000
            Server_Shutdown = &H1001
            Server_Info = &H1002
            Server_UpdateReply = &H1003
            GateWay_SendUserAuth = &H1010
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
