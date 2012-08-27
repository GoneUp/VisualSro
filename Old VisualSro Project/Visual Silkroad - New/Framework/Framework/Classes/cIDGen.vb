Option Strict On

Public Class cIDGen
    Private m_counter As Object
    Private m_counterStep As Object
    Private m_type As System.Type

    Sub New(ByVal initalvalue As Object, ByVal counterStep As Object)
        m_counter = initalvalue
        m_type = initalvalue.GetType
        m_counterStep = counterStep
    End Sub

    Public Function GetID() As Object
        Dim minValue As Object
        Dim maxValue As Object

        If m_counter.GetType = GetType(UInt32) Then
            If Convert.ToUInt32(m_counter) < UInt32.MaxValue Then
                m_counter = (Convert.ToUInt32(m_counter) + Convert.ToUInt32(m_counterStep))
                Return Convert.ToUInt32(m_counter)
            ElseIf Convert.ToUInt32(m_counter) = UInt32.MaxValue Then
                Return Nothing
            End If

        ElseIf m_counter.GetType = GetType(UInt64) Then
            If Convert.ToUInt64(m_counter) < UInt64.MaxValue Then
                m_counter = (Convert.ToUInt32(m_counter) + Convert.ToUInt32(m_counterStep))
                Return Convert.ToUInt32(m_counter)
            ElseIf Convert.ToUInt64(m_counter) = UInt64.MaxValue Then
                Return Nothing
            End If
        End If

        Return Nothing
    End Function
End Class
