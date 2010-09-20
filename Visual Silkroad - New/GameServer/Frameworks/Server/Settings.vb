﻿Module Settings
    'Here is Place for Settings like Xp Rates, etc...
    Public PlayerStartPos As New Position
    Public PlayerStartLevel As Byte
    Public PlayerStartGold As ULong
    Public PlayerStartMasteryLevel As Byte
    Public PlayerStartSkillPoints As UInteger
    Public PlayerStartGM As Boolean

    Public ServerXPRate As Long = 1
    Public ServerSPRate As Long = 1
    Public ServerGoldRate As Long = 1
    Public ServerDropRate As Long = 1

    Public Sub LoadSettings()
        'TODO: Load these Settings from a Config File
        PlayerStartPos.XSector = 168
        PlayerStartPos.YSector = 98
        PlayerStartPos.X = 978
        PlayerStartPos.Z = 1097
        PlayerStartPos.Y = 40

        PlayerStartGold = Long.MaxValue
        PlayerStartLevel = 100
        PlayerStartMasteryLevel = 100
        PlayerStartSkillPoints = 1000000 '1m
        PlayerStartGM = True

        ServerXPRate = 50
        ServerSPRate = 50
        ServerGoldRate = 50
        ServerDropRate = 50
    End Sub

End Module