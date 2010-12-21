Namespace LoginServer
    Module LoginDb

        'Timer
        Public WithEvents LoginDbUpdate As New Timers.Timer

        'Server
        Public Servers As New List(Of Server_)
        Public News As New List(Of News_)
        Public Users As New List(Of UserArray)

        Private First As Boolean

        Public Sub UpdateData() Handles LoginDbUpdate.Elapsed

            Try
                LoginDbUpdate.Stop()
                LoginDbUpdate.Interval = 20000 '20 secs

                If First = False Then
                    Log.WriteSystemLog("Load Data from Database.")
                End If

                GetServerData()
                GetNewsData()
                GetUserData()

                If First = False Then
                    Log.WriteSystemLog("Loading Completed.")
                    First = True
                End If

                LoginDbUpdate.Start()

            Catch ex As Exception
                Log.WriteSystemLog("[REFRESH ERROR][" & ex.Message & " Stack: " & ex.StackTrace & "]")
                LoginDbUpdate.Start()
            End Try
        End Sub

        Structure Server_
            Public ServerId As UInteger
            Public Name As String
            Public AcUs As UInt16
            Public MaxUs As UInt16
            Public State As Byte
            Public IP As String
            Public Port As UInt16
        End Structure
        Public Sub GetServerData()
            Dim tmp As DataSet = LoginServer.Database.GetDataSet("SELECT * From Servers")
            Servers.Clear()

            For i = 0 To tmp.Tables(0).Rows.Count - 1
                Dim tmp_server As New Server_
                tmp_server.ServerId = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                tmp_server.Name = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_server.AcUs = CUInt(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_server.MaxUs = CUInt(tmp.Tables(0).Rows(i).ItemArray(3))
                tmp_server.State = CByte(tmp.Tables(0).Rows(i).ItemArray(4))
                tmp_server.IP = CStr(tmp.Tables(0).Rows(i).ItemArray(5))
                tmp_server.Port = CUInt(tmp.Tables(0).Rows(i).ItemArray(6))

                Servers.Add(tmp_server)
            Next
        End Sub

        Structure News_
            Public NewsNumber As Integer
            Public Title As String
            Public Text As String
            Public Time As Date
        End Structure

        Public Sub GetNewsData()
            Dim tmp As DataSet = LoginServer.Database.GetDataSet("SELECT * From News")
            News.Clear()

            For i = 0 To tmp.Tables(0).Rows.Count - 1
                Dim tmp_news As New News_
                tmp_news.Title = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_news.Text = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_news.Time = CDate(tmp.Tables(0).Rows(i).ItemArray(3))

                News.Add(tmp_news)
            Next
        End Sub


        Structure UserArray
            Public AccountId As Integer
            Public Name As String
            Public Pw As String
            Public FailedLogins As Integer
            Public Banned As Boolean
            Public BannTime As Date
            Public BannReason As String
        End Structure
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
End Namespace
