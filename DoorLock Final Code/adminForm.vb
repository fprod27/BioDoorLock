Public Class adminForm

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        userPosition = "User"
        EnrollFP.Show()
    End Sub

    Private Sub btnAddmin_Click(sender As Object, e As EventArgs) Handles btnAddmin.Click
        userPosition = "Admin"
        EnrollFP.Show()
    End Sub

    Private Sub btnPrintLog_Click(sender As Object, e As EventArgs) Handles btnPrintLog.Click

    End Sub

    Private Sub btnShowLog_Click(sender As Object, e As EventArgs) Handles btnShowLog.Click
        viewLogs.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        viewUser.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class