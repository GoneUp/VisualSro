Module LoginDb

    'Timer
    Public WithEvents LoginDbUpdate As New Timers.Timer

    'Server
    Public Servers As New List(Of Server_)
    Structure Server_
        Public ServerId As UInteger
        Public ServerName As String
        Public ServerAcUs As UInt16
        Public ServerMaxUs As UInt16
        Public ServerState As Byte
        Public ServerIP As String
        Public ServerPort As UInt16
    End Structure


    'News
    Public News As New List(Of News_)
    Structure News_
        Public NewsNumber As Integer
        Public NewsTitle As String
        Public NewsText As String
        Public NewsMonth As Byte
        Public NewsDay As Byte
    End Structure

    'User
    Public Users As New List(Of UserArray)
    Public UserIdCounter As Integer

    Structure UserArray
        Public AccountId As Integer
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

        For i = 0 To tmp.Tables(0).Rows.Count - 1
            Dim tmp_server As New Server_
            tmp_server.ServerId = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
            tmp_server.ServerName = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
            tmp_server.ServerAcUs = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
            tmp_server.ServerMaxUs = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
            tmp_server.ServerState = CByte(tmp.Tables(0).Rows(i).ItemArray(4))
            tmp_server.ServerIP = CStr(tmp.Tables(0).Rows(i).ItemArray(5))
            tmp_server.ServerPort = CUInt(tmp.Tables(0).Rows(i).ItemArray(6))

            Servers.Add(tmp_server)
        Next


    End Sub

    Public Sub GetNewsData()

        Dim tmp As DataSet = LoginServer.Database.GetDataSet("SELECT * From News")

        For i = 0 To tmp.Tables(0).Rows.Count - 1
            Dim tmp_news As New News_
            tmp_news.NewsTitle = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
            tmp_news.NewsText = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
            tmp_news.NewsDay = CByte(tmp.Tables(0).Rows(i).ItemArray(3))
            tmp_news.NewsMonth = CByte(tmp.Tables(0).Rows(i).ItemArray(4))

            News.Add(tmp_news)
        Next



    End Sub

    Public Sub GetUserData()

        Dim tmp As DataSet = LoginServer.Database.GetDataSet("SELECT * From Users")
        Users.Clear()

        For i = 0 To tmp.Tables(0).Rows.Count - 1
            Dim tmpUser As New UserArray
            tmpUser.AccountId = CInt(tmp.Tables(0).Rows(i).ItemArray(0))
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
        For i = 0 To Servers.Count - 1
            If Servers(i).ServerId = id Then
                Return i
            End If
        Next
    End Function
End Module
