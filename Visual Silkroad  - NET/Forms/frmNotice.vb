Option Strict Off
Option Explicit On
Friend Class frmNotice
	Inherits System.Windows.Forms.Form
	Private Sub sndnoticebutton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles sndnoticebutton.Click
		If Not sndnoticefield.Text = "" And ServerStarted = True Then
			SendNotice(sndnoticefield.Text)
            Debug.Print("Notice '" & sndnoticefield.Text & "' sent!")
            frmLog.Log_Item_Add("Notice '" & sndnoticefield.Text & "' sent!")
		Else
            MsgBox("Error sending notice!")
            frmLog.Log_Item_Add("Error sending notice!")
			Me.Close()
		End If
	End Sub
End Class