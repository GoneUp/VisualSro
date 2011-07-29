Namespace Settings
    Public Class cINI

        Private Declare Ansi Function GetPrivateProfileString Lib "kernel32.dll" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
        Private Declare Ansi Function WritePrivateProfileString Lib "kernel32.dll" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32
        Private Declare Function WritePrivateProfileSection& Lib "kernel32" Alias "WritePrivateProfileSectionA" (ByVal lpAppName$, ByVal lpString$, ByVal lpFileName$)
        '############################################################
        Private pFile As String
        Public Property File() As String
            Get
                Return pFile
            End Get
            Set(ByVal value As String)
                pFile = value
            End Set
        End Property

        Public Sub New(ByVal FileName As String)
            pFile = FileName
        End Sub
        '############################################################
        Public Function Read(ByVal strSection As String, ByVal strKey As String, ByVal strDefault As String) As String
            'Funktion zum Lesen
            'strSection = Sektion in der INI-Datei
            'strKey = Name des Schlüssels
            'strDefault = Standardwert, wird zurückgegeben, wenn der Wert in der INI-Datei nicht gefunden wurde
            'strFile = Vollständiger Pfad zur INI-Datei
            Dim strTemp As String = Space(1024), lLength As Integer
            lLength = GetPrivateProfileString(strSection, strKey, strDefault, strTemp, strTemp.Length, pFile)
            Return (strTemp.Substring(0, lLength))
        End Function
        Public Function Write(ByVal strSection As String, ByVal strKey As String, ByVal strValue As String) As Boolean
            'Funktion zum Schreiben
            'strSection = Sektion in der INI-Datei
            'strKey = Name des Schlüssels
            'strValue = Wert, der geschrieben werden soll
            'strFile = Vollständiger Pfad zur INI-Datei
            Return (Not (WritePrivateProfileString(strSection, strKey, strValue, pFile) = 0))
        End Function
    End Class
End Namespace
