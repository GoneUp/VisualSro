Namespace GameServer.Agent
    Module AgentDb

        Public UserAuthCache As New Dictionary(Of UInt32, _UserAuth) 'Key = SessionId



    End Module

    Class _UserAuth
        Public SessionId As UInt32
        Public UserName As String
        Public UserPw As String
        Public GameServerId As UInt16
    End Class
End Namespace