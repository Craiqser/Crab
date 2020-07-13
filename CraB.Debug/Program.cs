using CraB.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CraB.Debug
{
	class Program
	{
		static void Main()
		{
			List<Assembly> assemblies = Project.Assemblies;

			Console.WriteLine(assemblies.Count);
			Console.ReadKey();
		}
	}
}
