Imports GlobalManager.Framework

Namespace Auth
    Module Auth

        Public Function GenarateKey()
            Dim Base As UInt16 = Rnd() * 65535
            Return Base
        End Function

        'Calulation in Client:
        'Base *= 1.1
        'Base *= Date.Now.DayOfYear
        'Base *= Date.Now.DayOfWeek

        Public Sub OnVerifyIdentity(ByVal packet As Framework.PacketReader, ByVal Index_ As Integer)
            Dim CalculatedKey As UInt32
            Dim Key As UInt32 = packet.DWord
            Key /= Date.Now.DayOfWeek
            Key /= Date.Now.DayOfYear
            CalculatedKey = Key / 1.1

            Dim writer As New Framework.PacketWriter
            writer.Create(ServerOpcodes.Handshake)

            If CalculatedKey = Framework.ClientList.SessionInfo(Index_).BaseKey Then
                writer.Byte(True)
                Server.Send(writer.GetBytes, Index_)
                ClientList.SessionInfo(Index_).HandshakeComplete = True
            Else
                writer.Byte(2)
                Server.Send(writer.GetBytes, Index_)
                Server.Dissconnect(Index_)
                Log.WriteSystemLog("Auth failed: " & ClientList.GetIP(Index_))
            End If
        End Sub

        Public Sub OnGateWay(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim tmp As _SessionInfo = ClientList.SessionInfo(Index_)
            tmp.ClientName = packet.String(packet.Word)
            tmp.ProtocolVersion = packet.DWord
            tmp.ServerId = packet.Word

            Select Case tmp.ClientName
                Case "GatewayServer"
                    tmp.Type = _SessionInfo._ServerTypes.GatewayServer
                Case "AgentServer"
                    tmp.Type = _SessionInfo._ServerTypes.GameServer
                Case "DownloadServer"
                    tmp.Type = _SessionInfo._ServerTypes.DownloadServer
                Case "AdminTool"
                    tmp.Type = _SessionInfo._ServerTypes.AdminTool
            End Select

            Dim writer As New PacketWriter
            Dim name As String = "GlobalManager"
            writer.Create(ServerOpcodes.ServerInfo)
            writer.Word(name.Length)
            writer.HexString(name)

            If tmp.HandshakeComplete Then
                If tmp.Type <> _SessionInfo._ServerTypes.Unknown Then
                    If tmp.ProtocolVersion = Settings.Server_ProtocolVersion Then
                        If GlobalDb.CheckServerCert(tmp.ServerId, tmp.ClientName, ClientList.GetIp(Index_)) Then
                            writer.Byte(True)
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
