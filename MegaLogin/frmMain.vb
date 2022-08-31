Imports System.IO

Public Class frmMain

    Private _folderPath As String
    Private _elapseTimerRunning As Boolean
    Private _elapseStartTime As DateTime

    Delegate Sub Set_Delegate(ByVal [ListBox] As ListBox, ByVal [Text] As String)

    Private Sub STS(ByVal [ListBox] As ListBox, ByVal [Text] As String)
        If [ListBox].InvokeRequired Then
            Dim MyDelegate As New Set_Delegate(AddressOf STS)
            Me.Invoke(MyDelegate, New Object() {[ListBox], [Text]})
        Else
            [ListBox].Items.Add([Text])
            [ListBox].TopIndex = [ListBox].Items.Count - 1
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = String.Format("{0} Version {1}", My.Application.Info.Title, My.Application.Info.Version.ToString)

        _elapseTimerRunning = False
        Timer1.Interval = 1000
        Timer1.Enabled = True
        txtElapsed.Text = String.Format("Time Elapsed {0} hr : {1} min : {2} sec", 0, 0, 0)
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim _files = My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.txt")

        For Each fileName As String In _files
            Dim name As String = My.Computer.FileSystem.GetFileInfo(fileName).Name

            STS(ListBox1, Now & vbTab & "Opening " & name)

            Dim line As String, words As String(), username As String, password As String
            Dim sr As StreamReader = My.Computer.FileSystem.OpenTextFileReader(fileName)

            Do
                line = sr.ReadLine

                If line Is Nothing Then Exit Do

                words = line.Split(New Char() {","c})

                If Trim(words(0)) = "" Then Exit Do

                checkSessionMega()

                username = words(0)
                password = words(1)

                STS(ListBox1, Now & vbTab & "Logging into the " & username & " account")

                LoginMegaAccount(username, password)

                STS(ListBox1, Now & vbTab & "--> " & username & " has been successfully logged in.")

                CheckSession()
            Loop Until line Is Nothing

            sr.Close()
        Next
    End Sub

    Public Sub checkSessionMega()
        Dim sOutput As String

        STS(ListBox1, Now & vbTab & "Checking the current session")

        sOutput = CheckSession()
        STS(ListBox1, Now & vbTab & "--> " & sOutput)

        If sOutput.Contains("Your (secret) session") Then
            logoutMega()
            STS(ListBox1, Now & vbTab & "--> Successfully logged out the current session")
        End If
    End Sub

    Public Sub logoutMega()
        STS(ListBox1, Now & vbTab & "--> " & LogoutMegaAccount())
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        _elapseTimerRunning = False

        Button1.Enabled = True
        Button2.Enabled = True

        STS(ListBox1, Now & vbTab & "Completed")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If BackgroundWorker1.IsBusy = False Then
            _elapseStartTime = DateTime.Now
            _elapseTimerRunning = True

            Button1.Enabled = False
            Button2.Enabled = False

            BackgroundWorker1.RunWorkerAsync()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If _elapseTimerRunning = True Then
            Dim elapsedtime = DateTime.Now.Subtract(_elapseStartTime)
            txtElapsed.Text = String.Format("Time Elapsed {0} hr : {1} min : {2} sec", elapsedtime.Hours, elapsedtime.Minutes, elapsedtime.Seconds)
        End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

    End Sub
End Class
