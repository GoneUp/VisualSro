Imports SRFramework

Module GlobalDef
    Public Database As New SRFramework.cDatabase

    Public GlobalManagerCon As New SRFramework.GlobalManagerClient

    Public Shard_Gateways As New Dictionary(Of UShort, GatewayServer) 'Key=ServerId
    Public Shard_Downloads As New Dictionary(Of UShort, DownloadServer)
    Public Shard_Gameservers As New Dictionary(Of UShort, SRFramework.GameServer)
End Module

