using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Host
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form, Interfaces.IHost
	{
		internal System.Windows.Forms.TextBox txtOutput;
		internal System.Windows.Forms.Button btnFunction1;
		internal System.Windows.Forms.Button btnEditScript;
		internal System.Windows.Forms.Button btnFunction2;
		internal System.Windows.Forms.Button btnFunction3;
		internal System.Windows.Forms.Button btnFunction4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			System.IO.Stream s;
			byte[] b;

			// Get default script source from embedded text file
			s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Host.defaultscript.txt");
			b = new byte[Convert.ToInt32(s.Length)];
			s.Read(b, 0, Convert.ToInt32(s.Length));
			ScriptSource = System.Text.ASCIIEncoding.ASCII.GetString(b);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.btnFunction1 = new System.Windows.Forms.Button();
			this.btnEditScript = new System.Windows.Forms.Button();
			this.btnFunction2 = new System.Windows.Forms.Button();
			this.btnFunction3 = new System.Windows.Forms.Button();
			this.btnFunction4 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtOutput
			// 
			this.txtOutput.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtOutput.Location = new System.Drawing.Point(26, 67);
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtOutput.Size = new System.Drawing.Size(404, 104);
			this.txtOutput.TabIndex = 8;
			this.txtOutput.Text = "";
			// 
			// btnFunction1
			// 
			this.btnFunction1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnFunction1.Location = new System.Drawing.Point(26, 187);
			this.btnFunction1.Name = "btnFunction1";
			this.btnFunction1.Size = new System.Drawing.Size(92, 36);
			this.btnFunction1.TabIndex = 7;
			this.btnFunction1.Text = "Function 1";
			this.btnFunction1.Click += new System.EventHandler(this.btnFunction1_Click);
			// 
			// btnEditScript
			// 
			this.btnEditScript.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnEditScript.Location = new System.Drawing.Point(166, 15);
			this.btnEditScript.Name = "btnEditScript";
			this.btnEditScript.Size = new System.Drawing.Size(120, 36);
			this.btnEditScript.TabIndex = 3;
			this.btnEditScript.Text = "Edit Script...";
			this.btnEditScript.Click += new System.EventHandler(this.btnEditScript_Click);
			// 
			// btnFunction2
			// 
			this.btnFunction2.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnFunction2.Location = new System.Drawing.Point(130, 187);
			this.btnFunction2.Name = "btnFunction2";
			this.btnFunction2.Size = new System.Drawing.Size(92, 36);
			this.btnFunction2.TabIndex = 6;
			this.btnFunction2.Text = "Function 2";
			this.btnFunction2.Click += new System.EventHandler(this.btnFunction2_Click);
			// 
			// btnFunction3
			// 
			this.btnFunction3.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnFunction3.Location = new System.Drawing.Point(234, 187);
			this.btnFunction3.Name = "btnFunction3";
			this.btnFunction3.Size = new System.Drawing.Size(92, 36);
			this.btnFunction3.TabIndex = 4;
			this.btnFunction3.Text = "Function 3";
			this.btnFunction3.Click += new System.EventHandler(this.btnFunction3_Click);
			// 
			// btnFunction4
			// 
			this.btnFunction4.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnFunction4.Location = new System.Drawing.Point(338, 187);
			this.btnFunction4.Name = "btnFunction4";
			this.btnFunction4.Size = new System.Drawing.Size(92, 36);
			this.btnFunction4.TabIndex = 5;
			this.btnFunction4.Text = "Function 4";
			this.btnFunction4.Click += new System.EventHandler(this.btnFunction4_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 239);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.txtOutput,
																		  this.btnFunction1,
																		  this.btnEditScript,
																		  this.btnFunction2,
																		  this.btnFunction3,
																		  this.btnFunction4});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Scripting Sample";
			this.ResumeLayout(false);

		}
		#endregion

		string ScriptSource = "";
		Interfaces.IScript Script = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		public System.Windows.Forms.TextBox TextBox
		{
			get
			{
				return txtOutput;
			}
		}

		public void ShowMessage(string Message)
		{
			MessageBox.Show(Message);
		}

		private void btnFunction1_Click(object sender, System.EventArgs e)
		{
			if (Script != null) Script.Method1();
		}

		private void btnFunction2_Click(object sender, System.EventArgs e)
		{
			if (Script != null) Script.Method2();
		}

		private void btnFunction3_Click(object sender, System.EventArgs e)
		{
			if (Script != null) Script.Method3();
		}

		private void btnFunction4_Click(object sender, System.EventArgs e)
		{
			if (Script != null) Script.Method4();
		}

		private void btnEditScript_Click(object sender, System.EventArgs e)
		{
			frmScript f = new frmScript();

			// Show script editing dialog with current script source
			f.ScriptSource = ScriptSource;
			if (f.ShowDialog(this) == DialogResult.Cancel)
				return;

			// Update local script source
			ScriptSource = f.ScriptSource;

			// Use the compiled plugin that was produced
			Script = f.CompiledScript;
			Script.Initialize(this);
		}
	}
}
