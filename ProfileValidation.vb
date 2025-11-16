' Shared profile validation helpers.

Imports System.Text.RegularExpressions

Public Module ProfileValidation
    Private ReadOnly DomainPattern As New Regex("^[A-Za-z0-9\.-]+$", RegexOptions.Compiled)

    Public Function NormalizeDriveLetter(letter As String) As String
        If String.IsNullOrWhiteSpace(letter) Then
            Return String.Empty
        End If

        Dim trimmed = letter.Trim()
        If trimmed.Length = 1 Then
            Dim ch = trimmed(0)
            If Char.IsLetter(ch) Then
                Return $"{Char.ToUpperInvariant(ch)}:"
            End If
            Return String.Empty
        End If

        If trimmed.Length = 2 AndAlso trimmed(1) = ":"c AndAlso Char.IsLetter(trimmed(0)) Then
            Return $"{Char.ToUpperInvariant(trimmed(0))}:"
        End If

        Return String.Empty
    End Function

    Public Function IsValidDriveLetter(letter As String) As Boolean
        Return Not String.IsNullOrEmpty(NormalizeDriveLetter(letter))
    End Function

    Public Function IsValidUnc(path As String) As Boolean
        If String.IsNullOrWhiteSpace(path) Then
            Return False
        End If

        Dim trimmed = path.Trim()
        If trimmed.Length < 5 OrElse Not trimmed.StartsWith("\\") Then
            Return False
        End If

        Dim withoutPrefix = trimmed.Substring(2)
        Dim segments = withoutPrefix.Split(New Char() {"\"c}, StringSplitOptions.RemoveEmptyEntries)
        If segments.Length < 2 Then
            Return False
        End If

        Return segments(0).Length > 0 AndAlso segments(1).Length > 0
    End Function

    Public Function IsValidDomain(domain As String) As Boolean
        If String.IsNullOrWhiteSpace(domain) Then
            Return True
        End If

        Dim trimmed = domain.Trim()
        If trimmed.Length = 0 OrElse trimmed.Length > 255 Then
            Return False
        End If

        If trimmed.Contains(" "c) OrElse trimmed.Contains(vbTab) Then
            Return False
        End If

        Return DomainPattern.IsMatch(trimmed)
    End Function

    Public Function TryGetHostFromUnc(unc As String, ByRef host As String) As Boolean
        host = Nothing
        If Not IsValidUnc(unc) Then
            Return False
        End If

        Dim trimmed = unc.Trim()
        Dim withoutPrefix = trimmed.Substring(2)
        Dim segments = withoutPrefix.Split(New Char() {"\"c}, StringSplitOptions.RemoveEmptyEntries)
        If segments.Length >= 1 Then
            host = segments(0)
            Return True
        End If

        Return False
    End Function
End Module
