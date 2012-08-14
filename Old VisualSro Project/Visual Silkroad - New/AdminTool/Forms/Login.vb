Public Class Login
    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        SessionInfo.ClientName = "AdminTool"
        SessionInfo.Login = boxUser.Text
        SessionInfo.Password = boxPasswort.Text
        Me.Close()
        Main.Show()
    End Sub
End Class