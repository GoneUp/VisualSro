Imports GlobalManager.Framework

Namespace Shard
    Module ServerDb
        Public Server_GateWay As New Dictionary(Of UInt16, _GateWayServer)
        Public Server_Game As New Dictionary(Of UInt16, _GameServer)
        Public Server_Download As New Dictionary(Of UInt16, _DownloadServer)
    End Module


    Public Class _GateWayServer
        Public ServerId As UInt32

        Public Online As Boolean = False
        Public IP As String
        Public Port As UInt16

        Public ActualUser As UInt16
        Public MaxUser As UInt16
        Public StartTime As DateTime
    End Class

    Public Class _GameServer
        Public ServerId As UInt32
        Public ServerName As String
        Public IP As String
        Public Port As UInt16

        Public ActualUser As UInt16
        Public MaxUser As UInt16
        Public StartTime As DateTime
        Public State As _ServerState = _ServerState.Check

        'General Server Data
        Public MobCount As UInt32 = 0
        Public ItemCount As UInt32 = 0
        Public NpcCount As UInt32 = 0
        'Statistic Data (Todo??)
        Public AllItemsCount As UInt32 = 0
        Public AllPlayersCount As UInt32 = 0
        Public AllSkillsCount As UInt32 = 0
        'Settings
        Public Server_XPRate As Long = 1
        Public Server_SPRate As Long = 1
        Public Server_GoldRate As Long = 1
        Public Server_DropRate As Long = 1
        Public Server_SpawnRate As Long = 1
        Public Server_Debug As Boolean = False


        Enum _ServerState
            Check = 0
            Online = 1
        End Enum
    End Class

    Public Class _DownloadServer
        Public ServerId As UInt32

        Public Online As Boolean = False
        Public IP As String
        Public Port As UInt16

        Public ActualUser As UInt16
        Public MaxUser As UInt16
        Public StartTime As DateTime
    End Class
End Namespace