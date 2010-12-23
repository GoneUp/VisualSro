Module Parser
    Sub Parse(ByVal Packet As PacketReader, ByVal Index_ As Integer)

        Dim length As UInteger = Packet.Word
        Dim opcode As UInteger = Packet.Word
        Dim security As UInteger = Packet.Word

        Select Case opcode
            Case ServerOpcodes.Handshake
                If GameServer(Index_) = False Then
                    SendLogin(Index_)

                    PingTimer(Index_).Interval = 5000
                    PingTimer(Index_).Start()
                Else
                    SendAuth(Index_)
                End If


            Case &HA102
                ConnectToGameServer(Index_, Packet)

            Case &HA103
                HandleAuth(Index_, Packet)

            Case ServerOpcodes.Character
                CharList(Index_, Packet)

            Case ServerOpcodes.CharacterInfo
                OnCharInfo(Index_, Packet)

            Case ServerOpcodes.CharacterID
                OnJoinWorld(Index_, Packet)

        End Select



    End Sub

    Sub SendLogin(ByVal Index_ As Integer)
        Dim name As String = "stress" & Index_
        Dim pass As String = "stress" & Index_

        Dim writer As New PacketWriter
        writer.Create(&H6102)
        writer.Byte(0)
        writer.Word(name.Length)
        writer.String(name)
        writer.Word(pass.Length)
        writer.String(pass)
        writer.Word(3)
        Send(writer.GetBytes, Index_)
    End Sub


    Public Sub ConnectToGameServer(ByVal index_ As Integer, ByVal packet As PacketReader)
        Dim succ = packet.Byte()
        If succ = 1 Then
            Dim mykey As UInt32 = packet.DWord
            Dim ip As String = packet.String(packet.Word)
            Dim port As Integer = packet.Word

            GameServer(index_) = True
            Key(index_) = mykey

            PingTimer(index_).Stop()
            'Rev(index_).Abort()

            Threading.Thread.Sleep(500)
            s(index_).Disconnect(False)
            s(index_) = New Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.SocketType.Stream, Net.Sockets.ProtocolType.Tcp)

            Threading.Thread.Sleep(500)
            s(index_).Connect(New Net.IPEndPoint(Net.IPAddress.Parse(ip), port))


            PingTimer(index_).Start()
            Rev(index_) = New Threading.Thread(AddressOf ReceiveData)
            Rev(index_).Start(index_)
        Else
            Threading.Thread.Sleep(20000)
            SendLogin(index_)
        End If
    End Sub

    Public Sub SendAuth(ByVal index_ As Integer)
        Dim name As String = "stress" & index_
        Dim pass As String = "stress" & index_

        Dim writer As New PacketWriter
        writer.Create(ClientOpcodes.Login)
        writer.DWord(Key(index_))
        writer.Word(name.Length)
        writer.String(name)
        writer.Word(pass.Length)
        writer.String(pass)
        Send(writer.GetBytes, index_)
    End Sub

    Sub HandleAuth(ByVal index_ As Integer, ByVal packet As PacketReader)
        Dim succ As Byte = packet.Byte

        If succ = 1 Then
            SendListRequest(index_)

        Else
            Console.WriteLine(23)
        End If

    End Sub

    Public Sub SendListRequest(ByVal index_ As Integer)
        Dim writer As New PacketWriter
        writer.Create(ClientOpcodes.Character)
        writer.Byte(2)
        Send(writer.GetBytes, index_)
    End Sub

    Sub CharList(ByVal index_ As Integer, ByVal packet As PacketReader)
        Dim mode As Byte = packet.Byte


        Select Case mode
            Case 1
                Dim count As Byte = packet.Byte
                If count = 1 Then
                    SendListRequest(index_)
                End If


            Case 2
                packet.Byte()
                Dim count As Byte = packet.Byte

                If count = 0 Then
                    'Crate Char
                    CreateChar(index_)
                Else
                    Dim model As UInt32 = packet.DWord
                    Dim namelen As UShort = packet.Word
                    Dim name As String = packet.String(namelen)
                    SelectChar(name, index_)
                End If

        End Select


    End Sub

    Sub CreateChar(ByVal Index_ As Integer)
        Dim name As String = "stress" & Index_
        Dim writer As New PacketWriter
        writer.Create(ClientOpcodes.Character)
        writer.Byte(1)
        writer.Word(name.Length)
        writer.String(name)
        writer.DWord(1907)
        writer.Byte(32)
        writer.DWord(3637)
        writer.DWord(3638)
        writer.DWord(3639)
        writer.DWord(3632)
        Send(writer.GetBytes, Index_)
    End Sub

    Public Sub SelectChar(ByVal Name As String, ByVal index_ As Integer)
        Dim writer As New PacketWriter
        writer.Create(ClientOpcodes.IngameReq)
        writer.Word(Name.Length)
        writer.String(Name)
        Send(writer.GetBytes, index_)
    End Sub

    Sub OnCharInfo(ByVal index_ As Integer, ByVal packet As PacketReader)
        packet.Skip(432)

        'Dim b() = packet.ByteArray(500)

        Pos(index_).XSector = packet.Byte
        Pos(index_).YSector = packet.Byte
        Pos(index_).X = packet.Float
        Pos(index_).Z = packet.Float
        Pos(index_).Y = packet.Float
    End Sub
    Public Sub OnJoinWorld(ByVal index_ As Integer, ByVal packet As PacketReader)
        Dim UniqueID As UInt32 = packet.DWord

        Dim writer As New PacketWriter
        writer.Create(ClientOpcodes.JoinWorldReq)
        Send(writer.GetBytes, index_)

        PlayerMoveTimer(index_).Interval = 5000
        PlayerMoveTimer(index_).Start()

        Console.WriteLine("Ingame: " & index_)
    End Sub

    Public Sub SendPing(ByVal Index_ As Integer)
        If s(Index_).Connected Then
            Dim writer As New PacketWriter
            writer.Create(ClientOpcodes.Ping)
            Send(writer.GetBytes, Index_)
        End If
    End Sub


    Public Sub OnMoveUser(ByVal ToPos As Position, ByVal Index_ As Integer)
        Dim writer As New PacketWriter
        writer.Create(ClientOpcodes.Movement)
        writer.Byte(1)
        writer.Byte(ToPos.XSector)
        writer.Byte(ToPos.YSector)
        writer.Byte(BitConverter.GetBytes(CShort(ToPos.X)))
        writer.Byte(BitConverter.GetBytes(CShort(ToPos.Z)))
        writer.Byte(BitConverter.GetBytes(CShort(ToPos.Y)))
        Send(writer.GetBytes, Index_)
    End Sub
End Module
