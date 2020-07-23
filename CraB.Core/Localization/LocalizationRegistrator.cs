using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Класс регистрации локализаций из различных источников.</summary>
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
					_localizationService.NotNull(nameof(_localizationService));

					_localizationService.RegisterAttributes();
					_localizationService.RegisterPermissions();
					_localizationService.RegisterEnums();
				}

				return _localizationService;
			}
		}

		/// <summary>Добавляет локализацию из классов, помеченных атрибутом <see cref="LocalizationAttribute" />.
		/// Ключ по умолчанию определяется по пути от внешнего к вложенным классам (если не указан префикс). Заменяет существующую локализацию.</summary>
		/// <param name="localizationService">Реестр для добавления локализаций.</param>
		private static void RegisterAttributes(this ILocalizationService localizationService)
		{
			foreach (Assembly assembly in Project.Assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					LocalizationAttribute attr = type.GetCustomAttribute<LocalizationAttribute>();

					if (attr != null)
					{
						localizationService.AttributeAdd(type, attr.LanguageId ?? string.Empty, attr.KeyPrefix ?? string.Empty);
					}
				}
			}
		}

		private static void AttributeAdd(this ILocalizationService localizationService, Type type, string languageId, string prefix)
		{
			MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			foreach (MemberInfo member in members)
			{
				FieldInfo fieldInfo = member as FieldInfo;

				if (fieldInfo != null)
				{
					object value = fieldInfo.GetValue(null) ?? fieldInfo.Name;
					localizationService.Add(languageId, $"{prefix}:{fieldInfo.Name}", value.ToString());
				}
			}
		}

		/// <summary>Получает ключи и локализации разрешений из вложенных классов, помеченных атрибутом <see cref="PermissionKeyAttribute" />.</summary>
		private static void RegisterPermissions(this ILocalizationService localizationService)
		{
			foreach (Assembly assembly in Project.Assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					PermissionKeyAttribute attr = type.GetCustomAttribute<PermissionKeyAttribute>();

					if (attr != null)
					{
						localizationService.PermissionAdd(type, attr.LanguageId ?? string.Empty);
					}
				}
			}
		}

		private static void PermissionAdd(this ILocalizationService localizationService, Type type, string languageId)
		{
			List<string> thisKeys = new List<string>();

			foreach (FieldInfo member in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly))
			{
				if (member.FieldType != typeof(string))
				{
					continue;
				}

				if (!(member.GetValue(null) is string key))
				{
					continue;
				}

				DescriptionAttribute descriptionAttribute;

				if (key.EndsWith(":", StringComparison.Ordinal))
				{
					descriptionAttribute = member.GetCustomAttribute<DescriptionAttribute>();
					localizationService.Add(languageId, $"Permission:{key}", descriptionAttribute != null ? descriptionAttribute.Description : member.Name);

					continue;
				}

				thisKeys.Add(key);
				descriptionAttribute = member.GetCustomAttribute<DescriptionAttribute>();
				localizationService.Add(languageId, $"Permission:{key}", descriptionAttribute != null ? descriptionAttribute.Description : member.Name);
			}

			if (thisKeys.Count > 0)
			{
				DisplayNameAttribute displayName = type.GetCustomAttribute<DisplayNameAttribute>();
				int lastColonIndex = thisKeys[0].LastIndexOf(":", StringComparison.Ordinal);

				if ((displayName != null) && (lastColonIndex > 0) && (lastColonIndex < thisKeys[0].Length - 1)
					&& thisKeys.TrueForAll(x => x.LastIndexOf(":", StringComparison.Ordinal) == lastColonIndex))
				{
					localizationService.Add(languageId, $"Permission:{thisKeys[0].Substring(0, lastColonIndex + 1)}", displayName.DisplayName);
				}
			}

			foreach (Type nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.DeclaredOnly))
			{
				localizationService.PermissionAdd(nested, languageId);
			}
		}

		/// <summary>Добавляет локализации, определенные атрибутами [Description] из enum-перечислениий. Локализации регистрируются в формате:
		/// "Enums.{EnumKey}.{EnumValueName}". EnumKey определяется из одноимённого атрибута, а в случае его отсутствия из полного имени перечисления.</summary>
		/// <param name="localizationService">Реестр для добавления локализаций.</param>
		/// <param name="languageId">Язык локализации (по-умолчанию - инвариантный).</param>
		private static void RegisterEnums(this ILocalizationService localizationService, string languageId = "")
		{
			foreach (Assembly assembly in Project.Assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsEnum)
					{
						string enumKey = EnumHelper.Key(type);

						foreach (string name in Enum.GetNames(type))
						{
							MemberInfo[] member = type.GetMember(name);

							if (member.Length == 0)
							{
								continue;
							}

							DescriptionAttribute descriptionAttribute = member[0].GetCustomAttribute<DescriptionAttribute>();

							if (descriptionAttribute != null)
							{
								localizationService.Add(languageId, $"Enums:{enumKey}:{name}", descriptionAttribute.Description);
							}
						}
					}
				}
			}
		}
	}
}
