Imports SRFramework

Namespace Functions

    Public Module Parser
        Public Sub Parse(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            Server.ClientList.LastPingTime(Index_) = Date.Now

            Select Case opcode
                Case ClientOpcodes.PING

                Case ClientOpcodes.HANDSHAKE_CONFIRM  'Client accepts
                    SessionInfo(Index_).SRConnectionSetup = cSessionInfo_LoginServer.SRConnectionStatus.HANDSHAKE

                Case ClientOpcodes.LOGIN_WHO_AM_I  'GateWay
                    Gateway(packet, Index_)

                Case ClientOpcodes.LOGIN_PATCH_REQ  'Client sends Patch Info
                    ClientInfo(packet, Index_)
                    SendPatchInfo(Index_)

                Case ClientOpcodes.LOGIN_LAUNCHER_REQ
                    SendLauncherInfo(Index_)

                Case ClientOpcodes.LOGIN_SERVER_LIST_REQ
                    SendServerList(Index_)

                Case ClientOpcodes.LOGIN_LOGIN_REQ
                    HandleLogin(packet, Index_)

                Case Else
                    Log.WriteSystemLog("opCode: " & opcode) '& " Packet : " & packet.Byte)
            End Select
        End Sub

        Public Sub ParseGlobalManager(ByVal packet As PacketReader)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            Select Case opcode
                Case ServerOpcodes.HANDSHAKE   'Client accepts
                    GlobalManager.OnHandshake(packet)
                Case ServerOpcodes.LOGIN_SERVER_INFO
                    GlobalManager.OnServerInfo(packet)

                Case InternalServerOpcodes.SERVER_INIT
                    GlobalManager.OnServerInit(packet)

                Case InternalServerOpcodes.SERVER_SHUTDOWN
                    GlobalManager.OnServerShutdown(packet)

                Case InternalServerOpcodes.GLOBAL_INFO
                    GlobalManager.OnGlobalInfo(packet)

                Case InternalServerOpcodes.AGENT_SEND_USERAUTH
                    GlobalManager.OnUserAuthReply(packet)

                Case Else
                    Log.WriteSystemLog("gmc opCode: " & opcode) '& " Packet : " & packet.Byte)
            End Select
        End Sub

    End Module
End Namespace

