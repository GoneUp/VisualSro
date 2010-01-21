Imports Microsoft.VisualBasic
	Imports System
	Imports System.Globalization
	Imports System.Text
Namespace GameServer

    Public Class convert
        Public Shared Function BytesToHex(ByVal bytes() As Byte) As String
            Dim builder As New StringBuilder(bytes.Length)
            For i As Integer = 0 To bytes.Length - 1
                builder.Append(bytes(i).ToString("X2"))
            Next i
            Return builder.ToString()
        End Function

        Public Shared Function Hex2Float(ByVal str As String) As Single
            Dim s As String = str
            Return BitConverter.ToSingle(BitConverter.GetBytes(UInteger.Parse(s, NumberStyles.AllowHexSpecifier)), 0)
        End Function


        Public Shared Function IntegerFromWord(ByVal data As String) As Integer
            Dim num As Integer = 0
            If data.Length = 2 Then
                Return System.Convert.ToInt32(data, &H10)
            End If
            If data.Length = 4 Then
                Return System.Convert.ToInt32(Inverse(data), &H10)
            End If
            If data.Length = 8 Then
                Return System.Convert.ToInt32(Inverse(data), &H10)
            End If
            If data.Length = &H10 Then
                num = System.Convert.ToInt32(Inverse(data), &H10)
            End If
            Return num
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

        Public Shared Function Left(ByVal param As String, ByVal length As Integer) As String
            Try
                Return param.Substring(0, length)
            Catch exception As Exception
                Console.WriteLine("****** Left Error ******")
                Console.WriteLine(exception.Message)
                Console.WriteLine("****** Source ******")
                Console.WriteLine(String.Concat(New Object() {"Data: ", param, " Length: ", length}))
                Console.WriteLine("****** Stack Trace ******")
                Console.WriteLine(exception.StackTrace)
                Console.ReadLine()
            End Try
            Return Nothing
        End Function

        Public Shared Function Mid(ByVal param As String, ByVal startIndex As Integer) As String
            Try
                Return param.Substring(startIndex)
            Catch exception As Exception
                Console.WriteLine("****** Mid Error ******")
                Console.WriteLine(exception.Message)
                Console.WriteLine("****** Source ******")
                Console.WriteLine(String.Concat(New Object() {"Data: ", param, " Start Index: ", startIndex}))
                Console.WriteLine("****** Stack Trace ******")
                Console.WriteLine(exception.StackTrace)
                Console.ReadLine()
            End Try
            Return Nothing
        End Function

        Public Shared Function Mid(ByVal param As String, ByVal startIndex As Integer, ByVal length As Integer) As String
            Try
                Return param.Substring(startIndex, length)
            Catch exception As Exception
                Console.WriteLine("****** Mid Error ******")
                Console.WriteLine(exception.Message)
                Console.WriteLine("****** Source ******")
                Console.WriteLine(String.Concat(New Object() {"Data: ", param, " Start Index: ", startIndex, " Length: ", length}))
                Console.WriteLine("****** Stack Trace ******")
                Console.WriteLine(exception.StackTrace)
                Console.ReadLine()
            End Try
            Return Nothing
        End Function

        Public Shared Function Right(ByVal param As String, ByVal length As Integer) As String
            Try
                Return param.Substring(param.Length - length, length)
            Catch exception As Exception
                Console.WriteLine("****** Right Error ******")
                Console.WriteLine(exception.Message)
                Console.WriteLine("****** Source ******")
                Console.WriteLine(String.Concat(New Object() {"Data: ", param, " Length: ", length}))
                Console.WriteLine("****** Stack Trace ******")
                Console.WriteLine(exception.StackTrace)
                Console.ReadLine()
            End Try
            Return Nothing
        End Function

        Public Shared Function ToByteArray(ByVal HexString As String) As Byte()
            If (HexString.Length Mod 2) = 0 Then
                Dim length As Integer = HexString.Length
                Dim buffer(length \ 2 - 1) As Byte
                For i As Integer = 0 To length - 1 Step 2
                    buffer(i \ 2) = System.Convert.ToByte(HexString.Substring(i, 2), &H10)
                Next i
                Return buffer
            End If
            Console.WriteLine("Packet Error: " & HexString)
            Return Nothing
        End Function

        Friend Shared Function WordFromInteger(ByRef data As Object) As String
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace

