Module LoginServer

    Public Function GateWay(ByVal index As Integer)

        Dim writer As New GameServer.PacketWriter
        Dim name As String = "GatewayServer"
       

        writer.Create(opLoginClientInfo)
        writer.Byte(208)
        writer.Byte(0)
        writer.HexString(name)
        writer.Byte(0)

        GameServer.Server.Send(writer.GetBytes, index)

    End Function


End Module
