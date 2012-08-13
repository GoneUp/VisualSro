Namespace Functions
    Public Class cGuild
        Public GuildID As UInteger
        Public Name As String
        Public Points As UInteger
        Public Level As Byte

        Public NoticeTitle As String
        Public Notice As String

        Public Member As New List(Of GuildMember_)

        Structure GuildMember_
            Public CharacterID As UInteger
            Public GuildID As Long
            Public DonantedGP As UInteger
            Public GrantName As String
            Public Rights As GuildRights_
        End Structure

        Structure GuildRights_
            Public Master As Boolean
            Public Invite As Boolean
            Public Kick As Boolean
            Public Notice As Boolean
            Public Union As Boolean
            Public Storage As Boolean
        End Structure
    End Class
End Namespace