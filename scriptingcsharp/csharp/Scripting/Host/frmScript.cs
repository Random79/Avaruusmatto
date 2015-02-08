using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.CodeDom.Compiler;

namespace Host
{
	/// <summary>
	/// Summary description for frmScript.
	/// </summary>
	public class frmScript : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.ColumnHeader ColumnHeader2;
		internal System.Windows.Forms.Button btnOk;
		internal System.Windows.Forms.ColumnHeader ColumnHeader1;
		internal System.Windows.Forms.ListView lvwErrors;
		internal System.Windows.Forms.TextBox txtScript;
		internal System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmScript()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ColumnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.btnOk = new System.Windows.Forms.Button();
			this.ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.lvwErrors = new System.Windows.Forms.ListView();
			this.txtScript = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ColumnHeader2
			// 
			this.ColumnHeader2.Text = "Line";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnOk.Location = new System.Drawing.Point(540, 343);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(96, 32);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// ColumnHeader1
			// 
			this.ColumnHeader1.Text = "Error";
			this.ColumnHeader1.Width = 456;
			// 
			// lvwErrors
			// 
			this.lvwErrors.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lvwErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.ColumnHeader1,
																						this.ColumnHeader2});
			this.lvwErrors.FullRowSelect = true;
			this.lvwErrors.GridLines = true;
			this.lvwErrors.Location = new System.Drawing.Point(4, 331);
			this.lvwErrors.MultiSelect = false;
			this.lvwErrors.Name = "lvwErrors";
			this.lvwErrors.Size = new System.Drawing.Size(520, 96);
			this.lvwErrors.TabIndex = 4;
			this.lvwErrors.View = System.Windows.Forms.View.Details;
			this.lvwErrors.ItemActivate += new System.EventHandler(this.lvwErrors_ItemActivate);
			// 
			// txtScript
			// 
			this.txtScript.AcceptsReturn = true;
			this.txtScript.AcceptsTab = true;
			this.txtScript.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtScript.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtScript.Location = new System.Drawing.Point(4, 3);
			this.txtScript.Multiline = true;
			this.txtScript.Name = "txtScript";
			this.txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtScript.Size = new System.Drawing.Size(644, 324);
			this.txtScript.TabIndex = 3;
			this.txtScript.Text = "";
			this.txtScript.WordWrap = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(540, 383);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 32);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			// 
			// frmScript
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(652, 431);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnOk,
																		  this.lvwErrors,
																		  this.txtScript,
																		  this.btnCancel});
			this.Name = "frmScript";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Script";
			this.ResumeLayout(false);

		}
		#endregion

		private Interfaces.IScript _compiledScript = null;

		public string ScriptSource
		{
			get { return txtScript.Text; }
			set
			{
				txtScript.Text = value;
				txtScript.SelectionLength = 0;
			}
		}

		public Interfaces.IScript CompiledScript
		{
			get { return _compiledScript; }
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			CompilerResults results;
			string reference;

			Cursor = Cursors.WaitCursor;

			// Find reference
			reference = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			if (!reference.EndsWith(@"\"))
				reference += @"\";
			reference += "interfaces.dll";

			// Compile script
			lvwErrors.Items.Clear();
			results = Scripting.CompileScript(ScriptSource, reference, Scripting.Languages.VB);

			if (results.Errors.Count == 0)
			{
				_compiledScript = (Interfaces.IScript)Scripting.FindInterface(results.CompiledAssembly, "IScript");
				DialogResult = DialogResult.OK;
			}
			else
			{
				ListViewItem l;

				// Add each error as a listview item with its line number
				foreach (CompilerError err in results.Errors)
				{
					l = new ListViewItem(err.ErrorText);
					l.SubItems.Add(err.Line.ToString());
					lvwErrors.Items.Add(l);
				}

	            MessageBox.Show("Compile failed with " + results.Errors.Count.ToString() + " errors.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}

			Cursor = Cursors.Default;
		}

		private void lvwErrors_ItemActivate(object sender, System.EventArgs e)
		{
			int l = Convert.ToInt32(lvwErrors.SelectedItems[0].SubItems[1].Text);
			int i, pos;

			if (l != 0)
			{
				i = 1;
				pos = 0;
				while (i < l)
				{
					pos = txtScript.Text.IndexOf(Environment.NewLine, pos + 1);
					i++;
				}
				txtScript.SelectionStart = pos;
				txtScript.SelectionLength = txtScript.Text.IndexOf(Environment.NewLine, pos + 1) - pos;
			}

			txtScript.Focus();
		}

	}
}
