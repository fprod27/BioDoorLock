Public Class viewLogs

    Private Sub viewLogs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        listLogs(DateTimePicker1, DateTimePicker2, lvLogs)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DateTimePicker1.Value = Now
        DateTimePicker2.Value = Now
    End Sub

    Private Sub DateTimePicker2_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker2.ValueChanged
        listLogs(DateTimePicker1, DateTimePicker2, lvLogs)
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        listLogs(DateTimePicker1, DateTimePicker2, lvLogs)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PrintForm1.Print()
    End Sub

    Private Sub lvLogs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvLogs.SelectedIndexChanged

    End Sub
End Class