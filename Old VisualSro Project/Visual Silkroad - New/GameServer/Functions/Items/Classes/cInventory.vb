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
End Namespace
