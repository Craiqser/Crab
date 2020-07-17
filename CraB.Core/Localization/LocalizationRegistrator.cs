using System;
using System.Collections.Generic;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Класс регистрации локализаций из разных источников.</summary>
	public static class LocalizationRegistrator
	{
		private static ILocalizationService _localizationService;

		public static ILocalizationService LocalizationService
		{
			get
			{
				if (_localizationService == null)
				{
					_localizationService = Dependencies.Resolve<ILocalizationService>();
					_localizationService.RegisterLocalizationAttribute();
				}

				return _localizationService;
			}
		}

		/// <summary>Добавляет локализацию из классов, помеченных атрибутом <see cref="LocalizationAttribute" />.
		/// Ключ по умолчанию определяется по пути от внешнего к вложенным классам (если в атрибуте не указан префикс). Заменяет существующую локализацию.</summary>
		public static void RegisterLocalizationAttribute(this ILocalizationService registry, IEnumerable<Assembly> assemblies = null)
		{
			ILocalizationService localizationService = registry ?? Dependencies.Resolve<ILocalizationService>();
			localizationService.NotNull(nameof(localizationService));

			assemblies ??= Project.Assemblies;
			assemblies.NotNull(nameof(assemblies));

			foreach (Assembly assembly in assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					LocalizationAttribute attr = type.GetCustomAttribute<LocalizationAttribute>();

					if (attr != null)
					{
						localizationService.LocalizationAttributeAdd(type, attr.LanguageId ?? string.Empty, attr.Prefix ?? string.Empty);
					}
				}
			}
		}

		private static void LocalizationAttributeAdd(this ILocalizationService registry, Type type, string languageId, string prefix)
		{
			MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			foreach (MemberInfo member in members)
			{
				FieldInfo fieldInfo = member as FieldInfo;

				if (fieldInfo != null)
				{
					object value = fieldInfo.GetValue(null) ?? fieldInfo.Name;
					registry.Add(languageId, $"{prefix}:{fieldInfo.Name}", value.ToString());
				}
			}
		}
	}
}
