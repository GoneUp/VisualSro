Imports SRFramework
Imports LoginServer.Framework

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
            writer.Word(Settings.Server_Id)
            writer.Word(Server.OnlineClient)
            writer.Word(Server.MaxClients)

            GlobalManagerCon.Send(writer.GetBytes)
            GlobalManagerCon.LastInfoTime = Date.Now
        End Sub

        Public Sub OnGlobalInfo(ByVal packet As PacketReader)
            Dim gateways As New List(Of GatewayServer)
            Dim downloads As New List(Of DownloadServer)
            Dim gameservers As New List(Of GameServer)

            Dim gateway_count As UShort = packet.Word
            For i = 0 To gateway_count - 1
                Dim tmp As New GatewayServer
                tmp.ServerId = packet.Word
                tmp.IP = packet.String(packet.Word)
                tmp.Port = packet.Word
                tmp.ActualUser = packet.Word
                tmp.MaxUser = packet.Word
                gateways.Add(tmp)
            Next

            Dim downloads_count As UShort = packet.Word
            For i = 0 To downloads_count - 1
                Dim tmp As New DownloadServer
                tmp.ServerId = packet.Word
                tmp.IP = packet.String(packet.Word)
                tmp.Port = packet.Word
                tmp.ActualUser = packet.Word
                tmp.MaxUser = packet.Word
                downloads.Add(tmp)
            Next

            Dim gameservers_count As UShort = packet.Word
            For i = 0 To gameservers_count - 1
                Dim tmp As New GameServer
                tmp.ServerId = packet.Word
                tmp.ServerName = packet.String(packet.Word)
                tmp.IP = packet.String(packet.Word)
                tmp.Port = packet.Word
                tmp.ActualUser = packet.Word
                tmp.MaxUser = packet.Word

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
            Shard_Gateways.Clear()
            Shard_Downloads.Clear()
            Shard_Gameservers.Clear()

            For Each server In gateways
                Shard_Gateways.Add(server.ServerId, server)
            Next
            For Each server In downloads
                Shard_Downloads.Add(server.ServerId, server)
            Next
            For Each server In gameservers
                Shard_Gameservers.Add(server.ServerId, server)
            Next
        End Sub
    End Module
End Namespace