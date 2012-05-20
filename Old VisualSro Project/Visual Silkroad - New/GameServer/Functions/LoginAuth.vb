Namespace GameServer.Functions
    Module LoginAuth
        Public Sub GateWay(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ClientString As String = packet.String(packet.Word)

            If ClientString = "SR_Client" Then
                Dim writer As New PacketWriter
                Dim name As String = "AgentServer"
                writer.Create(ServerOpcodes.ServerInfo)
                writer.Word(name.Length)
                writer.HexString(name)
                writer.Byte(0)
                Server.Send(writer.GetBytes, Index_)

                ClientList.SessionInfo(Index_).ClientType = _SessionInfo.ConnectionType.SR_Client
                ClientList.SessionInfo(Index_).Authorized = True
            ElseIf "SR_Admin" Then
                ClientList.SessionInfo(Index_).ClientType = _SessionInfo.ConnectionType.SR_Admin
                Dim UserName As String = packet.String(packet.Word)
                Dim Password As String = packet.String(packet.Word)

                Dim UserIndex As Integer = GameDB.GetUserWithAccName(UserName)
                If _
                    GameDB.Users(UserIndex).Name = UserName And GameDB.Users(UserIndex).Pw = Password And
                    GameDB.Users(UserIndex).Admin Then
                    ClientList.SessionInfo(Index_).Authorized = True
                    ClientList.SessionInfo(Index_).UserName = UserName
                End If
            End If
        End Sub

        Public Sub CheckLogin(ByVal index_ As Integer, ByVal packet As PacketReader)
            Dim Key As UInteger = packet.DWord()
            Dim Name As String = packet.String(packet.Word)
            Dim Password As String = packet.String(packet.Word)

            'Checking
            Dim UserIndex As Integer = GameDB.GetUserWithAccName(Name)
            Dim Real_Key As UInt32 = GetKey(index_)
            Dim Logged_In As Boolean = False

            For i = 0 To Server.MaxClients
                If ClientList.CharListing(i) IsNot Nothing Then
                    If ClientList.CharListing(i).LoginInformation.Id = GameDB.Users(UserIndex).Id Then
                        Logged_In = True
                    End If
                End If
            Next

            For i = 0 To Server.MaxClients
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).AccountID = GameDB.Users(UserIndex).Id Then
                        Logged_In = True
                    End If
                End If
            Next


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.LoginAuthInfo)

            If Logged_In = True Or GameDB.Users(UserIndex).Banned = True Then
                writer.Byte(2)
                writer.Byte(2)
                'Server Full aka User is already logged in
                Server.Send(writer.GetBytes, index_)
                Server.Disconnect(index_)
            ElseIf _
                GameDB.Users(UserIndex).Name = Name And GameDB.Users(UserIndex).Pw = Password And Key = Real_Key And
                ClientList.SessionInfo(index_).Authorized Then
                writer.Byte(1)
                Server.Send(writer.GetBytes, index_)
                ClientList.CharListing(index_) = New cCharListing
                ClientList.CharListing(index_).LoginInformation = New cCharListing.UserArray
                ClientList.CharListing(index_).LoginInformation = GameDB.Users(UserIndex)
                ClientList.CharListing(index_).LoginInformation.LoggedIn = True
            ElseIf GameDB.Users(UserIndex).Name <> Name Or GameDB.Users(UserIndex).Pw <> Password Or Key <> Real_Key _
                Then
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
