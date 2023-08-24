Public Class Form1
    Dim splash As Integer = 0
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If splash >= 50 Then
            Home.Show()
            Me.Close()

        Else
            splash = splash + 1
        End If
    End Sub
End Class
