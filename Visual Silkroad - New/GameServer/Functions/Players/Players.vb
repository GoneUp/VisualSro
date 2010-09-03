Namespace GameServer.Functions
    Module Players

        'Players
        Public PlayerData(5000) As [cChar]
        'items
        Public Inventorys(5000) As cInventory
        'Mobs
        'Skills

        Public Sub OnPlayerMovement(ByVal Index_ As Integer, ByVal packet As PacketReader)

            Dim tag As Byte = packet.Byte
            If tag = 1 Then
                '01 A8 60 61 02 FE FF 70 04
                Dim ToXSector As Byte = packet.Byte
                Dim ToYSector As Byte = packet.Byte
                Dim ToXPos As Integer = CInt(packet.Word)
                Dim ToZPos As Single = (packet.Word)
                Dim ToYPos As Integer = CInt(packet.Word)
                Debug.Print(ToXSector & " -- " & ToYSector & " -- " & ToXPos & " -- " & ToZPos & " -- " & ToYPos)
                Debug.Print("X:" & ((ToXSector - 135) * 192) + (ToXPos / 10) & " Y:" & ((ToYSector - 92) * 192) + (ToYPos / 10))


                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.Movement)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Byte(1) 'destination
                writer.Byte(ToXSector)
                writer.Byte(ToYSector)
                writer.Word(CUInt(ToXPos))
                writer.Word((ToZPos))
                writer.Word(CUInt(ToYPos))
                writer.Byte(0) '1= source
                'writer.Byte(PlayerData(Index_).XSector)
                'writer.Byte(PlayerData(Index_).YSector)
                'writer.Word((PlayerData(Index_).X))
                'writer.DWord((PlayerData(Index_).Z))
                'writer.Word((PlayerData(Index_).Y))

                PlayerData(Index_).XSector = ToXSector
                PlayerData(Index_).YSector = ToYSector
                PlayerData(Index_).X = ToXPos
                PlayerData(Index_).Z = ToZPos
                PlayerData(Index_).Y = ToYPos

                DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).XSector, PlayerData(Index_).YSector, PlayerData(Index_).X, PlayerData(Index_).Z, PlayerData(Index_).Y, PlayerData(Index_).UniqueId))

                SpawnMe(Index_)
                SpawnOtherPlayer(Index_) 'Spawn before sending the Packet is to prevent chrashes
                DespawnPlayerRange(Index_)
                Server.SendToAllInRange(writer.GetBytes, Index_)
            Else

            End If
        End Sub
        ''' <summary>
        ''' For Players
        ''' </summary>
        ''' <param name="Index"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateSpawnPacket(ByVal Index As Integer) As Byte()

            Dim chari As [cChar] = PlayerData(Index) 'Only for faster Code writing

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleSpawn)
            writer.DWord(chari.Model)
            writer.Byte(chari.Volume)
            writer.Byte(0)
            writer.Byte(0)

            'items
            Dim inventory As New cInventory(chari.MaxSlots)
            inventory = GameServer.DatabaseCore.FillInventory(chari)

            Dim PlayerItemCount As Integer = 0
            For b = 0 To 9
                If inventory.UserItems(b).Pk2Id <> 0 And inventory.UserItems(b).Pk2Id <> 62 Then
                    PlayerItemCount += 1
                End If
            Next

            writer.Byte(chari.MaxSlots)
            writer.Byte(PlayerItemCount)

            For b = 0 To 9
                If inventory.UserItems(b).Pk2Id <> 0 Then
                    If inventory.UserItems(b).Pk2Id <> 62 Then 'Dont send arrows
                        writer.DWord(inventory.UserItems(b).Pk2Id)
                        writer.Byte(inventory.UserItems(b).Plus)
                    End If

                End If
            Next

            writer.Byte(4) 'avatar start
            writer.Byte(0) '0 avatars fro now
            writer.Byte(0)

            writer.DWord(chari.UniqueId)
            writer.Byte(chari.XSector)
            writer.Byte(chari.YSector)
            writer.Float(chari.X)
            writer.Float(chari.Z)
            writer.Float(chari.Y)

            writer.Word(chari.Angle)
            writer.Byte(0) 'dest
            writer.Byte(1) 'walk run flag
            writer.Byte(0) 'dest
            writer.Word(chari.Angle)
            writer.Byte(0) ' death flag
            writer.Byte(0) 'movement flag
            writer.Byte(chari.Berserk) 'berserk activated
            writer.Float(chari.WalkSpeed)
            writer.Float(chari.RunSpeed)
            writer.Float(chari.BerserkSpeed)

            writer.Byte(0) 'no buffs for now

            writer.Word(chari.CharacterName.Length)
            writer.String(chari.CharacterName)

            writer.Byte(0) 'job lvl
            writer.Byte(1) 'job type
            writer.DWord(0) 'trader exp
            writer.DWord(0) 'theif exp
            writer.DWord(0) 'hunter exp
            writer.DWord(0)
            writer.DWord(0)
            writer.DWord(0)
            writer.DWord(0)
            writer.Byte(0)
            writer.Byte(chari.PVP)
			writer.Byte(1)

            Return writer.GetBytes
        End Function

        Public Function CreateSpawnPacket(ByVal Index As Integer, ByVal ToXSec As Byte, ByVal ToYSec As Byte, ByVal ToXCord As Integer, ByVal ToZCord As Integer, ByVal ToYCord As Integer) As Byte()

            Dim chari As [cChar] = PlayerData(Index) 'Only for faster Code writing

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleSpawn)
            writer.DWord(chari.Model)
            writer.Byte(chari.Volume)
            writer.Byte(0)
            writer.Byte(0)

            'items
            Dim inventory As New cInventory(chari.MaxSlots)
            inventory = GameServer.DatabaseCore.FillInventory(chari)

            Dim PlayerItemCount As Integer = 0
            For b = 0 To 9
                If inventory.UserItems(b).Pk2Id <> 0 And inventory.UserItems(b).Pk2Id <> 62 Then
                    PlayerItemCount += 1
                End If
            Next

            writer.Byte(chari.MaxSlots)
            writer.Byte(PlayerItemCount)

            For b = 0 To 9
                If inventory.UserItems(b).Pk2Id <> 0 Then
                    If inventory.UserItems(b).Pk2Id <> 62 Then 'Dont send arrows
                        writer.DWord(inventory.UserItems(b).Pk2Id)
                        writer.Byte(inventory.UserItems(b).Plus)
                    End If

                End If
            Next

            writer.Byte(4) 'avatar start
            writer.Byte(0) '0 avatars fro now
            writer.Byte(0)

            writer.DWord(chari.UniqueId)
            writer.Byte(chari.XSector)
            writer.Byte(chari.YSector)
            writer.Float(chari.X)
            writer.Float(chari.Z)
            writer.Float(chari.Y)

            writer.Word(chari.Angle)
            writer.Byte(1) 'dest
            writer.Byte(1) 'walk run flag
            writer.Byte(ToXSec)
            writer.Byte(ToYSec)
            writer.Word(ToXCord)
            writer.Word(ToZCord)
            writer.Word(ToYCord)

            writer.Byte(1)
            writer.Byte(3) 'movement flag
            writer.Byte(chari.Berserk) 'berserk activated

            writer.Float(chari.WalkSpeed)
            writer.Float(chari.RunSpeed)
            writer.Float(chari.BerserkSpeed)

            writer.Byte(0) 'no buffs for now

            writer.Word(chari.CharacterName.Length)
            writer.String(chari.CharacterName)

            writer.Byte(0) 'job lvl
            writer.Byte(1) 'job type
            writer.DWord(0) 'trader exp
            writer.DWord(0) 'theif exp
            writer.DWord(0) 'hunter exp
            writer.DWord(0)
            writer.DWord(0)
            writer.DWord(0)
            writer.DWord(0)
            writer.Byte(0)
            writer.Byte(chari.PVP)
            writer.Byte(1)

            Return writer.GetBytes
        End Function

        Public Function CreateDespawnPacket(ByVal UniqueID As Integer) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleDespawn)
            writer.DWord(UniqueID)
            Return writer.GetBytes
        End Function


        Public Sub SpawnMe(ByVal Index As Integer)
            Dim range As Integer = 750

            For refindex As Integer = 0 To Server.OnlineClient - 1
                Dim socket As Net.Sockets.Socket = ClientList.GetSocket(refindex)
                Dim player As [cChar] = PlayerData(refindex) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso Index <> refindex Then
                    Dim distance As Long = Math.Round(Math.Sqrt((PlayerData(Index).X - player.X) ^ 2 + (PlayerData(Index).Y - player.Y) ^ 2)) 'Calculate Distance
                    If distance < range Then
                        If PlayerData(refindex).SpawnedPlayers.Contains(Index) = False Then
							Server.Send(CreateSpawnPacket(Index), refindex)
							Server.Send(CreateHelperIconPacket(Index), refindex) 'TODO: Is there a proper way to do this?
                            PlayerData(refindex).SpawnedPlayers.Add(Index)
                        End If
                    End If
                End If
            Next refindex

        End Sub


        Public Sub SpawnOtherPlayer(ByVal Index As Integer)
            Dim range As Integer = 750

            For refindex As Integer = 0 To Server.OnlineClient
                Dim socket As Net.Sockets.Socket = ClientList.GetSocket(refindex)
                Dim player As [cChar] = PlayerData(refindex) 'Check if Player is ingame
                If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso Index <> refindex Then
                    Dim distance As Long = Math.Round(Math.Sqrt((PlayerData(Index).X - player.X) ^ 2 + (PlayerData(Index).Y - player.Y) ^ 2)) 'Calculate Distance
                    If distance < range Then
                        If PlayerData(Index).SpawnedPlayers.Contains(refindex) = False Then
							Server.Send(CreateSpawnPacket(refindex), Index)
							Server.Send(CreateHelperIconPacket(refindex), Index)	'TODO: Is there a proper way to do this?
                            PlayerData(Index).SpawnedPlayers.Add(refindex)
						End If
					End If
                End If
            Next refindex
        End Sub

        Public Sub DespawnPlayerTeleport(ByVal Index As Integer)

            For i = 0 To PlayerData(Index).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index).SpawnedPlayers(i)
                If PlayerData(Other_Index).SpawnedPlayers.Contains(Index) = True Then
                    Server.Send(CreateDespawnPacket(PlayerData(Index).UniqueId), Other_Index)
                    PlayerData(Other_Index).SpawnedPlayers.Remove(Index)

                    Server.Send(CreateDespawnPacket(PlayerData(Other_Index).UniqueId), Index)
                    PlayerData(Index).SpawnedPlayers.Remove(Other_Index)
                End If
            Next
        End Sub

        Public Sub DespawnPlayer(ByVal Index As Integer)
            For i = 0 To PlayerData(Index).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index).SpawnedPlayers(i)
                If PlayerData(Other_Index).SpawnedPlayers.Contains(Index) = True Then
                    Server.Send(CreateDespawnPacket(PlayerData(Index).UniqueId), Other_Index)
                    PlayerData(Other_Index).SpawnedPlayers.Remove(Index)
                End If
            Next
        End Sub

        Public Sub DespawnPlayerRange(ByVal Index As Integer)
            Dim range As Integer = 750

            For i = 0 To PlayerData(Index).SpawnedPlayers.Count - 1
                Dim Other_Index As Integer = PlayerData(Index).SpawnedPlayers(i)
                Dim distance As Long = Math.Round(Math.Sqrt((PlayerData(Index).X - PlayerData(Other_Index).X) ^ 2 + (PlayerData(Index).Y - PlayerData(Other_Index).Y) ^ 2)) 'Calculate Distance
                If distance > range Then
                    'Despawn for both
                    Server.Send(CreateDespawnPacket(PlayerData(Index).UniqueId), Other_Index)
                    PlayerData(Other_Index).SpawnedPlayers.Remove(Index)

                    Server.Send(CreateDespawnPacket(PlayerData(Other_Index).UniqueId), Index)
                    PlayerData(Index).SpawnedPlayers.Remove(Other_Index)
                End If
            Next
		End Sub

		Public Function CreateHelperIconPacket(ByVal index_ As Integer) As Byte()
			Dim writer As New PacketWriter
			writer.Create(ServerOpcodes.HelperIcon)
			writer.DWord(PlayerData(index_).UniqueId)
			writer.Byte(PlayerData(index_).HelperIcon)
			Return writer.GetBytes()
		End Function

	End Module
End Namespace
