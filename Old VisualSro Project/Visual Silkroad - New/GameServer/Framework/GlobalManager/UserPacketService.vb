Imports SRFramework
Imports System.Runtime.Serialization

Namespace GlobalManager
    Public Class UserPacketService
        'Funktionen:
        'Dieses Modul nur zum packete senden, extra Klasse die eine Art Loader darstellt, sie ist Event basiert soll einen Callback auf meine Haupt-Parsing Funktion ausführen

        Private m_formatter As IFormatter = New Formatters.Binary.BinaryFormatter()

        Public Event UserGetEvent As dUserGetEvent
        Public Event UserUpdateEvent As dUserUpdateEvent
        Public Event UserFailureEvent As dUserFailureEvent

        Public Delegate Sub dUserGetEvent(user As cUser)
        Public Delegate Sub dUserUpdateEvent(accountID As UInt32) 'only in case of success
        Public Delegate Sub dUserFailureEvent(accountID As UInt32, failbyte As Byte)

#Region "New/Disponse"
        Sub New()
            AddHandler GlobalManagerCon.OnUserPacketService, AddressOf UserReplyHandler
        End Sub

        Sub Disponse()
            RemoveHandler GlobalManagerCon.OnUserPacketService, AddressOf UserReplyHandler
        End Sub
#End Region


        Public Sub GetUser(accountID As UInt32)
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.AGENT_USERINFO)
            writer.Byte(1) 'mode
            writer.DWord(accountID)
            GlobalManagerCon.Send(writer.GetBytes)
        End Sub

        Public Sub UpdateUser(user As cUser)
            Dim writer As New PacketWriter
            writer.Create(InternalClientOpcodes.AGENT_USERINFO)
            writer.Byte(2) 'mode
            writer.DWord(user.AccountID)
            m_formatter.Serialize(writer.BaseStream, user)

            GlobalManagerCon.Send(writer.GetBytes)
        End Sub

        Public Sub UserReplyHandler(packet As PacketReader)
            Dim mode As Byte = packet.Byte()
            Dim accountID As UInt32 = packet.DWord

            Select Case mode
                Case 1
                    Dim submode As Byte = packet.Byte

                    If submode = 1 Then
                        'Succeed
                        Dim newUser As cUser = m_formatter.Deserialize(packet.BaseStream)

                        If newUser IsNot Nothing AndAlso accountID = newUser.AccountID Then
                            RaiseEvent UserGetEvent(newUser)
                        Else
                            RaiseEvent UserFailureEvent(accountID, 4)
                        End If
                    Else
                        'Failed
                        RaiseEvent UserFailureEvent(accountID, packet.Byte)
                    End If

                Case 2
                    Dim submode As Byte = packet.Byte

                    If submode = 1 Then
                        RaiseEvent UserUpdateEvent(accountID)
                    Else
                        RaiseEvent UserFailureEvent(accountID, packet.Byte)
                    End If
            End Select
        End Sub
    End Class
End Namespace
