' Copyright (c) 2025 Drew Schmidt
' Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
' See LICENSE for details.

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Threading.Tasks

Public Class MainForm
    Private _profiles As List(Of ProfilesStore.Profile) = New List(Of ProfilesStore.Profile)()
    Private ReadOnly _lastCredentialKeyByProfile As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
    Private _isPopulatingProfile As Boolean
    Private _isMappingInProgress As Boolean
    Private _cancelRequested As Boolean
    Private _currentOperationId As Integer
    Private Const CredentialPrefix As String = "DriveMapper_"

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboProfile.DisplayMember = NameOf(ProfilesStore.Profile.Name)
        LoadProfilesIntoCombo()
        txtDomain.ReadOnly = Not SecurityHelpers.IsElevatedAdmin()
        chkOpenOnConnect.Checked = My.Settings.OpenOnConnect
        btnCancel.Enabled = False
    End Sub

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        btnAdmin.Enabled = True
    End Sub

    Private Sub chkOpenOnConnect_CheckedChanged(sender As Object, e As EventArgs) Handles chkOpenOnConnect.CheckedChanged
        My.Settings.OpenOnConnect = chkOpenOnConnect.Checked
        My.Settings.Save()
    End Sub

    Private Sub btnAdmin_Click(sender As Object, e As EventArgs) Handles btnAdmin.Click
        If Not SecurityHelpers.IsElevatedAdmin() Then
            Dim response = MessageBox.Show(Me, "Admin mode requires elevation. Relaunch now?", "DriveMapper", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If response = DialogResult.Yes Then
                Try
                    SecurityHelpers.RelaunchElevated()
                Catch ex As Exception
                    UpdateStatus("Elevation failed.")
                    Logger.Error($"Failed to relaunch elevated: {ex}")
                End Try
            End If
            Return
        End If

        Using admin As New AdminForm()
            Dim result = admin.ShowDialog(Me)
            If result = DialogResult.OK Then
                LoadProfilesIntoCombo()
            End If
        End Using
    End Sub

    Private Async Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Dim profile = GetSelectedProfile()
        If profile Is Nothing Then
            UpdateStatus("Select a profile first.")
            Logger.Info("Connect requested without a selected profile.")
            Return
        End If

        If Not ProfileValidation.IsValidUnc(profile.Unc) OrElse Not ProfileValidation.IsValidDriveLetter(profile.DriveLetter) Then
            Logger.Error($"Selected profile '{profile.Name}' is invalid; mapping cancelled.")
            MessageBox.Show(Me, "The selected profile is invalid. Please review the UNC path and drive letter in the Admin panel.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            UpdateStatus("Profile validation failed.")
            Return
        End If

        _isMappingInProgress = True
        _cancelRequested = False
        _currentOperationId += 1
        Dim thisOperationId = _currentOperationId

        Dim connectEnabled = btnConnect.Enabled
        Dim disconnectEnabled = btnDisconnect.Enabled
        Dim profileSelectorEnabled = cboProfile.Enabled
        Dim adminEnabled = btnAdmin.Enabled

        btnConnect.Enabled = False
        btnDisconnect.Enabled = False
        btnCancel.Enabled = True
        cboProfile.Enabled = False
        btnAdmin.Enabled = False
        UpdateStatus("Connecting...")

        Dim refreshWithSelection = False
        Dim result As (Success As Boolean, Message As String, Code As Integer)

        Try
            Dim effectiveDomain = GetEffectiveDomain()
            Dim credentialTarget = GetCredentialTarget(profile, effectiveDomain)
            Dim useCredentialManager = profile.UseCredentialManager
            Dim useSavedCredential = useCredentialManager AndAlso chkUseSavedCredential.Checked
            Dim rememberCredential = useCredentialManager AndAlso chkRememberCredential.Checked
            Dim savedCredential As (User As String, Password As String)? = Nothing

            If useSavedCredential Then
                Try
                    savedCredential = CredentialHelper.ReadCredential(credentialTarget)
                    If Not savedCredential.HasValue Then
                        useSavedCredential = False
                        chkUseSavedCredential.Checked = False
                    End If
                Catch ex As Exception
                    Logger.Error($"Failed to load saved credential for {profile.Name}: {ex}")
                    useSavedCredential = False
                    chkUseSavedCredential.Checked = False
                End Try
            End If

            Dim user = If(useSavedCredential AndAlso savedCredential.HasValue, savedCredential.Value.User, txtUser.Text.Trim())
            Dim password = If(useSavedCredential AndAlso savedCredential.HasValue, savedCredential.Value.Password, txtPassword.Text)

            Dim loginIdentity = BuildLoginIdentity(user, effectiveDomain)
            Dim identityDisplay = DescribeLoginIdentity(loginIdentity)

            If rememberCredential AndAlso String.IsNullOrWhiteSpace(password) Then
                Logger.Info("Credential not saved because the password field was blank.")
                rememberCredential = False
            End If

            Dim hasPersistentCredential = useSavedCredential AndAlso savedCredential.HasValue
            Dim persist = useCredentialManager AndAlso (hasPersistentCredential OrElse rememberCredential)
            result = Await Task.Run(Function() NetDrive.MapDrive(profile, If(String.IsNullOrWhiteSpace(loginIdentity), Nothing, loginIdentity), password, persist, useCredentialManager))

            If thisOperationId <> _currentOperationId Then
                Return
            End If

            If _cancelRequested Then
                UpdateStatus("Connection cancelled.")
                refreshWithSelection = False
                Return
            End If

            If result.Success Then
                Dim statusMessage = $"{result.Message} as {identityDisplay}"
                UpdateStatus(statusMessage)

                _lastCredentialKeyByProfile(profile.Name) = credentialTarget

                If rememberCredential Then
                    Try
                        CredentialHelper.SaveCredential(credentialTarget, loginIdentity, password)
                        Logger.Info($"Saved credential for {profile.Name}; Windows will reconnect this drive after reboot.")
                    Catch ex As Exception
                        Logger.Error($"Failed to save credential for {profile.Name}: {ex}")
                    End Try
                End If

                If chkOpenOnConnect.Checked Then
                    Dim openPath = ProfileValidation.NormalizeDriveLetter(profile.DriveLetter)
                    If Not String.IsNullOrWhiteSpace(openPath) Then
                        openPath &= "\"
                    Else
                        openPath = profile.Unc
                    End If

                    If Not String.IsNullOrWhiteSpace(openPath) Then
                        Try
                            Process.Start("explorer.exe", openPath)
                        Catch ex As Exception
                            Logger.Error($"Failed to open File Explorer for {profile.Name}: {ex}")
                        End Try
                    End If
                End If

                refreshWithSelection = True
            Else
                Dim statusMessage = $"{result.Message} as {identityDisplay}"
                UpdateStatus(statusMessage)
                refreshWithSelection = False
            End If
        Catch ex As Exception
            If thisOperationId = _currentOperationId AndAlso _cancelRequested Then
                UpdateStatus("Connection cancelled.")
            Else
                UpdateStatus("Error connecting.")
                Logger.Error($"Unhandled connect error: {ex}")
                MessageBox.Show(Me, "Failed to connect. Check your profile settings or contact your administrator.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            If thisOperationId = _currentOperationId Then
                _isMappingInProgress = False
                btnCancel.Enabled = False
                btnConnect.Enabled = connectEnabled
                btnDisconnect.Enabled = disconnectEnabled
                cboProfile.Enabled = profileSelectorEnabled
                btnAdmin.Enabled = adminEnabled
                RefreshCredentialUiState(refreshWithSelection)
            End If
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _isMappingInProgress Then
            _cancelRequested = True
            UpdateStatus("Cancelling...")

            btnCancel.Enabled = False
            btnConnect.Enabled = True
            btnDisconnect.Enabled = True
            btnAdmin.Enabled = True
            cboProfile.Enabled = True

            _isMappingInProgress = False
        End If
    End Sub

    Private Async Sub btnDisconnect_Click(sender As Object, e As EventArgs) Handles btnDisconnect.Click
        Dim profile = GetSelectedProfile()
        If profile Is Nothing Then
            UpdateStatus("Select a profile first.")
            Logger.Info("Disconnect requested without a selected profile.")
            Return
        End If

        Dim connectEnabled = btnConnect.Enabled
        Dim disconnectEnabled = btnDisconnect.Enabled
        Dim profileSelectorEnabled = cboProfile.Enabled
        Dim adminEnabled = btnAdmin.Enabled

        btnConnect.Enabled = False
        btnDisconnect.Enabled = False
        cboProfile.Enabled = False
        btnAdmin.Enabled = False
        UpdateStatus("Disconnecting...")

        Dim refreshWithSelection = False

        Try
            Dim result = Await Task.Run(Function() NetDrive.UnmapDrive(profile, False))
            If result.Success Then
                UpdateStatus(result.Message)
                Logger.Info(result.Message)
            Else
                Dim errorMessage = $"{result.Message} (code {result.Code})"
                UpdateStatus(result.Message)
                Logger.Error($"Failed to unmap {profile.Name}: {errorMessage}")
            End If
        Catch ex As Exception
            UpdateStatus("Error disconnecting.")
            Logger.Error($"Unhandled disconnect error: {ex}")
            MessageBox.Show(Me, "Failed to disconnect. Check your connection and try again.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnConnect.Enabled = connectEnabled
            btnDisconnect.Enabled = disconnectEnabled
            cboProfile.Enabled = profileSelectorEnabled
            btnAdmin.Enabled = adminEnabled
            RefreshCredentialUiState(refreshWithSelection)
        End Try
    End Sub

    Private Sub LoadProfilesIntoCombo()
        Try
            _profiles = ProfilesStore.LoadProfiles()
            ReconcileCredentialKeyCache()
            cboProfile.DataSource = Nothing
            cboProfile.DataSource = _profiles
            cboProfile.DisplayMember = NameOf(ProfilesStore.Profile.Name)

            Dim message = $"Loaded {_profiles.Count} profile(s)."
            UpdateStatus(message)
            Dim hasProfiles = _profiles.Count > 0
            btnConnect.Enabled = hasProfiles
            btnDisconnect.Enabled = hasProfiles
            PopulateSelectedProfileFields()
        Catch ex As Exception
            _profiles = New List(Of ProfilesStore.Profile)()
            _lastCredentialKeyByProfile.Clear()
            cboProfile.DataSource = Nothing
            btnConnect.Enabled = False
            btnDisconnect.Enabled = False
            UpdateStatus("Failed to load profiles.")
            Logger.Error($"Failed to load profiles: {ex}")
            MessageBox.Show(Me, $"Drive profiles could not be loaded ({ex.Message}). Check permissions or the JSON file under %ProgramData%\DriveMapper.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Error)
            PopulateSelectedProfileFields()
        Finally
            RefreshCredentialUiState()
        End Try
    End Sub

    Private Sub cboProfile_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProfile.SelectedIndexChanged
        PopulateSelectedProfileFields()
    End Sub

    Private Function GetSelectedProfile() As ProfilesStore.Profile
        Return TryCast(cboProfile.SelectedItem, ProfilesStore.Profile)
    End Function

    Private Sub UpdateStatus(message As String)
        toolStatus.Text = $"Status: {message}"
    End Sub

    Private Sub PopulateSelectedProfileFields()
        If txtDomain Is Nothing Then
            Return
        End If

        Dim profile = GetSelectedProfile()
        _isPopulatingProfile = True
        Try
            If profile Is Nothing Then
                txtDomain.Text = String.Empty
            Else
                txtDomain.Text = If(profile.Domain, String.Empty)
            End If
        Finally
            _isPopulatingProfile = False
        End Try

        RefreshCredentialUiState(True)
    End Sub

    Private Function GetEffectiveDomain() As String
        If txtDomain Is Nothing Then
            Return String.Empty
        End If

        Return txtDomain.Text.Trim()
    End Function

    Private Sub RefreshCredentialUiState(Optional resetCredentialSelection As Boolean = False)
        Dim profile = GetSelectedProfile()
        Dim hasProfile = profile IsNot Nothing
        Dim profileAllowsCredentialManager = Not hasProfile OrElse profile.UseCredentialManager
        Dim effectiveDomain = GetEffectiveDomain()
        Dim credentialTarget = If(hasProfile, GetStoredCredentialTarget(profile, effectiveDomain), Nothing)
        Dim savedCredentialExists = False

        If hasProfile AndAlso Not String.IsNullOrWhiteSpace(credentialTarget) Then
            Try
                savedCredentialExists = CredentialHelper.CredentialExists(credentialTarget)
            Catch ex As Exception
                savedCredentialExists = False
                Dim profileLabel = If(profile Is Nothing OrElse String.IsNullOrWhiteSpace(profile.Name), "(unknown)", profile.Name)
                Logger.Error($"Failed to inspect saved credential for {profileLabel}: {ex}")
            End Try
        End If

        chkUseSavedCredential.Enabled = profileAllowsCredentialManager AndAlso savedCredentialExists
        If resetCredentialSelection Then
            chkUseSavedCredential.Checked = chkUseSavedCredential.Enabled
        ElseIf Not chkUseSavedCredential.Enabled Then
            chkUseSavedCredential.Checked = False
        End If

        chkRememberCredential.Enabled = profileAllowsCredentialManager
        If Not profileAllowsCredentialManager Then
            chkRememberCredential.Checked = False
        End If

        Dim profileName = If(profile Is Nothing, Nothing, profile.Name)
        Dim cachedKeyExists = profileName IsNot Nothing AndAlso _lastCredentialKeyByProfile.ContainsKey(profileName)
        btnDeleteSavedCredential.Enabled = profileAllowsCredentialManager AndAlso hasProfile AndAlso (savedCredentialExists OrElse cachedKeyExists)
    End Sub

    Private Function GetStoredCredentialTarget(profile As ProfilesStore.Profile, effectiveDomain As String) As String
        If profile Is Nothing Then
            Return Nothing
        End If

        Dim cachedTarget As String = Nothing
        If _lastCredentialKeyByProfile.TryGetValue(profile.Name, cachedTarget) Then
            Return cachedTarget
        End If

        Return GetCredentialTarget(profile, effectiveDomain)
    End Function

    Private Sub btnDeleteSavedCredential_Click(sender As Object, e As EventArgs) Handles btnDeleteSavedCredential.Click
        Dim profile = GetSelectedProfile()
        If profile Is Nothing Then
            Return
        End If

        Dim effectiveDomain = GetEffectiveDomain()
        Dim credentialTarget = GetStoredCredentialTarget(profile, effectiveDomain)
        If String.IsNullOrWhiteSpace(credentialTarget) Then
            Return
        End If

        Try
            CredentialHelper.DeleteCredential(credentialTarget)
            _lastCredentialKeyByProfile.Remove(profile.Name)
            Logger.Info($"Deleted saved credential for {profile.Name}.")
            UpdateStatus("Saved credential deleted.")
        Catch ex As Exception
            Logger.Error($"Failed to delete credential for {profile.Name}: {ex}")
            MessageBox.Show(Me, $"Failed to delete saved credential: {ex.Message}", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            RefreshCredentialUiState(True)
        End Try
    End Sub

    Private Sub txtDomain_TextChanged(sender As Object, e As EventArgs) Handles txtDomain.TextChanged
        If _isPopulatingProfile OrElse Not SecurityHelpers.IsElevatedAdmin() Then
            Return
        End If

        RefreshCredentialUiState(True)
    End Sub

    Private Sub ReconcileCredentialKeyCache()
        Dim validNames As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        For Each profile In _profiles
            If Not String.IsNullOrWhiteSpace(profile.Name) Then
                validNames.Add(profile.Name)
            End If
        Next

        Dim staleKeys As New List(Of String)()
        For Each entry In _lastCredentialKeyByProfile.Keys
            If Not validNames.Contains(entry) Then
                staleKeys.Add(entry)
            End If
        Next

        For Each stale In staleKeys
            _lastCredentialKeyByProfile.Remove(stale)
        Next
    End Sub

    Private Shared Function BuildLoginIdentity(user As String, domain As String) As String
        Dim normalizedUser = If(user, String.Empty).Trim()
        If String.IsNullOrEmpty(normalizedUser) Then
            Return String.Empty
        End If

        If normalizedUser.Contains("\") OrElse normalizedUser.Contains("@") Then
            Return normalizedUser
        End If

        Dim normalizedDomain = If(domain, String.Empty).Trim()
        If String.IsNullOrEmpty(normalizedDomain) Then
            Return normalizedUser
        End If

        Return $"{normalizedDomain}\{normalizedUser}"
    End Function

    Private Shared Function DescribeLoginIdentity(identity As String) As String
        Return If(String.IsNullOrWhiteSpace(identity), "current Windows user", identity)
    End Function

    Private Shared Function GetDomainKey(domain As String) As String
        Dim normalized = If(domain, String.Empty).Trim()
        If String.IsNullOrEmpty(normalized) Then
            Return "LOCAL"
        End If

        Return normalized.ToUpperInvariant()
    End Function

    Private Shared Function GetCredentialTarget(profile As ProfilesStore.Profile, domain As String) As String
        Dim domainKey = GetDomainKey(domain)
        If profile Is Nothing OrElse String.IsNullOrWhiteSpace(profile.Name) Then
            Return $"{CredentialPrefix}{domainKey}"
        End If

        Return $"{CredentialPrefix}{profile.Name}_{domainKey}"
    End Function

    Private Sub TableLayoutPanel4_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel4.Paint

    End Sub
End Class
