﻿Namespace GameServer.Functions
    Module GameMaster
        Public Sub OnGM(ByVal Packet As GameServer.PacketReader, ByVal Index As Integer)

            Dim tag As Byte = Packet.Byte
            Debug.Print("[GM][Tag:" & tag & "]")

            If PlayerData(Index).GM = True Then
                Select Case tag
                    Case GmTypes.MakeMonster
                        OnCreateObject(Packet, Index)

                    Case GmTypes.MakeItem  ' Create Item
                        OnGmCreateItem(Packet, Index)

                    Case GmTypes.WayPoints 'Teleport
                        OnGmTeleport(Packet, Index)

					Case GmTypes.MoveToUser
						OnMoveToUser(Packet, Index)

					Case GmTypes.RecallUser
                        OnRecallUser(Packet, Index)

                    Case GmTypes.Ban
                        OnBanUser(Packet, Index)

                    Case GmTypes.GoTown
                        OnGoTown(Index)

                    Case GmTypes.ToTown
                        OnMoveUserToTown(Index, Packet)

                End Select
            Else
                'Server.Dissconnect(Index)
                'Hack Versuch
            End If
        End Sub

        Public Sub OnGmCreateItem(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim pk2id As UInteger = packet.DWord
            Dim plus As Byte = packet.Byte  'Or Count
          


            For i = 13 To PlayerData(index_).MaxSlots - 1
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

                    If temp_item.Slot = 0 Then
                        Debug.Print(0)
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

                    Debug.Print("[ITEM CREATE][Info][Slot:{0}][ID:{1}][Dura:{2}][Amout:{3}][Plus:{4}]", temp_item.Slot, temp_item.Pk2Id, temp_item.Durability, temp_item.Amount, temp_item.Plus)
                    Exit For
                End If
            Next
        End Sub

        Public Sub OnGmTeleport(ByVal packet As PacketReader, ByVal index_ As Integer)

            Dim to_pos As New Position
            to_pos.XSector = packet.Byte
            to_pos.YSector = packet.Byte
            to_pos.X = packet.Float
            to_pos.Z = packet.Float
            to_pos.Y = packet.Float
            Dim Angle As UInt16 = packet.Word 'Not sure 

            PlayerData(index_).Position = to_pos
        

            DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(index_).Position.XSector, PlayerData(index_).Position.YSector, Math.Round(PlayerData(index_).Position.X), Math.Round(PlayerData(index_).Position.Z), Math.Round(PlayerData(index_).Position.Y), PlayerData(index_).UniqueId))

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Teleport_Annonce)
            writer.Byte(PlayerData(index_).Position.XSector)
            writer.Byte(PlayerData(index_).Position.YSector)
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

            For i As Integer = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).CharacterName = Name Then

                        PlayerData(index_).Position = PlayerData(i).Position

                        DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(index_).Position.XSector, PlayerData(index_).Position.YSector, Math.Round(PlayerData(index_).Position.X), Math.Round(PlayerData(index_).Position.Z), Math.Round(PlayerData(index_).Position.Y), PlayerData(index_).UniqueId))

                        OnTeleportUser(index_, PlayerData(index_).Position.XSector, PlayerData(index_).Position.YSector)

                        Exit For
                    End If
                End If
            Next

		End Sub

		Private Sub OnRecallUser(ByVal packet As PacketReader, ByVal index_ As Integer)
            Dim NameLength As UInt16 = packet.Word
			Dim Name As String = packet.String(NameLength)

            For i As Integer = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).CharacterName = Name Then

                        PlayerData(i).Position = PlayerData(index_).Position

                        DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(i).Position.XSector, PlayerData(i).Position.YSector, Math.Round(PlayerData(i).Position.X), Math.Round(PlayerData(i).Position.Z), Math.Round(PlayerData(i).Position.Y), PlayerData(i).UniqueId))

                        OnTeleportUser(i, PlayerData(i).Position.XSector, PlayerData(i).Position.YSector)
                        Exit For
                    End If
                End If
            Next
        End Sub

        Public Sub OnBanUser(ByVal Packet As PacketReader, ByVal index_ As Integer)
            Dim NameLength As UInt16 = Packet.Word
            Dim Name As String = Packet.String(NameLength)

            For i As Integer = 0 To DatabaseCore.CharCount
                If DatabaseCore.Chars(i).CharacterName = Name Then
                    DataBase.InsertData(String.Format("UPDATE users SET banned='1', bantime = '3000-01-01 00:00:00', banreason = 'You got banned by: {0}' where id='{1}'", PlayerData(index_).CharacterName, DatabaseCore.Chars(i).AccountID))

                    For U = 0 To DatabaseCore.UserCount
                        If DatabaseCore.Users(U).Id = DatabaseCore.Chars(i).AccountID Then
                            DatabaseCore.Users(U).Banned = True
                        End If
                    Next

                    SendPm(index_, "User got banned.", "[SERVER]")
                    Exit For
                End If
            Next

            For i As Integer = 0 To Server.OnlineClient
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).CharacterName = Name Then
                        Server.Dissconnect(i)
                    End If
                End If
            Next i

        End Sub

        Public Sub OnGoTown(ByVal Index_ As Integer)

            'Teleport the GM to Town
            PlayerData(Index_).Position = Functions.PlayerData(Index_).Position_Return 'Set new Pos
            DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).UniqueId))
            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)

        End Sub

        Public Sub OnMoveUserToTown(ByVal Index_ As Integer, ByVal packet As PacketReader)

            Dim NameLength As UInt16 = packet.Word
            Dim Name As String = packet.String(NameLength)

            For i As Integer = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).CharacterName = Name Then
                        OnGoTown(i)
                    End If
                End If
            Next
        End Sub

        Public Sub OnCreateObject(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Dim objectid As UInteger = Packet.DWord
            Dim type As Byte = Packet.Byte
            Dim refobject As Object_ = GetObjectById(objectid)

            Dim selector As String = refobject.Name.Substring(0, 3)

            Select Case selector
                Case "MOB"
                    SpawnMob(objectid, type, PlayerData(Index_).Position)
                Case "NPC"
                    SpawnNPC(objectid, PlayerData(Index_).Position)
            End Select
        End Sub


        Enum GmTypes
            FindUser = &H1
            GoTown = &H2
            ToTown = &H3
            MakeMonster = &H6
            MakeItem = &H7
            MoveToUser = &H8
			WayPoints = &H10
			RecallUser = &H11
            Ban = &HD
			MoveToNpc = &H31
		End Enum

    End Module
End Namespace
