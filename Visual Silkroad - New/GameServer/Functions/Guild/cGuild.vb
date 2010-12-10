Namespace GameServer
    Public Class cGuild
        Public GuildID As UInteger
        Public GuildName As String
        Public GuildPoints As UInteger
        Public GuildLevel As Byte

        Public GuildNoticeTitle As String
        Public GuildNotice As String

        Public Member As New List(Of GuildMember_)


        Structure GuildMember_
            Public CharacterID As UInteger
            Public GuildID As UInteger
            Public DonantedGP As UInteger
            Public Rights As GuildRights_
        End Structure

        Structure GuildRights_
            Public Master As Boolean
            Public Invite As Boolean
            Public Kick As Boolean
            Public Notice As Boolean
            Public Union As Boolean
            Public Strorage As Boolean
        End Structure
    End Class
End Namespace