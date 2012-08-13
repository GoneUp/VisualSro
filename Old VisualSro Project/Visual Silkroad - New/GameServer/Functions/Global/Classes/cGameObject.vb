Namespace Functions
    Public Class cGameObject
        Public Pk2ID As UInt32
        Public UniqueID As UInt32
        Public ChannelId As UInt32
        Public AvoidChannels As Boolean = False

        Public Overrides Function ToString() As String
            Return String.Format("GameObject, Pk2Id: {0}, UniqueId: {1}", Pk2ID, UniqueID)
        End Function
    End Class
End Namespace