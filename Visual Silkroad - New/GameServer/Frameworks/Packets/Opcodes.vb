Module Opcodes

	'All Opcodes for now

	'C --> S

	Enum ClientOpcodes As UInteger

		'Login
		Ping = &H2002
		Handshake = &H9000
		PatchReq = &H6100
		InfoReq = &H2001 'client whoami
		Login = &H6103
		Character = &H7007
		IngameReq = &H7001
        JoinWorldReq = &H3012
        JoinWorldReq2 = &H34C5

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
        HelperIcon = &H7402
        ItemUse = &H704C
        Scroll_Cancel = &H705B
        Hotkey_Update = &H7158

        Npc_Chat = &H7046
        Npc_Chat_Left = &H704B
        Npc_Teleport = &H705A


        Exchange_Invite = &H7081
        Exchange_Invite_Accept = &H3080
        Exchange_Confirm = &H7082
        Exchange_Approve = &H7083
        Exchange_Abort = &H7084

        Str_Up = &H7050
        Int_Up = &H7051

        'Skills
        Mastery_Up = &H70A2
        Skill_Up = &H70A1

	End Enum

	'S --> C

	Enum ServerOpcodes

		Handshake = &H5000
		ServerInfo = &H2001	'Gateway
		PatchInfo = &H600D 'Patch Info
		LaucherInfo = &HA104
		ServerList = &HA101
		LoginAuthInfo = &HA103
		Character = &HB007
		IngameReqRepley = &HB001

        LoadingStart = &H34A5
        LoadingEnd = &H34A6
        LoadingStart2 = &H34B5
        LoadingEnd2 = &H34B6
        CharacterInfo = &H3013
        CharacterID = &H3020
		CharacterStats = &H303D
		JoinWorldReply = &H3809

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

        'Items
        ItemUse = &HB04C
        ItemUseOtherPlayer = &H305C
		EquipItem = &H3038
        UnEquipItem = &H3039

        'Spawns
		SingleSpawn = &H3015
		SingleDespawn = &H3016
		GroupSpawnStart = &H3017
		GroupSpawnData = &H3019
		GroupSpawnEnd = &H3018
        Target = &HB045

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


		[Exit] = &HB005
		Exit2 = &H300A

	End Enum


End Module
