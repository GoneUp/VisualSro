Namespace GameServer.GateWay
    Module ServerDb
        Public Server_GateWay As New List(Of _GateWayServer)
        Public Server_Game As New List(Of _GameServer)


    End Module

    Public Class _GateWayServer
        Public SessionIndex As Integer
        Public Online As Boolean = False
        Public IP As String
        Public Port As UInt16

        Public ActualUser As UInt16
        Public MaxUser As UInt16
        Public StartTime As DateTime

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
        Public Server_PingDc As Boolean = True
    End Class

    Public Class _GameServer
        Public SessionIndex As Integer
        Public Online As Boolean = False
        Public IP As String
        Public Port As UInt16

        Public ActualUser As UInt16
        Public MaxUser As UInt16
        Public StartTime As DateTime

        Public GameServerId As UInt16
        Public State As _ServerState


        Enum _ServerState
            Check = 0
            Online = 1
        End Enum
    End Class
End Namespace