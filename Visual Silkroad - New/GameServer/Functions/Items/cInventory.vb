Imports GameServer.GameServer.DatabaseCore, GameServer.GameServer.Functions
Public Class cInventory

    Sub New(ByVal slots As Integer)
        ReDim UserItems(slots)
        For i = 0 To UserItems.Length - 1
            UserItems(i) = New cInvItem
        Next
    End Sub

    Sub ReOrderItems(ByVal index_ As Integer)
        Dim now As ULong = DateTime.Now.Ticks
        For A = 0 To AllItems.Length - 1
            For B = 0 To UserItems.Length - 1
                If AllItems(A).OwnerCharID = UserItems(B).OwnerCharID And AllItems(A).Slot = UserItems(B).Slot Then
                    AllItems(A) = UserItems(B)
                    Exit For
                End If
            Next
        Next
        Dim past As Long = DateTime.Now.Ticks - now
        Debug.Print("[Item Reorder][Time: " & past & "ms]")

        ReDim UserItems(PlayerData(index_).MaxSlots)
        For i = 0 To UserItems.Length - 1
            UserItems(i) = New cInvItem
        Next


        For I = 0 To (AllItems.Length - 1)
            If AllItems(I).OwnerCharID = PlayerData(index_).UniqueId Then
                Me.UserItems(AllItems(I).Slot) = AllItems(I)
            End If
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

    Public UserItems(255) As cInvItem
    Public ItemCount As Byte

End Class
