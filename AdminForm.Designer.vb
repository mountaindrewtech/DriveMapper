' Copyright (c) 2025 Drew Schmidt
' Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License.
' See LICENSE for details.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AdminForm
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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        gridProfiles = New DataGridView()
        colName = New DataGridViewTextBoxColumn()
        colUnc = New DataGridViewTextBoxColumn()
        colDriveLetter = New DataGridViewTextBoxColumn()
        Domain = New DataGridViewTextBoxColumn()
        colUseCredential = New DataGridViewCheckBoxColumn()
        btnAdd = New Button()
        btnEdit = New Button()
        btnRemove = New Button()
        btnTest = New Button()
        btnSave = New Button()
        chkUseCredentialManager = New CheckBox()
        StatusStrip1 = New StatusStrip()
        AdminStatusLabel = New ToolStripStatusLabel()
        CType(gridProfiles, ComponentModel.ISupportInitialize).BeginInit()
        StatusStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' gridProfiles
        ' 
        gridProfiles.AllowUserToAddRows = False
        gridProfiles.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.SelectionBackColor = Color.Gainsboro
        gridProfiles.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        gridProfiles.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        gridProfiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        gridProfiles.BackgroundColor = Color.Gainsboro
        gridProfiles.BorderStyle = BorderStyle.None
        gridProfiles.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        gridProfiles.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = SystemColors.Control
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle2.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.True
        gridProfiles.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        gridProfiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        gridProfiles.Columns.AddRange(New DataGridViewColumn() {colName, colUnc, colDriveLetter, Domain, colUseCredential})
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = SystemColors.Window
        DataGridViewCellStyle3.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle3.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = Color.White
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.False
        gridProfiles.DefaultCellStyle = DataGridViewCellStyle3
        gridProfiles.Location = New Point(21, 21)
        gridProfiles.Margin = New Padding(12)
        gridProfiles.MultiSelect = False
        gridProfiles.Name = "gridProfiles"
        gridProfiles.ReadOnly = True
        gridProfiles.RowHeadersVisible = False
        gridProfiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        gridProfiles.Size = New Size(559, 157)
        gridProfiles.TabIndex = 0
        ' 
        ' colName
        ' 
        colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        colName.FillWeight = 200F
        colName.HeaderText = "Name"
        colName.Name = "colName"
        colName.ReadOnly = True
        ' 
        ' colUnc
        ' 
        colUnc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        colUnc.FillWeight = 260F
        colUnc.HeaderText = "UNC Path"
        colUnc.Name = "colUnc"
        colUnc.ReadOnly = True
        ' 
        ' colDriveLetter
        ' 
        colDriveLetter.FillWeight = 60F
        colDriveLetter.HeaderText = "Drive"
        colDriveLetter.Name = "colDriveLetter"
        colDriveLetter.ReadOnly = True
        ' 
        ' Domain
        ' 
        Domain.FillWeight = 120F
        Domain.HeaderText = "Domain"
        Domain.Name = "Domain"
        Domain.ReadOnly = True
        ' 
        ' colUseCredential
        ' 
        colUseCredential.FillWeight = 80F
        colUseCredential.HeaderText = "CredMgr"
        colUseCredential.Name = "colUseCredential"
        colUseCredential.ReadOnly = True
        ' 
        ' btnAdd
        ' 
        btnAdd.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnAdd.BackColor = Color.White
        btnAdd.FlatStyle = FlatStyle.Flat
        btnAdd.Location = New Point(595, 9)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(110, 23)
        btnAdd.TabIndex = 1
        btnAdd.Text = "Add"
        btnAdd.UseVisualStyleBackColor = False
        ' 
        ' btnEdit
        ' 
        btnEdit.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnEdit.BackColor = Color.White
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.Location = New Point(595, 38)
        btnEdit.Name = "btnEdit"
        btnEdit.Size = New Size(110, 23)
        btnEdit.TabIndex = 2
        btnEdit.Text = "Edit"
        btnEdit.UseVisualStyleBackColor = False
        ' 
        ' btnRemove
        ' 
        btnRemove.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnRemove.BackColor = Color.White
        btnRemove.FlatStyle = FlatStyle.Flat
        btnRemove.Location = New Point(595, 67)
        btnRemove.Name = "btnRemove"
        btnRemove.Size = New Size(110, 23)
        btnRemove.TabIndex = 3
        btnRemove.Text = "Remove"
        btnRemove.UseVisualStyleBackColor = False
        ' 
        ' btnTest
        ' 
        btnTest.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnTest.BackColor = Color.White
        btnTest.FlatStyle = FlatStyle.Flat
        btnTest.Location = New Point(595, 96)
        btnTest.Name = "btnTest"
        btnTest.Size = New Size(110, 23)
        btnTest.TabIndex = 4
        btnTest.Text = "Test"
        btnTest.UseVisualStyleBackColor = False
        ' 
        ' btnSave
        ' 
        btnSave.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnSave.BackColor = SystemColors.Highlight
        btnSave.FlatAppearance.BorderSize = 0
        btnSave.FlatStyle = FlatStyle.Flat
        btnSave.ForeColor = Color.White
        btnSave.Location = New Point(595, 125)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(110, 23)
        btnSave.TabIndex = 5
        btnSave.Text = "Save"
        btnSave.UseVisualStyleBackColor = False
        ' 
        ' chkUseCredentialManager
        ' 
        chkUseCredentialManager.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        chkUseCredentialManager.AutoSize = True
        chkUseCredentialManager.Location = New Point(595, 153)
        chkUseCredentialManager.Name = "chkUseCredentialManager"
        chkUseCredentialManager.Size = New Size(121, 34)
        chkUseCredentialManager.TabIndex = 6
        chkUseCredentialManager.Text = "Enable Credential " & vbCrLf & "Manager"
        chkUseCredentialManager.UseVisualStyleBackColor = True
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {AdminStatusLabel})
        StatusStrip1.Location = New Point(0, 190)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Size = New Size(715, 22)
        StatusStrip1.TabIndex = 7
        StatusStrip1.Text = "StatusStrip1"
        ' 
        ' AdminStatusLabel
        ' 
        AdminStatusLabel.Name = "AdminStatusLabel"
        AdminStatusLabel.Size = New Size(39, 17)
        AdminStatusLabel.Text = "Ready"
        ' 
        ' AdminForm
        ' 
        AcceptButton = btnSave
        AutoScaleDimensions = New SizeF(96F, 96F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.White
        ClientSize = New Size(715, 212)
        Controls.Add(StatusStrip1)
        Controls.Add(chkUseCredentialManager)
        Controls.Add(btnSave)
        Controls.Add(btnTest)
        Controls.Add(btnRemove)
        Controls.Add(btnEdit)
        Controls.Add(btnAdd)
        Controls.Add(gridProfiles)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        Name = "AdminForm"
        StartPosition = FormStartPosition.CenterParent
        Text = "Admin"
        CType(gridProfiles, ComponentModel.ISupportInitialize).EndInit()
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents gridProfiles As DataGridView
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnRemove As Button
    Friend WithEvents btnTest As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents chkUseCredentialManager As CheckBox
    Friend WithEvents colName As DataGridViewTextBoxColumn
    Friend WithEvents colUnc As DataGridViewTextBoxColumn
    Friend WithEvents colDriveLetter As DataGridViewTextBoxColumn
    Friend WithEvents Domain As DataGridViewTextBoxColumn
    Friend WithEvents colUseCredential As DataGridViewCheckBoxColumn
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents AdminStatusLabel As ToolStripStatusLabel
End Class
