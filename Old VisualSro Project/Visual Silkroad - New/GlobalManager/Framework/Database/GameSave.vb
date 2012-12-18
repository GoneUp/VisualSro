Imports SRFramework

Namespace DBSave
    Module GameSave
        Public Sub SaveUser(ByVal user As cUser)
            Dim time As String = String.Format("{0}-{1}-{2} {3}:{4}:{5}", user.BannTime.Year, user.BannTime.Month, user.BannTime.Day, user.BannTime.Hour, user.BannTime.Minute, user.BannTime.Second)
            Database.SaveQuery(String.Format("UPDATE users SET failed_logins='{0}', banned='{1}', bantime='{2}', banreason='{3}', silk='{4}', silk_bonus='{5}', silk_points='{6}', permission='{7}' storage_slots='{8}',  where id='{9}'",
                                             user.FailedLogins, Convert.ToString(user.Banned), time, user.BannReason, _
                                             user.Silk, user.Silk_Bonus, user.Silk_Points, user.Permission, user.StorageSlots, _
                                             user.AccountID))

        End Sub

        Public Sub SaveUserSilk(ByVal user As cUser)
            Database.SaveQuery(String.Format("UPDATE users SET silk='{0}', silk_bonus='{1}', silk_points='{2}' where id='{3}'", user.Silk, user.Silk_Bonus, user.Silk_Points, user.AccountID))
        End Sub

        Public Sub SaveUserBan(ByVal user As cUser)
            Dim time As String = String.Format("{0}-{1}-{2} {3}:{4}:{5}", user.BannTime.Year, user.BannTime.Month, user.BannTime.Day, user.BannTime.Hour, user.BannTime.Minute, user.BannTime.Second)
            Database.SaveQuery(String.Format("UPDATE users SET banned='{0}', bantime = '{1}', banreason = '{2}' where id='{3}'", Convert.ToByte(user.Banned), time, user.BannReason, user.AccountID))
        End Sub

        Public Sub SaveFailedLogins(ByVal user As cUser)
            Database.SaveQuery(String.Format("UPDATE users SET failed_logins='{0}' where id='{1}'", user.FailedLogins, user.AccountID))
        End Sub
    End Module
End Namespace
