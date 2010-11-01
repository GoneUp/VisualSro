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

    Public LogGM As Boolean = False
    Public LogMall As Boolean = False
    Public LogChat As Boolean = False

    Public Sub LoadSettings()
        'TODO: Load these Settings from a Config File
        PlayerStartPos.XSector = 168
        PlayerStartPos.YSector = 98
        PlayerStartPos.X = 978
        PlayerStartPos.Z = 1097
        PlayerStartPos.Y = 40

        PlayerStartReturnPos.XSector = 168
        PlayerStartReturnPos.YSector = 97
        PlayerStartReturnPos.X = 980
        PlayerStartReturnPos.Z = 65504
        PlayerStartReturnPos.Y = 1330

        PlayerStartGold = 1000000 '1m
        PlayerStartLevel = 100
        PlayerStartMasteryLevel = 0
        PlayerStartSkillPoints = 1000000 '1m
        PlayerStartGM = True

        ServerXPRate = 50
        ServerSPRate = 50
        ServerGoldRate = 50
        ServerDropRate = 50
        ServerLevelCap = 100
        ServerMasteryCap = 300
        ServerRange = 3000

        LogChat = True
        LogGM = True
        LogMall = True
    End Sub

End Module
