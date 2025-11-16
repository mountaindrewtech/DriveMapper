' Copyright (c) 2025 Drew Schmidt
' Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
' See LICENSE for details.

Public Class MainForm
    Private _profiles As List(Of ProfilesStore.Profile) = New List(Of ProfilesStore.Profile)()
    Private Const CredentialPrefix As String = "DriveMapper_"

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboProfile.DisplayMember = NameOf(ProfilesStore.Profile.Name)
        LoadProfilesIntoCombo()
    End Sub

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        btnAdmin.Enabled = SecurityHelpers.IsElevatedAdmin()
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

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Dim profile = GetSelectedProfile()
        If profile Is Nothing Then
            UpdateStatus("Select a profile first.")
            Logger.Info("Connect requested without a selected profile.")
            Return
        End If

        Try
            Dim effectiveDomain = GetEffectiveDomain()
            Dim target = GetCredentialTarget(profile, effectiveDomain)
            Dim user As String = Nothing
            Dim password As String = Nothing
            Dim hasCredential As Boolean = False

            If profile.UseCredentialManager Then
                Dim stored = CredentialHelper.ReadCredential(target)
                If stored.HasValue Then
                    user = stored.Value.User
                    password = stored.Value.Password
                    hasCredential = True
                    Dim storedIdentity = BuildLoginIdentity(user, effectiveDomain)
                    Logger.Info($"Stored credential found for {profile.Name} as {DescribeLoginIdentity(storedIdentity)}; Windows will reconnect this drive after reboot.")
                End If
            End If

            If String.IsNullOrWhiteSpace(user) Then
                user = txtUser.Text.Trim()
            End If

            If String.IsNullOrEmpty(password) Then
                password = txtPassword.Text
            End If

            Dim loginIdentity = BuildLoginIdentity(user, effectiveDomain)
            Dim identityDisplay = DescribeLoginIdentity(loginIdentity)

            If profile.UseCredentialManager AndAlso chkRememberCreds.Checked AndAlso Not String.IsNullOrWhiteSpace(loginIdentity) Then
                Try
                    CredentialHelper.SaveCredential(target, loginIdentity, password)
                    hasCredential = True
                    Logger.Info($"Saved credential for {profile.Name} as {identityDisplay}; Windows will reconnect this drive after reboot.")
                Catch ex As Exception
                    Logger.Error($"Failed to save credential for {profile.Name} as {identityDisplay}: {ex}")
                End Try
            End If

            Dim persist = profile.UseCredentialManager AndAlso hasCredential
            Dim result = NetDrive.MapDrive(profile, If(String.IsNullOrWhiteSpace(loginIdentity), Nothing, loginIdentity), password, persist)

            If result.Success Then
                Dim statusMessage = $"{result.Message} as {identityDisplay}"
                UpdateStatus(statusMessage)
                Logger.Info(statusMessage)

                If persist Then
                    Logger.Info("Persistent mapping enabled; Windows will auto-reconnect after reboot using the saved credential.")
                ElseIf profile.UseCredentialManager Then
                    Logger.Info("No saved credential available, so this mapping will not persist across reboots.")
                End If
            Else
                Dim errorMessage = $"{result.Message} (code {result.Code})"
                Dim statusMessage = $"{result.Message} as {identityDisplay}"
                UpdateStatus(statusMessage)
                Logger.Error($"Failed to map {profile.Name} as {identityDisplay}: {errorMessage}")
            End If
        Catch ex As Exception
            UpdateStatus("Error connecting.")
            Logger.Error($"Unhandled connect error: {ex}")
        End Try
    End Sub

    Private Sub btnDisconnect_Click(sender As Object, e As EventArgs) Handles btnDisconnect.Click
        Dim profile = GetSelectedProfile()
        If profile Is Nothing Then
            UpdateStatus("Select a profile first.")
            Logger.Info("Disconnect requested without a selected profile.")
            Return
        End If

        Dim effectiveDomain = GetEffectiveDomain()

        Try
            Dim result = NetDrive.UnmapDrive(profile, False)
            If result.Success Then
                UpdateStatus(result.Message)
                Logger.Info(result.Message)

                If chkDeleteCreds.Checked AndAlso profile.UseCredentialManager Then
                    Try
                        Dim domainKey = GetDomainKey(effectiveDomain)
                        CredentialHelper.DeleteCredential(GetCredentialTarget(profile, effectiveDomain))
                        Logger.Info($"Deleted saved credential for {profile.Name} ({domainKey}).")
                    Catch ex As Exception
                        Logger.Error($"Failed to delete credential for {profile.Name}: {ex}")
                    End Try
                End If
            Else
                Dim errorMessage = $"{result.Message} (code {result.Code})"
                UpdateStatus(result.Message)
                Logger.Error($"Failed to unmap {profile.Name}: {errorMessage}")
            End If
        Catch ex As Exception
            UpdateStatus("Error disconnecting.")
            Logger.Error($"Unhandled disconnect error: {ex}")
        End Try
    End Sub

    Private Sub LoadProfilesIntoCombo()
        Try
            _profiles = ProfilesStore.LoadProfiles()
            cboProfile.DataSource = Nothing
            cboProfile.DataSource = _profiles
            cboProfile.DisplayMember = NameOf(ProfilesStore.Profile.Name)

            Dim message = $"Loaded {_profiles.Count} profile(s)."
            UpdateStatus(message)
            PopulateSelectedProfileFields()
        Catch ex As Exception
            _profiles = New List(Of ProfilesStore.Profile)()
            cboProfile.DataSource = Nothing
            UpdateStatus("Failed to load profiles.")
            Logger.Error($"Failed to load profiles: {ex}")
            PopulateSelectedProfileFields()
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
        If profile Is Nothing Then
            txtDomain.Text = String.Empty
            Return
        End If

        txtDomain.Text = If(profile.Domain, String.Empty)
    End Sub

    Private Function GetEffectiveDomain() As String
        If txtDomain Is Nothing Then
            Return String.Empty
        End If

        Return txtDomain.Text.Trim()
    End Function

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

    Private Sub chkRememberCreds_CheckedChanged(sender As Object, e As EventArgs) Handles chkRememberCreds.CheckedChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles lblProfile.Click

    End Sub

    Private Sub chkDeleteCreds_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeleteCreds.CheckedChanged

    End Sub

    Private Sub lblPassword_Click(sender As Object, e As EventArgs) Handles lblPassword.Click

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
End Class
