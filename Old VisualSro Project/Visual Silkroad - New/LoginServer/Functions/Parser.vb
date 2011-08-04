Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Imports LoginServer.Framework

Namespace Functions

    Public Module Parser
        Public Sub Parse(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            ClientList.LastPingTime(Index_) = Date.Now

            Select Case opcode
                Case ClientOpcodes.Ping

                Case ClientOpcodes.Handshake_Confirm  'Client accepts

                Case ClientOpcodes.Login_InfoReq  'GateWay
                    Functions.GateWay(packet, Index_)

                Case ClientOpcodes.Login_PatchReq  'Client sends Patch Info
                    Functions.ClientInfo(packet, Index_)
                    Functions.SendPatchInfo(Index_)

                Case ClientOpcodes.Login_LauncherReq
                    Functions.SendLauncherInfo(Index_)

                Case ClientOpcodes.Login_ServerListReq
                    Functions.SendServerList(Index_)

                Case ClientOpcodes.Login_LoginReq
                    Functions.HandleLogin(packet, Index_)

                Case Else
                    Log.WriteSystemLog("opCode: " & opcode) '& " Packet : " & packet.Byte)
            End Select
        End Sub

        Public Sub ParseGlobalManager(ByVal packet As PacketReader)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            Select Case opcode
                Case ClientOpcodes.Ping

                Case ClientOpcodes.Handshake_Confirm  'Client accepts
                    GlobalManager.OnHandshake(packet)

                    'Case ClientOpcodes.Login_InfoReq  'GateWay
                    '    GlobalManager.GateWa()

                    'Case ClientOpcodes.Login_PatchReq  'Client sends Patch Info
                    '    Functions.ClientInfo(packet, Index_)
                    '    Functions.SendPatchInfo(Index_)

                    'Case ClientOpcodes.Login_LauncherReq
                    '    Functions.SendLauncherInfo(Index_)

                    'Case ClientOpcodes.Login_ServerListReq
                    '    Functions.SendServerList(Index_)

                    'Case ClientOpcodes.Login_LoginReq
                    '    Functions.HandleLogin(packet, Index_)

                Case Else
                    Log.WriteSystemLog("opCode: " & opcode) '& " Packet : " & packet.Byte)
            End Select
        End Sub

    End Module
End Namespace

