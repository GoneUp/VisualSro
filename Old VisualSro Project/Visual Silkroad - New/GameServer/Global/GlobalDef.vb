Imports SRFramework

Module GlobalDef
    Public Database As New cDatabase
    Public GlobalManagerCon As New GlobalManagerClient
    Public Server As New cServer_Gameserver
    Public Log As New cLog

    Public Shard_Gateways As New Dictionary(Of UShort, GatewayServer) 'Key=ServerId
    Public Shard_Downloads As New Dictionary(Of UShort, DownloadServer)
    Public Shard_Gameservers As New Dictionary(Of UShort, SRFramework.GameServer)

    Public SessionInfo(1) As cSessionInfo_GameServer

    Public Sub Initalize(ByVal maxClients As Integer)
        ReDim SessionInfo(maxClients)

        Shard_Gateways.Clear()
        Shard_Downloads.Clear()
        Shard_Gameservers.Clear()
    End Sub
End Module

