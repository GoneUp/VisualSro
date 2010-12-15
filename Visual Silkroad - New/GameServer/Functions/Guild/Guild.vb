Namespace GameServer.Functions
    Module Guild

        Public Sub SendGuildInfo(ByVal Index_ As Integer, ByVal update As Boolean)
            Dim writer As New PacketWriter
            Dim guild As cGuild = DatabaseCore.GetGuildWithGuildID(PlayerData(Index_).GuildID)

            If update = False Then
                writer.Create(ServerOpcodes.Guild_Info)
                writer.DWord(guild.GuildID)
                writer.Word(guild.Name.Length)
                writer.String(guild.Name)
                writer.Byte(guild.Level)
                writer.DWord(guild.Points)

                writer.Word(guild.NoticeTitle.Length)
                writer.String(guild.NoticeTitle)
                writer.Word(guild.Notice.Length)
                writer.String(guild.Notice)
                writer.DWord(0)
                writer.Byte(0)

                writer.Byte(guild.Member.Count)
                For i = 0 To guild.Member.Count - 1
                    Dim char_ As [cChar] = DatabaseCore.GetCharWithCharID(guild.Member(i).CharacterID)

                    writer.DWord(char_.CharacterId)
                    writer.Word(char_.CharacterName.Length)
                    writer.String(char_.CharacterName)

                    'Rights
                    If guild.Member(i).Rights.Master = True Then
                        writer.Byte(0)
                        writer.Byte(char_.Level)
                        writer.DWord(guild.Member(i).DonantedGP)
                        writer.DWord(UInteger.MaxValue)
                    Else
                        writer.Byte(&HA)
                        writer.Byte(char_.Level)
                        writer.DWord(guild.Member(i).DonantedGP)
                        writer.DWord(1)
                    End If

                    writer.DWord(0)
                    writer.DWord(0)
                    writer.DWord(0)

                    writer.Word(guild.Member(i).GrantName.Length)
                    writer.String(guild.Member(i).GrantName)
                    writer.DWord(char_.Model)
                    writer.Word(2)
                Next


                Server.Send(writer.GetBytes, Index_)
            End If
        End Sub

        Public Sub LinkPlayerToGuild(ByVal Index_ As Integer)


        End Sub




    End Module
End Namespace
