Imports SRFramework

Namespace LoginDb
    Module LoginDb

        'Timer
        Public WithEvents LoginDbUpdate As New System.Timers.Timer

        'Server
        Public News As New List(Of News_)
        Public Users As New List(Of cUser)
        Public LoginInfoMessages As New List(Of LoginInfoMessage_)

        Public InitalLoad As Boolean = True

        Public Sub UpdateData() Handles LoginDbUpdate.Elapsed
            LoginDbUpdate.Stop()
            LoginDbUpdate.Interval = 20000 '20 secs


            Try
                If InitalLoad = True Then
                    Log.WriteSystemLog("Load Data from Database.")

                    GetNewsData()
                    GetUserData()

                    Log.WriteSystemLog("Loading Completed.")
                    InitalLoad = False

                Else
                    GetNewsData()
                    GetUserData()
                End If

            Catch ex As Exception
                Log.WriteSystemLog("[REFRESH ERROR][" & ex.Message & " Stack: " & ex.StackTrace & "]")
            End Try

            LoginDbUpdate.Start()
        End Sub

        Structure News_
            Public NewsNumber As Integer
            Public Title As String
            Public Text As String
            Public Time As Date
        End Structure

        Structure LoginInfoMessage_
            Public Text As String
            Public Delay As Integer
        End Structure

        Public Sub GetNewsData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From News")
            News.Clear()

            For i = 0 To tmp.Tables(0).Rows.Count - 1
                If CInt(tmp.Tables(0).Rows(i).ItemArray(1)) = 0 Then
                    'Type, 0=Launcher,1=Login Info Messages Subsystem
                    Dim tmpNews As New News_
                    tmpNews.Title = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
                    tmpNews.Text = CStr(tmp.Tables(0).Rows(i).ItemArray(3))
                    tmpNews.Time = CDate(tmp.Tables(0).Rows(i).ItemArray(4))

                    News.Add(tmpNews)
                ElseIf CInt(tmp.Tables(0).Rows(i).ItemArray(1)) = 1 Then
                    Dim tmpMsg As New LoginInfoMessage_
                    tmpMsg.Delay = CInt(tmp.Tables(0).Rows(i).ItemArray(2))
                    tmpMsg.Text = CStr(tmp.Tables(0).Rows(i).ItemArray(3))

                    LoginInfoMessages.Add(tmpMsg)
                End If


            Next
        End Sub

        Public Sub GetUserData()

            Dim tmp As DataSet = Database.GetDataSet("SELECT * From Users")
            Users.Clear()

            For i = 0 To tmp.Tables(0).Rows.Count - 1
                Dim tmpUser As New cUser
                tmpUser.AccountId = CInt(tmp.Tables(0).Rows(i).ItemArray(0))
                tmpUser.Name = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmpUser.Pw = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
                tmpUser.FailedLogins = CInt(tmp.Tables(0).Rows(i).ItemArray(3))
                tmpUser.Banned = CBool(tmp.Tables(0).Rows(i).ItemArray(4))
                tmpUser.BannReason = CStr(tmp.Tables(0).Rows(i).ItemArray(5))
                tmpUser.BannTime = CDate(tmp.Tables(0).Rows(i).ItemArray(6))
                tmpUser.Silk = CUInt(tmp.Tables(0).Rows(i).ItemArray(7))
                tmpUser.Permission = CBool(tmp.Tables(0).Rows(i).ItemArray(8))

                Users.Add(tmpUser)
            Next
        End Sub



        Public Function GetUser(ByVal id As String) As Integer
            For i = 0 To (Users.Count - 1)
                If Users(i).Name = id Then
                    Return i
                End If
            Next
            Return -1
        End Function
    End Module
End Namespace
