Namespace Agent
    Module AgentDb
        Public UserAuthCache As New Dictionary(Of UInt32, UserAuth)

    End Module

    Class UserAuth
        Public SessionId As UInt32 = 0
        Public UserIndex As UInteger = 0

        Public UserName As String = ""
        Public UserPw As String = ""
        Public UserIp As String = ""

        Public AgentServerId = 0
        Public GameServerId As UInt16 = 0
        Public ExpireTime As Date = Date.MinValue
    End Class
End Namespace