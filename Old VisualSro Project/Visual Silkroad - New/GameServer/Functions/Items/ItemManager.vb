Namespace Functions.ItemManager
    Module ItemManager
        Public Function AddItem(ByVal item As cItem) As UInt64
            Dim ID As UInt64 = Id_Gen.GetItemId
            item.ID = ID
            GameDB.Items.Add(ID, item)

            Database.SaveQuery(String.Format("INSERT INTO items(ID, ObjectID, Plus, Variance, Data, CreatorName) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", _
                                              item.ID, item.ObjectID, item.Plus, item.Variance, item.Data, item.CreatorName))
            UpdateBlues(item)

            Return ID
        End Function

        Public Sub AddInvItem(ByVal item As cInventoryItem, ByVal type As cInventoryItem.Type)
            If GameDB.Items.ContainsKey(item.ItemID) Or item.ItemID = 0 Then
                Select Case type
                    Case cInventoryItem.Type.Inventory
                        GameDB.InventoryItems.Add(item)

                        Database.SaveQuery(String.Format("INSERT INTO inventory(CharID, Slot, ItemID) VALUES ('{0}', '{1}', '{2}')", _
                                                         item.OwnerID, item.Slot, item.ItemID))

                    Case cInventoryItem.Type.AvatarInventory
                        GameDB.AvatarInventoryItems.Add(item)

                        Database.SaveQuery(String.Format("INSERT INTO inventory_avatar(CharID, Slot, ItemID) VALUES ('{0}', '{1}', '{2}')", _
                                                         item.OwnerID, item.Slot, item.ItemID))

                    Case cInventoryItem.Type.COSInventory
                        GameDB.COSInventoryItems.Add(item)

                        Database.SaveQuery(String.Format("INSERT INTO inventory_cos(COSID, Slot, ItemID) VALUES ('{0}', '{1}', '{2}')", _
                                                         item.OwnerID, item.Slot, item.ItemID))

                    Case cInventoryItem.Type.Storage
                        GameDB.StorageItems.Add(item)

                        Database.SaveQuery(String.Format("INSERT INTO storage(AccountID, Slot, ItemID) VALUES ('{0}', '{1}', '{2}')", _
                                                         item.OwnerID, item.Slot, item.ItemID))

                    Case cInventoryItem.Type.GuildStorage
                        GameDB.GuildStorageItems.Add(item)

                        Database.SaveQuery(String.Format("INSERT INTO storage_guild(GuildID, Slot, ItemID) VALUES ('{0}', '{1}', '{2}')", _
                                                         item.OwnerID, item.Slot, item.ItemID))

                End Select

            Else
                Throw New Exception("ItemManager:AddInvItem ItemID not found!")
            End If
        End Sub

        Public Sub UpdateItem(ByVal item As cItem)
            If GameDB.Items.ContainsKey(item.ID) Then
                GameDB.Items(item.ID) = item
                Database.SaveQuery(String.Format("Update items SET ObjectID='{0}', Plus='{1}', Variance='{2}', Data='{3}' WHERE ID='{4}'", _
                                                item.ObjectID, item.Plus, item.Variance, item.Data, item.ID))
            Else
                Throw New Exception("ItemManager:UpdateItem ItemID not found!")
            End If
        End Sub

        Public Sub UpdateInvItem(ByVal item As cInventoryItem, ByVal type As cInventoryItem.Type)
            Select Case type
                Case cInventoryItem.Type.Inventory
                    For i = 0 To GameDB.InventoryItems.Count - 1
                        If GameDB.InventoryItems(i).OwnerID = item.OwnerID And GameDB.InventoryItems(i).Slot = item.Slot Then
                            GameDB.InventoryItems(i) = item
                            Database.SaveQuery(String.Format("Update inventory SET ItemID='{0}' WHERE CharID='{1}' AND Slot='{2}'", _
                                                             item.ItemID, item.OwnerID, item.Slot))
                            Exit Sub
                        End If
                    Next

                Case cInventoryItem.Type.AvatarInventory
                    For i = 0 To GameDB.AvatarInventoryItems.Count - 1
                        If GameDB.AvatarInventoryItems(i).OwnerID = item.OwnerID And GameDB.AvatarInventoryItems(i).Slot = item.Slot Then
                            GameDB.AvatarInventoryItems(i) = item
                            Database.SaveQuery(String.Format("Update inventory_avatar SET ItemID='{0}' WHERE CharID='{1}' AND Slot='{2}'", _
                                                        item.ItemID, item.OwnerID, item.Slot))
                            Exit Sub
                        End If
                    Next

                Case cInventoryItem.Type.COSInventory
                    For i = 0 To GameDB.COSInventoryItems.Count - 1
                        If GameDB.COSInventoryItems(i).OwnerID = item.OwnerID And GameDB.COSInventoryItems(i).Slot = item.Slot Then
                            GameDB.COSInventoryItems(i) = item
                            Database.SaveQuery(String.Format("Update inventory_cos SET ItemID='{0}' WHERE COSID='{1}' AND Slot='{2}'", _
                                                        item.ItemID, item.OwnerID, item.Slot))
                            Exit Sub
                        End If
                    Next

                Case cInventoryItem.Type.Storage
                    For i = 0 To GameDB.StorageItems.Count - 1
                        If GameDB.StorageItems(i).OwnerID = item.OwnerID And GameDB.StorageItems(i).Slot = item.Slot Then
                            GameDB.StorageItems(i) = item
                            Database.SaveQuery(String.Format("Update storage SET ItemID='{0}' WHERE AccountID='{1}' AND Slot='{2}'", _
                                                        item.ItemID, item.OwnerID, item.Slot))
                            Exit Sub
                        End If
                    Next

                Case cInventoryItem.Type.GuildStorage
                    For i = 0 To GameDB.GuildStorageItems.Count - 1
                        If GameDB.GuildStorageItems(i).OwnerID = item.OwnerID And GameDB.GuildStorageItems(i).Slot = item.Slot Then
                            GameDB.GuildStorageItems(i) = item
                            Database.SaveQuery(String.Format("Update storage_guild SET ItemID='{0}' WHERE GuildID='{1}' AND Slot='{2}'", _
                                                      item.ItemID, item.OwnerID, item.Slot))
                            Exit Sub
                        End If
                    Next

            End Select
        End Sub

        Public Sub UpdateInvItem(ByVal ownerID As UInt32, ByVal slot As Byte, ByVal itemID As UInt64, ByVal type As cInventoryItem.Type)
            Select Case type
                Case cInventoryItem.Type.Inventory
                    For i = 0 To GameDB.InventoryItems.Count - 1
                        If GameDB.InventoryItems(i).OwnerID = ownerID And GameDB.InventoryItems(i).Slot = slot Then
                            GameDB.InventoryItems(i).ItemID = itemID
                            Database.SaveQuery(String.Format("Update inventory SET ItemID='{0}' WHERE CharID='{1}' AND Slot='{2}'", _
                                                            itemID, ownerID, slot))
                            Exit Sub
                        End If
                    Next

                Case cInventoryItem.Type.AvatarInventory
                    For i = 0 To GameDB.AvatarInventoryItems.Count - 1
                        If GameDB.AvatarInventoryItems(i).OwnerID = ownerID And GameDB.AvatarInventoryItems(i).Slot = slot Then
                            GameDB.AvatarInventoryItems(i).ItemID = itemID
                            Database.SaveQuery(String.Format("Update inventory_avatar SET ItemID='{0}' WHERE CharID='{1}' AND Slot='{2}'", _
                                                            itemID, ownerID, slot))
                            Exit Sub
                        End If
                    Next

                Case cInventoryItem.Type.COSInventory
                    For i = 0 To GameDB.COSInventoryItems.Count - 1
                        If GameDB.COSInventoryItems(i).OwnerID = ownerID And GameDB.COSInventoryItems(i).Slot = slot Then
                            GameDB.COSInventoryItems(i).ItemID = itemID
                            Database.SaveQuery(String.Format("Update inventory_cos SET ItemID='{0}' WHERE COSID='{1}' AND Slot='{2}'", _
                                                            itemID, ownerID, slot))
                            Exit Sub
                        End If
                    Next

                Case cInventoryItem.Type.Storage
                    For i = 0 To GameDB.StorageItems.Count - 1
                        If GameDB.StorageItems(i).OwnerID = ownerID And GameDB.StorageItems(i).Slot = slot Then
                            GameDB.StorageItems(i).ItemID = itemID
                            Database.SaveQuery(String.Format("Update storage SET ItemID='{0}' WHERE AccountID='{1}' AND Slot='{2}'", _
                                                            itemID, ownerID, slot))
                            Exit Sub
                        End If
                    Next

                Case cInventoryItem.Type.GuildStorage
                    For i = 0 To GameDB.GuildStorageItems.Count - 1
                        If GameDB.GuildStorageItems(i).OwnerID = ownerID And GameDB.GuildStorageItems(i).Slot = slot Then
                            GameDB.GuildStorageItems(i).ItemID = itemID
                            Database.SaveQuery(String.Format("Update storage_guild SET ItemID='{0}' WHERE GuildID='{1}' AND Slot='{2}'", _
                                                            itemID, ownerID, slot))
                            Exit Sub
                        End If
                    Next

            End Select
        End Sub

        Public Sub RemoveItem(ByVal ID As UInt64)
            If GameDB.Items.ContainsKey(ID) Then
                GameDB.Items.Remove(ID)
                Database.SaveQuery(String.Format("DELETE FROM items where ID='{0}'", ID))
            Else
                Throw New Exception("ItemManager:RemoveItem ItemID not found!")
            End If
        End Sub

        Public Sub RemoveInvItem(ByVal item As cInventoryItem, ByVal type As cInventoryItem.Type)
            Select Case type
                Case cInventoryItem.Type.Inventory
                    If GameDB.InventoryItems.Contains(item) Then
                        GameDB.InventoryItems.Remove(item)
                        Database.SaveQuery(String.Format("DELETE FROM inventory where CharID='{0}' AND Slot='{1}'", item.OwnerID, item.Slot))
                    End If

                Case cInventoryItem.Type.AvatarInventory
                    If GameDB.AvatarInventoryItems.Contains(item) Then
                        GameDB.AvatarInventoryItems.Remove(item)
                        Database.SaveQuery(String.Format("DELETE FROM inventory_avatar where CharID='{0}' AND Slot='{1}'", item.OwnerID, item.Slot))
                    End If

                Case cInventoryItem.Type.COSInventory
                    If GameDB.COSInventoryItems.Contains(item) Then
                        GameDB.COSInventoryItems.Remove(item)
                        Database.SaveQuery(String.Format("DELETE FROM inventory_cos where COSID='{0}' AND Slot='{1}'", item.OwnerID, item.Slot))
                    End If

                Case cInventoryItem.Type.Storage
                    If GameDB.StorageItems.Contains(item) Then
                        GameDB.StorageItems.Remove(item)
                        Database.SaveQuery(String.Format("DELETE FROM storage where AccountID='{0}' AND Slot='{1}'", item.OwnerID, item.Slot))
                    End If

                Case cInventoryItem.Type.GuildStorage
                    If GameDB.GuildStorageItems.Contains(item) Then
                        GameDB.GuildStorageItems.Remove(item)
                        Database.SaveQuery(String.Format("DELETE FROM storage_guild where GuildID='{0}' AND Slot='{1}'", item.OwnerID, item.Slot))
                    End If

            End Select
        End Sub

        Public Sub RemoveInvItem(ByVal ownerID As UInt32, ByVal slot As Byte, ByVal type As cInventoryItem.Type)
            Select Case Type
                Case cInventoryItem.Type.Inventory
                    For i = 0 To GameDB.InventoryItems.Count - 1
                        If GameDB.InventoryItems(i).OwnerID = ownerID And GameDB.InventoryItems(i).Slot = slot Then
                            GameDB.InventoryItems.Remove(GameDB.InventoryItems(i))
                            Database.SaveQuery(String.Format("DELETE FROM inventory where CharID='{0}' AND Slot='{1}'", GameDB.InventoryItems(i).OwnerID, GameDB.InventoryItems(i).Slot))
                            Exit For
                        End If
                    Next

                Case cInventoryItem.Type.AvatarInventory
                    For i = 0 To GameDB.AvatarInventoryItems.Count - 1
                        If GameDB.AvatarInventoryItems(i).OwnerID = ownerID And GameDB.AvatarInventoryItems(i).Slot = slot Then
                            GameDB.AvatarInventoryItems.Remove(GameDB.AvatarInventoryItems(i))
                            Database.SaveQuery(String.Format("DELETE FROM inventory_avatar where CharID='{0}' AND Slot='{1}'", GameDB.AvatarInventoryItems(i).OwnerID, GameDB.AvatarInventoryItems(i).Slot))
                            Exit For
                        End If
                    Next

                Case cInventoryItem.Type.COSInventory
                    For i = 0 To GameDB.COSInventoryItems.Count - 1
                        If GameDB.COSInventoryItems(i).OwnerID = ownerID And GameDB.COSInventoryItems(i).Slot = slot Then
                            GameDB.COSInventoryItems.Remove(GameDB.COSInventoryItems(i))
                            Database.SaveQuery(String.Format("DELETE FROM inventory_cos where COSID='{0}' AND Slot='{1}'", GameDB.COSInventoryItems(i).OwnerID, GameDB.COSInventoryItems(i).Slot))
                            Exit For
                        End If
                    Next

                Case cInventoryItem.Type.Storage
                    For i = 0 To GameDB.StorageItems.Count - 1
                        If GameDB.StorageItems(i).OwnerID = ownerID And GameDB.StorageItems(i).Slot = slot Then
                            GameDB.StorageItems.Remove(GameDB.StorageItems(i))
                            Database.SaveQuery(String.Format("DELETE FROM storage where AccountID='{0}' AND Slot='{1}'", GameDB.StorageItems(i).OwnerID, GameDB.StorageItems(i).Slot))
                            Exit For
                        End If
                    Next

                Case cInventoryItem.Type.GuildStorage
                    For i = 0 To GameDB.GuildStorageItems.Count - 1
                        If GameDB.GuildStorageItems(i).OwnerID = ownerID And GameDB.GuildStorageItems(i).Slot = slot Then
                            GameDB.GuildStorageItems.Remove(GameDB.GuildStorageItems(i))
                            Database.SaveQuery(String.Format("DELETE FROM storage_guild where GuildID='{0}' AND Slot='{1}'", GameDB.GuildStorageItems(i).OwnerID, GameDB.GuildStorageItems(i).Slot))
                            Exit For
                        End If
                    Next

            End Select
        End Sub

        Private Sub UpdateBlues(ByVal item As cItem)
            Dim array As UInt32(,) = New UInt32(7, 1) {}
            If item.Blues IsNot Nothing AndAlso item.Blues.Count > 0 Then
                For i = 0 To item.Blues.Count - 1
                    array(i, 0) = item.Blues(i).Type
                    array(i, 1) = item.Blues(i).Amout
                Next
                Database.SaveQuery(String.Format("UPDATE items SET blue1='{0}', blue1_amout='{1}', blue2='{2}', blue2_amout='{3}', blue3='{4}', blue3_amout='{5}', blue4='{6}', blue4_amout='{7}', " & _
                                                                 "blue5='{8}', blue5_amout='{9}', blue6='{10}', blue6_amout='{11}', blue7='{12}', blue7_amout='{13}', blue8='{14}', blue8_amout='{15}' " & _
                                                                 "WHERE ID='{16}'", _
                                                                 array(0, 0), array(0, 1), array(1, 0), array(1, 1), array(2, 0), array(2, 1), array(3, 0), array(3, 1), _
                                                                 array(4, 0), array(4, 1), array(5, 0), array(5, 1), array(6, 0), array(6, 1), array(7, 0), array(7, 1), _
                                                                 item.ID))
            End If
        End Sub
    End Module
End Namespace
