Imports System.IO

Module modHelpers
    Public ReadOnly folder As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
    Public ReadOnly MEGAcmd As String = Path.Combine(folder, "MEGAcmd")

    Public Function CommandShell(ByVal arguments As String) As String
        Dim oProcess As New Process()
        Dim oStartInfo As New ProcessStartInfo("cmd.exe", "/C " & arguments) ' Use /C to execute and exit
        Dim output As String = ""

        With oStartInfo
            .UseShellExecute = False
            .RedirectStandardOutput = True
            .RedirectStandardError = True ' Capture errors
            .CreateNoWindow = True
        End With

        oProcess.StartInfo = oStartInfo

        Try
            oProcess.Start()
            output = oProcess.StandardOutput.ReadToEnd() & oProcess.StandardError.ReadToEnd() ' Capture both output and errors
            oProcess.WaitForExit()
        Catch ex As Exception
            output = "Error: " & ex.Message
        Finally
            oProcess.Dispose()
        End Try

        Return output
    End Function

    Private Function ExecuteCommand(ByVal command As String) As String
        Console.WriteLine("Executing: " & command)
        Return CommandShell(command)
    End Function

    Public Function CheckSession() As String
        Dim scriptPath As String = Path.Combine(MEGAcmd, "mega-session.bat")
        If Not File.Exists(scriptPath) Then Return "Error: mega-session.bat not found"

        Dim command As String = String.Format("cd /D ""{0}"" & ""{1}""", MEGAcmd, scriptPath)
        Return ExecuteCommand(command)
    End Function

    Public Function LoginMegaAccount(ByVal Username As String, ByVal Password As String) As String
        Dim scriptPath As String = Path.Combine(MEGAcmd, "mega-login.bat")
        If Not File.Exists(scriptPath) Then Return "Error: mega-login.bat not found"

        Dim command As String = String.Format("cd /D ""{0}"" & ""{1}"" ""{2}"" ""{3}""", MEGAcmd, scriptPath, Username, Password)
        Return ExecuteCommand(command)
    End Function

    Public Function LogoutMegaAccount() As String
        Dim scriptPath As String = Path.Combine(MEGAcmd, "mega-logout.bat")
        If Not File.Exists(scriptPath) Then Return "Error: mega-logout.bat not found"

        Dim command As String = String.Format("cd /D ""{0}"" & ""{1}""", MEGAcmd, scriptPath)
        Return ExecuteCommand(command)
    End Function
End Module
