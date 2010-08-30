Namespace GameServer.Functions
    Module Alchemy
        Public Sub OnAlchemyRequest(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Dim tag As Byte = Packet.Byte
            Dim tag2 As Byte = Packet.Byte

            If tag = 2 And tag2 = 2 Then
                Dim weapon_slot As Byte = Packet.Byte
                Dim elex_slot As Byte = Packet.Byte
                Dim weapon As cInvItem = Inventorys(Index_).UserItems(weapon_slot)
                Dim elex As cInvItem = Inventorys(Index_).UserItems(elex_slot)

                If weapon Is Nothing Or elex Is Nothing Then
                    SendNotice("Weapon Slot Error.", Index_)
                End If
                Dim sucess As Boolean = CheckForSucess(weapon.Plus, 0)


                Dim writer As New PacketWriter
                If sucess = True Then
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

            End If
        End Sub

        ''' <summary>
        ''' Radom...
        ''' </summary>
        ''' <param name="Old_Plus"></param> Plus
        ''' <param name="Lucky"></param> Modifier
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckForSucess(ByVal Old_Plus As Byte, ByVal Lucky As Integer) As Boolean
            Dim New_Plus As Integer = Old_Plus + 1
            Dim ToReach As Integer = Old_Plus - Lucky
            Dim Rad As Integer = Math.Round((Rnd() * New_Plus), 0)
            If ToReach <= Rad Then
                'Sucess
                Return True
            Else
                'Failed
                Return False
            End If
        End Function
    End Module
End Namespace
