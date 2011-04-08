Namespace LoginServer.Functions
    Module Register

        Public RegisterList As New List(Of Register_)
        Public Rand As New Random

        Public Sub RegisterUser(ByVal Name As String, ByVal Password As String, ByVal Index_ As Integer)
            DataBase.InsertData(String.Format("INSERT INTO users(username, password) VALUE ('{0}','{1}')", Name, Password))

            Dim tmp As New UserArray
            tmp.AccountId = Rand.Next(500000, 1000000)
            tmp.Name = Name
            tmp.Pw = Password
            tmp.FailedLogins = 0
            tmp.Banned = True
            tmp.BannReason = "Please wait for Activation."
            tmp.BannTime = Date.Now.AddMinutes(2)
            Users.Add(tmp)

            Dim tmp_2 As New Register_
            tmp_2.IP = ClientList.GetIP(Index_)
            tmp_2.Name = Name
            tmp_2.Password = Password
            tmp_2.Time = Date.Now
            RegisterList.Add(tmp_2)

            If Log_Register Then
                Log.WriteGameLog(Index_, "Register", "(None)", String.Format("Name: {0}, Password: {1}", Name, Password))
            End If
        End Sub

        Public Sub Login_WriteSpecialText(ByVal Text As String, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.LoginAuthInfo)
            writer.Byte(3) 'failed
            writer.Word(527) 'failed
            writer.Word(Text.Length)
            writer.String(Text) 'grund
            writer.Word(0)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Function CheckIfUserCanRegister(ByVal IP As String) As Boolean
            Dim Count As Integer = 0

            For i = 0 To RegisterList.Count - 1
                If RegisterList(i).IP = IP And Date.Now.DayOfYear = RegisterList(i).Time.DayOfYear Then
                    Count += 1
                End If
            Next

            If Count >= Max_RegistersPerDay Then
                Return False
            End If

            Return True
        End Function


        Structure Register_
            Public IP As String
            Public Name As String
            Public Password As String
            Public Time As Date
        End Structure
    End Module
End Namespace

