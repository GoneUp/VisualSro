Namespace GameEdit
    Module GameEdit
        Public Sub WriteItemMallTables()
            'Clear the dable
            Database.SaveQuery("TRUNCATE item_mall")

            'Insert items into 'item_mall'
            For i = 0 To RefPackageItems.Count - 1
                Dim item As PackageItem = RefPackageItems(i)

                If item IsNot Nothing Then
                    If item.Payments.ContainsKey(MallPaymentEntry.PaymentDevices.Mall) Then
                        'Prevent Db Errors
                        item.NameReal = item.NameReal.Replace("'", "")
                        item.Description = item.Description.Replace("'", "")
                        'Make html compatible
                        item.Description = item.Description.Replace("<sml2>", "")
                        item.Description = item.Description.Replace("</sml2>", "")

                        If item.NameReal = "0" Then
                            item.NameReal = item.CodeName
                        End If

                        InsertMallItem(item)
                    End If
                End If
            Next
            Log.WriteSystemLog("Wrote IM Table!")
        End Sub

        Private Sub InsertMallItem(ByVal item As PackageItem)
            Database.SaveQuery(String.Format("INSERT into item_mall(code_name,  package_name, real_name, description, data, variance, price) " & _
                                               "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", _
                                               item.CodeName, item.PackageName, item.NameReal, item.Description, item.Data, item.Variance, item.Payments(MallPaymentEntry.PaymentDevices.Mall).Price))
        End Sub
    End Module
End Namespace
