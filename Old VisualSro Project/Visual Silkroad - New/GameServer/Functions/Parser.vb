Imports SRFramework

Namespace Functions
    Public Module Parser
        Public Sub Parse(ByVal packet As PacketReader, ByVal Index_ As Integer)
            Try
                Dim length As UInteger = packet.Word
                Dim opcode As UInteger = packet.Word
                Dim security As UInteger = packet.Word

                Server.ClientList.LastPingTime(Index_) = DateTime.Now

                Select Case opcode
                    Case ClientOpcodes.PING

                        '=============Login=============
                    Case ClientOpcodes.HANDSHAKE 'Client accepts
                        SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.HANDSHAKE

                    Case ClientOpcodes.LOGIN_WHO_AM_I 'GateWay
                        Functions.Gateway(packet, Index_)

                    Case ClientOpcodes.GAME_AUTH
                        Functions.CheckLogin(packet, Index_)

                    Case ClientOpcodes.GAME_CHARACTER
                        Functions.HandleCharPacket(packet, Index_)

                    Case ClientOpcodes.GAME_CHARACTER_NAME_EDIT
                        Functions.OnCharacterNamechangeRequest(packet, Index_)

                    Case ClientOpcodes.GAME_INGAME_REQ
                        Functions.CharLoading(packet, Index_)

                    Case ClientOpcodes.GAME_JOIN_WORLD_REQ
                        Functions.OnJoinWorldRequest(Index_)

                    Case ClientOpcodes.GAME_JOIN_WORLD_REQ2
                        Functions.OnJoinWorldRequest(Index_)
                End Select

                If SessionInfo(Index_) IsNot Nothing Then
                    If SessionInfo(Index_).SRConnectionSetup = cSessionInfo_GameServer.SRConnectionStatus.INGAME Then
                        'Prevents User to send Ingame packet for ex. when they are on charlist
                        Select Case opcode
                            Case ClientOpcodes.PING

                                '============Ingame===========
                            Case ClientOpcodes.GAME_CLIENT_STATUS
                                Functions.OnClientStatusUpdate(packet, Index_)

                            Case ClientOpcodes.GAME_MOVEMENT
                                Functions.OnPlayerMovement(Index_, packet)

                            Case ClientOpcodes.GAME_CHAT
                                Functions.OnChat(packet, Index_)

                            Case ClientOpcodes.GAME_CHAT_ITEM_LINK_ADD
                                Functions.OnAddItemLink(packet, Index_)

                            Case ClientOpcodes.GAME_CHAT_ITEM_LINK
                                Functions.OnItemLinkInfo(packet, Index_)

                            Case ClientOpcodes.GAME_GAMEMASTER
                                Functions.OnGM(packet, Index_)

                            Case ClientOpcodes.GAME_ACTION
                                Functions.OnPlayerAction(packet, Index_)

                            Case ClientOpcodes.GAME_ANGLE_UPDATE
                                Functions.OnAngleUpdate(packet, Index_)

                            Case ClientOpcodes.GAME_TELEPORT_REPLY
                                Functions.OnTeleportRequest(Index_)

                            Case ClientOpcodes.GAME_EXIT
                                Functions.OnLogout(packet, Index_)

                            Case ClientOpcodes.GAME_EMOTION
                                Functions.OnEmotion(packet, Index_)

                            Case ClientOpcodes.GAME_HELPER_ICON
                                Functions.OnHelperIcon(packet, Index_)

                            Case ClientOpcodes.GAME_HOTKEY_UPDATE
                                Functions.OnHotkeyUpdate(packet, Index_)

                            Case ClientOpcodes.GAME_SET_RETURN_POINT
                                Functions.OnSetReturnPoint(packet, Index_)

                            Case ClientOpcodes.GAME_BERSERK_ACTIVATE
                                Functions.OnUseBerserk(packet, Index_)

                                '=======ATTACK======

                            Case ClientOpcodes.GAME_TARGET
                                Functions.OnSelectObject(packet, Index_)

                            Case ClientOpcodes.GAME_ATTACK
                                Functions.OnPlayerAttack(packet, Index_)

                            Case ClientOpcodes.GAME_DEATH_RESPAWN
                                Functions.OnPlayerRespawn(packet, Index_)

                                '=======NPC========

                            Case ClientOpcodes.GAME_NPC_CHAT
                                Functions.OnNpcChatSelect(packet, Index_)

                            Case ClientOpcodes.GAME_NPC_CHAT_LEFT
                                Functions.OnNpcChatLeft(packet, Index_)

                            Case ClientOpcodes.GAME_NPC_TELEPORT
                                Functions.OnNpcTeleport(packet, Index_)

                                '=======ITEMS======
                            Case ClientOpcodes.GAME_ALCHEMY
                                Functions.OnAlchemyRequest(packet, Index_)

                            Case ClientOpcodes.GAME_ITEM_MOVE
                                Functions.OnInventory(packet, Index_)

                            Case ClientOpcodes.GAME_ITEM_USE
                                Functions.OnUseItem(packet, Index_)

                            Case ClientOpcodes.GAME_SCROLL_CANCEL
                                Functions.OnReturnScroll_Cancel(Index_)

                                '========STR+INT UP=====

                            Case ClientOpcodes.GAME_STR_UP
                                Functions.UpStrength(Index_)

                            Case ClientOpcodes.GAME_INT_UP
                                Functions.UpIntelligence(Index_)

                                '======Mastery+Skills

                            Case ClientOpcodes.GAME_MASTERY_UP
                                Functions.OnUpMastery(packet, Index_)

                            Case ClientOpcodes.GAME_SKILL_UP
                                Functions.OnAddSkill(packet, Index_)

                                '=========EXCHANGE======
                            Case ClientOpcodes.GAME_EXCHANGE_INVITE
                                Functions.OnExchangeInvite(packet, Index_)

                            Case ClientOpcodes.GAME_EXCHANGE_INVITE_ACCEPT
                                Functions.OnExchangeInviteReply(packet, Index_)

                            Case ClientOpcodes.GAME_EXCHANGE_CONFIRM
                                Functions.OnExchangeConfirm(packet, Index_)

                            Case ClientOpcodes.GAME_EXCHANGE_APPROVE
                                Functions.OnExchangeApprove(packet, Index_)

                            Case ClientOpcodes.GAME_EXCHANGE_ABORT
                                Functions.OnExchangeAbort(packet, Index_)

                                '========STALL==========
                            Case ClientOpcodes.GAME_STALL_OPEN
                                Functions.Stall_Open_Own(packet, Index_)

                            Case ClientOpcodes.GAME_STALL_SELECT
                                Functions.Stall_Open_Other(packet, Index_)

                            Case ClientOpcodes.GAME_STALL_DATA
                                Functions.Stall_Data(packet, Index_)

                            Case ClientOpcodes.GAME_STALL_BUY
                                Functions.Stall_Buy(packet, Index_)

                            Case ClientOpcodes.GAME_STALL_CLOSE_OWN
                                Functions.Stall_Close_Own(Index_)

                            Case ClientOpcodes.GAME_STALL_CLOSE_VISITOR
                                Functions.Stall_Close_Other(Index_)

                            Case Else
                                Log.WriteSystemLog(
                                    "opCode_2: " & Hex(opcode) & " Packet : " &
                                    BitConverter.ToString(packet.ByteArray(length), 0, length))
                        End Select
                    End If
                Else
                    'SessionInfo is nothing
                    'We just jump out, the server will cleaned/disconnected the player
                End If

            Catch ex As Exception
                Log.WriteSystemLog("Parsing Error:  " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index_)
            End Try
        End Sub

        Public Sub ParseGlobalManager(ByVal packet As PacketReader)
            Dim length As UInteger = packet.Word
            Dim opcode As UInteger = packet.Word
            Dim security As UInteger = packet.Word

            Select Case opcode
                Case ServerOpcodes.HANDSHAKE   'Client accepts
                    GlobalManager.OnHandshake(packet)

                Case ServerOpcodes.LOGIN_SERVER_INFO
                    GlobalManager.OnServerInfo(packet)

                Case InternalServerOpcodes.SERVER_INIT
                    GlobalManager.OnServerInit(packet)

                Case InternalServerOpcodes.SERVER_SHUTDOWN
                    GlobalManager.OnServerShutdown(packet)

                Case InternalServerOpcodes.GLOBAL_INFO
                    GlobalManager.OnGlobalInfo(packet)

                Case InternalServerOpcodes.AGENT_CHECK_USERAUTH
                    GlobalManager.OnGameserverUserAuthReply(packet)

                Case InternalServerOpcodes.AGENT_USERINFO
                    GlobalManagerCon.UserPacketService(packet)

                Case Else
                    Log.WriteSystemLog("gmc opCode: " & opcode) '& " Packet : " & packet.Byte)
            End Select
        End Sub
    End Module
End Namespace

