Imports SRFramework

Namespace Functions
    Module UseItemMall
        Public Sub OnUseGlobal(ByVal slot As Byte, ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim messagelength As UInt16 = packet.Word
            Dim bmessage As Byte() = packet.ByteArray(messagelength * 2)
            Dim message As String = Text.Encoding.Unicode.GetString(bmessage)

            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(slot)
    

            If invItem.ItemID <> 0 Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 3 And refitem.CLASS_C = 5 Then
                    UpdateItemAmout(Index_, slot, -1)


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
                    OnGlobalChat(message, Index_)
                Else
                    Server.Disconnect(Index_)
                End If
            End If
        End Sub

        Public Sub OnUseSkinScroll(ByVal slot As Byte, ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim newModel As UInteger = packet.DWord
            Dim newVolume As Byte = packet.Byte

            Dim invItem As cInventoryItem = Inventorys(Index_).UserItems(slot)

            If invItem.ItemID <> 0 Then
                Dim item As cItem = GameDB.Items(invItem.ItemID)
                Dim refitem As cRefItem = GetItemByID(item.ObjectID)

                If refitem.CLASS_A = 3 And refitem.CLASS_B = 13 And refitem.CLASS_C = 9 Then

                    '================Checks
                    Dim fail As Boolean = False
                    For I = 0 To 12
                        If Inventorys(Index_).UserItems(I).ItemID <> 0 Then
                            fail = True
                        End If
                    Next

                    If IsCharChinese(newModel) = fail And IsCharEurope(newModel) = False Then
                        'Wrong Model Code! 
                        fail = True
                    End If

                    If fail = True Then
                        UseItemError(Index_, &H92)
                        Exit Sub
                    End If

                    '==============Using..
                    UpdateItemAmout(Index_, slot, -1)


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

                    PlayerData(Index_).Pk2ID = newModel
                    PlayerData(Index_).Volume = newVolume
                    GameDB.SaveCharTypeAndVolume(Index_)

                    OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                End If
            End If
        End Sub

    End Module
End Namespace