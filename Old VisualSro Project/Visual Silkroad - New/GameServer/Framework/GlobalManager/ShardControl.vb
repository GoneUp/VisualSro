Imports SRFramework

Namespace GlobalManager
    Module ShardControl
        Public Sub OnUpdateServerOptions(ByVal packet As PacketReader)
            Dim maxNormalClients As UShort = packet.Word
            Dim maxClients As UShort = packet.Word

            Dim serverXPRate As UShort = packet.Word
            Dim serverSPRate As UShort = packet.Word
            Dim serverGoldRate As UShort = packet.Word
            Dim serverDropRate As UShort = packet.Word
            Dim serverSpawnRate As UShort = packet.Word
            Dim serverDebugMode As Byte = packet.Byte

            If Server.Online Then
                GlobalManagerCon.Log("Cannot change maxClients on a running Server!")
            Else
                Server.MaxNormalClients = maxNormalClients
                Server.MaxClients = maxClients
            End If

            Settings.Server_XPRate = serverXPRate
            Settings.Server_SPRate = serverSPRate
            Settings.Server_GoldRate = serverGoldRate
            Settings.Server_DropRate = serverDropRate
            Settings.Server_SpawnRate = serverSpawnRate
            Settings.Server_DebugMode = serverDebugMode
            GlobalManagerCon.Log("Updated Server Options!")
        End Sub
    End Module
End Namespace