Imports Microsoft.VisualBasic
	Imports System
	Imports System.Runtime.InteropServices
Namespace GameServer

    Public Class Parser

        Public Shared Sub Parse(ByVal rp As GameServer.ReadPacket)

            Dim packet As New GameServer.PacketReader(rp.buffer)
            Dim lastpingtime As DateTime = ClientList.LastPingTime(rp.index)

            ClientList.LastPingTime(rp.index) = DateTime.Now


                Select Case rp.opcode

                Case ClientOpcodes.Ping

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

                Case &H34C5
                    GameServer.Functions.OnJoinWorldRequest(rp.index)
                    Commands.WriteLog("[Join_World][2][Index: " & rp.index & "]")

                    '============Ingame===========

                Case ClientOpcodes.Movement
                    GameServer.Functions.Players.OnPlayerMovement(rp.index, packet)

                Case ClientOpcodes.Chat
                    GameServer.Functions.OnChat(packet, rp.index)

                Case ClientOpcodes.GameMaster
                    GameServer.Functions.OnGM(packet, rp.index)

                Case ClientOpcodes.Action
                    GameServer.Functions.OnPlayerAction(packet, rp.index)

                Case ClientOpcodes.Angle_Update
                    GameServer.Functions.OnAngleUpdate(packet, rp.index)

                Case ClientOpcodes.Teleport_Reply
                    GameServer.Functions.OnTeleportRequest(rp.index)

                Case ClientOpcodes.Exit
                    GameServer.Functions.OnLogout(packet, rp.index)

                Case ClientOpcodes.Emotion
                    GameServer.Functions.OnEmotion(packet, rp.index)

                Case ClientOpcodes.HelperIcon
                    GameServer.Functions.OnHelperIcon(packet, rp.index)

                    '=======ITEMS======
                Case ClientOpcodes.Alchemy
                    GameServer.Functions.OnAlchemyRequest(packet, rp.index)

                Case ClientOpcodes.ItemMove
                    GameServer.Functions.OnInventory(packet, rp.index)

                Case ClientOpcodes.ItemUse
                    GameServer.Functions.OnUseItem(packet, rp.index)
                    Debug.Print(rp.data)

                Case ClientOpcodes.Scroll_Cancel
                    GameServer.Functions.OnReturnScroll_Cancel(rp.index)

                    '========STR+INT UP=====

                Case ClientOpcodes.Str_Up
                    GameServer.Functions.UpStrength(rp.index)

                Case ClientOpcodes.Int_Up
                    GameServer.Functions.UpIntelligence(rp.index)

                    '======Mastery+Skills

                Case ClientOpcodes.Mastery_Up
                    GameServer.Functions.OnUpMastery(packet, rp.index)

                Case ClientOpcodes.Skill_Up
                    GameServer.Functions.OnAddSkill(packet, rp.index)

                    '=========EXCHANGE======
                Case ClientOpcodes.Exchange_Invite
                    GameServer.Functions.OnExchangeInvite(packet, rp.index)

                Case ClientOpcodes.Exchange_Invite_Accept
                    GameServer.Functions.OnExchangeInviteReply(packet, rp.index)

                Case ClientOpcodes.Exchange_Confirm
                    GameServer.Functions.OnExchangeConfirm(packet, rp.index)

                Case ClientOpcodes.Exchange_Approve
                    GameServer.Functions.OnExchangeApprove(packet, rp.index)

                Case ClientOpcodes.Exchange_Abort
                    GameServer.Functions.OnExchangeAbort(packet, rp.index)

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

