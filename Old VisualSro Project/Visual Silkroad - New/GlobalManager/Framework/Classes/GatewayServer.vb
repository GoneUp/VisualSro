Public Class GatewayServer
    Public ServerId As UInt32

    Public Online As Boolean = False
    Public IP As String
    Public Port As UInt16

    Public ActualUser As UInt16
    Public MaxUser As UInt16
    Public StartTime As DateTime
End Class
