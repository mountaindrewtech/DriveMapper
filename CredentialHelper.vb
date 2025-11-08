Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Text

Public Class CredentialHelper
    Private Const CRED_TYPE_GENERIC As Integer = 1
    Private Const CRED_PERSIST_LOCAL_MACHINE As Integer = 2
    Private Const ERROR_NOT_FOUND As Integer = 1168

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Private Structure CREDENTIAL
        Public Flags As Integer
        Public Type As Integer
        Public TargetName As IntPtr
        Public Comment As IntPtr
        Public LastWritten As FILETIME
        Public CredentialBlobSize As Integer
        Public CredentialBlob As IntPtr
        Public Persist As Integer
        Public AttributeCount As Integer
        Public Attributes As IntPtr
        Public TargetAlias As IntPtr
        Public UserName As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure FILETIME
        Public dwLowDateTime As Integer
        Public dwHighDateTime As Integer
    End Structure

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function CredWrite(ByRef Credential As CREDENTIAL, Flags As Integer) As Boolean
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function CredRead(TargetName As String, Type As Integer, Flags As Integer, ByRef Credential As IntPtr) As Boolean
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function CredDelete(TargetName As String, Type As Integer, Flags As Integer) As Boolean
    End Function

    <DllImport("advapi32.dll", SetLastError:=True)>
    Private Shared Sub CredFree(Buffer As IntPtr)
    End Sub

    Public Shared Sub SaveCredential(target As String, user As String, password As String)
        If String.IsNullOrWhiteSpace(user) Then
            Throw New ArgumentException("User is required", NameOf(user))
        End If

        If password Is Nothing Then
            Throw New ArgumentNullException(NameOf(password))
        End If

        Dim formattedTarget = BuildTargetName(target)

        Dim targetPtr = IntPtr.Zero
        Dim userPtr = IntPtr.Zero
        Dim passwordPtr = IntPtr.Zero

        Try
            targetPtr = Marshal.StringToCoTaskMemUni(formattedTarget)
            userPtr = Marshal.StringToCoTaskMemUni(user)

            Dim passwordBytes = Encoding.Unicode.GetBytes(password)
            If passwordBytes.Length > 0 Then
                passwordPtr = Marshal.AllocCoTaskMem(passwordBytes.Length)
                Marshal.Copy(passwordBytes, 0, passwordPtr, passwordBytes.Length)
            End If

            Dim cred As New CREDENTIAL With {
                .Type = CRED_TYPE_GENERIC,
                .TargetName = targetPtr,
                .UserName = userPtr,
                .CredentialBlobSize = passwordBytes.Length,
                .CredentialBlob = passwordPtr,
                .Persist = CRED_PERSIST_LOCAL_MACHINE
            }

            If Not CredWrite(cred, 0) Then
                Throw New Win32Exception(Marshal.GetLastWin32Error(), "Failed to save credential.")
            End If
        Finally
            If targetPtr <> IntPtr.Zero Then Marshal.FreeCoTaskMem(targetPtr)
            If userPtr <> IntPtr.Zero Then Marshal.FreeCoTaskMem(userPtr)
            If passwordPtr <> IntPtr.Zero Then Marshal.FreeCoTaskMem(passwordPtr)
        End Try
    End Sub

    Public Shared Function ReadCredential(target As String) As (User As String, Password As String)?
        Dim formattedTarget = BuildTargetName(target)
        Dim credPtr As IntPtr = IntPtr.Zero

        Try
            If Not CredRead(formattedTarget, CRED_TYPE_GENERIC, 0, credPtr) Then
                Dim errorCode = Marshal.GetLastWin32Error()
                If errorCode = ERROR_NOT_FOUND Then
                    Return Nothing
                End If

                Throw New Win32Exception(errorCode, "Failed to read credential.")
            End If

            Dim cred = CType(Marshal.PtrToStructure(credPtr, GetType(CREDENTIAL)), CREDENTIAL)
            Dim user = If(cred.UserName = IntPtr.Zero, String.Empty, Marshal.PtrToStringUni(cred.UserName))
            Dim password = ReadPasswordFromBlob(cred)

            Return (user, password)
        Finally
            If credPtr <> IntPtr.Zero Then
                CredFree(credPtr)
            End If
        End Try
    End Function

    Public Shared Sub DeleteCredential(target As String)
        Dim formattedTarget = BuildTargetName(target)
        If Not CredDelete(formattedTarget, CRED_TYPE_GENERIC, 0) Then
            Dim errorCode = Marshal.GetLastWin32Error()
            If errorCode = ERROR_NOT_FOUND Then
                Return
            End If
            Throw New Win32Exception(errorCode, "Failed to delete credential.")
        End If
    End Sub

    Private Shared Function BuildTargetName(target As String) As String
        If String.IsNullOrWhiteSpace(target) Then
            Throw New ArgumentException("Target is required", NameOf(target))
        End If

        Dim trimmed = target.Trim()
        If trimmed.StartsWith("DriveMapper_", StringComparison.OrdinalIgnoreCase) Then
            Return trimmed
        End If

        Return $"DriveMapper_{trimmed}"
    End Function

    Private Shared Function ReadPasswordFromBlob(cred As CREDENTIAL) As String
        If cred.CredentialBlob = IntPtr.Zero OrElse cred.CredentialBlobSize <= 0 Then
            Return String.Empty
        End If

        Dim data(cred.CredentialBlobSize - 1) As Byte
        Marshal.Copy(cred.CredentialBlob, data, 0, data.Length)
        Return Encoding.Unicode.GetString(data).TrimEnd(ChrW(0))
    End Function
End Class
