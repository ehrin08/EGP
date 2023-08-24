

Public Class Admin_Panel
    Private Sub GunaImageButton1_Click(sender As Object, e As EventArgs) Handles GunaImageButton1.Click
        Home.Show()
        Me.Close()
    End Sub

    Private Sub Admin_Panel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SearchSection()
        Log()
        Students()
        LoadSettings()
    End Sub

    Private Sub GunaButton9_Click(sender As Object, e As EventArgs) Handles GunaButton9.Click
        Add_Student.ShowDialog()
        SearchSection()
    End Sub

    Private Sub GunaImageButton3_Click(sender As Object, e As EventArgs) Handles GunaImageButton3.Click
        Students()
        GunaTextBox2.Text = ""
        GunaComboBox1.SelectedIndex = 0
    End Sub

    Private Sub GunaDataGridView2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles GunaDataGridView2.CellDoubleClick
        If Label5.Text = 0 Then
            MessageBox.Show("No Selected Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            EditStudent()
            Edit_Student.ShowDialog()
            Students()
            Label5.Text = 0
        End If

    End Sub

    Private Sub GunaDataGridView2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles GunaDataGridView2.CellClick
        If e.RowIndex >= 0 AndAlso e.RowIndex < GunaDataGridView2.Rows.Count Then
            Dim selectedRow As DataGridViewRow = GunaDataGridView2.Rows(e.RowIndex)
            If selectedRow.Cells(0).Value IsNot Nothing Then
                Dim id As String = selectedRow.Cells(0).Value.ToString()
                Label5.Text = id
            Else
                Label5.Text = 0
            End If
        End If
    End Sub

    Private Sub GunaButton1_Click(sender As Object, e As EventArgs) Handles GunaButton1.Click
        If Label5.Text = 0 Then
            MessageBox.Show("No Selected Data", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            DeleteStudent()
            Students()
            Label5.Text = 0
        End If
    End Sub

    Private Sub GunaImageButton2_Click(sender As Object, e As EventArgs) Handles GunaImageButton2.Click
        SearchStudent()
    End Sub

    Private Sub GunaTextBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles GunaTextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            SearchStudent()
        End If
    End Sub

    Private Sub GunaComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GunaComboBox1.SelectedIndexChanged
        If GunaComboBox1.SelectedIndex = 0 Then
            Students()
        Else
            SectionFilter()
        End If
    End Sub

    Private Sub GunaButton2_Click(sender As Object, e As EventArgs) Handles GunaButton2.Click
        SaveSettings()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "PNG Files(*.Png)|*.png"
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            PictureBox3.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub GunaImageButton4_Click(sender As Object, e As EventArgs) Handles GunaImageButton4.Click
        CrystalReportViewer1.ReportSource = Application.StartupPath + "\CrystalReport1.rpt"
        CrystalReportViewer1.SelectionFormula = "{log1.Section} = '" & GunaComboBox2.Text & "' AND {log1.Month} = '" & GunaComboBox3.Text & "'"
        CrystalReportViewer1.Refresh()
        CrystalReportViewer1.RefreshReport()
    End Sub


End Class