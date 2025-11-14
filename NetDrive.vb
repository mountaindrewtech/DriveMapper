' Copyright (c) 2025 Drew Schmidt
' Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
' See LICENSE for details.

Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class NetDrive
    Private Const RESOURCETYPE_DISK As Integer = &H1
    Private Const CONNECT_UPDATE_PROFILE As Integer = &H1
    Private Const CONNECT_COMMANDLINE As Integer = &H800
    Private Const CONNECT_CMD_SAVECRED As Integer = &H1000

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Private Structure NETRESOURCE
        Public dwScope As Integer
        Public dwType As Integer
        Public dwDisplayType As Integer
        Public dwUsage As Integer
        Public lpLocalName As String
        Public lpRemoteName As String
        Public lpComment As String
        Public lpProvider As String
    End Structure

    <DllImport("mpr.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function WNetAddConnection2(ByRef lpNetResource As NETRESOURCE, lpPassword As String, lpUserName As String, dwFlags As Integer) As Integer
    End Function

    <DllImport("mpr.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function WNetCancelConnection2(lpName As String, dwFlags As Integer, fForce As Boolean) As Integer
    End Function

    Public Shared Function MapDrive(profile As ProfilesStore.Profile, Optional user As String = Nothing, Optional password As String = Nothing, Optional persist As Boolean = True) As (Success As Boolean, Message As String, Code As Integer)
        If profile Is Nothing Then
            Return (False, "Profile is required.", -1)
        End If

        Dim driveLetter = NormalizeDriveLetter(profile.DriveLetter)
        If String.IsNullOrWhiteSpace(driveLetter) Then
            Return (False, "Drive letter is missing.", -1)
        End If

        If String.IsNullOrWhiteSpace(profile.Unc) Then
            Return (False, "UNC path is missing.", -1)
        End If

        Dim netResource As New NETRESOURCE With {
            .dwType = RESOURCETYPE_DISK,
            .lpLocalName = driveLetter,
            .lpRemoteName = profile.Unc
        }

        Dim flags = CONNECT_COMMANDLINE
        If persist Then
            flags = flags Or CONNECT_UPDATE_PROFILE
        End If
        If profile.UseCredentialManager Then
            flags = flags Or CONNECT_CMD_SAVECRED
        End If

        Dim result = WNetAddConnection2(netResource, password, user, flags)
        If result = 0 Then
            Return (True, $"Mapped {driveLetter} to {profile.Unc}.", result)
        End If

        Return (False, GetFriendlyMessage(result), result)
    End Function

    Public Shared Function UnmapDrive(profile As ProfilesStore.Profile, force As Boolean) As (Success As Boolean, Message As String, Code As Integer)
        If profile Is Nothing Then
            Return (False, "Profile is required.", -1)
        End If

        Dim driveLetter = NormalizeDriveLetter(profile.DriveLetter)
        If String.IsNullOrWhiteSpace(driveLetter) Then
            Return (False, "Drive letter is missing.", -1)
        End If

        Dim result = WNetCancelConnection2(driveLetter, CONNECT_UPDATE_PROFILE, force)
        If result = 0 Then
            Return (True, $"Unmapped {driveLetter}.", result)
        End If

        Return (False, GetFriendlyMessage(result), result)
    End Function

    Private Shared Function NormalizeDriveLetter(letter As String) As String
        If String.IsNullOrWhiteSpace(letter) Then
            Return Nothing
        End If

        Dim trimmed = letter.Trim()
        If trimmed.Length = 1 Then
            Return trimmed & ":"
        End If

        If trimmed.EndsWith(":") Then
            Return trimmed
        End If

        Return trimmed & ":"
    End Function

    Private Shared Function GetFriendlyMessage(code As Integer) As String
        Select Case code
            Case 5
                Return "Access denied. Check permissions or credentials."
            Case 53
                Return "The network path was not found. Verify the server/share name."
            Case 67
                Return "The network name cannot be found. Ensure the UNC path is correct."
            Case 85
                Return "The drive letter is already in use. Disconnect or pick another letter."
            Case 86
                Return "The specified network password is incorrect."
            Case 1219
                Return "Multiple connections with different credentials are not allowed. Disconnect existing mappings."
            Case Else
                Return New Win32Exception(code).Message
        End Select
    End Function
End Class
