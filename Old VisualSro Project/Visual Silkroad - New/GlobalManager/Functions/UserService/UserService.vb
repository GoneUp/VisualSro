Imports SRFramework
Imports System.Runtime.Serialization

Namespace UserService
    Module UserService

#Region "User"
        Public Sub OnGetUser(ByVal packet As PacketReader, ByVal Index_ As Integer)
            'Mode 1 = Get
            'Mode 2 = Update
            Dim mode As UInt32 = packet.Byte
            Dim accountID As UInt32 = packet.DWord

            Dim writer As New PacketWriter
            Dim formatter As IFormatter = New Formatters.Binary.BinaryFormatter()

            Dim user As cUser = GlobalDB.GetUser(accountID)

            If user Is Nothing Then
                writer.Byte(2) 'failed
                writer.Byte(1) 'user not exitis
                Server.Send(writer.GetBytes, Index_)
                Exit Sub

            Else
                Select Case mode
                    Case 1 'get
                        writer.Byte(1)
                        formatter.Serialize(writer.BaseStream, user)
                        writer.Byte(255) 'only for testing

                    Case 2 'update
                        Try
                            Dim new_user As cUser = formatter.Deserialize(writer.BaseStream)

                            If new_user IsNot Nothing AndAlso user.AccountId = new_user.AccountId Then
                                writer.Byte(1)

                                GlobalDB.UpdateUser(new_user)
                                DBSave.SaveUser(new_user)
                            Else
                                writer.Byte(2) 'fail
                                writer.Byte(3) 'object failure
                            End If
                        Catch ex As Exception
                            writer.Byte(2) 'fail
                            writer.Byte(2) 'deserialize
                        End Try
                End Select
            End If

            Server.Send(writer.GetBytes, Index_)
        End Sub

#End Region

#Region "Ban"
        Public Function CheckBannTime(ByVal user As cUser) As Boolean
            Try
                If user.Banned Then
                    Dim wert As Integer = Date.Compare(user.BannTime, Date.Now)
                    If wert = -1 Then
                        'Zeit abgelaufen
                        user.Banned = False

                        GlobalDB.UpdateUser(user)
                        DBSave.SaveUserBan(user)
                    Else
                        Return True
                    End If
                End If

            Catch ex As Exception
                Log.WriteSystemLog("[BAN_CHECK][ID:" & user.AccountId & "][NAME:" & user.Name & "][TIME:" & user.BannTime.ToLongTimeString & "]")
            End Try

            Return False
        End Function

        Public Sub BanUserForFailedLogins(ByVal expireTime As Date, ByVal user As cUser)
            Try
                user.Banned = True
                user.BannTime = expireTime
                user.BannReason = (String.Format("You got banned for {0} Minutes because of {1} failed Logins.", DateDiff(DateInterval.Minute, Date.Now, expireTime), Settings.AgentMaxFailedLogins))

                DBSave.SaveUserBan(user)
                GlobalDB.UpdateUser(user)
            Catch ex As Exception
                Log.WriteSystemLog("[BAN_USER][ID:" & user.AccountId & "][NAME:" & user.Name & "][TIME:" & user.BannTime.ToLongTimeString & "]")
            End Try
        End Sub
#End Region

