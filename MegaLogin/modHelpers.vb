Imports System.IO

Module modHelpers

    Public LocalApplicationData As String = Environment.SpecialFolder.LocalApplicationData
    Public folder As String = Environment.GetFolderPath(LocalApplicationData)
    Public MEGAcmd As String = Path.Combine(folder, "MEGAcmd")

    Public Function CommandShell(ByVal arguments As String)
        Dim oProcess As New Process()
        Dim oStartInfo As New ProcessStartInfo("cmd.exe", arguments)

        With oStartInfo
            .UseShellExecute = False
            .RedirectStandardOutput = True
        End With

        With oProcess
            .StartInfo = oStartInfo
            .StartInfo.CreateNoWindow = True
            .Start()
        End With

        Dim sOutput As String
        Using oStreamReader As System.IO.StreamReader = oProcess.StandardOutput
            sOutput = oStreamReader.ReadToEnd()
        End Using

        Return sOutput
    End Function

    Private Function ExecuteCommand(ByVal command As String) As String
        Console.WriteLine("Executing: " & command)
        Return CommandShell(command)
    End Function

    Public Function CheckSession() As String
        Dim scriptPath As String = Path.Combine(MEGAcmd, "mega-session.bat")
        Dim command As String = String.Format("cd ""{0}"" & ""{1}""", MEGAcmd, scriptPath)
        Return ExecuteCommand(command)
    End Function

    Public Function LoginMegaAccount(ByVal Username As String, ByVal Password As String) As String
        Dim scriptPath As String = Path.Combine(MEGAcmd, "mega-login.bat")

        Dim command As String = String.Format("cd ""{0}"" & ""{1}"" ""{2}"" ""{3}""", MEGAcmd, scriptPath, Username, Password)
        Return ExecuteCommand(command)
    End Function

    Public Function LogoutMegaAccount() As String
        Dim scriptPath As String = Path.Combine(MEGAcmd, "mega-logout.bat")
        Dim command As String = String.Format("cd ""{0}"" & ""{1}""", MEGAcmd, scriptPath)
        Return ExecuteCommand(command)
    End Function
End Module
