Namespace LoginDb
    Module LoginDb

        'Server
        Public ReadOnly News As New List(Of LauncherMessage)
        Public ReadOnly LoginInfoMessages As New List(Of LoginInfoMessage)


        Public Function LoadData() As Boolean
            Try
                Log.WriteSystemLog("Loading Data from Database.")

                GetNewsData()

                Log.WriteSystemLog("Loading Completed.")

            Catch ex As Exception
                Log.WriteSystemLog("Data loading failed! M: " & ex.Message & " Stacktrace: " & ex.StackTrace)
                Return False
            End Try

            Return True
        End Function

        Public Structure LauncherMessage
            Public Title As String
            Public Text As String
            Public Time As Date
        End Structure

        Structure LoginInfoMessage
            Public Text As String
            Public Delay As Integer
        End Structure

        Private Sub GetNewsData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From News")
            News.Clear()

            For i = 0 To tmp.Tables(0).Rows.Count - 1
                If CInt(tmp.Tables(0).Rows(i).ItemArray(1)) = 0 Then
                    'Type, 0=Launcher,1=Login Info Messages Subsystem
                    Dim tmpNews As New LauncherMessage
                    tmpNews.Title = CStr(tmp.Tables(0).Rows(i).ItemArray(2))
                    tmpNews.Text = CStr(tmp.Tables(0).Rows(i).ItemArray(3))
                    tmpNews.Time = CDate(tmp.Tables(0).Rows(i).ItemArray(4))

                    News.Add(tmpNews)
                ElseIf CInt(tmp.Tables(0).Rows(i).ItemArray(1)) = 1 Then
                    Dim tmpMsg As New LoginInfoMessage
                    tmpMsg.Delay = CInt(tmp.Tables(0).Rows(i).ItemArray(2))
                    tmpMsg.Text = CStr(tmp.Tables(0).Rows(i).ItemArray(3))

                    LoginInfoMessages.Add(tmpMsg)
                End If


            Next
        End Sub
    End Module
End Namespace
