Namespace Shard
    Module ServerDb
        Public Server_GateWay As New Dictionary(Of UInt16, GatewayServer)
        Public Server_Game As New Dictionary(Of UInt16, GameServer)
        Public Server_Download As New Dictionary(Of UInt16, DownloadServer)
    End Module
End Namespace