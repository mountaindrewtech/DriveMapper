' Copyright (c) 2025 Drew Schmidt
' Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
' See LICENSE for details.

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Security.Principal

Public Class SecurityHelpers
    Private Const TOKEN_QUERY As UInteger = &H8UI
    Private Const TOKEN_ELEVATION As Integer = 20

    <DllImport("advapi32.dll", SetLastError:=True)>
    Private Shared Function OpenProcessToken(processHandle As IntPtr, desiredAccess As UInteger, ByRef tokenHandle As IntPtr) As Boolean
    End Function

    <DllImport("advapi32.dll", SetLastError:=True)>
    Private Shared Function GetTokenInformation(tokenHandle As IntPtr, tokenInformationClass As Integer, tokenInformation As IntPtr, tokenInformationLength As Integer, ByRef returnLength As Integer) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function CloseHandle(hObject As IntPtr) As Boolean
    End Function

    <DllImport("shell32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function ShellExecute(hwnd As IntPtr, lpOperation As String, lpFile As String, lpParameters As String, lpDirectory As String, nShowCmd As Integer) As IntPtr
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Private Structure TOKEN_ELEVATION_STRUCT
        Public TokenIsElevated As UInteger
    End Structure

    Public Shared Function IsMemberOfBuiltinAdministrators() As Boolean
        Try
            Dim identity = WindowsIdentity.GetCurrent()
            If identity Is Nothing Then
                Return False
            End If

            Dim principal = New WindowsPrincipal(identity)
            Dim adminSid = New SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, Nothing)
            Return principal.IsInRole(adminSid)
        Catch
            Return False
        End Try
    End Function

    Public Shared Function IsProcessElevated() As Boolean
        Dim processHandle = Process.GetCurrentProcess().Handle
        Dim tokenHandle As IntPtr = IntPtr.Zero

        If Not OpenProcessToken(processHandle, TOKEN_QUERY, tokenHandle) Then
            Return False
        End If

        Try
            Dim elevationSize = Marshal.SizeOf(Of TOKEN_ELEVATION_STRUCT)()
            Dim elevationPtr = Marshal.AllocHGlobal(elevationSize)
            Try
                Dim returned As Integer
                If Not GetTokenInformation(tokenHandle, TOKEN_ELEVATION, elevationPtr, elevationSize, returned) Then
                    Return False
                End If

                Dim elevation = Marshal.PtrToStructure(Of TOKEN_ELEVATION_STRUCT)(elevationPtr)
                Return elevation.TokenIsElevated <> 0
            Finally
                Marshal.FreeHGlobal(elevationPtr)
            End Try
        Finally
            CloseHandle(tokenHandle)
        End Try
    End Function

    Public Shared Function IsElevatedAdmin() As Boolean
        Return IsMemberOfBuiltinAdministrators() AndAlso IsProcessElevated()
    End Function

    Public Shared Sub RelaunchElevated(Optional args As String = "")
        Dim exePath = Process.GetCurrentProcess().MainModule?.FileName
        If String.IsNullOrWhiteSpace(exePath) Then
            Throw New InvalidOperationException("Unable to determine executable path for elevation.")
        End If

        Dim parameters = If(String.IsNullOrWhiteSpace(args), Nothing, args)
        Dim result = ShellExecute(IntPtr.Zero, "runas", exePath, parameters, Nothing, 1)
        Dim resultCode = result.ToInt64()

        If resultCode <= 32 Then
            Throw New Win32Exception(CInt(resultCode), "Failed to launch elevated process.")
        End If

        Environment.Exit(0)
    End Sub
End Class
