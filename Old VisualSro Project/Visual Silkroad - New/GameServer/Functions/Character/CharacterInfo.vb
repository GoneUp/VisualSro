Imports SRFramework

Namespace Functions
    Module CharacterInfo
        Public Sub SendCharacterIngame(ByVal Index_ As Integer)
            OnCharacterIntialize(Index_)

            OnSendLoadStart(Index_)
            OnCharacterInfo(Index_)
            OnSendLoadEnd(Index_)

            OnSendIngameInfo(Index_)
        End Sub

        Public Sub OnCharacterIntialize(ByVal Index_ As Integer)
            'Prepare
            CleanUpPlayerComplete(Index_)
            PlayerCheckDeath(Index_, True)
            GameMod.Damage.OnPlayerLogon(Index_)

            'Stats
            PlayerData(Index_).SetCharGroundStats()
            PlayerData(Index_).AddItemsToStats(Index_)
            
            'Instance Stuff
            PlayerData(Index_).ChannelId = Settings.ServerWorldChannel
        End Sub

        Public Sub OnSendLoadStart(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_LOADING_START)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnSendLoadEnd(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_LOADING_END)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnSendIngameInfo(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_CHARACTER_ID)
            writer.DWord(PlayerData(Index_).UniqueID) 'charid
            writer.Word(Date.Now.Day) 'moon pos
            writer.Byte(Date.Now.Hour) 'hours
            writer.Byte(Date.Now.Minute) 'minute
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnCharacterInfo(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            Dim chari As cCharacter = PlayerData(Index_)
            writer = New PacketWriter
            writer.Create(ServerOpcodes.GAME_CHARACTER_INFO)
            writer.DWord(2289569290)  '@@@@@@@@@@@@@@@@@
            writer.DWord(chari.Pk2ID)      ' Character Model
            writer.Byte(chari.Volume)    ' Volume & Height
            writer.Byte(chari.Level)
            writer.Byte(chari.Level) ' Highest Level

            writer.QWord(chari.Experience)
            writer.DWord(chari.SkillPointBar)
            writer.QWord(chari.Gold)
            writer.DWord(chari.SkillPoints)
            writer.Word(chari.Attributes)
            writer.Byte(chari.BerserkBar)
            writer.DWord(0)
            writer.DWord(chari.CHP)
            writer.DWord(chari.CMP)
            writer.Byte(chari.HelperIcon)
            writer.Byte(0)   ' Daily PK (/15)
            writer.Word(0)   ' Total PK
            writer.DWord(0)   ' PK Penalty Point
            writer.Byte(0) ' Rank
            writer.Byte(0) ' Unknown
            
            'INVENTORY
            Inventorys(Index_).CalculateItemCount()
            writer.Byte(chari.MaxInvSlots)       ' Max Item Slot (0 Minimum + 13) (4 Seiten x 24 Slots = 96 Maximum + 13 --> 109)
            writer.Byte(Inventorys(Index_).ItemCount)         ' Amount of Items  

            For Each item As cInventoryItem In Inventorys(Index_).UserItems
                If item.ItemID <> 0 Then
                    writer.Byte(item.Slot)

                    AddItemDataToPacket(GameDB.Items(item.ItemID), writer)
                End If
            Next

            'Avatar Inventory
            Inventorys(Index_).CalculateAvatarCount()
            writer.Byte(5)  ' Avatar Item Max
            writer.Byte(Inventorys(Index_).AvatarCount)    ' Amount of Avatars

            For Each avatar As cInventoryItem In Inventorys(Index_).AvatarItems
                If avatar.ItemID <> 0 Then
                    Dim item As cItem = GameDB.Items(avatar.ItemID)
                    Dim refitem As cRefItem = GetItemByID(item.ObjectID)
                    writer.Byte(GetExternalAvatarSlot(refitem))
                    AddItemDataToPacket(item, writer)
                End If
            Next

            writer.Byte(0)  ' Duplicate List (00 - None) (01 - Duplicate)
            writer.Word(11) 'Unknown
            writer.Byte(0)  'Unknown

            'Mastery List
            For i = 0 To GameDB.Masterys.Length - 1
                If (GameDB.Masterys(i) IsNot Nothing) AndAlso GameDB.Masterys(i).OwnerID = chari.CharacterId Then
                    writer.Byte(1) 'Mastery start
                    writer.DWord(GameDB.Masterys(i).MasteryID)
                    writer.Byte(GameDB.Masterys(i).Level)
                End If
            Next

            writer.Byte(2)     'Mastery list end
            writer.Byte(0)     'Skill list start


            For i = 0 To GameDB.Skills.Length - 1
                If (GameDB.Skills(i) IsNot Nothing) AndAlso GameDB.Skills(i).OwnerID = chari.CharacterId Then
                    writer.Byte(1)    'skill start
                    writer.DWord(GameDB.Skills(i).SkillID)    'skill id
                    writer.Byte(1)   ' skill end
                End If
            Next

            writer.Byte(2)  'Skill List End



            writer.Word(0) ' Amount of Completed Quests
            'writer.DWord(1) 'event

            writer.Word(0) ' Amount of Pending Quests



            ' ID, Position, State, Speed
            writer.DWord(0)
            writer.DWord(chari.UniqueID)
            writer.Byte(chari.Position.XSector)
            writer.Byte(chari.Position.YSector)
            writer.Float(chari.Position.X)
            writer.Float(chari.Position.Z)
            writer.Float(chari.Position.Y)
            writer.Word(chari.Angle)
            writer.Byte(0)    ' Destination
            writer.Byte(1) ' Walk & Run Flag
            writer.Byte(0)   ' No Destination
            writer.Word(chari.Angle)
            writer.Byte(0)
            writer.Byte(0)
            writer.Byte(0)
            writer.Byte(0)
            writer.Byte(chari.Berserk)
            writer.Float(chari.WalkSpeed)
            writer.Float(chari.RunSpeed)
            writer.Float(chari.BerserkSpeed)


            writer.Byte(0) ' Buff Flag

            writer.Word(chari.CharacterName.Length)
            writer.String(chari.CharacterName)
            writer.Word(chari.AliasName.Length)
            writer.String(chari.AliasName)
            writer.Word(0)
            writer.Byte(0)            '0=not selected, 1 = hunter
            writer.Byte(0)
            writer.QWord(0)
            writer.DWord(0)
            writer.Byte(&HFF)    'PVP Flag


            'Account
            writer.Word(&HD3)
            writer.Word(&HA0)
            writer.DWord(0)
            writer.DWord(chari.AccountID)
            writer.Byte(chari.GM)
            writer.Byte(7)


            Dim hotkeycount As UInteger = 0
            For i = 0 To GameDB.Hotkeys.Count - 1
                If GameDB.Hotkeys(i).OwnerID = chari.CharacterId Then
                    If GameDB.Hotkeys(i).Type <> 0 And GameDB.Hotkeys(i).IconID <> 0 Then
                        hotkeycount += 1
                    End If
                End If
            Next

            writer.Byte(hotkeycount)

            For i = 0 To GameDB.Hotkeys.Count - 1
                If GameDB.Hotkeys(i).OwnerID = chari.CharacterId Then
                    If GameDB.Hotkeys(i).Type <> 0 And GameDB.Hotkeys(i).IconID <> 0 Then
                        writer.Byte(GameDB.Hotkeys(i).Slot)
                        writer.Byte(GameDB.Hotkeys(i).Type)
                        writer.DWord(GameDB.Hotkeys(i).IconID)
                    End If
                End If
            Next


            ' Autopotion
            writer.Word(chari.PotHp)
            writer.Word(chari.PotMp)
            writer.Word(chari.PotAbormal)
            writer.Byte(chari.PotDelay)


            writer.Byte(0)  ' Amount of Players Blocked
            writer.Byte(0)  'Other Block Shit (Trade or PTM)


            ' Other
            writer.DWord(0)
            writer.Word(0)
            writer.Byte(0)
            writer.Word(1)
            writer.Word(1)
            writer.Byte(0)
            writer.Byte(1)

            Server.Send(writer.GetBytes, Index_)
        End Sub
    End Module
End Namespace
