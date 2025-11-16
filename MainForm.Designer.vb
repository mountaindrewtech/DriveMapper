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
        lblProfile = New Label()
        lblCredentials = New Label()
        lblActions = New Label()
        miniToolStrip = New StatusStrip()
        chkDeleteCreds = New CheckBox()
        btnAdmin = New Button()
        btnDisconnect = New Button()
        btnConnect = New Button()
        TableLayoutPanel1 = New TableLayoutPanel()
        statusStrip = New StatusStrip()
        toolStatus = New ToolStripStatusLabel()
        TableLayoutPanel2 = New TableLayoutPanel()
        Panel1 = New Panel()
        TableLayoutPanel3 = New TableLayoutPanel()
        lblDomain = New Label()
        txtDomain = New TextBox()
        txtPassword = New TextBox()
        lblUsername = New Label()
        lblPassword = New Label()
        txtUser = New TextBox()
        chkRememberCreds = New CheckBox()
        Label1 = New Label()
        Panel2 = New Panel()
        FlowLayoutPanel1 = New FlowLayoutPanel()
        Label2 = New Label()
        TableLayoutPanel1.SuspendLayout()
        statusStrip.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        Panel1.SuspendLayout()
        TableLayoutPanel3.SuspendLayout()
        Panel2.SuspendLayout()
        FlowLayoutPanel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' cboProfile
        ' 
        cboProfile.BackColor = Color.Gainsboro
        cboProfile.Dock = DockStyle.Fill
        cboProfile.DropDownHeight = 120
        cboProfile.DropDownStyle = ComboBoxStyle.DropDownList
        cboProfile.FlatStyle = FlatStyle.Flat
        cboProfile.Font = New Font("Segoe UI", 12F)
        cboProfile.ForeColor = Color.Black
        cboProfile.FormattingEnabled = True
        cboProfile.IntegralHeight = False
        cboProfile.Location = New Point(59, 6)
        cboProfile.Margin = New Padding(0)
        cboProfile.Name = "cboProfile"
        cboProfile.Size = New Size(703, 29)
        cboProfile.TabIndex = 0
        ' 
        ' lblProfile
        ' 
        lblProfile.AutoSize = True
        lblProfile.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblProfile.Location = New Point(10, 6)
        lblProfile.Margin = New Padding(0, 0, 8, 0)
        lblProfile.Name = "lblProfile"
        lblProfile.Size = New Size(41, 15)
        lblProfile.TabIndex = 10
        lblProfile.Text = "Profile"
        lblProfile.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' lblCredentials
        ' 
        lblCredentials.AutoSize = True
        lblCredentials.Location = New Point(12, 44)
        lblCredentials.Name = "lblCredentials"
        lblCredentials.Size = New Size(0, 15)
        lblCredentials.TabIndex = 11
        ' 
        ' lblActions
        ' 
        lblActions.AutoSize = True
        lblActions.Location = New Point(12, 177)
        lblActions.Name = "lblActions"
        lblActions.Size = New Size(0, 15)
        lblActions.TabIndex = 15
        ' 
        ' miniToolStrip
        ' 
        miniToolStrip.AccessibleName = "New item selection"
        miniToolStrip.AccessibleRole = AccessibleRole.ButtonDropDown
        miniToolStrip.AutoSize = False
        miniToolStrip.Dock = DockStyle.None
        miniToolStrip.Location = New Point(43, 1)
        miniToolStrip.Name = "miniToolStrip"
        miniToolStrip.Size = New Size(860, 22)
        miniToolStrip.TabIndex = 6
        ' 
        ' chkDeleteCreds
        ' 
        chkDeleteCreds.AutoSize = True
        chkDeleteCreds.BackColor = Color.Transparent
        chkDeleteCreds.Font = New Font("Segoe UI", 9F)
        chkDeleteCreds.Location = New Point(0, 0)
        chkDeleteCreds.Margin = New Padding(0, 0, 0, 8)
        chkDeleteCreds.Name = "chkDeleteCreds"
        chkDeleteCreds.Size = New Size(153, 19)
        chkDeleteCreds.TabIndex = 8
        chkDeleteCreds.Text = "Delete existing mapping"
        chkDeleteCreds.UseVisualStyleBackColor = False
        ' 
        ' btnAdmin
        ' 
        btnAdmin.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnAdmin.BackColor = Color.White
        btnAdmin.FlatStyle = FlatStyle.Flat
        btnAdmin.Font = New Font("Segoe UI", 9F)
        btnAdmin.Location = New Point(391, 0)
        btnAdmin.Margin = New Padding(4, 0, 8, 0)
        btnAdmin.Name = "btnAdmin"
        btnAdmin.Padding = New Padding(8, 0, 8, 0)
        btnAdmin.Size = New Size(110, 32)
        btnAdmin.TabIndex = 5
        btnAdmin.Text = "Admin Panel"
        btnAdmin.UseVisualStyleBackColor = False
        ' 
        ' btnDisconnect
        ' 
        btnDisconnect.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnDisconnect.BackColor = Color.White
        btnDisconnect.FlatStyle = FlatStyle.Flat
        btnDisconnect.Font = New Font("Segoe UI", 9F)
        btnDisconnect.Location = New Point(269, 0)
        btnDisconnect.Margin = New Padding(4, 0, 8, 0)
        btnDisconnect.Name = "btnDisconnect"
        btnDisconnect.Padding = New Padding(8, 0, 8, 0)
        btnDisconnect.Size = New Size(110, 32)
        btnDisconnect.TabIndex = 4
        btnDisconnect.Text = "Disconnect"
        btnDisconnect.UseVisualStyleBackColor = False
        ' 
        ' btnConnect
        ' 
        btnConnect.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnConnect.BackColor = SystemColors.Highlight
        btnConnect.FlatAppearance.BorderSize = 0
        btnConnect.FlatStyle = FlatStyle.Flat
        btnConnect.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnConnect.ForeColor = Color.White
        btnConnect.Location = New Point(157, 0)
        btnConnect.Margin = New Padding(4, 0, 8, 0)
        btnConnect.Name = "btnConnect"
        btnConnect.Padding = New Padding(8, 0, 8, 0)
        btnConnect.Size = New Size(100, 32)
        btnConnect.TabIndex = 3
        btnConnect.Text = "Connect"
        btnConnect.UseVisualStyleBackColor = False
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.BackColor = Color.White
        TableLayoutPanel1.ColumnCount = 1
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanel1.Controls.Add(statusStrip, 0, 3)
        TableLayoutPanel1.Controls.Add(TableLayoutPanel2, 0, 0)
        TableLayoutPanel1.Controls.Add(Panel1, 0, 1)
        TableLayoutPanel1.Controls.Add(Panel2, 0, 2)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(0, 0)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 4
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Absolute, 45F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 58.82353F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 41.17647F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle())
        TableLayoutPanel1.Size = New Size(778, 321)
        TableLayoutPanel1.TabIndex = 18
        ' 
        ' statusStrip
        ' 
        statusStrip.BackColor = Color.White
        statusStrip.Items.AddRange(New ToolStripItem() {toolStatus})
        statusStrip.Location = New Point(0, 299)
        statusStrip.Name = "statusStrip"
        statusStrip.Size = New Size(778, 22)
        statusStrip.TabIndex = 6
        statusStrip.Text = "StatusStrip1"
        ' 
        ' toolStatus
        ' 
        toolStatus.ForeColor = SystemColors.GrayText
        toolStatus.Name = "toolStatus"
        toolStatus.Size = New Size(763, 17)
        toolStatus.Spring = True
        toolStatus.Text = "Ready"
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.BackColor = Color.White
        TableLayoutPanel2.ColumnCount = 2
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanel2.Controls.Add(cboProfile, 1, 0)
        TableLayoutPanel2.Controls.Add(lblProfile, 0, 0)
        TableLayoutPanel2.Dock = DockStyle.Fill
        TableLayoutPanel2.Location = New Point(3, 3)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.Padding = New Padding(10, 6, 10, 0)
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle())
        TableLayoutPanel2.Size = New Size(772, 39)
        TableLayoutPanel2.TabIndex = 18
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = Color.White
        Panel1.Controls.Add(TableLayoutPanel3)
        Panel1.Controls.Add(chkRememberCreds)
        Panel1.Controls.Add(Label1)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(3, 48)
        Panel1.Name = "Panel1"
        Panel1.Padding = New Padding(10)
        Panel1.Size = New Size(772, 143)
        Panel1.TabIndex = 15
        ' 
        ' TableLayoutPanel3
        ' 
        TableLayoutPanel3.AutoSize = True
        TableLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink
        TableLayoutPanel3.ColumnCount = 2
        TableLayoutPanel3.ColumnStyles.Add(New ColumnStyle())
        TableLayoutPanel3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        TableLayoutPanel3.Controls.Add(lblDomain, 0, 0)
        TableLayoutPanel3.Controls.Add(txtDomain, 1, 0)
        TableLayoutPanel3.Controls.Add(txtPassword, 1, 2)
        TableLayoutPanel3.Controls.Add(lblUsername, 0, 1)
        TableLayoutPanel3.Controls.Add(lblPassword, 0, 2)
        TableLayoutPanel3.Controls.Add(txtUser, 1, 1)
        TableLayoutPanel3.Dock = DockStyle.Top
        TableLayoutPanel3.Location = New Point(10, 29)
        TableLayoutPanel3.Margin = New Padding(0)
        TableLayoutPanel3.Name = "TableLayoutPanel3"
        TableLayoutPanel3.RowCount = 3
        TableLayoutPanel3.RowStyles.Add(New RowStyle())
        TableLayoutPanel3.RowStyles.Add(New RowStyle())
        TableLayoutPanel3.RowStyles.Add(New RowStyle())
        TableLayoutPanel3.Size = New Size(752, 81)
        TableLayoutPanel3.TabIndex = 1
        ' 
        ' lblDomain
        ' 
        lblDomain.AutoSize = True
        lblDomain.Font = New Font("Segoe UI", 12F)
        lblDomain.Location = New Point(0, 2)
        lblDomain.Margin = New Padding(0, 2, 8, 2)
        lblDomain.Name = "lblDomain"
        lblDomain.Size = New Size(65, 21)
        lblDomain.TabIndex = 12
        lblDomain.Text = "Domain"
        lblDomain.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' txtDomain
        ' 
        txtDomain.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        txtDomain.BackColor = Color.WhiteSmoke
        txtDomain.Font = New Font("Segoe UI", 9F)
        txtDomain.Location = New Point(89, 2)
        txtDomain.Margin = New Padding(0, 2, 0, 2)
        txtDomain.Name = "txtDomain"
        txtDomain.Size = New Size(663, 23)
        txtDomain.TabIndex = 9
        ' 
        ' txtPassword
        ' 
        txtPassword.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        txtPassword.BackColor = Color.WhiteSmoke
        txtPassword.Font = New Font("Segoe UI", 9F)
        txtPassword.Location = New Point(89, 56)
        txtPassword.Margin = New Padding(0, 2, 0, 2)
        txtPassword.Name = "txtPassword"
        txtPassword.Size = New Size(663, 23)
        txtPassword.TabIndex = 2
        txtPassword.UseSystemPasswordChar = True
        ' 
        ' lblUsername
        ' 
        lblUsername.AutoSize = True
        lblUsername.Font = New Font("Segoe UI", 12F)
        lblUsername.Location = New Point(0, 29)
        lblUsername.Margin = New Padding(0, 2, 8, 2)
        lblUsername.Name = "lblUsername"
        lblUsername.Size = New Size(81, 21)
        lblUsername.TabIndex = 13
        lblUsername.Text = "Username"
        lblUsername.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' lblPassword
        ' 
        lblPassword.AutoSize = True
        lblPassword.Font = New Font("Segoe UI", 12F)
        lblPassword.Location = New Point(0, 56)
        lblPassword.Margin = New Padding(0, 2, 8, 2)
        lblPassword.Name = "lblPassword"
        lblPassword.Size = New Size(76, 21)
        lblPassword.TabIndex = 14
        lblPassword.Text = "Password"
        lblPassword.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' txtUser
        ' 
        txtUser.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        txtUser.BackColor = Color.WhiteSmoke
        txtUser.Font = New Font("Segoe UI", 9F)
        txtUser.Location = New Point(89, 29)
        txtUser.Margin = New Padding(0, 2, 0, 2)
        txtUser.Name = "txtUser"
        txtUser.Size = New Size(663, 23)
        txtUser.TabIndex = 1
        ' 
        ' chkRememberCreds
        ' 
        chkRememberCreds.AutoSize = True
        chkRememberCreds.BackColor = Color.Transparent
        chkRememberCreds.Dock = DockStyle.Bottom
        chkRememberCreds.Font = New Font("Segoe UI", 9F)
        chkRememberCreds.Location = New Point(10, 114)
        chkRememberCreds.Margin = New Padding(0, 8, 0, 0)
        chkRememberCreds.Name = "chkRememberCreds"
        chkRememberCreds.Size = New Size(752, 19)
        chkRememberCreds.TabIndex = 7
        chkRememberCreds.Text = "Remember credentials"
        chkRememberCreds.UseVisualStyleBackColor = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Dock = DockStyle.Top
        Label1.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        Label1.Location = New Point(10, 10)
        Label1.Margin = New Padding(0, 0, 0, 4)
        Label1.Name = "Label1"
        Label1.Size = New Size(84, 19)
        Label1.TabIndex = 0
        Label1.Text = "Credentials"
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = Color.White
        Panel2.Controls.Add(FlowLayoutPanel1)
        Panel2.Controls.Add(Label2)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(0, 204)
        Panel2.Margin = New Padding(0, 10, 0, 0)
        Panel2.Name = "Panel2"
        Panel2.Padding = New Padding(10)
        Panel2.Size = New Size(778, 94)
        Panel2.TabIndex = 19
        ' 
        ' FlowLayoutPanel1
        ' 
        FlowLayoutPanel1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        FlowLayoutPanel1.AutoSize = True
        FlowLayoutPanel1.Controls.Add(chkDeleteCreds)
        FlowLayoutPanel1.Controls.Add(btnConnect)
        FlowLayoutPanel1.Controls.Add(btnDisconnect)
        FlowLayoutPanel1.Controls.Add(btnAdmin)
        FlowLayoutPanel1.Location = New Point(10, 29)
        FlowLayoutPanel1.Margin = New Padding(0)
        FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        FlowLayoutPanel1.Size = New Size(765, 32)
        FlowLayoutPanel1.TabIndex = 1
        FlowLayoutPanel1.WrapContents = False
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Dock = DockStyle.Top
        Label2.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        Label2.Location = New Point(10, 10)
        Label2.Margin = New Padding(0, 0, 0, 4)
        Label2.Name = "Label2"
        Label2.Size = New Size(58, 19)
        Label2.TabIndex = 0
        Label2.Text = "Actions"
        ' 
        ' MainForm
        ' 
        AcceptButton = btnConnect
        AutoScaleDimensions = New SizeF(96F, 96F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = SystemColors.AppWorkspace
        ClientSize = New Size(778, 321)
        Controls.Add(lblActions)
        Controls.Add(lblCredentials)
        Controls.Add(TableLayoutPanel1)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimumSize = New Size(600, 360)
        Name = "MainForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "DriveMapper"
        TableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel1.PerformLayout()
        statusStrip.ResumeLayout(False)
        statusStrip.PerformLayout()
        TableLayoutPanel2.ResumeLayout(False)
        TableLayoutPanel2.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        TableLayoutPanel3.ResumeLayout(False)
        TableLayoutPanel3.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        FlowLayoutPanel1.ResumeLayout(False)
        FlowLayoutPanel1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents cboProfile As ComboBox
    Friend WithEvents lblProfile As Label
    Friend WithEvents lblCredentials As Label
    Friend WithEvents lblActions As Label
    Friend WithEvents miniToolStrip As StatusStrip
    Friend WithEvents btnConnect As Button
    Friend WithEvents btnDisconnect As Button
    Friend WithEvents btnAdmin As Button
    Friend WithEvents chkDeleteCreds As CheckBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents lblDomain As Label
    Friend WithEvents txtDomain As TextBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents lblUsername As Label
    Friend WithEvents lblPassword As Label
    Friend WithEvents txtUser As TextBox
    Friend WithEvents chkRememberCreds As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents Label2 As Label
    Friend WithEvents statusStrip As StatusStrip
    Friend WithEvents toolStatus As ToolStripStatusLabel
End Class
