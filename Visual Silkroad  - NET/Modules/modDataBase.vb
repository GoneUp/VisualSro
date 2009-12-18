Option Strict Off
Option Explicit On
'import mySQL
Imports MySql.Data.MySqlClient

Module modDataBase


#Region "Old Stuff"
    Public rstConnGenericRecordset As ADODB.Connection
    Public strConnectionGenericRecordset As String
    Public rstGenericRecordset As ADODB.Recordset

    Public rstGetRecord As ADODB.Recordset
    Public rstCmdRecord As ADODB.Command
    Public strSQL2 As String
    Public strSQL As String
    Public rstConnGenericRecordset2 As ADODB.Connection
    Public strConnectionGenericRecordset2 As String
    Public rstGenericRecordset2 As ADODB.Recordset

    Public rstGetRecord2 As ADODB.Recordset
    Public rstCmdRecord2 As ADODB.Command
    Public DataBases As DAO.Database
    Public strSQL3 As String
    Public rstConnGenericRecordset3 As ADODB.Connection
    Public strConnectionGenericRecordset3 As String
    Public rstGenericRecordset3 As ADODB.Recordset

    Public rstGetRecord3 As ADODB.Recordset
    Public rstCmdRecord3 As ADODB.Command


    Public Sub OpenSremuDataBase()

        DataBases = DAODBEngine_definst.OpenDatabase(GetAppPath() & "DataBase\SremuDatabase.mdb")

    End Sub
    Public Sub CloseSkillDatabase()

        DataBases.Close()
        'UPGRADE_NOTE: Das Objekt DataBases kann erst dann gelöscht werden, wenn die Garbagecollection durchgeführt wurde. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        DataBases = Nothing

    End Sub
    Public Function GetAppPath() As String

        GetAppPath = IIf(Right(My.Application.Info.DirectoryPath, 1) = "\", My.Application.Info.DirectoryPath, My.Application.Info.DirectoryPath & "\")

    End Function

    Public Sub OpenDB()
        Dim strProvider As String
        Dim strSource As String

        strProvider = "Provider=Microsoft.Jet.OLEDB.4.0;"
        strSource = "Data Source=" & GetAppPath() & "\DataBase\SremuDatabase.mdb;"

        rstConnGenericRecordset = New ADODB.Connection
        'strConnectionGenericRecordset = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strServerDBPath & "\GenericRecordset.mdb;Persist Security Info=False"
        strConnectionGenericRecordset = strProvider & strSource & "Persist Security Info=False"
        rstConnGenericRecordset.Open(strConnectionGenericRecordset)

        rstGenericRecordset = New ADODB.Recordset

        With rstGenericRecordset
            .let_ActiveConnection(rstConnGenericRecordset)
            .CursorLocation = ADODB.CursorLocationEnum.adUseClient
            .CursorType = ADODB.CursorTypeEnum.adOpenKeyset
            .LockType = ADODB.LockTypeEnum.adLockOptimistic
            .Open("ShopData")
        End With
        rstGenericRecordset2 = New ADODB.Recordset

        With rstGenericRecordset2
            .let_ActiveConnection(rstConnGenericRecordset)
            .CursorLocation = ADODB.CursorLocationEnum.adUseClient
            .CursorType = ADODB.CursorTypeEnum.adOpenKeyset
            .LockType = ADODB.LockTypeEnum.adLockOptimistic
            .Open("NpcAll")
        End With
        'With rstGenericRecordset3
        '.ActiveConnection = rstConnGenericRecordset
        '.CursorLocation = adUseClient
        '.CursorType = adOpenKeyset
        '.LockType = adLockOptimistic
        '.Open "SkillData_5000"
        'End With
    End Sub
    Public Sub OpenDB2()
        Dim strProvider As String
        Dim strSource As String

        strProvider = "Provider=Microsoft.Jet.OLEDB.4.0;"
        strSource = "Data Source=" & My.Application.Info.DirectoryPath & "\DataBase\SremuDatabase.mdb;"

        rstConnGenericRecordset = New ADODB.Connection
        'strConnectionGenericRecordset = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strServerDBPath & "\GenericRecordset.mdb;Persist Security Info=False"
        strConnectionGenericRecordset = strProvider & strSource & "Persist Security Info=False"
        rstConnGenericRecordset.Open(strConnectionGenericRecordset)

        rstGenericRecordset = New ADODB.Recordset

        With rstGenericRecordset
            .let_ActiveConnection(rstConnGenericRecordset)
            .CursorLocation = ADODB.CursorLocationEnum.adUseClient
            .CursorType = ADODB.CursorTypeEnum.adOpenKeyset
            .LockType = ADODB.LockTypeEnum.adLockOptimistic
            .Open("ShopItem")
        End With
    End Sub

#End Region

#Region "New MySQL Stuff"

    'Now u can easy edit it 
    Public DbName As String = "visualsro"
    Public DbIP As String = "127.0.0.1"
    Public DbUser As String = "visro"
    Public DbPassword As String = "visro"

    Public MySQLConfig As String = "Database=" & DbName & _
                        ";Data Source=" & DbIP & _
                        ";User Id=" & DbUser & _
                        ";Password=" & DbPassword & _
                        "Connection Timeout=20"

    Public Server1() As String = {"", "", "", "", "", "", ""}
    Public Server2() As String = {"", "", "", "", "", "", ""}

    'Examples to use our DB

    'Update               [Table]     [New Name]  [Where it must update]
    'updateRecord("UPDATE Country SET Name='Test2' WHERE Code ='AAA'"

    'Insert                [Table]     []
    'updateRecord("INSERT INTO Country (Code, Name) VALUES ('AAA','Test Name')")

    'Delete
    'updateRecord("DELETE FROM Country WHERE Code ='AAA'")

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

    Function retriveDataServer() As String
        'Gets the server data

        Try
            Dim query As String = "SELECT * FROM servers"
            Dim connection As New MySqlConnection(MySQLConfig)
            Dim cmd As New MySqlCommand(query, connection)

            connection.Open()

            Dim reader As MySqlDataReader
            reader = cmd.ExecuteReader()

            Dim Server1() As String = {"", "", "", "", "", "", ""}
            Dim Server2() As String = {"", "", "", "", "", "", ""}




            While reader.Read
                Dim i As Integer = 1

                'Only 2 server supportet for now 

                If i = 1 Then
                    'Server 1
                    'Server1(1) = reader.GetValues("id") 'ID
                    Server1(2) = reader.GetString(2) 'Name
                    Server1(3) = reader.GetString(3) 'User_currecct
                    Server1(4) = reader.GetString(4) 'User_max
                    Server1(5) = reader.GetString(5) 'State
                    Server1(6) = reader.GetString(6) 'IP
                    Server1(7) = reader.GetString(7) 'Port
                    i = +1

                ElseIf i = 2 Then
                    'Server 2
                    Server2(1) = reader.GetString(1) 'ID
                    Server2(2) = reader.GetString(2) 'Name
                    Server2(3) = reader.GetString(3) 'User_currecct
                    Server2(4) = reader.GetString(4) 'User_max
                    Server2(5) = reader.GetString(5) 'State
                    Server2(6) = reader.GetString(6) 'IP
                    Server2(7) = reader.GetString(7) 'Port
                    i = +1

                End If

            End While

            reader.Close()
            connection.Close()
        Catch ex As Exception
            'something was wrong...
            Debug.WriteLine(ex.Message)
            frmLog.Log_Item_Add(ex.Message)
            MsgBox("MySQL Server read error! See the Log for more Details!", MsgBoxStyle.Critical)
        End Try
    End Function

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

#End Region

End Module