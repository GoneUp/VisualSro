Imports System.Globalization
Imports System.Resources
Imports System.Reflection
Imports System.Threading
Imports System.Text
Imports System.Security
Imports System.Runtime.InteropServices
Imports System.Security.Permissions


Public Class Utils
    Public NotInheritable Class Utils
        ' Methods
        Private Sub New()
        End Sub

        Friend Shared Function AdjustArraySuffix(ByVal sRank As String) As String
            Dim str2 As String = Nothing
            Dim i As Integer = sRank.Length
            Do While (i > 0)
                Dim ch As Char = sRank.Chars((i - 1))
                Select Case ch
                    Case "("c
                        str2 = (str2 & ")")
                        Continue Do
                    Case ")"c
                        str2 = (str2 & "(")
                        Continue Do
                    Case ","c
                        Continue Do
                End Select
                i -= 1
            Loop
            Return str2
        End Function

        Public Shared Function CopyArray(ByVal arySrc As Array, ByVal aryDest As Array) As Array
            If (Not arySrc Is Nothing) Then
                Dim length As Integer = arySrc.Length
                If (length = 0) Then
                    Return aryDest
                End If
                If (aryDest.Rank <> arySrc.Rank) Then
                End If
                Dim num8 As Integer = (aryDest.Rank - 2)
                Dim i As Integer = 0
                Do While (i <= num8)
                    If (aryDest.GetUpperBound(i) <> arySrc.GetUpperBound(i)) Then
                    End If
                    i += 1
                Loop
                If (length > aryDest.Length) Then
                    length = aryDest.Length
                End If
                If (arySrc.Rank > 1) Then
                    Dim rank As Integer = arySrc.Rank
                    Dim num7 As Integer = arySrc.GetLength((rank - 1))
                    Dim num6 As Integer = aryDest.GetLength((rank - 1))
                    If (num6 <> 0) Then
                        Dim num5 As Integer = Math.Min(num7, num6)
                        Dim num9 As Integer = ((arySrc.Length / num7) - 1)
                        Dim j As Integer = 0
                        Do While (j <= num9)
                            Array.Copy(arySrc, (j * num7), aryDest, (j * num6), num5)
                            j += 1
                        Loop
                    End If
                    Return aryDest
                End If
                Array.Copy(arySrc, aryDest, length)
            End If
            Return aryDest
        End Function

        Friend Shared Function FieldToString(ByVal Field As FieldInfo) As String
            Dim str As String = ""
            Dim fieldType As Type = Field.FieldType
            If Field.IsPublic Then
                str = (str & "Public ")
            ElseIf Field.IsPrivate Then
                str = (str & "Private ")
            ElseIf Field.IsAssembly Then
                str = (str & "Friend ")
            ElseIf Field.IsFamily Then
                str = (str & "Protected ")
            ElseIf Field.IsFamilyOrAssembly Then
                str = (str & "Protected Friend ")
            End If
            Return ((str & Field.Name) & " As " & Utils.VBFriendlyNameOfType(fieldType, True))
        End Function

        Private Shared Function GetArraySuffixAndElementType(ByRef typ As Type) As String
            If Not typ.IsArray Then
                Return Nothing
            End If
            Dim builder As New StringBuilder
            Do
                builder.Append("(")
                builder.Append(","c, (typ.GetArrayRank - 1))
                builder.Append(")")
                typ = typ.GetElementType
            Loop While typ.IsArray
            Return builder.ToString
        End Function

        Friend Shared Function GetCultureInfo() As CultureInfo
            Return Thread.CurrentThread.CurrentCulture
        End Function

        Friend Shared Function GetDateTimeFormatInfo() As DateTimeFormatInfo
            Return Thread.CurrentThread.CurrentCulture.DateTimeFormat
        End Function

        Friend Shared Function GetFileIOEncoding() As Encoding
            Return Encoding.Default
        End Function

        Private Shared Function GetGenericArgsSuffix(ByVal typ As Type) As String
            If Not typ.IsGenericType Then
                Return Nothing
            End If
            Dim genericArguments As Type() = typ.GetGenericArguments
            Dim length As Integer = genericArguments.Length
            Dim num2 As Integer = length
            If (typ.IsNested AndAlso typ.DeclaringType.IsGenericType) Then
                num2 = (num2 - typ.DeclaringType.GetGenericArguments.Length)
            End If
            If (num2 = 0) Then
                Return Nothing
            End If
            Dim builder As New StringBuilder
            builder.Append("(Of ")
            Dim num4 As Integer = (length - 1)
            Dim i As Integer = (length - num2)
            Do While (i <= num4)
                builder.Append(Utils.VBFriendlyNameOfType(genericArguments(i), False))
                If (i <> (length - 1)) Then
                    builder.Append(","c)
                End If
                i += 1
            Loop
            builder.Append(")")
            Return builder.ToString
        End Function

        Friend Shared Function GetInvariantCultureInfo() As CultureInfo
            Return CultureInfo.InvariantCulture
        End Function

        Friend Shared Function GetLocaleCodePage() As Integer
            Return Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage
        End Function




        Public Shared Function GetResourceString(ByVal ResourceKey As String, ByVal ParamArray Args As String()) As String
            Dim str As String = Nothing
            Dim format As String = Nothing
            Try
                format = Utils.GetResourceString(ResourceKey)
                str = String.Format(Thread.CurrentThread.CurrentUICulture, format, Args)
            Catch exception As StackOverflowException
                Throw exception
            Catch exception2 As OutOfMemoryException
                Throw exception2
            Catch exception3 As ThreadAbortException
                Throw exception3
            Catch exception4 As Exception
            End Try
            If (str <> "") Then
                Return str
            End If
            Return format
        End Function

        Friend Shared Function GetResourceString(ByVal ResourceKey As String, ByVal NotUsed As Boolean) As String
            Dim str2 As String
            If (Utils.VBAResourceManager Is Nothing) Then
                Return "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found."
            End If
            Try
                str2 = Utils.VBAResourceManager.GetString(ResourceKey, Utils.GetCultureInfo)
                If (str2 Is Nothing) Then
                    str2 = Utils.VBAResourceManager.GetString(ResourceKey)
                End If
            Catch exception As StackOverflowException
                Throw exception
            Catch exception2 As OutOfMemoryException
                Throw exception2
            Catch exception3 As ThreadAbortException
                Throw exception3
            Catch exception6 As Exception
                str2 = Nothing
            End Try
            Return str2
        End Function




        Friend Shared Function MapHRESULT(ByVal lNumber As Integer) As Integer
            If (lNumber <= 0) Then
                If ((lNumber And &H1FFF0000) = &HA0000) Then
                    Return (lNumber And &HFFFF)
                End If
                Select Case lNumber
                    Case -2147467263
                        Return &H8000
                    Case -2147467262
                        Return 430
                    Case -2147467260
                        Return &H11F
                    Case -2147352575
                        Return &H1B6
                    Case -2147352573
                        Return &H1B6
                    Case -2147352572
                        Return &H1C0
                    Case -2147352571
                        Return 13
                    Case -2147352570
                        Return &H1B6
                    Case -2147352569
                        Return &H1BE
                    Case -2147352568
                        Return &H1CA
                    Case -2147352566
                        Return 6
                    Case -2147352565
                        Return 9
                    Case -2147352564
                        Return &H1BF
                    Case -2147352563
                        Return 10
                    Case -2147352562
                        Return 450
                    Case -2147352561
                        Return &H1C1
                    Case -2147352559
                        Return &H1C3
                    Case -2147352558
                        Return 11
                    Case -2147319786
                        Return &H8016
                    Case -2147319785
                        Return &H1CD
                    Case -2147319784
                        Return &H8018
                    Case -2147319783
                        Return &H8019
                    Case -2147319780
                        Return &H801C
                    Case -2147319779
                        Return &H801D
                    Case -2147319769
                        Return &H8027
                    Case -2147319768
                        Return &H8028
                    Case -2147319767
                        Return &H8029
                    Case -2147319766
                        Return &H802A
                    Case -2147319765
                        Return &H802B
                    Case -2147319764
                        Return &H802C
                    Case -2147319763
                        Return &H802D
                    Case -2147319762
                        Return &H802E
                    Case -2147319761
                        Return &H1C5
                    Case -2147317571
                        Return &H88BD
                    Case -2147317563
                        Return &H88C5
                    Case -2147316576
                        Return 13
                    Case -2147316575
                        Return 9
                    Case -2147316574
                        Return &H39
                    Case -2147316573
                        Return &H142
                    Case -2147312566
                        Return &H30
                    Case -2147312509
                        Return &H9C83
                    Case -2147312508
                        Return &H9C84
                    Case -2147287039
                        Return &H8006
                    Case -2147287038
                        Return &H35
                    Case -2147287037
                        Return &H4C
                    Case -2147287036
                        Return &H43
                    Case -2147287035
                        Return 70
                    Case -2147287034
                        Return &H8004
                    Case -2147287032
                        Return 7
                    Case -2147287022
                        Return &H43
                    Case -2147287021
                        Return 70
                    Case -2147287015
                        Return &H8003
                    Case -2147287011
                        Return &H8005
                    Case -2147287010
                        Return &H8004
                    Case -2147287008
                        Return &H4B
                    Case -2147287007
                        Return 70
                    Case -2147286960
                        Return &H3A
                    Case -2147286928
                        Return &H3D
                    Case -2147286789
                        Return &H8018
                    Case -2147286788
                        Return &H35
                    Case -2147286787
                        Return &H8018
                    Case -2147286786
                        Return &H8000
                    Case -2147286784
                        Return 70
                    Case -2147286783
                        Return 70
                    Case -2147286782
                        Return &H8005
                    Case -2147286781
                        Return &H39
                    Case -2147286780
                        Return &H8019
                    Case -2147286779
                        Return &H8019
                    Case -2147286778
                        Return &H8015
                    Case -2147286777
                        Return &H8019
                    Case -2147286776
                        Return &H8019
                    Case -2147221230
                        Return &H1AD
                    Case -2147221164
                        Return &H1AD
                    Case -2147221021
                        Return &H1AD
                    Case -2147221018
                        Return &H1B0
                    Case -2147221014
                        Return &H1B0
                    Case -2147221005
                        Return &H1AD
                    Case -2147221003
                        Return &H1AD
                    Case -2147220994
                        Return &H1AD
                    Case -2147024891
                        Return 70
                    Case -2147024882
                        Return 7
                    Case -2147024809
                        Return 5
                    Case -2147023174
                        Return &H1CE
                    Case -2146959355
                        Return &H1AD
                End Select
            End If
            Return lNumber
        End Function

        Friend Shared Function MemberToString(ByVal Member As MemberInfo) As String
            Select Case Member.MemberType
                Case MemberTypes.Constructor, MemberTypes.Method
                    Return Utils.MethodToString(DirectCast(Member, MethodBase))
                Case MemberTypes.Field
                    Return Utils.FieldToString(DirectCast(Member, FieldInfo))
                Case MemberTypes.Property
                    Return Utils.PropertyToString(DirectCast(Member, PropertyInfo))
            End Select
            Return Member.Name
        End Function

        Public Shared Function MethodToString(ByVal Method As MethodBase) As String
            Dim flag As Boolean
            Dim typ As Type = Nothing
            Dim str As String = ""
            If (Method.MemberType = MemberTypes.Method) Then
                typ = DirectCast(Method, MethodInfo).ReturnType
            End If
            If Method.IsPublic Then
                str = (str & "Public ")
            ElseIf Method.IsPrivate Then
                str = (str & "Private ")
            ElseIf Method.IsAssembly Then
                str = (str & "Friend ")
            End If
            If ((Method.Attributes And MethodAttributes.Virtual) <> MethodAttributes.ReuseSlot) Then
                If Not Method.DeclaringType.IsInterface Then
                    str = (str & "Overrides ")
                End If
                str = (str & "Shared ")
            End If
            str = (str & "Narrowing ")
            str = (str & "Widening ")
            str = (str & "Operator ")
            str = (str & "Function ")
            str = (str & "New")
            str = (str & Method.Name)
            str = (str & "(Of ")
            flag = True
            If Not flag Then
                str = (str & ", ")
            Else
                flag = False
            End If
            str = (str & ")")
            str = (str & "(")
            flag = True
            Dim info As ParameterInfo
            For Each info In Method.GetParameters
                If Not flag Then
                    str = (str & ", ")
                Else
                    flag = False
                End If
                str = (str & Utils.ParameterToString(info))
            Next
            str = (str & ")")
            If ((typ Is Nothing) OrElse (typ Is Utils.VoidType)) Then
                Return str
            End If
            Return (str & " As " & Utils.VBFriendlyNameOfType(typ, True))
        End Function

        Friend Shared Function OctFromLong(ByVal Val As Long) As String
            Dim flag As Boolean
            Dim expression As String = ""
            Dim num As Integer = Convert.ToInt32("0"c)
            If (Val < 0) Then
                Val = ((&H7FFFFFFFFFFFFFFF + Val) + 1)
                flag = True
            End If
            Do
                Dim num2 As Integer = CInt((Val Mod 8))
                Val = (Val >> 3)
            Loop While (Val > 0)
            expression = Strings.StrReverse(expression)
            If flag Then
                expression = ("1" & expression)
            End If
            Return expression
        End Function

        Friend Shared Function OctFromULong(ByVal Val As UInt64) As String
            Dim expression As String = ""
            Dim num As Integer = Convert.ToInt32("0"c)
            Do
                Dim num2 As Integer = CInt((Val Mod CULng(8)))
                Val = (Val >> 3)
            Loop While (Val <> 0)
            Return Strings.StrReverse(expression)
        End Function

        Friend Shared Function ParameterToString(ByVal Parameter As ParameterInfo) As String
            Dim str2 As String = ""
            Dim parameterType As Type = Parameter.ParameterType
            If Parameter.IsOptional Then
                str2 = (str2 & "[")
            End If
            If parameterType.IsByRef Then
                str2 = (str2 & "ByRef ")
                parameterType = parameterType.GetElementType
                str2 = (str2 & "ParamArray ")
            End If
            str2 = (str2 & Parameter.Name & " As " & Utils.VBFriendlyNameOfType(parameterType, True))
            If Not Parameter.IsOptional Then
                Return str2
            End If
            Dim defaultValue As Object = Parameter.DefaultValue
            If (defaultValue Is Nothing) Then
                str2 = (str2 & " = Nothing")
            Else
                Dim type As Type = defaultValue.GetType
                If (Not type Is Utils.VoidType) Then
                Else
                End If
            End If
            Return (str2 & "]")
        End Function

        Friend Shared Function PropertyToString(ByVal Prop As PropertyInfo) As String
            Dim parameters As ParameterInfo()
            Dim returnType As Type
            Dim str2 As String = ""
            Dim readWrite As PropertyKind = PropertyKind.ReadWrite
            Dim getMethod As MethodInfo = Prop.GetGetMethod
            If (Not getMethod Is Nothing) Then
                If (Not Prop.GetSetMethod Is Nothing) Then
                    readWrite = PropertyKind.ReadWrite
                Else
                    readWrite = PropertyKind.ReadOnly
                End If
                parameters = getMethod.GetParameters
                returnType = getMethod.ReturnType
            Else
                readWrite = PropertyKind.WriteOnly
                getMethod = Prop.GetSetMethod
                Dim sourceArray As ParameterInfo() = getMethod.GetParameters
                parameters = New ParameterInfo(((sourceArray.Length - 2) + 1) - 1) {}
                Array.Copy(sourceArray, parameters, parameters.Length)
                returnType = sourceArray((sourceArray.Length - 1)).ParameterType
            End If
            str2 = (str2 & "Public ")
            If ((getMethod.Attributes And MethodAttributes.Virtual) <> MethodAttributes.ReuseSlot) Then
                If Not Prop.DeclaringType.IsInterface Then
                    str2 = (str2 & "Overrides ")
                End If
                str2 = (str2 & "Shared ")
            End If
            Select Case readWrite
                Case PropertyKind.ReadOnly
                    str2 = (str2 & "ReadOnly ")
                    Exit Select
                Case PropertyKind.WriteOnly
                    str2 = (str2 & "WriteOnly ")
                    Exit Select
            End Select
            str2 = (str2 & "Property " & Prop.Name & "(")
            Dim flag As Boolean = True
            Dim info2 As ParameterInfo
            For Each info2 In parameters
                If Not flag Then
                    str2 = (str2 & ", ")
                Else
                    flag = False
                End If
                str2 = (str2 & Utils.ParameterToString(info2))
            Next
            Return (str2 & ") As " & Utils.VBFriendlyNameOfType(returnType, True))
        End Function

        <HostProtection(SecurityAction.LinkDemand, Resources:=HostProtectionResource.SelfAffectingThreading)> _
        Public Shared Function SetCultureInfo(ByVal Culture As CultureInfo) As Object
            Dim currentCulture As CultureInfo = Thread.CurrentThread.CurrentCulture
            Thread.CurrentThread.CurrentCulture = Culture
            Return currentCulture
        End Function


        Friend Shared Function StdFormat(ByVal s As String) As String
            Dim ch As Char
            Dim ch2 As Char
            Dim ch3 As Char
            Dim numberFormat As NumberFormatInfo = Thread.CurrentThread.CurrentCulture.NumberFormat
            Dim index As Integer = s.IndexOf(numberFormat.NumberDecimalSeparator)
            If (index = -1) Then
                Return s
            End If
            Try
                ch = s.Chars(0)
                ch2 = s.Chars(1)
                ch3 = s.Chars(2)
            Catch exception As StackOverflowException
                Throw exception
            Catch exception2 As OutOfMemoryException
                Throw exception2
            Catch exception3 As ThreadAbortException
                Throw exception3
            Catch exception6 As Exception
            End Try
            If (s.Chars(index) = "."c) Then
                If ((ch = "0"c) AndAlso (ch2 = "."c)) Then
                    Return s.Substring(1)
                End If
                If (((ch <> "-"c) AndAlso (ch <> "+"c)) AndAlso (ch <> " "c)) Then
                    Return s
                End If
                If ((ch2 <> "0"c) OrElse (ch3 <> "."c)) Then
                    Return s
                End If
            End If
            Dim builder As New StringBuilder(s)
            builder.Chars(index) = "."c
            If ((ch = "0"c) AndAlso (ch2 = "."c)) Then
                Return builder.ToString(1, (builder.Length - 1))
            End If
            If ((((ch = "-"c) OrElse (ch = "+"c)) OrElse (ch = " "c)) AndAlso ((ch2 = "0"c) AndAlso (ch3 = "."c))) Then
                builder.Remove(1, 1)
                Return builder.ToString
            End If
            Return builder.ToString
        End Function

        Public Shared Sub ThrowException(ByVal hr As Integer)
        End Sub



        Friend Shared Function VBFriendlyName(ByVal Obj As Object) As String
            If (Obj Is Nothing) Then
                Return "Nothing"
            End If
            Return Utils.VBFriendlyName(Obj.GetType, Obj)
        End Function

        Friend Shared Function VBFriendlyName(ByVal typ As Type) As String
            Return Utils.VBFriendlyNameOfType(typ, False)
        End Function

        Friend Shared Function VBFriendlyName(ByVal typ As Type, ByVal o As Object) As String
            If (typ.IsCOMObject AndAlso (typ.FullName = "System.__ComObject")) Then
            End If
            Return Utils.VBFriendlyNameOfType(typ, False)
        End Function

        Friend Shared Function VBFriendlyNameOfType(ByVal typ As Type, Optional ByVal FullName As Boolean = False) As String
            Dim name As String
            Dim typeCode As TypeCode
            Dim arraySuffixAndElementType As String = Utils.GetArraySuffixAndElementType((typ))
            If typ.IsEnum Then
                typeCode = typeCode.Object
            Else
                typeCode = Type.GetTypeCode(typ)
            End If
            Select Case typeCode
                Case typeCode.DBNull
                    name = "DBNull"
                    Exit Select
                Case typeCode.Boolean
                    name = "Boolean"
                    Exit Select
                Case typeCode.Char
                    name = "Char"
                    Exit Select
                Case typeCode.SByte
                    name = "SByte"
                    Exit Select
                Case typeCode.Byte
                    name = "Byte"
                    Exit Select
                Case typeCode.Int16
                    name = "Short"
                    Exit Select
                Case typeCode.UInt16
                    name = "UShort"
                    Exit Select
                Case typeCode.Int32
                    name = "Integer"
                    Exit Select
                Case typeCode.UInt32
                    name = "UInteger"
                    Exit Select
                Case typeCode.Int64
                    name = "Long"
                    Exit Select
                Case typeCode.UInt64
                    name = "ULong"
                    Exit Select
                Case typeCode.Single
                    name = "Single"
                    Exit Select
                Case typeCode.Double
                    name = "Double"
                    Exit Select
                Case typeCode.Decimal
                    name = "Decimal"
                    Exit Select
                Case typeCode.DateTime
                    name = "Date"
                    Exit Select
                Case typeCode.String
                    name = "String"
                    Exit Select
                Case Else
                    name = typ.Name
                    Dim str6 As String = Nothing
                    Dim genericArgsSuffix As String = Utils.GetGenericArgsSuffix(typ)
                    If FullName Then
                        If typ.IsNested Then
                            str6 = Utils.VBFriendlyNameOfType(typ.DeclaringType, True)
                            FullName = typ.Name
                        Else
                            FullName = typ.FullName
                        End If
                    Else
                        FullName = typ.Name
                    End If
                    If (Not genericArgsSuffix Is Nothing) Then
                    End If
                    name = (FullName & genericArgsSuffix)
                    name = FullName
                    If (Not str6 Is Nothing) Then
                        name = (str6 & "." & name)
                    End If
                    Exit Select
            End Select
            If (Not arraySuffixAndElementType Is Nothing) Then
                name = (name & arraySuffixAndElementType)
            End If
            Return name
        End Function


        ' Properties
        Friend Shared ReadOnly Property VBAResourceManager() As ResourceManager
            Get
                If (Utils.m_VBAResourceManager Is Nothing) Then
                    Dim resourceManagerSyncObj As Object = Utils.ResourceManagerSyncObj
                    SyncLock resourceManagerSyncObj
                        If Not Utils.m_TriedLoadingResourceManager Then
                            Try
                                Utils.m_VBAResourceManager = New ResourceManager("Microsoft.VisualBasic", Assembly.GetExecutingAssembly)
                            Catch exception As StackOverflowException
                                Throw exception
                            Catch exception2 As OutOfMemoryException
                                Throw exception2
                            Catch exception3 As ThreadAbortException
                                Throw exception3
                            Catch exception6 As Exception
                            End Try
                            Utils.m_TriedLoadingResourceManager = True
                        End If
                    End SyncLock
                End If
                Return Utils.m_VBAResourceManager
            End Get
        End Property

        Friend Shared ReadOnly Property VBRuntimeAssembly() As Assembly
            Get
                If (Utils.m_VBRuntimeAssembly Is Nothing) Then
                    Utils.m_VBRuntimeAssembly = Assembly.GetExecutingAssembly
                End If
                Return Utils.m_VBRuntimeAssembly
            End Get
        End Property


        ' Fields
        Friend Const chBackslash As Char = "\"c
        Friend Const chCharH0A As Char = ChrW(10)
        Friend Const chCharH0B As Char = ChrW(11)
        Friend Const chCharH0C As Char = ChrW(12)
        Friend Const chCharH0D As Char = ChrW(13)
        Friend Const chColon As Char = ":"c
        Friend Const chDblQuote As Char = """"c
        Friend Const chGenericManglingChar As Char = "`"c
        Friend Const chHyphen As Char = "-"c
        Friend Const chIntlSpace As Char = ChrW(12288)
        Friend Const chLetterA As Char = "A"c
        Friend Const chLetterZ As Char = "Z"c
        Friend Const chLineFeed As Char = ChrW(10)
        Friend Const chPeriod As Char = "."c
        Friend Const chPlus As Char = "+"c
        Friend Const chSlash As Char = "/"c
        Friend Const chSpace As Char = " "c
        Friend Const chTab As Char = ChrW(9)
        Friend Const chZero As Char = "0"c
        Private Const ERROR_INVALID_PARAMETER As Integer = &H57
        Friend Const FACILITY_CONTROL As Integer = &HA0000
        Friend Const FACILITY_ITF As Integer = &H40000
        Friend Const FACILITY_RPC As Integer = &H10000
        Friend Shared m_achIntlSpace As Char() = New Char() {" "c, ChrW(12288)}
        Private Shared m_TriedLoadingResourceManager As Boolean
        Private Shared m_VBAResourceManager As ResourceManager
        Private Shared m_VBRuntimeAssembly As Assembly
        Friend Const OptionCompareTextFlags As CompareOptions = (CompareOptions.IgnoreWidth Or (CompareOptions.IgnoreKanaType Or CompareOptions.IgnoreCase))
        Private Shared ReadOnly ResourceManagerSyncObj As Object = New Object
        Private Const ResourceMsgDefault As String = "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found."
        Friend Const SCODE_FACILITY As Integer = &H1FFF0000
        Friend Const SEVERITY_ERROR As Integer = -2147483648
        Private Const VBDefaultErrorID As String = "ID95"
        Private Shared ReadOnly VoidType As Type = Type.GetType("System.Void")

        ' Nested Types
        Private Enum PropertyKind
            ' Fields
            [ReadOnly] = 1
            ReadWrite = 0
            [WriteOnly] = 2
        End Enum
    End Class

End Class