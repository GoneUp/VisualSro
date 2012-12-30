Imports SRFramework

Namespace Functions
    Module ItemMall
        Public Sub OnSendSilksGMCLoader(ByVal Index_ As Integer)
            Dim userLoader As New GameDB.GameUserLoader(Index_)
            AddHandler userLoader.GetCallback, AddressOf OnSendSilks
            userLoader.LoadFromGlobal(PlayerData(Index_).AccountID)
        End Sub

        Public Sub OnSendSilks(user As cUser, packet As PacketReader, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_SILK)
            writer.DWord(user.Silk)
            writer.DWord(user.Silk_Bonus)
            writer.DWord(user.Silk_Points)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnBuyItemFromMallGMCLoader(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim userLoader As New GameDB.GameUserLoader(packet, Index_)
            AddHandler userLoader.GetCallback, AddressOf OnBuyItemFromMall
           userLoader.LoadFromGlobal(PlayerData(Index_).AccountID)
        End Sub

        Public Sub OnBuyItemFromMall(user As cUser, ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim type1 As Byte = packet.Byte
            Dim type2 As Byte = packet.Byte
            Dim type3 As Byte = packet.Byte
            Dim type4 As Byte = packet.Byte
            Dim type5 As Byte = packet.Byte
            Dim longName As String = packet.String(packet.Word)
            Dim mallPackage As PackageItem = GetPackageItem(longName)
            Dim amout As UShort = packet.Word
            Dim writer As New PacketWriter

            
            If longName = mallPackage.PackageName Then
                Dim paymentEntry As MallPaymentEntry = mallPackage.Payments(MallPaymentEntry.PaymentDevices.Mall)

                If (paymentEntry IsNot Nothing) AndAlso (user.Silk - (paymentEntry.Price * amout) >= 0 And GetFreeItemSlot(Index_) <> -1) Then
                    Dim itemSlots As New List(Of Byte)

                    For i = 1 To amout
                        Dim refitem As cRefItem = GetItemByName(mallPackage.CodeName)
                        Dim slot As Byte = GetFreeItemSlot(Index_)
                        Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(slot)
                        Dim item As New cItem

                        item.ObjectID = refitem.Pk2Id
                        item.CreatorName = PlayerData(Index_).CharacterName & "#MALL"

                        Select Case refitem.CLASS_A
                            Case 1
                                item.Data = refitem.MAX_DURA
                                item.Variance = mallPackage.Variance
                            Case 2
                                item.Data = 1
                            Case 3
                                item.Data = mallPackage.Data
                        End Select

                        Dim ID As UInt64 = ItemManager.AddItem(item)
                        invItem.ItemID = ID
                        ItemManager.UpdateInvItem(invItem, InvItemTypes.Inventory)

                        itemSlots.Add(slot)

                        user.Silk -= paymentEntry.Price * amout

                        Dim userLoader As New GameDB.GameUserLoader(Index_)
                        userLoader.UpdateGlobal(user)

                        OnSendSilksGMCLoader(Index_)
                    Next

                    writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                    writer.Byte(1)
                    writer.Byte(24)
                    writer.Byte(type1)
                    writer.Byte(type2)
                    writer.Byte(type3)
                    writer.Byte(type4)
                    writer.Byte(type5)
                    writer.Byte(itemSlots.Count)
                    For Each slot In itemSlots
                        writer.Byte(slot)
                    Next
                    writer.Word(amout)
                    Server.Send(writer.GetBytes, Index_)

                    Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "Item_Mall", "Buy",
                                     String.Format("Item: {0}, Amout {1}, Payed: {2}", longName, amout, paymentEntry.Price))
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
