<Serializable()>
Public Class LauncherMessage
    Public Type As LauncherMessageTypes
    Public Title As String = ""
    Public Text As String = ""
    Public Time As Date
    Public Delay As Integer = 0

    Sub New()
        Time = New DateTime
    End Sub
End Class

<Serializable()>
Public Enum LauncherMessageTypes As Byte
    Launcher = 0
    Ingame = 1
End Enum
