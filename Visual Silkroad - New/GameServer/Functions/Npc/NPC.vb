Namespace GameServer.Functions
    Module NPC

        Public Function CreateNPCGroupSpawnPacket(ByVal NpcIndex As Integer) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GroupSpawnStart)
            writer.Byte(1) 'spawn
            writer.Word(1)
            Dim bytes1 As Byte() = writer.GetBytes
            Dim bytes2 As Byte() = CreateNPCSpawnPacket(NpcIndex)
            writer.Create(ServerOpcodes.GroupSpawnEnd)
            Dim bytes3 As Byte() = writer.GetBytes
            Dim finalbytes((bytes1.Length + bytes2.Length + bytes3.Length) - 1) As Byte
            Array.ConstrainedCopy(bytes1, 0, finalbytes, 0, bytes1.Length)
            Array.ConstrainedCopy(bytes2, 0, finalbytes, 9, bytes2.Length)
            Array.ConstrainedCopy(bytes3, 0, finalbytes, (bytes2.Length) + 9, bytes3.Length)
            Return finalbytes
        End Function


        Public Function CreateNPCSpawnPacket(ByVal NpcIndex As Integer) As Byte()
            Dim npc As cNPC = NpcList(NpcIndex)
            Dim obj As Object_ = GetObjectById(npc.Pk2ID)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GroupSpawnData)
            writer.DWord(npc.Pk2ID)
            writer.DWord(npc.UniqueID)
            writer.Byte(npc.Position.XSector)
            writer.Byte(npc.Position.YSector)
            writer.Float(npc.Position.X)
            writer.Float(npc.Position.Z)
            writer.Float(npc.Position.Y)

            Select Case obj.Type
                Case Object_.Type_.Npc
                    writer.Word(npc.Angle)
                    writer.Byte(0)
                    writer.Byte(1)
                    writer.Byte(0)
                    writer.Word(npc.Angle)
                    writer.Byte(1)
                    writer.Byte(0)
                    writer.Byte(0)
                    writer.DWord(0) 'speeds
                    writer.DWord(0)
                    writer.Float(100) 'berserker speed
                    writer.Word(0)
                    

                Case Object_.Type_.Teleport
                    writer.Word(0)

            End Select

            Return writer.GetBytes
        End Function

        Public Sub SpawnNPC(ByVal ItemId As UInteger, ByVal Position As Position, ByVal Angle As UInt16)
            Dim npc_ As Object_ = GetObjectById(ItemId)
            Dim toadd As New cNPC
            toadd.UniqueID = Id_Gen.GetUnqiueID
            toadd.Pk2ID = npc_.Pk2ID
            toadd.Angle = Angle
            If npc_.Type = Object_.Type_.Npc Then
                toadd.Position = Position
            ElseIf npc_.Type = Object_.Type_.Teleport Then
                toadd.Position = npc_.T_Position
            End If

            NpcList.Add(toadd)
            Dim MyIndex As UInteger = NpcList.IndexOf(toadd)
        End Sub

        Public Sub OnNpcChat(ByVal NpcIndex As Integer, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            Dim obj As Object_ = GetObjectById(NpcList(NpcIndex).Pk2ID)
            Dim name As String() = obj.TypeName.Split("_")

            writer.Create(ServerOpcodes.Target)
            writer.Byte(1) 'Sucess
            writer.DWord(NpcList(NpcIndex).UniqueID)

            If obj.ChatBytes IsNot Nothing Then
                For i = 0 To obj.ChatBytes.Length - 1
                    writer.Byte(obj.ChatBytes(i))
                Next
            Else
                writer.Byte(0)
                writer.DWord(0)
                writer.Byte(0)
            End If


            CheckForTax(obj.TypeName, writer)
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = True
        End Sub

        Public Sub OnNpcChatSelect(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim UniqueID As UInteger = packet.DWord
            Dim ChatId As Byte = packet.Byte
            Dim NpcIndex As Integer
            Dim RefObj As New Object_

            For i = 0 To NpcList.Count - 1
                If NpcList(i).UniqueID = UniqueID Then
                    NpcIndex = i
                    RefObj = GetObjectById(NpcList(NpcIndex).Pk2ID)
                    Exit For
                End If
            Next



            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Npc_Chat)
            writer.Byte(1) 'Sucess
            If RefObj.TypeName <> "NPC_CH_GACHA_MACHINE" Then
                writer.Byte(ChatId) 'Type
            Else
                writer.Byte(17)
            End If

            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = True
        End Sub

        Public Sub OnNpcChatLeft(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ObjectID As UInteger = packet.DWord

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Npc_Chat_Left)
            writer.Byte(1) 'Sucess
            Server.Send(writer.GetBytes, Index_)

            PlayerData(Index_).Busy = False
        End Sub


        Public Sub OnNpcTeleport(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ObjectID As UInteger = packet.DWord
            Dim type As Byte = packet.Byte() '2=normal teleport; 5=special point; 

            Select Case type
                Case 2
                    Dim TeleportNumber As Integer = packet.DWord
                    Dim Point_ As TeleportPoint_ = GetTeleportPoint(TeleportNumber)
                    Dim writer As New PacketWriter
                    writer.Create(ServerOpcodes.Npc_Teleport_Confirm)

                    If CSng(PlayerData(Index_).Gold) - Point_.Cost < 0 Then
                        'Not enough Gold...
                        writer.Byte(2)
                        writer.Byte(7)
                        'Server.Send(writer.GetBytes, Index_)

                    ElseIf PlayerData(Index_).Level < Point_.MinLevel And Point_.MinLevel > 0 Then
                        'Level too low
                        writer.Byte(2)
                        writer.Byte(&H15)
                        writer.Byte(&H1C)
                        Server.Send(writer.GetBytes, Index_)

                    ElseIf PlayerData(Index_).Level > Point_.MaxLevel And Point_.MaxLevel > 0 Then
                        'Level too high
                        writer.Byte(2)
                        writer.Byte(&H16)
                        writer.Byte(&H1C)
                        Server.Send(writer.GetBytes, Index_)

                    Else
                        PlayerData(Index_).Busy = True
                        PlayerData(Index_).Position = Point_.ToPos
                        PlayerData(Index_).TeleportType = TeleportType_.Npc

                        PlayerData(Index_).Gold -= Point_.Cost
                        UpdateGold(Index_)

                        writer.Create(ServerOpcodes.Npc_Teleport_Confirm)
                        writer.Byte(1)
                        Server.Send(writer.GetBytes, Index_)


                        DataBase.SaveQuery(String.Format("UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'", PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector, Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z), Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                        OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                    End If


                Case 5
                    Dim TeleportNumber As Integer = packet.Byte '0x02=recall point, 0x03=move to dead point
            End Select
        End Sub



        Private Sub CheckForTax(ByVal Model_Name As String, ByVal writer As PacketWriter)
            Select Case Model_Name
                Case "NPC_CH_SMITH", "NPC_CH_ARMOR", "NPC_CH_POTION", "NPC_CH_ACCESSORY", _
                 "STORE_CH_GATE", "NPC_CH_FERRY", "NPC_CH_FERRY2"
                    writer.Word(Settings.Server_TaxRate)
            End Select
        End Sub


    End Module
End Namespace
