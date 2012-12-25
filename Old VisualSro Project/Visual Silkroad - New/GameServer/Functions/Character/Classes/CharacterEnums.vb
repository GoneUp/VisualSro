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
    End Module
End Namespace
