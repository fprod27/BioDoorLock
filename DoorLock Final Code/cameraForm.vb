Imports System.IO
Public Class cameraForm
    Public stimer As Integer = 30
    Const CAP As Short = &H400S
    Const CAP_DRIVER_CONNECT As Integer = CAP + 10
    Const CAP_DRIVER_DISCONNECT As Integer = CAP + 11
    Const CAP_EDIT_COPY As Integer = CAP + 30
    Const CAP_SET_PREVIEW As Integer = CAP + 50
    Const CAP_SET_PREVIEWRATE As Integer = CAP + 52
    Const CAP_SET_SCALE As Integer = CAP + 53
    Const WS_CHILD As Integer = &H40000000
    Const WS_VISIBLE As Integer = &H10000000
    Const SWP_NOMOVE As Short = &H2S
    Const SWP_NOSIZE As Short = 1
    Const SWP_NOZORDER As Short = &H4S
    Const HWND_BOTTOM As Short = 1
    Public timercounter = 1

    Dim iDevice As Integer = 0  ' Normal device ID 
    Dim hHwnd As Integer  ' Handle value to preview window
    Dim hHwnd2 As Integer  ' Handle value to preview window
    Dim doorlock As Boolean = False
    Dim appliances As Boolean = False
    ' Declare function from AVI capture DLL.

    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, _
         ByVal lParam As Object) As Integer

    Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" (ByVal hwnd As Integer, _
        ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, _
        ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    Declare Function DestroyWindow Lib "user32" (ByVal hndw As Integer) As Boolean

    Declare Function capCreateCaptureWindowA Lib "avicap32.dll" _
        (ByVal lpszWindowName As String, ByVal dwStyle As Integer, _
        ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, _
        ByVal nHeight As Short, ByVal hWndParent As Integer, _
        ByVal nID As Integer) As Integer


    Private Sub OpenForm()
        Dim iHeight As Integer = picCapture.Height
        Dim iWidth As Integer = picCapture.Width

        ' Open Preview window in picturebox .
        ' Create a child window with capCreateCaptureWindowA so you can display it in a picturebox.

        hHwnd = capCreateCaptureWindowA(iDevice, WS_VISIBLE Or WS_CHILD, 0, 0, 640, _
            480, picCapture.Handle.ToInt32, 0)
        ' Connect to device
        If SendMessage(hHwnd, CAP_DRIVER_CONNECT, iDevice, 0) Then

            ' Set the preview scale
            SendMessage(hHwnd, CAP_SET_SCALE, True, 0)

            ' Set the preview rate in milliseconds
            SendMessage(hHwnd, CAP_SET_PREVIEWRATE, 66, 0)

            ' Start previewing the image from the camera 
            SendMessage(hHwnd, CAP_SET_PREVIEW, True, 0)

            ' Resize window to fit in picturebox 
            SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, picCapture.Width, picCapture.Height, _
                                   SWP_NOMOVE Or SWP_NOZORDER)
        Else
            ' Error connecting to device close window 
            DestroyWindow(hHwnd)

        End If
    End Sub



    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Disconnect from device
        SendMessage(hHwnd, CAP_DRIVER_DISCONNECT, iDevice, 0)
        ' close window 
        DestroyWindow(hHwnd)
        Application.Exit()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connectDB()
        OpenForm()
        'EnrollFP.Show()                    'Enable this comment when the database is clean or empty
        'EnrollFP.userPosition = "Admin"
        If SerialPort1.IsOpen = 0 Then
            SerialPort1.Open()
            lblCOMSTATUS.Text = "CONNECTED"
            btnCOM.Text = "Disconnect"
        End If
    End Sub

    Private Sub picCapture_Click(sender As Object, e As EventArgs) Handles picCapture.Click
        OpenForm()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        EnrollFP.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        verifyFP.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles btnPreview.Click
        OpenForm()
    End Sub

    Private Sub btnCOM_Click(sender As Object, e As EventArgs) Handles btnCOM.Click
        Try
            If btnCOM.Text = "Connect" Then
                btnCOM.Text = "Disconnect"
                lblCOMSTATUS.Text = "CONNECTED"
                SerialPort1.PortName = ComboBox1.Text
                SerialPort1.Open()
            Else
                btnCOM.Text = "Connect"
                lblCOMSTATUS.Text = "DISCONNECTED"
                SerialPort1.Close()
            End If
        Catch ex As Exception
            lblCOMSTATUS.Text = "MISSING PORT"
        End Try
    End Sub

    Private Sub btnDoorControl_Click(sender As Object, e As EventArgs)
        Try
            If SerialPort1.IsOpen Then
                If doorlock Then
                    doorlock = False
                    SerialPort1.WriteLine("c")
                Else
                    doorlock = True
                    SerialPort1.WriteLine("o")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnAppliancesControl_Click(sender As Object, e As EventArgs)
        Try
            If SerialPort1.IsOpen Then
                If appliances Then
                    appliances = False
                    SerialPort1.WriteLine("1")
                Else
                    appliances = True
                    SerialPort1.WriteLine("0")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblDate.Text = Format(Now, "MM-dd-yy")
        lblTime.Text = Format(Now, "HH:mm:ss")
    End Sub

    Private Sub tmrControl_Tick(sender As Object, e As EventArgs) Handles tmrControl.Tick
        Process.Start("MotionDetect.exe")
        Try
            If Clipboard.GetText.Contains("1") Then
                stimer = 30
            End If
        Catch ex As Exception

        End Try

        If stimer <= 0 And lblFName.Text <> Nothing And lblFName.Text <> "NOT APPLICABLE" Then
            If SerialPort1.IsOpen Then
                clearTexts()
                SerialPort1.WriteLine("0")
                System.Threading.Thread.Sleep(500)
                SerialPort1.WriteLine("o")
                stimer = 0
                lblShutDownTimer.Text = "0"
            End If
        Else
            stimer = stimer - 1
            lblShutDownTimer.Text = stimer
        End If

    End Sub
    Private Sub clearTexts()
        saveLogs(lblFName.Text & " " & lblMName.Text & " " & lblLName.Text, "User was automatically Logged out")
        lblFName.Text = ""
        lblGender.Text = ""
        lblLoginTime.Text = ""
        lblLName.Text = ""
        lblMName.Text = ""
        lblPosition.Text = ""
    End Sub
    Private Sub lblClose_Click(sender As Object, e As EventArgs) Handles lblClose.Click
        Dim mejai = MsgBox("Are you sure you want to exit the program?", vbYesNo, "System Message")
        If mejai = MsgBoxResult.Yes Then
            PasswordBox.Show()
            ' Application.ExitThread()
        End If

    End Sub

    Private Sub btnAccount_Click(sender As Object, e As EventArgs) Handles btnAccount.Click
        'frmProgressbar.Show()
        verifyFP.Show()
        verifyFP.myfilter = "User"
        'verifyFP.Visible = False
    End Sub

    Private Sub btnAdminLogin_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
        verifyFP.Show()
        verifyFP.myfilter = "Admin"
    End Sub

    Private Sub picProfile_Click(sender As Object, e As EventArgs)
        viewLogs.Show()
    End Sub
End Class
