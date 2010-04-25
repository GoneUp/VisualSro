Module Login

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
        writer.Byte(57)

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
            'User exestiert nicht
            writer.Byte(2) 'login failed
            writer.Byte(1)
            writer.Byte(3)
            writer.Word(0)
            writer.Byte(0)
            writer.Byte(1) 'number of falied logins
            writer.Word(0)
            writer.Byte(0)
            LoginServer.Server.Send(writer.GetBytes, index)
        End If

        If Users(UserIndex).Banned = True Then
            LoginServer.Server.Dissconnect(index)

        ElseIf Users(UserIndex).Pw <> Pw Then
            'pw falsch
            Users(UserIndex).FailedLogins += 1
            LoginServer.db.UpdateData("UPDATE user SET failed_logins = '" & Users(UserIndex).FailedLogins & "' WHERE id = '" & Users(UserIndex).Id & "'")

            writer.Byte(2) 'login failed
            writer.Byte(1)
            writer.Byte(3)
            writer.Word(0)
            writer.Byte(0)
            writer.Byte(CByte(Users(UserIndex).FailedLogins)) 'number of falied logins
            writer.Word(0)
            writer.Byte(0)

            LoginServer.Server.Send(writer.GetBytes, index)

            If Users(UserIndex).FailedLogins = 3 Then

                Users(UserIndex).FailedLogins = 0
                LoginServer.db.UpdateData("UPDATE user SET failed_logins = '0' WHERE id = '" & Users(UserIndex).Id & "'")

                LoginServer.Server.Dissconnect(index)
            End If


        ElseIf Users(UserIndex).Name = ID And Users(UserIndex).Pw = Pw Then
            Dim ServerIndex As Integer = GetServerIndexById(serverid)

            If (ServerAcUs(ServerIndex) + 1) > ServerMaxUs(ServerIndex) Then
                writer.Byte(4)
                writer.Byte(2) 'Server traffic... 
                LoginServer.Server.Send(writer.GetBytes, index)

            Else
                'sucess
                writer.Byte(1)
                writer.Byte(242)
                writer.Byte(1)
                writer.Word(0)
                writer.Word(ServerIP(ServerIndex).Length)
                writer.String(ServerIP(ServerIndex))
                writer.Word(ServerPort(ServerIndex))
                LoginServer.Server.Send(writer.GetBytes, index)
            End If




        End If




    End Sub

End Module
