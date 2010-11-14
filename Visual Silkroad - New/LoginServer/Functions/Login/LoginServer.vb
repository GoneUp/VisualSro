Namespace LoginServer.Functions
    Module Login


        Public Sub ClientInfo(ByVal packet As LoginServer.PacketReader, ByVal Index_ As Integer)
            Dim locale As Byte = packet.Byte
            Dim clientname As String = packet.String(packet.Word)
            Dim version As UInt32 = packet.DWord

            Log.WriteGameLog(Index_, "Client_Connect", "(None)", String.Format("Locale: {0}, Name: {1}, Version: {2}", locale, clientname, version))
        End Sub

        Public Sub GateWay(ByVal index As Integer)
            Dim writer As New LoginServer.PacketWriter
            Dim name As String = "GatewayServer"
            writer.Create(ServerOpcodes.ServerInfo)
            writer.Word(name.Length)
            writer.HexString(name)
            writer.Byte(0)
            LoginServer.Server.Send(writer.GetBytes, index)
        End Sub

        Public Sub SendPatchInfo(ByVal index As Integer)

            'Note: Patch Info for Rsro

            LoginServer.Server.Send(New Byte() {5, 0, 13, 96, 0, 0, 1, 1, 0, 5, 32, _
                                                11, 0, 13, 96, 0, 0, 0, 1, 0, 1, 9, 10, 5, 0, 0, 0, 2, _
                                                5, 0, 13, 96, 0, 0, 1, 1, 0, 5, 96, _
                                                6, 0, 13, 96, 0, 0, 0, 3, 0, 2, 0, 2, _
                                                5, 0, 13, 96, 0, 0, 1, 1, 0, 0, 161, _
                                                2, 0, 13, 96, 0, 0, 0, 1}, index)


        End Sub

        Public Sub SendLauncherInfo(ByVal index As Integer)

            Dim writer As New LoginServer.PacketWriter
            writer.Create(ServerOpcodes.PatchInfo)
            writer.Byte(1) 'Launcher News
            writer.Word(1) '1 Data Packet
            writer.Word(&HA104)
            Server.Send(writer.GetBytes, index)



            writer.Create(ServerOpcodes.PatchInfo)
            writer.Byte(0)
            writer.Byte(LoginDb.News.Count) 'nummer of news

            For i = 0 To LoginDb.News.Count - 1
                writer.Word(LoginDb.News(i).NewsTitle.Length)
                writer.String(LoginDb.News(i).NewsTitle.Length)

                writer.Word(LoginDb.News(i).NewsText.Length)
                writer.String(LoginDb.News(i).NewsText.Length)

                writer.Word(2010) 'jahr
                writer.Word(LoginDb.News(i).NewsMonth)
                writer.Word(LoginDb.News(i).NewsDay)
                writer.Word(0) 'hour
                writer.Word(0) 'minute
                writer.Word(0) 'secound
                writer.DWord(0) 'millisecond
            Next


            Server.Send(writer.GetBytes, index)


        End Sub

        Public Sub SendServerList(ByVal index As Integer)

            Dim NameServer As String = "SRO_Russia_Official"
            Dim writer As New PacketWriter

            writer.Create(ServerOpcodes.ServerList)
            writer.Byte(1) 'Nameserver
            writer.Byte(57) 'Nameserver ID
            writer.Word(NameServer.Length)
            writer.String(NameServer)
            writer.Byte(0) 'Out of nameservers


            For i = 0 To Servers.Count - 1
                writer.Byte(1) 'New Gameserver
                writer.Word(Servers(i).ServerId)
                writer.Word(Servers(i).Name.Length)
                writer.String(Servers(i).Name)
                writer.Word(Servers(i).AcUs)
                writer.Word(Servers(i).MaxUs)
                writer.Byte(Servers(i).State)
            Next

            writer.Byte(0)

            LoginServer.Server.Send(writer.GetBytes, index)


        End Sub

        Public Sub HandleLogin(ByVal packet As LoginServer.PacketReader, ByVal Index_ As Integer)

            Dim LoginMethod As Byte = packet.Byte()
            Dim ID As String = packet.String(packet.Word)
            Dim Pw As String = packet.String(packet.Word)
            Dim ServerID As Integer = packet.Word
            Dim writer As New LoginServer.PacketWriter

            writer.Create(ServerOpcodes.LoginAuthInfo)
            Dim UserIndex As Integer = GetUserWithID(ID)

            If UserIndex = -1 Then
                'User exestiert nicht == We register a User

                If Settings.AutoRegister = True Then
                    RegisterUser(ID, Pw, Index_)
                    Dim reason As String = String.Format("A new Account with the ID: {0} and Password: {1}. You can login in 60 Secounds.", ID, Pw)
                    writer.Byte(2) 'failed
                    writer.Byte(2) 'gebannt
                    writer.Byte(1) 'unknown
                    writer.Word(reason.Length)
                    writer.String(reason) 'grund
                    writer.Word(2012) 'jahr
                    writer.Word(12) 'monat
                    writer.Word(12) 'tag
                    writer.DWord(0) 'unknwon
                    writer.DWord(0) 'unknwon
                    writer.Word(0) 'unknwon

                ElseIf AutoRegister = False Then
                    'Normal Fail
                    writer.Byte(2) 'login failed
                    writer.Byte(1)
                    writer.Byte(3)
                    writer.Word(0)
                    writer.Byte(0)
                    writer.Byte(1) 'number of falied logins
                    writer.Word(0)
                    writer.Byte(0)
                End If

                Server.Send(writer.GetBytes, Index_)
            Else
                CheckBannTime(UserIndex)

                If Users(UserIndex).Banned = True Then

                    writer.Byte(2) 'failed
                    writer.Byte(2) 'gebannt
                    writer.Byte(1) 'unknown
                    writer.Word(Users(UserIndex).BannReason.Length)
                    writer.String(Users(UserIndex).BannReason) 'grund
                    writer.Word(Users(UserIndex).BannTime.Year) 'jahr
                    writer.Word(Users(UserIndex).BannTime.Month) 'monat
                    writer.Word(Users(UserIndex).BannTime.Day) 'tag
                    writer.Word(Users(UserIndex).BannTime.Hour) 'tag
                    writer.Word(Users(UserIndex).BannTime.Minute) 'tag
                    writer.DWord(Users(UserIndex).BannTime.Millisecond) 'tag

                    Server.Send(writer.GetBytes, Index_)


                ElseIf Users(UserIndex).Pw <> Pw Then
                    'pw falsch
                    Dim user As UserArray = Users(UserIndex)
                    user.FailedLogins += 1
                    Users(UserIndex) = user

                    Database.UpdateData(String.Format("UPDATE users SET failed_logins = '{0}' WHERE id = '{1}'", user.FailedLogins, user.AccountId))

                    writer.Byte(2) 'login failed
                    writer.Byte(1)
                    writer.DWord(MaxFailedLogins) 'Max Failed Logins
                    writer.DWord(user.FailedLogins) 'number of falied logins
                    Server.Send(writer.GetBytes, Index_)

                    If user.FailedLogins >= MaxFailedLogins Then
                        user.FailedLogins = 0
                        Users(UserIndex) = user

                        Database.UpdateData(String.Format("UPDATE users SET failed_logins = '0' WHERE id = '{0}'", user.AccountId))
                        BanUser(Date.Now.AddMinutes(10), UserIndex) 'Ban for 10 mins
                    End If



                ElseIf Users(UserIndex).Name = ID And Users(UserIndex).Pw = Pw Then
                    Dim ServerIndex As Integer = GetServerIndexById(ServerID)

                    If (Servers(ServerIndex).AcUs + 1) >= Servers(ServerIndex).MaxUs Then
                        writer.Byte(4)
                        writer.Byte(2) 'Server traffic... 
                        Server.Send(writer.GetBytes, Index_)

                    Else
                        'Sucess!
                        writer.Byte(1)
                        writer.DWord(GetKey(Index_))
                        writer.Word(Servers(ServerIndex).IP.Length)
                        writer.String(Servers(ServerIndex).IP)
                        writer.Word(Servers(ServerIndex).Port)
                        Server.Send(writer.GetBytes, Index_)

                        Log.WriteGameLog(Index_, "Login", "Sucess", String.Format("Name: {0}, Server: {1}", ID, Servers(ServerIndex).Name))
                    End If
                End If
            End If
        End Sub

        Private Sub RegisterUser(ByVal Name As String, ByVal Password As String, ByVal Index_ As Integer)
            Database.InsertData(String.Format("INSERT INTO users(username, password) VALUE ('{0}','{1}')", Name, Password))

            Dim tmpUser As New UserArray
            Dim r As New Random

            tmpUser.AccountId = r.Next(500000, 1000000)
            tmpUser.Name = Name
            tmpUser.Pw = Password
            tmpUser.FailedLogins = 0
            tmpUser.Banned = True
            tmpUser.BannReason = "Please wait for Activation."
            tmpUser.BannTime = Date.Now.AddMinutes(2)

            Users.Add(tmpUser)

            Log.WriteGameLog(Index_, "Register", "(None)", String.Format("Name: {0}, Password: {1}", Name, Password))
        End Sub

        Private Function GetKey(ByVal Index_ As Integer) As UInt32
            Dim split1 As String() = ClientList.GetSocket(Index_).RemoteEndPoint.ToString.Split(":")
            Dim split2 As String() = split1(0).Split(".")
            Dim key As UInt32 = CUInt(split2(0)) + CUInt(split2(1)) + CUInt(split2(2)) + CUInt(split2(3))
            Return key
        End Function
    End Module
End Namespace
