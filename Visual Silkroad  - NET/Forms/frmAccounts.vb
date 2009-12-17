Option Strict Off
Option Explicit On
Friend Class frmAccounts
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
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
		
		Kill(sPath)
		SetDefaultBoxes()
		txtAccount.Text = ""
		cmdDelete.Enabled = False
		
	End Sub
	
	Private Sub cmdGo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdGo.Click
		
		If txtAccount.Text <> "" And txtCharname.Text <> "" And txtPassword.Text <> "" Then
			iniWrite((txtPassword.Text), "password", "account", sPath)
			iniWrite((txtCharname.Text), "name", "character", sPath)
			iniWrite(Inverse(DecToHexLong(CInt(Rnd() * 2565535) + 310001)), "AccountID", "character", sPath)
			iniWrite((cmbChartype.Text), "chartype", "character", sPath)
			iniWrite((txtVolume.Text), "volume", "character", sPath)
			iniWrite((txtLevel.Text), "level", "character", sPath)
			iniWrite((txtLevel.Text), "highlevel", "character", sPath)
			iniWrite((txtStrength.Text), "strength", "character", sPath)
			iniWrite((txtIntelligence.Text), "intelligence", "character", sPath)
			iniWrite((txtSP.Text), "skillpoints", "character", sPath)
			iniWrite((txtHP.Text), "hp", "character", sPath)
			iniWrite((txtMP.Text), "mp", "character", sPath)
			iniWrite((txtMinPhyAtk.Text), "minphyatk", "character", sPath)
			iniWrite((txtMaxPhyAtk.Text), "maxphyatk", "character", sPath)
			iniWrite((txtMinMagAtk.Text), "minmagatk", "character", sPath)
			iniWrite((txtMaxMagAtk.Text), "maxmagatk", "character", sPath)
			iniWrite((txtPhyDef.Text), "phydef", "character", sPath)
			iniWrite((txtMagDef.Text), "magdef", "character", sPath)
			iniWrite((txtHit.Text), "Hit", "character", sPath)
			iniWrite((txtParry.Text), "Parry", "character", sPath)
			iniWrite(CStr(chkGM.CheckState), "GM", "character", sPath)
			'If there's no location already, write some default values:
			If iniGetStr("XSection", "character", sPath) = "(error)" Then
				iniWrite("169", "XSection", "character", sPath)
				iniWrite("95", "YSection", "character", sPath)
				iniWrite("6589.1", "XPos", "character", sPath)
				iniWrite("698.3", "YPos", "character", sPath)
			End If
			If CShort(cmbChartype.Text) < 2000 Then 'Chinese
				iniWrite("0", "Race", "character", sPath)
			Else 'European
				iniWrite("1", "Race", "character", sPath)
			End If
			'Default masteries, we should support these in the GUI
			iniWrite("0", "MasteryLvl(1)", "character", sPath)
			iniWrite("0", "MasteryLvl(2)", "character", sPath)
			iniWrite("0", "MasteryLvl(3)", "character", sPath)
			iniWrite("0", "MasteryLvl(4)", "character", sPath)
			iniWrite("0", "MasteryLvl(5)", "character", sPath)
			iniWrite("0", "MasteryLvl(6)", "character", sPath)
			iniWrite("0", "MasteryLvl(7)", "character", sPath)
			'Skills
			iniWrite("0", "NumSkills", "character", sPath)
			'New stuff
			iniWrite((txtBerserkBar.Text), "BerserkBar", "character", sPath)
			iniWrite((txtWalkSpeed.Text), "WalkSpeed", "character", sPath)
			iniWrite((txtRunSpeed.Text), "RunSpeed", "character", sPath)
			iniWrite((txtBerserkSpeed.Text), "BerserkSpeed", "character", sPath)
			iniWrite((txtAttributePoints.Text), "AttributePoints", "character", sPath)
			
		End If
		
	End Sub
	
	Private Sub frmAccounts_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CheckAccount()
		
	End Sub
	
	'UPGRADE_WARNING: Das Ereignis txtAccount.TextChanged kann ausgelöst werden, wenn das Formular initialisiert wird. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
	Private Sub txtAccount_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtAccount.TextChanged
		
		CheckAccount()
		
	End Sub
	
	Private Function CheckAccount() As Object


        sPath = (Replace(My.Application.Info.DirectoryPath, "\", "/") & "/accounts/" & txtAccount.Text & ".ini")
		If iniGetStr("password", "account", sPath) = "(error)" Then
			cmdGo.Text = "Create character"
			SetDefaultBoxes()
			cmdDelete.Enabled = False
		Else
			cmdGo.Text = "Modify character"
			LoadExistingData()
			cmdDelete.Enabled = True
		End If
		
	End Function
	
	Private Function SetDefaultBoxes() As Object
		
		txtCharname.Text = ""
		txtPassword.Text = ""
		cmbChartype.Text = "1911"
		txtVolume.Text = "68"
		txtLevel.Text = "1"
		txtStrength.Text = "20"
		txtIntelligence.Text = "20"
		txtSP.Text = "500"
		txtHP.Text = "200"
		txtMP.Text = "200"
		txtMinPhyAtk.Text = "10"
		txtMaxPhyAtk.Text = "20"
		txtMinMagAtk.Text = "10"
		txtMaxMagAtk.Text = "20"
		txtPhyDef.Text = "9"
		txtMagDef.Text = "15"
		txtHit.Text = "35"
		txtParry.Text = "24"
		chkGM.CheckState = System.Windows.Forms.CheckState.Unchecked
		txtBerserkBar.Text = "0"
		txtWalkSpeed.Text = "50"
		txtRunSpeed.Text = "50"
		txtBerserkSpeed.Text = "50"
		txtAttributePoints.Text = "10"
		
	End Function
	
	Private Function LoadExistingData() As Object
		
		txtCharname.Text = iniGetStr("name", "character", sPath)
		txtPassword.Text = iniGetStr("password", "account", sPath)
		cmbChartype.Text = iniGetStr("chartype", "character", sPath)
		txtVolume.Text = iniGetStr("volume", "character", sPath)
		txtLevel.Text = iniGetStr("level", "character", sPath)
		txtStrength.Text = iniGetStr("strength", "character", sPath)
		txtIntelligence.Text = iniGetStr("intelligence", "character", sPath)
		txtSP.Text = iniGetStr("skillpoints", "character", sPath)
		txtHP.Text = iniGetStr("hp", "character", sPath)
		txtMP.Text = iniGetStr("mp", "character", sPath)
		txtMinPhyAtk.Text = iniGetStr("minphyatk", "character", sPath)
		txtMaxPhyAtk.Text = iniGetStr("maxphyatk", "character", sPath)
		txtMinMagAtk.Text = iniGetStr("minmagatk", "character", sPath)
		txtMaxMagAtk.Text = iniGetStr("maxmagatk", "character", sPath)
		txtPhyDef.Text = iniGetStr("phydef", "character", sPath)
		txtMagDef.Text = iniGetStr("magdef", "character", sPath)
		txtHit.Text = iniGetStr("hit", "character", sPath)
		txtParry.Text = iniGetStr("parry", "character", sPath)
		
		chkGM.CheckState = CShort(iniGetStr("gm", "character", sPath))
		
		txtBerserkBar.Text = iniGetStr("BerserkBar", "character", sPath)
		txtWalkSpeed.Text = iniGetStr("WalkSpeed", "character", sPath)
		txtRunSpeed.Text = iniGetStr("RunSpeed", "character", sPath)
		txtBerserkSpeed.Text = iniGetStr("BerserkSpeed", "character", sPath)
		txtAttributePoints.Text = iniGetStr("AttributePoints", "character", sPath)
		
	End Function
End Class