Imports System.Timers
Namespace GameServer
    Module Timers
        Public PlayerAttack As Timer() = New Timer(14999) {}
        Public MonsterMovement As Timer() = New Timer(14999) {}
        Public MonsterDeath As Timer() = New Timer(14999) {}
        Public MonsterAttack As Timer() = New Timer(14999) {}
        Public CastAttackTimer As Timer() = New Timer(14999) {}
        Public CastBuffTimer As Timer() = New Timer(14999) {}
        Public UsingItemTimer As Timer() = New Timer(14999) {}

        Public Sub LoadTimers()
            Console.Write("Loading Timers...")
            For i As Integer = 0 To 14999
                PlayerAttack(i) = New Timer()
                AddHandler PlayerAttack(i).Elapsed, AddressOf AttackTimer_Elapsed

            Next

            PlayerAttack(0).Interval = 1
            PlayerAttack(0).Start()
            Console.WriteLine("finished!")
        End Sub

        Public Sub AttackTimer_Elapsed(ByVal sender As Object, ByVal e As ElapsedEventArgs)
            PlayerAttack(0).Stop()
            Dim objB As Timer = DirectCast(sender, Timer)
            Dim Index As Integer = -1
            For i As Integer = Information.LBound(PlayerAttack, 1) To Information.UBound(PlayerAttack, 1)
                If Object.ReferenceEquals(PlayerAttack(i), objB) Then
                    Index = i
                    Exit For
                End If
            Next

        End Sub
    End Module
End Namespace