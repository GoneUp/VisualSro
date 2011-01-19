Imports System.Timers
Module Timers
    Public PingTimer As Timer() = New Timer(14999) {}
    Public PlayerMoveTimer As Timer() = New Timer(14999) {}

    Dim rand As New Random
    Public Sub LoadTimers(ByVal TimerCount As Integer)

        Try
            ReDim PingTimer(TimerCount), PlayerMoveTimer(TimerCount)

            For i As Integer = 0 To TimerCount - 1
                PingTimer(i) = New Timer()
                AddHandler PingTimer(i).Elapsed, AddressOf PingTimer_Elapsed
                PlayerMoveTimer(i) = New Timer()
                AddHandler PlayerMoveTimer(i).Elapsed, AddressOf PlayerMoveTimer_Elapsed
            Next

        Catch ex As Exception

        End Try
    End Sub

    Public Sub PingTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        Dim Index As Integer = -1
        Try
            Dim objB As Timer = DirectCast(sender, Timer)
            For i As Integer = Information.LBound(PingTimer, 1) To Information.UBound(PingTimer, 1)
                If Object.ReferenceEquals(PingTimer(i), objB) Then
                    Index = i
                    Exit For
                End If
            Next

            PingTimer(Index).Stop()

            If s(Index).Connected Then
                Parser.SendPing(Index)
            End If


            PingTimer(Index).Start()

        Catch ex As Exception
            Console.WriteLine("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
        End Try

    End Sub

    Public Sub PlayerMoveTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        Dim Index As Integer = -1
        Try
            Dim objB As Timer = DirectCast(sender, Timer)
            For i As Integer = Information.LBound(PlayerMoveTimer, 1) To Information.UBound(PlayerMoveTimer, 1)
                If Object.ReferenceEquals(PlayerMoveTimer(i), objB) Then
                    Index = i
                    Exit For
                End If
            Next

            PlayerMoveTimer(Index).Stop()

            If Pos(Index).XSector = 0 Or Pos(Index).YSector = 0 Then
                Console.WriteLine("0 ERR")
                Exit Sub
            End If

            Dim ToX As Single = GetRealX(Pos(Index).XSector, Pos(Index).X) + rand.Next(-10, +10)
            Dim ToY As Single = GetRealY(Pos(Index).YSector, Pos(Index).Y) + rand.Next(-10, +10)

            Dim ToPos As New Position
            ToPos.XSector = GetXSec(ToX)
            ToPos.YSector = GetYSec(ToY)
            ToPos.X = GetXOffset(ToX)
            ToPos.Z = Pos(Index).Z
            ToPos.Y = GetYOffset(ToY)

            If ToPos.X = Pos(Index).X And ToPos.Y = Pos(Index).Y Then
                ToPos.X += 1
            End If


            'OnMoveUser(ToPos, Index)


            PlayerMoveTimer(Index).Start()

        Catch ex As Exception
            Console.WriteLine("Timer Error: " & ex.Message & " Stack: " & ex.StackTrace & " Index: " & Index) '
            PlayerMoveTimer(Index).Start()
        End Try

    End Sub
End Module
