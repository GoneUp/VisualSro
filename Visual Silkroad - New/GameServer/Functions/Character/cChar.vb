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
    Public SkillPointBar As UShort
    Public Attributes As UShort
    Public BerserkBar As Byte
    Public Berserk As Byte
    Public WalkSpeed As Single
    Public RunSpeed As Single
    Public BerserkSpeed As Single
    Public MinPhy As UShort
    Public MaxPhy As UShort
    Public MinMag As UShort
    Public MaxMag As UShort
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

    Public Position As New Position
    Public Position_Recall As New Position
    Public Position_Return As New Position
    Public Position_Dead As New Position

    Public SpawnedPlayers As New List(Of Integer)
    Public SpawnedMonsters As New List(Of Integer)
    Public SpawnedItems As New List(Of Integer)
    Public SpawnedNPCs As New List(Of Integer)

    Public Ingame As Boolean = False
    Public InExchange As Boolean = False
    Public InExchangeWith As Integer = -1
    Public ExchangeID As Integer = -1


    Sub SetCharGroundStats()

        ' Player.Stats[Index_].HP = (uint)((double)Math.Pow(1.02, (Player.Stats[Index_].Level - 1)) * Player.Stats[Index_].Strength * 10); 
        HP = (Math.Pow(1.02, Me.Level - 1) * Me.Strength * 10)
        MP = (Math.Pow(1.02, Me.Level - 1) * Me.Intelligence * 10)

        Hit = Math.Round(Me.Level + 10)
        Parry = Math.Round(Me.Level + 10)
    End Sub

    Sub AddItemsToStats(ByVal Index_ As Integer)

        For i = 0 To 12




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
