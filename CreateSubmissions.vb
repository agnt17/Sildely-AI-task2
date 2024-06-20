Imports System.Diagnostics
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class CreateSubmissions
    Private stopwatch As New Stopwatch()
    Private client As New HttpClient()

    Private Sub CreateSubmissions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True 'This is used so that Keys Ctrl + N etc should work
        'Always use left Ctrl with other key combo so that it should work.
        Timer1.Interval = 1000 ' 1 second interval
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        TextBox5.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If stopwatch.IsRunning Then
            stopwatch.Stop()
            Button1.Text = "Start Stopwatch (CTRL + T)"
        Else
            stopwatch.Start()
            Button1.Text = "Stop Stopwatch (CTRL + T)"
        End If
    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim submission As New Submission(
        TextBox1.Text,
        TextBox2.Text,
        TextBox3.Text,
        TextBox4.Text,
        TextBox5.Text
    )
        Await SaveSubmission(submission)
    End Sub

    Private Async Function SaveSubmission(submission As Submission) As Task
        Try
            Dim jsonSubmission = JsonConvert.SerializeObject(submission)
            Console.WriteLine("JSON Payload: " & jsonSubmission)
            Dim content = New StringContent(jsonSubmission, Encoding.UTF8, "application/json")

            Dim response = Await client.PostAsync("http://localhost:7000/submit", content)

            If response.IsSuccessStatusCode Then
                MessageBox.Show("Submission saved successfully.")
            Else
                Dim responseContent = Await response.Content.ReadAsStringAsync()
                MessageBox.Show("Error saving submission1: " & response.StatusCode & " - " & responseContent)
            End If
        Catch ex As Exception
            MessageBox.Show("Caught Exception: " & ex.Message)
        End Try
    End Function




    Private Sub CreateSubmissions_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.T Then
            Button1.PerformClick()
        ElseIf e.Control AndAlso e.KeyCode = Keys.S Then
            Button2.PerformClick()
        End If
    End Sub
End Class
