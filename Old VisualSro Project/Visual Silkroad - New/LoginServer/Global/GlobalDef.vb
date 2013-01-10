Imports SRFramework
Imports System.Runtime.Serialization

Module GlobalDef
    Public ReadOnly Database As New cDatabase
    Public ReadOnly GlobalManagerCon As New GlobalManagerClient
    Public ReadOnly Server As New ServerBase
    Public ReadOnly Log As New cLog

    Public ReadOnly ShardGateways As New Dictionary(Of UShort, GatewayServer) 'Key=ServerId
    Public ReadOnly ShardDownloads As New Dictionary(Of UShort, DownloadServer)
    Public ReadOnly ShardGameservers As New Dictionary(Of UShort, GameServer)

    Public LauncherMessages As New List(Of LauncherMessage)
    Public BinFormatter As IFormatter = New Formatters.Binary.BinaryFormatter()

    Public SessionInfo(1) As cSessionInfo_LoginServer


    Public Function Initalize(ByVal maxClients As Integer) As Boolean
        Try
            ReDim SessionInfo(maxClients)

            ShardGateways.Clear()
            ShardDownloads.Clear()
            ShardGameservers.Clear()

            LauncherMessages.Clear()
        Catch ex As Exception
            Log.WriteSystemLog("GlobalDef Init failed! EX:" & ex.Message & " Stacktrace: " & ex.StackTrace)
            Return False
        End Try

        Return True
    End Function
End Module

