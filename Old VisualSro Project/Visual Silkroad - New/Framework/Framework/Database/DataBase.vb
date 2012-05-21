Imports MySql.Data.MySqlClient
Imports System.Data
Public Class DataBase
    Private _connection As MySqlConnection
    Private _connectionString As String
    Private ReadOnly _query As New List(Of String)
    Private ReadOnly _mysqlLock As New Object

    Public DbIp As String
    Public DbPort As UShort
    Public DbDatabase As String
    Public DbUsername As String
    Public DbPassword As String

    Public Event OnDatabaseConnected As DelegateConnected
    Public Event OnDatabaseLog As DelegateLog
    Public Event OnDatabaseError As DelegateError

    Public Delegate Sub DelegateConnected()
    Public Delegate Sub DelegateLog(ByVal message As String)
    Public Delegate Sub DelegateError(ByVal ex As Exception, ByVal command As String)

#Region "Connect"
    Public Sub Connect()
        If _connection IsNot Nothing Then
            _connection.Close()
        End If
        _connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; pooling=false;", DbIp, DbPort, DbUsername, DbPassword, DbDatabase)
        Try
            _connection = New MySqlConnection(_connectionString)
            _connection.Open()
            RaiseEvent OnDatabaseConnected()
        Catch exception As Exception
            RaiseEvent OnDatabaseError(exception, _connectionString)
        End Try
    End Sub

    Public Sub ReConnect()
        If _connection IsNot Nothing Then
            _connection.Close()
        End If
        Try
            _connection = New MySqlConnection(_connectionString)
            _connection.Open()
            RaiseEvent OnDatabaseConnected()
        Catch exception As Exception
            RaiseEvent OnDatabaseError(exception, _connectionString)
        End Try
    End Sub
#End Region

#Region "GetData"
    Public Function GetDataSet(ByVal command As String) As DataSet
        Dim tmpset As New DataSet

        Try
            Dim tmpCon As New MySqlConnection(_connectionString)
            tmpCon.Open()

            Dim reader As New MySqlDataAdapter(command, tmpCon)
            reader.Fill(tmpset)

            reader.Dispose()
            tmpCon.Close()
            tmpCon.Dispose()
        Catch ex As MySqlException
            RaiseEvent OnDatabaseError(ex, command)
        End Try
        Return tmpset
    End Function

    Public Function GetRowsCount(ByVal command As String) As Integer
        Dim count As Integer = 0
        Try
            Dim tmpSqlDataAdapter = New MySqlDataAdapter(command, _connection)
            Dim dataSet As New DataSet()
            tmpSqlDataAdapter.Fill(dataSet)
            count = dataSet.Tables(0).Rows.Count
        Catch exception As Exception
            RaiseEvent OnDatabaseError(exception, command)
        End Try
        Return count
    End Function

    Public Sub TableList()
        Dim reader As MySqlDataReader = Nothing
        Dim command As New MySqlCommand("SHOW TABLES", _connection)
        Try
            RaiseEvent OnDatabaseLog("****** Tables ******")
            reader = command.ExecuteReader()
            Do While reader.Read()
                RaiseEvent OnDatabaseLog(reader.GetString(0))
            Loop
            RaiseEvent OnDatabaseLog("********************")
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
    Public Sub InsertData(ByVal command As String)
        SyncLock _mysqlLock
            Try
                Dim command2 As New MySqlCommand(command, _connection)
                command2.ExecuteNonQuery()
                command2.Dispose()
            Catch exception As Exception
                RaiseEvent OnDatabaseError(exception, command)

                If exception.Message = "Connection must be valid and open." Then
                    ReConnect()
                End If
            End Try
        End SyncLock
    End Sub

    Public Sub InsertData(ByVal command As String, ByVal newConnection As Boolean)

        Try
            Dim tmpCon As New MySqlConnection(_connectionString)
            tmpCon.Open()

            Dim command3 As New MySqlCommand(command, tmpCon)
            command3.ExecuteNonQuery()

            command3.Dispose()
            tmpCon.Close()
            tmpCon.Dispose()

        Catch exception As Exception
            RaiseEvent OnDatabaseError(exception, command)
        End Try
    End Sub
#End Region

#Region "Query, Cache"
    Public Sub SaveQuery(ByVal command As String)
        Try
            _query.Add(command)
        Catch exception As Exception
            RaiseEvent OnDatabaseError(exception, command)
        End Try
    End Sub

    Public Sub ExecuteQuerys()
        For i = 0 To _query.Count - 1
            InsertData(_query(i))
        Next

        _query.Clear()
    End Sub


#End Region

#Region "Injection Check"
    Public Function CheckForInjection(ByVal command As String)
        If command.Contains(";") Then
            Return True
        End If
        Return False
    End Function


#End Region
End Class


