Public Class cSessionInfo_GameServer
    Inherits cSessionInfo_Base

    Public LoginAuthRequired As Boolean = False
    Public LoginAuthTimeout As New DateTime

    Public Username As String = ""
    Public Charname As String = ""

    Public Authorized As Boolean = False
End Class
