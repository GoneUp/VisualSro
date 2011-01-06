Imports Microsoft.VisualBasic
	Imports System
	Imports System.Runtime.InteropServices
Namespace GameServer

    Public Class Parser


        Public Shared Sub Parse(ByVal packet As PacketReader, ByVal index As Integer)
            Try
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
                        Functions.GateWay(index)

                    Case ClientOpcodes.Login
                        Functions.CheckLogin(index, packet)

                    Case ClientOpcodes.Character
                        Functions.HandleCharPacket(index, packet)

                    Case ClientOpcodes.IngameReq
                        Functions.CharLoading(index, packet)

                    Case ClientOpcodes.JoinWorldReq
                        Functions.OnJoinWorldRequest(index)

                    Case ClientOpcodes.JoinWorldReq2
                        Functions.OnJoinWorldRequest(index)

                    Case ClientOpcodes.ClientStatus
                        Functions.OnClientStatusUpdate(packet, index)

                        '============Ingame===========

                    Case ClientOpcodes.Movement
                        Functions.OnPlayerMovement(index, packet)

                    Case ClientOpcodes.Chat
                        Functions.OnChat(packet, index)

                    Case ClientOpcodes.GameMaster
                        Functions.OnGM(packet, index)

                    Case ClientOpcodes.Action
                        Functions.OnPlayerAction(packet, index)

                    Case ClientOpcodes.Angle_Update
                        Functions.OnAngleUpdate(packet, index)

                    Case ClientOpcodes.Teleport_Reply
                        Functions.OnTeleportRequest(index)

                    Case ClientOpcodes.Exit
                        Functions.OnLogout(packet, index)

                    Case ClientOpcodes.Emotion
                        Functions.OnEmotion(packet, index)

                    Case ClientOpcodes.HelperIcon
                        Functions.OnHelperIcon(packet, index)

                    Case ClientOpcodes.Hotkey_Update
                        Functions.OnHotkeyUpdate(packet, index)

                        '=======ATTACK======

                    Case ClientOpcodes.Target
                        Functions.OnSelectObject(packet, index)


                    Case ClientOpcodes.Attack
                        Functions.OnPlayerAttack(packet, index)
                        '=======NPC========

                    Case ClientOpcodes.Npc_Chat
                        Functions.OnNpcChatSelect(packet, index)

                    Case ClientOpcodes.Npc_Chat_Left
                        Functions.OnNpcChatLeft(packet, index)

                    Case ClientOpcodes.Npc_Teleport
                        Functions.OnNpcTeleport(packet, index)

                        '=======ITEMS======
                    Case ClientOpcodes.Alchemy
                        Functions.OnAlchemyRequest(packet, index)

                    Case ClientOpcodes.ItemMove
                        Functions.OnInventory(packet, index)

                    Case ClientOpcodes.ItemUse
                        Functions.OnUseItem(packet, index)

                    Case ClientOpcodes.Scroll_Cancel
                        Functions.OnReturnScroll_Cancel(index)

                        '========STR+INT UP=====

                    Case ClientOpcodes.Str_Up
                        Functions.UpStrength(index)

                    Case ClientOpcodes.Int_Up
                        Functions.UpIntelligence(index)

                        '======Mastery+Skills

                    Case ClientOpcodes.Mastery_Up
                        Functions.OnUpMastery(packet, index)

                    Case ClientOpcodes.Skill_Up
                        Functions.OnAddSkill(packet, index)

                        '=========EXCHANGE======
                    Case ClientOpcodes.Exchange_Invite
                        Functions.OnExchangeInvite(packet, index)

                    Case ClientOpcodes.Exchange_Invite_Accept
                        Functions.OnExchangeInviteReply(packet, index)

                    Case ClientOpcodes.Exchange_Confirm
                        Functions.OnExchangeConfirm(packet, index)

                    Case ClientOpcodes.Exchange_Approve
                        Functions.OnExchangeApprove(packet, index)

                    Case ClientOpcodes.Exchange_Abort
                        Functions.OnExchangeAbort(packet, index)

                    Case Else
                        Log.WriteSystemLog("opCode: " & opcode & " Packet : " & BitConverter.ToString(packet.ByteArray(length), 0, length))
                End Select

            Catch ex As Exception
                Log.WriteSystemLog("Parsing Error:  " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & index)
            End Try
        End Sub

    End Class
End Namespace

