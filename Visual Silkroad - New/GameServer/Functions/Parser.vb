Imports Microsoft.VisualBasic
	Imports System
	Imports System.Runtime.InteropServices
Namespace GameServer

    Public Class Parser


        Public Shared Sub Parse(ByVal packet As PacketReader, ByVal index As Integer)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            Dim lastpingtime As DateTime = ClientList.LastPingTime(index)

            ClientList.LastPingTime(index) = DateTime.Now


            Select Case opcode

                Case ClientOpcodes.Ping

                    '=============Login=============
                Case ClientOpcodes.Handshake  'Client accepts

                Case ClientOpcodes.InfoReq  'GateWay
                    GameServer.Functions.GateWay(index)

                Case ClientOpcodes.Login
                    GameServer.Functions.CheckLogin(index, packet)

                Case ClientOpcodes.Character
                    GameServer.Functions.HandleCharPacket(index, packet)

                Case ClientOpcodes.IngameReq
                    GameServer.Functions.CharLoading(index, packet)

                Case ClientOpcodes.JoinWorldReq
                    GameServer.Functions.OnJoinWorldRequest(index)

                Case ClientOpcodes.JoinWorldReq2
                    GameServer.Functions.OnJoinWorldRequest(index)

                    '============Ingame===========

                Case ClientOpcodes.Movement
                    GameServer.Functions.OnPlayerMovement(index, packet)

                Case ClientOpcodes.Chat
                    GameServer.Functions.OnChat(packet, index)

                Case ClientOpcodes.GameMaster
                    GameServer.Functions.OnGM(packet, index)

                Case ClientOpcodes.Action
                    GameServer.Functions.OnPlayerAction(packet, index)

                Case ClientOpcodes.Angle_Update
                    GameServer.Functions.OnAngleUpdate(packet, index)

                Case ClientOpcodes.Teleport_Reply
                    GameServer.Functions.OnTeleportRequest(index)

                Case ClientOpcodes.Exit
                    GameServer.Functions.OnLogout(packet, index)

                Case ClientOpcodes.Emotion
                    GameServer.Functions.OnEmotion(packet, index)

                Case ClientOpcodes.HelperIcon
                    GameServer.Functions.OnHelperIcon(packet, index)

                Case ClientOpcodes.Hotkey_Update
                    GameServer.Functions.OnHotkeyUpdate(packet, index)

                Case ClientOpcodes.Target
                    GameServer.Functions.OnSelectObject(packet, index)
                    '=======NPC========

                Case ClientOpcodes.Npc_Chat
                    GameServer.Functions.OnNpcChatSelect(packet, index)

                Case ClientOpcodes.Npc_Chat_Left
                    GameServer.Functions.OnNpcChatLeft(packet, index)

                Case ClientOpcodes.Npc_Teleport
                    GameServer.Functions.OnNpcTeleport(packet, index)

                    '=======ITEMS======
                Case ClientOpcodes.Alchemy
                    GameServer.Functions.OnAlchemyRequest(packet, index)

                Case ClientOpcodes.ItemMove
                    GameServer.Functions.OnInventory(packet, index)

                Case ClientOpcodes.ItemUse
                    GameServer.Functions.OnUseItem(packet, index)

                Case ClientOpcodes.Scroll_Cancel
                    GameServer.Functions.OnReturnScroll_Cancel(index)

                    '========STR+INT UP=====

                Case ClientOpcodes.Str_Up
                    GameServer.Functions.UpStrength(index)

                Case ClientOpcodes.Int_Up
                    GameServer.Functions.UpIntelligence(index)

                    '======Mastery+Skills

                Case ClientOpcodes.Mastery_Up
                    GameServer.Functions.OnUpMastery(packet, index)

                Case ClientOpcodes.Skill_Up
                    GameServer.Functions.OnAddSkill(packet, index)

                    '=========EXCHANGE======
                Case ClientOpcodes.Exchange_Invite
                    GameServer.Functions.OnExchangeInvite(packet, index)

                Case ClientOpcodes.Exchange_Invite_Accept
                    GameServer.Functions.OnExchangeInviteReply(packet, index)

                Case ClientOpcodes.Exchange_Confirm
                    GameServer.Functions.OnExchangeConfirm(packet, index)

                Case ClientOpcodes.Exchange_Approve
                    GameServer.Functions.OnExchangeApprove(packet, index)

                Case ClientOpcodes.Exchange_Abort
                    GameServer.Functions.OnExchangeAbort(packet, index)

                Case Else
                    Log.WriteSystemLog("opCode: " & opcode & " Packet : " & BitConverter.ToString(packet.ByteArray(length), 0, length))
            End Select
        End Sub

    End Class
End Namespace

