Imports Microsoft.VisualBasic
	Imports System
	Imports System.IO
	Imports System.Text
Namespace GameServer

    Public Class PacketReader
        Private br As BinaryReader
        Private ms As MemoryStream

        Public Sub New(ByVal data() As Byte)
            Me.ms = New MemoryStream(data)
            Me.br = New BinaryReader(Me.ms)
        End Sub

        Public Function [Byte]() As Byte
            Return Me.br.ReadByte()
        End Function

        Public Sub Close()
            Me.br.Close()
            Me.ms.Close()
        End Sub

        Public Function DWord() As UInteger
            Return Me.br.ReadUInt32()
        End Function

        Public Function Float() As Single
            Return Me.br.ReadSingle()
        End Function

        Public Function QWord() As ULong
            Return Me.br.ReadUInt64()
        End Function

        Public Sub Skip(ByVal HowMany As Integer)
            For i As Integer = 1 To (HowMany \ 2)
                Me.br.ReadByte()
            Next i
        End Sub

        Public Function [String](ByVal len As Integer) As String
            Dim builder As New StringBuilder()
            For Each ch As Char In Me.br.ReadChars(len)
                builder.Append(ch.ToString())
            Next ch
            Return builder.ToString()
        End Function

        Public Function UString(ByVal len As Integer) As String
            Dim builder As New StringBuilder()
            For Each ch As Char In Me.br.ReadChars(len)
                builder.Append(ch.ToString())
            Next ch
            Return builder.ToString()
        End Function

        Public Function Word() As UShort
            Return Me.br.ReadUInt16()
        End Function
        Public Function WordInt() As Short
            Return Me.br.ReadInt16()
        End Function

        Public Function [Boolean]() As Boolean
            Return Me.br.ReadBoolean()
        End Function

        Public Function ByteArray(ByVal Count As Integer) As Byte()
            Return Me.br.ReadBytes(Count)
        End Function
    End Class
End Namespace

