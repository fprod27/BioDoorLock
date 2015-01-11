Public Class PasswordBox

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "bestinthesis" Then
            Application.ExitThread()
        Else
            MsgBox("Wrong Master Key", MsgBoxStyle.Critical, "System Message")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class