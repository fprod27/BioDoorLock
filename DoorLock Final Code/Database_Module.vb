Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Windows.Forms.DataGridView

Module DatabaseModule
    Public noIN As Integer = 0
    Dim conn As New OleDb.OleDbConnection
    Dim dbProvider As String = "Provider=Microsoft.ace.oledb.12.0;"
    'Dim dbPassword As String = "Jet OLEDB:Database Password=" & My.Settings.dbPassword & ";"
    Dim dbSource As String = "Data Source=Database\DBDL.accdb;"
    Dim dbSecurity As String = "Persist Security Info=False;"
    Public userPosition As String = "Admin"
    Public adminName As String = ""
    Public Sub getFPT(ByVal list1 As ListBox)
        ' make a reference to a directory
        Dim di As New IO.DirectoryInfo(Application.StartupPath & "\FPT")
        Dim diar1 As IO.FileInfo() = di.GetFiles()
        Dim dra As IO.FileInfo
        list1.Items.Clear()
        'list the names of all files in the specified directory
        For Each dra In diar1
            list1.Items.Add(dra)
        Next
    End Sub


    Public Sub connectDB()
        'conn.ConnectionString = dbProvider & dbPassword & dbSource
        conn.ConnectionString = dbProvider & dbSource
        Dim conec As New OleDbConnection(conn.ConnectionString)
        Try
            conn.Close()
            conn.Open()
        Catch ex As Exception
            MsgBox(ex.Message.ToString, , "Database Error")
        End Try
    End Sub

    Public Function insertSpecific(ByVal Table_Name As String, ByVal Field_Name As String, ByVal Value As String) As Boolean
        Dim returnValue As Boolean = False
        Try
            Dim insertData As String = "INSERT INTO " & Table_Name & " (" & Field_Name & ") VALUES ('" & Value & "')"
            Dim cmd As OleDbCommand = New OleDbCommand(insertData, conn)
            cmd.ExecuteNonQuery()
            returnValue = True
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "System Message")
        End Try
        Return returnValue
    End Function
    Public Function updateSpecific(ByVal Table_Name As String, ByVal Field_Name As String, ByVal Value As String, _
                              ByVal Comparator_Field As String, ByVal Comparator_Value As String, Optional _
                              ByVal Comparator_Field2 As String = "", Optional ByVal Comparator_Value2 As String = "") As Boolean
        Dim returnValue As Boolean = False
        Try
            Dim insertdata As String = ("Update [" & Table_Name & "] set " & Field_Name & " = '" & Value & "' where " & Comparator_Field & " = '" & Comparator_Value & "'; ")
            If Comparator_Field2 <> "" Then
                insertdata = "Update [" & Table_Name & "] set " & Field_Name & " = '" & Value & "' where " & Comparator_Field & " = '" & Comparator_Value & "' And " & Comparator_Field2 & " = '" & Comparator_Value2 & "'; "
            End If
            Dim cmd As OleDbCommand = New OleDbCommand(insertdata, conn)
            cmd.ExecuteNonQuery()
            returnValue = True
        Catch ex As Exception

        End Try
        Return returnValue
    End Function
    Public Sub deleteSpecific(ByVal TableName As String, ByVal ComparatorField1 As String, ByVal ComparatorValue1 As String, _
                                  Optional ByVal ComparatorField2 As String = "", Optional ByVal ComparatorValue2 As String = "")
        Dim deleteData As String = "Delete * From [" & TableName & "] Where " & ComparatorField1 & " = '" & ComparatorValue1 & "'"
        If ComparatorField2 <> "" Then
            deleteData &= "And " & ComparatorField2 & " = '" & ComparatorValue2 & "';"
        End If
        Dim cmd As OleDbCommand = New OleDbCommand(deleteData, conn)
        cmd.ExecuteNonQuery()
    End Sub
    Public Function returnSpecific(ByVal Table_Name As String, ByVal Field As String, ByVal Value As String, ByVal Return_Value As String) As String
        Dim returnvalue As String = ""
        Try
            Dim insertData As String = "Select * From [" & Table_Name & "] Where " & Field & " = '" & Value & "'"
            Dim cmd As OleDbCommand = New OleDbCommand(insertData, conn)
            cmd.ExecuteNonQuery()
            Dim sdr As OleDbDataReader = cmd.ExecuteReader()
            While sdr.Read
                returnvalue = sdr.Item(Return_Value)
            End While
            sdr.Close()
        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try
        Return returnvalue
    End Function
    Public Function checkExistingRecord(ByVal Table_Name As String, ByVal Field As String, ByVal Value As String) As Boolean
        Dim returnValue As Boolean = False
        Dim cnt As Integer = 0
        Try
            Dim selectData As String = "Select * From " & Table_Name & " Where " & Field & " = '" & Value & "';"
            Dim cmd As OleDbCommand = New OleDbCommand(selectData, conn)
            cmd.ExecuteNonQuery()
            Dim sdr As OleDbDataReader = cmd.ExecuteReader()
            While sdr.Read
                cnt += 1
            End While
            sdr.Close()
        Catch ex As Exception
            'MsgBox(ex.ToString, MsgBoxStyle.Information, "System Message")
        End Try
        If cnt > 0 Then
            returnValue = True
        End If
        Return returnValue
    End Function


    Public Sub listStudent(ByVal keyword As String, ByVal lview As ListView)
        Try
            Dim insertdata As String = ("Select * from [tblUsers] Where First_Name LIKE '%" & keyword & "%' AND User_Type = 'Student';")
            Dim cmd As OleDbCommand = New OleDbCommand(insertdata, conn)
            cmd.ExecuteNonQuery()
            Dim Datareader As OleDbDataReader = cmd.ExecuteReader
            lview.Items.Clear()
            While Datareader.Read
                Dim str(2) As String
                Dim itm As ListViewItem
                str(0) = Datareader.Item("First_Name") & " " & Datareader.Item("Middle_Name") & " " & Datareader.Item("Last_Name")
                str(1) = Datareader.Item("UserID")
                itm = New ListViewItem(str)
                lview.Items.Add(itm)
            End While
            Datareader.Close()
        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try
    End Sub

    Public Sub listLogs(ByVal dateb4 As DateTimePicker, ByVal dateAf As DateTimePicker, ByVal lview As ListView)
        Try
            Dim insertdata As String = ("Select * from [tblLogs];")
            Dim cmd As OleDbCommand = New OleDbCommand(insertdata, conn)
            cmd.ExecuteNonQuery()
            Dim Datareader As OleDbDataReader = cmd.ExecuteReader
            lview.Items.Clear()
            While Datareader.Read
                Dim str(4) As String
                Dim itm As ListViewItem
                str(0) = Datareader.Item("User_Involved")
                str(1) = Datareader.Item("Actions_Taken")
                str(2) = Datareader.Item("Date_Received")
                str(3) = Datareader.Item("Time_Received")
                itm = New ListViewItem(str)
                If DateTime.Parse(str(2)) >= dateb4.Value And DateTime.Parse(str(2)) <= dateAf.Value Then
                    lview.Items.Add(itm)
                    lview.FocusedItem = lview.Items.Item(lview.Items.Count - 1)
                    itm.EnsureVisible()
                End If
            End While
            Datareader.Close()
        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try
    End Sub

    Public Sub listUsers(ByVal keyword As String, ByVal lview As ListView)
        Try
            Dim insertdata As String = ("Select * from [tblUsers] Where First_Name LIKE '%" & keyword & "%';")
            Dim cmd As OleDbCommand = New OleDbCommand(insertdata, conn)
            cmd.ExecuteNonQuery()
            Dim Datareader As OleDbDataReader = cmd.ExecuteReader
            lview.Items.Clear()
            While Datareader.Read
                Dim str(4) As String
                Dim itm As ListViewItem
                str(0) = Datareader.Item("First_Name")
                str(1) = Datareader.Item("Middle_Name")
                str(2) = Datareader.Item("Last_Name")
                str(3) = Datareader.Item("ID_Number")
                itm = New ListViewItem(str)
                lview.Items.Add(itm)
            End While
            Datareader.Close()
        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try
    End Sub

    'Public Sub listMedicine(ByVal keyword As String, ByVal lview As ListView)
    '    Try
    '        Dim insertdata As String = ("Select * from [tblMedicine] Where Medicine_Name LIKE '%" & keyword & "%';")
    '        Dim cmd As OleDbCommand = New OleDbCommand(insertdata, conn)
    '        cmd.ExecuteNonQuery()
    '        Dim Datareader As OleDbDataReader = cmd.ExecuteReader
    '        lview.Items.Clear()
    '        While Datareader.Read
    '            Dim str(3) As String
    '            Dim itm As ListViewItem
    '            str(0) = Datareader.Item("Medicine_Name")
    '            str(1) = Datareader.Item("Expiration_Date")
    '            str(2) = Datareader.Item("Medicine_Count")
    '            itm = New ListViewItem(str)
    '            lview.Items.Add(itm)
    '        End While
    '        Datareader.Close()
    '    Catch ex As Exception
    '        'MsgBox(ex.ToString)
    '    End Try
    'End Sub


    Public Sub getIllness(ByVal combobox1 As ComboBox)
        Try
            Dim insertdata As String = ("Select * from [tblSickness];")
            Dim cmd As OleDbCommand = New OleDbCommand(insertdata, conn)
            cmd.ExecuteNonQuery()
            Dim Datareader As OleDbDataReader = cmd.ExecuteReader
            combobox1.Items.Clear()
            While Datareader.Read
                combobox1.Items.Add(Datareader.Item("Sickness"))
            End While
            Datareader.Close()
        Catch ex As Exception
            'MsgBox(ex.ToString)
        End Try
    End Sub
    'Public Function getHighestCount() As Integer
    '    Dim returnValue As Integer = MainForm.count + MainForm.send_Interval
    '    Dim highest As Integer = MainForm.count + MainForm.send_Interval
    '    Try
    '        Dim selectData As String = "Select * From tblQueue"
    '        Dim cmd As OleDbCommand = New OleDbCommand(selectData, conn)
    '        cmd.ExecuteNonQuery()
    '        Dim sdr As OleDbDataReader = cmd.ExecuteReader()
    '        While sdr.Read
    '            If highest < Val(sdr.Item("Corresponding_Count")) Then
    '                highest = Val(sdr.Item("Corresponding_Count"))
    '            End If
    '        End While
    '        returnValue = highest
    '        sdr.Close()
    '    Catch ex As Exception
    '        'MsgBox(ex.ToString, MsgBoxStyle.Information, "System Message")
    '    End Try
    '    Return returnValue
    'End Function


    Public Sub saveLogs(ByVal User_Involved As String, ByVal Actions_Taken As String)
        Try
            Dim insertData As String = "INSERT INTO tblLogs (User_Involved,Actions_Taken,Date_Received,Time_Received) VALUES ('" & _
                User_Involved & "','" & Actions_Taken & "','" & Date.Now.ToString("d") & "','" & Format(Now, "HH:mm:ss") & "')"
            Dim cmd As OleDbCommand = New OleDbCommand(insertData, conn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "System Message")
        End Try
    End Sub

    Public Sub registerNewUser(ByVal ID_Number As String, ByVal FPT As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal LastName As String, ByVal Position As String)
        Try
            Dim insertData As String = "INSERT INTO tblUsers (ID_Number,First_Name,Middle_Name,Last_Name," & _
                                       "APosition,FPT) VALUES ('" & ID_Number & "','" & _
                                       FirstName & "','" & MiddleName & "','" & LastName & "','" & Position & "','" & FPT & "')"
            Dim cmd As OleDbCommand = New OleDbCommand(insertData, conn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information, "System Message")
        End Try
    End Sub

End Module

