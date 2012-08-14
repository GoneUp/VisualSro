Imports SRFramework

Namespace Shard
    Module ShardManager
        Public Sub RemoveServer(ByVal serverID As UInt16, ByVal type As cSessionInfo_GlobalManager._ServerTypes)
            Select Case type
                Case cSessionInfo_GlobalManager._ServerTypes.GatewayServer
                    If Server_Gateway.ContainsKey(serverID) Then
                        Server_Gateway(serverID).Online = False
                        Server_Gateway.Remove(serverID)
                    End If
                Case cSessionInfo_GlobalManager._ServerTypes.GameServer
                    If Server_Game.ContainsKey(serverID) Then
                        Server_Game(serverID).State = GameServer._ServerState.Check
                        Server_Game.Remove(serverID)
                    End If
                Case cSessionInfo_GlobalManager._ServerTypes.DownloadServer
                    If Server_Download.ContainsKey(serverID) Then
                        Server_Download(serverID).Online = False
                        Server_Download.Remove(serverID)
                    End If
            End Select
        End Sub
    End Module
End Namespace
