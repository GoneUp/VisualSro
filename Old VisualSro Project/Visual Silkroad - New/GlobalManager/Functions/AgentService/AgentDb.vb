Namespace Agent
    Module AgentDb
        Public UserAuthCache As New Dictionary(Of UInt32, _UserAuth)

    End Module

    Class _UserAuth
        Public SessionId As UInt32
        Public UserIndex As UInteger
        Public UserName As String
        Public UserPw As String
        Public GameServerId As UInt16
        Public ExpireTime As Date
    End Class
End Namespace