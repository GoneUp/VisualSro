Imports SRFramework

Namespace Functions
    Module Alchemy
        Public Sub OnAlchemyRequest(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Dim tag As Byte = Packet.Byte
            Dim tag2 As Byte = Packet.Byte

            If tag = 2 Then
                Select Case tag2
                    Case 2 '===========WEAPON + ELEX
                        OnAlchemyPlusNormal(Packet, Index_)

                    Case 3 '===========WEAPON + ELEX + POWDER
                        OnAlchemyPlusLucky(Packet, Index_)

                End Select
            End If
        End Sub

        Public Sub OnAlchemyPlusNormal(ByVal Packet As PacketReader, ByVal Index_ As Integer)

            Dim weapon_slot As Byte = Packet.Byte
            Dim elex_slot As Byte = Packet.Byte
            Dim weapon_inv As cInventoryItem = Inventorys(Index_).UserItems(weapon_slot)
            Dim elex_inv As cInventoryItem = Inventorys(Index_).UserItems(elex_slot)

            If weapon_inv.ItemID = 0 Or GameDB.Items.ContainsKey(weapon_inv.ItemID) = False Or elex_inv.ItemID = 0 Or GameDB.Items.ContainsKey(elex_inv.ItemID) = False Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim weapon As cItem = GameDB.Items(weapon_inv.ItemID)
            Dim elex As cItem = GameDB.Items(elex_inv.ItemID)
            Dim ref_weapon As cRefItem = GetItemByID(weapon.ObjectID)
            Dim ref_elex As cRefItem = GetItemByID(elex.ObjectID)

            If ref_weapon.CLASS_A <> 1 Or ref_elex.CLASS_A <> 3 Or ref_elex.CLASS_B <> 3 <> ref_elex.CLASS_C <> 10 Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim success As Boolean = CheckForSuccess(weapon.Plus, 0)


            Dim writer As New PacketWriter
            If success = True Then
                weapon.Plus += 1
                writer.Create(ServerOpcodes.GAME_ALCHEMY)
                writer.Byte(1)
                writer.Byte(2)
                writer.Byte(weapon.Plus)
                writer.Byte(weapon_inv.Slot)

                AddItemDataToPacket(weapon, writer)
            Else
                weapon.Plus = 0
                writer.Create(ServerOpcodes.GAME_ALCHEMY)
                writer.Byte(1)
                writer.Byte(2) 'mode
                writer.Byte(weapon.Plus) 'changed to 0
                writer.Byte(weapon_inv.Slot) 'slot
                writer.Byte(0)

                AddItemDataToPacket(weapon, writer)
            End If

            Server.Send(writer.GetBytes, Index_)

            ItemManager.UpdateItem(weapon)

            'Delete Items
            elex.Data -= 1
            If elex.Data > 0 Then
                ItemManager.UpdateItem(elex)
            ElseIf elex.Data = 0 Then
                ItemManager.RemoveItem(elex.ID)
                Inventorys(Index_).UserItems(elex_slot).ItemID = 0
            End If


            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1)
            writer.Byte(15) 'remove
            writer.Byte(elex_slot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnAlchemyPlusLucky(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Dim weapon_slot As Byte = Packet.Byte
            Dim elex_slot As Byte = Packet.Byte
            Dim powder_slot As Byte = Packet.Byte

            Dim weapon_inv As cInventoryItem = Inventorys(Index_).UserItems(weapon_slot)
            Dim elex_inv As cInventoryItem = Inventorys(Index_).UserItems(elex_slot)
            Dim powder_inv As cInventoryItem = Inventorys(Index_).UserItems(powder_slot)

            If weapon_inv.ItemID = 0 Or GameDB.Items.ContainsKey(weapon_inv.ItemID) = False Or elex_inv.ItemID = 0 Or GameDB.Items.ContainsKey(elex_inv.ItemID) = False Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim weapon As cItem = GameDB.Items(weapon_inv.ItemID)
            Dim elex As cItem = GameDB.Items(elex_inv.ItemID)
            Dim powder As cItem = GameDB.Items(elex_inv.ItemID)

            Dim ref_weapon As cRefItem = GetItemByID(weapon.ObjectID)
            Dim ref_elex As cRefItem = GetItemByID(elex.ObjectID)
            Dim ref_powder As cRefItem = GetItemByID(powder.ObjectID)

            If ref_weapon.CLASS_A <> 1 Or ref_elex.CLASS_A <> 3 Or ref_elex.CLASS_B <> 3 <> ref_elex.CLASS_C <> 10 Or ref_powder.CLASS_A <> 3 Or ref_powder.CLASS_B <> 3 <> ref_powder.CLASS_C <> 10 Then
                Server.Disconnect(Index_)
                Exit Sub
            End If


            Dim success As Boolean = CheckForSuccess(weapon.Plus, 1)


            Dim writer As New PacketWriter
            If success = True Then
                weapon.Plus += 1
                writer.Create(ServerOpcodes.GAME_ALCHEMY)
                writer.Byte(1)
                writer.Byte(2)
                writer.Byte(weapon.Plus)
                writer.Byte(weapon_inv.Slot)

                AddItemDataToPacket(weapon, writer)

            Else
                weapon.Plus = 0
                writer.Create(ServerOpcodes.GAME_ALCHEMY)
                writer.Byte(1)
                writer.Byte(2)   'mode

                writer.Byte(weapon.Plus)  'chnged to 0
                writer.Byte(weapon_inv.Slot) 'slot
                writer.Byte(0)
                AddItemDataToPacket(weapon, writer)
            End If

            Server.Send(writer.GetBytes, Index_)

            ItemManager.UpdateItem(weapon)

            'Delete Items
            'Elex
            elex.Data -= 1
            If elex.Data > 0 Then
                ItemManager.UpdateItem(elex)
            ElseIf elex.Data = 0 Then
                ItemManager.RemoveItem(elex.ID)
                Inventorys(Index_).UserItems(elex_slot).ItemID = 0
            End If

            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1)
            writer.Byte(15)
            writer.Byte(elex_slot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, Index_)

            'Lucky Powder
            powder.Data -= 1
            If powder.Data > 0 Then
                ItemManager.UpdateItem(powder)
            ElseIf powder.Data = 0 Then
                ItemManager.RemoveItem(powder.ID)
                Inventorys(Index_).UserItems(powder_slot).ItemID = 0
            End If

            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1)
            writer.Byte(15)
            writer.Byte(powder_slot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, Index_)
        End Sub


        ''' <summary>
        ''' Radom...
        ''' </summary>
        ''' <param name="Old_Plus"></param> Plus
        ''' <param name="Lucky"></param> Modifier
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckForSuccess(ByVal Old_Plus As Byte, ByVal Lucky As Integer) As Boolean
            Dim New_Plus As Integer = Old_Plus + 1
            Dim ToReach As Integer = Old_Plus - Lucky
            Dim Rad As Integer = Math.Round((Rnd() * New_Plus), 0)
            If ToReach <= Rad Then
                'Success
                Return True
            Else
                'Failed
                Return False
            End If
        End Function
    End Module
End Namespace
