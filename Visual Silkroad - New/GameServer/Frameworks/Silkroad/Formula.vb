Namespace GameServer.Functions
    Module Formula
        Public Function CalculateDistance(ByVal Position1 As Position, ByVal Position2 As Position) As Long
            Dim distance As Long = Math.Round(Math.Sqrt((Position1.X - Position2.X) ^ 2 + (Position1.Y - Position2.Y) ^ 2)) 'Calculate Distance
            Return distance
        End Function







    End Module
End Namespace