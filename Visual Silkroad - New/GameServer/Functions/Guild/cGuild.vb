Namespace GameServer.Functions
    Public Class cGuild
        Public GuildID As UInteger
        Public GuildName As String
        Public GuildPoints As UInteger
        Public GuildLevel As Byte

        Public GuildNoticeTitle As String
        Public GuildNotice


        Public Member As New List(Of GuildMember_)


        Structure GuildMember_
            Public CharacterID As UInteger
            Public DonantedGP As UInteger
            Public Rights As 
        End Structure

        Structure GuildRights_
            Public Master As Boolean
            Public Kick As Boolean
        End Structure
    End Class
End Namespace