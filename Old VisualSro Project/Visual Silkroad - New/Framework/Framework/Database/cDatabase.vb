Imports MySql.Data.MySqlClient
Imports System.Data
Public Class cDatabase
    Private _connection As MySqlConnection
    Private _connectionString As String
    Private ReadOnly _query As New List(Of String)
    Private ReadOnly _mysqlLock As New Object

#Region "Propertys"
    Private _DbIp As String
    Public Property DbIP As String
        Get
            Return _DbIp
        End Get
        Set(ByVal value As String)
            _DbIp = value
        End Set
    End Property

    Private _DbPort As UShort
    Public Property DbPort As UShort
        Get
            Return _DbPort
        End Get
        Set(ByVal value As UShort)
            _DbPort = value
        End Set
    End Property

    Private _DbDatabase As String
    Public Property DbDatabase As String
        Get
            Return _DbDatabase
        End Get
        Set(ByVal value As String)
            _DbDatabase = value
        End Set
    End Property

    Private _DbUsername As String
    Public Property DbUsername As String
        Get
            Return _DbUsername
        End Get
        Set(ByVal value As String)
            _DbUsername = value
        End Set
    End Property

    Private _DbPassword As String
    Public Property DbPassword As String
        Get
            Return _DbPassword
        End Get
        Set(ByVal value As String)
            _DbPassword = value
        End Set
    End Property

    Private _autoExecuteQuerys As Boolean = False
    Public Property autoExecuteQuerys As Boolean
        Get
            Return _autoExecuteQuerys
        End Get
        Set(ByVal value As Boolean)
            _autoExecuteQuerys = value
        End Set
    End Property

    Public ReadOnly Property Connected As Boolean
        Get
            If _connection IsNot Nothing Then
                If _connection.State = ConnectionState.Open Then
                    Return True
                End If
            End If

            Return False
        End Get
    End Property
#End Region

#Region "Events"
    Public Event OnDatabaseConnected As DelegateConnected
    Public Event OnDatabaseLog As DelegateLog
    Public Event OnDatabaseError As DelegateError

    Public Delegate Sub DelegateConnected()
    Public Delegate Sub DelegateLog(ByVal message As String)
    Public Delegate Sub DelegateError(ByVal ex As Exception, ByVal command As String)
#End Region

#Region "Connect"
    Public Sub Connect()
        If _connection IsNot Nothing Then
            _connection.Close()
        End If
        _connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; pooling=false;", DbIP, DbPort, DbUsername, DbPassword, DbDatabase)
        Try
            _connection = New MySqlConnection(_connectionString)
            _connection.Open()

            RaiseEvent OnDatabaseConnected()
        Catch exception As Exception
            RaiseEvent OnDatabaseError(exception, _connectionString)
        End Try
    End Sub

    Public Sub Disconnect()
        Try
            If _connection IsNot Nothing Then
                _connection.Close()
            End If
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

            Dim command2 As New MySqlCommand(command, tmpCon)
            command2.ExecuteNonQuery()

            command2.Dispose()
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
            If autoExecuteQuerys Then
                ExecuteQuerys()
            End If
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
    Public Shared Function CheckForInjection(ByVal parameter As String) As String
        Dim tmpStr As String = parameter
        tmpStr.Replace(";", "")
        tmpStr.Replace("--", "")
        tmpStr.Replace("'", "")
        tmpStr.Replace("=", "")
        Return tmpStr
    End Function


#End Region
End Class


