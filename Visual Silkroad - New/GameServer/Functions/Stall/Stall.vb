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
                Case 2
                    'Additem
                    Stall_AddItem(packet, Index_)
                Case 3
                    'REmove
                    Stall_RemoveItem(packet, Index_)
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


                    Stall_SendItemsOwn(index, Index_)
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

                    Stall_SendItemsOwn(index, Index_)
                    Server.SendToStallSession(Stall_SendItemsOther(index, index), PlayerData(Index_).StallID, False)
                End If
            End If
        End Sub

        Public Sub ChangeState(ByVal packet As PacketReader, ByVal Index_ As Integer)
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

        Public Sub Stall_Open_Other(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ToUniqueID As UInt32 = packet.DWord

            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).UniqueId = ToUniqueID Then
                        Dim writer As New PacketWriter
                        writer.Create(ServerOpcodes.Stall_Join)
                        writer.Byte(2)
                        writer.DWord(PlayerData(Index_).UniqueId)
                        Server.SendToStallSession(writer.GetBytes, PlayerData(i).StallID, True)

                        Dim Stall_index As Integer = GetStallIndex(PlayerData(i).StallID)
                        Stalls(Stall_index).Visitors.Add(Index_)
                        PlayerData(Index_).Busy = True


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
            End If
        End Sub


        Public Sub Stall_Close_Other(ByVal Index_ As Integer)
            For i = 0 To Stalls.Count - 1
                If Stalls(i).Visitors.Contains(Index_) Then
                    Stalls(i).Visitors.Remove(Index_)
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Stall_Join)
                    writer.Byte(1)
                    writer.DWord(PlayerData(Index_).UniqueId)
                    Server.SendToStallSession(writer.GetBytes, PlayerData(i).StallID, True)

                    writer.Create(ServerOpcodes.Stall_Close_Visitor)
                    writer.Byte(1)
                    Server.Send(writer.GetBytes, Index_)
                End If
            Next
        End Sub


        Public Sub Stall_SendItemsOwn(ByVal Stall_Index As Integer, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Stall_Data)
            writer.Byte(1)
            writer.Byte(2)
            writer.Byte(0)

            For i = 0 To Stalls(Stall_Index).Items.Count - 1
                If Stalls(Stall_Index).Items(i).Slot <> 0 Then
                    Dim item As cInvItem = Inventorys(Index_).UserItems(Stalls(Stall_Index).Items(i).Slot)
                    Dim refitem As cItem = GetItemByID(item.Pk2Id)
                    writer.Byte(0)
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
                    writer.Byte(0)
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
