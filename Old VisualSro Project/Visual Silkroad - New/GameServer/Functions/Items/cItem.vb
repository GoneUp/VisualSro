Namespace GameServer.Functions
    Public Class cInventory

        Sub New(ByVal slots As Integer)
            ReDim UserItems(slots - 1)
            For i = 0 To UserItems.Length - 1
                UserItems(i) = New cInvItem
            Next

            For i = 0 To AvatarItems.Count - 1
                AvatarItems(i) = New cInvItem
            Next
        End Sub
        Sub CalculateItemCount()
            ItemCount = 0
            For i = 0 To UserItems.Length - 1
                If UserItems(i).Pk2Id <> 0 Then
                    ItemCount += 1
                End If
            Next
        End Sub

        Sub CalculateAvatarCount()
            AvatarCount = 0
            For i = 0 To AvatarItems.Length - 1
                If AvatarItems(i).Pk2Id <> 0 Then
                    AvatarCount += 1
                End If
            Next
        End Sub

        Public UserItems(255) As cInvItem
        Public ItemCount As Byte

        Public AvatarItems(5) As cInvItem
        Public AvatarCount As Byte

    End Class

    Public Class cItem
        Public Pk2Id As UInteger
        Public ITEM_TYPE_NAME As String
        Public ITEM_MALL As Byte
        Public CLASS_A As Byte
        Public CLASS_B As Byte
        Public CLASS_C As Byte
        Public RACE As Byte
        Public SHOP_PRICE As ULong
        Public MIN_REPAIR As UShort
        Public MAX_REPAIR As UShort
        Public STORE_PRICE As ULong
        Public SELL_PRICE As ULong
        Public LV_REQ As Byte
        Public REQ1 As Integer
        Public REQ1_LV As Byte
        Public REQ2 As Integer
        Public REQ2_LV As Byte
        Public REQ3 As Integer
        Public REQ3_LV As Byte
        Public MAX_POSSES As Integer
        Public MAX_STACK As UShort
        Public GENDER As Byte
        Public MIN_DURA As Single
        Public MAX_DURA As Single
        Public MIN_PHYSDEF As Double
        Public MAX_PHYSDEF As Double
        Public PHYSDEF_INC As Double
        Public MIN_PARRY As Single
        Public MAX_PARRY As Single

        Public MIN_PHYS_ABSORB As Double
        Public MAX_PHYS_ABSORB As Double
        Public PHYS_ABSORB_INC As Double

        Public MIN_MAG_ABSORB As Double
        Public MAX_MAG_ABSORB As Double
        Public MAG_ABSORB_INC As Double

        Public MIN_BLOCK As Single
        Public MAX_BLOCK As Single
        Public MIN_MAGDEF As Double
        Public MAX_MAGDEF As Double
        Public MAGDEF_INC As Double
        Public MIN_PHYS_REINFORCE As Single
        Public MAX_PHYS_REINFORCE As Single
        Public MIN_MAG_REINFORCE As Single
        Public MAX_MAG_REINFORCE As Single
        Public ATTACK_DISTANCE As Single

        Public MIN_FROM_PHYATK As Double
        Public MAX_FROM_PHYATK As Double
        Public MIN_TO_PHYATK As Double
        Public MAX_TO_PHYATK As Double
        Public PHYATK_INC As Double

        Public MIN_FROM_MAGATK As Double
        Public MAX_FROM_MAGATK As Double
        Public MIN_TO_MAGATK As Double
        Public MAX_TO_MAGATK As Double
        Public MAGATK_INC As Double

        Public MIN_FROM_PHYS_REINFORCE As Single
        Public MAX_FROM_PHYS_REINFORCE As Single
        Public MIN_TO_PHYS_REINFORCE As Single
        Public MAX_TO_PHYS_REINFORCE As Single
        Public MIN_FROM_MAG_REINFORCE As Single
        Public MAX_FROM_MAG_REINFORCE As Single
        Public MIN_TO_MAG_REINFORCE As Single
        Public MAX_TO_MAG_REINFORCE As Single

        Public MIN_ATTACK_RATING As Single
        Public MAX_ATTACK_RATING As Single
        Public MIN_CRITICAL As Single
        Public MAX_CRITICAL As Single
        Public USE_TIME_HP As Integer  ' steht drin wieviel HP ein potion heilt          
        Public USE_TIME_HP_PER As Integer ' steht drin wieviel prozent HP ein grain heilt   
        Public USE_TIME_MP As Integer ' steht drin wieviel MP ein potion heilt        
        Public USE_TIME_MP_PER As Integer ' steht drin wieviel prozent MP ein grain heilt   
    End Class

    Public Class cInvItem
        ' Fields
        Public OwnerCharID As Integer = 0
        Public Slot As Integer = 0
        Public Amount As Long = 0
        Public Durability As Integer = 30
        Public Blues As New List(Of sBlue)
        Public Pk2Id As Long = 0
        Public Plus As Byte = 0
        Public DatabaseID As UInteger = 0
        Public ItemType As sUserItemType

        Public PerDurability As Byte
        Public PerPhyRef As Byte
        Public PerMagRef As Byte
        Public PerPhyAtk As Byte
        Public PerMagAtk As Byte
        Public PerPhyDef As Byte
        Public PerMagDef As Byte
        Public PerBlock As Byte
        Public PerCritical As Byte
        Public PerAttackRate As Byte
        Public PerParryRate As Byte
        Public PerPhyAbs As Byte
        Public PerMagAbs As Byte

        Public Function GetWhiteStats() As ULong
            Dim ws As ULong = 0

            If Pk2Id <> 0 Then
                Dim item As cItem = GameServer.GetItemByID(Pk2Id)
                If item.CLASS_A = 1 Then
                    Select Case item.CLASS_B
                        Case 1, 2, 3, 9, 10, 11 'Equipment

                            ws += Math.Round(31 * PerDurability / 100)
                            ws += Math.Round(31 * PerPhyRef / 100) * 32
                            ws += Math.Round(31 * PerMagRef / 100) * 1024
                            ws += Math.Round(31 * PerPhyDef / 100) * 32768
                            ws += Math.Round(31 * PerMagDef / 100) * 1048576
                            ws += Math.Round(31 * PerParryRate / 100) * 33554432

                        Case 4 'Shield

                            ws += Math.Round(31 * PerDurability / 100)
                            ws += Math.Round(31 * PerPhyRef / 100) * 32
                            ws += Math.Round(31 * PerMagRef / 100) * 1024
                            ws += Math.Round(31 * PerBlock / 100) * 32768
                            ws += Math.Round(31 * PerPhyDef / 100) * 1048576
                            ws += Math.Round(31 * PerMagDef / 100) * 33554432

                        Case 6 'Weapon

                            ws += Math.Round(31 * PerDurability / 100)
                            ws += Math.Round(31 * PerPhyRef / 100) * 32
                            ws += Math.Round(31 * PerMagRef / 100) * 1024
                            ws += Math.Round(31 * PerAttackRate / 100) * 32768
                            ws += Math.Round(31 * PerPhyAtk / 100) * 1048576
                            ws += Math.Round(31 * PerMagAtk / 100) * 33554432
                            ws += Math.Round(31 * PerCritical / 100) * 1073741824

                        Case 5, 12 'Accessory

                            ws += Math.Round(31 * PerPhyAbs / 100)
                            ws += Math.Round(31 * PerMagAbs / 100) * 32

                    End Select
                End If
            End If
            Return ws
        End Function

        Public Locked As Boolean

        ' Nested Types
        Public Structure sBlue
            Public Typ As UInt32
            Public Amount As UInt32
        End Structure

        Enum sUserItemType
            Inventory = 0
            Avatar = 1
            Storage_User = 2
            Storage_Guild = 3
        End Enum
    End Class


    Class cItemDrop
        Public UniqueID As UInteger
        Public DroppedBy As UInteger
        Public DespawnTime As DateTime
        Public Position As Position
        Public Item As cInvItem
    End Class
End Namespace
