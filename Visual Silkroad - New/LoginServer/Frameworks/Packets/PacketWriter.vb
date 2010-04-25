Imports Microsoft.VisualBasic
	Imports System
	Imports System.IO
Namespace LoginServer

    Friend Class PacketWriter
        Private bw As BinaryWriter
        Private dataLen As Integer
        Private ms As New MemoryStream()

        Public Sub [Byte](ByVal data As Byte)
            Me.bw.Write(data)
            Me.dataLen += 2
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

        Public Sub DWord(ByVal data As UInteger)
            Me.bw.Write(data)
            Me.dataLen += 8
        End Sub

        Public Sub Float(ByVal data As Single)
            Me.bw.Write(data)
            Me.dataLen += 8
        End Sub

        Public Function GetBytes() As Byte()
            Dim buffer(0) As Byte
            Me.ms.Position = 0L
            Dim num As UShort = (bw.BaseStream.Length - 6)
            Me.bw.Write(num)
            Me.bw.Close()
            buffer = Me.ms.ToArray()
            Me.ms.Close()
            Return buffer
        End Function

        Public Sub HexString(ByVal data As String)
            Dim chArray(data.Length - 1) As Char
            For i As Integer = 0 To data.Length - 1
                chArray(i) = System.Convert.ToChar(data.Substring(i, 1))
                Me.bw.Write(chArray(i))
            Next i
            Me.dataLen += data.Length
        End Sub

        Public Sub LWord(ByVal data As ULong)
            Me.bw.Write(data)
            Me.dataLen += &H10
        End Sub

        Public Sub [String](ByVal data As String)
            Dim chArray(data.Length - 1) As Char
            For i As Integer = 0 To data.Length - 1
                chArray(i) = System.Convert.ToChar(data.Substring(i, 1))
                Me.bw.Write(chArray(i))
            Next i
            Me.dataLen += data.Length * 2
        End Sub

        Public Sub UString(ByVal data As String)
            Dim chArray(data.Length - 1) As Char
            For i As Integer = 0 To data.Length - 1
                chArray(i) = System.Convert.ToChar(data.Substring(i, 1))
                Me.bw.Write(chArray(i))
                Me.bw.Write(CByte(0))
            Next i
            Me.dataLen += data.Length * 4
        End Sub

        Public Sub Word(ByVal data As UShort)
            Me.bw.Write(data)
            Me.dataLen += 4
        End Sub
    End Class
End Namespace

