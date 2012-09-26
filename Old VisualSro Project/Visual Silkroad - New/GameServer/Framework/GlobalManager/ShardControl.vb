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

            Settings.ServerXPRate = serverXPRate
            Settings.ServerSPRate = serverSPRate
            Settings.ServerGoldRate = serverGoldRate
            Settings.ServerDropRate = serverDropRate
            Settings.ServerSpawnRate = serverSpawnRate
            Settings.ServerDebugMode = serverDebugMode
            GlobalManagerCon.Log("Updated Server Options!")
        End Sub
    End Module
End Namespace