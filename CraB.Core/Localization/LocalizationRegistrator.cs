using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Класс регистрации локализаций из разных источников.</summary>
	public static class LocalizationRegistrator
	{
		/// <summary>Добавляет локализацию из классов, помеченных атрибутом <see cref="LocalizationAttribute" />.
		/// Ключ по умолчанию определяется по пути от внешнего к вложенным классам (если в атрибуте не указан префикс). Заменяет существующую локализацию.</summary>
		public static void RegisterLocalizationAttribute(this ILocalizationService registry, IEnumerable<Assembly> assemblies = null)
		{
			assemblies ??= Project.Assemblies;

			assemblies.NotNull(nameof(assemblies));

			foreach (Assembly assembly in assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					LocalizationAttribute attr = type.GetCustomAttribute<LocalizationAttribute>();

					if (attr != null)
					{
						LocalizationAttributeAdd(registry, type, attr.LanguageId ?? string.Empty, attr.Prefix ?? string.Empty);
					}
				}
			}
		}

		private static void LocalizationAttributeAdd(ILocalizationService registry, Type type, string languageId, string prefix)
		{
			ILocalizationService provider = registry ?? Dependencies.Resolve<ILocalizationService>();
			MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			foreach (MemberInfo member in members)
			{
				FieldInfo fi = member as FieldInfo;
				if (fi != null)
				{
					Console.WriteLine(fi.GetValue(null));
				}
				Console.WriteLine($"{member.Name}, {member.MemberType}.");
			}
		}
	}
}
