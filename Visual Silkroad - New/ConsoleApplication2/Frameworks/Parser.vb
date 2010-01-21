Imports Microsoft.VisualBasic
	Imports System
	Imports System.Runtime.InteropServices
Namespace GameServer

    Public Class Parser
        Public Shared Sub Parse(ByVal rp As ReadPacket)
            Select Case rp.opcode
                Case Else
                    Console.WriteLine("opCode: " & rp.opcode & " Packet : " & rp.data)
            End Select
        End Sub

        <StructLayout(LayoutKind.Sequential)> _
        Private Structure Packet
            Public test As String
        End Structure
    End Class
End Namespace

