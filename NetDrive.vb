' Copyright (c) 2025 Drew Schmidt
' Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
' See LICENSE for details.

Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading

Public Class NetDrive
    Private Const RESOURCETYPE_DISK As Integer = &H1
    Private Const CONNECT_UPDATE_PROFILE As Integer = &H1
    Private Const CONNECT_COMMANDLINE As Integer = &H800
    Private Const CONNECT_CMD_SAVECRED As Integer = &H1000
    Private Const ERROR_MORE_DATA As Integer = 234
    Private Const ERROR_NOT_CONNECTED As Integer = 2250
    Private Const ERROR_TIMEOUT As Integer = 1460
    Private Const DEFAULT_BUFFER_SIZE As Integer = 512
    Private Const VERIFY_ATTEMPTS As Integer = 10
    Private Const VERIFY_DELAY_MS As Integer = 250

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

    <DllImport("mpr.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function WNetGetConnection(lpLocalName As String, lpRemoteName As StringBuilder, ByRef lpnLength As Integer) As Integer
    End Function

    Public Shared Function MapDrive(profile As ProfilesStore.Profile, Optional user As String = Nothing, Optional password As String = Nothing, Optional persist As Boolean = True, Optional useCredentialManager As Boolean = True) As (Success As Boolean, Message As String, Code As Integer)
        If profile Is Nothing Then
            Return (False, "Profile is required.", -1)
        End If

        Dim driveLetter = ProfileValidation.NormalizeDriveLetter(profile.DriveLetter)
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
        If useCredentialManager Then
            flags = flags Or CONNECT_CMD_SAVECRED
        End If

        Dim existing = CheckDriveMapping(driveLetter)
        If existing.IsMapped Then
            If PathsEqual(existing.RemotePath, profile.Unc) Then
                Logger.Info($"Drive {driveLetter} already maps to {profile.Unc}; reusing existing mapping.")
                MappingLogger.LogMappingAttempt(profile.Name, driveLetter, profile.Unc)
                MappingLogger.LogMappingResult(profile.Name, driveLetter, profile.Unc, True, 0)
                Return (True, $"Drive {driveLetter} is already mapped to {profile.Unc}.", 0)
            End If

            Logger.Info($"Drive {driveLetter} is currently mapped to {existing.RemotePath}; refusing to overwrite.")
            Return (False, $"Drive {driveLetter} is already mapped to {existing.RemotePath}. Disconnect it first.", 85)
        End If

        MappingLogger.LogMappingAttempt(profile.Name, driveLetter, profile.Unc)
        Dim result = WNetAddConnection2(netResource, password, user, flags)
        Dim wnetSucceeded = result = 0
        If Not wnetSucceeded Then
            ' If Windows thinks the drive is busy, double-check whether it's already mapped to the requested UNC.
            If result = 85 Then
                Dim afterFailure = CheckDriveMapping(driveLetter)
                If afterFailure.IsMapped AndAlso PathsEqual(afterFailure.RemotePath, profile.Unc) Then
                    Logger.Info($"Drive {driveLetter} reported as busy but is mapped to the requested UNC; treating as success.")
                    MappingLogger.LogMappingResult(profile.Name, driveLetter, profile.Unc, True, 0)
                    Return (True, $"Drive {driveLetter} is already mapped to {profile.Unc}.", 0)
                End If
            End If

            MappingLogger.LogMappingResult(profile.Name, driveLetter, profile.Unc, False, result)
            Return (False, GetFriendlyMessage(result), result)
        End If

        Dim verification = VerifyMappingState(driveLetter, profile.Unc)
        If verification.Success Then
            MappingLogger.LogMappingResult(profile.Name, driveLetter, profile.Unc, True, 0)
            Return (True, verification.Message, 0)
        End If

        Logger.Error($"Mapping verification failed for {driveLetter}: {verification.Message}")
        ' Attempt to clean up any partial mapping if verification failed.
        Dim cleanup = ForceCleanupDrive(driveLetter)
        If Not cleanup.Success Then
            Logger.Error($"Failed to clean up drive {driveLetter} after verification failure: {cleanup.Message}")
        End If

        MappingLogger.LogMappingResult(profile.Name, driveLetter, profile.Unc, False, verification.Code)
        Return (False, $"Mapping verification failed: {verification.Message}", verification.Code)
    End Function

    Public Shared Function UnmapDrive(profile As ProfilesStore.Profile, force As Boolean) As (Success As Boolean, Message As String, Code As Integer)
        If profile Is Nothing Then
            Return (False, "Profile is required.", -1)
        End If

        Dim driveLetter = ProfileValidation.NormalizeDriveLetter(profile.DriveLetter)
        If String.IsNullOrWhiteSpace(driveLetter) Then
            Return (False, "Drive letter is missing.", -1)
        End If

        Dim result = WNetCancelConnection2(driveLetter, CONNECT_UPDATE_PROFILE, force)
        If result = 0 Then
            If WaitForDriveRelease(driveLetter) Then
                Return (True, $"Unmapped {driveLetter}.", 0)
            End If

            Return (False, $"Windows reported success but {driveLetter} is still mapped.", ERROR_TIMEOUT)
        End If

        If result = ERROR_NOT_CONNECTED Then
            Return (True, $"{driveLetter} was already disconnected.", 0)
        End If

        Return (False, GetFriendlyMessage(result), result)
    End Function

    Public Shared Function ForceCleanupDrive(driveLetter As String) As (Success As Boolean, Message As String, Code As Integer)
        If String.IsNullOrWhiteSpace(driveLetter) Then
            Return (False, "Drive letter is missing.", -1)
        End If

        Dim normalized = ProfileValidation.NormalizeDriveLetter(driveLetter)
        If String.IsNullOrEmpty(normalized) Then
            Return (False, "Drive letter is invalid.", -1)
        End If

        Dim result = WNetCancelConnection2(normalized, CONNECT_UPDATE_PROFILE, True)
        If result = 0 Then
            If WaitForDriveRelease(normalized) Then
                Return (True, $"Drive {normalized} released.", 0)
            End If

            Return (False, $"Cleanup succeeded but {normalized} is still reported as mapped.", ERROR_TIMEOUT)
        End If

        If result = ERROR_NOT_CONNECTED Then
            Return (True, $"Drive {normalized} is already free.", 0)
        End If

        Return (False, GetFriendlyMessage(result), result)
    End Function

    Public Shared Function CheckDriveMapping(driveLetter As String) As (IsMapped As Boolean, RemotePath As String, Code As Integer)
        If String.IsNullOrWhiteSpace(driveLetter) Then
            Return (False, Nothing, -1)
        End If

        Dim normalized = ProfileValidation.NormalizeDriveLetter(driveLetter)
        If String.IsNullOrEmpty(normalized) Then
            Return (False, Nothing, -1)
        End If

        Dim remote = TryGetRemotePath(normalized)
        If remote.Success Then
            Return (True, remote.RemotePath, 0)
        End If

        If remote.Code = ERROR_NOT_CONNECTED Then
            Return (False, Nothing, 0)
        End If

        Return (False, Nothing, remote.Code)
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

    Private Shared Function VerifyMappingState(driveLetter As String, expectedUnc As String) As (Success As Boolean, Message As String, Code As Integer)
        Dim normalizedDrive = ProfileValidation.NormalizeDriveLetter(driveLetter)
        If String.IsNullOrEmpty(normalizedDrive) Then
            Return (False, "Drive letter is invalid.", -1)
        End If

        Dim normalizedExpected = NormalizeUnc(expectedUnc)

        For attempt = 1 To VERIFY_ATTEMPTS
            Dim remote = TryGetRemotePath(normalizedDrive)
            If remote.Success Then
                If PathsEqual(remote.RemotePath, normalizedExpected) Then
                    If WaitForDriveReady(normalizedDrive) Then
                        Return (True, $"Mapped {normalizedDrive} to {expectedUnc}.", 0)
                    End If

                    Return (False, $"Remote path matched but {normalizedDrive} is not ready.", ERROR_TIMEOUT)
                End If

                Return (False, $"Drive {normalizedDrive} is mapped to {remote.RemotePath} instead of {expectedUnc}.", 85)
            End If

            If remote.Code = ERROR_NOT_CONNECTED Then
                Thread.Sleep(VERIFY_DELAY_MS)
                Continue For
            End If

            Return (False, GetFriendlyMessage(remote.Code), remote.Code)
        Next

        Return (False, $"Timed out waiting for {driveLetter} to finalize.", ERROR_TIMEOUT)
    End Function

    Private Shared Function TryGetRemotePath(driveLetter As String) As (Success As Boolean, RemotePath As String, Code As Integer)
        If String.IsNullOrEmpty(driveLetter) Then
            Return (False, Nothing, -1)
        End If

        Dim bufferLength = DEFAULT_BUFFER_SIZE
        While bufferLength <= 16384
            Dim builder As New StringBuilder(bufferLength)
            Dim length = builder.Capacity
            Dim result = WNetGetConnection(driveLetter, builder, length)
            If result = 0 Then
                Return (True, builder.ToString(), 0)
            End If

            If result = ERROR_MORE_DATA Then
                bufferLength = length + 1
                Continue While
            End If

            Return (False, Nothing, result)
        End While

        Return (False, Nothing, ERROR_MORE_DATA)
    End Function

    Private Shared Function PathsEqual(first As String, second As String) As Boolean
        Dim normalizedFirst = NormalizeUnc(first)
        Dim normalizedSecond = NormalizeUnc(second)
        Return StringComparer.OrdinalIgnoreCase.Equals(normalizedFirst, normalizedSecond)
    End Function

    Private Shared Function NormalizeUnc(path As String) As String
        If String.IsNullOrWhiteSpace(path) Then
            Return String.Empty
        End If

        Return path.Trim().TrimEnd("\"c)
    End Function

    Private Shared Function WaitForDriveReady(driveLetter As String) As Boolean
        For attempt = 1 To VERIFY_ATTEMPTS
            Try
                Dim drive As New DriveInfo(driveLetter)
                If drive.IsReady Then
                    Return True
                End If
            Catch ex As IOException
                ' ignore and retry
            Catch ex As UnauthorizedAccessException
                ' ignore and retry
            End Try

            Thread.Sleep(VERIFY_DELAY_MS)
        Next

        Return False
    End Function

    Private Shared Function WaitForDriveRelease(driveLetter As String) As Boolean
        For attempt = 1 To VERIFY_ATTEMPTS
            Dim remote = TryGetRemotePath(driveLetter)
            If Not remote.Success AndAlso remote.Code = ERROR_NOT_CONNECTED Then
                Return True
            End If

            Thread.Sleep(VERIFY_DELAY_MS)
        Next

        Return False
    End Function
End Class
