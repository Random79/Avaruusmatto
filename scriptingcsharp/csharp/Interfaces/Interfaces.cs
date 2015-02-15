using System;

namespace Interfaces
{
	public interface IScript
	{
		void Initialize(IHost Host);

		void Hail();
		void ParameterChanged(string parameter, string value);
		void Method3();
		void Method4();
	}

	public interface IHost
	{
		System.Windows.Forms.TextBox TextBox { get; }
		void ShowMessage(string Message);
        void SetParameter(string name, string value);
        string GetParameter(string name);

	}
}
