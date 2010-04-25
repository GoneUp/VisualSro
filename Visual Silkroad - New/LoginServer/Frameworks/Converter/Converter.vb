Imports Microsoft.VisualBasic
	Imports System
	Imports System.Text
Namespace LoginServer

    Public Class Converter
        Public Shared Function ByteFromInteger(ByVal data As Integer) As String
            Dim str As String = Nothing
            str = System.Convert.ToString(data, &H10).ToUpper()
            If str.Length = 1 Then
                str = "0" & str
            End If
            Return str
        End Function

        Public Shared Function BytesToHex(ByVal bytes() As Byte) As String
            Dim builder As New StringBuilder(bytes.Length)
            For i As Integer = 0 To bytes.Length - 1
                builder.Append(bytes(i).ToString("X2"))
            Next i
            Return builder.ToString()
        End Function

        Public Shared Function DWordFromInteger(ByVal data As Integer) As String
            Dim str As String = Nothing
            Dim num As Integer = 0
            str = System.Convert.ToString(data, &H10).ToUpper()
            If ((str.Length = 1) OrElse (str.Length = 3)) OrElse ((str.Length = 5) OrElse (str.Length = 7)) Then
                str = "0" & str
            End If
            Do While str.Length < 8
                str = "0" & str
            Loop
            Dim str2 As String = Nothing
            For num = 8 To 1 Step -1
                str2 = str2 & str.Substring(num - 2, 2)
                num -= 1
            Next num
            Return str2
        End Function

        Public Shared Function Float2Hex(ByVal tmpHex As Single) As String
            Dim num As Single = tmpHex
            Dim num2 As Double = System.Convert.ToDouble(num)
            BitConverter.DoubleToInt64Bits(num2).ToString("X")
            Dim num4 As Single = CSng(num2)
            Return BitConverter.ToInt32(BitConverter.GetBytes(num4), 0).ToString("X")
        End Function

        Public Shared Function Inverse(ByVal DataString As String) As String
            Dim str As String = Nothing
            Dim num As Short = 0
            If ((DataString.Length Mod 2) = 0) AndAlso (DataString.Length > 0) Then
                num = 1
                Do While num <= DataString.Length
                    str = DataString.Substring(num - 1, 2) & str
                    num = CShort(Fix(num + 2))
                Loop
                If (str IsNot Nothing) Then
                    Return (str)
                Else
                    Return ("")
                End If
            End If
            Return DataString
        End Function

        Public Shared Function LongWordFromInteger(ByVal data As Long) As String
            Dim str As String = Nothing
            Dim num As Integer = 0
            str = System.Convert.ToString(data, &H10).ToUpper()
            If ((str.Length = 1) OrElse (str.Length = 3)) OrElse ((str.Length = 5) OrElse (str.Length = 7)) Then
                str = "0" & str
            End If
            Do While str.Length < &H10
                str = "0" & str
            Loop
            Dim str2 As String = Nothing
            For num = &H10 To 1 Step -1
                str2 = str2 & str.Substring(num - 2, 2)
                num -= 1
            Next num
            Return str2
        End Function


        Public Shared Function WordFromInteger(ByVal data As Integer) As String
            Dim str As String = Nothing
            str = System.Convert.ToString(data, &H10).ToUpper()
            If str.Length = 1 Then
                Return ("0" & str & "00")
            End If
            If str.Length = 2 Then
                Return (str & "00")
            End If
            If str.Length = 3 Then
                Return (str.Substring(1, 2) & "0" & str.Substring(0, 1))
            End If
            If str.Length = 4 Then
                str = str.Substring(2, 2) & str.Substring(0, 2)
            End If
            Return str
        End Function
    End Class
End Namespace

