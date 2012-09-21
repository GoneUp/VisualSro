Imports System.IO

Public Class PacketReader : Implements IDisposable

#Region "Fields"
    Private m_br As BinaryReader
    Private m_ms As MemoryStream

    Public ReadOnly Property Length() As UShort
        Get
            Return Me.m_ms.Length
        End Get
    End Property
#End Region

#Region "General Functions"
    Public Sub New(ByVal data() As Byte)
        Me.m_ms = New MemoryStream(data)
        Me.m_br = New BinaryReader(Me.m_ms)
    End Sub

    Public Sub Skip(ByVal howMany As Integer)
        For i = 1 To (howMany \ 2)
            Me.m_br.ReadByte()
        Next i
    End Sub
#End Region

    Public Function [Boolean]() As Boolean
        Return Me.m_br.ReadBoolean()
    End Function

    Public Function [Byte]() As Byte
        Return Me.m_br.ReadByte()
    End Function

    Public Function ByteArray(ByVal count As Integer) As Byte()
        Return Me.m_br.ReadBytes(count)
    End Function

    Public Function Word() As UShort
        Return Me.m_br.ReadUInt16()
    End Function

    Public Function WordInt() As Short
        Return Me.m_br.ReadInt16()
    End Function

    Public Function DWord() As UInteger
        Return Me.m_br.ReadUInt32()
    End Function

    Public Function DWordInt() As Integer
        Return Me.m_br.ReadInt32()
    End Function

    Public Function Float() As Single
        Return Me.m_br.ReadSingle()
    End Function

    Public Function QWord() As ULong
        Return Me.m_br.ReadUInt64()
    End Function

    Public Function [String](ByVal len As Integer) As String
        Dim builder As New Text.StringBuilder()
        For Each ch As Char In Me.m_br.ReadChars(len)
            builder.Append(ch.ToString())
        Next ch
        Return builder.ToString()
    End Function

    Public Function UString(ByVal len As Integer) As String
        Dim bytes As Byte() = Me.ByteArray(len * 2)
        Return System.Text.Encoding.Unicode.GetString(bytes)
    End Function

    Public Function [Date]() As Date
        Dim year As UInt16 = Me.m_br.ReadUInt16()
        Dim month As UInt16 = Me.m_br.ReadUInt16()
        Dim day As UInt16 = Me.m_br.ReadUInt16()
        Dim hour As UInt16 = Me.m_br.ReadUInt16()
        Dim minute As UInt16 = Me.m_br.ReadUInt16()
        Dim second As UInt16 = Me.m_br.ReadUInt16()
        Dim milisecond As UInt16 = Me.m_br.ReadUInt32()


        Dim culture = New Globalization.CultureInfo("de-DE", True)
        Dim timestring As String = String.Format("{0}/{1}/{2} {3}:{4}:{5}", day, month, year, hour, second)
        Dim myDate As DateTime = _
                             DateTime.Parse(timestring, _
                                            culture, _
                                            Globalization.DateTimeStyles.NoCurrentDateDefault)
        Return myDate
    End Function

#Region "IDisposable"
    Private m_disponsed As Boolean = False
    Sub Dispose() Implements IDisposable.Dispose
        If m_disponsed = False Then
            Me.m_br.Close()
            Me.m_ms.Close()
        End If

        Me.m_br = Nothing
        Me.m_ms = Nothing

        Console.WriteLine("Cleanup..")
        GC.SuppressFinalize(Me)
        m_disponsed = True
    End Sub
#End Region
End Class


