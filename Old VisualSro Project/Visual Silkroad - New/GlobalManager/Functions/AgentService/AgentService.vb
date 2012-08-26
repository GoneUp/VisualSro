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


            Dim tmp As New _UserAuth
            tmp.UserIndex = packet.DWord
            tmp.GameServerId = packet.Word
            tmp.UserName = packet.String(packet.Word)
            tmp.UserPw = packet.String(packet.Word)
            tmp.SessionId = Id_Gen.GetSessionId
            tmp.ExpireTime = Date.Now.AddSeconds(25)

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
                                If CheckBannTime(user) = False Then
                                    writer.Byte(1)
                                    writer.DWord(tmp.SessionId)
                                    UserAuthCache.Add(tmp.SessionId, tmp)
                                Else
                                    'user is banned
                                    writer.Byte(2)
                                    writer.Byte(5)
                                End If
                                'wrong pw
                                user.FailedLogins += 1
                                If user.FailedLogins >= Settings.Server_DebugMode Then
                                Else
                                    writer.Byte(2)
                                    writer.Byte(5)
                                    writer.DWord(user.FailedLogins)
                                End If
                            End If
                        Else
                            'user not existis
                            writer.Byte(2)
                            writer.Byte(4)
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

            Dim tmp As New _UserAuth
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

        Private Function CheckBannTime(ByVal user As cUser) As Boolean
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
#End Region

#Region "Silk"
        Public Sub OnSilk(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If SessionInfo(Index_).Type <> cSessionInfo_GlobalManager._ServerTypes.GameServer Then
                Log.WriteSystemLog("OnGetSilk:: ServerType is wrong!!!")
                If Settings.Server_DebugMode = False Then
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

#Region "User"
        Public Sub OnGetUser(ByVal packet As PacketReader, ByVal Index_ As Integer)
            'Mode 1 = Get
            'Mode 2 = Update
            Dim mode As UInt32 = packet.Byte
            Dim accountID As UInt32 = packet.DWord

            Dim writer As New PacketWriter
            Dim formatter As IFormatter = New BinaryFormatter()

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

    End Module
End Namespace
