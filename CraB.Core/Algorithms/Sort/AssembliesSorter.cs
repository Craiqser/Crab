using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Класс сортировки ассемблерных сборок.</summary>
	public static class AssembliesSorter
	{
		/// <summary>Сортирует указанные сборки на основе их зависимостей.</summary>
		/// <param name="assemblies">Сборки.</param>
		public static IEnumerable<Assembly> Sort(IEnumerable<Assembly> assemblies)
		{
			List<AssemblyItem> assembliesList = assemblies.Select(assembly => new AssemblyItem(assembly)).ToList();

			foreach (AssemblyItem assembly in assembliesList)
			{
				foreach (AssemblyName reference in assembly.Item.GetReferencedAssemblies())
				{
					AssemblyItem dependency = assembliesList.SingleOrDefault(i => i.Item.FullName == reference.FullName);

					if (dependency != null)
					{
						assembly.Dependencies.Add(dependency);
					}
				}
			}

			return ItemDependenciesSorter.Sort(assembliesList, i => i.Dependencies).Select(x => x.Item);
		}
	}
}
