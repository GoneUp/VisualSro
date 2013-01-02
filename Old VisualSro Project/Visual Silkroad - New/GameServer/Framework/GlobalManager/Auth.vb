Imports SRFramework

Namespace GlobalManager
    Module Auth
        Public Sub OnHandshake(ByVal packet As PacketReader)
            If packet.Length = 8 Then
                Dim baseKey As UInt16 = packet.Word
                Dim newKey = CalculateNewKey(baseKey)

                Dim writer As New PacketWriter
                writer.Create(ClientOpcodes.HANDSHAKE)
                writer.DWord(newKey)
                GlobalManagerCon.Send(writer.GetBytes)

            ElseIf packet.Length = 7 Then
                Dim success As Byte = packet.Byte

                If success = 1 Then
                    Log.WriteSystemLog("Handshake with GlobalManager Succeed!!!")
                    OnSendAuthInfo()
                Else
                    Log.WriteSystemLog("Handshake with GlobalManager Failed!!!")
                    Log.WriteSystemLog("Cannot start GameServer!")
                End If
            End If
        End Sub

        Private Function CalculateNewKey(ByVal baseKey As UInt16) As UInt32
            CalculateNewKey = BaseKey
            CalculateNewKey *= 4
            CalculateNewKey *= Date.Now.DayOfYear
            CalculateNewKey *= Date.Now.Day
            Return CalculateNewKey
        End Function

        Public Sub OnSendAuthInfo()
            Const clientstring As String = "AgentServer"

            Dim writer As New PacketWriter
            writer.Create(ClientOpcodes.LOGIN_WHO_AM_I)
            writer.Word(clientstring.Length)
            writer.String(clientstring)
            writer.DWord(Settings.GlobalManagerProtocolVersion)
            writer.Word(Settings.ServerId)

            GlobalManagerCon.Send(writer.GetBytes)
        End Sub

        Public Sub OnServerInfo(ByVal packet As PacketReader)
            Dim servername As String = packet.String(packet.Word)
            Dim success As Byte = packet.Byte

            If success = 1 Then
                Log.WriteSystemLog("GlobalManager: Auth completed!")
                GlobalManagerCon.UpdateInfoAllowed = True
                OnSendMyInfo()
                OnSendServerInit()
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


                Log.WriteSystemLog("Cannot start Server!")
            End If
        End Sub


    End Module
End Namespace
