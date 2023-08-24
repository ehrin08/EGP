Imports System.Data.SqlClient
Imports System.Drawing.Imaging
Imports System.IO
Imports System.IO.Ports
Imports Guna.UI.WinForms
Imports MySql.Data.MySqlClient

Module SQL
    Dim conn As MySqlConnection = New MySqlConnection("server=localhost;user id=root;password=;database=egp_rfid;")
    Public reader As MySqlDataReader
    Dim cmd As MySqlCommand = conn.CreateCommand()

    Public Sub Login()
        conn.Open()
        cmd.CommandText = "SELECT * FROM accounts WHERE username='" & Home.GunaTextBox1.Text & "' AND password='" & Home.GunaTextBox2.Text & "'"
        reader = cmd.ExecuteReader
        If reader.HasRows Then
            MessageBox.Show("Login Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            conn.Close()
            Admin_Panel.Show()
            Home.Close()
        Else
            MessageBox.Show("Invalid Account!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Home.GunaTextBox1.Text = ""
            Home.GunaTextBox2.Text = ""
            conn.Close()
        End If
        reader.Close()
        conn.Close()
    End Sub

    Public Sub Log()
        Admin_Panel.GunaDataGridView1.Rows.Clear()
        conn.Open()
        cmd.CommandText = "SELECT * FROM log"
        reader = cmd.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                Admin_Panel.GunaDataGridView1.Rows.Add(reader("id1"), reader("id2"), reader("Name"), reader("Section"), reader("Date"), reader("Time_in"), reader("Time_out"))
            End While
        Else
            Admin_Panel.GunaDataGridView1.Rows.Clear()
        End If
        reader.Close()
        conn.Close()
    End Sub

    Public Sub Students()
        Admin_Panel.GunaDataGridView2.Rows.Clear()
        conn.Open()
        cmd.CommandText = "SELECT * FROM person"
        reader = cmd.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                Admin_Panel.GunaDataGridView2.Rows.Add(reader("id1"), reader("id2"), reader("Name"), reader("LRN"), reader("Section"), reader("Birthday"), reader("Contact"))
            End While
        Else
            Admin_Panel.GunaDataGridView2.Rows.Clear()
        End If
        reader.Close()
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub

    Public Sub AddStudent()
        conn.Open()
        Try
            Dim image As Image = Add_Student.PictureBox2.Image
            Dim ms As New MemoryStream()
            image.Save(ms, ImageFormat.Jpeg)
            Dim imageBytes As Byte() = ms.ToArray()
            cmd.CommandText = "INSERT INTO person (id1,id2,Name,LRN,Section,Birthday,Contact,Picture) VALUES (@id1,@id2,@Name,@LRN,@Section,@Birthday,@Contact,@photo)"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@id1", Add_Student.GunaTextBox1.Text)
            cmd.Parameters.AddWithValue("@id2", Add_Student.GunaTextBox2.Text)
            cmd.Parameters.AddWithValue("@Name", Add_Student.GunaTextBox3.Text)
            cmd.Parameters.AddWithValue("@LRN", Add_Student.GunaTextBox4.Text)
            cmd.Parameters.AddWithValue("@Section", Add_Student.GunaTextBox5.Text)
            cmd.Parameters.AddWithValue("@Birthday", Add_Student.GunaDateTimePicker1.Text)
            cmd.Parameters.AddWithValue("@Contact", Add_Student.GunaTextBox6.Text)
            cmd.Parameters.AddWithValue("@photo", imageBytes)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Student Added", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Add_Student.GunaTextBox1.Text = ""
            Add_Student.GunaTextBox2.Text = ""
            Add_Student.GunaTextBox3.Text = ""
            Add_Student.GunaTextBox4.Text = ""
            Add_Student.GunaTextBox5.Text = ""
            Add_Student.PictureBox2.Image = My.Resources.Picture
            Add_Student.GunaTextBox6.Text = ""
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        conn.Close()
        SearchSection()
    End Sub

    Public Sub EditStudent()
        Add_Student.GunaTextBox1.Text = ""
        Add_Student.GunaTextBox2.Text = ""
        Add_Student.GunaTextBox3.Text = ""
        Add_Student.GunaTextBox4.Text = ""
        Add_Student.GunaTextBox5.Text = ""
        Add_Student.PictureBox2.Image = My.Resources.Picture
        Add_Student.GunaTextBox6.Text = ""
        conn.Open()
        Try
            cmd.CommandText = "SELECT * FROM person where id1='" & Admin_Panel.Label5.Text & "'"
            reader = cmd.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    Edit_Student.GunaTextBox1.Text = reader("id1")
                    Edit_Student.GunaTextBox2.Text = reader("id2")
                    Edit_Student.GunaTextBox3.Text = reader("Name")
                    Edit_Student.GunaTextBox4.Text = reader("LRN")
                    Edit_Student.GunaTextBox5.Text = reader("Section")
                    Edit_Student.GunaDateTimePicker1.Value = reader("Birthday")
                    Edit_Student.GunaTextBox6.Text = reader("Contact")
                    Dim imageData As Byte() = DirectCast(reader("Picture"), Byte())
                    Dim ms As MemoryStream = New MemoryStream(imageData)
                    Dim image As Bitmap = New Bitmap(ms)
                    Edit_Student.PictureBox2.Image = image
                End While
            End If
            reader.Close()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        reader.Close()
        conn.Close()
    End Sub
    Public Sub UpdateStudent()
        conn.Open()
        Try
            Dim image As Image = Admin_Panel.PictureBox3.Image
            Dim ms As New MemoryStream()
            image.Save(ms, ImageFormat.Jpeg)
            Dim imageBytes As Byte() = ms.ToArray()
            cmd.CommandText = "UPDATE person SET Name='" & Edit_Student.GunaTextBox3.Text & "',LRN='" & Edit_Student.GunaTextBox4.Text & "',Section='" & Edit_Student.GunaTextBox5.Text & "',Birthday='" & Edit_Student.GunaDateTimePicker1.Text & "',Contact='" & Edit_Student.GunaTextBox3.Text & "',Picture=@photo WHERE id1='" & Edit_Student.GunaTextBox1.Text & "'"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@photo", imageBytes)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Data Saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        conn.Close()
        Edit_Student.Close()
    End Sub
    Public Sub DeleteStudent()
        conn.Open()
        If MsgBox("Are you sure you want to Delete?", MsgBoxStyle.OkCancel, "Confirmation") = MsgBoxResult.Ok Then
            cmd.CommandText = "DELETE FROM person WHERE id1='" & Admin_Panel.Label5.Text & "'"
            cmd.ExecuteNonQuery()
            MessageBox.Show("Deleting Record Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Records not Deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        conn.Close()
    End Sub
    Public Sub SearchSection()
        conn.Open()
        cmd.CommandText = "SELECT Section FROM person"
        reader = cmd.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                If Not Admin_Panel.GunaComboBox1.Items.Contains(reader("Section")) Then
                    Admin_Panel.GunaComboBox1.Items.Add(reader("Section"))
                End If
            End While
        End If
        reader.Close()
        cmd.CommandText = "SELECT Section FROM person"
        reader = cmd.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                If Not Admin_Panel.GunaComboBox2.Items.Contains(reader("Section")) Then
                    Admin_Panel.GunaComboBox2.Items.Add(reader("Section"))
                End If
            End While
        End If
        reader.Close()
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub

    Public Sub SectionFilter()
        Admin_Panel.GunaDataGridView2.Rows.Clear()
        conn.Open()
        cmd.CommandText = "SELECT * FROM person WHERE Section='" & Admin_Panel.GunaComboBox1.Text & "'"
        reader = cmd.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                Admin_Panel.GunaDataGridView2.Rows.Add(reader("id1"), reader("id2"), reader("Name"), reader("LRN"), reader("Section"), reader("Birthday"), reader("Contact"))
            End While
        Else
            Admin_Panel.GunaDataGridView2.Rows.Clear()
        End If
        reader.Close()
        conn.Close()
    End Sub

    Public Sub SearchStudent()
        Admin_Panel.GunaDataGridView2.Rows.Clear()
        conn.Open()
        If Admin_Panel.GunaComboBox1.SelectedIndex = 0 Then
            cmd.CommandText = "SELECT * FROM person WHERE id1='" & Admin_Panel.GunaTextBox2.Text & "' OR id2='" & Admin_Panel.GunaTextBox2.Text & "' OR Name='" & Admin_Panel.GunaTextBox2.Text & "' OR LRN='" & Admin_Panel.GunaTextBox2.Text & "' OR Contact='" & Admin_Panel.GunaTextBox2.Text & "'"
            reader = cmd.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    Admin_Panel.GunaDataGridView2.Rows.Add(reader("id1"), reader("id2"), reader("Name"), reader("LRN"), reader("Section"), reader("Birthday"), reader("Contact"))
                End While
                reader.Close()
                cmd.ExecuteNonQuery()
                conn.Close()
            Else
                MessageBox.Show("Records not Found!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                reader.Close()
                cmd.ExecuteNonQuery()
                conn.Close()
                Students()
                Admin_Panel.GunaTextBox2.Text = ""
                Admin_Panel.GunaComboBox1.SelectedIndex = 0
            End If
        Else
            cmd.CommandText = "SELECT * FROM person WHERE Section='" & Admin_Panel.GunaComboBox1.Text & "' AND id1='" & Admin_Panel.GunaTextBox2.Text & "' OR id2='" & Admin_Panel.GunaTextBox2.Text & "' OR Name='" & Admin_Panel.GunaTextBox2.Text & "' OR LRN='" & Admin_Panel.GunaTextBox2.Text & "' OR Contact='" & Admin_Panel.GunaTextBox2.Text & "'"
            reader = cmd.ExecuteReader
            If reader.HasRows Then
                While reader.Read
                    Admin_Panel.GunaDataGridView2.Rows.Add(reader("id1"), reader("id2"), reader("Name"), reader("LRN"), reader("Section"), reader("Birthday"), reader("Contact"))
                End While
                reader.Close()
                cmd.ExecuteNonQuery()
                conn.Close()
            Else
                MessageBox.Show("Records not Found!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                reader.Close()
                cmd.ExecuteNonQuery()
                conn.Close()
                Students()
                Admin_Panel.GunaTextBox2.Text = ""
                Admin_Panel.GunaComboBox1.SelectedIndex = 0
            End If
        End If
    End Sub
    Public Sub LoadSettings()
        conn.Open()
        Try
            cmd.CommandText = "SELECT * FROM settings WHERE id=1"
            reader = cmd.ExecuteReader()
            If reader.HasRows Then
                While reader.Read
                    Admin_Panel.GunaTextBox1.Text = reader("Heading")
                    Admin_Panel.GunaTextBox3.Text = reader("Subheading")
                    Admin_Panel.GunaTextBox4.Text = reader("Announcement")
                    Dim imageData As Byte() = DirectCast(reader("Logo"), Byte())
                    Dim ms As MemoryStream = New MemoryStream(imageData)
                    Dim image As Bitmap = New Bitmap(ms)
                    Admin_Panel.PictureBox3.Image = image
                End While
            End If
            reader.Close()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        conn.Close()
    End Sub

    Public Sub SaveSettings()
        conn.Open()
        Try
            Dim image As Image = Admin_Panel.PictureBox3.Image
            Dim ms As New MemoryStream()
            image.Save(ms, ImageFormat.Jpeg)
            Dim imageBytes As Byte() = ms.ToArray()
            cmd.CommandText = "UPDATE settings SET Heading='" & Admin_Panel.GunaTextBox1.Text & "',Subheading='" & Admin_Panel.GunaTextBox3.Text & "',Announcement='" & Admin_Panel.GunaTextBox4.Text & "',Logo=@photo WHERE id=1"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@photo", imageBytes)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Saved Successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        conn.Close()
    End Sub
    Public Sub LoadScan()
        conn.Open()
        Try
            cmd.CommandText = "SELECT * FROM settings WHERE id=1"
            reader = cmd.ExecuteReader()
            If reader.HasRows Then
                While reader.Read
                    Scan.Label1.Text = reader("Heading")
                    Scan.Label2.Text = reader("Subheading")
                    Scan.Announcement.Text = reader("Announcement")
                    Dim imageData As Byte() = DirectCast(reader("Logo"), Byte())
                    Dim ms As MemoryStream = New MemoryStream(imageData)
                    Dim image As Bitmap = New Bitmap(ms)
                    Scan.GunaCirclePictureBox1.Image = image
                End While
            End If
            reader.Close()
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        conn.Close()
    End Sub

    Public Sub TapIn()
        Scan.Id1In.Text = ""
        Scan.Id2In.Text = ""
        Scan.NumberIn.Text = ""
        Scan.BdayIn.Text = ""
        Scan.NameIn.Text = ""
        Scan.SectionIn.Text = ""
        Scan.PictureBox3.Image = My.Resources.Picture
        conn.Open()
        Try
            Scan.TimeIn.Text = Scan.HidTime.Text
            cmd.CommandText = "SELECT * FROM person WHERE id1='" & Scan.TextBox1.Text & "'"
            reader = cmd.ExecuteReader()
            If reader.Read() Then
                Dim imageData As Byte() = DirectCast(reader("Picture"), Byte())
                Dim ms As MemoryStream = New MemoryStream(imageData)
                Dim image As Bitmap = New Bitmap(ms)
                Scan.PictureBox3.Image = image
                Scan.Id1In.Text = reader("id1")
                Scan.Id2In.Text = reader("id2")
                Scan.NumberIn.Text = reader("Contact")
                Scan.BdayIn.Text = reader("Birthday")
                Scan.NameIn.Text = reader("Name")
                Scan.SectionIn.Text = reader("Section")
            End If
            reader.Close()
            cmd.ExecuteNonQuery()
            Scan.GunaLinePanelIn.Show()
            Scan.Excp.Text = ""
        Catch ex As Exception
            Scan.Excp.Text = ex.Message
        End Try

        cmd = conn.CreateCommand()
        Try
            cmd.CommandText = "SELECT * FROM log WHERE id1='" & Scan.Id1In.Text & "' AND id2='" & Scan.Id2In.Text & "'  AND Date='" & Scan.HidDate.Text & "'"
            reader = cmd.ExecuteReader()
            If reader.Read Then
                reader.Close()
                cmd = conn.CreateCommand
                cmd.CommandText = "UPDATE log SET id1='" & Scan.Id1In.Text & "',id2='" & Scan.Id2In.Text & "',Name='" & Scan.NameIn.Text & "',Section='" & Scan.SectionIn.Text & "',Month='" & Scan.Month.Text & "',Date='" & Scan.HidDate.Text & "',Time_in='" & Scan.HidTime.Text & "' WHERE id1='" & Scan.Id1In.Text & "' AND id2='" & Scan.Id2In.Text & "'"
                cmd.ExecuteNonQuery()
            Else
                reader.Close()
                cmd = conn.CreateCommand
                cmd.CommandText = "INSERT INTO log (id1,id2,Name,Section,Month,Date,Time_in) VALUES ('" & Scan.Id1In.Text & "','" & Scan.Id2In.Text & "','" & Scan.NameIn.Text & "','" & Scan.SectionIn.Text & "','" & Scan.Month.Text & "','" & Scan.HidDate.Text & "','" & Scan.HidTime.Text & "')"
                cmd.ExecuteNonQuery()
            End If
            Scan.Excp.Text = ""
        Catch ex As Exception
            Scan.Excp.Text = ex.Message
        End Try

        If Scan.BdayIn.Text = Scan.Bday.Text Then
            Scan.HBD1.Show()
        Else
            Scan.HBD1.Hide()
        End If


        Try
            Dim port As New SerialPort(Scan.Label6.Text, 115200)
            port.Open()
            port.Write("AT+CMGF=1" & vbCrLf)
            port.Write("AT+CMGS=""" & Scan.NumberIn.Text & """" & vbCrLf)
            Dim messageout As String = "Good Day!" & vbCrLf & Scan.NameIn.Text + " has entered the school campus" & vbCrLf & Scan.Label3.Text & vbCrLf & Scan.HidTime.Text & vbCrLf & vbCrLf & "PLEASE DO NOT REPLY"
            port.Write(messageout & Chr(26))
            port.Close()
            Scan.Excp.Text = ""
        Catch ex As Exception
            Scan.Excp.Text = "GSM Modem not found."
        End Try
        conn.Close()
    End Sub
    Public Sub TapOut()
        Scan.Id1Out.Text = ""
        Scan.Id2Out.Text = ""
        Scan.NumberOut.Text = ""
        Scan.BdayOut.Text = ""
        Scan.NameOut.Text = ""
        Scan.SectionOut.Text = ""
        Scan.PictureBox4.Image = My.Resources.Picture
        conn.Open()
        Try
            Scan.TimeOut.Text = Scan.HidTime.Text
            cmd.CommandText = "SELECT * FROM person WHERE id2='" & Scan.TextBox2.Text & "'"
            reader = cmd.ExecuteReader()
            If reader.Read() Then
                Dim imageData As Byte() = DirectCast(reader("Picture"), Byte())
                Dim ms As MemoryStream = New MemoryStream(imageData)
                Dim image As Bitmap = New Bitmap(ms)
                Scan.PictureBox4.Image = image
                Scan.Id1Out.Text = reader("id1")
                Scan.Id2Out.Text = reader("id2")
                Scan.NumberOut.Text = reader("Contact")
                Scan.BdayOut.Text = reader("Birthday")
                Scan.NameOut.Text = reader("Name")
                Scan.SectionOut.Text = reader("Section")
            End If
            reader.Close()
            cmd.ExecuteNonQuery()
            Scan.GunaLinePanelOut.Show()
            Scan.Excp.Text = ""
        Catch ex As Exception
            Scan.Excp.Text = ex.Message
        End Try

        cmd = conn.CreateCommand()
        Try
            cmd.CommandText = "SELECT * FROM log WHERE id1='" & Scan.Id1Out.Text & "' AND id2='" & Scan.Id2Out.Text & "'  AND Date='" & Scan.HidDate.Text & "'"
            reader = cmd.ExecuteReader()
            If reader.Read Then
                reader.Close()
                cmd = conn.CreateCommand
                cmd.CommandText = "UPDATE log SET id1='" & Scan.Id1Out.Text & "',id2='" & Scan.Id2Out.Text & "',Name='" & Scan.NameOut.Text & "',Section='" & Scan.SectionOut.Text & "',Month='" & Scan.Month.Text & "',Date='" & Scan.HidDate.Text & "',Time_out='" & Scan.HidTime.Text & "' WHERE id1='" & Scan.Id1Out.Text & "' AND id2='" & Scan.Id2Out.Text & "'"
                cmd.ExecuteNonQuery()
            Else
                reader.Close()
                cmd = conn.CreateCommand
                cmd.CommandText = "INSERT INTO log (id1,id2,Name,Section,Month,Date,Time_in) VALUES ('" & Scan.Id1Out.Text & "','" & Scan.Id2Out.Text & "','" & Scan.NameOut.Text & "','" & Scan.SectionOut.Text & "','" & Scan.Month.Text & "','" & Scan.HidDate.Text & "','" & Scan.HidTime.Text & "')"
                cmd.ExecuteNonQuery()
            End If
            Scan.Excp.Text = ""
        Catch ex As Exception
            Scan.Excp.Text = ex.Message
        End Try

        If Scan.BdayIn.Text = Scan.Bday.Text Then
            Scan.HBD2.Show()
        Else
            Scan.HBD2.Hide()
        End If


        Try
            Dim port As New SerialPort(Scan.Label6.Text, 115200)
            port.Open()
            port.Write("AT+CMGF=1" & vbCrLf)
            port.Write("AT+CMGS=""" & Scan.NumberOut.Text & """" & vbCrLf)
            Dim messageout As String = "Good Day!" & vbCrLf & Scan.NameOut.Text + " has left the school campus" & vbCrLf & Scan.Label3.Text & vbCrLf & Scan.HidTime.Text & vbCrLf & vbCrLf & "PLEASE DO NOT REPLY"
            port.Write(messageout & Chr(26))
            port.Close()
        Catch ex As Exception
            Scan.Excp.Text = "GSM Modem not found."
        End Try
        conn.Close()
        Scan.TextBox2.Text = ""
    End Sub
End Module
