Option Strict Off
Option Explicit On
Imports MySql.Data.MySqlClient

Module modLoginserver
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
	
	Private fData As String
	Private pLen As Short
	Private i As Short
	Private x As Short
	
	Public Function ParseLoginData(ByRef data As String, ByRef index As Short) As Object
		
		Dim dData As String
		dData = cv_HexFromString(data)
		
		Dim sSize As Short
		Dim sOpcode As String
		Dim sData As String
		Dim fData As String
		Dim pLen As String
		'UPGRADE_WARNING: Arrays in Struktur ServerDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim ServerDataBase As DAO.Recordset
		Dim BookMark As Object
		Dim ServerNames As String
		Dim ServerIds As String
		Dim ServerUsers As String
		Dim ServerUsersMax As String
		Dim ServerNewOld As String
		Dim ServerStatus As String
        'OpenSremuDataBase()
        'Mysql stuff


        Dim query As String = "SELECT * FROM servers"
        Dim connection As New MySqlConnection(MySQLConfig)
        Dim cmd As New MySqlCommand(query, connection)

        'connection.Open()
        'Dim reader2 As MySqlDataReader
        'reader2 = cmd.ExecuteReader()

        'ServerIds = reader2.GetValue(1)
        'ServerNames = reader2.GetValue(2)
        'ServerUsers = reader2.GetValue(3)
        'ServerUsersMax = reader2.GetValue(4)
        'ServerStatus = reader2.GetValue(5)

        ServerIds = 100
        ServerNames = "Vis"
        ServerUsers = 100
        ServerUsersMax = 500
        ServerStatus = "01"
        ServerNewOld = 0

        'connection.Close()

        '//Commited out because we will add mysql database 

        'Try
        'With ServerDataBase
        'ServerIds = .Fields("Serverid").Value
        'ServerNames = .Fields("ServerName").Value
        'ServerUsers = .Fields("ServerUsers").Value
        'ServerUsersMax = .Fields("ServerMaxUsers").Value
        'ServerNewOld = "1" '!ServerNewOld
        'ServerStatus = .Fields("ServerStatus").Value
        'End With
        'Catch ex As Exception
        'Debug.Print(ex.Message)
        'End Try

        sSize = CShort("&H" & Mid(dData, 3, 2) & Mid(dData, 1, 2))
        sOpcode = Mid(dData, 7, 2) & Mid(dData, 5, 2)
        If sSize > 0 Then sData = Mid(dData, 13, sSize * 2)

        Dim nData As String
        Dim iNameSize As Short
        Dim iPassSize As Short
        Dim iServerID As Short
        Dim Username As String
        Dim Password As String
        Dim temp As String
        Dim AccountId As String
        Dim AccountPass As String
        Dim rPassword As String
        Dim ip As String 'Acc PassWord 'Userid
        'UPGRADE_WARNING: Arrays in Struktur AccountDataBase müssen möglicherweise initialisiert werden, bevor sie verwendet werden können. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim AccountDataBase As DAO.Recordset
        Select Case sOpcode

            Case "0000"

                nData = "For some unknown reason, SREmu was not able to properly handle one of the client's packets."
                nData = nData & vbCrLf & vbCrLf & "Make sure you:" & vbCrLf
                nData = nData & "- Have not modified media.pk2 in any way (mediapatcher)." & vbCrLf
                nData = nData & "- Have updated Silkroad to the latest version." & vbCrLf
                nData = nData & "- Are using sro_client.exe as provided on our forums." & vbCrLf
                nData = nData & "- Are using a proper version of VB6 and mswinsck.ocx" & vbCrLf & vbCrLf
                nData = nData & "If this error despite the above, please visit our forums:" & vbCrLf & "http://www.sremu.org/"
                MsgBox(nData, MsgBoxStyle.Critical, "Error")
                End

            Case "2001" 'Client wants to know who we are

                fData = "1000"
                fData = fData & "0120"
                fData = fData & "0000"
                fData = fData & "0D00"
                fData = fData & cv_HexFromString("GatewayServer")
                fData = fData & "00"

                modGlobal.LoginSocket(index).SendData(cv_StringFromHex(fData))

            Case "6100" 'Client whoami. Send patch-info.

                fData = "05000D6000000101000520"
                fData = fData & "0B000D6000000001000154070500000002"
                fData = fData & "05000D6000000101000560"
                fData = fData & "06000D600000000300020002"
                fData = fData & "05000D60000001010000A1"
                fData = fData & "02000D6000000001"
                fData = fData & "05000D60000001010004A1"
                modGlobal.LoginSocket(index).SendData((cv_StringFromHex(fData)))

            Case "9000"
                'Client accepts communication type.
            Case "6104"
                launcher((index))

            Case "2002"
                'Ping packet, ignore.

            Case "6101" 'Serverlist request...

                fData = "01A1"
                fData = fData & "0000"
                fData = fData & "0115"
                fData = fData & "1200" & cv_HexFromString("SRO_Global_TestBed") & "00"
                fData = fData & "0" & ServerNewOld                     '1 or 0
                fData = fData & ServerIds                        'Server ID
                fData = fData & WordFromInteger(Len((ServerNames)))
                fData = fData & cv_HexFromString((ServerNames))
                fData = fData & WordFromInteger((ServerUsers))           'Users
                fData = fData & WordFromInteger((ServerUsersMax))       'Max users (500)
                fData = fData & "0" & ServerStatus                      'Servermode (00 = check, 01 = online)
                fData = fData & "00"                            'End serverlist

                pLen = CStr((Len(fData) - 8) / 2)
                fData = WordFromInteger(pLen) & fData
                modGlobal.LoginSocket(index).SendData(cv_StringFromHex(fData))

            Case "6102" 'Login data...


                iNameSize = CShort("&H" & Mid(sData, 5, 2) & Mid(sData, 3, 2))
                Username = cv_StringFromHex(Mid(sData, 7, iNameSize * 2))
                iPassSize = CShort("&H" & Mid(sData, 9 + (iNameSize * 2), 2) & Mid(sData, 7 + (iNameSize * 2), 2))
                Password = cv_StringFromHex(Mid(sData, 11 + iNameSize * 2, iPassSize * 2))
                temp = Right(sData, 4)
                iServerID = CShort("&H" & Mid(temp, 3, 2) & Mid(temp, 1, 2))

                fData = "02A1"
                fData = fData & "0000"

                'Set AccountDataBase = DataBases.OpenRecordset("Accounts", dbOpenTable)  'DB Table
                'BookMark = AccountDataBase.BookMark

                'AccountDataBase.index = "AccountID" 'will be added later
                'AccountDataBase.Seek ">=", "92915002" 'Usernameid

                'With AccountDataBase
                '    AccountId = !Username
                '    AccountPass = !Password
                'End With

                rPassword = iniGetStr("password", "account", Replace(My.Application.Info.DirectoryPath, "\", "/") & "/accounts/" & Username & ".ini")
                ip = iniGetStr("Ip", "ServerIp", Replace(My.Application.Info.DirectoryPath, "\", "/") & "/config/" & "ServerSet.ini")

                'If AccountDataBase.NoMatch Then 'Stops the Login Incase the UserName is not found
                '    AccountDataBase.BookMark = BookMark
                '    Debug.Print "Username does not exist"
                '    fData = fData & "02"  'failed
                '    fData = fData & "01"  'wrong password
                '    fData = fData & "0100000005000000"
                'End If

                If rPassword = "(error)" Or rPassword <> Password Then
                    Debug.Print("Username does not exist or password is incorrect.")
                    fData = fData & "02" 'failed
                    fData = fData & "01" 'wrong password
                    fData = fData & "0100000005000000"
                Else
                    fData = fData & "01" 'Success
                    fData = fData & "7B000000" 'Login ID
                    fData = fData & WordFromInteger(Len(ip))
                    fData = fData & cv_HexFromString(ip)
                    fData = fData & WordFromInteger(15778) 'Port

                End If

                pLen = CStr((Len(fData) - 8) / 2)
                fData = WordFromInteger(pLen) & fData
                modGlobal.LoginSocket(index).SendData(cv_StringFromHex(fData))

            Case Else
                Debug.Print("Unknown opcode: " & dData & " - " & sOpcode)
        End Select

    End Function
	Public Sub launcher(ByVal index As Short)
		Dim Total As Short
		Dim fData As String
		Dim hextotal As String
		Dim msg As String
		Dim news As String
		Dim l1 As String
		Dim l2 As String
		'Dim ServerDataBase As Recordset
		'Dim BookMark As Variant
		Dim month1 As String
		Dim day1 As String
        news = "Welcome to VisualSro!"
        msg = "<b>Welcome To Visual Silkroad!<br><font color=red>-The Visro Team (GoneUp, Manneke and Seponer)</font></b>"
		
		
		
		'<b><font.color=yellow>News For Check Forum [dsremu.smf4u.com]</font></b>"
		'OpenSremuDataBase
		'Set ServerDataBase = DataBases.OpenRecordset("Launchers", dbOpenTable)  'DB Table
		'BookMark = ServerDataBase.BookMark
		
		
		'With ServerDataBase
		'msg = !notice
		'news = !news
		'l1 = !newlength
		'l2 = !noticelength
		'month1 = !Month
		'day1 = !Day
		'End With
		
		fData = "000D60"
		fData = fData & "0000"
		fData = fData & "00"
		fData = fData & "01" 'Number of news
		fData = fData & NumToHex(Len(news)) ' Length title
		fData = fData & "00"
		fData = fData & cv_HexFromString(news)
		fData = fData & NumToHex(Len(msg)) 'Length msg
		fData = fData & "00"
		fData = fData & cv_HexFromString(msg)
		fData = fData & "d807" '?
		fData = fData & NumToHex("2") 'date Month
		fData = fData & "00"
		fData = fData & NumToHex("3") 'date Day
		fData = fData & "00"
		fData = fData & "0A0019001F00C0C9F228" '???
		Total = (Len(fData) - 10) / 2
		hextotal = NumToHex(Total) 'Total Data
        modGlobal.LoginSocket(index).SendData((cv_StringFromHex(hextotal) & cv_StringFromHex(fData)))
		
	End Sub
End Module