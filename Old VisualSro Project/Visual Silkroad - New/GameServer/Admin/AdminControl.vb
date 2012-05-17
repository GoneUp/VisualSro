Imports GameServer.GameServer.Functions
Namespace GameServer.Admin
    Module AdminControl

        Public Sub Parse(ByVal Packet As PacketReader, ByVal Index_ As Integer)
            Try
                If ClientList.SessionInfo(Index_).ClientType = _SessionInfo.ConnectionType.SR_Admin And ClientList.SessionInfo(Index_).Authorized Then

                    Dim tag As Byte = Packet.Byte
                    Select Case tag
                        Case 1
                            SendServerInfo(Index_)
                        Case 2
                            ChangeServerInfo(Packet, Index_)
                        Case 3
                            SendCharInfo(Packet, Index_)
                    End Select


                Else
                    Server.Disconnect(Index_)

                    Log.WriteGameLog(Index_, "ADMIN", "Unautorized", "Gm_Command_Try: " & BitConverter.ToString(Packet.GetData()))
                End If


            Catch ex As Exception
                Log.WriteSystemLog("Admin Error: " & ex.Message & " Stack: " & ex.StackTrace)
            End Try

        End Sub

        Public Sub SendServerInfo(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SR_Admin)
            writer.DWord(Server.OnlineClient)
            writer.DWord(MobList.Count)
            writer.DWord(NpcList.Count)
            writer.DWord(ItemList.Count)
            writer.Word(Settings.Server_XPRate)
            writer.Word(Settings.Server_SPRate)
            writer.Word(Settings.Server_GoldRate)
            writer.Word(Settings.Server_DropRate)
            writer.Word(Settings.Server_SpawnRate)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub ChangeServerInfo(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Settings.Server_PingDc = packet.Byte
            Settings.Server_XPRate = packet.Word
            Settings.Server_SPRate = packet.Word
            Settings.Server_GoldRate = packet.Word
            Settings.Server_DropRate = packet.Word
            Settings.Server_SpawnRate = packet.Word
        End Sub

        Public Sub SendCharInfo(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim iChar As New [cChar]
            Dim UserIndex As Integer = GameDB.GetUserWithAccName(ClientList.SessionInfo(Index_).UserName)
            Dim Account As New cCharListing With {.LoginInformation = GameDB.Users(UserIndex)}

            Dim CharList As Boolean
            Dim CharListIndex As Integer
            Dim Ingame As Boolean
            Dim IngameIndex As Integer

            Dim tag As Byte = packet.Byte
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SR_Admin)

            If tag = 1 Then
                For i = 0 To Server.MaxClients
                    If ClientList.CharListing(i) IsNot Nothing Then
                        If ClientList.CharListing(i).LoginInformation.Id = GameDB.Users(UserIndex).Id Then
                            CharList = True
                            CharListIndex = i
                            GameDB.FillCharList(Account)
                            iChar = Account.Chars(0)
                            Exit For
                        End If
                    End If
                Next

                For i = 0 To Server.MaxClients
                    If PlayerData(i) IsNot Nothing Then
                        If PlayerData(i).AccountID = GameDB.Users(UserIndex).Id Then
                            Ingame = True
                            IngameIndex = i
                            iChar = PlayerData(IngameIndex)
                            Exit For
                        End If
                    End If
                Next

                writer.Word(iChar.CharacterName.Length)
                writer.String(iChar.CharacterName)
                If Ingame Then
                    writer.Byte(True) 'Online
                    writer.Byte(1)
                ElseIf CharList Then
                    writer.Byte(True) 'Online
                    writer.Byte(2)
                Else
                    writer.Byte(False) 'Offline
                    writer.Byte(0)
                End If
                writer.Byte(iChar.Level)
                writer.DWord(iChar.SkillPoints)
                writer.QWord(iChar.Gold)
                writer.Word(iChar.Attributes)
                writer.Word(iChar.Strength)
                writer.Word(iChar.Intelligence)
                writer.Word(Account.LoginInformation.Silk)

            Else
                Dim TmpName As String = packet.String(packet.Word)
                For i = 0 To Server.MaxClients
                    If PlayerData(i) IsNot Nothing Then
                        If PlayerData(i).CharacterName = TmpName Then
                            Ingame = True
                            IngameIndex = i
                            Exit For
                        End If
                    End If
                Next

                writer.Word(iChar.CharacterName.Length)
                writer.String(iChar.CharacterName)
                If Ingame Then
                    writer.Byte(True) 'Online
                    writer.Byte(1)
                ElseIf CharList Then
                    writer.Byte(True) 'Online
                    writer.Byte(2)
                Else
                    writer.Byte(False) 'Offline
                    writer.Byte(0)
                End If
                writer.Byte(iChar.Level)
                writer.DWord(iChar.SkillPoints)
                writer.QWord(iChar.Gold)
                writer.Word(iChar.Attributes)
                writer.Word(iChar.Strength)
                writer.Word(iChar.Intelligence)
                writer.Word(Account.LoginInformation.Silk)
            End If

            Server.Send(writer.GetBytes, Index_)
        End Sub

    End Module
End Namespace
