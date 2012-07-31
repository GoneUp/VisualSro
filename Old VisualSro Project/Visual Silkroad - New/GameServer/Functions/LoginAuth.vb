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

                Dim userIndex As Integer = GameDB.GetUser(userName)
                If GameDB.Users(userIndex).Name = userName And GameDB.Users(userIndex).Pw = password And GameDB.Users(userIndex).Permission Then
                    ClientList.SessionInfo(Index_).Authorized = True
                    ClientList.SessionInfo(Index_).UserName = userName
                End If
            End If
        End Sub

        Public Sub CheckLogin(ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim key As UInteger = packet.DWord()
            Dim name As String = packet.String(packet.Word)
            Dim password As String = packet.String(packet.Word)

            'Checking
            Dim userIndex As Integer = GameDB.GetUser(name)
            Dim realKey As UInt32 = GetKey(Index_)
            Dim loggedIn As Boolean = False

            For i = 0 To Server.MaxClients
                If ClientList.CharListing(i) IsNot Nothing Then
                    If ClientList.CharListing(i).LoginInformation.Id = GameDB.Users(userIndex).Id Then
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


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_AUTH)

            If loggedIn = True Or GameDB.Users(userIndex).Banned = True Then
                writer.Byte(2)
                writer.Byte(2)
                'Server Full aka User is already logged in
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
            ElseIf GameDB.Users(userIndex).Name = name And GameDB.Users(userIndex).Pw = password And key = realKey And ClientList.SessionInfo(Index_).Authorized Then
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
                ClientList.CharListing(Index_) = New cCharListing
                ClientList.CharListing(Index_).LoginInformation = New cCharListing.UserArray
                ClientList.CharListing(Index_).LoginInformation = GameDB.Users(userIndex)
                ClientList.CharListing(Index_).LoginInformation.LoggedIn = True
            ElseIf GameDB.Users(userIndex).Name <> name Or GameDB.Users(userIndex).Pw <> password Or key <> realKey Then
                writer.Byte(2)
                writer.Byte(2)
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
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
