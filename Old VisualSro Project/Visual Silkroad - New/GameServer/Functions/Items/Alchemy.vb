Imports SRFramework

Namespace Functions
    Module Alchemy
        Public Sub OnAlchemyRequest(ByVal packet As PacketReader, ByVal Index_ As Integer)
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

        Public Sub OnAlchemyPlusNormal(ByVal packet As PacketReader, ByVal Index_ As Integer)

            Dim weaponSlot As Byte = packet.Byte
            Dim elexSlot As Byte = packet.Byte
            Dim weaponInv As cInventoryItem = Inventorys(Index_).UserItems(weaponSlot)
            Dim elexInv As cInventoryItem = Inventorys(Index_).UserItems(elexSlot)

            If weaponInv.ItemID = 0 Or GameDB.Items.ContainsKey(weaponInv.ItemID) = False Or elexInv.ItemID = 0 Or GameDB.Items.ContainsKey(elexInv.ItemID) = False Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim weapon As cItem = GameDB.Items(weaponInv.ItemID)
            Dim elex As cItem = GameDB.Items(elexInv.ItemID)
            Dim refWeapon As cRefItem = GetItemByID(weapon.ObjectID)
            Dim refElex As cRefItem = GetItemByID(elex.ObjectID)

            If refWeapon.CLASS_A <> 1 Or refElex.CLASS_A <> 3 Or refElex.CLASS_B <> 3 <> refElex.CLASS_C <> 10 Then
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
                writer.Byte(weaponInv.Slot)

                AddItemDataToPacket(weapon, writer)
            Else
                weapon.Plus = 0
                writer.Create(ServerOpcodes.GAME_ALCHEMY)
                writer.Byte(1)
                writer.Byte(2) 'mode
                writer.Byte(weapon.Plus) 'changed to 0
                writer.Byte(weaponInv.Slot) 'slot
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
                Inventorys(Index_).UserItems(elexSlot).ItemID = 0
            End If


            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1)
            writer.Byte(15) 'remove
            writer.Byte(elexSlot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnAlchemyPlusLucky(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim weaponSlot As Byte = packet.Byte
            Dim elexSlot As Byte = packet.Byte
            Dim powderSlot As Byte = packet.Byte

            Dim weaponInv As cInventoryItem = Inventorys(Index_).UserItems(weaponSlot)
            Dim elexInv As cInventoryItem = Inventorys(Index_).UserItems(elexSlot)
            Dim powderInv As cInventoryItem = Inventorys(Index_).UserItems(powderSlot)

            If weaponInv.ItemID = 0 Or GameDB.Items.ContainsKey(weaponInv.ItemID) = False Or elexInv.ItemID = 0 Or GameDB.Items.ContainsKey(elexInv.ItemID) = False Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim weapon As cItem = GameDB.Items(weaponInv.ItemID)
            Dim elex As cItem = GameDB.Items(elexInv.ItemID)
            Dim powder As cItem = GameDB.Items(elexInv.ItemID)

            Dim refWeapon As cRefItem = GetItemByID(weapon.ObjectID)
            Dim refElex As cRefItem = GetItemByID(elex.ObjectID)
            Dim refPowder As cRefItem = GetItemByID(powder.ObjectID)

            If refWeapon.CLASS_A <> 1 Or refElex.CLASS_A <> 3 Or refElex.CLASS_B <> 3 <> refElex.CLASS_C <> 10 Or refPowder.CLASS_A <> 3 Or refPowder.CLASS_B <> 3 <> refPowder.CLASS_C <> 10 Then
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
                writer.Byte(weaponInv.Slot)

                AddItemDataToPacket(weapon, writer)

            Else
                weapon.Plus = 0
                writer.Create(ServerOpcodes.GAME_ALCHEMY)
                writer.Byte(1)
                writer.Byte(2)   'mode

                writer.Byte(weapon.Plus)  'chnged to 0
                writer.Byte(weaponInv.Slot) 'slot
                writer.Byte(0)
                AddItemDataToPacket(weapon, writer)
            End If

            Server.Send(writer.GetBytes, Index_)

            ItemManager.UpdateItem(weapon)

            'Delete Items
            'Elex
            UpdateItemAmout(Index_, elexInv.Slot, -1)

            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1)
            writer.Byte(15)
            writer.Byte(elexSlot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, Index_)

            'Lucky Powder
            UpdateItemAmout(Index_, powderInv.Slot, -1)

            writer.Create(ServerOpcodes.GAME_ITEM_MOVE)
            writer.Byte(1)
            writer.Byte(15)
            writer.Byte(powderSlot)
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
