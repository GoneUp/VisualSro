Imports SRFramework

Namespace Functions
    Module LoginAuth
        Public Sub Gateway(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim clientString As String = packet.String(packet.Word)

            If clientString = "SR_Client" Then
                Dim writer As New PacketWriter
                Dim name As String = "AgentServer"
                writer.Create(ServerOpcodes.LOGIN_SERVER_INFO)
                writer.Word(name.Length)
                writer.HexString(name)
                writer.Byte(0)
                Server.Send(writer.GetBytes, Index_)

                SessionInfo(Index_).ClientName = clientString
                SessionInfo(Index_).Authorized = True
            End If

            If SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.HANDSHAKE Then
                SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.WHOAMI
            Else
                Server.Disconnect(Index_)
            End If
        End Sub

        Public Sub CheckLogin(ByVal Index_ As Integer, ByVal packet As PacketReader)
            SessionInfo(Index_).LoginAuthRequired = False 'to prevent a dc

            If SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.WHOAMI Then
                SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.AUTH
            Else
                Server.Disconnect(Index_)
                Exit Sub
            End If


            Dim sessionID As UInteger = packet.DWord()
            Dim username As String = packet.String(packet.Word)
            Dim password As String = packet.String(packet.Word)

            'Checking
            Dim userIndex As Integer = GameDB.GetUserIndex(username)
            Dim loggedIn As Boolean = False

            'Logged in check
            For i = 0 To Server.MaxClients - 1
                If CharListing(i) IsNot Nothing Then
                    If CharListing(i).LoginInformation.AccountId = GameDB.Users(userIndex).AccountId Then
                        loggedIn = True
                        Exit For
                    End If
                End If
            Next

            For i = 0 To Server.MaxClients - 1
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).AccountID = GameDB.Users(userIndex).AccountId Then
                        loggedIn = True
                        Exit For
                    End If
                End If
            Next

            'Reply in case of fail, if it succeed on these basic checks it will be sent to the globalmanager
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_AUTH)

            If loggedIn = True Or GameDB.Users(userIndex).Banned = True Then
                writer.Byte(2)
                writer.Byte(2) 'User is already logged in (c4?)
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
            ElseIf Server.OnlineClients + 1 > Server.MaxNormalClients And GameDB.Users(userIndex).Permission = cUser.UserType.Normal Then
                'overload prevention --> user conencts back to login
                writer.Byte(2)
                writer.Byte(4)  'Server Full 
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
            Else
                'GlobalManager Rulez!!! 
                SessionInfo(Index_).Username = username

                GlobalManager.OnGameserverSendUserAuth(sessionID, username, password, Index_)
            End If
        End Sub

        Public Sub Check_GlobalManagerUserAuthReply(ByVal succeed As Byte, ByVal errortag As Byte, ByVal index_ As Integer)
            Dim userIndex As Integer = GameDB.GetUserIndex(SessionInfo(index_).Username)
            Dim user As cUser = GameDB.Users(userIndex)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_AUTH)
            If succeed = 1 Then
                writer.Byte(1)
                Server.Send(writer.GetBytes, index_)
                CharListing(index_) = New cCharListing
                CharListing(index_).LoginInformation = New cUser
                CharListing(index_).LoginInformation = user
                CharListing(index_).LoginInformation.LoggedIn = True
            Else
                Select Case errortag
                    Case 1
                        'wrong SessionID
                    Case 2
                        'wrong ID/PW
                End Select

                writer.Byte(2)
                writer.Byte(2)
                Server.Send(writer.GetBytes, index_)
                Server.Disconnect(index_)
            End If
        End Sub


        Private Function GetKey(ByVal Index_ As Integer) As UInt32
            Dim split1 As String() = Server.ClientList.GetSocket(Index_).RemoteEndPoint.ToString.Split(":")
            Dim split2 As String() = split1(0).Split(".")
            Dim key As UInt32 = CUInt(split2(0)) + CUInt(split2(1)) + CUInt(split2(2)) + CUInt(split2(3))
            Return key
        End Function
    End Module
End Namespace
