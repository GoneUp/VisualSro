Namespace Functions
    Public Class cGameObject
        Public Pk2ID As UInt32
        Public UniqueID As UInt32
        Public PosTracker As cPositionTracker
        Public ChannelId As UInt32
        Public AvoidChannels As Boolean = False

        Public Property Position() As Position
            Get
                If PosTracker IsNot Nothing Then
                    Return PosTracker.GetCurPos
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Position)
                If PosTracker IsNot Nothing Then
                    PosTracker.LastPos = value
                Else
                    'A basic creation for static things like npc, that use position only
                    PosTracker = New cPositionTracker(value, 0, 0, 0)
                End If
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(newPk2ID As UInt32, newUniqueID As UInt32)
            Pk2ID = newPk2ID
            UniqueID = newUniqueID
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("GameObject, Pk2Id: {0}, UniqueId: {1}", Pk2ID, UniqueID)
        End Function
    End Class
End Namespace