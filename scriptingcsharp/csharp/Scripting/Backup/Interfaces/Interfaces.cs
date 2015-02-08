using System;

namespace Interfaces
{
	public interface IScript
	{
		void Initialize(IHost Host);

		void Method1();
		void Method2();
		void Method3();
		void Method4();
	}

	public interface IHost
	{
		System.Windows.Forms.TextBox TextBox { get; }
		void ShowMessage(string Message);
	}
}
