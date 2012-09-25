Imports SRFramework

Module GlobalDef
    Public ReadOnly Database As New cDatabase
    Public ReadOnly Server As New ServerBase
    Public ReadOnly Log As New cLog

    Public SessionInfo(1) As cSessionInfo_GlobalManager

    Public Function Initalize(ByVal maxClients As Integer) As Boolean
        Try
            ReDim SessionInfo(maxClients)
        Catch ex As Exception
            Log.WriteSystemLog("GlobalDef Init failed! EX:" & ex.Message & " Stacktrace: " & ex.StackTrace)
            Return False
        End Try

        Return True
    End Function
End Module

