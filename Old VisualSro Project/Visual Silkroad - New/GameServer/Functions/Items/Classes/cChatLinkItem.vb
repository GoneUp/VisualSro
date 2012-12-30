Namespace Functions
    Public Class cChatLinkItem
        Private m_linkID As UInt64 = 0
        Private m_itemID As UInt64 = 0
        Private m_realName As String = ""
        Private m_creatorName As String
        Private m_creationDate As Date

        Public Property LinkID As UInt64
            Get
                Return m_linkID
            End Get
            Set(value As UInt64)
                m_linkID = value
            End Set
        End Property

        Public Property ItemID As UInt64
            Get
                Return m_itemID
            End Get
            Set(value As UInt64)
                m_itemID = value
            End Set
        End Property

        Public Property RealName As String
            Get
                Return m_realName
            End Get
            Set(value As String)
                m_realName = value
            End Set
        End Property

        Public Property CreatorName As String
            Get
                Return m_creatorName
            End Get
            Set(value As String)
                m_creatorName = value
            End Set
        End Property

        Public Property CreationDate As DateTime
            Get
                Return m_creationDate
            End Get
            Set(value As DateTime)
                m_creationDate = value
            End Set
        End Property

        Sub New()
            m_creationDate = Date.Now
        End Sub

        Sub New(ByVal linkID As ULong, ByVal itemID As ULong, ByVal realName As String, ByVal creatorName As String)
            m_linkID = linkID
            m_itemID = itemID
            m_realName = realName
            m_creatorName = creatorName
            m_creationDate = Date.Now
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("ChatLinkItem, LinkID:{0}, ItemID:{1}, CreatorName:{2}", m_linkID, m_itemID, m_creatorName)
        End Function
    End Class
End Namespace