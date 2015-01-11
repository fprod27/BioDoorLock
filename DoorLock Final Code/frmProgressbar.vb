Partial Public Class frmProgressBar
    Inherits Form
    Private startTime As System.DateTime = DateTime.Now
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub lblClose_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Me.Tag = "Cancelled"
        Me.Hide()
    End Sub

    Public Sub SetThinkingBar([on] As Boolean)
        If [on] Then
            lblTime.Text = "0:00:00"
            startTime = DateTime.Now
            timer1.Enabled = True
            timer1.Start()
        Else
            timer1.Enabled = False
            timer1.[Stop]()
        End If
    End Sub

    Private Sub timer1_Tick(sender As Object, e As EventArgs)
        Dim diff = New TimeSpan()
        diff = DateTime.Now.Subtract(startTime)
        lblTime.Text = diff.Hours + ":" + diff.Minutes.ToString("00") + ":" + diff.Seconds.ToString("00")
        lblTime.Invalidate()
    End Sub
End Class