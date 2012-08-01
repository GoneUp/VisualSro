Imports SRFramework

Module GlobalDef
    Public Database As New cDatabase

    Public ClientList As cClientList

    Public SessionInfo(1) As cSessionInfo_GlobalManager

    Public Sub Initalize(ByVal maxClients As Integer)
        ReDim SessionInfo(maxClients)
    End Sub
End Module

