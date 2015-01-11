Public Class viewUser

    Private Sub viewUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        listUsers("", Me.lvUsers)
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        listUsers(txtSearch.Text, Me.lvUsers)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim result = MsgBox("Are you sure you want to delete this account", MsgBoxStyle.YesNo, "System Message")
        If result = MsgBoxResult.Yes Then
            deleteSpecific("tblUsers", "ID_Number", lvUsers.FocusedItem.SubItems(3).Text)
            saveLogs(lvUsers.FocusedItem.SubItems(0).Text & " " & lvUsers.FocusedItem.SubItems(1).Text & " " & _
                     lvUsers.FocusedItem.SubItems(2).Text, adminName & " deleted this account")
            MsgBox("User Successfully Deleted", MsgBoxStyle.Information, "System Message")
            listUsers("", Me.lvUsers)
        End If
    End Sub
End Class