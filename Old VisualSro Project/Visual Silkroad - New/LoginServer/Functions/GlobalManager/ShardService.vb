Imports LoginServer.Framework

Namespace GlobalManager
    Module ShardService

        Public Sub OnSendServerInit()
            Dim writer As New Framework.PacketWriter
            writer.Create(ClientOpcodes.Server_Init)
            GlobalManager.Send(writer.GetBytes)
        End Sub

        Public Sub OnServerInit(ByVal packet As PacketReader)
            Dim success As Byte = packet.Byte

            If success = 1 Then
                Log.WriteSystemLog("GlobalManager: Init Complete!")
            Else
                Log.WriteSystemLog("GlobalManager: Init failed!")
                Log.WriteSystemLog("Cannot start GameServer!")
            End If
        End Sub

        Public Sub OnSendServerShutdown()
            Dim writer As New Framework.PacketWriter
            writer.Create(ClientOpcodes.Server_Shutdown)
            GlobalManager.Send(writer.GetBytes)
        End Sub

        Public Sub OnServerShutdown(ByVal packet As PacketReader)
            Dim success As Byte = packet.Byte

            If success = 1 Then
                Log.WriteSystemLog("GlobalManager: Shutdown Complete!")
            Else
                Log.WriteSystemLog("GlobalManager: WTF: Shutdown failed!")
                Log.WriteSystemLog("Cannot close GameServer ^^")
            End If
        End Sub

        Public Sub OnSendClientInfo()
            Dim writer As New Framework.PacketWriter
            writer.Create(ClientOpcodes.Server_Info)
            writer.Word(Settings.Server_Id)
            writer.Word(Server.OnlineClient)
            writer.Word(Server.MaxClients)


            GlobalManager.Send(writer.GetBytes)
        End Sub

        Public Sub OnShardInfo(ByVal packet As PacketReader)


        End Sub
    End Module
End Namespace