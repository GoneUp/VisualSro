Imports SRFramework

Namespace Functions
    Module LoginAuth
        Public Sub GateWay(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim clientString As String = packet.String(packet.Word)

            If clientString = "SR_Client" Then
                Dim writer As New PacketWriter
                Dim name As String = "AgentServer"
                writer.Create(ServerOpcodes.LOGIN_SERVER_INFO)
                writer.Word(name.Length)
                writer.HexString(name)
                writer.Byte(0)
                Server.Send(writer.GetBytes, Index_)

                ClientList.SessionInfo(Index_).ClientType = _SessionInfo.ConnectionType.SR_Client
                ClientList.SessionInfo(Index_).Authorized = True
            ElseIf "SR_Admin" Then
                ClientList.SessionInfo(Index_).ClientType = _SessionInfo.ConnectionType.SR_Admin
                Dim userName As String = packet.String(packet.Word)
                Dim password As String = packet.String(packet.Word)

                Dim userIndex As Integer = GameDB.GetUserIndex(userName)
                If GameDB.Users(userIndex).Name = userName And GameDB.Users(userIndex).Pw = password And GameDB.Users(userIndex).Permission = 1 Then
                    ClientList.SessionInfo(Index_).Authorized = True
                    ClientList.SessionInfo(Index_).Username = userName
                End If
            End If
        End Sub

        Public Sub CheckLogin(ByVal Index_ As Integer, ByVal packet As PacketReader)
            ClientList.SessionInfo(Index_).LoginAuthRequired = False 'to prevent a dc

            Dim sessionID As UInteger = packet.DWord()
            Dim username As String = packet.String(packet.Word)
            Dim password As String = packet.String(packet.Word)

            'Checking
            Dim userIndex As Integer = GameDB.GetUserIndex(username)
            Dim loggedIn As Boolean = False

            'Logged in check
            For i = 0 To Server.MaxClients
                If CharListing(i) IsNot Nothing Then
                    If CharListing(i).LoginInformation.Id = GameDB.Users(userIndex).Id Then
                        loggedIn = True
                        Exit For
                    End If
                End If
            Next

            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).AccountID = GameDB.Users(userIndex).Id Then
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
            ElseIf Server.OnlineClients + 1 > Server.MaxNormalClients And GameDB.Users(userIndex).Permission = 0 Then
                'overload prevention --> user conencts back to login
                writer.Byte(2)
                writer.Byte(4)  'Server Full 
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
            Else
                'GlobalManager Rulez!!! 
                ClientList.SessionInfo(Index_).Username = username

                GlobalManager.OnGameserverSendUserAuth(sessionID, username, password, Index_)
            End If
        End Sub

        Public Sub Check_GlobalManagerUserAuthReply(ByVal succeed As Byte, ByVal errortag As Byte, ByVal index_ As Integer)
            Dim userIndex As Integer = GameDB.GetUserIndex(ClientList.SessionInfo(index_).Username)
            Dim user As cCharListing.UserArray = GameDB.Users(userIndex)

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_AUTH)
            If succeed = 1 Then
                writer.Byte(1)
                Server.Send(writer.GetBytes, index_)
                CharListing(index_) = New cCharListing
                CharListing(index_).LoginInformation = New cCharListing.UserArray
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
            Dim split1 As String() = ClientList.GetSocket(Index_).RemoteEndPoint.ToString.Split(":")
            Dim split2 As String() = split1(0).Split(".")
            Dim key As UInt32 = CUInt(split2(0)) + CUInt(split2(1)) + CUInt(split2(2)) + CUInt(split2(3))
            Return key
        End Function
    End Module
End Namespace
