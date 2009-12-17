Imports MySql.Data.MySqlClient


Module modMySql



    Public MySQLConfig As String = "Database=visualsro;" & _
                        "Data Source=127.0.0.1;" & _
                        "User Id=sremu;Password=sremu;" & _
                        "Connection Timeout=20"


    Public Sub TestConnection()
        Try

            'Create a new Connection
            Dim connection As New MySqlConnection(MySQLConfig)

            'Test it
            connection.Open()
            connection.Close()

            'All right
            MsgBox("Connection is okay.")
        Catch ex As Exception

            'Fail!
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub retriveData()
        Try
            Dim query As String = "SELECT * FROM servers"
            Dim connection As New MySqlConnection(MySQLConfig)
            Dim cmd As New MySqlCommand(query, connection)

            connection.Open()

            Dim reader As MySqlDataReader
            reader = cmd.ExecuteReader()

            While reader.Read()
                Debug.WriteLine((reader.GetString(0) & ", " & _
                    reader.GetString(1)))
            End While

            reader.Close()
            connection.Close()
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Function updateRecord(ByVal query As String) As Integer
        Try
            Dim rowsEffected As Integer = 0
            Dim connection As New MySqlConnection(MySQLConfig)
            Dim cmd As New MySqlCommand(query, connection)

            connection.Open()

            rowsEffected = cmd.ExecuteNonQuery()

            connection.Close()

            Return rowsEffected
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Function
End Module
