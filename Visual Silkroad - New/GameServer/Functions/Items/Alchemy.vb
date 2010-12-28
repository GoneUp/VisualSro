Namespace GameServer.Functions
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

            Inventorys(Index_).ReOrderItems(Index_)
        End Sub
        Public Sub OnAlchemyPlusNormal(ByVal Packet As PacketReader, ByVal Index_ As Integer)

            Dim weapon_slot As Byte = Packet.Byte
            Dim elix_slot As Byte = Packet.Byte
            Dim weapon As cInvItem = Inventorys(Index_).UserItems(weapon_slot)
            Dim elix As cInvItem = Inventorys(Index_).UserItems(elix_slot)

            If weapon Is Nothing Or elix Is Nothing Then
                SendNotice("Weapon Slot Error.", Index_)
            End If
            Dim success As Boolean = CheckForSuccess(weapon.Plus, 0)


            Dim writer As New PacketWriter
            If success = True Then
                weapon.Plus += 1
                writer.Create(ServerOpcodes.Alchemy)
                writer.Byte(1)
                writer.Byte(2)
                writer.Byte(weapon.Plus)
                writer.Byte(weapon.Slot)
                writer.DWord(weapon.Pk2Id)
                writer.Byte(weapon.Plus)
                writer.QWord(0) 'modifier
                writer.DWord(weapon.Durability)
                writer.Byte(0) 'clues
            Else
                weapon.Plus = 0
                writer.Create(ServerOpcodes.Alchemy)
                writer.Byte(1)
                writer.Byte(2) 'mode
                writer.Byte(weapon.Plus) 'chnged to 0
                writer.Byte(weapon.Slot) 'slot
                writer.Byte(0)
                writer.DWord(weapon.Pk2Id)
                writer.Byte(weapon.Plus)
                writer.QWord(0) 'modifier
                writer.DWord(weapon.Durability)
                writer.Byte(0) 'blues
            End If

            Server.Send(writer.GetBytes, Index_)

            DataBase.SaveQuery(String.Format("UPDATE items SET plusvalue='{0}' where id='{1}'", weapon.Plus, weapon.DatabaseID))
            Inventorys(Index_).UserItems(weapon_slot) = weapon

            'Delete Items
            DeleteItemFromDB(elix_slot, Index_)
            Inventorys(Index_).UserItems(elix_slot) = ClearItem(Inventorys(Index_).UserItems(elix_slot))

            writer.Create(ServerOpcodes.ItemMove)
            writer.Byte(1)
            writer.Byte(&HF)
            writer.Byte(elix_slot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnAlchemyPlusLucky(ByVal Packet As PacketReader, ByVal Index_ As Integer)

            Dim weapon_slot As Byte = Packet.Byte
            Dim elix_slot As Byte = Packet.Byte
            Dim powder_slot As Byte = Packet.Byte
            Dim weapon As cInvItem = Inventorys(Index_).UserItems(weapon_slot)
            Dim elix As cInvItem = Inventorys(Index_).UserItems(elix_slot)
            Dim powder As cInvItem = Inventorys(Index_).UserItems(powder_slot)

            If weapon Is Nothing Or elix Is Nothing Or powder Is Nothing Then
                SendNotice("Weapon Slot Error.", Index_)
            End If
            Dim success As Boolean = CheckForSuccess(weapon.Plus, 1)


            Dim writer As New PacketWriter
            If success = True Then
                weapon.Plus += 1
                writer.Create(ServerOpcodes.Alchemy)
                writer.Byte(1)
                writer.Byte(2)
                writer.Byte(weapon.Plus)
                writer.Byte(weapon.Slot)
                writer.DWord(weapon.Pk2Id)
                writer.Byte(weapon.Plus)
                writer.QWord(0) 'modifier
                writer.DWord(weapon.Durability)
                writer.Byte(0) 'clues
            Else
                weapon.Plus = 0
                writer.Create(ServerOpcodes.Alchemy)
                writer.Byte(1)
                writer.Byte(2) 'mode
                writer.Byte(weapon.Plus) 'chnged to 0
                writer.Byte(weapon.Slot) 'slot
                writer.Byte(0)
                writer.DWord(weapon.Pk2Id)
                writer.Byte(weapon.Plus)
                writer.QWord(0) 'modifier
                writer.DWord(weapon.Durability)
                writer.Byte(0) 'blues
            End If

            Server.Send(writer.GetBytes, Index_)
            DataBase.SaveQuery(String.Format("UPDATE items SET plusvalue='{0}' where id='{1}'", weapon.Plus, weapon.DatabaseID))
            Inventorys(Index_).UserItems(weapon_slot) = weapon

            'Delete Items
            'Elex
            DeleteItemFromDB(elix_slot, Index_)
            Inventorys(Index_).UserItems(elix_slot) = ClearItem(Inventorys(Index_).UserItems(elix_slot))

            writer.Create(ServerOpcodes.ItemMove)
            writer.Byte(1)
            writer.Byte(&HF)
            writer.Byte(elix_slot)
            writer.Byte(4)
            Server.Send(writer.GetBytes, Index_)

            'Lucky Powder
            DeleteItemFromDB(powder_slot, Index_)
            Inventorys(Index_).UserItems(powder_slot) = ClearItem(Inventorys(Index_).UserItems(powder_slot))

            writer.Create(ServerOpcodes.ItemMove)
            writer.Byte(1)
            writer.Byte(&HF)
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
