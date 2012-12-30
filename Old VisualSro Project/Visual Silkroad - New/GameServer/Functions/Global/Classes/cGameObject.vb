Namespace Functions
    Public Class cGameObject
        Private m_pk2ID As UInt32
        Private m_uniqueID As UInt32
        Protected m_posTracker As cPositionTracker
        Private m_channelId As UInt32
        Private m_avoidChannels As Boolean = False

        Public Property Pk2ID As UInt32
            Get
                Return m_pk2ID
            End Get
            Set(value As UInt32)
                m_pk2ID = value
            End Set
        End Property

        Public Property UniqueID As UInt32
            Get
                Return m_uniqueID
            End Get
            Set(value As UInt32)
                m_uniqueID = value
            End Set
        End Property

        Public Property PosTracker As cPositionTracker
            Get
                Return m_posTracker
            End Get
            Set(value As cPositionTracker)
                m_posTracker = value
            End Set
        End Property

        Public Property ChannelID As UInt32
            Get
                Return m_channelId
            End Get
            Set(value As UInt32)
                m_channelId = value
            End Set
        End Property

        Public Property AvoidChannels As Boolean
            Get
                Return m_avoidChannels
            End Get
            Set(value As Boolean)
                m_avoidChannels = value
            End Set
        End Property
        
        Public Property Position() As Position
            Get
                If m_posTracker IsNot Nothing Then
                    Return m_posTracker.GetCurPos
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Position)
                If m_posTracker IsNot Nothing Then
                    m_posTracker.LastPos = value
                Else
                    'A basic creation for static things like npc, that use position only
                    m_posTracker = New cPositionTracker(value, 0, 0, 0)
                End If
            End Set
        End Property

#Region "Functions"
        Public Sub New()

        End Sub

        Public Sub New(newPk2ID As UInt32, newUniqueID As UInt32)
            m_pk2ID = newPk2ID
            m_uniqueID = newUniqueID
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("GameObject, Pk2Id: {0}, UniqueId: {1}", m_pk2ID, m_uniqueID)
        End Function
#End Region
    End Class
End Namespace