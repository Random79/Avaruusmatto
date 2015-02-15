Imports System.CodeDom.Compiler

Public Class frmScript
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtScript As System.Windows.Forms.TextBox
    Friend WithEvents lvwErrors As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtScript = New System.Windows.Forms.TextBox()
        Me.lvwErrors = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader()
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtScript
        '
        Me.txtScript.AcceptsReturn = True
        Me.txtScript.AcceptsTab = True
        Me.txtScript.Anchor = (((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right)
        Me.txtScript.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScript.Location = New System.Drawing.Point(4, 4)
        Me.txtScript.Multiline = True
        Me.txtScript.Name = "txtScript"
        Me.txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtScript.Size = New System.Drawing.Size(644, 324)
        Me.txtScript.TabIndex = 0
        Me.txtScript.Text = ""
        Me.txtScript.WordWrap = False
        '
        'lvwErrors
        '
        Me.lvwErrors.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right)
        Me.lvwErrors.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvwErrors.FullRowSelect = True
        Me.lvwErrors.GridLines = True
        Me.lvwErrors.Location = New System.Drawing.Point(4, 332)
        Me.lvwErrors.MultiSelect = False
        Me.lvwErrors.Name = "lvwErrors"
        Me.lvwErrors.Size = New System.Drawing.Size(520, 96)
        Me.lvwErrors.TabIndex = 1
        Me.lvwErrors.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Error"
        Me.ColumnHeader1.Width = 456
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Line"
        '
        'btnOk
        '
        Me.btnOk.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)
        Me.btnOk.Location = New System.Drawing.Point(540, 344)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(96, 32)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "Ok"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(540, 384)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(96, 32)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        '
        'frmScript
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(652, 431)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnOk, Me.lvwErrors, Me.txtScript, Me.btnCancel})
        Me.Name = "frmScript"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Script"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _compiledScript As Interfaces.IScript = Nothing

    Public Property ScriptSource() As String
        Get
            Return txtScript.Text
        End Get
        Set(ByVal Value As String)
            txtScript.Text = Value
            txtScript.SelectionLength = 0
        End Set
    End Property

    Public ReadOnly Property CompiledScript() As Interfaces.IScript
        Get
            Return _compiledScript
        End Get
    End Property

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim results As CompilerResults
        Dim reference As String

        Cursor = Cursors.WaitCursor

        'Find reference
        reference = System.IO.Path.GetDirectoryName(Application.ExecutablePath)
        If Not reference.EndsWith("\") Then reference &= "\"
        reference &= "interfaces.dll"

        'Compile script
        lvwErrors.Items.Clear()
        results = Scripting.CompileScript(ScriptSource, reference, Scripting.Languages.VB)

        If results.Errors.Count = 0 Then
            _compiledScript = DirectCast(Scripting.FindInterface(results.CompiledAssembly, "IScript"), Interfaces.IScript)
            DialogResult = DialogResult.OK
        Else
            Dim err As CompilerError
            Dim l As ListViewItem

            'Add each error as a listview item with its line number
            For Each err In results.Errors
                l = New ListViewItem(err.ErrorText)
                l.SubItems.Add(err.Line.ToString())
                lvwErrors.Items.Add(l)
            Next

            MessageBox.Show("Compile failed with " & results.Errors.Count.ToString() & " errors.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End If

        Cursor = Cursors.Default
    End Sub

    Private Sub lvwErrors_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwErrors.ItemActivate
        Dim l As Integer = Convert.ToInt32(lvwErrors.SelectedItems(0).SubItems(1).Text)
        Dim i, pos As Integer

        If l <> 0 Then
            i = 1
            pos = 0
            Do While i < l
                pos = txtScript.Text.IndexOf(Environment.NewLine, pos + 1)
                i += 1
            Loop
            txtScript.SelectionStart = pos
            txtScript.SelectionLength = txtScript.Text.IndexOf(Environment.NewLine, pos + 1) - pos
        End If

        txtScript.Focus()
    End Sub

End Class
