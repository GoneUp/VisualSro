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

        Public Sub OnBuyItemFromMall(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim type1 As Byte = packet.Byte
            Dim type2 As Byte = packet.Byte
            Dim type3 As Byte = packet.Byte
            Dim type4 As Byte = packet.Byte
            Dim type5 As Byte = packet.Byte
            Dim LongName As String = packet.String(packet.Word)
            Dim RefObject As MallPackage_ = GetItemMallItem(LongName)
            Dim amout As UShort = packet.Word
            Dim writer As New PacketWriter

            Dim UserIndex As Integer = GameDB.GetUserIndex(PlayerData(index_).AccountID)

            If LongName = RefObject.Name_Package Then
                If GameDB.Users(UserIndex).Silk - (RefObject.Price * amout) >= 0 And GetFreeItemSlot(index_) <> -1 Then
                    Dim ItemSlots As New List(Of Byte)

                    For i = 1 To amout
                        Dim _Refitem As cRefItem = GetItemByName(RefObject.Name_Normal)
                        Dim slot As Byte = GetFreeItemSlot(index_)
                        Dim item As cInvItem = Inventorys(index_).UserItems(slot)
                        item.Pk2Id = _Refitem.Pk2Id

                        Select Case _Refitem.CLASS_A
                            Case 1
                                item.Durability = 30
                                item.Amount = 0
                            Case 2
                                item.Durability = 30
                                item.Amount = 10
                            Case 3
                                item.Durability = 30
                                item.Amount = RefObject.Amout
                        End Select
                        UpdateItem(item)
                        ItemSlots.Add(slot)

                        GameDB.Users(UserIndex).Silk -= RefObject.Price
                        GameDB.SaveSilk(index_, GameDB.Users(UserIndex).Silk, GameDB.Users(UserIndex).Silk_Bonus, GameDB.Users(UserIndex).Silk_Points)
                        OnSendSilks(index_)
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
                    Server.Send(writer.GetBytes, index_)

                    Log.WriteGameLog(index_, "Item_Mall", "Buy",
                                     String.Format("Item: {0}, Amout {1}, Payed: {2}", LongName, amout, RefObject.Price))
                Else
                    writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                    writer.Byte(2)
                    writer.Byte(24)
                    writer.Byte(1)
                    Server.Send(writer.GetBytes, index_)
                End If
            End If
        End Sub
    End Module
End Namespace
