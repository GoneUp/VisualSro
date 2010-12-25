Namespace GameServer.Functions
    Module Chat
        Public Sub OnChat(ByVal Packet As PacketReader, ByVal Index_ As Integer)


            Dim tag As Byte = Packet.Byte


            Select Case tag
                Case ChatModes.AllChat
                    OnPublicChat(Packet, Index_)

                Case ChatModes.PmIncome
                    OnWhisperIncome(Packet, Index_)

                Case ChatModes.GameMaster
                    OnGameMasterChat(Packet, Index_)

                Case ChatModes.Party
                Case ChatModes.Guild
                Case ChatModes.Notice
                    OnNoticeChat(Packet, Index_)

                Case ChatModes.Union
                Case ChatModes.Academy

            End Select
        End Sub

        Public Sub OnPublicChat(ByVal packet As PacketReader, ByVal Index_ As Integer)

            Dim counter As Byte = packet.Byte
            Dim messagelength As UInt16 = packet.Word
            Dim message As String = packet.UString(messagelength)

            If message.Contains("\n") = False Then 'Filter :P
                Dim writer As New PacketWriter 'Reply to sender
                writer.Create(ServerOpcodes.Chat_Accept)
                writer.Byte(1)
                writer.Byte(1)
                writer.Byte(counter)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.Chat)
                writer.Byte(1)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Word(messagelength)
                writer.UString(message)
                Server.SendToAllInRangeExpectMe(writer.GetBytes, Index_)

                If Log_Chat Then
                    Log.WriteGameLog(Index_, "Chat", "Public", "Message: " & message)
                End If
            End If
        End Sub

        Public Sub OnWhisperIncome(ByVal Packet As PacketReader, ByVal Index_ As Integer)

            Dim counter As Byte = Packet.Byte
            Dim senderlength As UInt16 = Packet.Word
            Dim sender As String = Packet.String(senderlength)
            Dim senderindex As Integer = -1

            Dim messagelength As UInt16 = Packet.Word
            Dim byte_message As Byte() = Packet.ByteArray(messagelength * 2)
            Dim message As String = System.Text.Encoding.Unicode.GetString(byte_message)

            For i = 0 To Server.OnlineClient - 1
                If PlayerData(i) IsNot Nothing Then
                    If PlayerData(i).CharacterName = sender Then
                        senderindex = i
                        Exit For
                    End If
                End If
            Next

            Dim writer As New PacketWriter

            If senderindex <> -1 Then
                writer.Create(ServerOpcodes.Chat_Accept)
                writer.Byte(1)
                writer.Byte(ChatModes.PmIncome)
                writer.Byte(counter)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.Chat)
                writer.Byte(ChatModes.PmIncome)
                writer.Word(PlayerData(Index_).CharacterName.Length)
                writer.String(PlayerData(Index_).CharacterName)

                writer.Word(messagelength)
                writer.UString(message)

                Server.Send(writer.GetBytes, senderindex)

                If Log_Chat Then
                    Log.WriteGameLog(Index_, "Chat", "Whisper", String.Format("Sender: {0} Message: {1}", sender, message))
                End If
            Else
                'Opposite not online
                writer.Create(ServerOpcodes.Chat_Accept)
                writer.Byte(2)
                writer.Byte(3) 'error byte
                writer.Byte(ChatModes.PmIncome)
                writer.Byte(counter)

                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub

        Public Sub SendPm(ByVal Rev_Index As Integer, ByVal Message As String, ByVal SenderName As String)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Chat)
            writer.Byte(ChatModes.PmIncome)
            writer.Word(SenderName.Length)
            writer.String(SenderName)

            writer.Word(Message.Length)
            writer.UString(Message)

            Server.Send(writer.GetBytes, Rev_Index)
        End Sub

        Public Sub OnGameMasterChat(ByVal Packet As PacketReader, ByVal Index_ As Integer)

            If PlayerData(Index_).GM = True Then
                Dim counter As Byte = Packet.Byte
                Dim messagelength As UInt16 = Packet.Word
                Dim bmessage As Byte() = Packet.ByteArray(messagelength * 2)
                Dim message As String = System.Text.Encoding.Unicode.GetString(bmessage)

                Dim writer As New PacketWriter 'Reply to sender
                writer.Create(ServerOpcodes.Chat_Accept)
                writer.Byte(1)
                writer.Byte(ChatModes.GameMaster)
                writer.Byte(counter)
                Server.Send(writer.GetBytes, Index_)

                writer.Create(ServerOpcodes.Chat)
                writer.Byte(ChatModes.GameMaster)
                writer.DWord(PlayerData(Index_).UniqueId)
                writer.Word(messagelength)
                writer.UString(message)
                Server.SendToAllInRangeExpectMe(writer.GetBytes, Index_)

                [Mod].CheckForCoustum(message, Index_)

                If Log_Chat Then
                    Log.WriteGameLog(Index_, "Chat", "GM", "Message: " & message)
                End If
            End If

        End Sub

        Public Sub OnNoticeChat(ByVal packet As PacketReader, ByVal Index_ As Integer)
            If PlayerData(Index_).GM = True Then
                Dim counter As Byte = packet.Byte
                Dim messagelength As UInt16 = packet.Word
                Dim bmessage As Byte() = packet.ByteArray(messagelength * 2)
                Dim message As String = System.Text.Encoding.Unicode.GetString(bmessage)

                SendNotice(message)

                If Log_Chat Then
                    Log.WriteGameLog(Index_, "Chat", "Notice", "Message: " & message)
                End If

            End If
        End Sub
        Public Sub SendNotice(ByVal message As String)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Chat)
            writer.Byte(ChatModes.Notice)
            writer.Word(message.Length)
            writer.UString(message)
            Server.SendToAllIngame(writer.GetBytes)
        End Sub

        Public Sub OnGlobalChat(ByVal Message As String, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Chat)
            writer.Byte(ChatModes.Globals)
            writer.Word(PlayerData(Index_).CharacterName.Length)
            writer.String(PlayerData(Index_).CharacterName)
            writer.Word(Message.Length)
            writer.UString(Message)
            Server.SendToAllIngame(writer.GetBytes)

            If Log_Chat Then
                Log.WriteGameLog(Index_, "Chat", "Global", "Message: " & Message)
            End If
        End Sub

        ''' <summary>
        ''' Only to Specific Player Index
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="index"></param>
        ''' <remarks></remarks>
        Public Sub SendNotice(ByVal message As String, ByVal index As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.Chat)
            writer.Byte(ChatModes.Notice)
            writer.Word(message.Length)
            writer.UString(message)
            Server.Send(writer.GetBytes, index)
        End Sub

    End Module

    Enum ChatModes
        AllChat = &H1
        PmIncome = &H2
        GameMaster = &H3
        Party = &H4
        Guild = &H5
        Globals = &H6
        Notice = &H7
        Union = &HB
        Academy = &H10
    End Enum
End Namespace