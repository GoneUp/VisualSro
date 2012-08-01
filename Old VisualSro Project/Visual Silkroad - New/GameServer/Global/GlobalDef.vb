Imports SRFramework

Module GlobalDef
    Public Database As New cDatabase

    Public GlobalManagerCon As New GlobalManagerClient

    Public Clientlist As New cClientList

    Public Shard_Gateways As New Dictionary(Of UShort, GatewayServer) 'Key=ServerId
    Public Shard_Downloads As New Dictionary(Of UShort, DownloadServer)
    Public Shard_Gameservers As New Dictionary(Of UShort, SRFramework.GameServer)

    Public SessionInfo(1) As cSessionInfo_GameServer

    Public Sub Initalize(ByVal maxClients As Integer)
        ReDim SessionInfo(maxClients)
    End Sub
End Module

