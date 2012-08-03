Imports SRFramework

Module GlobalDef
    Public Database As New SRFramework.cDatabase

    Public GlobalManagerCon As New SRFramework.GlobalManagerClient

    Public Server As New cServer_Base

    Public Shard_Gateways As New Dictionary(Of UShort, GatewayServer) 'Key=ServerId
    Public Shard_Downloads As New Dictionary(Of UShort, DownloadServer)
    Public Shard_Gameservers As New Dictionary(Of UShort, GameServer)

    Public SessionInfo(1) As cSessionInfo_LoginServer

    Public Sub Initalize(ByVal maxClients As Integer)
        ReDim SessionInfo(maxClients)
    End Sub
End Module

