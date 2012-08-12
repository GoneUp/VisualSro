Namespace Functions
    Public Class cSkillSet
        Private m_SetID As UInt32
        Property SetID As UInt32
            Get
                Return m_SetID
            End Get
            Set(ByVal value As UInt32)
                m_SetID = value
            End Set
        End Property

        Private m_Name As String
        Property Name As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
            End Set
        End Property

        Private m_skills As New List(Of UInt32)
        Property Skills As List(Of UInt32)
            Get
                Return m_skills
            End Get
            Set(ByVal value As List(Of UInt32))
                m_skills = value
            End Set
        End Property
    End Class
End Namespace
