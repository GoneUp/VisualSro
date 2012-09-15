Public Class cByteCounter
#Region "Theory"
    'Functions: Total Byte Count, Bytes/s, packets total, P/s
    'Up- and Downlink
    'Correct kb/mb/gb
    'Performance! No huge Lists...

    'Possible Ways: Add all Packets to a List (Fields: Direction, Bytes, Time)
    'Dictionary(Of DateTime, cByteCounterPacket)
    'Totalbytes, TotalPackets Zähler
    'Bei Aufruf von GetBytesS/GetPacketsS Dictionary leeren
    'Vorteil: Jedesmal rechnen, Nachteil: evtl. große Datenmengen

    'Other Way: Timer alle 1000ms, b/s p/s berechnen, Dictinary leeren
    'Vorteil: 1x rechnen, Nachteil: Kommt aus dem Takt?
    'ACCEPTED, wegen PeakTrafffic und möglicher Memory Leaks, Timer IMMER auf näcste Sekunde einstellen

    'Fucntions
    'AddPacket
    'GetTotalbytes
    'GetBytesS
    'GetPeakBytessS
    'GetTotalPackets
    'GetPacketsS
    'GetPeakPacketsS
#End Region

#Region "Fields"
    Private Class cByteCounterPacket
        Private m_Source As PacketSource
        Public Property Source As PacketSource
            Get
                Return m_Source
            End Get
            Set(ByVal value As PacketSource)
                m_Source = value
            End Set
        End Property

        Private m_Bytes As UInt16 = 0
        Public Property Bytes As UInt16
            Get
                Return m_Bytes
            End Get
            Set(ByVal value As UInt16)
                m_Bytes = value
            End Set
        End Property

        Private m_Timestamp As New Date
        Public Property Timestamp As Date
            Get
                Return m_Timestamp
            End Get
            Set(ByVal value As Date)
                m_Timestamp = value
            End Set
        End Property

    End Class

    Private m_PacketList As Dictionary(Of UInt64, cByteCounterPacket)

    Private m_TotalBytes As UInt64 = 0
    Public ReadOnly Property TotalBytes As UInt64
        Get
            Return m_TotalBytes
        End Get
    End Property
    Private m_TotalPackets As UInt64 = 0
    Public ReadOnly Property TotalPackets As UInt64
        Get
            Return m_TotalPackets
        End Get
    End Property

    Private m_sTimer As Timers.Timer
    Private m_BytesPerS As Single = 0
    Public ReadOnly Property BytesPerS As Single
        Get
            Return m_BytesPerS
        End Get
    End Property
    Private m_PacketsPerS As Single = 0
    Public ReadOnly Property PacketsPerS As Single
        Get
            Return m_PacketsPerS
        End Get
    End Property

    '=================Session Id
    Private m_IdLock As New Object
    Private m_ListLock As New Object
    Private IDCounter As UInt64 = 1
    Private Random As New Random(Date.Now.Millisecond * 5)

    Public ReadOnly Property GetID() As UInt64
        Get
            SyncLock m_IdLock
                Dim toreturn As ULong = IDCounter
                If IDCounter < UInt64.MaxValue Then
                    IDCounter += 1
                ElseIf IDCounter = UInt64.MaxValue Then
                    IDCounter = 0
                End If

                Return toreturn
            End SyncLock
        End Get
    End Property

#End Region

#Region "New"
    Sub New()
        m_PacketList = New Dictionary(Of UInt64, cByteCounterPacket)
        m_sTimer = New Timers.Timer

        m_TotalBytes = 0
        m_TotalPackets = 0
        m_BytesPerS = 0
        m_PacketsPerS = 0

        AddHandler m_sTimer.Elapsed, AddressOf TimerElapsed
        StartTimer()
    End Sub
#End Region

#Region "Add"
    Public Sub AddPacket(ByVal packet As PacketWriter, ByVal source As PacketSource)
        Dim tmp As New cByteCounterPacket
        tmp.Bytes = packet.Length
        tmp.Source = source
        tmp.Timestamp = Date.Now

        SyncLock m_ListLock
            m_PacketList.Add(GetID, tmp)
            AddTotal(tmp.Bytes, 1)
        End SyncLock
    End Sub

    Public Sub AddPacket(ByVal packet As PacketReader, ByVal source As PacketSource)
        Dim tmp As New cByteCounterPacket
        tmp.Bytes = packet.Length
        tmp.Source = source
        tmp.Timestamp = Date.Now

        SyncLock m_ListLock
            m_PacketList.Add(GetID, tmp)
            AddTotal(tmp.Bytes, 1)
        End SyncLock
    End Sub


    Public Sub AddPacket(ByVal bytes As UShort, ByVal source As PacketSource)
        Dim tmp As New cByteCounterPacket
        tmp.Bytes = bytes
        tmp.Source = source
        tmp.Timestamp = Date.Now

        SyncLock m_ListLock
            m_PacketList.Add(GetID, tmp)
            AddTotal(tmp.Bytes, 1)
        End SyncLock
    End Sub
#End Region

#Region "Total"
    Private Sub AddTotal(ByVal bytes As UInt16, ByVal packets As UInt16)
        If m_TotalBytes + bytes < UInt64.MaxValue Then
            m_TotalBytes += bytes
        Else
            'Full ^^
        End If

        If m_TotalPackets + packets < UInt64.MaxValue Then
            m_TotalPackets += packets
        Else
            'Full ^^
        End If
    End Sub
#End Region

#Region "Packet per Secound"
    Public Sub StartTimer()
        m_sTimer.Interval = 1000 - Date.Now.Millisecond
        m_sTimer.Start()
    End Sub

    Public Sub TimerElapsed()
        m_sTimer.Stop()

        Try
            Dim tmpBytesS As UInt64 = 0
            Dim tmpPacketsS As UInt64 = 0

            Dim List = m_PacketList.Keys.ToList
            For i = 0 To List.Count - 1
                Dim key As UInt64 = List(i)
                If m_PacketList.ContainsKey(key) Then
                    Try
                        tmpBytesS += m_PacketList(key).Bytes
                        tmpPacketsS += 1

                        SyncLock m_ListLock
                            m_PacketList.Remove(key)
                        End SyncLock
                    Catch ex As Exception

                    End Try

                    'It also is possible to add an additional check to search only for the last secound here
                End If
            Next

            m_BytesPerS = tmpBytesS
            m_PacketsPerS = tmpPacketsS

        Catch ex As Exception
        End Try

        StartTimer()
    End Sub
#End Region

#Region "Format"
    Public Shared Function FormatBandwidth(ByVal bytes As UInt64) As String
        Dim prefix As String = "B/s"

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "KB/s"
        End If

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "MB/s"
        End If

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "GB/s"
        End If

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "TB/s"
        End If

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "PB/s"
        End If

        Return bytes & " " & prefix
    End Function

    Public Shared Function FormatVolume(ByVal bytes As UInt64) As String
        Dim prefix As String = "B"

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "KB"
        End If

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "MB"
        End If

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "GB"
        End If

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "TB"
        End If

        If bytes > 1024 Then
            bytes /= 1024
            prefix = "PB"
        End If

        Return bytes & " " & prefix
    End Function
#End Region
End Class

Public Enum PacketSource As Byte
    Client = 0
    Server = 1
End Enum
