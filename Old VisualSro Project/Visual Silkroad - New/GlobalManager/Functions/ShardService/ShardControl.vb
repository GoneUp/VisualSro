Imports SRFramework

Namespace Shard
    Module ShardControl
        Public Sub OnSendServerOptions(ByVal Index_ As Integer)
            Dim serverId As UInt16 = SessionInfo(Index_).ServerId
            If SessionInfo(Index_).Type = cSessionInfo_GlobalManager.ServerTypes.GameServer Then
                If Server_Game.ContainsKey(serverId) Then
                    Dim writer As New PacketWriter
                    writer.Create(InternalServerOpcodes.SERVER_UPDATE)
                    writer.Word(Server_Game(serverId).MaxNormalClients)
                    writer.Word(Server_Game(serverId).MaxClients)
                    writer.Word(Server_Game(serverId).Server_XPRate)
                    writer.Word(Server_Game(serverId).Server_SPRate)
                    writer.Word(Server_Game(serverId).Server_GoldRate)
                    writer.Word(Server_Game(serverId).Server_DropRate)
                    writer.Word(Server_Game(serverId).Server_SpawnRate)
                    writer.Byte(Server_Game(serverId).Server_Debug)
                    Server.Send(writer.GetBytes, Index_)
                End If
            End If
        End Sub

    End Module
End Namespace
