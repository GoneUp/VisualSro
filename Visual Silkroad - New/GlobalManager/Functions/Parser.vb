Namespace GlobalManager.Functions
    Module Parser

        Public Sub Parse(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            ClientList.LastPingTime(Index_) = Date.Now

            If ClientList.SessionInfo(Index_).Authorized = False Then
                '===Pre-Game===
                Select Case opcode
                    Case ClientOpcodes.Handshake
                        Auth.OnVerifyIdentity(packet, Index_)
                    Case ClientOpcodes.WhoAmI
                        Auth.OnGateWay(packet, Index_)
                End Select


            ElseIf ClientList.SessionInfo(Index_).Authorized Then
                '===SECURE===
                Select Case opcode


                End Select

            End If
        End Sub
    End Module

End Namespace