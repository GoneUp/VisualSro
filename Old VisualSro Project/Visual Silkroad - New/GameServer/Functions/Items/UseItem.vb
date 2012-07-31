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
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)

            If _item.Pk2Id <> 0 And PlayerData(Index_).Alive Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)

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
                    writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
                    writer.Byte(&HEC)
                    writer.Byte(&H8)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)
                End If
            End If
        End Sub

        Public Sub OnUseMPPot(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)

            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)

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
                    writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
                    writer.Byte(&HEC)
                    writer.Byte(&H10)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)
                End If
            End If
        End Sub

        Public Sub OnUseReverseScroll(ByVal Slot As Byte, ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)
            Dim writer As New PacketWriter
            Dim tag As Byte = packet.Byte
            '2 = recall 3= dead point
            Dim id As UInteger = 0

            If PlayerData(Index_).UsedItem <> UseItemTypes.None Then
                writer.Create(ServerOpcodes.GAME_ITEM_USE)
                writer.Byte(2)
                'error
                writer.Byte(&H5D)
                'already teleporting
                Server.Send(writer.GetBytes, Index_)
                Exit Sub
            End If

            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)
                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 3 Then 'Check for right Item
                    UpdateAmout(Index_, Slot, -1)
                    UpdateState(&HB, 1, Index_)

                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
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
                            id = packet.DWord
                            PlayerData(Index_).UsedItem = UseItemTypes.Reverse_Scroll_Point
                            PlayerData(Index_).UsedItemParameter = id
                    End Select

                    PlayerData(Index_).Busy = True
                End If
            End If
        End Sub

        Public Sub OnUseReturnScroll(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)
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

            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 1 Then
                    UpdateAmout(Index_, Slot, -1)
                    UpdateState(&HB, 1, Index_)

                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
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

            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)
            Dim writer As New PacketWriter


            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 5 Then
                    UpdateAmout(Index_, Slot, -1)

                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
                    writer.Byte(&HED)
                    writer.Byte(&H29)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)

                    OnGlobalChat(message, Index_)
                End If
            End If
        End Sub

        Public Sub OnUseSkinScroll(ByVal Slot As Byte, ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim NewModel As UInteger = packet.DWord
            Dim NewVolume As Byte = packet.Byte

            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)
            Dim writer As New PacketWriter


            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)
                If refitem.CLASS_A = 3 And refitem.CLASS_B = 13 And refitem.CLASS_C = 9 Then

                    '================Checks
                    Dim Fail As Boolean = False
                    For I = 0 To 12
                        If Inventorys(Index_).UserItems(I).Pk2Id <> 0 Then
                            Fail = True
                        End If
                    Next

                    If NewModel >= 1907 And NewModel <= 1932 = False And NewModel >= 14717 And NewModel <= 14743 = False _
                        Then
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
                    writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
                    writer.Byte(&HED)
                    writer.Byte(&H29)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)


                    PlayerData(Index_).Pk2ID = NewModel
                    PlayerData(Index_).Volume = NewVolume

                    Database.SaveQuery(String.Format("UPDATE characters SET chartype='{0}', volume='{1}' where id='{2}'",
                                                     PlayerData(Index_).Pk2ID, PlayerData(Index_).Volume,
                                                     PlayerData(Index_).CharacterId))
                    OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                End If
            End If
        End Sub


        Public Sub OnUseBerserkScroll(ByVal Slot As Byte, ByVal Index_ As Integer)
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)

            If _item.Pk2Id <> 0 Then
                Dim refitem As cItem = GetItemByID(_item.Pk2Id)
                If refitem.CLASS_A = 3 And refitem.CLASS_B = 1 And refitem.CLASS_C = 8 Then
                    UpdateAmout(Index_, Slot, -1)

                    PlayerData(Index_).BerserkBar = 5
                    UpdateBerserk(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(Inventorys(Index_).UserItems(Slot).Amount)
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
        Public Function UpdateAmout(ByVal Index_ As Integer, ByVal Slot As Byte, ByVal ToAdd As Integer) As UShort
            Dim _item As cInvItem = Inventorys(Index_).UserItems(Slot)
            If _item.Amount + ToAdd = 0 Then
                'Despawn Item

                _item.Pk2Id = 0
                _item.Durability = 0
                _item.Plus = 0
                _item.Amount = 0

                Inventorys(Index_).UserItems(Slot) = _item
                DeleteItemFromDB(Slot, Index_)
            ElseIf Inventorys(Index_).UserItems(Slot).Amount + ToAdd > 0 Then
                _item.Amount += ToAdd
                Inventorys(Index_).UserItems(Slot) = _item
                UpdateItem(_item)
            End If
        End Function

        ''' <summary>
        ''' Checks that the Equip Slots are empty
        ''' </summary>
        ''' <param name="Index_"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function EquipSlotsEmpty(ByVal Index_ As Integer)
            For i = 0 To 13
                If Inventorys(Index_).UserItems(i).Pk2Id <> 0 Then
                    Return False
                End If
            Next
            Return True
        End Function
    End Module
End Namespace
