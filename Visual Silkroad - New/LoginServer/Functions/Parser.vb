Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Namespace LoginServer

    Public Class Parser

        Public Shared Sub Parse(ByVal packet As LoginServer.PacketReader, ByVal index As Integer)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            Select Case opcode
                Case ClientOpcodes.Ping

                Case ClientOpcodes.Handshake  'Client accepts

                Case ClientOpcodes.InfoReq  'GateWay
                    Functions.GateWay(index)

                Case ClientOpcodes.PatchReq  'Client sends Patch Info
                    Functions.ClientInfo(packet)
                    Functions.SendPatchInfo(index)

                Case ClientOpcodes.LauncherReq
                    Functions.SendLauncherInfo(index)

                Case ClientOpcodes.ServerListReq
                    Functions.SendServerList(index)

                Case ClientOpcodes.Login
                    Functions.HandleLogin(packet, index)

                Case Else
                    Commands.WriteLog("opCode: " & opcode) '& " Packet : " & packet.Byte)
            End Select
        End Sub
    End Class
End Namespace

