Namespace GameServer.Functions
    Module ItemShop
        Public Sub OnBuyItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            If PlayerData(index_).InExchange Or PlayerData(index_).InStall Then
                Exit Sub
            End If


            Dim shopline As Byte = packet.Byte
            Dim itemline As Byte = packet.Byte
            Dim amout As UShort = packet.Word
            Dim UniqueID As UInteger = packet.DWord

            Dim RefObj As New Object_
            Dim Price As UInt32 = 0
            Dim PackageName As String = ""

            For i = 0 To NpcList.Count - 1
                If NpcList(i).UniqueID = UniqueID Then
                    RefObj = GetObjectById(NpcList(i).Pk2ID)
                End If
            Next


            PackageName = RefObj.Shop.Tab(shopline).Items(itemline).PackageName
            Dim Package As MallPackage_ = GetItemMallItemByName(PackageName)
            Dim BuyItem As cItem = GetItemByName(Package.Name_Normal)

            Select Case BuyItem.CLASS_A
                Case 1
                    Price = BuyItem.SHOP_PRICE * amout
                Case 2
                    Price = BuyItem.SHOP_PRICE * amout
                Case 3
                    Price = BuyItem.SHOP_PRICE * amout
            End Select

            If CSng(PlayerData(index_).Gold) - Price < 0 Then
                'No Gold

            Else
                Dim Slot As Byte = GetFreeItemSlot(index_)

                If Slot <> -1 Then
                    Dim temp_item As cInvItem = Inventorys(index_).UserItems(Slot)
                    temp_item.Pk2Id = BuyItem.ITEM_TYPE
                    temp_item.OwnerCharID = PlayerData(index_).CharacterId

                    If BuyItem.CLASS_A = 1 Then
                        'Equip
                        temp_item.Plus = 0
                        temp_item.Durability = BuyItem.MAX_DURA
                        temp_item.Blues = New List(Of cInvItem.sBlue)

                    ElseIf BuyItem.CLASS_A = 2 Then
                        'Pet
                    ElseIf BuyItem.CLASS_A = 3 Then
                        'Etc
                        temp_item.Amount = amout
                    End If

                    Inventorys(index_).UserItems(Slot) = temp_item
                    UpdateItem(Inventorys(index_).UserItems(Slot)) 'SAVE IT


                    PlayerData(index_).Gold -= Price
                    UpdateGold(index_)


                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.ItemMove)
                    writer.Byte(1)
                    writer.Byte(8)
                    writer.Byte(shopline)
                    writer.Byte(itemline)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(amout)
                    Server.Send(writer.GetBytes, index_)
                End If
            End If
        End Sub


        Public Sub OnSellItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            If PlayerData(index_).InExchange Or PlayerData(index_).InStall Then
                Exit Sub
            End If


            Dim slot As Byte = packet.Byte
            Dim amout As UShort = packet.Word
            Dim UniqueID As UInt32 = packet.DWord
            Dim Gold As ULong = 0

            If Inventorys(index_).UserItems(slot).Pk2Id <> 0 Then
                Dim Item As cItem = GetItemByID(Inventorys(index_).UserItems(slot).Pk2Id)

                Select Case Item.CLASS_A
                    Case 1
                        Gold = Item.SELL_PRICE
                    Case 2
                        Gold = Item.SELL_PRICE
                    Case 3
                        Gold = Item.SELL_PRICE * Inventorys(index_).UserItems(slot).Amount
                End Select

                Inventorys(index_).UserItems(slot) = ClearItem(Inventorys(index_).UserItems(slot))
                UpdateItem(Inventorys(index_).UserItems(slot))

                PlayerData(index_).Gold += Gold
                UpdateGold(index_)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.ItemMove)
                writer.Byte(1)
                writer.Byte(9)
                writer.Byte(slot)
                writer.Word(amout)
                writer.DWord(UniqueID)
                writer.Byte(1)
                Server.Send(writer.GetBytes, index_)
            End If
        End Sub

        Public Sub BuyBack(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim UnqiueID As UInt32 = packet.DWord
            Dim BuyBackSlot As Byte = packet.Byte



        End Sub


        Class ShopData_
            Public Pk2ID As UInteger
            Public StoreName As String
            Public Tab(20) As ShopTab_

            Class ShopTab_
                Public TabName As String
                Public Items(60) As ShopItem_
            End Class

            Class ShopItem_
                Public ItemLine As Byte
                Public PackageName As String
            End Class

            Sub Init()
                For i = 0 To Tab.Count - 1
                    Tab(i) = New ShopTab_
                Next
            End Sub
        End Class
    End Module
End Namespace
