Imports SRFramework

Namespace GlobalManager
    Module ShardService

        Public Sub OnSendServerInit()
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.SERVER_INIT)
            GlobalManagerCon.Send(writer.GetBytes)
        End Sub

        Public Sub OnServerInit(ByVal packet As PacketReader)
            Dim success As Byte = packet.Byte

            If success = 1 Then
                Log.WriteSystemLog("GlobalManager: Init Complete!")
                RequestNews()
                GlobalManagerCon.InitComplete()
            Else
                Log.WriteSystemLog("GlobalManager: Init failed!")
                Log.WriteSystemLog("Cannot start Loginserver!")
            End If
        End Sub

        Public Sub OnSendServerShutdown()
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.SERVER_SHUTDOWN)
            GlobalManagerCon.Send(writer.GetBytes)
        End Sub

        Public Sub OnServerShutdown(ByVal packet As PacketReader)
            Dim success As Byte = packet.Byte

            If success = 1 Then
                Log.WriteSystemLog("GlobalManager: Shutdown Comfirmed!")
                GlobalManagerCon.ShutdownComplete()
            Else
                Log.WriteSystemLog("GlobalManager: WTF: Shutdown failed!")
                Log.WriteSystemLog("Cannot close Loginserver ^^")
            End If
        End Sub

        Public Sub OnSendMyInfo()
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.SERVER_INFO)
            writer.Word(Settings.ServerId)
            writer.Word(Server.OnlineClients)
            writer.Word(Server.MaxNormalClients)
            writer.Word(Server.MaxClients)

            writer.Word(Settings.ServerIp.Length)
            writer.String(Settings.ServerIp)
            writer.Word(Settings.ServerPort)

            GlobalManagerCon.Send(writer.GetBytes)
            GlobalManagerCon.LastInfoTime = Date.Now
        End Sub

        Public Sub OnGlobalInfo(ByVal packet As PacketReader)
            Dim gateways As New List(Of GatewayServer)
            Dim downloads As New List(Of DownloadServer)
            Dim gameservers As New List(Of SRFramework.GameServer)

            Dim gateway_count As UShort = packet.Word
            For i = 0 To gateway_count - 1
                Dim tmp As New GatewayServer
                tmp.ServerId = packet.Word
                tmp.IP = packet.String(packet.Word)
                tmp.Port = packet.Word
                tmp.OnlineClients = packet.Word
                tmp.MaxNormalClients = packet.Word
                tmp.MaxClients = packet.Word
                tmp.Online = packet.Byte
                gateways.Add(tmp)
            Next

            Dim downloads_count As UShort = packet.Word
            For i = 0 To downloads_count - 1
                Dim tmp As New DownloadServer
                tmp.ServerId = packet.Word
                tmp.IP = packet.String(packet.Word)
                tmp.Port = packet.Word
                tmp.OnlineClients = packet.Word
                tmp.MaxNormalClients = packet.Word
                tmp.MaxClients = packet.Word
                tmp.Online = packet.Byte
                downloads.Add(tmp)
            Next

            Dim gameservers_count As UShort = packet.Word
            For i = 0 To gameservers_count - 1
                Dim tmp As New SRFramework.GameServer
                tmp.ServerId = packet.Word
                tmp.IP = packet.String(packet.Word)
                tmp.Port = packet.Word
                tmp.OnlineClients = packet.Word
                tmp.MaxNormalClients = packet.Word
                tmp.MaxClients = packet.Word
                tmp.Online = packet.Byte

                tmp.ServerName = packet.String(packet.Word)
                tmp.MobCount = packet.DWord
                tmp.NpcCount = packet.DWord
                tmp.ItemCount = packet.DWord

                tmp.Server_XPRate = packet.Word
                tmp.Server_SPRate = packet.Word
                tmp.Server_GoldRate = packet.Word
                tmp.Server_DropRate = packet.Word
                tmp.Server_SpawnRate = packet.Word
                tmp.Server_Debug = packet.Byte
                gameservers.Add(tmp)
            Next

            UpdateGlobalInfo(gateways, downloads, gameservers)
        End Sub

        Public Sub UpdateGlobalInfo(ByVal gateways As List(Of GatewayServer), ByVal downloads As List(Of DownloadServer), ByVal gameservers As List(Of GameServer))
            ShardGateways.Clear()
            ShardDownloads.Clear()
            ShardGameservers.Clear()

            For Each tmp In gateways
                ShardGateways.Add(tmp.ServerId, tmp)
            Next
            For Each tmp In downloads
                ShardDownloads.Add(tmp.ServerId, tmp)
            Next
            For Each tmp In gameservers
                ShardGameservers.Add(tmp.ServerId, tmp)
            Next
        End Sub
    End Module
End Namespace