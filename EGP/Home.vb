Public Class Home
    Private Sub GunaCheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles GunaCheckBox1.CheckedChanged
        If GunaCheckBox1.Checked Then
            GunaTextBox2.UseSystemPasswordChar = False
            GunaTextBox2.PasswordChar = ""
        Else
            GunaTextBox2.UseSystemPasswordChar = True
        End If
    End Sub
    Private Sub GunaTextBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles GunaTextBox2.KeyDown, GunaTextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Login()
        End If
    End Sub
    Private Sub GunaAdvenceButton1_Click(sender As Object, e As EventArgs) Handles GunaAdvenceButton1.Click
        Login()
    End Sub

    Private Sub GunaAdvenceTileButton1_Click(sender As Object, e As EventArgs) Handles GunaAdvenceTileButton1.Click
        Scan.Show()
        Me.Close()
    End Sub
End Class