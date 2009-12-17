Option Strict Off
Option Explicit On
Friend Class frmKickPlayer
	Inherits System.Windows.Forms.Form
	Dim i As Short
	
	Private Sub CancelButton_Renamed_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelButton_Renamed.Click
		Me.Close()
	End Sub
	
	'UPGRADE_WARNING: Das Ereignis GMCheckBox.CheckStateChanged kann ausgelöst werden, wenn das Formular initialisiert wird. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub GMCheckBox_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles GMCheckBox.CheckStateChanged
		If GMCheckBox.CheckState = 1 Then
			KickField.Text = "[GM]" & KickField.Text
		Else
			KickField.Text = Replace(KickField.Text, "[GM]", "")
		End If
	End Sub
	
	Private Sub OKButton_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OKButton.Click
		If Not KickField.Text = "Enter player to be kicked..." And Not KickField.Text = "" And ServerStarted = True Then
			For i = 1 To UBound(PlayerData)
				'UPGRADE_WARNING: Die Standardeigenschaft des Objekts PlayerData(i).Charname konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				If PlayerData(i).Charname = KickField.Text Then
					QuitGame(i, "01")
                    Debug.Print("Player '" & KickField.Text & "' kicked!")
                    frmLog.Log_Item_Add("Player '" & KickField.Text & "' kicked!")
					Me.Close()
				End If
			Next i
		Else
            MsgBox("Error kicking player!")
            frmLog.Log_Item_Add("Error to kick the Player:" & KickField.Text)
		End If
	End Sub
End Class