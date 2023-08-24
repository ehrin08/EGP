Imports PCSC

Public Class Scan
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

    Private Sub ScanOut_Tick(sender As Object, e As EventArgs) Handles ScanOut.Tick
        Try
            Dim context As New SCardContext()
            context.Establish(SCardScope.System)
            Dim readerName As String = GetReaderName(context)
            If readerName IsNot Nothing Then
                Dim cardNumber As String = ReadCardNumber(context, readerName)
                TextBox2.Text = cardNumber
                TapOut()
                context.Release()
            End If
        Catch ex As Exception
            Excp.Text = ex.Message
        End Try
    End Sub

    Private Sub Scan_Load(sender As Object, e As EventArgs) Handles Me.Load
        TimeDate.Start()
        ScanOut.Start()
        Marquee.Start()
        LoadScan()
        Excp.Text = ""
    End Sub

    Private Sub Timedate_Tick(sender As Object, e As EventArgs) Handles TimeDate.Tick
        Label3.Text = Date.Today.ToString("D")
        Label4.Text = DateTime.Now.ToString("h:mm:ss tt")
        Month.Text = Date.Today.ToString("MMMM")
        Bday.Text = Date.Today.ToString("MMMM-dd")
        HidDate.Text = Date.Today.ToString("MM-dd-yyyy")
        HidTime.Text = DateTime.Now.ToString("h:mm tt")
        TextBox1.Select()
    End Sub

    Private Sub Marquee_Tick(sender As Object, e As EventArgs) Handles Marquee.Tick
        Label5.Left -= 1
        If Label5.Right < 0 Then
            Label5.Left = Me.Width
        End If
    End Sub

    Private Sub GunaImageButton1_Click(sender As Object, e As EventArgs) Handles GunaImageButton1.Click
        Home.Show()
        Me.Close()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            TapIn()
            TextBox1.Text = ""
        End If
    End Sub


End Class