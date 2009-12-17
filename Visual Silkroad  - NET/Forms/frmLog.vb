Public Class frmLog

    Public LogLineNumber As Integer

    Public Sub Log_Item_Add(ByVal message As String)

        'Check if a line exitis
        If logBox.Items.Count = 0 Then
            LogLineNumber = 0
        End If

        'Add the Text into the Box
        Me.logBox.Items.Add(message)

    End Sub

    Private Sub Delete_Log(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButDelete.Click

        logBox.Items.Clear()
        LogLineNumber = 0

    End Sub

    Private Sub ButClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButClose.Click

        Me.Hide()

    End Sub
End Class