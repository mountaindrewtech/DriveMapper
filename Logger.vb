' Copyright (c) 2025 Drew Schmidt
' Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
' See LICENSE for details.

Imports System.Diagnostics
Imports System.IO

Public Class Logger
    Private Const SourceName As String = "DriveMapper"
    Private Const LogName As String = "Application"
    Private Shared ReadOnly LogDirectory As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DriveMapper", "logs")
    Private Shared ReadOnly LogFilePath As String = Path.Combine(LogDirectory, "app.log")

    Public Shared Sub Info(message As String)
        WriteEntry(message, EventLogEntryType.Information)
    End Sub

    Public Shared Sub [Error](message As String)
        WriteEntry(message, EventLogEntryType.Error)
    End Sub

    Private Shared Sub WriteEntry(message As String, entryType As EventLogEntryType)
        If String.IsNullOrWhiteSpace(message) Then
            Return
        End If

        Try
            EnsureEventSource()
            EventLog.WriteEntry(SourceName, message, entryType)
        Catch ex As Exception
            WriteFallbackLog($"[{entryType}] {message} -- EventLog write failed: {ex.Message}")
        End Try
    End Sub

    Private Shared Sub EnsureEventSource()
        If EventLog.SourceExists(SourceName) Then
            Return
        End If

        Try
            EventLog.CreateEventSource(SourceName, LogName)
        Catch
            ' Ignore; fallback handles failures.
        End Try
    End Sub

    Private Shared Sub WriteFallbackLog(message As String)
        Try
            If Not Directory.Exists(LogDirectory) Then
                Directory.CreateDirectory(LogDirectory)
            End If

            Dim line = $"{DateTimeOffset.Now:u} {message}"
            File.AppendAllText(LogFilePath, line & Environment.NewLine)
        Catch
            ' Last resort: nothing else we can do.
        End Try
    End Sub
End Class
