Imports Framework
Imports LoginServer.Framework

Namespace Functions
    Module Login
        Public Sub ClientInfo(ByVal packet As PacketReader, ByVal Index_ As Integer)
            ClientList.SessionInfo(Index_).Locale = packet.Byte
            ClientList.SessionInfo(Index_).ClientName = packet.String(packet.Word)
            ClientList.SessionInfo(Index_).Version = packet.DWord

            If Settings.Log_Connect Then
                With ClientList.SessionInfo(Index_)
                    Log.WriteGameLog(Index_, "Client_Connect", "(None)", String.Format("Locale: {0}, Name: {1}, Version: {2}", .Locale, .ClientName, .Version))
                End With
            End If
        End Sub

        Public Sub GateWay(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ClientString As String = packet.String(packet.Word)

            If ClientString = "SR_Client" Then
                Dim writer As New PacketWriter
                Dim name As String = "GatewayServer"
                writer.Create(ServerOpcodes.ServerInfo)
                writer.Word(name.Length)
                writer.HexString(name)
                writer.Byte(0)
                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub

        Public Sub SendPatchInfo(ByVal Index_ As Integer)

            'Note: Patch Info for Rsro
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.MassiveMessage)
            writer.Byte(1) 'Header Byte
            writer.Word(1) '1 Data Packet
            writer.Word(&H2005)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.MassiveMessage)
            writer.Byte(0) 'Data
            writer.Word(1)
            writer.Byte(1)
            writer.Byte(8)
            writer.Byte(&HA)
            writer.DWord(5)
            writer.Byte(2)
            Server.Send(writer.GetBytes, Index_)

            '====================================
            writer.Create(ServerOpcodes.MassiveMessage)
            writer.Byte(1) 'Header Byte
            writer.Word(1) '1 Data Packet
            writer.Word(&H6005)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.MassiveMessage)
            writer.Byte(0) 'Data
            writer.Word(3)
            writer.Word(2)
            writer.Byte(2)
            Server.Send(writer.GetBytes, Index_)

            '====================================
            writer.Create(ServerOpcodes.MassiveMessage)
            writer.Byte(1) 'Header Byte
            writer.Word(1) '1 Data Packet
            writer.Word(&HA100)
            Server.Send(writer.GetBytes, Index_)

            writer.Create(ServerOpcodes.MassiveMessage)
            writer.Byte(0) 'Data

            If ClientList.SessionInfo(Index_).Version = Settings.Server_CurrectVersion Then
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
            ElseIf ClientList.SessionInfo(Index_).Version > Settings.Server_CurrectVersion Then
                'Client too new 
                writer.Byte(2)
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
                Server.Dissconnect(Index_)
            ElseIf ClientList.SessionInfo(Index_).Version < Settings.Server_CurrectVersion Then
                'Client too old 
                writer.Byte(2)
                writer.Byte(5)
                Server.Send(writer.GetBytes, Index_)
                Server.Dissconnect(Index_)
            ElseIf ClientList.SessionInfo(Index_).Locale <> Settings.Server_Local Then
                'Wrong Local
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
                Server.Dissconnect(Index_)
            ElseIf ClientList.SessionInfo(Index_).ClientName <> "SR_Client" Then
                'Wrong Clientname
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
                Server.Dissconnect(Index_)
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

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.MassiveMessage)
            writer.Byte(1) 'Header Byte
            writer.Word(1) '1 Data Packet
            writer.Word(&HA104)
            Server.Send(writer.GetBytes, Index_)



            writer.Create(ServerOpcodes.MassiveMessage)
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

            Dim NameServer As String = "SRO_Russia_Official"
            Dim writer As New PacketWriter

            writer.Create(ServerOpcodes.ServerList)
            writer.Byte(1) 'Nameserver
            writer.Byte(57) 'Nameserver ID
            writer.Word(NameServer.Length)
            writer.String(NameServer)
            writer.Byte(0) 'Out of nameservers


            For i = 0 To LoginDb.Servers.Count - 1
                writer.Byte(1) 'New Gameserver
                writer.Word(LoginDb.Servers(i).ServerId)
                writer.Word(LoginDb.Servers(i).Name.Length)
                writer.String(LoginDb.Servers(i).Name)
                writer.Word(LoginDb.Servers(i).AcUs)
                writer.Word(LoginDb.Servers(i).MaxUs)
                writer.Byte(LoginDb.Servers(i).State)
            Next

            writer.Byte(0)

            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub HandleLogin(ByVal packet As PacketReader, ByVal Index_ As Integer)

            Dim LoginMethod As Byte = packet.Byte()
            Dim ID As String = packet.String(packet.Word).ToLower
            Dim Pw As String = packet.String(packet.Word).ToLower
            Dim ServerID As Integer = packet.Word
            Dim writer As New PacketWriter

            writer.Create(ServerOpcodes.LoginAuthInfo)
            Dim UserIndex As Integer = LoginDb.GetUserWithID(ID)

            If UserIndex = -1 Then
                'User exestiert nicht == We register a User

                If Settings.Auto_Register = True Then
                    If CheckIfUserCanRegister(ClientList.GetIP(Index_)) = True Then
                        If RegisterUser(ID, Pw, Index_) Then
                            Login_WriteSpecialText(String.Format("A new Account with the ID: {0} and Password: {1}. You can login in 60 Secounds.", ID, Pw), Index_)
                        End If

                    Else
                        Login_WriteSpecialText(String.Format("You can only register {0} Accounts a day!", Settings.Max_RegistersPerDay), Index_)
                    End If


                ElseIf Settings.Auto_Register = False Then
                    'Normal Fail
                    Login_WriteSpecialText("User does not existis!", Index_)
                End If

            Else
                CheckBannTime(UserIndex)

                If LoginDb.Users(UserIndex).Banned = True Then

                    writer.Byte(2) 'failed
                    writer.Byte(2) 'gebannt
                    writer.Byte(1) 'unknown
                    writer.Word(LoginDb.Users(UserIndex).BannReason.Length)
                    writer.String(LoginDb.Users(UserIndex).BannReason) 'grund
                    writer.Word(LoginDb.Users(UserIndex).BannTime.Year) 'jahr
                    writer.Word(LoginDb.Users(UserIndex).BannTime.Month) 'monat
                    writer.Word(LoginDb.Users(UserIndex).BannTime.Day) 'tag
                    writer.Word(LoginDb.Users(UserIndex).BannTime.Hour) 'stunde
                    writer.Word(LoginDb.Users(UserIndex).BannTime.Minute) 'minute
                    writer.Word(LoginDb.Users(UserIndex).BannTime.Second) 'sekunde
                    writer.DWord(LoginDb.Users(UserIndex).BannTime.Millisecond) 'tag

                    Server.Send(writer.GetBytes, Index_)


                ElseIf LoginDb.Users(UserIndex).Pw <> Pw Then
                    'pw falsch
                    Dim user As LoginDb.UserArray = LoginDb.Users(UserIndex)
                    user.FailedLogins += 1
                    LoginDb.Users(UserIndex) = user

                    Framework.DataBase.SaveQuery(String.Format("UPDATE users SET failed_logins = '{0}' WHERE id = '{1}'", user.FailedLogins, user.AccountId))

                    writer.Byte(2) 'login failed
                    writer.Byte(1)
                    writer.DWord(Settings.Max_FailedLogins) 'Max Failed Logins
                    writer.DWord(user.FailedLogins) 'number of falied logins
                    Server.Send(writer.GetBytes, Index_)

                    If user.FailedLogins >= Settings.Max_FailedLogins Then
                        user.FailedLogins = 0
                        LoginDb.Users(UserIndex) = user

                        Framework.DataBase.SaveQuery(String.Format("UPDATE users SET failed_logins = '0' WHERE id = '{0}'", user.AccountId))
                        BanUser(Date.Now.AddMinutes(10), UserIndex) 'Ban for 10 mins
                    End If



                ElseIf LoginDb.Users(UserIndex).Name = ID And LoginDb.Users(UserIndex).Pw = Pw Then
                    Dim ServerIndex As Integer = LoginDb.GetServerIndexById(ServerID)

                    If (LoginDb.Servers(ServerIndex).AcUs + 1) >= LoginDb.Servers(ServerIndex).MaxUs Then
                        writer.Byte(4)
                        writer.Byte(2) 'Server traffic... 
                        Server.Send(writer.GetBytes, Index_)

                    Else
                        'Sucess!
                        writer.Byte(1)
                        writer.DWord(GetKey(Index_))
                        writer.Word(LoginDb.Servers(ServerIndex).IP.Length)
                        writer.String(LoginDb.Servers(ServerIndex).IP)
                        writer.Word(LoginDb.Servers(ServerIndex).Port)
                        Server.Send(writer.GetBytes, Index_)

                        If Settings.Log_Login Then
                            Log.WriteGameLog(Index_, "Login", "Sucess", String.Format("Name: {0}, Server: {1}", ID, LoginDb.Servers(ServerIndex).Name))
                        End If
                    End If
                End If
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
