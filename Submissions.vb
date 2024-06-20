Public Class Submissions
    Public Property Name As String
    Public Property Email As String
    Public Property Phone As String
    Public Property GitHubLink As String
    Public Property StopwatchTime As String

    ' Constructor to initialize all properties
    Public Sub New(name As String, email As String, phone As String, githubLink As String, stopwatchTime As String)
        Me.Name = name
        Me.Email = email
        Me.Phone = phone
        Me.GitHubLink = githubLink
        Me.StopwatchTime = stopwatchTime
    End Sub
End Class
