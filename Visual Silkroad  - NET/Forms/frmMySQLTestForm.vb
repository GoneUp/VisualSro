Imports MySql.Data.MySqlClient

Public Class frmMySQLTestForm

    Public connStr As String = "Database=world;" & _
                      "Data Source=127.0.0.1;" & _
                      "User Id=sremu;Password=sremu;" & _
                      "Connection Timeout=20"

    Private Sub Form1_Load(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Public Sub retriveData()
        Try
            Dim query As String = "SELECT * FROM Country"
            Dim connection As New MySqlConnection(connStr)
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
            Dim connection As New MySqlConnection(connStr)
            Dim cmd As New MySqlCommand(query, connection)

            connection.Open()

            rowsEffected = cmd.ExecuteNonQuery()

            connection.Close()

            Return rowsEffected
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Function



    Public Sub TestConnection()
        Try

            Dim connection As New MySqlConnection(connStr)
            connection.Open()
            connection.Close()
            MsgBox("Connection is okay.")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        TestConnection()

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        retriveData()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Debug.WriteLine(updateRecord("INSERT INTO Country (Code, Name) VALUES ('AAA','Test Name')"))

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Debug.WriteLine(updateRecord("UPDATE Country SET Name='Test2' WHERE Code ='AAA'"))
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Debug.WriteLine(updateRecord("DELETE FROM Country WHERE Code ='AAA'"))
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

        Dim ExterneAnwendung As New System.Diagnostics.Process()
        ExterneAnwendung.StartInfo.FileName = "http://www.linglom.com/2009/02/12/accessing-mysql-on-vbnet-using-mysql-connectornet-part-i-introduction/"
        ExterneAnwendung.Start()

    End Sub
End Class
