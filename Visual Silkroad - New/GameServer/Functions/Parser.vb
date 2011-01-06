Imports Microsoft.VisualBasic
	Imports System
	Imports System.Runtime.InteropServices
Namespace GameServer

    Public Class Parser


        Public Shared Sub Parse(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Try
                Dim length As UInteger = packet.Word
                Dim opcode As UInteger = packet.Word
                Dim security As UInteger = packet.Word

                Dim lastpingtime As DateTime = ClientList.LastPingTime(Index_)

                ClientList.LastPingTime(Index_) = DateTime.Now


                Select Case opcode

                    Case ClientOpcodes.Ping

                        '=============Login=============
                    Case ClientOpcodes.Handshake  'Client accepts

                    Case ClientOpcodes.InfoReq  'GateWay
                        Functions.GateWay(Index_)

                    Case ClientOpcodes.Login
                        Functions.CheckLogin(Index_, packet)

                    Case ClientOpcodes.Character
                        Functions.HandleCharPacket(Index_, packet)

                    Case ClientOpcodes.IngameReq
                        Functions.CharLoading(Index_, packet)

                    Case ClientOpcodes.JoinWorldReq
                        Functions.OnJoinWorldRequest(Index_)

                    Case ClientOpcodes.JoinWorldReq2
                        Functions.OnJoinWorldRequest(Index_)

                    Case ClientOpcodes.ClientStatus
                        Functions.OnClientStatusUpdate(packet, Index_)

                        '============Ingame===========

                    Case ClientOpcodes.Movement
                        Functions.OnPlayerMovement(Index_, packet)

                    Case ClientOpcodes.Chat
                        Functions.OnChat(packet, Index_)

                    Case ClientOpcodes.GameMaster
                        Functions.OnGM(packet, Index_)

                    Case ClientOpcodes.Action
                        Functions.OnPlayerAction(packet, Index_)

                    Case ClientOpcodes.Angle_Update
                        Functions.OnAngleUpdate(packet, Index_)

                    Case ClientOpcodes.Teleport_Reply
                        Functions.OnTeleportRequest(Index_)

                    Case ClientOpcodes.Exit
                        Functions.OnLogout(packet, Index_)

                    Case ClientOpcodes.Emotion
                        Functions.OnEmotion(packet, Index_)

                    Case ClientOpcodes.HelperIcon
                        Functions.OnHelperIcon(packet, Index_)

                    Case ClientOpcodes.Hotkey_Update
                        Functions.OnHotkeyUpdate(packet, Index_)

                        '=======ATTACK======

                    Case ClientOpcodes.Target
                        Functions.OnSelectObject(packet, Index_)


                    Case ClientOpcodes.Attack
                        Functions.OnPlayerAttack(packet, Index_)
                        '=======NPC========

                    Case ClientOpcodes.Npc_Chat
                        Functions.OnNpcChatSelect(packet, Index_)

                    Case ClientOpcodes.Npc_Chat_Left
                        Functions.OnNpcChatLeft(packet, Index_)

                    Case ClientOpcodes.Npc_Teleport
                        Functions.OnNpcTeleport(packet, Index_)

                        '=======ITEMS======
                    Case ClientOpcodes.Alchemy
                        Functions.OnAlchemyRequest(packet, Index_)

                    Case ClientOpcodes.ItemMove
                        Functions.OnInventory(packet, Index_)

                    Case ClientOpcodes.ItemUse
                        Functions.OnUseItem(packet, Index_)

                    Case ClientOpcodes.Scroll_Cancel
                        Functions.OnReturnScroll_Cancel(Index_)

                        '========STR+INT UP=====

                    Case ClientOpcodes.Str_Up
                        Functions.UpStrength(Index_)

                    Case ClientOpcodes.Int_Up
                        Functions.UpIntelligence(Index_)

                        '======Mastery+Skills

                    Case ClientOpcodes.Mastery_Up
                        Functions.OnUpMastery(packet, Index_)

                    Case ClientOpcodes.Skill_Up
                        Functions.OnAddSkill(packet, Index_)

                        '=========EXCHANGE======
                    Case ClientOpcodes.Exchange_Invite
                        Functions.OnExchangeInvite(packet, Index_)

                    Case ClientOpcodes.Exchange_Invite_Accept
                        Functions.OnExchangeInviteReply(packet, Index_)

                    Case ClientOpcodes.Exchange_Confirm
                        Functions.OnExchangeConfirm(packet, Index_)

                    Case ClientOpcodes.Exchange_Approve
                        Functions.OnExchangeApprove(packet, Index_)

                    Case ClientOpcodes.Exchange_Abort
                        Functions.OnExchangeAbort(packet, Index_)

                        'Stall
                    Case ClientOpcodes.Stall_Open
                        Functions.Stall_Open_Own(packet, Index_)

                    Case ClientOpcodes.Stall_Select
                        Functions.Stall_Open_Other(packet, Index_)

                    Case ClientOpcodes.Stall_Data
                        Functions.Stall_Data(packet, Index_)

                    Case ClientOpcodes.Stall_Close_Own
                        Functions.Stall_Close_Own(Index_)

                    Case ClientOpcodes.Stall_Close_Visitor
                        Functions.Stall_Close_Other(Index_)

                    Case Else
                        Log.WriteSystemLog("opCode: " & opcode & " Packet : " & BitConverter.ToString(packet.ByteArray(length), 0, length))
                End Select

            Catch ex As Exception
                Log.WriteSystemLog("Parsing Error:  " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index_)
            End Try
        End Sub

    End Class
End Namespace

