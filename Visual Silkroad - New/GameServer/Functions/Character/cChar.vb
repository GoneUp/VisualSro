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

    Public InStall As Boolean
    Public StallID As UInteger = 0
    Public StallOwner As Boolean

    Public PickUpId As UInteger = 0
    Public Attacking As Boolean = False
    Public AttackedId As UInt32 = 0
    Public UsingSkillId As UInt32 = 0
    Public AttackType As AttackType_
    Public SkillOverId As UInt32 = 0
    Public LastSelected As UInt32 = 0

    Public Pot_HP_Slot As Byte = 0
    Public Pot_HP_Value As Byte = 0
    Public Pot_MP_Slot As Byte = 0
    Public Pot_MP_Value As Byte = 0
    Public Pot_Abormal_Slot As Byte = 0
    Public Pot_Abormal_Value As Byte = 0
    Public Pot_Delay As Byte = 0

    Public Buffs As New List(Of cBuff)

    Public TeleportType As TeleportType_


    Sub SetCharGroundStats()
        HP = (Math.Pow(1.02, Me.Level - 1) * Me.Strength * 10)
        MP = (Math.Pow(1.02, Me.Level - 1) * Me.Intelligence * 10)

        Hit = Math.Round(Me.Level + 10)
        Parry = Math.Round(Me.Level + 10)

        '=================Really unsure of these Formulas=============
        Me.WalkSpeed = 15
        Me.RunSpeed = Me.Level + 49
        Me.BerserkSpeed = Me.RunSpeed * 2


        MinPhy = GameServer.Functions.GetMinPhy(Me.Strength)
        MaxPhy = GameServer.Functions.GetMaxPhy(Me.Strength, Level)
        MinMag = GameServer.Functions.GetMinMag(Me.Intelligence, Level)
        MaxMag = GameServer.Functions.GetMaxMag(Me.Intelligence, Level)
        PhyDef = GameServer.Functions.GetPhyDef(Me.Strength, Level)
        MagDef = GameServer.Functions.GetMagDef(Me.Intelligence)
    End Sub

    Sub AddItemsToStats(ByVal Index_ As Integer)
        For i = 0 To 12
            Dim _item As cInvItem = GameServer.Functions.Inventorys(Index_).UserItems(i)
            If _item.Pk2Id <> 0 Then
                Dim _refitem As cItem = GameServer.GetItemByID(_item.Pk2Id)

                If _refitem.CLASS_A = 1 Then
                    'Is a Equip
                    If _item.Slot = 6 Then 'Weapon
                        '=============Unsure
                        MinPhy += _refitem.MIN_HPHYATK + ((Me.Strength * _refitem.MIN_LPHYS_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)
                        MaxPhy += _refitem.MAX_HPHYATK + ((Me.Strength * _refitem.MAX_LPHYS_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)

                        MinMag += _refitem.MIN_LMAGATK + ((Me.Intelligence * _refitem.MIN_LMAG_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)
                        MaxMag += _refitem.MAX_LMAGATK + ((Me.Intelligence * _refitem.MAX_LMAG_REINFORCE) / 100) * (1 + GameServer.Functions.GetWeaponMasteryLevel(Index_) / 100)
                    Else
                        'Prevent Errors
                        If PhyDef + _refitem.MIN_PHYSDEF < UShort.MaxValue Then
                            PhyDef += _refitem.MIN_PHYSDEF
                        End If
                        If MagDef + _refitem.MAGDEF_MIN < UShort.MaxValue Then
                            MagDef += _refitem.MAGDEF_MIN
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Sub AddBuffsToStats(ByVal Index_ As Integer)

        For i = 0 To Buffs.Count - 1
            Select Case Buffs(i).Type
                Case BuffType_.ItemBuff
                    Dim Ref As cItem = GameServer.GetItemByID(Buffs(i).ItemID)


                Case BuffType_.SkillBuff
                    Dim Ref As GameServer.Skill_ = GameServer.GetSkillById(Buffs(i).SkillID)


            End Select
        Next
    End Sub
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

Public Structure cBuff
    Public OwnerID As UInteger
    Public DurationStart As DateTime
    Public DurationEnd As DateTime

    Public Type As BuffType_

    'If Skill
    Public SkillID As UInteger

    'If Item
    Public ItemID As UInteger



End Structure

Public Enum BuffType_
    ItemBuff = 0
    SkillBuff = 0
End Enum
