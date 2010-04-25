Namespace GameServer.Functions
    Module Character

        Public Sub HandleCharPacket(ByVal Index_ As Integer, ByVal pack As GameServer.PacketReader)

            Dim tag As Byte = pack.Byte

            Select Case tag
                Case 1 'CHar ersterllen
                Case 2 'Char List
                Case 3 'Char löschen
                Case 4 'Nick Check
                Case 5 'Löschen aufheben
            End Select
        End Sub

        Public Sub CharList(ByVal Index_ As Integer)




        End Sub
    End Module
End Namespace
