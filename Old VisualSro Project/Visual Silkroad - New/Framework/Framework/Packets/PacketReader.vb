Imports System.IO
Public Class PacketReader
    Private ReadOnly _br As BinaryReader
    Private ReadOnly _ms As MemoryStream
    Private ReadOnly _packetData() As Byte

    Public Sub New(ByVal data() As Byte)
        Me._ms = New MemoryStream(data)
        Me._br = New BinaryReader(Me._ms)
        _packetData = data
    End Sub

    Public Function [Byte]() As Byte
        Return Me._br.ReadByte()
    End Function

    Public Sub Close()
        Me._br.Close()
        Me._ms.Close()
    End Sub

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

    Public Sub Skip(ByVal howMany As Integer)
        For i = 1 To (HowMany \ 2)
            Me._br.ReadByte()
        Next i
    End Sub

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

    Public Function Word() As UShort
        Return Me._br.ReadUInt16()
    End Function
    Public Function WordInt() As Short
        Return Me._br.ReadInt16()
    End Function

    Public Function [Boolean]() As Boolean
        Return Me._br.ReadBoolean()
    End Function

    Public Function ByteArray(ByVal count As Integer) As Byte()
        Return Me._br.ReadBytes(Count)
    End Function

    Public Function GetData() As Byte()
        Return Me._packetData
    End Function

    Public ReadOnly Property DataLen() As Integer
        Get
            Return Me._ms.Length
        End Get
    End Property



End Class


