Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class Form1
    Private WithEvents Button4 As Button ' Declare Button4 with WithEvents

    Private client As HttpClient = New HttpClient()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Any initialization code you want to add when the form loads
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim viewSubmissionsForm As New ViewSubmissions()
        viewSubmissionsForm.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim createSubmissionForm As New CreateSubmissions()
        createSubmissionForm.ShowDialog()
    End Sub

    Private Async Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Handle delete button click (Button4)
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this submission?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Dim submissionIdToDelete As String = GetSubmissionIdToDelete() ' Implement logic to get submission ID to delete
            If Not String.IsNullOrEmpty(submissionIdToDelete) Then
                Await DeleteSubmissionAsync(submissionIdToDelete)
            End If
        End If
    End Sub

    Private Function GetSubmissionIdToDelete() As String
        ' Implement your logic to get the submission ID to delete
        ' For example, you can prompt the user for input or get it from another control
        ' Replace this with your actual implementation
        Dim submissionId As String = InputBox("Enter Submission ID to delete:", "Delete Submission", "")
        Return submissionId
    End Function

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
End Class
