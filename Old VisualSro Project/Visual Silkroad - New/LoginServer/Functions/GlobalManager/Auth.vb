Imports SRFramework

Namespace GlobalManager
    Module Auth
        Public Sub OnHandshake(ByVal packet As PacketReader)
            If packet.DataLen = 2 Then
                Dim BaseKey As UInt16 = packet.Word

                Dim writer As New PacketWriter
                writer.Create(ClientOpcodes.Handshake)
                writer.DWord(CalculateNewKey(BaseKey))
                Send(writer.GetBytes)

            ElseIf packet.DataLen = 1 Then
                Dim success As Byte = packet.Byte

                If success = 1 Then
                    Log.WriteSystemLog("Handshake with GlobalManager Succeed!!!")
                Else
                    Log.WriteSystemLog("Handshake with GlobalManager Failed!!!")
                    Log.WriteSystemLog("Cannot start GameServer!")
                End If
            End If
        End Sub

        Private Function CalculateNewKey(ByVal BaseKey As UInt16) As UInt32
            BaseKey *= 1.1
            BaseKey *= Date.Now.DayOfYear
            BaseKey *= Date.Now.DayOfWeek
        End Function

        Public Sub OnSendAuthInfo()
            Dim clientstring As String = "GatewayServer"

            Dim writer As New PacketWriter
            writer.Create(ClientOpcodes.LOGIN_WHO_AM_I)
            writer.Word(clientstring.Length)
            writer.String(clientstring)
            writer.DWord(Settings.GlobalManager_ProtocolVersion)
            writer.Word(Settings.Server_Id)

            GlobalManager.Send(writer.GetBytes)
        End Sub

        Public Sub OnServerInfo(ByVal packet As PacketReader)
            Dim servername As String = packet.String(packet.Word)
            Dim success As Byte = packet.Byte

            If success = 1 Then
                Log.WriteSystemLog("GlobalManager: Auth completed!")
            Else
                Dim errortag As Byte = packet.Byte
                Select Case errortag
                    Case 1
                        Log.WriteSystemLog("GlobalManager: Handhake with GlobalManager Failed!")
                    Case 2
                        Log.WriteSystemLog("GlobalManager: Wrong Servername!")
                    Case 3
                        Log.WriteSystemLog("GlobalManager: Wrong Protocolversion!")
                    Case 4
                        Log.WriteSystemLog("GlobalManager: Server Cert Error!")
                End Select


                Log.WriteSystemLog("Cannot start Loginserver!")
            End If
        End Sub
    End Module
End Namespace
