Public Class verifyFP
    Dim trial As Integer = 3
    Dim enableMe As Integer = 10
    Public myfilter As String = "Admin"

    Private Sub verifyFP_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
    End Sub
    Private Sub verifyFP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.Visible = False
        getFPT(ListBox1)
    End Sub


    Private Sub VerificationControl1_Load(sender As Object, e As EventArgs) Handles VerificationControl1.Load

    End Sub

    Private Sub VerificationControl1_OnComplete(Control As Object, FeatureSet As DPFP.FeatureSet, ByRef EventHandlerStatus As DPFP.Gui.EventHandlerStatus) Handles VerificationControl1.OnComplete
        Dim ver As New DPFP.Verification.Verification()
        Dim res As New DPFP.Verification.Verification.Result()
        Dim tmplate = New DPFP.Template()
        'Dim closeMe As Boolean = False
        For i = 0 To ListBox1.Items.Count - 1
            tmplate.DeSerialize(System.IO.File.ReadAllBytes(Application.StartupPath & "\FPT\" & ListBox1.Items(i).ToString))
            Try
                ver.Verify(FeatureSet, tmplate, res)
            Catch ex As Exception

            End Try
            Dim UserName As String = returnSpecific("tblUsers", "FPT", ListBox1.Items(i).ToString, "First_Name")
            Dim UserID As String = returnSpecific("tblUsers", "FPT", ListBox1.Items(i).ToString, "ID_Number")
            If res.Verified Then
                If myfilter = "Admin" And returnSpecific("tblUsers", "FPT", ListBox1.Items(i).ToString, "APosition") = "Admin" Then
                    Me.Hide()
                    adminName = UserName
                    adminForm.Show()
                    Exit For
                    Me.Close()
                ElseIf myfilter = "User" And returnSpecific("tblUsers", "FPT", ListBox1.Items(i).ToString, "APosition") = "User" Then
                    If cameraForm.stimer >= 1 Then
                        MsgBox("You are not allowed to enter the room, someone is currently in the room", MsgBoxStyle.Information, "System Message")
                    Else
                        With cameraForm
                            .lblFName.Text = returnSpecific("tblUsers", "FPT", ListBox1.Items(i).ToString, "First_Name")
                            .lblMName.Text = returnSpecific("tblUsers", "FPT", ListBox1.Items(i).ToString, "Middle_Name")
                            .lblLName.Text = returnSpecific("tblUsers", "FPT", ListBox1.Items(i).ToString, "Last_Name")
                            '.lblGender.Text = returnSpecific("tblUsers", "FPT", ListBox1.Items(i).ToString, "Gender")
                            .lblPosition.Text = returnSpecific("tblUsers", "FPT", ListBox1.Items(i).ToString, "APosition")
                            .lblLoginTime.Text = .lblTime.Text
                            saveLogs(.lblFName.Text & " " & .lblMName.Text & " " & .lblLName.Text, "Login as User")
                            cameraForm.stimer = 30
                            If .SerialPort1.IsOpen Then
                                .SerialPort1.WriteLine("c")
                                System.Threading.Thread.Sleep(500)
                                .SerialPort1.WriteLine("1")
                            End If
                            enableMe = 10
                            '.BackgroundImage = Image.FromFile("Photos/" & UserID & ".jpg")
                        End With
                    End If
                    Exit For
                Else
                    MsgBox("Your Account Position doesn't match your desired login priviledge.", MsgBoxStyle.Exclamation, "System Message")
                End If
               
            End If
        Next
        If Not res.Verified Then
            EventHandlerStatus = DPFP.Gui.EventHandlerStatus.Failure
            If trial < 1 Then
                MsgBox("Max Trial Reached. Please Wait for 30 Seconds", MsgBoxStyle.Critical, "System Message")
                Timer1.Enabled = True
                Me.Enabled = False
            Else
                MsgBox("User not Found", MsgBoxStyle.Critical, "System Message")
                trial = trial - 1
            End If

        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If enableMe < 1 Then
            enableMe = 10
            Me.Enabled = True
            trial = 3
            Timer1.Enabled = False
        Else
            enableMe = enableMe - 1
        End If
    End Sub

 
    Private Sub verifyFP_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'Me.Visible = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub
End Class