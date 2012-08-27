Public Class cGM_Server_Base
    Public ServerId As UInt32

    Public Online As Boolean = False
    Public IP As String
    Public Port As UInt16

    Public OnlineClients As UInt16
    Public MaxNormalClients As UInt16
    Public MaxClients As UInt16
    Public StartTime As DateTime
End Class
