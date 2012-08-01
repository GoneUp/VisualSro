Imports GlobalManager.Framework
Imports SRFramework

Namespace Shard
    Module ShardControl

        Public Sub OnSendServerOptions(ByVal Index_ As Integer)
            Dim ServerId As UInt16 = SessionInfo(Index_).ServerId
            If SessionInfo(Index_).Type = cSessionInfo_GlobalManager._ServerTypes.GameServer Then
                If Server_Game.ContainsKey(ServerId) Then
                    Dim writer As New PacketWriter
                    writer.Create(InternalServerOpcodes.SERVER_UPDATE)
                    writer.Word(Server_Game(ServerId).Server_XPRate)
                    writer.Word(Server_Game(ServerId).Server_SPRate)
                    writer.Word(Server_Game(ServerId).Server_GoldRate)
                    writer.Word(Server_Game(ServerId).Server_DropRate)
                    writer.Word(Server_Game(ServerId).Server_SpawnRate)
                    writer.Byte(Server_Game(ServerId).Server_Debug)
                    Server.Send(writer.GetBytes, Index_)
                End If
            End If
        End Sub

    End Module
End Namespace
