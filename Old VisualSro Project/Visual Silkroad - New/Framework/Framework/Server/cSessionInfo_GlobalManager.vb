Public Class cSessionInfo_GlobalManager
    Inherits cSessionInfo_Base
    Public ProtocolVersion As UInt32 = 0
    Public BaseKey As UInt16 = 0
    Public ServerId As UInt16 = 0

    Public HandshakeComplete As Boolean = False
    Public Authorized As Boolean = False

    Public Type As _ServerTypes = _ServerTypes.Unknown

    Enum _ServerTypes
        Unknown = 0
        GlobalManager = 1
        GatewayServer = 2
        GameServer = 3
        DownloadServer = 4
        AdminTool = 5
    End Enum
End Class
