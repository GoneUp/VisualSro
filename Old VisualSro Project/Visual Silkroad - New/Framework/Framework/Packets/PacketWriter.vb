Imports System.IO

Public Class PacketWriter : Implements IDisposable

#Region "Propertys/Fields"
    Private m_bw As BinaryWriter
    Private m_ms As New MemoryStream()

    Public Property BaseStream As MemoryStream
        Get
            Return m_ms
        End Get
        Set(ByVal value As MemoryStream)
            m_ms = value
        End Set
    End Property

    Public ReadOnly Property Length() As UInt16
        Get
            If m_ms IsNot Nothing Then
                Return m_ms.Length
            Else
                Return 0
            End If
        End Get
    End Property
#End Region

#Region "General Functions"
    Public Sub New()

    End Sub

    Public Sub New(ByVal opcode As UShort)
        Me.m_bw = Nothing
        Me.m_ms = Nothing
        Me.m_ms = New MemoryStream()
        Me.m_bw = New BinaryWriter(Me.m_ms)
        Me.m_bw.Write(CUShort(0))
        Me.Word(opcode)
        Me.Word(0)
    End Sub

    Public Sub Create(ByVal opcode As UShort)
        Me.m_bw = Nothing
        Me.m_ms = Nothing
        Me.m_ms = New MemoryStream()
        Me.m_bw = New BinaryWriter(Me.m_ms)
        Me.m_bw.Write(CUShort(0))
        Me.Word(opcode)
        Me.Word(0)
    End Sub

    Public Function GetBytes() As Byte()
        Dim buffer(0) As Byte
        Me.m_ms.Position = 0
        Me.m_bw.Write(m_bw.BaseStream.Length - 6)
        buffer = Me.m_ms.ToArray()

        Dispose()

        Return buffer
    End Function
#End Region

    Public Sub [Byte](ByVal data As Byte)
        Me.m_bw.Write(data)
    End Sub

    Public Sub [Byte](ByVal array As Byte())
        For Each b As Byte In array
            Me.m_bw.Write(b)
        Next
    End Sub

    Public Sub Word(ByVal data As UShort)
        Me.m_bw.Write(data)
    End Sub

    Public Sub DWord(ByVal data As UInteger)
        Me.m_bw.Write(data)
    End Sub

    Public Sub Float(ByVal data As Single)
        Me.m_bw.Write(data)
    End Sub

    Public Sub QWord(ByVal data As ULong)
        Me.m_bw.Write(data)
    End Sub

    Public Sub [String](ByVal data As String)
        Dim chArray(data.Length - 1) As Char
        For i As Integer = 0 To data.Length - 1
            chArray(i) = Convert.ToChar(data.Substring(i, 1))
            Me.m_bw.Write(chArray(i))
        Next i
    End Sub

    Public Sub UString(ByVal data As String)
        Dim chArray(data.Length - 1) As Char
        For i As Integer = 0 To data.Length - 1
            chArray(i) = Convert.ToChar(data.Substring(i, 1))
            Me.m_bw.Write(chArray(i))
            Me.m_bw.Write(CByte(0))
        Next i
    End Sub

    Public Sub [Date](data As Date)
        Me.m_bw.Write(Convert.ToUInt16(data.Year)) 'jahr
        Me.m_bw.Write(Convert.ToUInt16(data.Month)) 'monat
        Me.m_bw.Write(Convert.ToUInt16(data.Day)) 'tag
        Me.m_bw.Write(Convert.ToUInt16(data.Hour)) 'stunde
        Me.m_bw.Write(Convert.ToUInt16(data.Minute)) 'minute
        Me.m_bw.Write(Convert.ToUInt16(data.Second)) 'sekunde
        Me.m_bw.Write(Convert.ToUInt32(data.Millisecond)) 'tag
    End Sub

#Region "IDisposable"
    Private m_disponsed As Boolean = False
    Sub Dispose() Implements IDisposable.Dispose
        If m_disponsed = False Then
            Me.m_bw.Close()
            Me.m_ms.Close()
        End If

        Me.m_bw = Nothing
        Me.m_ms = Nothing

        Console.WriteLine("Cleanup..")
        GC.SuppressFinalize(Me)
        m_disponsed = True
    End Sub
#End Region
End Class

