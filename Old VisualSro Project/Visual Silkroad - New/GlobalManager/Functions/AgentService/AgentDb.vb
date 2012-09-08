Namespace Agent
    Module AgentDb
        Public UserAuthCache As New Dictionary(Of UInt32, UserAuth)
        Public UserLoginCache As New Dictionary(Of String, UserLoginTry)
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
        Public LoginTime As Date = Date.MinValue
    End Class

    Class UserLoginTry
        Public SessionId As UInt32 = 0
        Public UserIndex As UInteger = 0

        Public UserName As String = ""
        Public UserPw As String = ""
        Public UserIp As String = ""
    End Class
End Namespace