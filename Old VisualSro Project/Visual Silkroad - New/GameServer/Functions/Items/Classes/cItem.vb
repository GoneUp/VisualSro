Namespace Functions
    Public Class cItem
        Private m_ID As UInt64
        Property ID As UInt64
            Get
                Return m_ID
            End Get
            Set(ByVal value As UInt64)
                m_ID = value
            End Set
        End Property

        Private m_ObjectID As UInt32
        Property ObjectID As UInt32
            Get
                Return m_ObjectID
            End Get
            Set(ByVal value As UInt32)
                m_ObjectID = value
            End Set
        End Property

        Private m_Plus As Byte
        Property Plus As Byte
            Get
                Return m_Plus
            End Get
            Set(ByVal value As Byte)
                m_Plus = value
            End Set
        End Property

        Private m_Variance As UInt64
        Property Variance As UInt64
            Get
                Return m_Variance
            End Get
            Set(ByVal value As UInt64)
                m_Variance = value
            End Set
        End Property


        'ITEM_CH/EU = Durability, ITEM_ETC = Amount, Pet=COS ID of COS Table
        Private m_Data As UInt32
        Property Data As UInt32
            Get
                Return m_Data
            End Get
            Set(ByVal value As UInt32)
                m_Data = value
            End Set
        End Property

        Private m_CreatorName As String
        Property CreatorName As String
            Get
                Return m_CreatorName
            End Get
            Set(ByVal value As String)
                m_CreatorName = value
            End Set
        End Property

        Private m_Blues As New List(Of cBluestat)
        Property Blues As List(Of cBluestat)
            Get
                Return m_Blues
            End Get
            Set(ByVal value As List(Of cBluestat))
                m_Blues = value
            End Set
        End Property
    End Class

    Public Class cBluestat
        Private m_BlueTyp As UInt32
        Property Type As UInt32
            Get
                Return m_BlueTyp
            End Get
            Set(ByVal value As UInt32)
                m_BlueTyp = value
            End Set
        End Property
        Private m_Blue_Amout As UInt32
        Property Amout As UInt32
            Get
                Return m_BlueTyp
            End Get
            Set(ByVal value As UInt32)
                m_BlueTyp = value
            End Set
        End Property
    End Class
End Namespace
