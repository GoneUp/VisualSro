Imports System.Text
Imports SRFramework

Namespace Functions
    Module UseItemHandler
        Public Sub OnUseItem(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim slot As Byte = packet.Byte
            Dim unknown As UInt16 = packet.Word()
            Dim tid2 As Byte = packet.Byte()
            Dim tid3 As Byte = packet.Byte()

            Debug.Print("[USE_ITEM][TID2:" & tid2 & "][TID3:" & tid3 & "]")


            Select Case tid2
                Case 1
                    'Potions
                    Select Case tid3
                        Case 1
                            'HP
                            OnUseHPPot(slot, Index_)
                        Case 2
                            'MP
                            OnUseMPPot(slot, Index_)
                        Case 3
                            'All
                            OnUseVigorPot(slot, Index_)
                        Case 4
                            'Pet Heal
                        Case 6
                            'Pet Revival
                        Case 8
                            'HWAN
                            OnUseBerserkScroll(slot, Index_)
                        Case 9
                            'HGP Portion
                        Case 10
                            'Repait Kit
                    End Select

                Case 2
                    'Pills
                    Select Case tid3
                        Case 1
                            'Purification
                        Case 6
                            'Universal Pill
                        Case 7
                            'Abnormal State
                    End Select
                Case 3
                    'Scrolls
                    Select Case tid3
                        Case 1
                            'return
                            OnUseReturnScroll(slot, Index_)
                        Case 2
                            'summon scrool
                        Case 3
                            'reverse
                            OnUseReverseScroll(slot, Index_, packet)
                        Case 4
                            'stall avatar
                        Case 5
                            'global chat
                            OnUseGlobal(slot, Index_, packet)
                    End Select
                Case 6
                    'Firework
                Case 7
                    'Campfire!!!!
                    If tid3 = 1 Then

                    End If
                Case 8
                    'Special Item
                Case 12
                    'Guild Item Scroll
                Case 13
                    'Item Mall
                    Select Case tid3
                        Case 9
                            'Skin Scroll
                            OnUseSkinScroll(slot, Index_, packet)
                    End Select
                Case 15
                    'Event Item
            End Select


        End Sub

        Public Sub OnUseHPPot(ByVal slot As Byte, ByVal Index_ As Integer)
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

                    UpdateItemAmout(Index_, Slot, -1)
                    UpdateHP(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(Slot)
                    writer.Word(item.Data)
                    writer.Word(&HC30)
                    writer.Byte(refitem.CLASS_B)
                    writer.Byte(refitem.CLASS_C)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)
                End If
            End If
        End Sub

        Public Sub OnUseMPPot(ByVal slot As Byte, ByVal Index_ As Integer)
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(Slot)

            If invItem.ItemID <> 0 And PlayerData(Index_).Alive Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 1 And refitem.CLASS_C = 2 Then
                    If PlayerData(Index_).CHP + refitem.USE_TIME_HP >= PlayerData(Index_).HP Then
                        PlayerData(Index_).CHP = PlayerData(Index_).HP
                    Else
                        PlayerData(Index_).CHP += refitem.USE_TIME_HP
                    End If

                    If PlayerData(Index_).CMP + refitem.USE_TIME_MP >= PlayerData(Index_).MP Then
                        PlayerData(Index_).CMP = PlayerData(Index_).MP
                    Else
                        PlayerData(Index_).CMP += refitem.USE_TIME_MP
                    End If

                    UpdateItemAmout(Index_, Slot, -1)
                    UpdateHPMP(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(slot)
                    writer.Word(item.Data)
                    writer.Word(&HC30)
                    writer.Byte(refitem.CLASS_B)
                    writer.Byte(refitem.CLASS_C)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)
                End If
            End If
        End Sub

        Public Sub OnUseVigorPot(ByVal slot As Byte, ByVal Index_ As Integer)
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(slot)

            If invItem.ItemID <> 0 And PlayerData(Index_).Alive Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 1 And refitem.CLASS_C = 3 Then
                    If PlayerData(Index_).CMP + refitem.USE_TIME_MP >= PlayerData(Index_).MP Then
                        PlayerData(Index_).CMP = PlayerData(Index_).MP
                    Else
                        PlayerData(Index_).CMP += refitem.USE_TIME_MP
                    End If


                    UpdateItemAmout(Index_, slot, -1)
                    UpdateMP(Index_)

                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.GAME_ITEM_USE)
                    writer.Byte(1)
                    writer.Byte(slot)
                    writer.Word(item.Data)
                    writer.Word(&HC30)
                    writer.Byte(refitem.CLASS_B)
                    writer.Byte(refitem.CLASS_C)
                    Server.Send(writer.GetBytes, Index_)

                    ShowOtherPlayerItemUse(refitem.Pk2Id, Index_)
                End If
            End If
        End Sub

        Public Sub UseItemError(ByVal errorByte As Byte, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_USE)
            writer.Byte(2)
            writer.Byte(errorByte)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub ShowOtherPlayerItemUse(ByVal itemID As UInteger, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_ITEM_USE_OTHERPLAYER)
            writer.DWord(PlayerData(Index_).UniqueID)
            writer.DWord(itemID)
            Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)
        End Sub


        ''' <summary>
        ''' Changing the Item Amout
        ''' </summary>
        ''' <param name="Index_"></param>
        ''' <param name="slot"></param>
        ''' <param name="toAdd"></param>
        ''' <returns>New Item amout</returns>
        ''' <remarks></remarks>
        Public Function UpdateItemAmout(ByVal Index_ As Integer, ByVal slot As Byte, ByVal toAdd As Integer) As UInt16
            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(slot)
            Dim item As cItem = GameDB.Items(invItem.ItemID)
            If item.Data + toAdd <= 0 Then
                Inventorys(Index_).UserItems(slot).ItemID = 0
                ItemManager.UpdateInvItem(Inventorys(Index_).UserItems(slot), InvItemTypes.Inventory)
                ItemManager.RemoveItem(item.ID)
            ElseIf item.Data + toAdd > 0 Then
                item.Data += toAdd
                ItemManager.UpdateItem(item)
            End If

            Return item.Data
        End Function
    End Module
End Namespace
