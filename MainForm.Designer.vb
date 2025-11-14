' Copyright (c) 2025 Drew Schmidt
' Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
' See LICENSE for details.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        cboProfile = New ComboBox()
        txtUser = New TextBox()
        txtPassword = New TextBox()
        btnConnect = New Button()
        btnDisconnect = New Button()
        btnAdmin = New Button()
        chkRememberCreds = New CheckBox()
        chkDeleteCreds = New CheckBox()
        toolStatus = New ToolStripStatusLabel()
        statusStrip = New StatusStrip()
        txtDomain = New TextBox()
        lblProfile = New Label()
        lblCredentials = New Label()
        lblDomain = New Label()
        lblUsername = New Label()
        lblPassword = New Label()
        lblActions = New Label()
        grpCredentials = New GroupBox()
        grpActions = New GroupBox()
        statusStrip.SuspendLayout()
        grpCredentials.SuspendLayout()
        grpActions.SuspendLayout()
        SuspendLayout()
        ' 
        ' cboProfile
        ' 
        cboProfile.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        cboProfile.DropDownStyle = ComboBoxStyle.DropDownList
        cboProfile.Font = New Font("Segoe UI", 12F)
        cboProfile.FormattingEnabled = True
        cboProfile.Location = New Point(71, 6)
        cboProfile.Name = "cboProfile"
        cboProfile.Size = New Size(415, 29)
        cboProfile.TabIndex = 0
        ' 
        ' txtUser
        ' 
        txtUser.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtUser.Font = New Font("Segoe UI", 12F)
        txtUser.Location = New Point(91, 58)
        txtUser.MinimumSize = New Size(180, 0)
        txtUser.Name = "txtUser"
        txtUser.PlaceholderText = "Username"
        txtUser.Size = New Size(383, 29)
        txtUser.TabIndex = 1
        ' 
        ' txtPassword
        ' 
        txtPassword.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtPassword.Font = New Font("Segoe UI", 12F)
        txtPassword.Location = New Point(91, 94)
        txtPassword.MinimumSize = New Size(180, 0)
        txtPassword.Name = "txtPassword"
        txtPassword.PlaceholderText = "Password"
        txtPassword.Size = New Size(293, 29)
        txtPassword.TabIndex = 2
        txtPassword.UseSystemPasswordChar = True
        ' 
        ' btnConnect
        ' 
        btnConnect.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnConnect.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        btnConnect.Location = New Point(68, 20)
        btnConnect.Margin = New Padding(6, 3, 6, 3)
        btnConnect.Name = "btnConnect"
        btnConnect.Padding = New Padding(8, 0, 8, 0)
        btnConnect.Size = New Size(130, 36)
        btnConnect.TabIndex = 3
        btnConnect.Text = "Connect"
        btnConnect.UseVisualStyleBackColor = True
        ' 
        ' btnDisconnect
        ' 
        btnDisconnect.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnDisconnect.Font = New Font("Segoe UI", 12F)
        btnDisconnect.Location = New Point(207, 20)
        btnDisconnect.Name = "btnDisconnect"
        btnDisconnect.Padding = New Padding(8, 0, 8, 0)
        btnDisconnect.Size = New Size(130, 36)
        btnDisconnect.TabIndex = 4
        btnDisconnect.Text = "Disconnect"
        btnDisconnect.UseVisualStyleBackColor = True
        ' 
        ' btnAdmin
        ' 
        btnAdmin.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnAdmin.Font = New Font("Segoe UI", 12F)
        btnAdmin.Location = New Point(343, 20)
        btnAdmin.Name = "btnAdmin"
        btnAdmin.Padding = New Padding(8, 0, 8, 0)
        btnAdmin.Size = New Size(130, 36)
        btnAdmin.TabIndex = 5
        btnAdmin.Text = "Admin Panel"
        btnAdmin.UseVisualStyleBackColor = True
        ' 
        ' chkRememberCreds
        ' 
        chkRememberCreds.Anchor = AnchorStyles.Right
        chkRememberCreds.AutoSize = True
        chkRememberCreds.Font = New Font("Segoe UI", 9F)
        chkRememberCreds.Location = New Point(390, 101)
        chkRememberCreds.Name = "chkRememberCreds"
        chkRememberCreds.Size = New Size(84, 19)
        chkRememberCreds.TabIndex = 7
        chkRememberCreds.Text = "Remember"
        chkRememberCreds.UseVisualStyleBackColor = True
        ' 
        ' chkDeleteCreds
        ' 
        chkDeleteCreds.AutoSize = True
        chkDeleteCreds.Font = New Font("Segoe UI", 9F)
        chkDeleteCreds.Location = New Point(10, 32)
        chkDeleteCreds.Name = "chkDeleteCreds"
        chkDeleteCreds.Size = New Size(59, 19)
        chkDeleteCreds.TabIndex = 8
        chkDeleteCreds.Text = "Delete"
        chkDeleteCreds.UseVisualStyleBackColor = True
        ' 
        ' toolStatus
        ' 
        toolStatus.ForeColor = SystemColors.GrayText
        toolStatus.Name = "toolStatus"
        toolStatus.Size = New Size(42, 17)
        toolStatus.Text = "Status:"
        ' 
        ' statusStrip
        ' 
        statusStrip.Items.AddRange(New ToolStripItem() {toolStatus})
        statusStrip.Location = New Point(0, 269)
        statusStrip.Name = "statusStrip"
        statusStrip.Size = New Size(504, 22)
        statusStrip.TabIndex = 6
        statusStrip.Text = "StatusStrip1"
        ' 
        ' txtDomain
        ' 
        txtDomain.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtDomain.Font = New Font("Segoe UI", 12F)
        txtDomain.Location = New Point(91, 25)
        txtDomain.MinimumSize = New Size(180, 0)
        txtDomain.Name = "txtDomain"
        txtDomain.PlaceholderText = "Domain"
        txtDomain.Size = New Size(383, 29)
        txtDomain.TabIndex = 9
        ' 
        ' lblProfile
        ' 
        lblProfile.AutoSize = True
        lblProfile.Font = New Font("Segoe UI", 12F)
        lblProfile.Location = New Point(12, 9)
        lblProfile.Name = "lblProfile"
        lblProfile.Size = New Size(58, 21)
        lblProfile.TabIndex = 10
        lblProfile.Text = "Profile:"
        ' 
        ' lblCredentials
        ' 
        lblCredentials.AutoSize = True
        lblCredentials.Location = New Point(12, 44)
        lblCredentials.Name = "lblCredentials"
        lblCredentials.Size = New Size(0, 15)
        lblCredentials.TabIndex = 11
        ' 
        ' lblDomain
        ' 
        lblDomain.AutoSize = True
        lblDomain.Font = New Font("Segoe UI", 12F)
        lblDomain.Location = New Point(6, 28)
        lblDomain.Name = "lblDomain"
        lblDomain.Size = New Size(68, 21)
        lblDomain.TabIndex = 12
        lblDomain.Text = "Domain:"
        ' 
        ' lblUsername
        ' 
        lblUsername.AutoSize = True
        lblUsername.Font = New Font("Segoe UI", 12F)
        lblUsername.Location = New Point(6, 61)
        lblUsername.Name = "lblUsername"
        lblUsername.Size = New Size(84, 21)
        lblUsername.TabIndex = 13
        lblUsername.Text = "Username:"
        ' 
        ' lblPassword
        ' 
        lblPassword.AutoSize = True
        lblPassword.Font = New Font("Segoe UI", 12F)
        lblPassword.Location = New Point(6, 97)
        lblPassword.Name = "lblPassword"
        lblPassword.Size = New Size(79, 21)
        lblPassword.TabIndex = 14
        lblPassword.Text = "Password:"
        ' 
        ' lblActions
        ' 
        lblActions.AutoSize = True
        lblActions.Location = New Point(12, 177)
        lblActions.Name = "lblActions"
        lblActions.Size = New Size(0, 15)
        lblActions.TabIndex = 15
        ' 
        ' grpCredentials
        ' 
        grpCredentials.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        grpCredentials.Controls.Add(lblDomain)
        grpCredentials.Controls.Add(txtDomain)
        grpCredentials.Controls.Add(lblPassword)
        grpCredentials.Controls.Add(lblUsername)
        grpCredentials.Controls.Add(chkRememberCreds)
        grpCredentials.Controls.Add(txtUser)
        grpCredentials.Controls.Add(txtPassword)
        grpCredentials.Font = New Font("Segoe UI Semibold", 14F, FontStyle.Bold)
        grpCredentials.Location = New Point(12, 44)
        grpCredentials.Name = "grpCredentials"
        grpCredentials.Size = New Size(480, 147)
        grpCredentials.TabIndex = 16
        grpCredentials.TabStop = False
        grpCredentials.Text = "Credentials"
        ' 
        ' grpActions
        ' 
        grpActions.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        grpActions.Controls.Add(btnConnect)
        grpActions.Controls.Add(btnDisconnect)
        grpActions.Controls.Add(btnAdmin)
        grpActions.Controls.Add(chkDeleteCreds)
        grpActions.Font = New Font("Segoe UI Semibold", 14F, FontStyle.Bold)
        grpActions.Location = New Point(13, 197)
        grpActions.Name = "grpActions"
        grpActions.Size = New Size(479, 69)
        grpActions.TabIndex = 17
        grpActions.TabStop = False
        grpActions.Text = "Actions"
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(504, 291)
        Controls.Add(grpActions)
        Controls.Add(grpCredentials)
        Controls.Add(lblActions)
        Controls.Add(lblCredentials)
        Controls.Add(lblProfile)
        Controls.Add(statusStrip)
        Controls.Add(cboProfile)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MinimumSize = New Size(520, 330)
        Name = "MainForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "DriveMapper"
        statusStrip.ResumeLayout(False)
        statusStrip.PerformLayout()
        grpCredentials.ResumeLayout(False)
        grpCredentials.PerformLayout()
        grpActions.ResumeLayout(False)
        grpActions.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents cboProfile As ComboBox
    Friend WithEvents txtUser As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents btnConnect As Button
    Friend WithEvents btnDisconnect As Button
    Friend WithEvents btnAdmin As Button
    Friend WithEvents chkRememberCreds As CheckBox
    Friend WithEvents chkDeleteCreds As CheckBox
    Friend WithEvents toolStatus As ToolStripStatusLabel
    Friend WithEvents statusStrip As StatusStrip
    Friend WithEvents txtDomain As TextBox
    Friend WithEvents lblProfile As Label
    Friend WithEvents lblCredentials As Label
    Friend WithEvents lblDomain As Label
    Friend WithEvents lblUsername As Label
    Friend WithEvents lblPassword As Label
    Friend WithEvents lblActions As Label
    Friend WithEvents grpCredentials As GroupBox
    Friend WithEvents grpActions As GroupBox
End Class
