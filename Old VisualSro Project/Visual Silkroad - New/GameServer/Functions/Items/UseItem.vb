Imports System.Text
Imports SRFramework

Namespace Functions
    Module UseItem
        Public Sub OnUseItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim ID1 As Byte = packet.Byte
            Dim ID2 As Byte = packet.Byte

            Debug.Print("[USE_ITEM][ID1:" & Hex(ID1) & "][2:" & ID2 & "]")

            If ID1 = &HEC Then
                Select Case ID2
                    Case 8 'HP-Pot
                        OnUseHPPot(slot, Index_)
                    Case 9 'Return Scroll
                        OnUseReturnScroll(slot, Index_)
                    Case 14 'Speed Drug
                    Case 16 'MP-Pot 
                        OnUseMPPot(slot, Index_)
                    Case 24 'gngwc 100% scrol
                    Case 64 'Berserk Scroll
                        OnUseBerserkScroll(slot, Index_)
                End Select

            ElseIf ID1 = &HED Then
                Select Case ID2
                    Case 8
                        OnUseHPPot(slot, Index_)
                    Case 9 'Return Scroll
                        OnUseReturnScroll(slot, Index_)
                    Case 14 'Use Ballon
                    Case 16 'MP-Pot 
                        OnUseMPPot(slot, Index_)
                    Case 25 'Reverse
                        OnUseReverseScroll(slot, Index_, packet)
                    Case 33 'Add Stall Decoration
                    Case 41 'Globals
                        OnUseGlobal(slot, Index_, packet)
                    Case 70 'Gender Switch Scroll
                    Case 78 'Skin Change Scroll
                        OnUseSkinScroll(slot, Index_, packet)
                    Case 118 'Premium
                End Select
            End If
        End Sub

        Public Sub OnUseHPPot(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)

            If invItem.ItemID <> 0 And PlayerData(Index_).Alive Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 1 And refitem.CLASS_C = 1 Then
                    If PlayerData(Index_).CHP + refitem.USE_TIME_HP >= PlayerData(Index_).HP Then
                        PlayerData(Index_).CHP = PlayerData(Index_).HP
                    Else
                        PlayerData(Index_).CHP += refitem.USE_TIME_HP
                    End If

                    UpdateAmout(Index_, Slot, -1)
                    UpdateHP(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(item.Data)
                    writer.Byte(&HEC)
                    writer.Byte(&H8)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)
                End If
            End If
        End Sub

        Public Sub OnUseMPPot(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)

            If invItem.ItemID <> 0 And PlayerData(Index_).Alive Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 1 And refitem.CLASS_C = 2 Then
                    If PlayerData(Index_).CMP + refitem.USE_TIME_MP >= PlayerData(Index_).MP Then
                        PlayerData(Index_).CMP = PlayerData(Index_).MP
                    Else
                        PlayerData(Index_).CMP += refitem.USE_TIME_MP
                    End If

                    UpdateAmout(Index_, Slot, -1)
                    UpdateMP(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(item.Data)
                    writer.Byte(&HEC)
                    writer.Byte(&H10)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)
                End If
            End If
        End Sub

        Public Sub OnUseReverseScroll(ByVal Slot As Byte, ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim tag As Byte = packet.Byte '2 = recall, 3= dead point, 7 = special point

            Dim writer As New PacketWriter

            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                writer.Create(ServerOpcodes.GAME_ITEM_USE)
                writer.Byte(2) 'error
                writer.Byte(&H5D)    'already teleporting
                Server.Send(writer.GetBytes, Index_)
                Exit Sub
            End If

            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)

            If invItem.ItemID <> 0 And PlayerData(Index_).Alive Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 3 Then 'Check for right Item
                    UpdateAmout(Index_, Slot, -1)
                    UpdateState(&HB, 1, Index_)

                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(item.Data)
                    writer.Byte(&HED)
                    writer.Byte(&H19)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)

                    UsingItemTimer(Index_).Interval = refitem.USE_TIME_HP
                    UsingItemTimer(Index_).Start()

                    Select Case tag
                        Case 2
                            PlayerData(Index_).UsedItem = UseItemTypes.Reverse_Scroll_Recall
                        Case 3
                            PlayerData(Index_).UsedItem = UseItemTypes.Reverse_Scroll_Dead
                        Case 7
                            PlayerData(Index_).UsedItem = UseItemTypes.Reverse_Scroll_Point
                            PlayerData(Index_).UsedItemParameter = packet.DWord
                    End Select

                    PlayerData(Index_).Busy = True
                End If
            End If
        End Sub

        Public Sub OnUseReturnScroll(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)
            Dim writer As New PacketWriter

            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                writer.Create(ServerOpcodes.GAME_ITEM_USE)
                writer.Byte(2)
                'error
                writer.Byte(&H5D)
                'already teleporting
                Server.Send(writer.GetBytes, Index_)
                Exit Sub
            End If

            If invItem.ItemID <> 0 And PlayerData(Index_).Alive Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 1 Then
                    UpdateAmout(Index_, Slot, -1)
                    UpdateState(&HB, 1, Index_)

                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(item.Data)
                    writer.Byte(&HEC)
                    writer.Byte(&H9)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)

                    UsingItemTimer(Index_).Interval = refitem.USE_TIME_HP
                    UsingItemTimer(Index_).Start()
                    PlayerData(Index_).UsedItem = UseItemTypes.Return_Scroll
                    PlayerData(Index_).Busy = True
                End If
            End If
        End Sub

        Public Sub OnUseGlobal(ByVal Slot As Byte, ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim messagelength As UInt16 = packet.Word
            Dim bmessage As Byte() = packet.ByteArray(messagelength * 2)
            Dim message As String = Encoding.Unicode.GetString(bmessage)

            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)
            Dim writer As New PacketWriter


            If invItem.ItemID <> 0 Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 5 Then
                    UpdateAmout(Index_, Slot, -1)

                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(item.Data)
                    writer.Byte(&HED)
                    writer.Byte(&H29)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)

                    OnGlobalChat(message, Index_)
                Else
                    Server.Disconnect(Index_)
                End If
            End If
        End Sub

        Public Sub OnUseSkinScroll(ByVal Slot As Byte, ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim NewModel As UInteger = packet.DWord
            Dim NewVolume As Byte = packet.Byte

            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)
            Dim writer As New PacketWriter


            If invItem.ItemID <> 0 Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 13 And refitem.CLASS_C = 9 Then

                    '================Checks
                    Dim Fail As Boolean = False
                    For I = 0 To 12
                        If Inventorys(Index_).UserItems(I).ItemID <> 0 Then
                            Fail = True
                        End If
                    Next

                    If IsCharChinese(NewModel) = Fail And IsCharEurope(NewModel) = False Then
                        'Wrong Model Code! 
                        Fail = True
                    End If

                    If Fail = True Then
                        UseItemError(Index_, &H92)
                        Exit Sub
                    End If

                    '==============Using..
                    UpdateAmout(Index_, Slot, -1)


                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(item.Data)
                    writer.Byte(&HED)
                    writer.Byte(&H29)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)

                    PlayerData(Index_).Pk2ID = NewModel
                    PlayerData(Index_).Volume = NewVolume

                    GameDB.SaveCharTypeAndVolume(Index_)
                    OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                End If
            End If
        End Sub


        Public Sub OnUseBerserkScroll(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)

            If invItem.ItemID <> 0 Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 1 And refitem.CLASS_C = 8 Then
                    UpdateAmout(Index_, Slot, -1)

                    PlayerData(Index_).BerserkBar = 5
                    UpdateBerserk(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(item.Data)
                    writer.Byte(&HEC)
                    writer.Byte(&H10)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)
                End If
            End If
        End Sub

        Public Sub UseItemError(ByVal Index_ As Integer, ByVal ErrorByte As Byte)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_USE)
            writer.Byte(2)
            writer.Byte(ErrorByte)
            Server.Send(writer.GetBytes, Index_)
        End Sub


        Public Sub OnReturnScroll_Cancel(ByVal Index_ As Integer)
            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                PlayerData(Index_).UsedItem = UseItemTypes.None
                UsingItemTimer(Index_).Stop()
                UpdateState(&HB, 0, Index_)
                PlayerData(Index_).Busy = False
            End If
        End Sub

        Public Sub ShowOtherPlayerItemUse(ByVal ItemID As Integer, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_USE_OTHERPLAYER)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.DWord(ItemID)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub


        ''' <summary>
        ''' Changing the Item Amout
        ''' </summary>
        ''' <param name="Index_"></param>
        ''' <param name="Slot"></param>
        ''' <param name="ToAdd"></param>
        ''' <returns>New Item amout</returns>
        ''' <remarks></remarks>
        Public Sub UpdateAmout(ByVal Index_ As Integer, ByVal Slot As Byte, ByVal ToAdd As Integer)
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)
            Dim item As cItem = GameDB.Items(invItem.ItemID)
            If item.Data + ToAdd <= 0 Then
                Inventorys(Index_).UserItems(Slot).ItemID = 0
                ItemManager.RemoveItem(item.ID)
            ElseIf item.Data + ToAdd > 0 Then
                item.Data += ToAdd
                ItemManager.UpdateItem(item)
            End If
        End Sub

        ''' <summary>
        ''' Checks that the Equip Slots are empty
        ''' </summary>
        ''' <param name="Index_"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function EquipSlotsEmpty(ByVal Index_ As Integer) As Boolean
            For i = 0 To 13
                If Inventorys(Index_).UserItems(i).ItemID <> 0 Then
                    Return False
                End If
            Next
            Return True
        End Function
    End Module
End Namespace
