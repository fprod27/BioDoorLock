Imports System.IO
Public Class registerUser
    Dim pSource As String = ""
    Public FPTB As Byte() = Nothing
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'EnrollFP.Show()
        If checkExistingRecord("tblUsers", "First_Name", txtFName.Text) And checkExistingRecord("tblUsers", "Middle_Name", txtFName.Text) And checkExistingRecord("tblUsers", "Last_Name", txtLName.Text) Then
            MsgBox("Invalid record details, duplicate entry", MsgBoxStyle.Critical, "Error Message")
        Else
            pDP.BackgroundImage.Save("Photos\" & txtUserID.Text & ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
            registerNewUser(txtUserID.Text, txtUserID.Text & ".fpt", txtFName.Text, txtMName.Text, txtLName.Text, txtPosition.Text)
            saveLogs(txtFName.Text & " " & txtMName.Text & " " & txtLName.Text, adminName & " registered this account as " & userPosition)
            IO.File.WriteAllBytes("FPT\" & txtUserID.Text & ".fpt", FPTB)
            If EnrollFP.IsHandleCreated Then
                EnrollFP.Dispose()
            End If
            MsgBox("User Successfully saved in the database", MsgBoxStyle.Information, "System Message")
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim mew = MsgBox("Are you sure you want to cancel?", MsgBoxStyle.YesNo, "System Message")
        If mew = MsgBoxResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        OpenFileDialog1.ShowDialog()
        Try
            pSource = OpenFileDialog1.FileName
            pDP.BackgroundImage = Image.FromFile(pSource)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub registerUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtUserID.Text = "DL" & Format(Now, "MMddyyHHmmss")
        txtPosition.Text = userPosition
    End Sub
End Class