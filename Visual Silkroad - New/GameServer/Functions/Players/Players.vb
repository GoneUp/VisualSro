Namespace GameServer
    Module Players

        'Players
        Public PlayerData(5000) As [cChar]
        'items
        Public Inventorys(5000) As cInventory
        'Mobs
        'Skills

        Public Sub OnPlayerMovement(ByVal Index_ As Integer, ByVal packet As PacketReader)

            Dim tag As Byte = packet.Byte
            If tag = 1 Then
                '01 A8 60 61 02 FE FF 70 04
                Dim ToXSector As Byte = packet.Byte
                Dim ToYSector As Byte = packet.Byte
                Dim ToYPos As UInt16 = packet.Word
                Dim ToZPos As UInt16 = packet.Word
                Dim ToXPos As UInt16 = packet.Word

                PlayerData(Index_).XSector = ToXSector
                PlayerData(Index_).YSector = ToYSector
                PlayerData(Index_).X = ToXPos
                PlayerData(Index_).Z = ToZPos
                PlayerData(Index_).Y = ToYPos

            Else

            End If
        End Sub


    End Module
End Namespace
