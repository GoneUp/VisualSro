Imports Microsoft.VisualBasic
	Imports System
	Imports System.IO
Namespace LoginServer

    Public Class ReadPacket
        Private br As BinaryReader
        Private BUFFER_Renamed() As Byte
        Private DATA_Renamed As String
        Private INDEX_Renamed As Integer
        Private ms As MemoryStream
        Private OPCODE_Renamed As String

        Public Sub New(ByVal buffer() As Byte, ByVal index As Integer)
            Me.INDEX_Renamed = index
            Me.BUFFER_Renamed = buffer
            Me.ms = New MemoryStream(buffer)
            Me.br = New BinaryReader(Me.ms)
            Dim param As String = convert.BytesToHex(buffer)
            Dim num As UShort = Me.br.ReadUInt16()
            If num > 0 Then
                Me.DATA_Renamed = convert.Mid(param, 12, num * 2)
                Me.BUFFER_Renamed = convert.ToByteArray(Me.DATA_Renamed)
            End If
            Me.OPCODE_Renamed = Me.br.ReadUInt16().ToString("X")
            Me.br.ReadUInt16()
        End Sub

        Public ReadOnly Property buffer() As Byte()
            Get
                Return Me.BUFFER_Renamed
            End Get
        End Property

        Public ReadOnly Property data() As String
            Get
                Return Me.DATA_Renamed
            End Get
        End Property

        Public ReadOnly Property index() As Integer
            Get
                Return Me.INDEX_Renamed
            End Get
        End Property

        Public ReadOnly Property opcode() As String
            Get
                Return Me.OPCODE_Renamed
            End Get
        End Property
    End Class
End Namespace

