Namespace GameServer.Functions
    Module Stall

        Public Stalls As New List(Of cStall)
        Public StallIdCounter As UInt32 = 0


        Public Sub Stall_Open_Own(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim NameLen As UShort = packet.Word
            Dim Name As String = packet.UString(NameLen)

            If PlayerData(Index_).Busy = False And PlayerData(Index_).InStall = False Then
                PlayerData(Index_).InStall = True
                PlayerData(Index_).StallID = GetStallId()
                PlayerData(Index_).StallOwner = True
                PlayerData(Index_).Busy = True

                Dim tmp As New cStall
                tmp.StallID = PlayerData(Index_).StallID
                tmp.OwnerID = PlayerData(Index_).UniqueId
                tmp.OwnerIndex = Index_
                tmp.StallName = Name
                tmp.Init()

                Stalls.Add(tmp)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Stall_Open_Reply)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.Stall_Open_ToOther)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Word(tmp.StallName.Length)
                writer.UString(tmp.StallName)
                writer.DWord(0)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
            End If

        End Sub

        Public Sub Stall_Data(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim tag As Byte = packet.Byte

            Select Case tag
                Case 1
                    'change Price
                    Stall_ChangePrice(packet, Index_)
                Case 2
                    'Additem
                    Stall_AddItem(packet, Index_)
                Case 3
                    'REmove
                    Stall_RemoveItem(packet, Index_)
                Case 5
                    'Open and Close
                    Stall_ChangeState(packet, Index_)
                Case 6
                    'Welcome Message
                    Stall_ChangeWelcomeMessage(packet, Index_)
                Case 7
                    'Change Name
                    Stall_ChangeName(packet, Index_)
            End Select
        End Sub

        Public Sub Stall_ChangeWelcomeMessage(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim NameLen As UShort = packet.Word
            Dim Name As String = packet.UString(NameLen)

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim Index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                Stalls(Index).WelcomeMessage = Name

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Stall_Data)
                writer.Byte(1)
                writer.Byte(6)
                writer.Word(NameLen)
                writer.UString(Name)
                Server.Send(writer.GetBytes, Index)
            End If

        End Sub

        Public Sub Stall_ChangeName(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim NameLen As UShort = packet.Word
            Dim Name As String = packet.UString(NameLen)

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim Stall_Index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                Stalls(Stall_Index).StallName = Name

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Stall_Data)
                writer.Byte(1)
                writer.Byte(7)
                Server.Send(writer.GetBytes, Index_)


                writer.Create(ServerOpcodes.Stall_Name)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Word(NameLen)
                writer.UString(Name)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
            End If
        End Sub

        Public Sub Stall_AddItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ToSlot As Byte = packet.Byte
            Dim FromSlot As Byte = packet.Byte
            Dim Amout As UInt16 = packet.Word
            Dim Gold As ULong = packet.QWord
            Dim Unknown As UInt32 = packet.DWord

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If FromSlot < 13 Then
                    'Cannot Sell a Equiped Item
                    Exit Sub
                End If

                If Inventorys(Index_).UserItems(FromSlot).Pk2Id <> 0 And Stalls(index).Items(ToSlot).Slot = 0 Then
                    Stalls(index).Items(ToSlot).Slot = FromSlot
                    Stalls(index).Items(ToSlot).Gold = Gold


                    Stall_SendItemsOwn(index, Index_, 2)
                    Server.SendToStallSession(Stall_SendItemsOther(index, index), PlayerData(Index_).StallID, False)
                End If
            End If
        End Sub

        Public Sub Stall_RemoveItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim Slot As Byte = packet.Byte
            Dim amout As UInt16 = packet.Word

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If Stalls(index).Items(Slot).Slot <> 0 Then
                    Stalls(index).Items(Slot).Slot = 0
                    Stalls(index).Items(Slot).Gold = 0

                    Stall_SendItemsOwn(index, Index_, 3)
                    Server.SendToStallSession(Stall_SendItemsOther(index, index), PlayerData(Index_).StallID, False)
                End If
            End If
        End Sub

        Public Sub Stall_ChangeState(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim State As Byte = packet.Byte
            Dim Options As UInt16 = packet.Word

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If State = 1 And Stalls(index).Open = False Then
                    'Open Stall
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Stall_Data)
                    writer.Byte(1)
                    writer.Byte(5)
                    writer.Byte(State)
                    writer.Word(Options)
                    Server.SendToStallSession(writer.GetBytes, Stalls(index).StallID, True)

                    Stalls(index).Open = True
                ElseIf State = 0 And Stalls(index).Open Then
                    'Close Stall
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Stall_Data)
                    writer.Byte(1)
                    writer.Byte(5)
                    writer.Byte(State)
                    writer.Word(Options)
                    Server.SendToStallSession(writer.GetBytes, Stalls(index).StallID, True)

                    Stalls(index).Open = False
                End If
            End If
        End Sub

        Public Sub Stall_ChangePrice(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim amout As UShort = packet.Word
            Dim price As ULong = packet.QWord
            Dim options As UShort = packet.Word

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 And PlayerData(Index_).StallOwner = True Then
                Dim Stall_index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If slot >= 0 And slot <= 9 And Stalls(Stall_index).Items(slot).Slot <> 0 Then
                    Stalls(Stall_index).Items(slot).Gold = price

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Stall_Data)
                    writer.Byte(1)
                    writer.Byte(1)
                    writer.Byte(slot)
                    writer.Word(amout)
                    writer.QWord(price)
                    writer.Word(options)
                    Server.SendToStallSession(writer.GetBytes, Stalls(Stall_index).StallID, True)
                End If
            End If
        End Sub

        Public Sub Stall_Buy(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim slot As Byte = packet.Byte

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 And PlayerData(Index_).StallOwner = False Then
                Dim Stall_index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If slot >= 0 And slot <= 9 And Stalls(Stall_index).Items(slot).Slot <> 0 Then
                    If CULng(PlayerData(Index_).Gold) - Stalls(Stall_index).Items(slot).Gold >= 0 Then
                        PlayerData(Index_).Gold -= Stalls(Stall_index).Items(slot).Gold
                        UpdateGold(Index_)

                        PlayerData(Stalls(Stall_index).OwnerIndex).Gold += Stalls(Stall_index).Items(slot).Gold
                        UpdateGold(Stalls(Stall_index).OwnerIndex)

                        Dim From_item As cInvItem = Inventorys(Stalls(Stall_index).OwnerIndex).UserItems(Stalls(Stall_index).Items(slot).Slot)
                        Dim To_Slot As Byte = GetFreeItemSlot(Index_)
                        Dim To_item As cInvItem = Inventorys(Index_).UserItems(To_Slot)

                        'Overwrite
                        To_item.Pk2Id = From_item.Pk2Id
                        To_item.Durability = From_item.Durability
                        To_item.Plus = From_item.Plus
                        To_item.Amount = From_item.Amount
                        To_item.Blues = From_item.Blues
                        To_item.Mod_1 = From_item.Mod_1
                        To_item.Mod_2 = From_item.Mod_2
                        To_item.Mod_3 = From_item.Mod_3
                        To_item.Mod_4 = From_item.Mod_4
                        To_item.Mod_5 = From_item.Mod_5
                        To_item.Mod_6 = From_item.Mod_6
                        To_item.Mod_7 = From_item.Mod_7
                        To_item.Mod_8 = From_item.Mod_8

                        'Add to new...
                        Inventorys(Index_).UserItems(To_item.Slot) = To_item
                        UpdateItem(To_item)

                        'Remove...
                        DeleteItemFromDB(Stalls(Stall_index).Items(slot).Slot, Stalls(Stall_index).OwnerIndex)
                        Inventorys(Stalls(Stall_index).OwnerIndex).UserItems(Stalls(Stall_index).Items(slot).Slot) = ClearItem(From_item)

                        Stalls(Stall_index).Items(slot).Gold = 0
                        Stalls(Stall_index).Items(slot).Slot = 0

                        'Packets..
                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.Stall_Buy)
                        writer.Byte(1)
                        writer.Byte(slot)
                        Server.Send(writer.GetBytes, Index_)

                        writer.Create(ServerOpcodes.Stall_Message)
                        writer.Byte(3)
                        writer.Byte(slot)
                        writer.Word(PlayerData(Index_).CharacterName.Length)
                        writer.String(PlayerData(Index_).CharacterName)
                        writer.Byte(&HFF)
                        Server.SendToStallSession(writer.GetBytes, Stalls(Stall_index).StallID, True)
                    End If
                End If
            End If
        End Sub

        Public Sub Stall_Chat(ByVal packet As PacketReader, ByVal Index_ As Integer)

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim counter As Byte = packet.Byte
                Dim messagelength As UInt16 = packet.Word
                Dim message As String = packet.UString(messagelength)

                Dim writer As New PacketWriter 'Reply to sender
                writer.Create(ServerOpcodes.Chat_Accept)
                writer.Byte(1)
                writer.Byte(ChatModes.Stall)
                writer.Byte(counter)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.Chat)
                writer.Byte(ChatModes.Stall)
                writer.Word(PlayerData(Index_).CharacterName.Length)
                writer.String(PlayerData(Index_).CharacterName)
                writer.Word(messagelength)
                writer.UString(message)
                Server.SendToStallSession(writer.GetBytes, PlayerData(Index_).StallID, True)
            End If
        End Sub


        Public Sub Stall_Open_Other(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ToUniqueID As UInt32 = packet.DWord

            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).UniqueId = ToUniqueID Then
                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.Stall_Message)
                        writer.Byte(2)
                        writer.DWord(PlayerData(Index_).UniqueId)
                        Server.SendToStallSession(writer.GetBytes, PlayerData(i).StallID, True)

                        Dim Stall_index As Integer = GetStallIndex(PlayerData(i).StallID)
                        Stalls(Stall_index).Visitors.Add(Index_)

                        PlayerData(Index_).Busy = True
                        PlayerData(Index_).InStall = True
                        PlayerData(Index_).StallID = Stalls(Stall_index).StallID
                        PlayerData(Index_).StallOwner = False


                        Server.Send(Stall_SendItemsOther(Stall_index, Index_), Index_)
                    End If
                End If
            Next
        End Sub

        Public Sub Stall_Close_Own(ByVal Index_ As Integer)

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Stall_Close_Owner_Other)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Byte(&H17)
                writer.Byte(&H3C)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.Stall_Close_Owner)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)

                Dim Stall_index As Integer = GetStallIndex(PlayerData(Index_).StallID)
                Stalls.RemoveAt(Stall_index)

                PlayerData(Index_).Busy = False
                PlayerData(Index_).StallID = 0
                PlayerData(Index_).InStall = False
                PlayerData(Index_).StallOwner = False
            End If
        End Sub


        Public Sub Stall_Close_Other(ByVal Index_ As Integer)
            For i = 0 To Stalls.Count - 1
                If Stalls(i).Visitors.Contains(Index_) Then
                    Stalls(i).Visitors.Remove(Index_)
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Stall_Message)
                    writer.Byte(1)
                    writer.DWord(PlayerData(Index_).UniqueId)
                    Server.SendToStallSession(writer.GetBytes, PlayerData(i).StallID, True)

                    writer.Create(ServerOpcodes.Stall_Close_Visitor)
                    writer.Byte(1)
                    Server.Send(writer.GetBytes, Index_)

                    PlayerData(Index_).Busy = False
                    PlayerData(Index_).StallID = 0
                    PlayerData(Index_).InStall = False
                    PlayerData(Index_).StallOwner = False
                End If
            Next
        End Sub


        Public Sub Stall_SendItemsOwn(ByVal Stall_Index As Integer, ByVal Index_ As Integer, ByVal Mode As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Stall_Data)
            writer.Byte(1)
            writer.Byte(Mode)
            writer.Word(0)

            For i = 0 To Stalls(Stall_Index).Items.Count - 1
                If Stalls(Stall_Index).Items(i).Slot <> 0 Then
                    Dim item As cInvItem = Inventorys(Index_).UserItems(Stalls(Stall_Index).Items(i).Slot)
                    Dim refitem As cItem = GetItemByID(item.Pk2Id)
                    writer.Byte(i) 'slot
                    AddItemDataToPacket(item, writer)
                    writer.Byte(Stalls(Stall_Index).Items(i).Slot)
                    If refitem.CLASS_A = 1 Then
                        writer.Word(1)
                    Else
                        writer.Word(item.Amount)
                    End If
                    writer.QWord(Stalls(Stall_Index).Items(i).Gold)
                End If
            Next

            writer.Byte(&HFF)
            Server.Send(writer.GetBytes, Index_)
        End Sub


        Public Function Stall_SendItemsOther(ByVal Stall_Index As Integer, ByVal Index_ As Integer) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Stall_Items)
            writer.Byte(1)
            writer.DWord(Stalls(Stall_Index).OwnerID)
            writer.Word(Stalls(Stall_Index).WelcomeMessage.Length)
            writer.UString(Stalls(Stall_Index).WelcomeMessage)
            writer.Byte(Stalls(Stall_Index).Open)
            writer.Byte(0)

            For i = 0 To Stalls(Stall_Index).Items.Count - 1
                If Stalls(Stall_Index).Items(i).Slot <> 0 Then
                    Dim Slot As Byte = Stalls(Stall_Index).Items(i).Slot
                    Dim item As cInvItem = Inventorys(Stalls(Stall_Index).OwnerIndex).UserItems(Slot)
                    Dim refitem As cItem = GetItemByID(item.Pk2Id)
                    writer.Byte(i) 'slot
                    AddItemDataToPacket(item, writer)
                    writer.Byte(Stalls(Stall_Index).Items(i).Slot)
                    If refitem.CLASS_A = 1 Then
                        writer.Word(1)
                    Else
                        writer.Word(item.Amount)
                    End If
                    writer.QWord(Stalls(Stall_Index).Items(i).Gold)
                End If
            Next

            writer.Byte(&HFF)
            writer.Byte(0)
            Return writer.GetBytes
        End Function

        Public Sub LinkPlayerToStall(ByVal FromIndex As Integer, ByVal ToIndex As Integer)
            If PlayerData(FromIndex).InStall = True And PlayerData(FromIndex).StallID <> 0 Then
                Dim Index As Integer = GetStallIndex(PlayerData(FromIndex).StallID)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Stall_Open_ToOther)
                writer.DWord(PlayerData(FromIndex).UniqueId)
                writer.Word(Stalls(Index).StallName.Length)
                writer.UString(Stalls(Index).StallName)
                writer.DWord(0)
                Server.Send(writer.GetBytes, ToIndex)
            End If
        End Sub


        Private Function GetStallId() As UInt32
            StallIdCounter += 1
            Return StallIdCounter
        End Function


        Private Function GetStallIndex(ByVal StallId As UInt32) As Integer
            For i = 0 To Stalls.Count - 1
                If Stalls(i).StallID = StallId Then
                    Return i
                End If
            Next
            Return -1
        End Function

    End Module
End Namespace
