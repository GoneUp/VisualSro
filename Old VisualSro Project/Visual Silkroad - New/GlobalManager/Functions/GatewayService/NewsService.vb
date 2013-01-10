Imports SRFramework

Namespace Gateway
    Module NewsService
        Public Sub RefreshNewsOnGWs()
            For i = 0 To SessionInfo.Count - 1
                If SessionInfo(i) IsNot Nothing AndAlso SessionInfo(i).Type = cSessionInfo_GlobalManager.ServerTypes.GatewayServer Then
                    OnNewsRequest(i)
                End If
            Next
        End Sub

        Public Sub OnNewsRequest(ByVal Index_ As Integer)
            Dim writer As New PacketWriter(InternalServerOpcodes.AGENT_NEWS)
            writer.Word(GlobalDB.LauncherMessages.Count)

            For Each tmpMsg In GlobalDB.LauncherMessages
                BinFormatter.Serialize(writer.BaseStream, tmpMsg)
            Next

            Server.Send(writer.GetBytes, Index_)
        End Sub
    End Module
End Namespace
