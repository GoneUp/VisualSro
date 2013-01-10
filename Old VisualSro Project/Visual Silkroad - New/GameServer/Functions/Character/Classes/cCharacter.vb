Imports SRFramework

Namespace Functions
    Friend Class cCharacter
        Inherits cGameObject

        Public AccountID As UInteger = 0
        Public CharacterName As String = ""
        Public CharacterId As UInteger = 0
        Public HP As UInteger = 0
        Public MP As UInteger = 0
        Public CHP As UInteger = 0
        Public CMP As UInteger = 0
        Public Volume As Byte = 0
        Public Level As Byte = 0
        Public Experience As ULong = 0
        Public Gold As ULong = 0
        Public SkillPoints As UInteger = 0
        Public SkillPointBar As ULong = 0
        Public Attributes As UShort = 0
        Public BerserkBar As Byte = 0
        Public Berserk As Boolean = False

        Public MinPhy As UInteger = 0
        Public MaxPhy As UInteger = 0

        Public MinMag As UInteger = 0
        Public MaxMag As UInteger = 0

        Public PhyDef As UShort = 0
        Public MagDef As UShort = 0
        Public PhyAbs As UShort = 0
        Public MagAbs As UShort = 0
        Public AccessoryAbs As UShort = 0

        Public Hit As UShort = 0
        Public Parry As UShort = 0

        Public Strength As UShort = 0
        Public Intelligence As UShort = 0

        'Job
        Public AliasName As String = ""
        Public JobType As Byte = 0
        Public JobLevel As Byte = 1
        Public JobExperience As ULong = 0

        'Flags
        Public GM As Boolean = False
        Public PVP As Byte = 0
        Public MaxInvSlots As Byte = 0
        Public MaxAvatarSlots As Byte = 5
        Public Angle As UInt16 = 0
        Public Deleted As Boolean = False
        Public DeletionTime As DateTime
        Public HelperIcon As Byte = 0
        Public ActionFlag As Byte = 0        '0x00 = standing, 0x02 = walking, 0x03 = running, 0x04 = sitting
        Public Busy As Boolean = False
        Public Alive As Boolean = True
        Public Skilling As Boolean = False
        Public GroupSpawnPacketsToSend As New Queue(Of PacketWriter)
        Public UsedItem As UseItemTypes
        Public UsedItemParameter As Integer = 0

        Public PositionRecall As New Position
        Public PositionReturn As New Position
        Public PositionDead As New Position

        Public SpawnedPlayers As New List(Of UInt32)
        Public SpawnedMonsters As New List(Of UInt32)
        Public SpawnedItems As New List(Of UInt32)
        Public SpawnedNPCs As New List(Of UInt32)

        Public Ingame As Boolean = False
        Public Invisible As Boolean = False  'For Gms
        Public Invincible As Boolean = False

        Public InGuild As Boolean = False
        Public GuildID As UInteger = 0

        Public InExchange As Boolean = False
        Public InExchangeWith As UInteger = 0
        Public ExchangeID As UInteger = 0

        Public InStall As Boolean = False
        Public StallID As UInteger = 0
        Public StallOwner As Boolean = False

        Public InParty As Boolean = False
        Public PartyID As UInteger = 0

        Public PickUpId As UInteger = 0
        Public Attacking As Boolean = False
        Public AttackedId As UInt32 = 0
        Public UsingSkillId As UInt32 = 0
        Public AttackType As Functions.AttackTypes
        Public SkillOverId As UInt32 = 0
        Public CastingId As UInt32 = 0
        Public LastSelected As UInt32 = 0

        Public PotHp As UInt16 = 0
        Public PotMp As UInt16 = 0
        Public PotAbormal As UInt16 = 0
        Public PotDelay As UInt16 = 0

        Public Buffs As New Dictionary(Of UInteger, cBuff) 'Key = SkillOverId

        Public TeleportType As TeleportTypes
        Public ExitType As ExitTypes
        Public BuybackList As New List(Of cItem)
        Public TmpSilkAddValue As Int64

        Public ReadOnly Property Position() As Position
            Get
                Return PosTracker.GetCurPos
            End Get
        End Property

        Public WriteOnly Property SetPosition() As Position
            Set(ByVal value As Position)
                PosTracker.LastPos = value
            End Set
        End Property

        Public Property WalkSpeed As Single
            Get
                Return PosTracker.WalkSpeed
            End Get
            Set(ByVal value As Single)
                PosTracker.WalkSpeed = value
            End Set
        End Property

        Public Property RunSpeed As Single
            Get
                Return PosTracker.RunSpeed
            End Get
            Set(ByVal value As Single)
                PosTracker.RunSpeed = value
            End Set
        End Property

        Public Property BerserkSpeed As Single
            Get
                Return PosTracker.BerserkSpeed
            End Get
            Set(ByVal value As Single)
                PosTracker.BerserkSpeed = value
            End Set
        End Property
    End Class
End Namespace