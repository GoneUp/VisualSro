Imports Microsoft.VisualBasic
	Imports System
	Imports System.Runtime.InteropServices
Namespace LoginServer

    Public Class Parser

        Public Shared Sub Parse(ByVal rp As ReadPacket)

            Dim buff As Byte() = rp.buffer
            Dim pack As New PacketReader(buff)

            Select Case rp.opcode

                Case ClientOpcodes.Ping


                Case ClientOpcodes.Handshake  'Client accepts

                Case ClientOpcodes.InfoReq  'GateWay
                    GateWay(rp.index)

                Case ClientOpcodes.PatchReq  'Client sends Patch Info
                    SendPatchInfo(rp.index)

                Case ClientOpcodes.LauncherReq
                    SendLauncherInfo(rp.index)

                Case ClientOpcodes.ServerListReq
                    SendServerList(rp.index)

                Case ClientOpcodes.Login

                    HandleLogin(pack, rp.index)

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

