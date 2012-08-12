Namespace Functions
    Public Class cInventory
        Public UserItems(255) As cInventoryItem
        Public ItemCount As Byte

        Public AvatarItems(5) As cInventoryItem
        Public AvatarCount As Byte

        Sub New(ByVal charID As UInt32, ByVal char_slots As Integer)
            ReDim UserItems(char_slots - 1)
            For i = 0 To UserItems.Length - 1
                UserItems(i) = New cInventoryItem
            Next

            For i = 0 To AvatarItems.Count - 1
                AvatarItems(i) = New cInventoryItem
            Next

            For i = 0 To GameDB.InventoryItems.Count - 1
                If GameDB.InventoryItems(i) IsNot Nothing Then
                    If GameDB.InventoryItems(i).OwnerID = charID Then
                        UserItems(i) = GameDB.InventoryItems(i)
                    End If
                End If
            Next

            For i = 0 To GameDB.AvatarInventoryItems.Count - 1
                If GameDB.AvatarInventoryItems(i) IsNot Nothing Then
                    If GameDB.AvatarInventoryItems(i).OwnerID = charID Then
                        UserItems(i) = GameDB.AvatarInventoryItems(i)
                    End If
                End If
            Next
        End Sub

        Sub CalculateItemCount()
            ItemCount = 0
            For i = 0 To UserItems.Length - 1
                If UserItems(i).ItemID <> 0 Then
                    ItemCount += 1
                End If
            Next
        End Sub

        Sub CalculateAvatarCount()
            AvatarCount = 0
            For i = 0 To AvatarItems.Length - 1
                If AvatarItems(i).ItemID <> 0 Then
                    AvatarCount += 1
                End If
            Next
        End Sub
    End Class

    Public Class cInvItem
        '18.446.744.073.709.551.615
        ' Fields
        Public OwnerCharID As Integer = 0
        Public Slot As Integer = 0
        Public Amount As Long = 0
        Public Durability As Integer = 30
        Public Blues As New List(Of sBlue)
        Public Pk2Id As Long = 0
        Public Plus As Byte = 0
        Public DatabaseID As UInteger = 0
        Public Locked As Boolean
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
                Dim item As cRefItem = GetItemByID(Pk2Id)
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
End Namespace
