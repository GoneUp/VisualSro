Option Strict Off
Option Explicit On
Module modDataBase
	
	
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
		strSource = "Data Source=" & GetAppPath & "\DataBase\SremuDatabase.mdb;"
		
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
End Module