Namespace Functions
    Public Class Position
        Public XSector As Byte = 0
        Public YSector As Byte = 0
        Public X As Single = 0
        Public Z As Single = 0
        Public Y As Single = 0

        Public Function ToGameX() As Single
            Return CSng((XSector - 135) * 192 + (X / 10))
        End Function

        Public Function ToGameY() As Single
            Return CSng((YSector - 92) * 192 + (Y / 10))
        End Function
    End Class
End Namespace