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
            Dim uniqueID As UInteger = packet.DWord

            Dim refObj As New SilkroadObject
            Dim price As UInt32 = 0
            Dim packageName As String = ""


            If NpcList.ContainsKey(uniqueID) Then
                refObj = GetObject(NpcList(uniqueID).Pk2ID)
            Else
                Server.Disconnect(Index_)
                Exit Sub
            End If

            packageName = refObj.ItemShop.Tabs(shopline).Items(itemline)
            Dim package As PackageItem = GetPackageItem(packageName)
            Dim buyItem As cRefItem = GetItemByName(package.CodeName)

            Select Case buyItem.CLASS_A
                Case 1
                    price = buyItem.SHOP_PRICE * amout
                Case 2
                    price = buyItem.SHOP_PRICE * amout
                Case 3
                    price = buyItem.SHOP_PRICE * amout
            End Select

            If CSng(PlayerData(Index_).Gold) - price < 0 Then
                'No Gold

            Else
                Dim itemSlots As New List(Of Byte)

                If buyItem.CLASS_A = 1 Then
                    For i = 1 To amout
                        Dim slot As Byte = GetFreeItemSlot(Index_)

                        If slot <> -1 Then
                            Dim tempItem As New cItem
                            tempItem.ObjectID = buyItem.Pk2Id
                            tempItem.CreatorName = PlayerData(Index_).CharacterName & "#SHOP"


                            'Equip
                            tempItem.Plus = 0
                            tempItem.Data = package.Data


                            Dim ID As UInt64 = ItemManager.AddItem(tempItem)
                            Inventorys(Index_).UserItems(slot).ItemID = ID
                            ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(slot), InvItemTypes.Inventory)

                            'SAVE IT
                            itemSlots.Add(slot)
                        End If
                    Next

                Else
                    Dim slot As Byte = GetFreeItemSlot(Index_)

                    If slot <> -1 Then
                        Dim tempItem As New cItem
                        tempItem.ObjectID = buyItem.Pk2Id
                        tempItem.CreatorName = PlayerData(Index_).CharacterName & "#SHOP"

                        'Equip
                        tempItem.Data = amout

                        Dim ID As UInt64 = ItemManager.AddItem(tempItem)
                        Inventorys(Index_).UserItems(slot).ItemID = ID
                        ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(slot), InvItemTypes.Inventory)

                        'SAVE IT
                        itemSlots.Add(slot)
                    End If
                End If

                PlayerData(Index_).Gold -= price
                UpdateGold(Index_)


                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
                writer.Byte(1)
                writer.Byte(8)
                writer.Byte(shopline)
                writer.Byte(itemline)
                writer.Byte(itemSlots.Count)
                For Each slot In itemSlots
                    writer.Byte(slot)
                Next
                writer.Word(amout)
                If buyItem.CLASS_A = 1 Then
                    For i = 1 To amout
                        writer.DWord(0)
                    Next
                Else
                    writer.DWord(0)
                End If

                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub


        Public Sub OnSellItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If PlayerData(Index_).InExchange Or PlayerData(Index_).InStall Then
                Exit Sub
            End If

            Dim slot As Byte = packet.Byte
            Dim amout As UShort = packet.Word
            Dim uniqueID As UInt32 = packet.DWord
            Dim gold As ULong = 0

            If Inventorys(Index_).UserItems(slot).ItemID = 0 Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(slot)
            Dim item As cItem = GameDB.Items(invItem.ItemID)
            Dim refItem As cRefItem = GetItemByID(item.ObjectID)


            Select Case refItem.CLASS_A
                Case 1
                    gold = refItem.SELL_PRICE
                Case 2
                    gold = refItem.SELL_PRICE
                Case 3
                    gold = refItem.SELL_PRICE * item.Data
            End Select

            PlayerData(Index_).BuybackList.Insert(0, item)

            If refItem.CLASS_A = 1 Then
                invItem.ItemID = 0
                ItemManager.UpdateInvItem(invItem, InvItemTypes.Inventory)
                ItemManager.RemoveItem(item.ID)
            Else
                UpdateItemAmout(Index_, invItem.Slot, amout * -1)
            End If

            PlayerData(Index_).Gold += gold
            UpdateGold(Index_)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1)
            writer.Byte(9)
            writer.Byte(slot)
            writer.Word(amout)
            writer.DWord(uniqueID)
            writer.Byte(1) 'buyback slot
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub BuyBack(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim UnqiueID As UInt32 = packet.DWord
            Dim BuyBackSlot As Byte = packet.Byte
        End Sub


        Public Class ShopGroup
            Public Group_Name As String = ""
            Public Object_Code_Name As String = ""
            Public Store_Name As String = ""
        End Class

        Public Class Shop
            Public Name As String = ""
            Public Object_Code_Name As String = ""
            Public TabGroups As New List(Of ShopTabGroup)
            Public Tabs As New Dictionary(Of Byte, ShopTab) 'after parsing the whole stuff we insert our tabs here
        End Class

        Public Class ShopTabGroup
            Public Group_Name As String = ""
            Public ShopTabs As New Dictionary(Of Byte, ShopTab)
        End Class

        Public Class ShopTab
            Public Tab_Name As String = ""
            Public Items As New Dictionary(Of Byte, String)
        End Class
    End Module
End Namespace
