Module LoginDb

    'Timer
    Public WithEvents LoginDbUpdate As New Timers.Timer

    'Server
    Public ServerNumber As Integer
    Public ServerID(50) As UInteger
    Public ServerName(50) As String
    Public ServerAcUs(50) As UInt16
    Public ServerMaxUs(50) As UInt16
    Public ServerState(50) As Byte
    Public ServerIP(50) As String
    Public ServerPort(50) As UInt16


    'News
    Public NewsNumber As Integer
    Public NewsTitle(50) As String
    Public NewsText(50) As String
    Public NewsMonth(50) As Byte
    Public NewsDay(50) As Byte

    'User
    Public Users As New List(Of UserArray)
    Public UserIdCounter As Integer


    Public Structure UserArray
        Public Id As Integer
        Public Name As String
        Public Pw As String
        Public FailedLogins As Integer
        Public Banned As Boolean
        Public BannTime As Date
        Public BannReason As String
    End Structure

    Private First As Boolean


    Public Sub UpdateData() Handles LoginDbUpdate.Elapsed

        Try
            LoginDbUpdate.Stop()
            LoginDbUpdate.Interval = 20000 '20 secs

            If First = False Then
                Commands.WriteLog("Load Data from Database.")
            End If

            GetServerData()
            GetNewsData()
            GetUserData()

            If First = False Then
                Commands.WriteLog("Loading Completed.")
                First = True
            End If

            LoginDbUpdate.Start()

        Catch ex As Exception
            Commands.WriteLog("[REFRESH ERROR][" & ex.Message & " Stack: " & ex.StackTrace & "]")
        End Try
    End Sub


    Public Sub GetServerData()

        Dim tmp As DataSet = LoginServer.Database.GetDataSet("SELECT * From Servers")
        ServerNumber = tmp.Tables(0).Rows.Count

        ReDim ServerID(ServerNumber)
        ReDim ServerName(ServerNumber)
        ReDim ServerAcUs(ServerNumber)
        ReDim ServerMaxUs(ServerNumber)
        ReDim ServerState(ServerNumber)
        ReDim ServerIP(ServerNumber)
        ReDim ServerPort(ServerNumber)

        For i = 0 To ServerNumber - 1
            ServerID(i) = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
            ServerName(i) = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
            ServerAcUs(i) = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
            ServerMaxUs(i) = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
            ServerState(i) = CByte(tmp.Tables(0).Rows(i).ItemArray(4))
            ServerIP(i) = CStr(tmp.Tables(0).Rows(i).ItemArray(5))
            ServerPort(i) = CUInt(tmp.Tables(0).Rows(i).ItemArray(6))
        Next





    End Sub

    Public Sub GetNewsData()

        Dim tmp As DataSet = LoginServer.Database.GetDataSet("SELECT * From News")
        NewsNumber = tmp.Tables(0).Rows.Count

        ReDim NewsTitle(NewsNumber)
        ReDim NewsText(NewsNumber)
        ReDim NewsDay(NewsNumber)
        ReDim NewsMonth(NewsNumber)


        For i = 0 To NewsNumber - 1
            NewsTitle(i) = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
            NewsText(i) = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
            NewsDay(i) = CByte(tmp.Tables(0).Rows(i).ItemArray(3))
            NewsMonth(i) = CByte(tmp.Tables(0).Rows(i).ItemArray(4))
        Next



    End Sub

    Public Sub GetUserData()

        Dim tmp As DataSet = LoginServer.Database.GetDataSet("SELECT * From Users")
        Users.Clear()

        For i = 0 To tmp.Tables(0).Rows.Count - 1
            Dim tmpUser As New UserArray
            tmpUser.Id = CInt(tmp.Tables(0).Rows(i).ItemArray(0))
            tmpUser.Name = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
            tmpUser.Pw = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
            tmpUser.FailedLogins = CInt(tmp.Tables(0).Rows(i).ItemArray(3))
            tmpUser.Banned = CBool(tmp.Tables(0).Rows(i).ItemArray(4))
            tmpUser.BannReason = CStr(tmp.Tables(0).Rows(i).ItemArray(5))
            tmpUser.BannTime = CDate(tmp.Tables(0).Rows(i).ItemArray(6))

            Users.Add(tmpUser)
        Next

    End Sub

    Public Function GetUserWithID(ByVal id As String) As Integer

        Dim i As Integer = 0

        For i = 0 To (Users.Count - 1)
            If Users(i).Name = id Then
                Exit For
            End If
        Next

 
        If Users.Count = i Then
            Return -1
        End If

        Return i
    End Function



    Public Function GetServerIndexById(ByVal id As Integer)
        For i = 0 To ServerID.Length
            If ServerID(i) = id Then
                Return i
            End If
        Next
    End Function
End Module
