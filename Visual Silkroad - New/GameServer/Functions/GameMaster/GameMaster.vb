Namespace GameServer.Functions
    Module GameMaster
        Public Sub OnGM(ByVal Packet As GameServer.PacketReader, ByVal Index As Integer)

            Dim tag As Byte = Packet.Byte
            Debug.Print("[GM][Tag:" & tag & "]")

            If PlayerData(Index).GM = True Then
                Select Case tag

                    Case GmTypes.MakeItem  ' Create Item
                        OnGmCreateItem(Packet, Index)

                    Case GmTypes.WayPoints 'Teleport
                        OnGmTeleport(Packet, Index)


                End Select
            Else
                'Server.Dissconnect(Index)
                'Hack Versuch
            End If
        End Sub

        Public Sub OnGmCreateItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim pk2id As UInteger = packet.DWord
            Dim plus As Byte = packet.Byte  'Or Count

            For i = 13 To PlayerData(index_).MaxSlots
                If Inventorys(index_).UserItems(i).Pk2Id = 0 Then
                    Dim temp_item As cInvItem = Inventorys(index_).UserItems(i)
                    temp_item.Pk2Id = pk2id
                    temp_item.OwnerCharID = PlayerData(index_).UniqueId


                    Dim ref As cItem = GetItemByID(pk2id)
                    If ref.CLASS_A = 1 Then
                        'Equip
                        temp_item.Plus = plus
                        temp_item.Durability = 30

                    ElseIf ref.CLASS_A = 2 Then
                        'Pet

                    ElseIf ref.CLASS_A = 3 Then
                        'Etc
                        temp_item.Amount = plus
                    End If


                    Inventorys(index_).UserItems(i) = temp_item
                    UpdateItem(Inventorys(index_).UserItems(i)) 'SAVE IT

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.ItemMove)
                    writer.Byte(1)
                    writer.Byte(6) 'type = new item
                    writer.Byte(Inventorys(index_).UserItems(i).Slot)
                    writer.DWord(Inventorys(index_).UserItems(i).Pk2Id)

                    Select Case ref.CLASS_A
                        Case 1 'Equipment

                            writer.Byte(Inventorys(index_).UserItems(i).Plus)
                            writer.QWord(0) 'blue, unknwown
                            writer.DWord(Inventorys(index_).UserItems(i).Durability)
                            writer.Byte(0)
                        Case 2 'Pets

                        Case 3 'etc
                            writer.Word(Inventorys(index_).UserItems(i).Amount)
                    End Select

                    Server.Send(writer.GetBytes, index_)
                    Exit For
                End If
            Next
        End Sub

        Public Sub OnGmTeleport(ByVal packet As PacketReader, ByVal index_ As Integer)

            Dim ToXSector As Byte = packet.Byte
            Dim ToYSector As Byte = packet.Byte
            Dim Angle As UInt16 = packet.Word 'Not sure 
            Dim ToYPos As UInt16 = (packet.Word)
            Dim ToZPos As UInt16 = (packet.Word)
            Dim ToXPos As UInt16 = (packet.Word)


            PlayerData(index_).XSector = ToXSector
            PlayerData(index_).YSector = ToYSector
            PlayerData(index_).X = ToXPos
            PlayerData(index_).Z = ToZPos
            PlayerData(index_).Y = ToYPos
        End Sub
        Public Sub OnSetWeather(ByVal packet As PacketReader)




        End Sub

        Enum GmTypes
            FindUser = &H1
            GoTown = &H2
            MakeMonster = &H6
            MakeItem = &H7
            MoveToUser = &H8
            WayPoints = &H10
            Ban = &H13
            MoveToNpc = &H31
        End Enum

    End Module
End Namespace
