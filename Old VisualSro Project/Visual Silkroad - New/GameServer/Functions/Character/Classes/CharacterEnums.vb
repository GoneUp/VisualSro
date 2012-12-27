Namespace Functions
    Module CharacterEnums
        Public Enum UseItemTypes
            None = 0
            Pot = 1
            ReturnScroll = 2
            ReverseScrollDead = 3
            ReverseScrollRecall = 4
            ReverseScrollPoint = 5
        End Enum

        Public Enum TeleportTypes
            None = 0
            Npc = 1
            GM = 2
        End Enum

        Public Enum AttackTypes
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

        Enum SilkroadObjectTypes
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

    End Module
End Namespace
