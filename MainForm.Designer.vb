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
        statusStrip.SuspendLayout()
        SuspendLayout()
        ' 
        ' cboProfile
        ' 
        cboProfile.DropDownStyle = ComboBoxStyle.DropDownList
        cboProfile.FormattingEnabled = True
        cboProfile.Location = New Point(12, 12)
        cboProfile.Name = "cboProfile"
        cboProfile.Size = New Size(121, 23)
        cboProfile.TabIndex = 0
        ' 
        ' txtUser
        ' 
        txtUser.Location = New Point(139, 12)
        txtUser.Name = "txtUser"
        txtUser.Size = New Size(100, 23)
        txtUser.TabIndex = 1
        ' 
        ' txtPassword
        ' 
        txtPassword.Location = New Point(245, 11)
        txtPassword.Name = "txtPassword"
        txtPassword.Size = New Size(100, 23)
        txtPassword.TabIndex = 2
        txtPassword.UseSystemPasswordChar = True
        ' 
        ' btnConnect
        ' 
        btnConnect.Location = New Point(441, 11)
        btnConnect.Name = "btnConnect"
        btnConnect.Size = New Size(75, 23)
        btnConnect.TabIndex = 3
        btnConnect.Text = "Connect"
        btnConnect.UseVisualStyleBackColor = True
        ' 
        ' btnDisconnect
        ' 
        btnDisconnect.Location = New Point(522, 12)
        btnDisconnect.Name = "btnDisconnect"
        btnDisconnect.Size = New Size(75, 23)
        btnDisconnect.TabIndex = 4
        btnDisconnect.Text = "Disconnect"
        btnDisconnect.UseVisualStyleBackColor = True
        ' 
        ' btnAdmin
        ' 
        btnAdmin.Location = New Point(603, 12)
        btnAdmin.Name = "btnAdmin"
        btnAdmin.Size = New Size(75, 23)
        btnAdmin.TabIndex = 5
        btnAdmin.Text = "Admin"
        btnAdmin.UseVisualStyleBackColor = True
        ' 
        ' chkRememberCreds
        ' 
        chkRememberCreds.AutoSize = True
        chkRememberCreds.Location = New Point(351, 10)
        chkRememberCreds.Name = "chkRememberCreds"
        chkRememberCreds.Size = New Size(84, 19)
        chkRememberCreds.TabIndex = 7
        chkRememberCreds.Text = "Remember"
        chkRememberCreds.UseVisualStyleBackColor = True
        ' 
        ' chkDeleteCreds
        ' 
        chkDeleteCreds.AutoSize = True
        chkDeleteCreds.Location = New Point(351, 35)
        chkDeleteCreds.Name = "chkDeleteCreds"
        chkDeleteCreds.Size = New Size(59, 19)
        chkDeleteCreds.TabIndex = 8
        chkDeleteCreds.Text = "Delete"
        chkDeleteCreds.UseVisualStyleBackColor = True
        ' 
        ' toolStatus
        ' 
        toolStatus.Name = "toolStatus"
        toolStatus.Size = New Size(42, 17)
        toolStatus.Text = "Status:"
        ' 
        ' statusStrip
        ' 
        statusStrip.Items.AddRange(New ToolStripItem() {toolStatus})
        statusStrip.Location = New Point(0, 59)
        statusStrip.Name = "statusStrip"
        statusStrip.Size = New Size(684, 22)
        statusStrip.TabIndex = 6
        statusStrip.Text = "StatusStrip1"
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(684, 81)
        Controls.Add(chkRememberCreds)
        Controls.Add(statusStrip)
        Controls.Add(chkDeleteCreds)
        Controls.Add(btnAdmin)
        Controls.Add(btnDisconnect)
        Controls.Add(btnConnect)
        Controls.Add(txtPassword)
        Controls.Add(txtUser)
        Controls.Add(cboProfile)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        Name = "MainForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "DriveMapper"
        statusStrip.ResumeLayout(False)
        statusStrip.PerformLayout()
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
End Class
