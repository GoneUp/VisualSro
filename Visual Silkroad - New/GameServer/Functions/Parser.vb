Imports Microsoft.VisualBasic
	Imports System
	Imports System.Runtime.InteropServices
Namespace GameServer

    Public Class Parser

        Public Shared Sub Parse(ByVal rp As GameServer.ReadPacket)

            Dim buff As Byte() = rp.buffer
            Dim pack As New GameServer.PacketReader(buff)

            Select Case rp.opcode

                Case ClientOpcodes.Ping

                Case ClientOpcodes.Handshake  'Client accepts

                Case ClientOpcodes.InfoReq  'GateWay
                    GameServer.Functions.GateWay(rp.index)

                Case ClientOpcodes.Login
                    GameServer.Functions.CheckLogin(rp.index, pack)

                Case ClientOpcodes.Character
                    GameServer.Functions.HandleCharPacket(rp.index, pack)

                Case ClientOpcodes.IngameReq
                    GameServer.Functions.CharLoading(rp.index, pack)


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

