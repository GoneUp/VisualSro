Imports SRFramework

Namespace GameDB
    Public Class GameUserLoader
        Public Event GetCallback As dGetCallback
        Public Event UpdateCallback As dUpdateCallback
        Public Event FailureCallback As dFailureCallback

        Public Delegate Sub dGetCallback(user As cUser, packet As PacketReader, Index_ As Integer)
        Public Delegate Sub dUpdateCallback(accountID As UInt32, Index_ As Integer)
        Public Delegate Sub dFailureCallback(accountID As UInt32, failbyte As Byte, Index_ As Integer)

        Public UserPacket As PacketReader
        Public SocketIndex As Integer
        Public UserAccountID As UInt32

        Private m_loader As GlobalManager.UserPacketService

        Sub New(Index_ As Integer)
            SocketIndex = Index_

            m_loader = New GlobalManager.UserPacketService()
            AddHandler m_loader.UserGetEvent, AddressOf LoaderGetEvent
            AddHandler m_loader.UserUpdateEvent, AddressOf LoaderUpdateEvent
            AddHandler m_loader.UserFailureEvent, AddressOf LoaderFailureEvent
        End Sub

        Sub New(packet As PacketReader, Index_ As Integer)
            SocketIndex = Index_
            UserPacket = packet

            m_loader = New GlobalManager.UserPacketService()
            AddHandler m_loader.UserGetEvent, AddressOf LoaderGetEvent
            AddHandler m_loader.UserUpdateEvent, AddressOf LoaderUpdateEvent
            AddHandler m_loader.UserFailureEvent, AddressOf LoaderFailureEvent
        End Sub

        Sub Disponse()
            RemoveHandler m_loader.UserGetEvent, AddressOf LoaderGetEvent
            RemoveHandler m_loader.UserUpdateEvent, AddressOf LoaderUpdateEvent
            RemoveHandler m_loader.UserFailureEvent, AddressOf LoaderFailureEvent

            m_loader.Disponse()
        End Sub

        Public Sub LoadFromGlobal(accountID As UInt32)
            UserAccountID = accountID
            m_loader.GetUser(accountID)
        End Sub

        Public Sub UpdateGlobal(user As cUser)
            UserAccountID = user.AccountID
            m_loader.UpdateUser(user)
        End Sub

        Private Sub LoaderGetEvent(user As cUser)
            If user.AccountID = UserAccountID Then
                RaiseEvent GetCallback(user, UserPacket, SocketIndex)
            End If
        End Sub

        Private Sub LoaderUpdateEvent(accountID As UInt32)
            If accountID = UserAccountID Then
                RaiseEvent UpdateCallback(accountID, SocketIndex)
            End If
        End Sub

        Private Sub LoaderFailureEvent(accountID As UInt32, failbyte As Byte)
            If accountID = UserAccountID Then
                RaiseEvent FailureCallback(accountID, failbyte, SocketIndex)
            End If
        End Sub
    End Class
End Namespace
