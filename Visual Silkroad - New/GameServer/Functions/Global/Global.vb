Namespace GameServer.Functions
    Module [Global]
        'Players
        Public PlayerData(5000) As [cChar]
        'Items
        Public Inventorys(5000) As cInventory
        Public ItemList As New Dictionary(Of UInteger, cItemDrop)
        'Monster
        Public MobList As New Dictionary(Of UInteger, cMonster)
        'NPC
        Public NpcList As New List(Of cNPC)
        'Exchange
        Public ExchangeData As New Dictionary(Of UInteger, cExchange)
        'Stall
        Public Stalls As New List(Of cStall)


    End Module
End Namespace
