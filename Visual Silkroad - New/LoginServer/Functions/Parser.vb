Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Namespace LoginServer

    Public Module Parser
        Public Sub Parse(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            ClientList.LastPingTime(Index_) = Date.Now

            Select Case opcode
                Case ClientOpcodes.Ping

                Case ClientOpcodes.Handshake  'Client accepts

                Case ClientOpcodes.InfoReq  'GateWay
                    Functions.GateWay(packet, Index_)

                Case ClientOpcodes.PatchReq  'Client sends Patch Info
                    Functions.ClientInfo(packet, Index_)
                    Functions.SendPatchInfo(Index_)

                Case ClientOpcodes.LauncherReq
                    Functions.SendLauncherInfo(Index_)

                Case ClientOpcodes.ServerListReq
                    Functions.SendServerList(Index_)

                Case ClientOpcodes.Login
                    Functions.HandleLogin(packet, Index_)

                Case Else
                    Log.WriteSystemLog("opCode: " & opcode) '& " Packet : " & packet.Byte)
            End Select
        End Sub
    End Module
End Namespace

