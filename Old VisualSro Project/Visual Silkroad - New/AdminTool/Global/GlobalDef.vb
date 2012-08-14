Imports SRFramework

Module GlobalDef
    Public GlobalManagerCon As New SRFramework.GlobalManagerClient

    Public Shard_Gateways As New Dictionary(Of UShort, GatewayServer) 'Key=ServerId
    Public Shard_Downloads As New Dictionary(Of UShort, DownloadServer)
    Public Shard_Gameservers As New Dictionary(Of UShort, GameServer)

    Public SessionInfo As New cSessionInfo_AdminTool
End Module

