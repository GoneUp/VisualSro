Public Class [cChar]

    Public AccountID As UInteger
    Public CharacterName As String
    Public CharacterId As UInteger
    Public UniqueId As UInteger
    Public HP As UInteger
    Public MP As UInteger
    Public CHP As Integer
    Public CMP As Integer
    Public Model As UInteger
    Public Volume As Byte
    Public Level As Byte
    Public Experience As ULong
    Public Gold As ULong
    Public SkillPoints As UInteger
    Public SkillPointBar As ULong
    Public Attributes As UShort
    Public BerserkBar As Byte
    Public Berserk As Byte
    Public WalkSpeed As Single
    Public RunSpeed As Single
    Public BerserkSpeed As Single
    Public MinPhy As UInteger
    Public MaxPhy As UInteger
    Public MinMag As UInteger
    Public MaxMag As UInteger
    Public PhyDef As UShort
    Public MagDef As UShort
    Public Hit As UShort
    Public Parry As UShort
    Public Strength As UShort
    Public Intelligence As UShort
    Public GM As Boolean
    Public PVP As Byte
    Public MaxSlots As Byte
    Public Angle As UInt16
    Public Deleted As Boolean
    Public DeletionTime As DateTime
    Public HelperIcon As Byte
    Public ActionFlag As Byte
    Public Busy As Boolean

    Public UsedItem As UseItemTypes
    Public UsedItemParameter As Integer

    Public Position As New Position

    Public Position_Recall As New Position
    Public Position_Return As New Position
    Public Position_Dead As New Position

    Public Position_FromPos As Position
    Public Position_ToPos As Position

    Public WalkStart As New Date
    Public WalkEnd As New Date
    Public Walking As Boolean = False
    Public MovementType As MoveType_ = MoveType_.Run

    Public SpawnedPlayers As New List(Of Integer)
    Public SpawnedMonsters As New List(Of Integer)
    Public SpawnedItems As New List(Of Integer)
    Public SpawnedNPCs As New List(Of Integer)

    Public Ingame As Boolean = False
    Public Invisible As Boolean = False 'For Gms
    Public Invincible As Boolean = False

    Public InGuild As Boolean = False
    Public GuildID As Long = -1

    Public InExchange As Boolean = False
    Public InExchangeWith As Integer = -1
    Public ExchangeID As Integer = -1
    Public PickUpId As UInteger = 0

    Public Attacking As Boolean = False
    Public AttackedMonsterID As UInt32 = 0
    Public AttackSkill As UInt32 = 0
    Public AttackType As AttackType_
    Public LastSelected As UInt32 = 0

    Public TeleportType As TeleportType_


    

    

End Class

Public Structure Position
    Public XSector As Byte
    Public YSector As Byte
    Public X As Single
    Public Z As Single
    Public Y As Single
End Structure

Public Enum UseItemTypes
    None = 0
    Pot = 1
    Return_Scroll = 2
    Reverse_Scroll_Dead = 3
    Reverse_Scroll_Recall = 4
    Reverse_Scroll_Point = 5
End Enum

Public Structure cHotKey
    Public OwnerID As UInteger
    Public Type As UInteger
    Public Slot As UInteger
    Public IconID As UInteger
End Structure

Public Enum TeleportType_
    Npc = 0
    GM = 1
End Enum


Public Enum AttackType_
    Normal = 0
    Skill = 1
End Enum

Public Enum MoveType_
    Walk = 0
    Run = 1
    Berserk = 2
End Enum
