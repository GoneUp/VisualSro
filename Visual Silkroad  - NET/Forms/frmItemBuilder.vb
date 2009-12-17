Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.PowerPacks
Friend Class frmItemBuilder
	Inherits System.Windows.Forms.Form
	Dim sPath As String
	
	
	'SRVB - SREmu VB Open-Source Project
	'Copyright (C) 2008 DarkInc Community
	'
	'This program is free software: you can redistribute it and/or modify
	'it under the terms of the GNU General Public License as published by
	'the Free Software Foundation, either version 3 of the License, or
	'(at your option) any later version.
	'
	'This program is distributed in the hope that it will be useful,
	'but WITHOUT ANY WARRANTY; without even the implied warranty of
	'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	'GNU General Public License for more details.
	'
	'You should have received a copy of the GNU General Public License
	'along with this program.  If not, see <http://www.gnu.org/licenses/>.
	
	'UPGRADE_WARNING: Das Ereignis cmbSlot.SelectedIndexChanged kann ausgelöst werden, wenn das Formular initialisiert wird. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub cmbSlot_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbSlot.SelectedIndexChanged
		
		'Clear all first.
		ClearBoxes()
		
		'Then read values and place them in textboxes.
		ReadValues()
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
		
		iniWrite(vbNullString, vbNullString, "item" & cmbSlot.Text, sPath)
		ClearBoxes()
		
	End Sub
	
	Private Sub cmdModify_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdModify.Click
		
		'Write values.
		iniWrite((cmbType.Text), "type", "item" & cmbSlot.Text, sPath)
		iniWrite((txtAmount.Text), "amount", "item" & cmbSlot.Text, sPath)
		iniWrite((txtID.Text), "ID", "item" & cmbSlot.Text, sPath)
		iniWrite((txtPlus.Text), "plusvalue", "item" & cmbSlot.Text, sPath)
		iniWrite((txtDurability.Text), "durability", "item" & cmbSlot.Text, sPath)
		iniWrite((txtPhysical.Text), "phyreinforce", "item" & cmbSlot.Text, sPath)
		iniWrite((txtMagical.Text), "magreinforce", "item" & cmbSlot.Text, sPath)
		
	End Sub
	
	Private Sub frmItemBuilder_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CheckAccount()
		
	End Sub
	
	'UPGRADE_WARNING: Das Ereignis txtUsername.TextChanged kann ausgelöst werden, wenn das Formular initialisiert wird. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub txtUsername_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtUsername.TextChanged
		
		CheckAccount()
		
	End Sub
	
	Private Function CheckAccount() As Object
		
		sPath = (Replace(My.Application.Info.DirectoryPath, "\", "/") & "/accounts/" & txtUsername.Text & ".ini")
		'Only with a valid account can you press button.
		If iniGetStr("password", "account", sPath) = "(error)" Then
			cmdModify.Enabled = False
			cmdModify.Text = "Please enter a character name."
			cmdDelete.Enabled = False
			cmdDelete.Text = "Please enter a character name."
			ClearBoxes()
		Else
			cmdModify.Enabled = True
			cmdModify.Text = "Add / Modify"
			cmdDelete.Enabled = True
			cmdDelete.Text = "Delete Item"
			ReadValues()
		End If
		
	End Function
	
	Private Function ClearBoxes() As Object
		
		cmbType.Text = ""
		txtID.Text = ""
		txtPlus.Text = ""
		txtDurability.Text = ""
		txtPhysical.Text = ""
		txtMagical.Text = ""
		txtAmount.Text = ""
		
	End Function
	
	Private Function ReadValues() As Object
		
		Dim section As String
		Dim stype As String
		Dim amount As String
		Dim ID As String
		Dim PlusValue As String
		Dim Durability As String
		Dim PhyReinforce As String
		Dim MagReinforce As String
		
		section = "item" & cmbSlot.Text
		stype = iniGetStr("type", section, sPath)
		amount = iniGetStr("amount", section, sPath)
		ID = iniGetStr("ID", section, sPath)
		PlusValue = iniGetStr("plusvalue", section, sPath)
		Durability = iniGetStr("durability", section, sPath)
		PhyReinforce = iniGetStr("phyreinforce", section, sPath)
		MagReinforce = iniGetStr("magreinforce", section, sPath)
		
		If stype <> "(error)" Then
			cmbType.Text = stype
		End If
		If amount <> "(error)" Then
			txtAmount.Text = amount
		End If
		If ID <> "(error)" Then
			txtID.Text = ID
		End If
		If PlusValue <> "(error)" Then
			txtPlus.Text = PlusValue
		End If
		If Durability <> "(error)" Then
			txtDurability.Text = Durability
		End If
		If PhyReinforce <> "(error)" Then
			txtPhysical.Text = PhyReinforce
		End If
		If MagReinforce <> "(error)" Then
			txtMagical.Text = MagReinforce
		End If
		
	End Function
End Class