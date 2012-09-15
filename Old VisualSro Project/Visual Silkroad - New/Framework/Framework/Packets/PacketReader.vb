Imports System.IO
Imports System.ComponentModel

Public Class PacketReader

#Region "Fields"
    Private ReadOnly _br As BinaryReader
    Private ReadOnly _ms As MemoryStream
    Private ReadOnly _packetData() As Byte

    Public ReadOnly Property Length() As UShort
        Get
            Return Me._ms.Length
        End Get
    End Property
#End Region

#Region "General Functions"
    Public Sub New(ByVal data() As Byte)
        Me._ms = New MemoryStream(data)
        Me._br = New BinaryReader(Me._ms)
        _packetData = data
    End Sub

    Public Sub Disponse()
        Me._br.Close()
        Me._ms.Close()
    End Sub

    Public Function GetData() As Byte()
        Return Me._packetData
    End Function

    Public Sub Skip(ByVal howMany As Integer)
        For i = 1 To (howMany \ 2)
            Me._br.ReadByte()
        Next i
    End Sub
#End Region

    Public Function [Boolean]() As Boolean
        Return Me._br.ReadBoolean()
    End Function

    Public Function [Byte]() As Byte
        Return Me._br.ReadByte()
    End Function

    Public Function ByteArray(ByVal count As Integer) As Byte()
        Return Me._br.ReadBytes(count)
    End Function

    Public Function Word() As UShort
        Return Me._br.ReadUInt16()
    End Function

    Public Function WordInt() As Short
        Return Me._br.ReadInt16()
    End Function

    Public Function DWord() As UInteger
        Return Me._br.ReadUInt32()
    End Function

    Public Function DWordInt() As Integer
        Return Me._br.ReadInt32()
    End Function

    Public Function Float() As Single
        Return Me._br.ReadSingle()
    End Function

    Public Function QWord() As ULong
        Return Me._br.ReadUInt64()
    End Function

    Public Function [String](ByVal len As Integer) As String
        Dim builder As New Text.StringBuilder()
        For Each ch As Char In Me._br.ReadChars(len)
            builder.Append(ch.ToString())
        Next ch
        Return builder.ToString()
    End Function

    Public Function UString(ByVal len As Integer) As String
        Dim Bytes As Byte() = Me.ByteArray(len * 2)
        Return System.Text.Encoding.Unicode.GetString(Bytes)
    End Function

    Public Function [Date]() As Date
        Dim year As UInt16 = Me._br.ReadUInt16()
        Dim month As UInt16 = Me._br.ReadUInt16()
        Dim day As UInt16 = Me._br.ReadUInt16()
        Dim hour As UInt16 = Me._br.ReadUInt16()
        Dim minute As UInt16 = Me._br.ReadUInt16()
        Dim second As UInt16 = Me._br.ReadUInt16()
        Dim milisecond As UInt16 = Me._br.ReadUInt32()


        Dim culture = New Globalization.CultureInfo("de-DE", True)
        Dim timestring As String = String.Format("{0}/{1}/{2} {3}:{4}:{5}", day, month, year, hour, second)
        Dim myDate As DateTime = _
                             DateTime.Parse(timestring, _
                                            culture, _
                                            Globalization.DateTimeStyles.NoCurrentDateDefault)
        Return myDate
    End Function
End Class


