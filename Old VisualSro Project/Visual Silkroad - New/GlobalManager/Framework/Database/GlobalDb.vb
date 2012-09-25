Imports SRFramework

Namespace GlobalDB
    Module GlobalDb

        'Timer
        Private WithEvents m_globalDbUpdate As New System.Timers.Timer

        'Server
        Public CertServers As New List(Of CertServer)
        Public Users As New List(Of cUser)

        Private m_initalLoad As Boolean = True

        Public Function LoadData() As Boolean Handles m_globalDbUpdate.Elapsed
            m_globalDbUpdate.Stop()
            m_globalDbUpdate.Interval = 20000 '20 secs

            Try
                If m_initalLoad = True Then
                    Log.WriteSystemLog("Loading Certification Data from Database.")

                    GetCertData()
                    GetUserData()

                    Log.WriteSystemLog("Loading Completed.")
                    m_initalLoad = False

                Else
                    GetCertData()
                    GetUserData()
                End If
            Catch ex As Exception
                Log.WriteSystemLog("[REFRESH ERROR][" & ex.Message & " Stack: " & ex.StackTrace & "]")
                Return False
            End Try

            m_globalDbUpdate.Start()
            Return True
        End Function

        Structure CertServer
            Public ServerId As UInteger
            Public TypeName As String
            Public Ip As String
        End Structure

        Private Sub GetCertData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From Certification")
            CertServers.Clear()

            For i = 0 To tmp.Tables(0).Rows.Count - 1
                Dim tmp_server As New CertServer
                tmp_server.ServerId = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                tmp_server.TypeName = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_server.Ip = CStr(tmp.Tables(0).Rows(i).ItemArray(2))

                CertServers.Add(tmp_server)
            Next
        End Sub

        ''' <summary>
        ''' Matches the data from the client with the database
        ''' </summary>
        ''' <param name="serverId"></param>
        ''' <param name="clientName"></param>
        ''' <param name="ip"></param>
        ''' <returns>True is valid</returns>
        ''' <remarks></remarks>
        Public Function CheckServerCert(ByVal serverId As UInt16, ByVal clientName As String, ByVal ip As String) As Boolean
            For i = 0 To CertServers.Count - 1
                If CertServers(i).ServerId = serverId And CertServers(i).TypeName = clientName And CertServers(i).Ip = Ip Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Sub GetUserData()

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

        Public Function GetUser(ByVal name As String) As cUser
            For i = 0 To (Users.Count - 1)
                If Users(i).Name = name Then
                    Return Users(i)
                End If
            Next
            Return Nothing
        End Function

        Public Function GetUser(ByVal accountID As UInt32) As cUser
            For i = 0 To (Users.Count - 1)
                If Users(i).AccountId = accountID Then
                    Return Users(i)
                End If
            Next
            Return Nothing
        End Function

        Public Sub UpdateUser(ByVal user As cUser)
            For i = 0 To (Users.Count - 1)
                If Users(i).AccountId = user.AccountId Then
                    Users(i) = user
                End If
            Next
        End Sub

    End Module
End Namespace
