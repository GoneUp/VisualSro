Namespace Functions
    Module GlobalGame
        'CharList
        Public CharListing(1) As cCharListing
        'Players
        Public PlayerData(1) As cCharacter
        'Items
        Public Inventorys(1) As cInventory
        Public ItemList As New Dictionary(Of UInteger, cItemDrop)
        'Monster
        Public MobList As New Dictionary(Of UInteger, cMonster)
        'NPC
        Public NpcList As New Dictionary(Of UInteger, cNPC)
        'Exchange
        Public ExchangeData As New Dictionary(Of UInteger, cExchange)
        'Stall
        Public Stalls As New List(Of Stall)

        Public Function GlobalInit(ByVal slots As UInt32) As Boolean
            Try
                ReDim PlayerData(slots), Inventorys(slots), CharListing(slots)

                ItemList.Clear()
                MobList.Clear()
                NpcList.Clear()
                ExchangeData.Clear()
                Stalls.Clear()
            Catch ex As Exception
                Log.WriteSystemLog("GlobalInit failed! EX:" & ex.Message & " Stacktrace: " & ex.StackTrace)
                Return False
            End Try

            Return True
        End Function
    End Module
End Namespace
