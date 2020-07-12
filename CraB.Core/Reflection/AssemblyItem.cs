using System.Collections.Generic;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Сборка и её зависимости.</summary>
	public class AssemblyItem
	{
		public Assembly Item { get; }
		public List<AssemblyItem> Dependencies { get; }

		public AssemblyItem(Assembly item)
		{
			Item = item;
			Dependencies = new List<AssemblyItem>();
		}
	}
}
