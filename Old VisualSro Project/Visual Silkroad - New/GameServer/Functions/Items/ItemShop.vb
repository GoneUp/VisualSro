Imports SRFramework

Namespace Functions
    Module ItemShop
        Public Sub OnBuyItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If PlayerData(Index_).InExchange Or PlayerData(Index_).InStall Then
                Exit Sub
            End If


            Dim shopline As Byte = packet.Byte
            Dim itemline As Byte = packet.Byte
            Dim amout As UShort = packet.Word
            Dim UniqueID As UInteger = packet.DWord

            Dim RefObj As New SilkroadObject
            Dim Price As UInt32 = 0
            Dim PackageName As String = ""


            If NpcList.ContainsKey(UniqueID) Then
                RefObj = GetObject(NpcList(UniqueID).Pk2ID)
            Else
                Server.Disconnect(Index_)
            End If

            PackageName = RefObj.Shop.Tab(shopline).Items(itemline).PackageName
            Dim Package As MallPackage_ = GetItemMallItem(PackageName)
            Dim BuyItem As cRefItem = GetItemByName(Package.Name_Normal)

            Select Case BuyItem.CLASS_A
                Case 1
                    Price = BuyItem.SHOP_PRICE * amout
                Case 2
                    Price = BuyItem.SHOP_PRICE * amout
                Case 3
                    Price = BuyItem.SHOP_PRICE * amout
            End Select

            If CSng(PlayerData(Index_).Gold) - Price < 0 Then
                'No Gold

            Else
                Dim ItemSlots As New List(Of Byte)

                If BuyItem.CLASS_A = 1 Then
                    For i = 1 To amout
                        Dim Slot As Byte = GetFreeItemSlot(Index_)

                        If Slot <> -1 Then
                            Dim temp_item As New cItem
                            temp_item.ObjectID = BuyItem.Pk2Id
                            temp_item.CreatorName = PlayerData(Index_).CharacterName & "#SHOP"


                            'Equip
                            temp_item.Plus = 0
                            temp_item.Data = BuyItem.MAX_DURA


                            Dim invItem As New cInventoryItem
                            invItem.OwnerID = PlayerData(Index_).CharacterId
                            invItem.Slot = Slot
                            invItem.ItemID = ItemManager.AddItem(temp_item)

                            'SAVE IT
                            ItemSlots.Add(Slot)
                        End If
                    Next

                Else
                    Dim Slot As Byte = GetFreeItemSlot(Index_)

                    If Slot <> -1 Then
                        Dim temp_item As New cItem
                        temp_item.ObjectID = BuyItem.Pk2Id
                        temp_item.CreatorName = PlayerData(Index_).CharacterName & "#SHOP"

                        'Equip
                        temp_item.Data = amout

                        Dim invItem As New cInventoryItem
                        invItem.OwnerID = PlayerData(Index_).CharacterId
                        invItem.Slot = Slot
                        invItem.ItemID = ItemManager.AddItem(temp_item)

                        'SAVE IT
                        ItemSlots.Add(Slot)
                    End If
                End If

                PlayerData(Index_).Gold -= Price
                UpdateGold(Index_)


                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                writer.Byte(1)
                writer.Byte(8)
                writer.Byte(shopline)
                writer.Byte(itemline)
                writer.Byte(ItemSlots.Count)
                For Each slot In ItemSlots
                    writer.Byte(slot)
                Next
                writer.Word(amout)
                For i = 1 To amout
                    writer.DWord(0)
                Next
                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub


        Public Sub OnSellItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If PlayerData(Index_).InExchange Or PlayerData(Index_).InStall Then
                Exit Sub
            End If

            Dim slot As Byte = packet.Byte
            Dim amout As UShort = packet.Word
            Dim UniqueID As UInt32 = packet.DWord
            Dim Gold As ULong = 0

            If Inventorys(Index_).UserItems(slot).ItemID = 0 Then
                Server.Disconnect(Index_)
            End If

            Dim InvItem As cInventoryItem = Inventorys(Index_).UserItems(slot)
            Dim Item As cItem = GameDB.Items(InvItem.ItemID)
            Dim RefItem As cRefItem = GetItemByID(Item.ObjectID)


            Select Case RefItem.CLASS_A
                Case 1
                    Gold = RefItem.SELL_PRICE
                Case 2
                    Gold = RefItem.SELL_PRICE
                Case 3
                    Gold = RefItem.SELL_PRICE * Item.Data
            End Select

            PlayerData(Index_).BuybackList.Insert(0, Item)

            UpdateAmout(Index_, InvItem.Slot, amout * -1)

            PlayerData(Index_).Gold += Gold
            UpdateGold(Index_)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1)
            writer.Byte(9)
            writer.Byte(slot)
            writer.Word(amout)
            writer.DWord(UniqueID)
            writer.Byte(1) 'buyback slot
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub BuyBack(ByVal packet As PacketReader, ByVal Index_ As Integer)
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
