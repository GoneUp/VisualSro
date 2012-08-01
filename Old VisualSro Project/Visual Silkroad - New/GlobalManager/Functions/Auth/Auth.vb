Imports GlobalManager.Framework
Imports SRFramework

Namespace Auth
    Module Auth

        Public Function GenarateKey()
            Dim Base As UInt16 = Rnd() * 65535
            Return Base
        End Function

        'Calulation in Client:
        'Base *= 4
        'Base *= Date.Now.DayOfYear
        'Base *= Date.Now.Day

        Public Sub OnVerifyIdentity(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim CalculatedKey As UInt32
            Dim Key As UInt32 = packet.DWord
            Key /= Date.Now.Day
            Key /= Date.Now.DayOfYear
            CalculatedKey = Key / 4

            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.HANDSHAKE)

            If CalculatedKey = SessionInfo(Index_).BaseKey Then
                writer.Byte(1)
                Server.Send(writer.GetBytes, Index_)
                SessionInfo(Index_).HandshakeComplete = True
            Else
                writer.Byte(2)
                Server.Send(writer.GetBytes, Index_)
                Server.Disconnect(Index_)
                Log.WriteSystemLog("Auth failed: " & ClientList.GetIp(Index_))
            End If
        End Sub

        Public Sub OnGateWay(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim tmp As cSessionInfo_GlobalManager = SessionInfo(Index_)
            tmp.ClientName = packet.String(packet.Word)
            tmp.ProtocolVersion = packet.DWord
            tmp.ServerId = packet.Word

            Select Case tmp.ClientName
                Case "GatewayServer"
                    tmp.Type = cSessionInfo_GlobalManager._ServerTypes.GatewayServer
                Case "AgentServer"
                    tmp.Type = cSessionInfo_GlobalManager._ServerTypes.GameServer
                Case "DownloadServer"
                    tmp.Type = cSessionInfo_GlobalManager._ServerTypes.DownloadServer
                Case "AdminTool"
                    tmp.Type = cSessionInfo_GlobalManager._ServerTypes.AdminTool
            End Select

            Dim writer As New PacketWriter
            Dim name As String = "GlobalManager"
            writer.Create(ServerOpcodes.LOGIN_SERVER_INFO)
            writer.Word(name.Length)
            writer.HexString(name)

            If tmp.HandshakeComplete Then
                If tmp.Type <> cSessionInfo_GlobalManager._ServerTypes.Unknown Then
                    If tmp.ProtocolVersion = Settings.Server_ProtocolVersion Then
                        If GlobalDb.CheckServerCert(tmp.ServerId, tmp.ClientName, ClientList.GetIP(Index_).Split(":")(0)) Then
                            writer.Byte(1)
                            tmp.Authorized = True
                        Else
                            writer.Byte(2)
                            writer.Byte(4) 'Server Cert Error
                        End If
                    Else
                        writer.Byte(2)
                        writer.Byte(3) 'Wrong ProtocolVersion
                    End If
                Else
                    writer.Byte(2)
                    writer.Byte(2) 'Wrong Servername
                End If
            Else
                writer.Byte(2)
                writer.Byte(1) 'Handshake Failed!!  
            End If


            Server.Send(writer.GetBytes, Index_)
        End Sub

    End Module
End Namespace
