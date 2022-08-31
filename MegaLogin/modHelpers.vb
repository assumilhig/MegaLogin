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

    Public Function CheckSession()
        Return CommandShell("/c cd " & MEGAcmd & " & " & MEGAcmd & "\mega-session.bat")
    End Function

    Public Function LoginMegaAccount(ByVal Username As String, ByVal Password As String)
        Return CommandShell("/c cd " & MEGAcmd & " & " & MEGAcmd & "\mega-login.bat " & Username & " " & Password)
    End Function

    Public Function LogoutMegaAccount()
        Return CommandShell("/c cd " & MEGAcmd & " & " & MEGAcmd & "\mega-logout.bat")
    End Function
End Module
