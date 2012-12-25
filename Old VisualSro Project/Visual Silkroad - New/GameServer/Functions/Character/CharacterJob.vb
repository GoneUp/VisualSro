Imports SRFramework

Namespace Functions
    Module CharacterJob
        Public Sub OnJobSelectionPopup(packet As PacketReader, Index_ As Integer)
            Dim writer As New PacketWriter(ServerOpcodes.GAME_CHARACTER)
            writer.Byte(9)
            writer.Byte(1) 'Preselection Selection , 0=hunter, 1= thief?  
            writer.Byte(50) 'hunter percent
            writer.Byte(25) 'theif percent
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Sub OnJobSelection(packet As PacketReader, Index_ As Integer)
            Dim nick As String = packet.String(packet.Word)
            Dim selection As JobTypes = packet.Byte


            For i = 0 To CharListing(Index_).Chars.Count - 1
                If CharListing(Index_).Chars(i) Is Nothing Then
                    Log.WriteSystemLog("Charlist, Char is nothing, AccID: " & CharListing(Index_).LoginInformation.Name)
                    Server.Disconnect(Index_)
                    Exit Sub
                End If

                With CharListing(Index_).Chars(i)
                    If .CharacterName = nick Then
                       
                    End If
                End With
            Next
        End Sub
    End Module
End Namespace
