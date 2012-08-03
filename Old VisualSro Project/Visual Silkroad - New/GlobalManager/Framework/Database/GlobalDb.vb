Namespace GlobalDb
    Module GlobalDb

        'Timer
        Public WithEvents GlobalDbUpdate As New System.Timers.Timer

        'Server
        Public Servers As New List(Of Server_)
        Public CertServers As New List(Of _CertServer)

        Private First As Boolean

        Public Sub UpdateData() Handles GlobalDbUpdate.Elapsed
            GlobalDbUpdate.Stop()
            GlobalDbUpdate.Interval = 20000 '20 secs

            Try

                Log.WriteSystemLog("Loading Certification Data from Database.")

                GetServerData()
                GetCertData()

                Log.WriteSystemLog("Loading Completed.")


            Catch ex As Exception
                Log.WriteSystemLog("[REFRESH ERROR][" & ex.Message & " Stack: " & ex.StackTrace & "]")
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
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From Servers")
            Servers.Clear()

            For i = 0 To tmp.Tables(0).Rows.Count - 1
                Dim tmp_server As New Server_
                tmp_server.ServerId = CUInt(tmp.Tables(0).Rows(i).ItemArray(0))
                tmp_server.Name = CStr(tmp.Tables(0).Rows(i).ItemArray(1))
                tmp_server.AcUs = CUShort(tmp.Tables(0).Rows(i).ItemArray(2))
                tmp_server.MaxUs = CUShort(tmp.Tables(0).Rows(i).ItemArray(3))
                tmp_server.State = CByte(tmp.Tables(0).Rows(i).ItemArray(4))
                tmp_server.IP = CStr(tmp.Tables(0).Rows(i).ItemArray(5))
                tmp_server.Port = CUShort(tmp.Tables(0).Rows(i).ItemArray(6))

                Servers.Add(tmp_server)
            Next
        End Sub


        Structure _CertServer
            Public ServerId As UInteger
            Public TypeName As String
            Public Ip As String
        End Structure
        Public Sub GetCertData()
            Dim tmp As DataSet = Database.GetDataSet("SELECT * From Certification")
            Servers.Clear()

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
    End Module
End Namespace
