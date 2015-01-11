Imports System.IO.File
Public Class EnrollFP
    Public userID As String = ""
    Dim closeCounter As Integer = 3
    Public userPosition As String = ""
    Private Sub EnrollmentControl_OnEnroll(Control As Object, FingerMask As Integer, Template As DPFP.Template, ByRef EventHandlerStatus As DPFP.Gui.EventHandlerStatus) Handles EnrollmentControl.OnEnroll
        userID = "DL" & Format(Now, "MMddyyHHmmss")
        Dim mew As Byte() = Nothing
        Template.Serialize(mew)
        If registerUser.IsHandleCreated Then
            registerUser.txtUserID.Text = userID
            registerUser.FPTB = mew
            Me.Hide()
            registerUser.Visible = True
            closeTimer.Enabled = True
            'Me.Close()
            'Me.Hide()
        End If
    End Sub

    Private Sub closeTimer_Tick(sender As Object, e As EventArgs) Handles closeTimer.Tick
        If closeCounter = 0 Then
            Me.Close()
        Else
            closeCounter -= 1
        End If
    End Sub

    Private Sub EnrollFP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If verifyFP.IsHandleCreated Then
            verifyFP.Hide()
        End If
        If userPosition = "Admin" Then
            registerUser.Text = "Register User as Admin"
        Else
            registerUser.Text = "Register User as User"
        End If
        registerUser.Show()
        registerUser.Visible = False
        If verifyFP.IsHandleCreated Then
            verifyFP.Dispose()
        End If
    End Sub

    Private Sub EnrollmentControl_Load(sender As Object, e As EventArgs) Handles EnrollmentControl.Load

    End Sub
End Class