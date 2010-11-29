Namespace GameServer.Functions
    Module ItemSpawn

        Public ItemList As New List(Of cInvItem)

        Public Function CreateItemSpawnPacket(ByVal Item_ As cItemDrop) As Byte()
            Dim refitem As cItem = GetItemByID(Item_.Item.Pk2Id)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleSpawn)
            writer.DWord(Item_.Item.Pk2Id)
            Select Case refitem.CLASS_A
                Case 1 'Equipment
                    writer.Byte(Item_.Item.Plus)
            End Select
            writer.DWord(Item_.Item.Pk2Id)
            writer.DWord(Item_.UniqueID)
            writer.Byte(Item_.Position.XSector)
            writer.Byte(Item_.Position.YSector)
            writer.Float(Item_.Position.X)
            writer.Float(Item_.Position.Z)
            writer.Float(Item_.Position.Y)

            writer.Word(&HAAA6)
            writer.Byte(0)
            writer.Byte(0)
            writer.Byte(6)
            writer.DWord(Item_.DroppedBy)

            Return writer.GetBytes
        End Function

        Public Sub DropItem(ByVal Item As cInvItem, ByVal Position As Position)
            Dim tmp_ As New cItemDrop
            tmp_.UniqueID = DatabaseCore.GetUnqiueID
            tmp_.DroppedBy = Item.OwnerCharID
            tmp_.Position = Position
            tmp_.Item = Item



        End Sub



    End Module

    Class cItemDrop
        Public UniqueID As UInteger
        Public DroppedBy As UInteger
        Public Position As Position
        Public Item As cInvItem
    End Class
End Namespace
