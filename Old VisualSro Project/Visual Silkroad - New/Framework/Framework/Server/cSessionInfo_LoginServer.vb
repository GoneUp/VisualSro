Public Class cSessionInfo_LoginServer
    Inherits cSessionInfo_Base

    Public SRConnectionSetup As SRConnectionStatus = SRConnectionStatus.HANDSHAKE

    Public Version As UInt32 = 0
    Public Locale As Byte = 0

    Public LoginTextIndex As UInt32 = 0
    Public gameserverId As UShort = 0
    Public userName As String = ""

    Public Enum SRConnectionStatus
        HANDSHAKE = 0
        WHOAMI = 1
        PATCH_INFO = 2
        LAUNCHER = 3
        LOGIN = 4
    End Enum
End Class