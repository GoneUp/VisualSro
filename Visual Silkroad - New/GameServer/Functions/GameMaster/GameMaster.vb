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

					Case GmTypes.MoveToUser
						OnMoveToUser(Packet, Index)

					Case GmTypes.RecallUser
						OnRecallUser(Packet, Index)

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
            Dim ToXPos As Single = packet.Float
            Dim ToZPos As Single = packet.Float
            Dim ToYPos As Single = packet.Float
            Dim Angle As UInt16 = packet.Word 'Not sure 

            PlayerData(index_).XSector = ToXSector
            PlayerData(index_).YSector = ToYSector
            PlayerData(index_).X = ToXPos
            PlayerData(index_).Z = ToZPos
            PlayerData(index_).Y = ToYPos

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Teleport_Annonce)
            writer.Byte(PlayerData(index_).XSector)
            writer.Byte(PlayerData(index_).YSector)
            Server.Send(writer.GetBytes, index_)

        End Sub
        Public Sub OnSetWeather(ByVal Type As Byte, ByVal Strength As Byte, ByVal index_ As Integer)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Weather)
            writer.Byte(Type)
            writer.Byte(Strength)
            Server.SendToAllIngame(writer.GetBytes, index_)


		End Sub

		Private Sub OnMoveToUser(ByVal packet As PacketReader, ByVal index_ As Integer)
			Dim NameLength As Byte = packet.Word
			Dim Name As String = packet.String(NameLength)

			For i As Integer = 0 To Server.OnlineClient
				If PlayerData(i).CharacterName = Name Then
					Dim ToXSector As Byte = PlayerData(i).XSector
					Dim ToYSector As Byte = PlayerData(i).YSector
					Dim ToXPos As Single = PlayerData(i).X
					Dim ToZPos As Single = PlayerData(i).Z
					Dim ToYPos As Single = PlayerData(i).Y
					Dim Angle As UInt16 = PlayerData(i).Angle

					PlayerData(index_).XSector = ToXSector
					PlayerData(index_).YSector = ToYSector
					PlayerData(index_).X = ToXPos
					PlayerData(index_).Z = ToZPos
					PlayerData(index_).Y = ToYPos

					Dim writer As New PacketWriter
					writer.Create(ServerOpcodes.Teleport_Annonce)
					writer.Byte(PlayerData(index_).XSector)
					writer.Byte(PlayerData(index_).YSector)
					Server.Send(writer.GetBytes, index_)

					Exit For
				End If
			Next

		End Sub

		Private Sub OnRecallUser(ByVal packet As PacketReader, ByVal index_ As Integer)
			Dim NameLength As Byte = packet.Word
			Dim Name As String = packet.String(NameLength)

			For i As Integer = 0 To Server.OnlineClient
				If PlayerData(i).CharacterName = Name Then
					Dim ToXSector As Byte = PlayerData(index_).XSector
					Dim ToYSector As Byte = PlayerData(index_).YSector
					Dim ToXPos As Single = PlayerData(index_).X
					Dim ToZPos As Single = PlayerData(index_).Z
					Dim ToYPos As Single = PlayerData(index_).Y
					Dim Angle As UInt16 = PlayerData(index_).Angle

					PlayerData(i).XSector = ToXSector
					PlayerData(i).YSector = ToYSector
					PlayerData(i).X = ToXPos
					PlayerData(i).Z = ToZPos
					PlayerData(i).Y = ToYPos

					Dim writer As New PacketWriter
					writer.Create(ServerOpcodes.Teleport_Annonce)
					writer.Byte(PlayerData(i).XSector)
					writer.Byte(PlayerData(i).YSector)
					Server.Send(writer.GetBytes, i)

					Exit For
				End If
			Next

		End Sub

        Enum GmTypes
            FindUser = &H1
            GoTown = &H2
            MakeMonster = &H6
            MakeItem = &H7
            MoveToUser = &H8
			WayPoints = &H10
			RecallUser = &H11
			Ban = &H13
			MoveToNpc = &H31
		End Enum

    End Module
End Namespace
