Imports System.Net.Http
Imports Newtonsoft.Json

Public Class ViewSubmissions
    Private client As HttpClient = New HttpClient()
    Private submissions As New List(Of Submission)()
    Private currentIndex As Integer = 0
    Private currentSubmissionId As String = ""

    Private Async Sub ViewSubmissions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Await LoadSubmissionsAsync()
        DisplaySubmission()
    End Sub

    Private Async Function LoadSubmissionsAsync() As Task
        Try
            Dim response = Await client.GetAsync("http://localhost:7000/read")

            If response.IsSuccessStatusCode Then
                Dim responseData = Await response.Content.ReadAsStringAsync()
                submissions = JsonConvert.DeserializeObject(Of List(Of Submission))(responseData)
                DisplaySubmission()
            Else
                Dim errorMessage = $"Error loading submissions: {response.StatusCode}"
                MessageBox.Show(errorMessage)
            End If
        Catch ex As Exception
            MessageBox.Show($"Caught Exception: {ex.Message}")
        End Try
    End Function

    Private Sub DisplaySubmission()
        ClearSubmissionControls()

        If submissions.Count > 0 AndAlso currentIndex >= 0 AndAlso currentIndex < submissions.Count Then
            Dim submission = submissions(currentIndex)
            TextBox1.Text = submission.Name
            TextBox2.Text = submission.Email
            TextBox3.Text = submission.Phone
            TextBox4.Text = submission.GitHubLink
            TextBox5.Text = submission.StopwatchTime

            ' Store current submission ID
            currentSubmissionId = submission.Id
        End If
    End Sub

    Private Sub ClearSubmissionControls()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
    End Sub

    Private Async Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If String.IsNullOrEmpty(currentSubmissionId) Then
            MessageBox.Show("No submission to delete.")
            Return
        End If

        ' Send DELETE request to backend API
        Await DeleteSubmissionAsync(currentSubmissionId)

        ' Reload submissions after deletion
        Await LoadSubmissionsAsync()
    End Sub

    Private Async Function DeleteSubmissionAsync(submissionId As String) As Task
        Try
            Dim response = Await client.DeleteAsync($"http://localhost:7000/delete/{submissionId}")

            If response.IsSuccessStatusCode Then
                MessageBox.Show("Submission deleted successfully.")
            Else
                MessageBox.Show($"Error deleting submission: {response.StatusCode}")
            End If
        Catch ex As Exception
            MessageBox.Show($"Error deleting submission: {ex.Message}")
        End Try
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If currentIndex > 0 Then
            currentIndex -= 1
            DisplaySubmission()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If currentIndex < submissions.Count - 1 Then
            currentIndex += 1
            DisplaySubmission()
        End If
    End Sub

    Private Sub ViewSubmissions_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.P Then
            Button1.PerformClick()
        ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
            Button2.PerformClick()
        End If
    End Sub
End Class

Public Class Submission
    Public Property Id As String
    Public Property Name As String
    Public Property Email As String
    Public Property Phone As String
    Public Property GitHubLink As String
    Public Property StopwatchTime As String

    Public Sub New()
    End Sub

    Public Sub New(name As String, email As String, phone As String, githubLink As String, stopwatchTime As String)
        Me.Name = name
        Me.Email = email
        Me.Phone = phone
        Me.GitHubLink = githubLink
        Me.StopwatchTime = stopwatchTime
    End Sub
End Class
