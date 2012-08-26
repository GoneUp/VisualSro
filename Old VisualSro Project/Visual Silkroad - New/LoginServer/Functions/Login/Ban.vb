Imports SRFramework

Namespace Functions
    Module Ban

        Public Sub CheckBannTime(ByVal UserIndex As Integer)
            Dim user As cUser = LoginDb.Users(UserIndex)
            Try
                If user.Banned = True Then
                    Dim wert As Integer = Date.Compare(user.BannTime, Date.Now)
                    If wert = -1 Then
                        'Zeit abgelaufen
                        user.Banned = False
                        Database.SaveQuery(String.Format("UPDATE users SET banned = '0' WHERE id = '{0}'", user.AccountId))

                        LoginDb.Users(UserIndex) = user
                    End If
                End If

            Catch ex As Exception
                Log.WriteSystemLog("[BAN_CHECK][ID:" & user.AccountId & "][NAME:" & user.Name & "][TIME:" & user.BannTime.ToLongTimeString & "]")
            End Try
        End Sub

        Public Sub BanUser(ByVal LeakTime As Date, ByVal UserIndex As Integer)
            Dim user As cUser = LoginDb.Users(UserIndex)
            Try
                user.Banned = True
                user.BannTime = LeakTime
                user.BannReason = (String.Format("You got banned for {0} Minutes because of {1} failed Logins.", DateDiff(DateInterval.Minute, Date.Now, LeakTime), Settings.Max_FailedLogins))
                Dim time As String = String.Format("{0}-{1}-{2} {3}:{4}:{5}", LeakTime.Year, LeakTime.Month, LeakTime.Day, LeakTime.Hour, LeakTime.Minute, LeakTime.Second)

                Database.SaveQuery(String.Format("UPDATE users SET banned='1', bantime = '{0}', banreason = '{1}' where id='{2}'", time, user.BannReason, user.AccountId))
                LoginDb.Users(UserIndex) = user
            Catch ex As Exception
                Log.WriteSystemLog("[BAN_USER][ID:" & user.AccountId & "][NAME:" & user.Name & "][TIME:" & user.BannTime.ToLongTimeString & "]")
            End Try
        End Sub





    End Module
End Namespace
