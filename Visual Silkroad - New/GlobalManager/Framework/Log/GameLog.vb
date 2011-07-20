Namespace GlobalManager.Log
    Module GameLog

        Public Sub WriteGameLog(ByVal Index_ As Integer, ByVal Action As String, ByVal Action2 As String, ByVal Message As String)
            Dim IP As String = ClientList.GetSocket(Index_).RemoteEndPoint.ToString
            Dim time As String = String.Format("{0}-{1}-{2} {3}:{4}:{5}", Date.Now.Year, Date.Now.Month, Date.Now.Day, Date.Now.Hour, Date.Now.Minute, Date.Now.Second)
            DataBase.SaveQuery(String.Format("INSERT INTO log(ip_adress, charname, action, action2, parameter, time) VALUE ('{0}','[UNKNWON]','{1}','{2}','{3}','{4}')", IP, Action, Action2, Message, time))
        End Sub


    End Module
End Namespace

