Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Net
Imports System.Net.Sockets

Public Class AdminForm
    Private ReadOnly _binding As New BindingSource()
    Private _profiles As BindingList(Of ProfilesStore.Profile) = New BindingList(Of ProfilesStore.Profile)()
    Private _isElevated As Boolean
    Private _suppressCredentialEvent As Boolean

    Private Sub AdminForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _isElevated = SecurityHelpers.IsElevatedAdmin()
        ConfigureGrid()
        LoadProfiles()
        ApplyAdminState()
        UpdateButtonStates()

        If Not _isElevated Then
            MessageBox.Show(Me, "Admin changes require elevation. View-only mode enabled.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub ConfigureGrid()
        gridProfiles.AutoGenerateColumns = False
        colName.DataPropertyName = NameOf(ProfilesStore.Profile.Name)
        colUnc.DataPropertyName = NameOf(ProfilesStore.Profile.Unc)
        colDriveLetter.DataPropertyName = NameOf(ProfilesStore.Profile.DriveLetter)
        Domain.DataPropertyName = NameOf(ProfilesStore.Profile.Domain)
        colUseCredential.DataPropertyName = NameOf(ProfilesStore.Profile.UseCredentialManager)

        gridProfiles.DataSource = _binding
    End Sub

    Private Sub LoadProfiles()
        Dim loaded = ProfilesStore.LoadProfiles()
        _profiles = New BindingList(Of ProfilesStore.Profile)(loaded)
        _binding.DataSource = _profiles
        _binding.ResetBindings(False)
        If gridProfiles.Rows.Count > 0 Then
            gridProfiles.Rows(0).Selected = True
        End If
        UpdateButtonStates()
        SyncCredentialCheckbox()
    End Sub

    Private Sub ApplyAdminState()
        btnAdd.Enabled = _isElevated
        btnEdit.Enabled = _isElevated AndAlso gridProfiles.SelectedRows.Count > 0
        btnRemove.Enabled = _isElevated AndAlso gridProfiles.SelectedRows.Count > 0
        btnSave.Enabled = _isElevated
        chkUseCredentialManager.Enabled = _isElevated AndAlso gridProfiles.SelectedRows.Count > 0
    End Sub

    Private Sub UpdateButtonStates()
        btnEdit.Enabled = _isElevated AndAlso gridProfiles.SelectedRows.Count > 0
        btnRemove.Enabled = _isElevated AndAlso gridProfiles.SelectedRows.Count > 0
        chkUseCredentialManager.Enabled = _isElevated AndAlso gridProfiles.SelectedRows.Count > 0
        btnTest.Enabled = gridProfiles.SelectedRows.Count > 0
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Not EnsureEditingAllowed() Then
            Return
        End If

        Dim newProfile = PromptForProfile(Nothing)
        If newProfile Is Nothing Then
            Return
        End If

        _profiles.Add(newProfile)
        _binding.ResetBindings(False)
        UpdateButtonStates()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If Not EnsureEditingAllowed() Then
            Return
        End If

        Dim selected = GetSelectedProfile()
        If selected Is Nothing Then
            Return
        End If

        Dim updated = PromptForProfile(selected)
        If updated Is Nothing Then
            Return
        End If

        selected.Name = updated.Name
        selected.Unc = updated.Unc
        selected.DriveLetter = updated.DriveLetter
        selected.UseCredentialManager = updated.UseCredentialManager
        selected.Domain = updated.Domain
        _binding.ResetBindings(False)
        SyncCredentialCheckbox()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If Not EnsureEditingAllowed() Then
            Return
        End If

        Dim selected = GetSelectedProfile()
        If selected Is Nothing Then
            Return
        End If

        If MessageBox.Show(Me, $"Remove profile '{selected.Name}'?", "DriveMapper", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            _profiles.Remove(selected)
            _binding.ResetBindings(False)
            SyncCredentialCheckbox()
            UpdateButtonStates()
        End If
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        Dim profile = GetSelectedProfile()
        If profile Is Nothing Then
            MessageBox.Show(Me, "Select a profile to test.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim host As String = Nothing
        If Not TryGetHostFromUnc(profile.Unc, host) Then
            MessageBox.Show(Me, "Profile UNC must be in the form \\server\share.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim addresses = Dns.GetHostAddresses(host)
            Dim success = False
            Dim errors As New List(Of String)()

            For Each address In addresses
                Using client As New TcpClient()
                    Try
                        Dim connectTask = client.ConnectAsync(address, 445)
                        Dim completed = connectTask.Wait(TimeSpan.FromSeconds(5))
                        If completed AndAlso client.Connected Then
                            success = True
                            Exit For
                        ElseIf Not completed Then
                            errors.Add($"Timeout connecting to {address}.")
                        Else
                            errors.Add($"Unable to connect to {address}.")
                        End If
                    Catch ex As Exception
                        errors.Add($"{address}: {ex.Message}")
                    End Try
                End Using
            Next

            If success Then
                MessageBox.Show(Me, $"Successfully connected to {host} on port 445.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim detail = String.Join(Environment.NewLine, errors)
                MessageBox.Show(Me, $"Unable to reach {host} on port 445.{Environment.NewLine}{detail}", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show(Me, $"DNS lookup failed for {host}: {ex.Message}", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not EnsureEditingAllowed() Then
            Return
        End If

        Try
            ProfilesStore.SaveProfiles(_profiles.ToList())
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show(Me, $"Failed to save profiles: {ex.Message}", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub gridProfiles_SelectionChanged(sender As Object, e As EventArgs) Handles gridProfiles.SelectionChanged
        UpdateButtonStates()
        SyncCredentialCheckbox()
    End Sub

    Private Sub chkUseCredentialManager_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseCredentialManager.CheckedChanged
        If _suppressCredentialEvent OrElse Not _isElevated Then
            Return
        End If

        Dim profile = GetSelectedProfile()
        If profile Is Nothing Then
            Return
        End If

        profile.UseCredentialManager = chkUseCredentialManager.Checked
        _binding.ResetBindings(False)
    End Sub

    Private Sub SyncCredentialCheckbox()
        _suppressCredentialEvent = True
        Dim profile = GetSelectedProfile()
        If profile Is Nothing Then
            chkUseCredentialManager.Checked = False
            chkUseCredentialManager.Enabled = False
        Else
            chkUseCredentialManager.Checked = profile.UseCredentialManager
            chkUseCredentialManager.Enabled = _isElevated
        End If
        _suppressCredentialEvent = False
    End Sub

    Private Function GetSelectedProfile() As ProfilesStore.Profile
        Return TryCast(gridProfiles.CurrentRow?.DataBoundItem, ProfilesStore.Profile)
    End Function

    Private Function EnsureEditingAllowed() As Boolean
        If _isElevated Then
            Return True
        End If

        MessageBox.Show(Me, "Elevation is required to modify profiles.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Return False
    End Function

    Private Function PromptForProfile(existing As ProfilesStore.Profile) As ProfilesStore.Profile
        Dim seed As ProfilesStore.Profile = Nothing
        If existing IsNot Nothing Then
            seed = CloneProfile(existing)
        End If

        While True
            Using editor As New ProfileEditorDialog(seed)
                If editor.ShowDialog(Me) <> DialogResult.OK Then
                    Return Nothing
                End If

                Dim candidate As New ProfilesStore.Profile With {
                    .Name = editor.ProfileName,
                    .Unc = editor.ProfileUnc,
                    .DriveLetter = NormalizeDriveLetter(editor.ProfileDriveLetter),
                    .UseCredentialManager = editor.ProfileUsesCredentialManager,
                    .Domain = editor.ProfileDomain
                }

                Dim validationError = ValidateProfile(candidate, existing)
                If validationError Is Nothing Then
                    Return candidate
                End If

                MessageBox.Show(Me, validationError, "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                seed = candidate
            End Using
        End While

        Return Nothing
    End Function

    Private Function ValidateProfile(profile As ProfilesStore.Profile, original As ProfilesStore.Profile) As String
        If String.IsNullOrWhiteSpace(profile.Name) Then
            Return "Name is required."
        End If

        If Not IsValidUnc(profile.Unc) Then
            Return "UNC path must be in the form \\server\share."
        End If

        If String.IsNullOrWhiteSpace(profile.DriveLetter) Then
            Return "Drive letter is required."
        End If

        If profile.DriveLetter.Length <> 2 OrElse profile.DriveLetter(1) <> ":"c OrElse Not Char.IsLetter(profile.DriveLetter(0)) Then
            Return "Drive letter must be a single letter (e.g. Z:)."
        End If

        If Not IsDriveLetterUnique(profile.DriveLetter, original) Then
            Return "Drive letter must be unique across profiles."
        End If

        Return Nothing
    End Function

    Private Shared Function IsValidUnc(unc As String) As Boolean
        If String.IsNullOrWhiteSpace(unc) Then
            Return False
        End If

        Dim trimmed = unc.Trim()
        If trimmed.Length < 3 OrElse Not trimmed.StartsWith("\\") Then
            Return False
        End If

        If trimmed.Length < 4 OrElse trimmed(1) <> "\"c Then
            Return False
        End If

        Dim path = trimmed.Substring(2)
        Dim segments = path.Split(New Char() {"\"c}, StringSplitOptions.RemoveEmptyEntries)
        Return segments.Length >= 2 AndAlso segments(0).Length > 0 AndAlso segments(1).Length > 0
    End Function

    Private Function IsDriveLetterUnique(letter As String, original As ProfilesStore.Profile) As Boolean
        Dim normalized = NormalizeDriveLetter(letter)
        For Each profile In _profiles
            If original IsNot Nothing AndAlso Object.ReferenceEquals(profile, original) Then
                Continue For
            End If

            If String.Equals(NormalizeDriveLetter(profile.DriveLetter), normalized, StringComparison.OrdinalIgnoreCase) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Shared Function NormalizeDriveLetter(letter As String) As String
        If String.IsNullOrWhiteSpace(letter) Then
            Return String.Empty
        End If

        Dim trimmed = letter.Trim()
        If trimmed.Length = 0 Then
            Return String.Empty
        End If

        Dim ch = trimmed(0)
        If Not Char.IsLetter(ch) Then
            Return trimmed.ToUpperInvariant()
        End If

        Return $"{Char.ToUpperInvariant(ch)}:"
    End Function

    Private Shared Function TryGetHostFromUnc(unc As String, ByRef host As String) As Boolean
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

    Private Shared Function CloneProfile(profile As ProfilesStore.Profile) As ProfilesStore.Profile
        Return New ProfilesStore.Profile With {
            .Name = profile.Name,
            .Unc = profile.Unc,
            .DriveLetter = profile.DriveLetter,
            .UseCredentialManager = profile.UseCredentialManager,
            .Domain = profile.Domain
        }
    End Function

    Private Class ProfileEditorDialog
        Inherits Form

        Private ReadOnly _txtName As New TextBox()
        Private ReadOnly _txtUnc As New TextBox()
        Private ReadOnly _txtDrive As New TextBox()
        Private ReadOnly _txtDomain As New TextBox()
        Private ReadOnly _chkCredential As New CheckBox()

        Public ReadOnly Property ProfileName As String
            Get
                Return _txtName.Text.Trim()
            End Get
        End Property

        Public ReadOnly Property ProfileUnc As String
            Get
                Return _txtUnc.Text.Trim()
            End Get
        End Property

        Public ReadOnly Property ProfileDriveLetter As String
            Get
                Return _txtDrive.Text.Trim()
            End Get
        End Property

        Public ReadOnly Property ProfileDomain As String
            Get
                Return _txtDomain.Text.Trim()
            End Get
        End Property

        Public ReadOnly Property ProfileUsesCredentialManager As Boolean
            Get
                Return _chkCredential.Checked
            End Get
        End Property

        Public Sub New(seed As ProfilesStore.Profile)
            Text = If(seed Is Nothing, "Add Profile", "Edit Profile")
            FormBorderStyle = FormBorderStyle.FixedDialog
            StartPosition = FormStartPosition.CenterParent
            MaximizeBox = False
            MinimizeBox = False
            ClientSize = New Size(360, 230)
            KeyPreview = True

            Dim lblName As New Label() With {.Text = "Name", .Location = New Point(12, 15), .AutoSize = True}
            _txtName.Location = New Point(120, 12)
            _txtName.Width = 210

            Dim lblUnc As New Label() With {.Text = "UNC", .Location = New Point(12, 50), .AutoSize = True}
            _txtUnc.Location = New Point(120, 47)
            _txtUnc.Width = 210

            Dim lblDomain As New Label() With {.Text = "Domain (optional)", .Location = New Point(12, 85), .AutoSize = True}
            _txtDomain.Location = New Point(120, 82)
            _txtDomain.Width = 210

            Dim lblDrive As New Label() With {.Text = "Drive Letter", .Location = New Point(12, 120), .AutoSize = True}
            _txtDrive.Location = New Point(120, 117)
            _txtDrive.Width = 60

            _chkCredential.Text = "Use Credential Manager"
            _chkCredential.Location = New Point(120, 150)
            _chkCredential.AutoSize = True

            Dim btnOk As New Button() With {.Text = "OK", .DialogResult = DialogResult.None, .Location = New Point(168, 185), .Width = 75}
            Dim btnCancel As New Button() With {.Text = "Cancel", .DialogResult = DialogResult.Cancel, .Location = New Point(249, 185), .Width = 75}

            AddHandler btnOk.Click, AddressOf OnOkClicked

            AcceptButton = btnOk
            CancelButton = btnCancel

            Controls.AddRange(New Control() {
                lblName, _txtName,
                lblUnc, _txtUnc,
                lblDomain, _txtDomain,
                lblDrive, _txtDrive,
                _chkCredential, btnOk, btnCancel})

            If seed IsNot Nothing Then
                _txtName.Text = seed.Name
                _txtUnc.Text = seed.Unc
                _txtDrive.Text = seed.DriveLetter
                _txtDomain.Text = seed.Domain
                _chkCredential.Checked = seed.UseCredentialManager
            Else
                _chkCredential.Checked = True
            End If
        End Sub

        Private Sub OnOkClicked(sender As Object, e As EventArgs)
            If String.IsNullOrWhiteSpace(_txtName.Text) Then
                MessageBox.Show(Me, "Name is required.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtName.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(_txtUnc.Text) Then
                MessageBox.Show(Me, "UNC is required.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtUnc.Focus()
                Return
            End If

            If String.IsNullOrWhiteSpace(_txtDrive.Text) Then
                MessageBox.Show(Me, "Drive letter is required.", "DriveMapper", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                _txtDrive.Focus()
                Return
            End If

            DialogResult = DialogResult.OK
            Close()
        End Sub
    End Class
End Class
