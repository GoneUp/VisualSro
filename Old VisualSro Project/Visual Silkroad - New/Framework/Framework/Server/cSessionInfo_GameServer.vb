Public Class cSessionInfo_GameServer
    Inherits cSessionInfo_Base

    Public LoginAuthRequired As Boolean = False
    Public LoginAuthTimeout As New DateTime

    Public SRConnectionSetup As SRConnectionStatus = SRConnectionStatus.HANDSHAKE

    Public Username As String = ""
    Public Charname As String = ""

    Public Authorized As Boolean = False

    Public Enum SRConnectionStatus
        HANDSHAKE = 0
        WHOAMI = 1
        AUTH = 2
        CHARLIST = 3
        GOING_INGAME = 4
        INGAME = 5
    End Enum
End Class
