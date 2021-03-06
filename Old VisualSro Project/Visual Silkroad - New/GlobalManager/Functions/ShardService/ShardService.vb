﻿Imports SRFramework

Namespace Shard
    Module ShardService

        Friend Sub OnInitServer(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim serverId As UInt16 = SessionInfo(Index_).ServerId

            Select Case SessionInfo(Index_).Type
                Case cSessionInfo_GlobalManager.ServerTypes.GatewayServer
                    If Server_Gateway.ContainsKey(serverId) = False AndAlso Server_Gateway(serverId).Online Then
                        Log.WriteSystemLog("Server not exitis or is already marked as online! NAME: " & SessionInfo(Index_).ClientName & " ID: " & SessionInfo(Index_).ServerId)
                    Else
                        'Init Serverr
                        Server_Gateway(serverId).Online = True
                        Log.WriteSystemLog(String.Format("Gatewayserver [{0}] fully initialized!", Server.ClientList.GetIP(Index_)))
                    End If
                Case cSessionInfo_GlobalManager.ServerTypes.GameServer
                    If Server_Game.ContainsKey(serverId) = False AndAlso Server_Game(serverId).Online Then
                        Log.WriteSystemLog("Server not exitis or is already marked as online! NAME: " & SessionInfo(Index_).ClientName & " ID: " & SessionInfo(Index_).ServerId)
                    Else
                        'Init Server
                        Server_Game(serverId).State = GameServer._ServerState.Online
                        Log.WriteSystemLog(String.Format("Gameserver [{0}] fully initialized!", Server.ClientList.GetIP(Index_)))
                    End If
                Case cSessionInfo_GlobalManager.ServerTypes.DownloadServer
                    If Server_Download.ContainsKey(serverId) = False AndAlso Server_Download(serverId).Online Then
                        Log.WriteSystemLog("Server not exitis or is already marked as online! NAME: " & SessionInfo(Index_).ClientName & " ID: " & SessionInfo(Index_).ServerId)
                    Else
                        'Init Server
                        Server_Download(serverId).Online = True
                        Log.WriteSystemLog(String.Format("Downloadserver [{0}] fully initialized!", Server.ClientList.GetIP(Index_)))
                    End If
            End Select

            SendServerInit(Index_)
            SendGlobalInfo(Index_, True)
        End Sub

        Private Sub SendServerInit(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(InternalServerOpcodes.SERVER_INIT)
            writer.Byte(1)
            Server.Send(writer.GetBytes, index_)
        End Sub

        Friend Sub OnShutdownServer(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim serverId As UInt16 = SessionInfo(Index_).ServerId
            RemoveServer(serverId, SessionInfo(Index_).Type)

            Select Case SessionInfo(Index_).Type
                Case cSessionInfo_GlobalManager.ServerTypes.GatewayServer
                    Log.WriteSystemLog(String.Format("Gatewayserver [{0}] turned off successfully!", Server.ClientList.GetIP(Index_)))
                Case cSessionInfo_GlobalManager.ServerTypes.GameServer
                    Log.WriteSystemLog(String.Format("Gameserver [{0}] turned off successfully!", Server.ClientList.GetIP(Index_)))
                Case cSessionInfo_GlobalManager.ServerTypes.DownloadServer
                    Log.WriteSystemLog(String.Format("Downloadserver [{0}] turned off successfully!", Server.ClientList.GetIP(Index_)))
            End Select

            SendGlobalInfo(Index_, True) 'Sent the news to all

            SendShutdown(Index_)
        End Sub

        Private Sub SendShutdown(ByVal index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(InternalServerOpcodes.SERVER_SHUTDOWN)
            writer.Byte(1)
            Server.Send(writer.GetBytes, index_)
        End Sub

        Friend Sub OnServerInfo(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim firstAnnonce As Boolean = False

            Dim serverID As UShort = packet.Word
            Dim onlineClients As UShort = packet.Word
            Dim maxNormalClients As UShort = packet.Word
            Dim maxClients As UShort = packet.Word

            Dim ip As String = packet.String(packet.Word)
            Dim port As UShort = packet.Word

            Select Case SessionInfo(Index_).Type
                Case cSessionInfo_GlobalManager.ServerTypes.GatewayServer
                    Dim tmp As GatewayServer
                    If Server_Gateway.ContainsKey(serverID) Then
                        tmp = Server_Gateway(serverID)
                    Else
                        tmp = New GatewayServer
                    End If

                    tmp.IP = ip
                    tmp.Port = port
                    tmp.ServerId = serverID
                    tmp.OnlineClients = onlineClients
                    tmp.MaxNormalClients = maxNormalClients
                    tmp.MaxClients = maxClients

                    If Server_Gateway.ContainsKey(tmp.ServerId) Then
                        Server_Gateway(tmp.ServerId) = tmp
                    Else
                        tmp.Online = False
                        Server_Gateway.Add(tmp.ServerId, tmp)
                        firstAnnonce = True
                    End If

                Case cSessionInfo_GlobalManager.ServerTypes.GameServer
                    Dim tmp As GameServer
                    If Server_Game.ContainsKey(serverID) Then
                        tmp = Server_Game(serverID)
                    Else
                        tmp = New GameServer
                    End If

                    tmp.IP = ip
                    tmp.Port = port
                    tmp.ServerId = serverID
                    tmp.OnlineClients = onlineClients
                    tmp.MaxNormalClients = maxNormalClients
                    tmp.MaxClients = maxClients

                    tmp.ServerName = packet.String(packet.Word)
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
                        firstAnnonce = True
                    End If

                Case cSessionInfo_GlobalManager.ServerTypes.DownloadServer
                    Dim tmp As DownloadServer
                    If Server_Download.ContainsKey(serverID) Then
                        tmp = Server_Download(serverID)
                    Else
                        tmp = New DownloadServer
                    End If

                    tmp.IP = ip
                    tmp.Port = port
                    tmp.ServerId = serverID
                    tmp.OnlineClients = onlineClients
                    tmp.MaxNormalClients = maxNormalClients
                    tmp.MaxClients = maxClients

                    If Server_Download.ContainsKey(tmp.ServerId) Then
                        Server_Download(tmp.ServerId) = tmp
                    Else
                        tmp.Online = False
                        Server_Download.Add(tmp.ServerId, tmp)
                        firstAnnonce = True
                    End If
            End Select


            SendGlobalInfo(Index_, firstAnnonce)
        End Sub

        Friend Sub SendGlobalInfo(ByVal Index_ As Integer, ByVal firstAnnonce As Boolean)
            Dim writer As New PacketWriter
            writer.Create(InternalServerOpcodes.GLOBAL_INFO)
            writer.Word(Server_Gateway.Count)
            Dim tmplist = Server_Gateway.Keys.ToList
            For i = 0 To tmplist.Count - 1
                writer.Word(Server_Gateway(tmplist(i)).ServerId)
                writer.Word(Server_Gateway(tmplist(i)).IP.Length)
                writer.String(Server_Gateway(tmplist(i)).IP)
                writer.Word(Server_Gateway(tmplist(i)).Port)
                writer.Word(Server_Gateway(tmplist(i)).OnlineClients)
                writer.Word(Server_Gateway(tmplist(i)).MaxNormalClients)
                writer.Word(Server_Gateway(tmplist(i)).MaxClients)
                writer.Byte(Convert.ToByte(Server_Gateway(tmplist(i)).Online))
            Next

            writer.Word(Server_Download.Count)
            tmplist = Server_Download.Keys.ToList
            For i = 0 To tmplist.Count - 1
                writer.Word(Server_Download(tmplist(i)).ServerId)
                writer.Word(Server_Download(tmplist(i)).IP.Length)
                writer.String(Server_Download(tmplist(i)).IP)
                writer.Word(Server_Download(tmplist(i)).Port)
                writer.Word(Server_Download(tmplist(i)).OnlineClients)
                writer.Word(Server_Download(tmplist(i)).MaxNormalClients)
                writer.Word(Server_Download(tmplist(i)).MaxClients)
                writer.Byte(Convert.ToByte(Server_Download(tmplist(i)).Online))
            Next

            writer.Word(Server_Game.Count)
            tmplist = Server_Game.Keys.ToList
            For i = 0 To tmplist.Count - 1
                writer.Word(Server_Game(tmplist(i)).ServerId)
                writer.Word(Server_Game(tmplist(i)).IP.Length)
                writer.String(Server_Game(tmplist(i)).IP)
                writer.Word(Server_Game(tmplist(i)).Port)
                writer.Word(Server_Game(tmplist(i)).OnlineClients)
                writer.Word(Server_Game(tmplist(i)).MaxNormalClients)
                writer.Word(Server_Game(tmplist(i)).MaxClients)
                writer.Byte(Convert.ToByte(Server_Game(tmplist(i)).State))

                writer.Word(Server_Game(tmplist(i)).ServerName.Length)
                writer.String(Server_Game(tmplist(i)).ServerName)
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

            If FirstAnnonce Then
                Server.SendToAll(writer.GetBytes)
            Else
                Server.Send(writer.GetBytes, Index_)
            End If

        End Sub

    End Module
End Namespace
