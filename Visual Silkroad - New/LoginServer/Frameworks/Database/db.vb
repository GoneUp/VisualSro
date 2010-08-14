Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
	Imports System
	Imports System.Data
	Imports System.Runtime.CompilerServices
Namespace LoginServer

    Public Class db
        Private Shared connection As MySqlConnection
        Private Shared ConnectionString As String
        Private Shared da As MySqlDataAdapter
        Private Shared gets As String
        Private Shared getsDb As Double
        Private Shared getsInt As Integer

        Public Shared Event OnConnectedToDatabase As dConnected

        Public Shared Event OnDatabaseError As dError



        Public Shared Sub Connect(ByVal ip As String, ByVal port As Integer, ByVal database As String, ByVal username As String, ByVal password As String)
            If connection IsNot Nothing Then
                connection.Close()
            End If
            ConnectionString = String.Format("server={0};port={4} ;user id={1}; password={2}; database={3}; pooling=false;", New Object() {ip, username, password, database, port})
            Try
                connection = New MySqlConnection(ConnectionString)
                connection.Open()
                RaiseEvent OnConnectedToDatabase()
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception)
            End Try
        End Sub

        Public Shared Sub DeleteData(ByVal command As String)
            InsertData(command)
        End Sub

        Public Shared Function GetData(ByVal command As String, ByVal index As Integer) As String
            Dim reader As MySqlDataReader = Nothing
            Dim command2 As New MySqlCommand(command, connection)
            Try
                reader = command2.ExecuteReader()
                Do While reader.Read()
                    gets = reader.GetString(index)
                Loop
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception)
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try
            Return gets
        End Function

        Public Shared Function GetData(ByVal command As String, ByVal column As String) As String
            Dim str As String = Nothing
            Dim reader As MySqlDataReader = Nothing
            Dim command2 As New MySqlCommand(command, connection)
            Try
                reader = command2.ExecuteReader()
                Do While reader.Read()
                    str = reader.GetString(column)
                Loop
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception)
            Finally
                If reader IsNot Nothing Then
                    reader.Dispose()
                    reader.Close()
                End If
            End Try
            Return str
        End Function

        Public Shared Function GetData(ByVal command As String, ByVal column As String, ByVal index As Integer) As String
            Dim reader As MySqlDataReader = New MySqlCommand(command, connection).ExecuteReader()
            reader.Read()
            gets = reader(column).ToString()
            reader.Close()
            Return gets
        End Function

        Public Shared Function GetDataDouble(ByVal command As String, ByVal column As String) As Double
            Dim reader As MySqlDataReader = Nothing
            Dim command2 As New MySqlCommand(command, connection)
            Try
                reader = command2.ExecuteReader()
                Do While reader.Read()
                    getsDb = reader.GetDouble(column)
                Loop
            Catch exception As MySqlException
                RaiseEvent OnDatabaseError(exception)
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try
            Return getsDb
        End Function
        Public Shared Function GetDataSet(ByVal command As String) As DataSet


            Dim reader As New MySqlDataAdapter(command, connection)
            Dim tmpset As New DataSet

            Try
                reader.Fill(tmpset)
            Catch ex As MySqlException
                RaiseEvent OnDatabaseError(ex)
            End Try




            Return tmpset
        End Function



        Public Shared Function GetDataInt(ByVal command As String, ByVal column As String) As Integer
            Dim reader As MySqlDataReader = Nothing
            Dim command2 As New MySqlCommand(command, connection)
            Dim num As Integer = 0
            Try
                reader = command2.ExecuteReader()
                Do While reader.Read()
                    num = System.Convert.ToInt32(reader.GetString(column))
                Loop
            Catch exception As MySqlException
                RaiseEvent OnDatabaseError(exception)
            Finally
                If reader IsNot Nothing Then
                    reader.Dispose()
                    reader.Close()
                End If
            End Try
            Return num
        End Function

        Public Shared Function GetDataInt(ByVal command As String, ByVal column As String, ByVal index As Integer) As Integer
            Dim reader As MySqlDataReader = New MySqlCommand(command, connection).ExecuteReader()
            reader.Read()
            getsInt = System.Convert.ToInt32(reader(column))
            reader.Close()
            Return getsInt
        End Function


        Public Shared Function GetList(ByVal Command As String, ByVal column As String, ByVal i As Integer) As String
            Dim reader As MySqlDataReader = Nothing
            Dim command_14 As New MySqlCommand(Command, connection)
            Try
                reader = command_14.ExecuteReader()
                Do While reader.Read()
                    gets = gets & reader.GetString("name") & " "
                Loop
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception)
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try
            Return gets
        End Function

        Public Shared Function GetRowsCount(ByVal command As String) As Integer
            Dim count As Integer = 0
            Try
                da = New MySqlDataAdapter(command, connection)
                Dim dataSet As New DataSet()
                da.Fill(dataSet)
                count = dataSet.Tables(0).Rows.Count
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception)
            End Try
            Return count
        End Function

        Public Shared Sub InsertData(ByVal command As String)
            Dim command2 As New MySqlCommand(command, connection)
            Try
                command2.ExecuteNonQuery()
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception)
            End Try
        End Sub

        Public Shared Sub TableList()
            Dim reader As MySqlDataReader = Nothing
            Dim command As New MySqlCommand("SHOW TABLES", connection)
            Try
                Console.WriteLine("****** Tables ******")
                reader = command.ExecuteReader()
                Do While reader.Read()
                    Console.WriteLine(reader.GetString(0))
                Loop
                Console.WriteLine("********************")
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception)
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try
        End Sub

        Public Shared Sub UpdateData(ByVal command As String)
            InsertData(command)
        End Sub

        Public Delegate Sub dConnected()

        Public Delegate Sub dError(ByVal ex As Exception)
    End Class
End Namespace

