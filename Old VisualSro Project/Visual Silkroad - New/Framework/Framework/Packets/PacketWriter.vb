Imports System.IO

Public Class PacketWriter

#Region "Propertys/Fileds"
    Private bw As BinaryWriter
    Private dataLen As UInt16

    Private ms As New MemoryStream()
    Public Property BaseStream As MemoryStream
        Get
            Return ms
        End Get
        Set(ByVal value As MemoryStream)
            ms = value
        End Set
    End Property

    Public ReadOnly Property Length() As UInt16
        Get
            If dataLen = 0 Then
                Return 0
            End If
            Return dataLen / 2
        End Get
    End Property
#End Region

#Region "General Functions"
    Public Sub New()

    End Sub

    Public Sub New(ByVal opcode As UShort)
        Me.bw = Nothing
        Me.ms = Nothing
        Me.ms = New MemoryStream()
        Me.bw = New BinaryWriter(Me.ms)
        Me.bw.Write(CUShort(0))
        Me.dataLen = 0
        Me.Word(opcode)
        Me.Word(0)
    End Sub

    Public Sub Create(ByVal opcode As UShort)
        Me.bw = Nothing
        Me.ms = Nothing
        Me.ms = New MemoryStream()
        Me.bw = New BinaryWriter(Me.ms)
        Me.bw.Write(CUShort(0))
        Me.dataLen = 0
        Me.Word(opcode)
        Me.Word(0)
    End Sub

    Public Function GetBytes() As Byte()
        Dim buffer(0) As Byte
        Me.ms.Position = 0
        Dim num As UShort = (bw.BaseStream.Length - 6)
        Me.bw.Write(num)
        Me.bw.Close()
        buffer = Me.ms.ToArray()
        Me.ms.Close()
        Return buffer
    End Function
#End Region

    Public Sub [Byte](ByVal data As Byte)
        Me.bw.Write(data)
        Me.dataLen += 2
    End Sub

    Public Sub [Byte](ByVal array As Byte())
        For Each b As Byte In array
            Me.bw.Write(b)
            Me.dataLen += 2
        Next
    End Sub

    Public Sub Word(ByVal data As UShort)
        Me.bw.Write(data)
        Me.dataLen += 4
    End Sub

    Public Sub DWord(ByVal data As UInteger)
        Me.bw.Write(data)
        Me.dataLen += 8
    End Sub

    Public Sub Float(ByVal data As Single)
        Me.bw.Write(data)
        Me.dataLen += 8
    End Sub

    Public Sub QWord(ByVal data As ULong)
        Me.bw.Write(data)
        Me.dataLen += 16
    End Sub

    Public Sub [String](ByVal data As String)
        Dim chArray(data.Length - 1) As Char
        For i As Integer = 0 To data.Length - 1
            chArray(i) = Convert.ToChar(data.Substring(i, 1))
            Me.bw.Write(chArray(i))
        Next i
        Me.dataLen += data.Length * 2
    End Sub

    Public Sub UString(ByVal data As String)
        Dim chArray(data.Length - 1) As Char
        For i As Integer = 0 To data.Length - 1
            chArray(i) = Convert.ToChar(data.Substring(i, 1))
            Me.bw.Write(chArray(i))
            Me.bw.Write(CByte(0))
        Next i
        Me.dataLen += data.Length * 4
    End Sub

    Public Sub [Date](data As Date)
        Me.bw.Write(Convert.ToUInt16(data.Year)) 'jahr
        Me.bw.Write(Convert.ToUInt16(data.Month)) 'monat
        Me.bw.Write(Convert.ToUInt16(data.Day)) 'tag
        Me.bw.Write(Convert.ToUInt16(data.Hour)) 'stunde
        Me.bw.Write(Convert.ToUInt16(data.Minute)) 'minute
        Me.bw.Write(Convert.ToUInt16(data.Second)) 'sekunde
        Me.bw.Write(Convert.ToUInt32(data.Millisecond)) 'tag
        Me.dataLen += 32
    End Sub
End Class

