Imports System.Net.Sockets
Imports SRFramework

Namespace Functions
    Module Movement
        Public Sub OnPlayerMovement(ByVal Index_ As Integer, ByVal packet As PacketReader)
            If PlayerData(Index_).Busy = True Or PlayerData(Index_).Attacking = True Then
                Exit Sub
            End If

            Dim tag As Byte = packet.Byte
            If tag = 1 Then
                Dim toPos As New Position
                toPos.XSector = packet.Byte
                toPos.YSector = packet.Byte

                If IsInCave(toPos) = False Then
                    toPos.X = packet.WordInt
                    toPos.Z = packet.WordInt
                    toPos.Y = packet.WordInt
                    Debug.Print("x: " & toPos.X & " y: " & toPos.Y)
                Else
                    'In Cave
                    toPos.X = packet.DWordInt
                    toPos.Z = packet.DWordInt
                    toPos.Y = packet.DWordInt
                End If


                MoveUser(Index_, toPos)
            ElseIf tag = 0 Then
                Dim tag2 As Byte = packet.Byte
                Dim toAngle As UShort = packet.Word
                Dim toGrad As Single = (toAngle / 65535) * 360
                SendPm(Index_, "You are tyring to Angle Move to: " & toGrad, "Debug")

            End If
        End Sub

        Public Sub MoveUser(ByVal Index_ As Integer, ByVal toPos As Position)
            Try
                Dim writer As New PacketWriter
                SendMoveObject(writer, PlayerData(Index_).UniqueID, toPos, PlayerData(Index_).Position, True)
                Server.SendIfPlayerIsSpawned(writer.GetBytes, Index_)

                PlayerData(Index_).PosTracker.Move(ToPos)
                PlayerMoveTimer(Index_).Interval = 50
                PlayerMoveTimer(Index_).Start()
                ' End If

            Catch ex As Exception
                Console.WriteLine("OnMoveUser::error...")
                Debug.Write(ex)
            End Try
        End Sub

        Public Sub SendMoveObject(writer As PacketWriter, uniqueID As UInt32, ByVal toPos As Position, curPos As Position, sendSource As Boolean)
            writer.Create(ServerOpcodes.GAME_MOVEMENT)
            writer.DWord(uniqueID)
            writer.Byte(1)
            'destination
            writer.Byte(toPos.XSector)
            writer.Byte(toPos.YSector)

            If IsInCave(toPos) = False Then
                writer.Byte(BitConverter.GetBytes(CShort(toPos.X)))
                writer.Byte(BitConverter.GetBytes(CShort(toPos.Z)))
                writer.Byte(BitConverter.GetBytes(CShort(toPos.Y)))
            Else
                'In Cave
                writer.Byte(BitConverter.GetBytes(CInt(toPos.X)))
                writer.Byte(BitConverter.GetBytes(CInt(toPos.Z)))
                writer.Byte(BitConverter.GetBytes(CInt(toPos.Y)))
            End If

            writer.Byte(1) '1= source

            writer.Byte(curPos.XSector)
            writer.Byte(curPos.YSector)
            writer.Byte(BitConverter.GetBytes(CShort(curPos.X * -1)))
            writer.Byte(BitConverter.GetBytes(curPos.Z))
            writer.Byte(BitConverter.GetBytes(CShort(curPos.Y * -1)))
        End Sub

        ''' <summary>
        ''' Moves a User To a Object Based on the Range
        ''' </summary>
        ''' <param name="Index_"></param>
        ''' <param name="objectPos"></param>
        ''' <param name="Range"></param>
        ''' <returns>The Walktime in ms</returns>
        ''' <remarks></remarks>
        Public Function MoveUserToObject(ByVal Index_ As Integer, ByVal objectPos As Position, ByVal Range As Integer) As Single
            Dim toPos As Position = objectPos

            Dim distanceX As Double = PlayerData(Index_).Position.ToGameX - toPos.ToGameX
            Dim distanceY As Double = PlayerData(Index_).Position.ToGameY - toPos.ToGameY
            Dim distance As Double = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY))

            If distance > Range Then
                Dim cosinus As Double = Math.Cos(distanceX / distance)
                Dim sinus As Double = Math.Sin(distanceY / distance)

                Dim distanceXNew As Double = Range * cosinus
                Dim distanceYNew As Double = Range * sinus

                toPos.X = GetXOffset(toPos.ToGameX + distanceXNew)
                toPos.Y = GetYOffset(toPos.ToGameY + distanceYNew)
                toPos.XSector = GetXSecFromGameX(toPos.ToGameX)
                toPos.YSector = GetYSecFromGameY(toPos.ToGameY)
                Debug.Print(toPos.ToGameX & " Y " & toPos.ToGameY)
            End If

            Dim walkTime As Single
            Select Case PlayerData(Index_).PosTracker.SpeedMode
                Case cPositionTracker.enumSpeedMode.Walking
                    walkTime = (distance / PlayerData(Index_).WalkSpeed) * 10000
                Case cPositionTracker.enumSpeedMode.Running
                    walkTime = (distance / PlayerData(Index_).RunSpeed) * 10000
                Case cPositionTracker.enumSpeedMode.Zerking
                    walkTime = (distance / PlayerData(Index_).BerserkSpeed) * 10000
            End Select

            MoveUser(Index_, toPos)
            Return walkTime
        End Function
        
        Public Sub CheckForCaveTeleporter(ByVal Index_ As Integer)
            If PlayerData(Index_) IsNot Nothing Then
                For i = 0 To RefCaveTeleporter.Count - 1
                    If CalculateDistance(PlayerData(Index_).Position, RefCaveTeleporter(i).FromPosition) <= RefCaveTeleporter(i).Range Then
                        'In Range --> Teleport
                        Dim point As TeleportPoint = GetTeleportPointByNumber(RefCaveTeleporter(i).ToTeleporterID)
                        '####################### Notice : Dont work
                        'TODO: Rework this
                        Dim link As TeleportLink = point.Links(0)

                        If PlayerData(Index_).Level < link.MinLevel And link.MinLevel > 0 Then
                            'Level too low
                            Dim writer As New PacketWriter
                            writer.Create(ServerOpcodes.GAME_TELEPORT_START)
                            writer.Byte(2)
                            writer.Byte(&H15)
                            writer.Byte(&H1C)
                            Server.Send(writer.GetBytes, Index_)
                        ElseIf PlayerData(Index_).Level > link.MaxLevel And link.MaxLevel > 0 Then
                            'Level too high
                            SendNotice("Cannot Teleport because your Level is too high.", Index_)
                        ElseIf PlayerData(Index_).Busy = False Then
                            PlayerData(Index_).Busy = True
                            PlayerData(Index_).SetPosition = point.ToPos

                            Database.SaveQuery(
                                String.Format(
                                    "UPDATE characters SET xsect='{0}', ysect='{1}', xpos='{2}', zpos='{3}', ypos='{4}' where id='{5}'",
                                    PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector,
                                    Math.Round(PlayerData(Index_).Position.X), Math.Round(PlayerData(Index_).Position.Z),
                                    Math.Round(PlayerData(Index_).Position.Y), PlayerData(Index_).CharacterId))
                            OnTeleportUser(Index_, PlayerData(Index_).Position.XSector, PlayerData(Index_).Position.YSector)
                        End If
                    End If
                Next
            End If
        End Sub
    End Module
End Namespace
