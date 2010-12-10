Namespace GameServer.Functions
    Module Guild

        Public Sub SendGuildInfo(ByVal Index_ As Integer, ByVal update As Boolean)
            Dim writer As New PacketWriter
            Dim guild As cGuild = DatabaseCore.GetGuildWithGuildID(PlayerData(Index_).GuildID)

            If update = False Then
                writer.Create(ServerOpcodes.Guild_Info)
                writer.DWord(guild.GuildID)
                writer.Word(guild.GuildName.Length)
                writer.String(guild.GuildName)
                writer.Byte(guild.GuildLevel)
                writer.DWord(guild.GuildPoints)
                writer.Word(guild.GuildNoticeTitle.Length)
                writer.String(guild.GuildNoticeTitle)
                writer.Word(guild.GuildNotice.Length)
                writer.String(guild.GuildNotice)
            End If
        End Sub





    End Module
End Namespace
