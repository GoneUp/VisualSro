Imports GlobalManager.Framework
Imports SRFramework

Namespace Functions
    Module Parser

        Public Sub Parse(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            ClientList.LastPingTime(Index_) = Date.Now

            If ClientList.SessionInfo(Index_).Authorized = False Then
                '===Pre-Game===
                Select Case opcode
                    Case ClientOpcodes.PING
                    Case ClientOpcodes.HANDSHAKE
                        Auth.OnVerifyIdentity(packet, Index_)
                    Case ClientOpcodes.LOGIN_WHO_AM_I
                        Auth.OnGateWay(packet, Index_)
                End Select


            ElseIf ClientList.SessionInfo(Index_).Authorized Then
                '===SECURE===
                Select Case opcode
                    Case ClientOpcodes.PING
                    Case InternalClientOpcodes.Server_Init
                        Shard.OnInitServer(packet, Index_)
                    Case InternalClientOpcodes.Server_Shutdown
                        Shard.OnShutdownServer(packet, Index_)
                    Case InternalClientOpcodes.Server_Info
                        Shard.OnServerInfo(packet, Index_)
                    Case InternalClientOpcodes.GateWay_SendUserAuth
                        Agent.OnSendUserAuth(packet, Index_)
                    Case InternalClientOpcodes.GameServer_CheckUserAuth
                        Agent.OnCheckUserAuth(packet, Index_)
                End Select
            End If
        End Sub
    End Module

End Namespace