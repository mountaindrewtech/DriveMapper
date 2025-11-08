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
        gridProfiles = New DataGridView()
        colName = New DataGridViewTextBoxColumn()
        colUnc = New DataGridViewTextBoxColumn()
        colDriveLetter = New DataGridViewTextBoxColumn()
        colUseCredential = New DataGridViewCheckBoxColumn()
        btnAdd = New Button()
        btnEdit = New Button()
        btnRemove = New Button()
        btnTest = New Button()
        btnSave = New Button()
        chkUseCredentialManager = New CheckBox()
        CType(gridProfiles, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' gridProfiles
        ' 
        gridProfiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        gridProfiles.Columns.AddRange(New DataGridViewColumn() {colName, colUnc, colDriveLetter, colUseCredential})
        gridProfiles.Location = New Point(12, 12)
        gridProfiles.Name = "gridProfiles"
        gridProfiles.ReadOnly = True
        gridProfiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        gridProfiles.Size = New Size(343, 181)
        gridProfiles.TabIndex = 0
        ' 
        ' colName
        ' 
        colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        colName.HeaderText = "Name"
        colName.Name = "colName"
        colName.ReadOnly = True
        ' 
        ' colUnc
        ' 
        colUnc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        colUnc.HeaderText = "UNC"
        colUnc.Name = "colUnc"
        colUnc.ReadOnly = True
        ' 
        ' colDriveLetter
        ' 
        colDriveLetter.HeaderText = "Drive"
        colDriveLetter.Name = "colDriveLetter"
        colDriveLetter.ReadOnly = True
        ' 
        ' colUseCredential
        ' 
        colUseCredential.HeaderText = "CredMgr"
        colUseCredential.Name = "colUseCredential"
        colUseCredential.ReadOnly = True
        ' 
        ' btnAdd
        ' 
        btnAdd.Location = New Point(361, 12)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(110, 23)
        btnAdd.TabIndex = 1
        btnAdd.Text = "Add"
        btnAdd.UseVisualStyleBackColor = True
        ' 
        ' btnEdit
        ' 
        btnEdit.Location = New Point(361, 41)
        btnEdit.Name = "btnEdit"
        btnEdit.Size = New Size(110, 23)
        btnEdit.TabIndex = 2
        btnEdit.Text = "Edit"
        btnEdit.UseVisualStyleBackColor = True
        ' 
        ' btnRemove
        ' 
        btnRemove.Location = New Point(361, 70)
        btnRemove.Name = "btnRemove"
        btnRemove.Size = New Size(110, 23)
        btnRemove.TabIndex = 3
        btnRemove.Text = "Remove"
        btnRemove.UseVisualStyleBackColor = True
        ' 
        ' btnTest
        ' 
        btnTest.Location = New Point(361, 99)
        btnTest.Name = "btnTest"
        btnTest.Size = New Size(110, 23)
        btnTest.TabIndex = 4
        btnTest.Text = "Test"
        btnTest.UseVisualStyleBackColor = True
        ' 
        ' btnSave
        ' 
        btnSave.Location = New Point(361, 128)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(110, 23)
        btnSave.TabIndex = 5
        btnSave.Text = "Save"
        btnSave.UseVisualStyleBackColor = True
        ' 
        ' chkUseCredentialManager
        ' 
        chkUseCredentialManager.AutoSize = True
        chkUseCredentialManager.Location = New Point(361, 157)
        chkUseCredentialManager.Name = "chkUseCredentialManager"
        chkUseCredentialManager.Size = New Size(121, 34)
        chkUseCredentialManager.TabIndex = 6
        chkUseCredentialManager.Text = "Enable Credential " & vbCrLf & "Manager"
        chkUseCredentialManager.UseVisualStyleBackColor = True
        ' 
        ' AdminForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(483, 200)
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
        Text = "AdminForm"
        CType(gridProfiles, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents gridProfiles As DataGridView
    Friend WithEvents colName As DataGridViewTextBoxColumn
    Friend WithEvents colUnc As DataGridViewTextBoxColumn
    Friend WithEvents colDriveLetter As DataGridViewTextBoxColumn
    Friend WithEvents colUseCredential As DataGridViewCheckBoxColumn
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnRemove As Button
    Friend WithEvents btnTest As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents chkUseCredentialManager As CheckBox
End Class
