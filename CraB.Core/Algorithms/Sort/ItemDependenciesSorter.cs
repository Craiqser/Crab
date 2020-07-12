using System;
using System.Collections.Generic;

namespace CraB.Core
{
	public static class ItemDependenciesSorter
	{
		private static void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies, bool throwOnExists)
		{
			if (!visited.Contains(item))
			{
				_ = visited.Add(item);

				foreach (T dependency in dependencies(item))
				{
					Visit(dependency, visited, sorted, dependencies, throwOnExists);
				}

				sorted.Add(item);
			}
			else
			{
				if (throwOnExists)
				{
					throw new ArgumentOutOfRangeException($"Обнаружена циклическая зависимость: {item}.");
				}
			}
		}

		/// <summary>Общая функция сортировки элементов с зависимостями.</summary>
		/// <typeparam name="T">Тип</typeparam>
		/// <param name="source">Исходные элементы.</param>
		/// <param name="dependencies">Функция, получающая зависимости элемента.</param>
		/// <param name="throwOnExists">Если установлен в <c>true</c>, выбрасывает исключение циклической зависимости.</param>
		public static IEnumerable<T> Sort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies, bool throwOnExists = false)
		{
			source.NotNull(nameof(source));
			dependencies.NotNull(nameof(dependencies));

			List<T> sorted = new List<T>();
			HashSet<T> visited = new HashSet<T>();

			foreach (T item in source)
			{
				Visit(item, visited, sorted, dependencies, throwOnExists);
			}

			return sorted;
		}
	}
}
