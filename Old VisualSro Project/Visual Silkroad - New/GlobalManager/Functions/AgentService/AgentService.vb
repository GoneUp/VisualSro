Imports SRFramework
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Agent
    Module AgentService

#Region "UserAuth"
        Public Sub OnUserAuth(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If SessionInfo(Index_).Type <> cSessionInfo_GlobalManager._ServerTypes.GatewayServer Then
                Log.WriteSystemLog("OnSendUserAuth:: ServerType is wrong!!!")
                If Settings.Server_DebugMode = False Then
                    Server.Disconnect(Index_)
                End If
            End If


            Dim tmp As New UserAuth
            tmp.UserIndex = packet.DWord
            tmp.GameServerId = packet.Word
            tmp.UserName = packet.String(packet.Word)
            tmp.UserPw = packet.String(packet.Word)
            tmp.UserIp = packet.String(packet.Word)

            tmp.SessionId = Id_Gen.GetSessionId
            tmp.ExpireTime = Date.Now.AddSeconds(30)

            Dim user As cUser = GlobalDB.GetUser(tmp.UserName)

            'Checks now 
            Dim writer As New PacketWriter
            writer.Create(InternalServerOpcodes.AGENT_SEND_USERAUTH)
            writer.DWord(tmp.UserIndex)

            If Shard.Server_Game.ContainsKey(tmp.GameServerId) Then
                If Shard.Server_Game(tmp.GameServerId).State = GameServer._ServerState.Online Then
                    If Shard.Server_Game(tmp.GameServerId).OnlineClients + 1 < Server.MaxNormalClients Then
                        If user IsNot Nothing Then
                            If user.Pw = tmp.UserPw Then
                                If UserService.CheckBannTime(user) = False Then
                                    writer.Byte(1)
                                    writer.DWord(tmp.SessionId)
                                    UserAuthCache.Add(tmp.SessionId, tmp)
                                Else
                                    'user is banned
                                    writer.Byte(2)
                                    writer.Byte(5)

                                    writer.Byte(1) 'type for the client -- unknown
                                    writer.Word(user.BannReason.Length)
                                    writer.String(user.BannReason) 'grund
                                    writer.Date(user.BannTime) 'zeit
                                End If

                                'wrong pw
                                user.FailedLogins += 1

                                If user.FailedLogins >= Settings.Server_DebugMode Then
                                    '10 min ban
                                    user.FailedLogins = 0
                                    UserService.BanUserForFailedLogins(Date.Now.AddMinutes(15), user)
                                Else
                                    writer.Byte(2)
                                    writer.Byte(5)
                                    writer.DWord(user.FailedLogins)

                                    DBSave.SaveFailedLogins(user)
                                    GlobalDB.UpdateUser(user)
                                End If
                            End If
                        Else
                            'User not existis!
                            'Then we should look for autoregister
                            If Settings.Agent_Auto_Register Then
                                If UserService.CheckIfUserCanRegister(Server.ClientList.GetIP(Index_)) = True Then
                                    If UserService.RegisterUser(tmp.UserName, tmp.UserPw, tmp.UserIndex, Index_) Then
                                        UserService.LoginWriteSpecialText(String.Format("A new Account with the ID: {0} and Password: {1}. You can login in 60 Secounds.", tmp.UserName, tmp.UserPw), tmp.UserIndex, Index_)
                                    End If

                                Else
                                    UserService.LoginWriteSpecialText(String.Format("You can only register {0} Accounts a day!", Settings.Agent_Max_RegistersPerDay), tmp.UserIndex, Index_)
                                End If

                            Else
                                writer.Byte(2)
                                writer.Byte(4)
                                writer.Byte(1) 'type = not registered
                            End If
                        End If
                    Else
                        'gs over capacity
                        writer.Byte(2)
                        writer.Byte(3)
                    End If
                Else
                    'gs not online
                    writer.Byte(2)
                    writer.Byte(2)
                End If
            Else
                'no gs
                writer.Byte(2)
                writer.Byte(1)
            End If

            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnCheckUserAuth(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If SessionInfo(Index_).Type <> cSessionInfo_GlobalManager._ServerTypes.GameServer Then
                Log.WriteSystemLog("OnCheckUserAuth:: ServerType is wrong!!!")
                If Settings.Server_DebugMode = False Then
                    Server.Disconnect(Index_)
                End If
            End If

            Dim tmp As New UserAuth
            tmp.UserIndex = packet.DWord
            tmp.SessionId = packet.DWord
            tmp.UserName = packet.String(packet.Word)
            tmp.UserPw = packet.String(packet.Word)

            Dim writer As New PacketWriter
            writer.Create(InternalServerOpcodes.AGENT_CHECK_USERAUTH)
            writer.DWord(tmp.UserIndex)

            If UserAuthCache.ContainsKey(tmp.SessionId) Then
                If UserAuthCache(tmp.SessionId).UserName = tmp.UserName And UserAuthCache(tmp.SessionId).UserPw = tmp.UserPw Then
                    writer.Byte(1)
                Else
                    writer.Byte(2) 'Fail
                    writer.Byte(2) 'Wrong Id/Pw
                End If
            Else
                writer.Byte(2) 'Fail
                writer.Byte(1) 'Wrong SessionId
            End If

            Server.Send(writer.GetBytes, Index_)

        End Sub


#End Region

    End Module
End Namespace