#Region "Register"
        ''' <summary>
        ''' Registers a User
        ''' </summary>
        ''' <param name="Name"></param>
        ''' <param name="Password"></param>
        ''' <param name="Index_"></param>
        ''' <returns>True at success, False on fail.</returns>
        ''' <remarks></remarks>
        Public Function RegisterUser(ByVal name As String, ByVal password As String, userIndex As Integer, ByVal Index_ As Integer)
            If name.Contains("ä") Or name.Contains("ü") Or name.Contains("ö") Or password.Contains("ä") Or password.Contains("ü") Or password.Contains("ö") Then
                LoginWriteSpecialText("Please don't enter any Chars like ä, ö, ü.", userIndex, Index_)
                Return False
            ElseIf name.Length < 6 Or name.Length < 16 Then
                LoginWriteSpecialText("The account name length must be between 6 and 16 characters.", userIndex, Index_)
                Return False
            ElseIf password.Length < 6 Or password.Length < 16 Then
                LoginWriteSpecialText("The account password length must be between 6 and 16 characters.", userIndex, Index_)
                Return False
            End If

            'Security ;)
            name = cDatabase.CheckForInjection(name)
            password = cDatabase.CheckForInjection(password)

            'Insert the new User
            Database.SaveQuery(String.Format("INSERT INTO users(username, password) VALUE ('{0}','{1}')", name, password))

            'Add to GlobalDB
            Dim tmp As New cUser
            tmp.AccountId = Id_Gen.GetNewAccountId
            tmp.Name = name
            tmp.Pw = password
            tmp.FailedLogins = 0
            tmp.Banned = True
            tmp.BannReason = "Please wait for Activation."
            tmp.BannTime = Date.Now.AddMinutes(2)
            GlobalDB.Users.Add(tmp)

            'Add to RegisterList
            Dim tmp_2 As New cRegisteredUsed
            tmp_2.IP = Server.ClientList.GetIP(Index_)
            tmp_2.Name = name
            tmp_2.Password = password
            tmp_2.Time = Date.Now
            RegisterList.Add(tmp_2)

            'Log it
            If Settings.LogRegister Then
                Log.WriteGameLog(Index_, Server.ClientList.GetIP(Index_), "Register", "(None)", String.Format("Name: {0}, Password: {1}", cDatabase.CheckForInjection(name), cDatabase.CheckForInjection(password)))
            End If

            Return True
        End Function

        Public Function CheckIfUserCanRegister(ByVal IP As String) As Boolean
            Dim count As Integer = 0

            For i = 0 To RegisterList.Count - 1
                If RegisterList(i).IP = IP And Date.Now.DayOfYear = RegisterList(i).Time.DayOfYear Then
                    count += 1
                End If
            Next

            If count >= Settings.AgentMaxRegistersPerDay Then
                Return False
            End If

            Return True
        End Function
#End Region

#Region "Login Message"
        Public Sub LoginWriteSpecialText(ByVal text As String, userIndex As Integer, ByVal Index_ As Integer)
            'Send a special message to a login client, will be displayed instantly
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.LOGIN_AUTH)
            writer.DWord(userIndex)
            writer.Word(text.Length)
            writer.String(text)
            Server.Send(writer.GetBytes, Index_)
        End Sub
#End Region

#Region "Silk"
        Public Sub OnSilk(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If SessionInfo(Index_).Type <> cSessionInfo_GlobalManager._ServerTypes.GameServer Then
                Log.WriteSystemLog("OnGetSilk:: ServerType is wrong!!!")
                If Settings.ServerDebugMode = False Then
                    Server.Disconnect(Index_)
                End If
            End If

            'Mode 1 = GetSilk
            'Mode 2 = UpdateSkilk
            Dim mode As UInt32 = packet.Byte
            Dim accountID As UInt32 = packet.DWord


            Dim writer As New PacketWriter
            Dim user As cUser = GlobalDB.GetUser(accountID)

            writer.Create(InternalServerOpcodes.AGENT_SILK)
            writer.DWord(accountID)
            writer.Byte(mode)

            If user Is Nothing Then
                writer.Byte(2) 'failed
                writer.Byte(1) 'user not exitis
                Server.Send(writer.GetBytes, Index_)
                Exit Sub

            Else
                Select Case mode
                    Case 1 'get
                        writer.Byte(1)
                        writer.DWord(user.Silk)
                        writer.DWord(user.Silk_Bonus)
                        writer.DWord(user.Silk_Points)
                    Case 2 'update
                        user.Silk = packet.DWord
                        user.Silk_Bonus = packet.DWord
                        user.Silk_Points = packet.DWord

                        DBSave.SaveUserSilk(user)

                        writer.Byte(1)
                End Select
            End If

            Server.Send(writer.GetBytes, Index_)
        End Sub
#End Region

    End Module
End Namespace
