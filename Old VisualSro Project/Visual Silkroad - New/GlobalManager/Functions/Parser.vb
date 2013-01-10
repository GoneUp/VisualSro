Imports SRFramework

Namespace Functions
    Module Parser

        Public Sub Parse(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            Server.ClientList.LastPingTime(Index_) = Date.Now

            If SessionInfo(Index_).Authorized = False Then
                '===Pre-Game===
                Select Case opcode
                    Case ClientOpcodes.PING
                    Case ClientOpcodes.HANDSHAKE
                        Auth.OnVerifyIdentity(packet, Index_)
                    Case ClientOpcodes.LOGIN_WHO_AM_I
                        Auth.OnGateWay(packet, Index_)
                End Select


            ElseIf SessionInfo(Index_).Authorized Then
                '===SECURE===
                Select Case opcode
                    Case ClientOpcodes.PING
                    Case InternalClientOpcodes.SERVER_INIT
                        Shard.OnInitServer(packet, Index_)
                    Case InternalClientOpcodes.SERVER_SHUTDOWN
                        Shard.OnShutdownServer(packet, Index_)
                    Case InternalClientOpcodes.SERVER_INFO
                        Shard.OnServerInfo(packet, Index_)
                    Case InternalClientOpcodes.SERVER_UPDATEREPLY

                    Case InternalClientOpcodes.AGENT_USERAUTH
                        Agent.OnUserAuth(packet, Index_)
                    Case InternalClientOpcodes.AGENT_CHECK_USERAUTH
                        Agent.OnCheckUserAuth(packet, Index_)
                    Case InternalClientOpcodes.AGENT_SILK
                        UserService.OnSilk(packet, Index_)
                    Case InternalClientOpcodes.AGENT_USERINFO
                        UserService.OnUserHandler(packet, Index_)
                    Case InternalClientOpcodes.AGENT_NEWS
                        Gateway.OnNewsRequest(Index_)
                End Select
            End If
        End Sub
    End Module

End Namespace