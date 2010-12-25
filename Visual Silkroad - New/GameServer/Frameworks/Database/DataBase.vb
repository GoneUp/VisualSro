Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
	Imports System
	Imports System.Data
	Imports System.Runtime.CompilerServices
Namespace GameServer

    Public Class DataBase
        Private Shared Connection As MySqlConnection
        Private Shared ConnectionString As String
        Private Shared da As MySqlDataAdapter
        Private Shared gets As String
        Private Shared getsDb As Double
        Private Shared getsInt As Integer
        Private Shared Query As New List(Of String)

        Public Shared Event OnConnectedToDatabase As dConnected
        Public Shared Event OnDatabaseError As dError

        Public Delegate Sub dConnected()
        Public Delegate Sub dError(ByVal ex As Exception, ByVal command As String)

#Region "Connect"
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
                RaiseEvent OnDatabaseError(exception, ConnectionString)
            End Try
        End Sub

        Public Shared Sub ReConnect()
            If connection IsNot Nothing Then
                connection.Close()
            End If
            Try
                connection = New MySqlConnection(ConnectionString)
                connection.Open()
                RaiseEvent OnConnectedToDatabase()
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception, ConnectionString)
            End Try
        End Sub
#End Region

#Region "Unused"
        Public Shared Function GetDataSet(ByVal command As String) As DataSet
            Dim tmpset As New DataSet

            Try
                Dim tmp_con As New MySqlConnection(ConnectionString)
                tmp_con.Open()

                Dim reader As New MySqlDataAdapter(command, tmp_con)
                reader.Fill(tmpset)

                tmp_con.Close()

            Catch ex As MySqlException
                RaiseEvent OnDatabaseError(ex, command)
            End Try
            Return tmpset
        End Function

        Public Shared Function GetRowsCount(ByVal command As String) As Integer
            Dim count As Integer = 0
            Try
                da = New MySqlDataAdapter(command, connection)
                Dim dataSet As New DataSet()
                da.Fill(dataSet)
                count = dataSet.Tables(0).Rows.Count
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception, command)
            End Try
            Return count
        End Function

        Public Shared Sub TableList()
            Dim reader As MySqlDataReader = Nothing
            Dim command As New MySqlCommand("SHOW TABLES", connection)
            Try
                Log.WriteSystemLog("****** Tables ******")
                reader = command.ExecuteReader()
                Do While reader.Read()
                    Log.WriteSystemLog(reader.GetString(0))
                Loop
                Log.WriteSystemLog("********************")
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception, command.CommandText)
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try
        End Sub
#End Region

#Region "Insert/update"
        Public Shared Sub InsertData(ByVal command As String)
            Dim command2 As New MySqlCommand(command, connection)
            Try
                command2.ExecuteNonQuery()


            Catch exception As MySqlException
                If exception.ErrorCode = -2147467259 Then
                    Dim tmp_con As New MySqlConnection(ConnectionString)
                    tmp_con.Open()

                    Dim command3 As New MySqlCommand(command, tmp_con)
                    'command3.ExecuteNonQuery()

                    tmp_con.Close()
                End If

            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception, command)
            End Try
        End Sub
#End Region

#Region "Query"

        Public Shared Sub SaveQuery(ByVal command As String)
            Try
                Query.Add(command)
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception, command)
            End Try
        End Sub

        Public Shared Sub ExecuteQuerys()
            For i = 0 To Query.Count - 1
                InsertData(Query(i))
            Next

            Query.Clear()
        End Sub


#End Region
    End Class
End Namespace

