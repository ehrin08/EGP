Imports System.Reflection.Emit
Imports Guna.UI.WinForms
Imports PCSC
Public Class Add_Student
    Inherits Form
    Private Function GetReaderName(context As SCardContext) As String
        Dim readerNames() As String = context.GetReaders()
        If readerNames.Length > 0 Then
            Return readerNames(0)
        End If
        Return Nothing
    End Function

    Private Function ReadCardNumber(context As SCardContext, readerName As String) As String
        Dim cardNumber As String = Nothing

        Dim reader As New SCardReader(context)
        reader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.Any)

        Dim apduCommand() As Byte = {&HFF, &HCA, &H0, &H0, &H0}
        Dim apduResponse(256) As Byte
        Dim responseLength As Integer = apduResponse.Length

        Dim result As SCardError = reader.Transmit(apduCommand, apduResponse, responseLength)
        If result = SCardError.Success Then
            Dim responseBytes() As Byte = apduResponse.Take(responseLength).ToArray()
            cardNumber = BitConverter.ToString(responseBytes).Replace("-", "")
        End If

        reader.Disconnect(SCardReaderDisposition.Leave)
        Return cardNumber
    End Function
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            Dim context As New SCardContext()
            context.Establish(SCardScope.System)
            Dim readerName As String = GetReaderName(context)
            If readerName IsNot Nothing Then
                Dim cardNumber As String = ReadCardNumber(context, readerName)
                GunaTextBox2.ReadOnly = True
                GunaTextBox2.Text = cardNumber
            End If
            context.Release()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub GunaButton1_Click(sender As Object, e As EventArgs) Handles GunaButton1.Click
        AddStudent()
    End Sub

    Private Sub GunaButton3_Click(sender As Object, e As EventArgs) Handles GunaButton3.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "JPG Files(*.Jpg)|*.jpg"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox2.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub Add_Student_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()
    End Sub
End Class