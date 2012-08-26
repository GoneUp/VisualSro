Imports SRFramework

Namespace Functions
    Module ItemMall
        Public Sub OnSendSilks(ByVal Index_ As Integer)
            Dim userIndex As Integer = GameDB.GetUserIndex(PlayerData(Index_).AccountID)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_SILK)
            writer.DWord(GameDB.Users(userIndex).Silk)
            writer.DWord(GameDB.Users(userIndex).Silk_Bonus)
            writer.DWord(GameDB.Users(userIndex).Silk_Points)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnBuyItemFromMall(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim type1 As Byte = packet.Byte
            Dim type2 As Byte = packet.Byte
            Dim type3 As Byte = packet.Byte
            Dim type4 As Byte = packet.Byte
            Dim type5 As Byte = packet.Byte
            Dim LongName As String = packet.String(packet.Word)
            Dim mallPackage As MallPackage_ = GetItemMallItem(LongName)
            Dim amout As UShort = packet.Word
            Dim writer As New PacketWriter

            Dim UserIndex As Integer = GameDB.GetUserIndex(PlayerData(Index_).AccountID)

            If LongName = mallPackage.Name_Package Then
                If GameDB.Users(UserIndex).Silk - (mallPackage.Price * amout) >= 0 And GetFreeItemSlot(Index_) <> -1 Then
                    Dim ItemSlots As New List(Of Byte)

                    For i = 1 To amout
                        Dim _Refitem As cRefItem = GetItemByName(mallPackage.Name_Normal)
                        Dim slot As Byte = GetFreeItemSlot(Index_)
                        Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(slot)
                        Dim item As New cItem

                        item.ObjectID = _Refitem.Pk2Id
                        item.CreatorName = PlayerData(Index_).CharacterName & "#MALL"

                        Select Case _Refitem.CLASS_A
                            Case 1
                                item.Data = _Refitem.MAX_DURA
                                item.Variance = mallPackage.Variance
                            Case 2
                                item.Data = 1
                            Case 3
                                item.Data = mallPackage.Amout
                        End Select

                        Dim ID As UInt64 = ItemManager.AddItem(item)
                        invItem.ItemID = ID
                        ItemManager.UpdateInvItem(invItem, cInventoryItem.Type.Inventory)

                        ItemSlots.Add(slot)

                        GameDB.Users(UserIndex).Silk -= mallPackage.Price * amout
                        GameDB.SaveSilk(Index_, GameDB.Users(UserIndex).Silk, GameDB.Users(UserIndex).Silk_Bonus, GameDB.Users(UserIndex).Silk_Points)
                        OnSendSilks(Index_)
                    Next

                    writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                    writer.Byte(1)
                    writer.Byte(24)
                    writer.Byte(type1)
                    writer.Byte(type2)
                    writer.Byte(type3)
                    writer.Byte(type4)
                    writer.Byte(type5)
                    writer.Byte(ItemSlots.Count)
                    For Each slot In ItemSlots
                        writer.Byte(slot)
                    Next
                    writer.Word(amout)
                    Server.Send(writer.GetBytes, Index_)

                    Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "Item_Mall", "Buy",
                                     String.Format("Item: {0}, Amout {1}, Payed: {2}", LongName, amout, mallPackage.Price))
                Else
                    writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                    writer.Byte(2)
                    writer.Byte(24)
                    writer.Byte(1)
                    Server.Send(writer.GetBytes, Index_)
                End If
            Else
                Server.Disconnect(Index_)
            End If
        End Sub
    End Module
End Namespace
