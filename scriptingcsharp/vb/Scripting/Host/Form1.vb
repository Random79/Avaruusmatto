Imports System.IO

Public Class Form1
    Inherits System.Windows.Forms.Form
    Implements Interfaces.IHost

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Initialize()
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
    Friend WithEvents btnEditScript As System.Windows.Forms.Button
    Friend WithEvents btnFunction1 As System.Windows.Forms.Button
    Friend WithEvents btnFunction2 As System.Windows.Forms.Button
    Friend WithEvents btnFunction3 As System.Windows.Forms.Button
    Friend WithEvents btnFunction4 As System.Windows.Forms.Button
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnEditScript = New System.Windows.Forms.Button()
        Me.btnFunction1 = New System.Windows.Forms.Button()
        Me.btnFunction2 = New System.Windows.Forms.Button()
        Me.btnFunction3 = New System.Windows.Forms.Button()
        Me.btnFunction4 = New System.Windows.Forms.Button()
        Me.txtOutput = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnEditScript
        '
        Me.btnEditScript.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnEditScript.Location = New System.Drawing.Point(168, 16)
        Me.btnEditScript.Name = "btnEditScript"
        Me.btnEditScript.Size = New System.Drawing.Size(120, 36)
        Me.btnEditScript.TabIndex = 0
        Me.btnEditScript.Text = "Edit Script..."
        '
        'btnFunction1
        '
        Me.btnFunction1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.btnFunction1.Location = New System.Drawing.Point(28, 188)
        Me.btnFunction1.Name = "btnFunction1"
        Me.btnFunction1.Size = New System.Drawing.Size(92, 36)
        Me.btnFunction1.TabIndex = 1
        Me.btnFunction1.Text = "Function 1"
        '
        'btnFunction2
        '
        Me.btnFunction2.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.btnFunction2.Location = New System.Drawing.Point(132, 188)
        Me.btnFunction2.Name = "btnFunction2"
        Me.btnFunction2.Size = New System.Drawing.Size(92, 36)
        Me.btnFunction2.TabIndex = 1
        Me.btnFunction2.Text = "Function 2"
        '
        'btnFunction3
        '
        Me.btnFunction3.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.btnFunction3.Location = New System.Drawing.Point(236, 188)
        Me.btnFunction3.Name = "btnFunction3"
        Me.btnFunction3.Size = New System.Drawing.Size(92, 36)
        Me.btnFunction3.TabIndex = 1
        Me.btnFunction3.Text = "Function 3"
        '
        'btnFunction4
        '
        Me.btnFunction4.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.btnFunction4.Location = New System.Drawing.Point(340, 188)
        Me.btnFunction4.Name = "btnFunction4"
        Me.btnFunction4.Size = New System.Drawing.Size(92, 36)
        Me.btnFunction4.TabIndex = 1
        Me.btnFunction4.Text = "Function 4"
        '
        'txtOutput
        '
        Me.txtOutput.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutput.Location = New System.Drawing.Point(28, 68)
        Me.txtOutput.Multiline = True
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtOutput.Size = New System.Drawing.Size(404, 104)
        Me.txtOutput.TabIndex = 2
        Me.txtOutput.Text = ""
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(456, 239)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.txtOutput, Me.btnFunction1, Me.btnEditScript, Me.btnFunction2, Me.btnFunction3, Me.btnFunction4})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Scripting Sample"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private ScriptSource As String = ""
    Private Script As Interfaces.IScript = Nothing

    Private Sub Initialize()
        Dim s As Stream
        Dim b() As Byte

        'Get default script source from embedded text file
        s = Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Host.defaultscript.txt")
        ReDim b(Convert.ToInt32(s.Length) - 1)
        s.Read(b, 0, Convert.ToInt32(s.Length))
        ScriptSource = System.Text.ASCIIEncoding.ASCII.GetString(b)
    End Sub

    Public ReadOnly Property TextBox() As System.Windows.Forms.TextBox Implements Interfaces.IHost.TextBox
        Get
            Return txtOutput
        End Get
    End Property

    Public Sub ShowMessage(ByVal Message As String) Implements Interfaces.IHost.ShowMessage
        MessageBox.Show(Message)
    End Sub

    Private Sub btnFunction1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFunction1.Click
        If Not Script Is Nothing Then Script.Method1()
    End Sub

    Private Sub btnFunction2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFunction2.Click
        If Not Script Is Nothing Then Script.Method2()
    End Sub

    Private Sub btnFunction3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFunction3.Click
        If Not Script Is Nothing Then Script.Method3()
    End Sub

    Private Sub btnFunction4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFunction4.Click
        If Not Script Is Nothing Then Script.Method4()
    End Sub

    Private Sub btnEditScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditScript.Click
        Dim f As New frmScript()

        'Show script editing dialog with current script source
        f.ScriptSource = ScriptSource
        If f.ShowDialog(Me) = DialogResult.Cancel Then Return

        'Update local script source
        ScriptSource = f.ScriptSource

        'Use the compiled plugin that was produced
        Script = f.CompiledScript
        Script.Initialize(Me)
    End Sub

End Class
