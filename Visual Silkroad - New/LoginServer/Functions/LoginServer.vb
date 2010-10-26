Namespace LoginServer.Functions
    Module Login


        Public Sub ClientInfo(ByVal packet As LoginServer.PacketReader)
            Dim locale As Byte = packet.Byte
            Dim clientname As String = packet.String(packet.Word)
            Dim version As UInt32 = packet.DWord

            Debug.Print(String.Format("[Client Info][Locale: {0}][Name: {1}][Version: {2}]", locale, clientname, version))

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

            LoginServer.Server.Send(New Byte() {5, 0, 13, 96, 0, 0, 1, 1, 0, 4, 161}, index)

            Dim numberofnews As Integer = LoginDb.NewsNumber

            Dim writer As New LoginServer.PacketWriter
            writer.Create(ServerOpcodes.PatchInfo)
            writer.Byte(0)
            writer.Byte(numberofnews) 'nummer of news

            For i = 0 To numberofnews - 1
                writer.Word(LoginDb.NewsTitle(i).Length)
                writer.String(LoginDb.NewsTitle(i))

                writer.Word(LoginDb.NewsText(i).Length)
                writer.String(LoginDb.NewsText(i))

                writer.Word(2010) 'jahr
                writer.Byte(LoginDb.NewsMonth(i))
                writer.Byte(0)
                writer.Byte(LoginDb.NewsDay(i))
                writer.Byte(0)

                'UNBEKANNT 15 00 01 00  03 00 00 EC  60 21 /10 unbekannt   
                writer.Word(15)
                writer.Byte(1)
                writer.Byte(0)
                writer.Byte(3)
                writer.Word(0)
                writer.Byte(236)
                writer.Byte(96)
                writer.Byte(21)
            Next


            LoginServer.Server.Send(writer.GetBytes, index)


        End Sub

        Public Sub SendServerList(ByVal index As Integer)

            Dim name As String = "SRO_Russia_Official"
            Dim writer As New LoginServer.PacketWriter
            Dim ServerNumber As Integer = LoginDb.ServerNumber

            writer.Create(ServerOpcodes.ServerList)
            writer.Byte(1)
            writer.Byte(57) 'locale

            writer.Word(name.Length)
            writer.String(name)

            writer.Byte(0)


            For i = 0 To ServerNumber - 1

                writer.Byte(1) 'new =1 old=0

                writer.Word(LoginDb.ServerID(i))
                writer.Word(LoginDb.ServerName(i).Length)
                writer.String(LoginDb.ServerName(i))
                writer.Word(LoginDb.ServerAcUs(i))
                writer.Word(LoginDb.ServerMaxUs(i))
                writer.Byte(LoginDb.ServerState(i))
                'writer.Byte(1)

            Next

            writer.Byte(0)

            LoginServer.Server.Send(writer.GetBytes, index)


        End Sub

        Public Sub HandleLogin(ByVal packet As LoginServer.PacketReader, ByVal index As Integer)

            packet.Byte()
            Dim IdLength As Integer = packet.Word
            Dim ID As String = packet.String(IdLength)
            Dim PwLength As Integer = packet.Word
            Dim Pw As String = packet.String(PwLength)
            Dim serverid As Integer = packet.Word

            Dim writer As New LoginServer.PacketWriter
            writer.Create(ServerOpcodes.LoginAuthInfo)

            Dim UserIndex As Integer = GetUserWithID(ID)

            If UserIndex = -1 Then
                'User exestiert nicht == We register a User

                If Settings.AutoRegister = True Then
                    RegisterUser(ID, Pw)
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

                Server.Send(writer.GetBytes, index)
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
                    writer.DWord(0) 'unknwon
                    writer.DWord(0) 'unknwon
                    writer.Word(0) 'unknwon
                    LoginServer.Server.Send(writer.GetBytes, index)


                ElseIf Users(UserIndex).Pw <> Pw Then
                    'pw falsch
                    Dim user As UserArray = Users(UserIndex)
                    user.FailedLogins += 1


                    Database.UpdateData("UPDATE users SET failed_logins = '" & Users(UserIndex).FailedLogins & "' WHERE id = '" & Users(UserIndex).Id & "'")

                    writer.Byte(2) 'login failed
                    writer.Byte(1)
                    writer.Byte(3)
                    writer.Word(0)
                    writer.Byte(0)
                    writer.Byte(user.FailedLogins) 'number of falied logins
                    writer.Word(0)
                    writer.Byte(0)

                    LoginServer.Server.Send(writer.GetBytes, index)

                    If user.FailedLogins = 3 Then
                        user.FailedLogins = 0
                        Database.UpdateData("UPDATE users SET failed_logins = '0' WHERE id = '" & Users(UserIndex).Id & "'")
                        Server.Dissconnect(index)
                    End If

                    Users(UserIndex) = user

                ElseIf Users(UserIndex).Name = ID And Users(UserIndex).Pw = Pw Then
                    Dim ServerIndex As Integer = GetServerIndexById(serverid)

                    If (ServerAcUs(ServerIndex) + 1) > ServerMaxUs(ServerIndex) Then
                        writer.Byte(4)
                        writer.Byte(2) 'Server traffic... 
                        LoginServer.Server.Send(writer.GetBytes, index)

                    Else
                        'sucess

                        Dim sock As Net.Sockets.Socket = LoginServer.ClientList.GetSocket(index)
                        Dim endpoint = sock.RemoteEndPoint
                        Dim split1 As String() = endpoint.ToString.Split(":")
                        Dim split2 As String() = split1(0).Split(".")
                        Dim key As UInt32 = CUInt(split2(0)) + CUInt(split2(1)) + CUInt(split2(2)) + CUInt(split2(3))

                        writer.Byte(1)
                        writer.DWord(key)
                        writer.Word(ServerIP(ServerIndex).Length)
                        writer.String(ServerIP(ServerIndex))
                        writer.Word(ServerPort(ServerIndex))
                        LoginServer.Server.Send(writer.GetBytes, index)
                    End If
                End If
            End If
        End Sub

        Private Sub RegisterUser(ByVal Name As String, ByVal Password As String)
            Database.InsertData(String.Format("INSERT INTO users(username, password) VALUE ('{0}','{1}')", Name, Password))

            Dim tmpUser As New UserArray

            UserIdCounter += 1
            tmpUser.Id = UserIdCounter
            tmpUser.Name = Name
            tmpUser.Pw = Password
            tmpUser.FailedLogins = 0
            tmpUser.Banned = True
            tmpUser.BannReason = "Please wait for Activation"
            tmpUser.BannTime = Date.Now.AddMinutes(2)

            Users.Add(tmpUser)
        End Sub


        Public Sub CheckBannTime(ByVal UserIndex As Integer)
            Dim user As UserArray = Users(UserIndex)
            Try


                If user.Banned = True Then
                    Dim wert As Integer = Date.Compare(user.BannTime, Date.Now)
                    If wert = -1 Then
                        'Zeit abgelaufen
                        user.Banned = False
                        Database.UpdateData(String.Format("UPDATE users SET banned = '0' WHERE id = '{0}'", user.Id))

                        Users(UserIndex) = user
                    End If
                End If

            Catch ex As Exception
                WriteLog("[BAN_CHECK][ID:" & user.Id & "][NAME:" & user.Name & "][TIME:" & user.BannTime.ToLongTimeString & "]")
            End Try
        End Sub
    End Module
End Namespace
