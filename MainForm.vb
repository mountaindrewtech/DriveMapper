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
                    AppendDetail($"Failed to relaunch elevated: {ex.Message}")
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
            AppendDetail("Connect requested without a selected profile.")
            Return
        End If

        Try
            Dim target = GetCredentialTarget(profile)
            Dim user As String = Nothing
            Dim password As String = Nothing
            Dim hasCredential As Boolean = False

            If profile.UseCredentialManager Then
                Dim stored = CredentialHelper.ReadCredential(target)
                If stored.HasValue Then
                    user = stored.Value.User
                    password = stored.Value.Password
                    hasCredential = True
                    AppendDetail("Stored credential found; Windows will reconnect this drive after reboot.")
                End If
            End If

            If String.IsNullOrWhiteSpace(user) Then
                user = txtUser.Text.Trim()
            End If

            If String.IsNullOrEmpty(password) Then
                password = txtPassword.Text
            End If

            If profile.UseCredentialManager AndAlso chkRememberCreds.Checked AndAlso Not String.IsNullOrWhiteSpace(user) Then
                Try
                    CredentialHelper.SaveCredential(target, user, password)
                    hasCredential = True
                    AppendDetail("Saved credential to Windows Credential Manager for future logons.")
                Catch ex As Exception
                    AppendDetail($"Failed to save credential: {ex.Message}")
                    Logger.Error($"Failed to save credential for {profile.Name}: {ex}")
                End Try
            End If

            Dim persist = profile.UseCredentialManager AndAlso hasCredential
            Dim result = NetDrive.MapDrive(profile, user, password, persist)

            If result.Success Then
                UpdateStatus(result.Message)
                AppendDetail(result.Message)
                Logger.Info(result.Message)

                If persist Then
                    AppendDetail("Persistent mapping enabled; Windows will auto-reconnect after reboot using the saved credential.")
                ElseIf profile.UseCredentialManager Then
                    AppendDetail("No saved credential available, so this mapping will not persist across reboots.")
                End If
            Else
                Dim errorMessage = $"{result.Message} (code {result.Code})"
                UpdateStatus(result.Message)
                AppendDetail(errorMessage)
                Logger.Error($"Failed to map {profile.Name}: {errorMessage}")
            End If
        Catch ex As Exception
            UpdateStatus("Error connecting.")
            AppendDetail($"Unhandled error: {ex.Message}")
            Logger.Error($"Unhandled connect error: {ex}")
        End Try
    End Sub

    Private Sub btnDisconnect_Click(sender As Object, e As EventArgs) Handles btnDisconnect.Click
        Dim profile = GetSelectedProfile()
        If profile Is Nothing Then
            UpdateStatus("Select a profile first.")
            AppendDetail("Disconnect requested without a selected profile.")
            Return
        End If

        Try
            Dim result = NetDrive.UnmapDrive(profile, False)
            If result.Success Then
                UpdateStatus(result.Message)
                AppendDetail(result.Message)
                Logger.Info(result.Message)

                If chkDeleteCreds.Checked AndAlso profile.UseCredentialManager Then
                    Try
                        CredentialHelper.DeleteCredential(GetCredentialTarget(profile))
                        AppendDetail("Deleted saved credential.")
                    Catch ex As Exception
                        AppendDetail($"Failed to delete credential: {ex.Message}")
                        Logger.Error($"Failed to delete credential for {profile.Name}: {ex}")
                    End Try
                End If
            Else
                Dim errorMessage = $"{result.Message} (code {result.Code})"
                UpdateStatus(result.Message)
                AppendDetail(errorMessage)
                Logger.Error($"Failed to unmap {profile.Name}: {errorMessage}")
            End If
        Catch ex As Exception
            UpdateStatus("Error disconnecting.")
            AppendDetail($"Unhandled error: {ex.Message}")
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
            AppendDetail(message)
        Catch ex As Exception
            _profiles = New List(Of ProfilesStore.Profile)()
            cboProfile.DataSource = Nothing
            UpdateStatus("Failed to load profiles.")
            AppendDetail($"Failed to load profiles: {ex.Message}")
            Logger.Error($"Failed to load profiles: {ex}")
        End Try
    End Sub

    Private Function GetSelectedProfile() As ProfilesStore.Profile
        Return TryCast(cboProfile.SelectedItem, ProfilesStore.Profile)
    End Function

    Private Sub UpdateStatus(message As String)
        toolStatus.Text = $"Status: {message}"
    End Sub

    Private Sub AppendDetail(message As String)
        If txtDetails Is Nothing Then
            Return
        End If

        Dim line = $"{DateTimeOffset.Now:HH:mm:ss} {message}"
        txtDetails.AppendText(line & Environment.NewLine)
    End Sub

    Private Shared Function GetCredentialTarget(profile As ProfilesStore.Profile) As String
        If profile Is Nothing OrElse String.IsNullOrWhiteSpace(profile.Name) Then
            Return CredentialPrefix
        End If

        Return $"{CredentialPrefix}{profile.Name}"
    End Function
End Class
