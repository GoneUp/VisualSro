Namespace Functions
    Module StaticEnums
        Public Enum UseItemTypes
            None = 0
            Pot = 1
            ReturnScroll = 2
            ReverseScrollDead = 3
            ReverseScrollRecall = 4
            ReverseScrollPoint = 5
        End Enum

        Public Enum TeleportTypes As Byte
            None = 0
            Npc = 1
            GM = 2
        End Enum

        Public Enum AttackTypes As Byte
            Normal = 0
            Skill = 1
            Buff = 2
        End Enum

        Public Enum ExitTypes As Byte
            NormalExit = 0
            Reconnect = 1
        End Enum

        Public Enum JobTypes As Byte
            Hunter = 1
            Thief = 2
        End Enum

        Enum SilkroadObjectTypes As Byte
            MobNormal = 0
            Npc = 1
            Teleport = 2
            [Structure] = 3
            MobCave = 4
            COS = 5
            MobUnique = 6
            MobQuest = 7
            Character = 8
            Trade = 9
            MovePet = 10
        End Enum

        Enum SkillTypeTable As Byte
            Phy = &H1
            Mag = &H2
            Bicheon = &H3
            Heuksal = &H4
            Bow = &H5
            All = &H6
        End Enum

        Enum SkilActiveTypes As Byte
            Active = 3
            Passive = 4
        End Enum

        Enum ChatModes As Byte
            AllChat = &H1
            PmIncome = &H2
            GameMaster = &H3
            Party = &H4
            Guild = &H5
            Globals = &H6
            Notice = &H7
            Stall = &H9
            Union = &HB
            Academy = &H10
        End Enum
    End Module
End Namespace
