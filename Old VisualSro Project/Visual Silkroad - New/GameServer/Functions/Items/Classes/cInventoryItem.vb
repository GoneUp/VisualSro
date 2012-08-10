Namespace Functions
    Public Class cInventoryItem
        Private m_OwnerID As UInt32 = 0
        Property OwnerID As UInt32
            Get
                Return m_OwnerID
            End Get
            Set(ByVal value As UInt32)
                m_OwnerID = value
            End Set
        End Property

        Private m_Slot As Byte = 0
        Property Slot As Byte
            Get
                Return m_Slot
            End Get
            Set(ByVal value As Byte)
                m_Slot = value
            End Set
        End Property

        Private m_ItemID As UInt64 = 0
        Property ItemID As UInt64
            Get
                Return m_ItemID
            End Get
            Set(ByVal value As UInt64)
                m_ItemID = value
            End Set
        End Property

        Private m_Locked As Boolean = False
        Public Property Locked As Boolean
            Get
                Return m_Locked
            End Get
            Set(ByVal value As Boolean)
                m_Locked = value
            End Set
        End Property
    End Class
End Namespace
