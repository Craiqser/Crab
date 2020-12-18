using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Содержит вспомогательные функции для работы с ассемблерными сборками.</summary>
	public static class Project
	{
		private const string assemblyPrefix = "CraB";
		private static List<Assembly> _assemblies = new List<Assembly>();

		/// <summary>Получает сборки проекта.</summary>
		/// <value>Сборки.</value>
		public static List<Assembly> Assemblies
		{
			get
			{
				if (_assemblies.Count == 0)
				{
					_assemblies = AssembliesDetermine();
				}

				return _assemblies;
			}
		}

		private static List<Assembly> AssembliesDetermine()
		{
			Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>(StringComparer.Ordinal);

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				string name = assembly.GetName().Name;

				if (!assemblies.ContainsKey(name) && ReferencesToMain(assembly))
				{
					assemblies.Add(name, assembly);

					foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
					{
						name = referencedAssembly.Name;

						if (!assemblies.ContainsKey(name))
						{
							Assembly refasm = Assembly.Load(referencedAssembly);

							if (ReferencesToMain(refasm))
							{
								assemblies.Add(name, refasm);
							}
						}
					}
				}
			}

			return AssembliesSorter.Sort(assemblies.Values).ToList();
		}

		private static bool ReferencesToMain(Assembly assembly)
		{
			return assembly.FullName.Contains(assemblyPrefix, StringComparison.Ordinal)
				|| assembly.GetReferencedAssemblies().Any(a => a.Name.Contains(assemblyPrefix, StringComparison.Ordinal));
		}

		/// <summary>Получает атрибут.</summary>
		/// <typeparam name="T">Тип атрибута.</typeparam>
		/// <param name="memberInfo">Член.</param>
		/// <param name="inherit">Если установлен в <c>true</c>, то поиск происходит по всей цепочке наследования.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static T AttributeGet<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
		{
			object[] attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);

			if (!attributes.Any())
			{
				return null;
			}

			if (attributes.Length > 1)
			{
				throw new InvalidOperationException($"'{memberInfo.Name}' имеет более одного атрибута '{typeof(T).Name}'.");
			}

			return (T)attributes.First();
		}

		/// <summary>Добавление пользовательской сборки.</summary>
		/// <param name="assembly">Сборка.</param>
		public static void AssemblyAdd(Assembly assembly)
		{
			if (!Assemblies.Contains(assembly))
			{
				_assemblies.Add(assembly);
			}
		}

		/// <summary>Получает типы интерфейса.</summary>
		/// <param name="interfaceType">Тип интерфейса.</param>
		/// <returns>Типы с данным интерфейсом.</returns>
		public static IEnumerable<Type> InterfaceTypesGet(Type interfaceType)
		{
			interfaceType.NotNull(nameof(interfaceType));

			foreach (Assembly assembly in Assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (!type.IsInterface && interfaceType.IsAssignableFrom(type))
					{
						yield return type;
					}
				}
			}
		}
	}
}
