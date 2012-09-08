Imports SRFramework

Namespace Functions
    Module Login

        Public Sub Gateway(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ClientString As String = packet.String(packet.Word)

            If ClientString = "SR_Client" Then
                Dim writer As New PacketWriter
                Dim name As String = "GatewayServer"
                writer.Create(ServerOpcodes.LOGIN_SERVER_INFO)
                writer.Word(name.Length)
                writer.String(name)
                writer.Byte(0)
                Server.Send(writer.GetBytes, Index_)
            End If

            If SessionInfo(Index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.HANDSHAKE Then
                SessionInfo(Index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.WHOAMI
            Else
                Server.Disconnect(Index_)
            End If
        End Sub
        Public Sub ClientInfo(ByVal packet As PacketReader, ByVal Index_ As Integer)
            SessionInfo(Index_).Locale = packet.Byte
            SessionInfo(Index_).ClientName = packet.String(packet.Word)
            SessionInfo(Index_).Version = packet.DWord

            If Settings.Log_Connect Then
                With SessionInfo(Index_)
                    Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "Client_Connect", "(None)", String.Format("Locale: {0}, Name: {1}, Version: {2}", .Locale, .ClientName, .Version))
                End With
            End If

            If SessionInfo(Index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.WHOAMI Then
                SessionInfo(Index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.PATCH_INFO
            Else
                Server.Disconnect(Index_)
            End If
        End Sub

        Public Sub SendPatchInfo(ByVal Index_ As Integer)
            'Note: Patch Info for Rsro
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.LOGIN_MASSIVE_MESSAGE)
            writer.Byte(1) 'Header Byte
            writer.Word(1) '1 Data Packet
            writer.Word(&H2005)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.LOGIN_MASSIVE_MESSAGE)
            writer.Byte(0) 'Data
            writer.Word(1)
            writer.Byte(1)
            writer.Byte(8)
            writer.Byte(&HA)
            writer.DWord(5)
            writer.Byte(2)
            Server.Send(writer.GetBytes, Index_)

            '====================================
            writer.Create(ServerOpcodes.LOGIN_MASSIVE_MESSAGE)
            writer.Byte(1) 'Header Byte
            writer.Word(1) '1 Data Packet
            writer.Word(&H6005)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.LOGIN_MASSIVE_MESSAGE)
            writer.Byte(0) 'Data
            writer.Word(3)
            writer.Word(2)
            writer.Byte(2)
            Server.Send(writer.GetBytes, Index_)

            '====================================
            writer.Create(ServerOpcodes.LOGIN_MASSIVE_MESSAGE)
            writer.Byte(1) 'Header Byte
            writer.Word(1) '1 Data Packet
            writer.Word(&HA100)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.LOGIN_MASSIVE_MESSAGE)
            writer.Byte(0) 'Data

            If SessionInfo(Index_).Version = Settings.Server_CurrectVersion Then
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
            ElseIf SessionInfo(Index_).Version > Settings.Server_CurrectVersion Then
                'Client too new 
                writer.Byte(2)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
            ElseIf SessionInfo(Index_).Version < Settings.Server_CurrectVersion Then
                'Client too old 
                writer.Byte(2)
                writer.Byte(5)
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
            ElseIf SessionInfo(Index_).Locale <> Settings.Server_Local Then
                'Wrong Local
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
            ElseIf SessionInfo(Index_).ClientName <> "SR_Client" Then
                'Wrong Clientname
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
            End If


            '2005
            '6005
            'A100

            '05 00 0D 60 00 00 01 01 00 05 60 
            '06 00 0D 60 00 00 00  03 00 02 00 02 
            '05 00 0D 60 00 00 01 01 00 00 A1 
            '02 00 0D 60 00 00 00 01                                  

        End Sub

        Public Sub SendLauncherInfo(ByVal Index_ As Integer)
            If SessionInfo(Index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.PATCH_INFO Then
                SessionInfo(Index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.LAUNCHER
            Else
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.LOGIN_MASSIVE_MESSAGE)
            writer.Byte(1) 'Header Byte
            writer.Word(1) '1 Data Packet
            writer.Word(&HA104)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.LOGIN_MASSIVE_MESSAGE)
            writer.Byte(0) 'Data
            writer.Byte(LoginDb.News.Count) 'nummer of news

            For i = 0 To LoginDb.News.Count - 1
                writer.Word(LoginDb.News(i).Title.Length)
                writer.String(LoginDb.News(i).Title)

                writer.Word(LoginDb.News(i).Text.Length)
                writer.String(LoginDb.News(i).Text)

                writer.Word(LoginDb.News(i).Time.Year) 'jahr
                writer.Word(LoginDb.News(i).Time.Month)
                writer.Word(LoginDb.News(i).Time.Day)
                writer.Word(LoginDb.News(i).Time.Hour) 'hour
                writer.Word(LoginDb.News(i).Time.Minute) 'minute
                writer.Word(LoginDb.News(i).Time.Second) 'secound
                writer.DWord(LoginDb.News(i).Time.Millisecond) 'millisecond
            Next


            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub SendServerList(ByVal Index_ As Integer)
            If SessionInfo(Index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.PATCH_INFO Then
                SessionInfo(Index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.LOGIN
                Timers.LoginInfoTimer(Index_).Interval = 1000
                Timers.LoginInfoTimer(Index_).Start()
            ElseIf SessionInfo(Index_).SRConnectionSetup <> cSessionInfo_LoginServer.SRConnectionStatus.LOGIN Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim NameServer As String = "SRO_Russia_Official"
            Dim writer As New PacketWriter

            writer.Create(ServerOpcodes.LOGIN_SERVER_LIST)
            writer.Byte(1) 'Nameserver
            writer.Byte(57) 'Nameserver ID
            writer.Word(NameServer.Length)
            writer.String(NameServer)
            writer.Byte(0) 'Out of nameservers

            Dim tmplist = Shard_Gameservers.Keys.ToList
            For i = 0 To tmplist.Count - 1
                Dim key As UShort = tmplist(i)
                If Shard_Gameservers.ContainsKey(key) Then
                    writer.Byte(1) 'New Gameserver
                    writer.Word(Shard_Gameservers(key).ServerId)
                    writer.Word(Shard_Gameservers(key).ServerName.Length)
                    writer.String(Shard_Gameservers(key).ServerName)
                    writer.Word(Shard_Gameservers(key).OnlineClients)
                    writer.Word(Shard_Gameservers(key).MaxNormalClients)
                    writer.Byte(Shard_Gameservers(key).Online)
                End If
            Next

            writer.Byte(0)

            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub HandleLogin(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If SessionInfo(Index_).SRConnectionSetup <> cSessionInfo_LoginServer.SRConnectionStatus.LOGIN Then
                Server.Disconnect(Index_)
                Exit Sub
            End If

            Dim loginMethod As Byte = packet.Byte()
            Dim id As String = packet.String(packet.Word).ToLower
            Dim pw As String = packet.String(packet.Word).ToLower
            Dim serverID As Integer = packet.Word


            Timers.LoginInfoTimer(Index_).Stop()
            SessionInfo(Index_).GameServerId = serverID
            SessionInfo(Index_).UserName = id
            GlobalManager.OnSendUserAuth(serverID, id, pw, SessionInfo(Index_).IP, Index_)
            

            'Dim writer As New PacketWriter
            'writer.Create(ServerOpcodes.LOGIN_AUTH)
            'writer.Byte(2) 'fail
            'writer.Byte(3) 'already connected
            'writer.Byte(4) 'C5
            'writer.Byte(6) 'server full
            'writer.Byte(7) 'nothing
            'writer.Byte(8) 'ip limit
            'writer.Byte(9) 'billing server error
            'writer.Byte(10) 'aduslts over 18
            'writer.Byte(11) 'aduslts over 12
            'writer.Byte(12) 'only teen
            'writer.Byte(15) 'only teen
            '##########
            'writer.Byte(2) 'fail
            'writer.Byte(2) 'fail subcode
            'writer.Byte(1) 'block
            'writer.Byte(2) 'server in insepction
            'writer.Byte(3) 'use aggrement
            'writer.Byte(4) 'free service over
            '###########
            'Server.Send(writer.GetBytes, Index_)
            'Exit Sub

            'Dim userIndex As Integer = LoginDb.GetUser(id)
            'If userIndex = -1 Then
            '    'User exestiert nicht == We register a User

            '    If Settings.Auto_Register = True Then
            '        If CheckIfUserCanRegister(Server.ClientList.GetIP(Index_)) = True Then
            '            If RegisterUser(id, pw, Index_) Then
            '                LoginWriteSpecialText(String.Format("A new Account with the ID: {0} and Password: {1}. You can login in 60 Secounds.", id, pw), Index_)
            '            End If

            '        Else
            '            LoginWriteSpecialText(String.Format("You can only register {0} Accounts a day!", Settings.Max_RegistersPerDay), Index_)
            '        End If


            '    ElseIf Settings.Auto_Register = False Then
            '        'Normal Fail
            '        LoginWriteSpecialText("User does not existis!", Index_)
            '    End If

            'Else
            '    CheckBannTime(userIndex)

            '    If LoginDb.Users(userIndex).Banned = True Then

            '        writer.Byte(2) 'failed
            '        writer.Byte(2) 'subcode
            '        writer.Byte(1) 'ban
            '        writer.Word(LoginDb.Users(userIndex).BannReason.Length)
            '        writer.String(LoginDb.Users(userIndex).BannReason) 'grund
            '        writer.Date(LoginDb.Users(userIndex).BannTime) 'zeit

            '        Server.Send(writer.GetBytes, Index_)


            '    ElseIf LoginDb.Users(userIndex).Pw <> pw Then
            '        'pw falsch
            '        Dim user As cUser = LoginDb.Users(userIndex)
            '        user.FailedLogins += 1
            '        LoginDb.Users(userIndex) = user

            '        Database.SaveQuery(String.Format("UPDATE users SET failed_logins = '{0}' WHERE id = '{1}'", user.FailedLogins, user.AccountId))

            '        writer.Byte(2) 'login failed
            '        writer.Byte(1) 'wrong pw
            '        writer.DWord(user.FailedLogins) 'number of falied logins
            '        writer.DWord(Settings.Max_FailedLogins) 'Max Failed Logins
            '        Server.Send(writer.GetBytes, Index_)

            '        If user.FailedLogins >= Settings.Max_FailedLogins Then
            '            user.FailedLogins = 0
            '            LoginDb.Users(userIndex) = user

            '            Database.SaveQuery(String.Format("UPDATE users SET failed_logins = '0' WHERE id = '{0}'", user.AccountId))
            '            BanUser(Date.Now.AddMinutes(10), userIndex) 'Ban for 10 mins
            '        End If



            '    ElseIf LoginDb.Users(userIndex).Name = id And LoginDb.Users(userIndex).Pw = pw Then
            '        Dim gs As SRFramework.GameServer = Shard_Gameservers(serverID)

            '        If (gs.OnlineClients + 1) >= gs.MaxNormalClients And LoginDb.Users(userIndex).Permission = 0 Then
            '            writer.Byte(4)
            '            writer.Byte(2) 'Server traffic... 
            '            Server.Send(writer.GetBytes, Index_)

            '        Else
            '            Timers.LoginInfoTimer(Index_).Stop()

            '            'GlobalManager Part:
            '            SessionInfo(Index_).GameServerId = serverID
            '            SessionInfo(Index_).UserName = id
            '            GlobalManager.OnSendUserAuth(serverID, id, pw, SessionInfo(Index_).IP, Index_)

            '            If Settings.Log_Login Then
            '                Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "Login", "Sucess", String.Format("Name: {0}, Server: {1}", id, gs.ServerName))
            '            End If
            '        End If
            '    End If
            'End If
        End Sub

        Public Sub LoginSendUserAuthSucceed(packet As PacketReader)
            'ServerIndex + SessionID + Port + Index
            Dim Index_ As Integer = packet.DWord
            Dim succeed As Byte = packet.Byte

            If Server.ClientList.GetSocket(Index_) Is Nothing Then
                GlobalManagerCon.Log("Index_ from GlobalManager dosen't existis! user: " & Index_)

            ElseIf Shard_Gameservers.ContainsKey(SessionInfo(Index_).GameServerId) = False Then
                GlobalManagerCon.Log("GS from GlobalManager dosen't existis! user: " & Index_)

            Else
                Dim user As String = SessionInfo(Index_).UserName

                Dim writer As New PacketWriter
                writer.Create(ServerOpcodes.LOGIN_AUTH)

                If succeed = 1 Then
                    Dim sessionID As UInteger = packet.DWord
                    Dim gs As GameServer = Shard_Gameservers(SessionInfo(Index_).GameServerId)

                    writer.Byte(1)
                    writer.DWord(sessionID)
                    writer.Word(gs.IP.Length)
                    writer.String(gs.IP)
                    writer.Word(gs.Port)

                    If Settings.Log_Login Then
                        Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "Login", "Sucess", String.Format("Name: {0}, Server: {1}", SessionInfo(Index_).UserName, gs.ServerName))
                    End If
                Else
                    Select Case packet.Byte
                        Case 1
                            'GS not exitis
                            If Settings.Server_DebugMode Then
                                GlobalManagerCon.Log("gmc err: GS from GlobalManager dosen't existis! user: " & user)
                            End If

                            Server.Disconnect(Index_)
                            Exit Sub
                        Case 2
                            'GS not online
                            If Settings.Server_DebugMode Then
                                GlobalManagerCon.Log("gmc err: GS not online! user: " & user)
                            End If

                            writer.Byte(2) 'fail
                            writer.Byte(2) 'fail subcode
                            writer.Byte(2) 'server in insepction
                        Case 3
                            'GS over capacity
                            If Settings.Server_DebugMode Then
                                GlobalManagerCon.Log("gmc err: GS over capacity! user: " & user)
                            End If

                            writer.Byte(4)
                            writer.Byte(2) 'Server traffic... 
                        Case 4
                            'User not existis!
                            Select Case packet.Byte
                                Case 1
                                    'type = not registered
                                    LoginWriteSpecialText("User does not existis!", Index_)
                                    Exit Sub
                                Case 2
                                    'type = registered
                                    LoginWriteSpecialText(String.Format("A new Account with the ID: {0} was created. You can login in 60 Secounds.", SessionInfo(Index_).UserName), Index_)
                                    Exit Sub
                                Case 3
                                    'type = register fail, in this case maximal account amout per day
                                    LoginWriteSpecialText(String.Format("You can only register {0} Accounts a day!", packet.DWord), Index_)
                                    Exit Sub
                            End Select
                        Case 5
                            'Wrong pw --> failed login

                            writer.Byte(2) 'fail
                            writer.Byte(1) 'wrong pw
                            writer.DWord(packet.DWord) 'number of falied logins
                            writer.DWord(packet.DWord) 'Max Failed Logins
                        Case 6
                            'Banned
                            Dim banreason As String = packet.String(packet.Word)
                            Dim bantime As Date = packet.Date

                            writer.Byte(2) 'failed
                            writer.Byte(2) 'subcode
                            writer.Byte(1) 'ban
                            writer.Word(banreason.Length)
                            writer.String(banreason) 'grund
                            writer.Date(bantime) 'zeit
                    End Select
                End If

                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub
    End Module
End Namespace
