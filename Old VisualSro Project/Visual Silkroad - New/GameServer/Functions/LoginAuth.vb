Imports SRFramework

Namespace Functions
    Module LoginAuth
        Public Sub Gateway(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim clientString As String = packet.String(packet.Word)

            If clientString = "SR_Client" Then
                Dim writer As New PacketWriter
                Const name As String = "AgentServer"
                writer.Create(ServerOpcodes.LOGIN_SERVER_INFO)
                writer.Word(name.Length)
                writer.String(name)
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

        Public Sub CheckLoginHandler(ByVal Index_ As Integer, ByVal packet As PacketReader)
            Dim userLoader As New GameDB.GameUserLoader(packet, Index_)
            AddHandler userLoader.GetCallback, AddressOf CheckLogin
            userLoader.LoadFromGlobal(0)

        End Sub
        
        Public Sub CheckLogin(user As cUser, ByVal packet As PacketReader, ByVal Index_ As Integer)
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

           'GlobalManager Rulez!!! 
            SessionInfo(Index_).Username = username
            GlobalManager.OnGameserverSendUserAuth(sessionID, username, password, Index_)

        End Sub

        Public Sub CheckGlobalManagerUserAuthReply(ByVal succeed As Byte, ByVal errortag As Byte, user As cUser, ByVal Index_ As Integer)
          
            'Checking
            Dim loggedIn As Boolean = False

            'Logged in check
            For i = 0 To Server.MaxClients - 1
                If CharListing(i) IsNot Nothing And user IsNot Nothing Then
                    If CharListing(i).LoginInformation.AccountID = user.AccountID Then
                        loggedIn = True
                        Exit For
                    End If
                End If
            Next

            For i = 0 To Server.MaxClients - 1
                If PlayerData(i) IsNot Nothing And user IsNot Nothing Then
                    If PlayerData(i).AccountID = user.AccountID Then
                        loggedIn = True
                        Exit For
                    End If
                End If
            Next


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.GAME_AUTH)

            'Checks based on our Server
            If loggedIn Then
                writer.Byte(2)
                writer.Byte(2) 'User is already logged in (c4?)
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)

            ElseIf Server.OnlineClients + 1 > Server.MaxNormalClients And user.Permission = cUser.UserType.Normal Then
                'overload prevention --> user conencts back to login
                writer.Byte(2)
                writer.Byte(4)  'Server Full 
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
           
            'Check based on GlobalManager Data
            ElseIf succeed = 1 And user IsNot Nothing Then
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
                CharListing(Index_) = New cCharListing
                CharListing(Index_).LoginInformation = New cUser
                CharListing(Index_).LoginInformation = user
                CharListing(Index_).LoginInformation.LoggedIn = True
            ElseIf succeed = 2 Then
                Select Case errortag
                    Case 1
                        'wrong SessionID
                        If Server.Server_DebugMode Then
                            Log.WriteSystemLog("CheckGlobalManagerUserAuthReply::wrong SessionID")
                        End If
                    Case 2
                        'wrong ID/PW
                        If Server.Server_DebugMode Then
                            Log.WriteSystemLog("CheckGlobalManagerUserAuthReply::wrong ID/PW")
                        End If
                    Case 3
                        'User Object Fail
                        If Server.Server_DebugMode Then
                            Log.WriteSystemLog("CheckGlobalManagerUserAuthReply::User Object Fail")
                        End If
                End Select

                writer.Byte(2)
                writer.Byte(2)
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
            End If
        End Sub
    End Module
End Namespace
