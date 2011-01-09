Namespace GameServer.Settings
    Module Settings
        'Here is Place for Settings like Xp Rates, etc...
        Public PlayerStartPos As New Position
        Public PlayerStartReturnPos As New Position
        Public PlayerStartLevel As Byte
        Public PlayerStartGold As ULong
        Public PlayerStartMasteryLevel As Byte
        Public PlayerStartSkillPoints As UInteger
        Public PlayerStartGM As Boolean

        Public ServerXPRate As Long = 1
        Public ServerSPRate As Long = 1
        Public ServerGoldRate As Long = 1
        Public ServerDropRate As Long = 1
        Public ServerLevelCap As Byte = 100
        Public ServerMasteryCap As UInteger = 300
        Public ServerRange As UInteger = 750
        Public ServerTaxRate As UInt16 = 0
        Public ServerSpawnsPerSec As UInteger = 50

        Public Log_Detail As Boolean = False
        Public Log_GM As Boolean = False
        Public Log_Mall As Boolean = False
        Public Log_Chat As Boolean = False

        'Enable or disable Mods
        Public ModGeneral As Boolean = True
        Public ModDamage As Boolean = False

        Public Sub LoadSettings()
            'TODO: Load these Settings from a Config File
            PlayerStartPos.XSector = 168
            PlayerStartPos.YSector = 98
            PlayerStartPos.X = 978
            PlayerStartPos.Z = 0
            PlayerStartPos.Y = 40

            PlayerStartReturnPos.XSector = 168
            PlayerStartReturnPos.YSector = 97
            PlayerStartReturnPos.X = 980
            PlayerStartReturnPos.Z = 0
            PlayerStartReturnPos.Y = 1330

            PlayerStartGold = 1000000 '1m
            PlayerStartLevel = 1
            PlayerStartMasteryLevel = 0
            PlayerStartSkillPoints = 1000000 '1m
            PlayerStartGM = True

            ServerXPRate = 50
            ServerSPRate = 50
            ServerGoldRate = 50
            ServerDropRate = 50
            ServerLevelCap = 100
            ServerMasteryCap = 300
            ServerRange = 100
            ServerTaxRate = 20
            ServerSpawnsPerSec = 50

            Log_Chat = True
            Log_GM = True
            Log_Mall = True

            ModGeneral = True
            ModDamage = True
        End Sub

    End Module
End Namespace
