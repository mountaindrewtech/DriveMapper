' Copyright (c) 2025 Drew Schmidt
' Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
' See LICENSE for details.

Imports System.IO
Imports System.Text.Json

Public Class ProfilesStore
    Public Class Profile
        Public Property Name As String = String.Empty
        Public Property Unc As String = String.Empty
        Public Property DriveLetter As String = String.Empty
        Public Property UseCredentialManager As Boolean = True
        Public Property Domain As String = String.Empty
    End Class

    Private Shared ReadOnly ProfilesDirectory As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DriveMapper")
    Private Shared ReadOnly ProfilesPath As String = Path.Combine(ProfilesDirectory, "Profiles.json")

    Private Shared ReadOnly SerializerOptions As New JsonSerializerOptions With {
        .WriteIndented = True
    }

    Public Shared Function LoadProfiles() As List(Of Profile)
        Try
            If Not File.Exists(ProfilesPath) Then
                Return New List(Of Profile)()
            End If

            Using stream = File.OpenRead(ProfilesPath)
                Dim profiles = JsonSerializer.Deserialize(Of List(Of Profile))(stream, SerializerOptions)
                If profiles Is Nothing Then
                    Return New List(Of Profile)()
                End If
                For Each profile In profiles
                    If profile.Domain Is Nothing Then
                        profile.Domain = String.Empty
                    End If
                Next
                Return profiles
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"ProfilesStore.LoadProfiles failed: {ex}")
            Return New List(Of Profile)()
        End Try
    End Function

    Public Shared Sub SaveProfiles(profiles As IEnumerable(Of Profile))
        If profiles Is Nothing Then
            Throw New ArgumentNullException(NameOf(profiles))
        End If

        Try
            If Not Directory.Exists(ProfilesDirectory) Then
                Directory.CreateDirectory(ProfilesDirectory)
            End If

            Dim data = New List(Of Profile)()
            For Each profile In profiles
                If profile Is Nothing Then
                    Continue For
                End If

                If profile.Domain Is Nothing Then
                    profile.Domain = String.Empty
                Else
                    profile.Domain = profile.Domain.Trim()
                End If

                data.Add(profile)
            Next

            Using stream = File.Create(ProfilesPath)
                JsonSerializer.Serialize(stream, data, SerializerOptions)
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"ProfilesStore.SaveProfiles failed: {ex}")
        End Try
    End Sub
End Class
