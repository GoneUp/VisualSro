Namespace Functions
    Public Class cWhitestats
        Private ReadOnly weaponArray() As UInt64 = New UInt64() {1073741824, 33554432, 1048576, 32768, 1024, 32, 1}
        Private ReadOnly equipementArray() As UInt64 = New UInt64() {33554432, 1048576, 32768, 1024, 32, 1}
        Private ReadOnly shieldArray() As UInt64 = New UInt64() {33554432, 1048576, 32768, 1024, 32, 1}
        Private ReadOnly accesoryArray() As UInt64 = New UInt64() {32, 1}


        Public PerDurability As Byte
        Public PerPhyRef As Byte
        Public PerMagRef As Byte
        Public PerPhyAtk As Byte
        Public PerMagAtk As Byte
        Public PerPhyDef As Byte
        Public PerMagDef As Byte
        Public PerBlock As Byte
        Public PerCritical As Byte
        Public PerAttackRate As Byte
        Public PerParryRate As Byte
        Public PerPhyAbs As Byte
        Public PerMagAbs As Byte

        Public Sub New()

        End Sub

        Public Sub New(ByVal ItemType As Type, ByVal ws As UInt64)
            SetWhitestats(ItemType, ws)
        End Sub

        Public Function GetWhiteStats(ByVal ItemType As Type) As ULong
            Dim ws As ULong = 0

            Select Case ItemType
                Case Type.Equipment
                    ws += Math.Round(31 * PerDurability / 100)
                    ws += Math.Round(31 * PerPhyRef / 100) * 32
                    ws += Math.Round(31 * PerMagRef / 100) * 1024
                    ws += Math.Round(31 * PerPhyDef / 100) * 32768
                    ws += Math.Round(31 * PerMagDef / 100) * 1048576
                    ws += Math.Round(31 * PerParryRate / 100) * 33554432

                Case Type.Shield

                    ws += Math.Round(31 * PerDurability / 100)
                    ws += Math.Round(31 * PerPhyRef / 100) * 32
                    ws += Math.Round(31 * PerMagRef / 100) * 1024
                    ws += Math.Round(31 * PerBlock / 100) * 32768
                    ws += Math.Round(31 * PerPhyDef / 100) * 1048576
                    ws += Math.Round(31 * PerMagDef / 100) * 33554432

                Case Type.Weapon

                    ws += Math.Round(31 * PerDurability / 100)
                    ws += Math.Round(31 * PerPhyRef / 100) * 32
                    ws += Math.Round(31 * PerMagRef / 100) * 1024
                    ws += Math.Round(31 * PerAttackRate / 100) * 32768
                    ws += Math.Round(31 * PerPhyAtk / 100) * 1048576
                    ws += Math.Round(31 * PerMagAtk / 100) * 33554432
                    ws += Math.Round(31 * PerCritical / 100) * 1073741824

                Case Type.Accesory

                    ws += Math.Round(31 * PerPhyAbs / 100)
                    ws += Math.Round(31 * PerMagAbs / 100) * 32

            End Select
            Return ws
        End Function

        Public Sub SetWhitestats(ByVal ItemType As Type, ByVal ws As UInt64)
            Dim round As Byte = 0
            Dim array As ULong()

            Select Case ItemType
                Case Type.Weapon
                    array = weaponArray
                Case Type.Shield
                    array = shieldArray
                Case Type.Equipment
                    array = equipementArray
                Case Type.Accesory
                    array = equipementArray
            End Select

            Dim results As Byte() = New Byte(array.Length) {}

            Do While (round < array.Length)
                Dim counter As Int32 = 31
                Do While (counter <> 0)
                    Dim value As UInt64 = (Convert.ToUInt64(counter) * array(round))
                    If (ws >= value) Then
                        results(round) = Convert.ToInt32(Math.Round(((Convert.ToDouble(counter) / 31) * 100)))

                        ws = (ws - value)
                        Exit Do
                    End If
                    counter -= 1
                Loop
                round += 1
            Loop

            Select Case ItemType
                Case Type.Weapon
                    PerCritical = results(0)
                    PerMagAtk = results(1)
                    PerPhyAtk = results(2)
                    PerAttackRate = results(3)
                    PerMagRef = results(4)
                    PerPhyRef = results(5)
                    PerDurability = results(6)
                Case Type.Shield
                    PerMagDef = results(0)
                    PerPhyRef = results(1)
                    PerBlock = results(2)
                    PerMagRef = results(3)
                    PerPhyRef = results(4)
                    PerDurability = results(5)
                Case Type.Equipment
                    PerParryRate = results(0)
                    PerMagDef = results(1)
                    PerPhyDef = results(2)
                    PerMagRef = results(3)
                    PerPhyRef = results(4)
                    PerDurability = results(5)
                Case Type.Accesory
                    PerMagAbs = results(0)
                    PerPhyAbs = results(1)
            End Select
        End Sub


        Enum Type
            Weapon
            Shield
            Accesory
            Equipment
        End Enum
    End Class
End Namespace
