Imports GlobalManager.Framework
Imports SRFramework

Namespace Shard
    Module ShardService

        Friend Sub OnInitServer(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ServerId As UInt16 = SessionInfo(Index_).ServerId

            Select Case SessionInfo(Index_).Type
                Case cSessionInfo_GlobalManager._ServerTypes.GatewayServer
                    If Server_GateWay.ContainsKey(ServerId) = False AndAlso Server_GateWay(ServerId).Online Then
                        Log.WriteSystemLog("Server not exitis or is already marked as online! NAME: " & SessionInfo(Index_).ClientName & " ID: " & SessionInfo(Index_).ServerId)
                    Else
                        'Init Serverr
                        Server_GateWay(ServerId).Online = True
                        Log.WriteSystemLog(String.Format("Gatewayserver [{0}] fully initialized!", ClientList.GetIP(Index_)))
                        SendServerInit(Index_)
                    End If
                Case cSessionInfo_GlobalManager._ServerTypes.GameServer
                    If Server_Game.ContainsKey(ServerId) = False AndAlso Server_Game(ServerId).Online Then
                        Log.WriteSystemLog("Server not exitis or is already marked as online! NAME: " & SessionInfo(Index_).ClientName & " ID: " & SessionInfo(Index_).ServerId)
                    Else
                        'Init Server
                        Server_Game(ServerId).State = GameServer._ServerState.Online
                        Log.WriteSystemLog(String.Format("Gameserver [{0}] fully initialized!", ClientList.GetIP(Index_)))
                        SendServerInit(Index_)
                    End If
                Case cSessionInfo_GlobalManager._ServerTypes.DownloadServer

                    If Server_Download.ContainsKey(ServerId) = False AndAlso Server_Download(ServerId).Online Then
                        Log.WriteSystemLog("Server not exitis or is already marked as online! NAME: " & SessionInfo(Index_).ClientName & " ID: " & SessionInfo(Index_).ServerId)
                    Else
                        'Init Server
                        Server_Download(ServerId).Online = True
                        Log.WriteSystemLog(String.Format("Downloadserver [{0}] fully initialized!", ClientList.GetIP(Index_)))
                        SendServerInit(Index_)
                    End If
            End Select

            SendGlobalInfo(Index_)
        End Sub

        Private Sub SendServerInit(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(InternalServerOpcodes.SERVER_INIT)
            writer.Byte(1)
            Server.Send(writer.GetBytes, index_)
        End Sub

        Friend Sub OnShutdownServer(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim ServerId As UInt16 = SessionInfo(Index_).ServerId

            Select Case SessionInfo(Index_).Type
                Case cSessionInfo_GlobalManager._ServerTypes.GatewayServer
                    If Server_GateWay.ContainsKey(ServerId) Then
                        Server_GateWay(ServerId).Online = False
                        Server_GateWay.Remove(ServerId)
                        Log.WriteSystemLog(String.Format("Gatewayserver [{0}] turned off successfully!", ClientList.GetIP(Index_)))
                        SendShutdown(Index_)
                    End If
                Case cSessionInfo_GlobalManager._ServerTypes.GameServer
                    If Server_Game.ContainsKey(ServerId) Then
                        Server_Game(ServerId).State = GameServer._ServerState.Check
                        Server_Game.Remove(ServerId)
                        Log.WriteSystemLog(String.Format("Gameserver [{0}] turned off successfully!", ClientList.GetIP(Index_)))
                        SendShutdown(Index_)
                    End If
                Case cSessionInfo_GlobalManager._ServerTypes.DownloadServer

                    If Server_Download.ContainsKey(ServerId) Then
                        Server_Download(ServerId).Online = False
                        Server_Download.Remove(ServerId)
                        Log.WriteSystemLog(String.Format("Downloadserver [{0}] turned off successfully!", ClientList.GetIP(Index_)))
                        SendShutdown(Index_)
                    End If
            End Select

        End Sub

        Private Sub SendShutdown(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(InternalServerOpcodes.SERVER_SHUTDOWN)
            writer.Byte(1)
            Server.Send(writer.GetBytes, index_)
        End Sub

        Friend Sub OnServerInfo(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Select Case SessionInfo(Index_).Type
                Case cSessionInfo_GlobalManager._ServerTypes.GatewayServer
                    Dim tmp As New GatewayServer
                    tmp.IP = ClientList.GetIP(Index_).Split(":")(0)
                    tmp.Port = Convert.ToUInt16(ClientList.GetIP(Index_).Split(":")(1))
                    tmp.ServerId = packet.Word
                    tmp.ActualUser = packet.Word
                    tmp.MaxUser = packet.Word

                    If Server_GateWay.ContainsKey(tmp.ServerId) Then
                        Server_GateWay(tmp.ServerId) = tmp
                    Else
                        tmp.Online = False
                        Server_GateWay.Add(tmp.ServerId, tmp)
                    End If

                Case cSessionInfo_GlobalManager._ServerTypes.GameServer
                    Dim tmp As New GameServer
                    tmp.ServerId = packet.Word
                    tmp.ServerName = packet.String(packet.Word)
                    tmp.ActualUser = packet.Word
                    tmp.MaxUser = packet.Word

                    tmp.MobCount = packet.DWord
                    tmp.NpcCount = packet.DWord
                    tmp.ItemCount = packet.DWord

                    tmp.Server_XPRate = packet.Word
                    tmp.Server_SPRate = packet.Word
                    tmp.Server_GoldRate = packet.Word
                    tmp.Server_DropRate = packet.Word
                    tmp.Server_SpawnRate = packet.Word
                    tmp.Server_Debug = packet.Byte

                    If Server_Game.ContainsKey(tmp.ServerId) Then
                        Server_Game(tmp.ServerId) = tmp
                    Else
                        tmp.State = GameServer._ServerState.Check
                        Server_Game.Add(tmp.ServerId, tmp)
                    End If

                Case cSessionInfo_GlobalManager._ServerTypes.DownloadServer
                    Dim tmp As New DownloadServer
                    tmp.ServerId = packet.Word
                    tmp.ActualUser = packet.Word
                    tmp.MaxUser = packet.Word

                    If Server_Download.ContainsKey(tmp.ServerId) Then
                        Server_Download(tmp.ServerId) = tmp
                    Else
                        tmp.Online = False
                        Server_Download.Add(tmp.ServerId, tmp)
                    End If
            End Select




            SendGlobalInfo(Index_)
        End Sub

        Friend Sub SendGlobalInfo(ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(InternalServerOpcodes.GLOBAL_INFO)
            writer.Word(Server_GateWay.Count)
            Dim tmplist = Server_GateWay.Keys.ToList
            For i = 0 To tmplist.Count - 1
                writer.Word(Server_GateWay(tmplist(i)).ServerId)
                writer.Word(Server_GateWay(tmplist(i)).IP.Length)
                writer.String(Server_GateWay(tmplist(i)).IP)
                writer.Word(Server_GateWay(tmplist(i)).Port)
                writer.Word(Server_GateWay(tmplist(i)).ActualUser)
                writer.Word(Server_GateWay(tmplist(i)).MaxUser)
            Next

            writer.Word(Server_Download.Count)
            tmplist = Server_Download.Keys.ToList
            For i = 0 To tmplist.Count - 1
                writer.Word(Server_Download(tmplist(i)).ServerId)
                writer.Word(Server_Download(tmplist(i)).IP.Length)
                writer.String(Server_Download(tmplist(i)).IP)
                writer.Word(Server_Download(tmplist(i)).Port)
                writer.Word(Server_Download(tmplist(i)).ActualUser)
                writer.Word(Server_Download(tmplist(i)).MaxUser)
            Next

            writer.Word(Server_Game.Count)
            tmplist = Server_Game.Keys.ToList
            For i = 0 To tmplist.Count - 1
                writer.Word(Server_Game(tmplist(i)).ServerId)
                writer.Word(Server_Game(tmplist(i)).ServerName.Length)
                writer.String(Server_Game(tmplist(i)).ServerName)

                writer.Word(Server_Game(tmplist(i)).IP.Length)
                writer.String(Server_Game(tmplist(i)).IP)
                writer.Word(Server_Game(tmplist(i)).Port)
                writer.Word(Server_Game(tmplist(i)).ActualUser)
                writer.Word(Server_Game(tmplist(i)).MaxUser)

                writer.DWord(Server_Game(tmplist(i)).MobCount)
                writer.DWord(Server_Game(tmplist(i)).NpcCount)
                writer.DWord(Server_Game(tmplist(i)).ItemCount)

                writer.Word(Server_Game(tmplist(i)).Server_XPRate)
                writer.Word(Server_Game(tmplist(i)).Server_SPRate)
                writer.Word(Server_Game(tmplist(i)).Server_GoldRate)
                writer.Word(Server_Game(tmplist(i)).Server_DropRate)
                writer.Word(Server_Game(tmplist(i)).Server_SpawnRate)
                writer.Byte(Server_Game(tmplist(i)).Server_Debug)
            Next

            Server.SendToAll(writer.GetBytes)
        End Sub

    End Module
End Namespace
