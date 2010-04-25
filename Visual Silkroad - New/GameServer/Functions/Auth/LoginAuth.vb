Namespace GameServer.Functions


    Module LoginAuth

        Public Sub GateWay(ByVal index As Integer)
            Dim writer As New GameServer.PacketWriter
            Dim name As String = "AgentServer"
            writer.Create(ServerOpcodes.ServerInfo)
            writer.Word(name.Length)
            writer.HexString(name)
            writer.Byte(0)
            GameServer.Server.Send(writer.GetBytes, index)

        End Sub

        Public Sub SendPatchInfo(ByVal index_ As Integer)

            'Note: Patch Info for Rsro


            GameServer.Server.Send(New Byte() {5, 0, 13, 96, 0, 0, 1, 1, 0, 5, 32, _
                                                11, 0, 13, 96, 0, 0, 0, 1, 0, 1, 16, 10, 5, 0, 0, 0, 2, _
                                                5, 0, 13, 96, 0, 0, 1, 1, 0, 5, 96, _
                                                6, 0, 13, 96, 0, 0, 0, 3, 0, 2, 0, 2}, index_)


        End Sub

        Public Sub CheckLogin(ByVal index_ As Integer, ByVal packet As PacketReader)
            packet.DWord()
            Dim name As String = packet.String(packet.Word)
            Dim password As String = packet.String(packet.Word)

            Dim UserIndex As Integer = GetUserWithID(name)


            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.LoginAuthInfo)


            If Users(UserIndex).Name = name And Users(UserIndex).Pw = password Then
                writer.Byte(1)
                GameServer.Server.Send(writer.GetBytes, index_)
                GameServer.ClientList.OnCharListing(index_) = New cCharListing
                GameServer.ClientList.OnCharListing(index_).LoginInformation = New cCharListing.UserArray
                GameServer.ClientList.OnCharListing(index_).LoginInformation = Users(UserIndex)

            Else
                GameServer.Server.Dissconnect(index_)
            End If



        End Sub
    End Module
End Namespace
