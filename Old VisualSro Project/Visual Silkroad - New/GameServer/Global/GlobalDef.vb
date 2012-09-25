Imports SRFramework

Module GlobalDef
    Public ReadOnly Database As New cDatabase
    Public ReadOnly GlobalManagerCon As New GlobalManagerClient
    Public ReadOnly Server As New cServer_Gameserver
    Public ReadOnly Log As New cLog

    Public ReadOnly ShardGateways As New Dictionary(Of UShort, GatewayServer) 'Key=ServerId
    Public ReadOnly ShardDownloads As New Dictionary(Of UShort, DownloadServer)
    Public ReadOnly ShardGameservers As New Dictionary(Of UShort, SRFramework.GameServer)

    Public SessionInfo(1) As cSessionInfo_GameServer

    Public Function Initalize(ByVal maxClients As Integer) As Boolean
        Try
            ReDim SessionInfo(maxClients)

            ShardGateways.Clear()
            ShardDownloads.Clear()
            ShardGameservers.Clear()
        Catch ex As Exception
            Log.WriteSystemLog("GlobalDef Init failed! EX:" & ex.Message & " Stacktrace: " & ex.StackTrace)
            Return False
        End Try

        Return True
    End Function
End Module

