Namespace GameServer.Functions
    Module MonsterSpawn

        Public MobList As New List(Of cMonster)

        Public Function CreateMonsterSpawnPacket(ByVal mob_index As Integer) As Byte()
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.SingleSpawn)
            writer.DWord(MobList(mob_index).Pk2ID)
            writer.DWord(MobList(mob_index).UniqueID)
            writer.Byte(MobList(mob_index).Position.XSector)
            writer.Byte(MobList(mob_index).Position.YSector)
            writer.Float(MobList(mob_index).Position.X)
            writer.Float(MobList(mob_index).Position.Z)
            writer.Float(MobList(mob_index).Position.Y)
            writer.Word(0) 'angle
            writer.Byte(0) 'dest
            writer.Byte(1) 'walk run flag
            writer.Byte(0) 'dest
            writer.Word(0) 'angle
            writer.Byte(MobList(mob_index).Death) ' death flag

            writer.Float(20) 'walkspeed
            writer.Float(50) 'runspeed
            writer.Float(100) 'berserkerspeed
            Return writer.GetBytes
        End Function


    End Module
End Namespace
