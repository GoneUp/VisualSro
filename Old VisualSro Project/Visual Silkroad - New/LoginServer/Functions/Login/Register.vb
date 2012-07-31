Imports SRFramework
Imports LoginServer.Framework

Namespace Functions
    Module Register

        Public RegisterList As New List(Of Register_)
        Public Rand As New Random

        ''' <summary>
        ''' Registers a User
        ''' </summary>
        ''' <param name="Name"></param>
        ''' <param name="Password"></param>
        ''' <param name="Index_"></param>
        ''' <returns>True at success, False on fail.</returns>
        ''' <remarks></remarks>
        Public Function RegisterUser(ByVal Name As String, ByVal Password As String, ByVal Index_ As Integer)
            If Name.Contains("ä") Or Name.Contains("ü") Or Name.Contains("ö") Or Password.Contains("ä") Or Password.Contains("ü") Or Password.Contains("ö") Then
                LoginWriteSpecialText("Please don't enter any Chars like ä, ö, ü.", Index_)
                Return False
            End If



            Framework.Database.SaveQuery(String.Format("INSERT INTO users(username, password) VALUE ('{0}','{1}')", Name, Password))

            Dim tmp As New LoginDb.UserArray
            tmp.AccountId = Id_Gen.GetNewAccountId
            tmp.Name = Name
            tmp.Pw = Password
            tmp.FailedLogins = 0
            tmp.Banned = True
            tmp.BannReason = "Please wait for Activation."
            tmp.BannTime = Date.Now.AddMinutes(2)
            LoginDb.Users.Add(tmp)

            Dim tmp_2 As New Register_
            tmp_2.IP = ClientList.GetIP(Index_)
            tmp_2.Name = Name
            tmp_2.Password = Password
            tmp_2.Time = Date.Now
            RegisterList.Add(tmp_2)

            If Settings.Log_Register Then
                Log.WriteGameLog(Index_, "Register", "(None)", String.Format("Name: {0}, Password: {1}", Name, Password))
            End If

            Return True
        End Function

        Public Sub LoginWriteSpecialText(ByVal Text As String, ByVal Index_ As Integer)
            Dim writer As New PacketWriter
            writer.Create(ServerOpcodes.LOGIN_AUTH)
            writer.Byte(3) 'failed
            writer.Word(527) 'failed
            writer.Word(Text.Length)
            writer.String(Text) 'grund
            writer.Word(0)
            Server.Send(writer.GetBytes, Index_)
        End Sub

        Public Function CheckIfUserCanRegister(ByVal IP As String) As Boolean
            Dim count As Integer = 0

            For i = 0 To RegisterList.Count - 1
                If RegisterList(i).IP = IP And Date.Now.DayOfYear = RegisterList(i).Time.DayOfYear Then
                    count += 1
                End If
            Next

            If count >= Settings.Max_RegistersPerDay Then
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

