Imports System.Net.Sockets
Imports SRFramework
Imports GameServer.Functions


Public Class cServer_Gameserver
    Inherits ServerBase

    Public Sub SendToAllIngame(ByVal buff() As Byte)
        For i As Integer = 0 To OnlineClients
            Dim socket As Socket = ClientList.GetSocket(i)
            Dim player As cCharacter = PlayerData(i)
            'Check if Player is ingame
            If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                If player.Ingame = True Then
                    Send(buff, i)
                End If
            End If
        Next i
    End Sub

    Public Sub SendToAllIngameExpectMe(ByVal buff() As Byte, ByVal index As Integer)
        For i As Integer = 0 To MaxClients - 1
            Dim socket As Socket = ClientList.GetSocket(i)
            Dim player As cCharacter = PlayerData(i)
            'Check if Player is ingame
            If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso (i <> index) _
                Then
                If player.Ingame = True Then
                    Send(buff, i)
                End If
            End If
        Next i
    End Sub

    Public Sub SendToAllInRange(ByVal buff() As Byte, ByVal Position As Position)
        For i As Integer = 0 To OnlineClients
            Dim socket As Socket = ClientList.GetSocket(i)
            Dim player As cCharacter = PlayerData(i)
            'Check if Player is ingame
            If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected Then
                Dim distance As Long = CalculateDistance(Position, player.Position)
                'Calculate Distance
                If distance < Settings.ServerRange Then
                    'In Rage 
                    If player.Ingame = True Then
                        Send(buff, i)
                    End If
                End If
            End If
        Next i
    End Sub

    Public Sub SendToAllInRangeExpectMe(ByVal buff() As Byte, ByVal Index As Integer)

        For i As Integer = 0 To OnlineClients
            Dim socket As Socket = ClientList.GetSocket(i)
            Dim player As cCharacter = PlayerData(i)
            'Check if Player is ingame
            If (socket IsNot Nothing) AndAlso (player IsNot Nothing) AndAlso socket.Connected AndAlso (i <> Index) _
                Then
                Dim distance As Long = CalculateDistance(PlayerData(Index).Position, player.Position)
                'Calculate Distance
                If distance < Settings.ServerRange Then
                    'In Rage 
                    If player.Ingame = True Then
                        Send(buff, i)
                    End If
                End If
            End If
        Next i
    End Sub

    Public Sub SendIfPlayerIsSpawned(ByVal buff() As Byte, ByVal Index_ As Integer, Optional ByVal expectMe As Boolean = False)
        For i = 0 To MaxClients - 1
            If PlayerData(i) IsNot Nothing Then
                If PlayerData(i).SpawnedPlayers.Contains(Index_) Or (Index_ = i And expectMe = False) Then
                    If PlayerData(i).Ingame = True Then
                        Send(buff, i)
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub SendIfMobIsSpawned(ByVal buff() As Byte, ByVal mobUniqueID As Integer)
        For i = 0 To MaxClients - 1
            If PlayerData(i) IsNot Nothing Then
                If PlayerData(i).SpawnedMonsters.Contains(MobUniqueID) Then
                    Send(buff, i)
                End If
            End If
        Next
    End Sub

    Public Sub SendIfItemIsSpawned(ByVal buff() As Byte, ByVal itemUniqueID As Integer)
        For i = 0 To MaxClients - 1
            If PlayerData(i) IsNot Nothing Then
                If PlayerData(i).SpawnedItems.Contains(ItemUniqueID) Then
                    Send(buff, i)
                End If
            End If
        Next
    End Sub


    Public Sub SendToGuild(ByVal buff() As Byte, ByVal guildID As UInteger)
        For i = 0 To MaxClients - 1
            If PlayerData(i) IsNot Nothing Then
                If PlayerData(i).GuildID = GuildID Then
                    Send(buff, i)
                End If
            End If
        Next
    End Sub


    Public Sub SendToStallSession(ByVal buff() As Byte, ByVal stallID As UInteger, ByVal owner As Boolean)
        For i = 0 To Stalls.Count - 1
            If Stalls(i).StallID = stallID Then
                'Send to Visitors
                For v = 0 To Stalls(i).Visitors.Count - 1
                    Send(buff, Stalls(i).Visitors(v))
                Next

                'Send To Owner
                If Owner Then
                    Send(buff, Stalls(i).OwnerIndex)
                End If

                Exit For
            End If
        Next
    End Sub
End Class
