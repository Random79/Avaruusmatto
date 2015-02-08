using System;
using System.CodeDom.Compiler;

namespace Host
{
	public class Scripting
	{

		public enum Languages
		{
			VB,
			CSharp
		}

		public static CompilerResults CompileScript(string Source, string Reference, Languages Language)
		{
			CodeDomProvider provider = null;

			switch(Language)
			{
				case Languages.VB:
					provider = new Microsoft.VisualBasic.VBCodeProvider();
					break;
				case Languages.CSharp:
					provider = new Microsoft.CSharp.CSharpCodeProvider();
					break;
			}

			return CompileScript(Source, Reference, provider);
		}

		public static CompilerResults CompileScript(string Source, string Reference, CodeDomProvider Provider)
		{
			ICodeCompiler compiler = Provider.CreateCompiler();
			CompilerParameters parms = new CompilerParameters();
			CompilerResults results;

			// Configure parameters
			parms.GenerateExecutable = false;
			parms.GenerateInMemory = true;
			parms.IncludeDebugInformation = false;
			if (Reference != null && Reference.Length != 0)
				parms.ReferencedAssemblies.Add(Reference);
			parms.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			parms.ReferencedAssemblies.Add("System.dll");

			// Compile
			results = compiler.CompileAssemblyFromSource(parms, Source);

			return results;
		}

		public static object FindInterface(System.Reflection.Assembly DLL, string InterfaceName)
		{
			// Loop through types looking for one that implements the given interface
			foreach(Type t in DLL.GetTypes())
			{
				if (t.GetInterface(InterfaceName, true) != null)
					return DLL.CreateInstance(t.FullName);
			}

			return null;
		}

	}
}
