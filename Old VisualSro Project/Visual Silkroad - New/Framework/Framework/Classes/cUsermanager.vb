Public Class cUsermanager
    'Funktionen, soll User vom GlobalManager requesten und der funtkion beritstekllen
    'Sollte mit events gehen, damit es keine threadlocks wegen der delays gibt
    'Möglichkeit eines Caches? NEIN

#Region "Fields"

#End Region
#Region "Events"
    Public Event UserRequestInit As dUserRequestInit
    Public Event UserRequestLoaded As dUserRequestLoaded
    Public Event UserRequestUpdate As dUserUpdate

    Public Event SilkInit As dSilkInit
    Public Event SilkLoaded As dSilkLoaded
    Public Event SilkUpdate As dSilkUpdate

    Public Delegate Sub dUserRequestInit(accountID As UInt32, Index_ As Int32)
    Public Delegate Sub dUserRequestLoaded(user As cUser, Index_ As Int32)
    Public Delegate Sub dUserUpdate(accountID As UInt32, user As cUser, Index_ As Int32)

    Public Delegate Sub dSilkInit(accountID As UInt32, Index_ As Int32)
    Public Delegate Sub dSilkLoaded(accountID As UInt32, silk As UInt32, silkBonus As UInt32, silkPoints As UInt32, Index_ As Int32)
    Public Delegate Sub dSilkUpdate(accountID As UInt32, silk As UInt32, silkBonus As UInt32, silkPoints As UInt32, Index_ As Int32)
#End Region
#Region "Request User"

#End Region
#Region "Update User"

#End Region
#Region "Silk"

#End Region


End Class
