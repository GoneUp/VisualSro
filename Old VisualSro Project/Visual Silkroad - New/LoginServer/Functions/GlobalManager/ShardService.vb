Imports Framework
Imports LoginServer.Framework

Namespace GlobalManager
    Module ShardService

        Public Sub OnSendServerInit()
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.Server_Init)
            GlobalManager.Send(writer.GetBytes)
        End Sub

        Public Sub OnServerInit(ByVal packet As PacketReader)
            Dim success As Byte = packet.Byte

            If success = 1 Then
                Log.WriteSystemLog("GlobalManager: Init Complete!")
            Else
                Log.WriteSystemLog("GlobalManager: Init failed!")
                Log.WriteSystemLog("Cannot start Loginserver!")
            End If
        End Sub

        Public Sub OnSendServerShutdown()
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.Server_Shutdown)
            GlobalManager.Send(writer.GetBytes)
        End Sub

        Public Sub OnServerShutdown(ByVal packet As PacketReader)
            Dim success As Byte = packet.Byte

            If success = 1 Then
                Log.WriteSystemLog("GlobalManager: Shutdown Comfirmed!")
            Else
                Log.WriteSystemLog("GlobalManager: WTF: Shutdown failed!")
                Log.WriteSystemLog("Cannot close Loginserver ^^")
            End If
        End Sub

        Public Sub OnSendClientInfo()
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.Server_Info)
            writer.Word(Settings.Server_Id)
            writer.Word(Server.OnlineClient)
            writer.Word(Server.MaxClients)


            GlobalManager.Send(writer.GetBytes)
        End Sub

        Public Sub OnGlobalInfo(ByVal packet As PacketReader)


        End Sub
    End Module
End Namespace