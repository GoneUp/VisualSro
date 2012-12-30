Namespace Functions
    Public Class cChatLinkItem
        Private m_linkID As UInt64 = 0
        Private m_itemID As UInt64 = 0
        Private m_realName As String = ""
        Private m_creatorName As String

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

        Sub New()

        End Sub

        Sub New(ByVal linkID As ULong, ByVal itemID As ULong, ByVal realName As String, ByVal creatorName As String)
            m_linkID = linkID
            m_itemID = itemID
            m_realName = realName
            m_creatorName = creatorName
        End Sub
    End Class
End Namespace