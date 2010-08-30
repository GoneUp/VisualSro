Imports Microsoft.VisualBasic
	Imports System
	Imports System.Runtime.InteropServices
Namespace GameServer

    Public Class Parser

        Public Shared Sub Parse(ByVal rp As GameServer.ReadPacket)

            Dim packet As New GameServer.PacketReader(rp.buffer)

            Select Case rp.opcode

                Case ClientOpcodes.Ping
                    Dim writer As New PacketWriter
                    writer.Create(&H3055)
                    writer.Word(&H5A30)
                    'Server.Send(writer.GetBytes, rp.index)

                    '=============Login=============
                Case ClientOpcodes.Handshake  'Client accepts

                Case ClientOpcodes.InfoReq  'GateWay
                    GameServer.Functions.GateWay(rp.index)

                Case ClientOpcodes.Login
                    GameServer.Functions.CheckLogin(rp.index, packet)

                Case ClientOpcodes.Character
                    GameServer.Functions.HandleCharPacket(rp.index, packet)

                Case ClientOpcodes.IngameReq
                    GameServer.Functions.CharLoading(rp.index, packet)

                Case ClientOpcodes.JoinWorldReq
                    GameServer.Functions.OnJoinWorldRequest(rp.index)
                    Commands.WriteLog("[Join_World][1][Index: " & rp.index & "]")

                Case "34C5"
                    GameServer.Functions.OnJoinWorldRequest(rp.index)
                    Commands.WriteLog("[Join_World][2][Index: " & rp.index & "]")

                    '============Ingame===========

                Case ClientOpcodes.Movement
                    GameServer.Functions.Players.OnPlayerMovement(rp.index, packet)

                Case ClientOpcodes.Chat
                    GameServer.Functions.OnChat(packet, rp.index)

                Case ClientOpcodes.GameMaster
                    GameServer.Functions.OnGM(packet, rp.index)

                Case ClientOpcodes.Alchemy
                    GameServer.Functions.OnAlchemyRequest(packet, rp.index)

                Case ClientOpcodes.ItemMove
                    GameServer.Functions.OnInventory(packet, rp.index)

                Case ClientOpcodes.Exit
                    GameServer.Functions.OnLogout(packet, rp.index)

                Case Else
                    Commands.WriteLog("opCode: " & rp.opcode & " Packet : " & rp.data)
            End Select
        End Sub

        <StructLayout(LayoutKind.Sequential)> _
        Private Structure Packet
            Public test As String
        End Structure
    End Class
End Namespace

