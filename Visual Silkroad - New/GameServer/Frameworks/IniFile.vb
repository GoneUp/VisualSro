Imports Microsoft.VisualBasic
	Imports System
	Imports System.Runtime.InteropServices
	Imports System.Text
Namespace GameServer

    Public Class IniFile
        Public path As String

        Public Sub New(ByVal INIPath As String)
            Me.path = INIPath
        End Sub

        <DllImport("kernel32")> _
        Private Shared Function GetPrivateProfileString(ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer
        End Function
        Public Function IniReadValue(ByVal Section As String, ByVal Key As String) As String
            Dim retVal As New StringBuilder(&HFF)
            GetPrivateProfileString(Section, Key, "", retVal, &HFF, Me.path)
            Return retVal.ToString()
        End Function

        Public Sub IniWriteValue(ByVal Section As String, ByVal Key As String, ByVal Value As String)
            WritePrivateProfileString(Section, Key, Value, Me.path)
        End Sub

        <DllImport("kernel32")> _
        Private Shared Function WritePrivateProfileString(ByVal section As String, ByVal key As String, ByVal val As String, ByVal filePath As String) As Long
        End Function
    End Class
End Namespace

