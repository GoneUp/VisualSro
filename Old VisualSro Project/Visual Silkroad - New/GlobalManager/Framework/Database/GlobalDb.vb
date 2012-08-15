Imports SRFramework

Namespace GlobalDb
    Module GlobalDb

        'Timer
        Public WithEvents GlobalDbUpdate As New System.Timers.Timer

        'Server
        Public CertServers As New List(Of _CertServer)
        Public Users As New List(Of cUser)

        Private InitalLoad As Boolean = True

        Public Sub UpdateData() Handles GlobalDbUpdate.Elapsed
            GlobalDbUpdate.Stop()
            GlobalDbUpdate.Interval = 20000 '20 secs

            Try
                If InitalLoad = True Then
                    Log.WriteSystemLog("Loading Certification Data from Database.")

                    GetCertData()
                    GetUserData()

                    Log.WriteSystemLog("Loading Completed.")
                    InitalLoad = False

                Else
                    GetCertData()
                    GetUserData()
                End If
            Catch ex As Exception
                Log.WriteSystemLog("[REFRESH ERROR][" & ex.Message & " Stack: " & ex.StackTrace & "]")
            End Try

            GlobalDbUpdate.Start()
        End Sub

        Structure _CertServer
            Public ServerId As UInteger
            Public TypeName As String
            Public Ip As String
        End Structure
        Public Sub GetCertData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From Certification")
            CertServers.Clear()

            For i = 0 To tmp.Tables(0).Rows.Count - 1
                Dim tmp_server As New _CertServer
                tmp_server.ServerId = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                tmp_server.TypeName = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_server.Ip = CStr(tmp.Tables(0).Rows(i).ItemArray(2))

                CertServers.Add(tmp_server)
            Next
        End Sub

        ''' <summary>
        ''' Matches the data from the client with the database
        ''' </summary>
        ''' <param name="ServerId"></param>
        ''' <param name="ClientName"></param>
        ''' <param name="Ip"></param>
        ''' <returns>True is valid</returns>
        ''' <remarks></remarks>
        Public Function CheckServerCert(ByVal ServerId As UInt16, ByVal ClientName As String, ByVal Ip As String) As Boolean
            For i = 0 To CertServers.Count - 1
                If CertServers(i).ServerId = ServerId And CertServers(i).TypeName = ClientName And CertServers(i).Ip = Ip Then
                    Return True
                End If
            Next
            Return False
        End Function

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
                tmpUser.Silk_Bonus = CUInt(tmp.Tables(0).Rows(i).ItemArray(8))
                tmpUser.Silk_Points = CUInt(tmp.Tables(0).Rows(i).ItemArray(9))
                tmpUser.Permission = CBool(tmp.Tables(0).Rows(i).ItemArray(10))
                tmpUser.StorageSlots = Convert.ToByte(tmp.Tables(0).Rows(i).ItemArray(11))

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
