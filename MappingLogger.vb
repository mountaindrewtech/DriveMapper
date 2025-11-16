' Centralized mapping telemetry helper.

Public Module MappingLogger
    Public Sub LogMappingAttempt(profileName As String, driveLetter As String, unc As String)
        Dim safeProfile = If(String.IsNullOrWhiteSpace(profileName), "(unnamed)", profileName)
        Dim safeDrive = If(String.IsNullOrWhiteSpace(driveLetter), "(none)", driveLetter)
        Dim safeUnc = If(String.IsNullOrWhiteSpace(unc), "(none)", unc)
        Logger.Info($"Mapping attempt: profile '{safeProfile}', drive {safeDrive}, UNC {safeUnc}.")
    End Sub

    Public Sub LogMappingResult(profileName As String, driveLetter As String, unc As String, success As Boolean, resultCode As Integer)
        Dim safeProfile = If(String.IsNullOrWhiteSpace(profileName), "(unnamed)", profileName)
        Dim safeDrive = If(String.IsNullOrWhiteSpace(driveLetter), "(none)", driveLetter)
        Dim safeUnc = If(String.IsNullOrWhiteSpace(unc), "(none)", unc)

        If success Then
            Logger.Info($"Mapping succeeded: profile '{safeProfile}', drive {safeDrive}, UNC {safeUnc}. Code {resultCode}.")
        Else
            Logger.Error($"Mapping failed: profile '{safeProfile}', drive {safeDrive}, UNC {safeUnc}. Code {resultCode}.")
        End If
    End Sub
End Module
