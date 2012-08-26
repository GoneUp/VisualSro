<Serializable()>
Public Class cUser
    Private m_AccountId As Integer = 0
    Public Property AccountId As String
        Get
            Return m_AccountId
        End Get
        Set(ByVal value As String)
            m_AccountId = value
        End Set
    End Property

    Private m_Name As String = ""
    Public Property Name As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property

    Private m_Pw As String = ""
    Public Property Pw As String
        Get
            Return m_Pw
        End Get
        Set(ByVal value As String)
            m_Pw = value
        End Set
    End Property

    Private m_FailedLogins As Byte = 0
    Public Property FailedLogins As Byte
        Get
            Return m_FailedLogins
        End Get
        Set(ByVal value As Byte)
            m_FailedLogins = value
        End Set
    End Property


    Private m_Banned As Boolean = False
    Public Property Banned As Boolean
        Get
            Return m_Banned
        End Get
        Set(ByVal value As Boolean)
            m_Banned = value
        End Set
    End Property

    Private m_BannTime As DateTime = Date.MinValue
    Public Property BannTime As DateTime
        Get
            Return m_BannTime
        End Get
        Set(ByVal value As DateTime)
            m_BannTime = value
        End Set
    End Property

    Private m_BannReason As String = ""
    Public Property BannReason As String
        Get
            Return m_BannReason
        End Get
        Set(ByVal value As String)
            m_BannReason = value
        End Set
    End Property


    Private m_loggedIn As Boolean = False
    Public Property LoggedIn As Boolean
        Get
            Return m_loggedIn
        End Get
        Set(ByVal value As Boolean)
            m_loggedIn = value
        End Set
    End Property

    Private m_permission As UserType = UserType.Normal  '0x00 = normal user, 0x01 = prefered access to the server (premium), 0x02 = gm, 0x03 = admin
    Public Property Permission As UserType
        Get
            Return m_permission
        End Get
        Set(ByVal value As UserType)
            m_permission = value
        End Set
    End Property


    Private m_silk As UInteger = 0
    Public Property Silk As UInt32
        Get
            Return m_silk
        End Get
        Set(ByVal value As UInt32)
            m_silk = value
        End Set
    End Property

    Private m_silk_Bonus As UInteger = 0
    Public Property Silk_Bonus As UInt32
        Get
            Return m_silk_Bonus
        End Get
        Set(ByVal value As UInt32)
            m_silk_Bonus = value
        End Set
    End Property

    Private m_silk_Points As UInteger = 0
    Public Property Silk_Points As UInt32
        Get
            Return m_silk_Points
        End Get
        Set(ByVal value As UInt32)
            m_silk_Points = value
        End Set
    End Property

    Private m_storageSlots As Byte = 0
    Public Property StorageSlots As Byte
        Get
            Return m_storageSlots
        End Get
        Set(ByVal value As Byte)
            m_storageSlots = value
        End Set
    End Property

    Enum UserType
        Normal = 0
        Prefered = 1
        GM = 2
        Admin_Restricted = 3
        Admin_Full = 4
    End Enum
End Class

