using System;
using System.Reflection;

namespace CraB.Core
{
	/// <summary>Вспомогательные методы маппинга перечислений.</summary>
	public static class EnumHelper
	{
		/// <summary>Определяет ключ перечисления.</summary>
		/// <param name="type">Тип перечисления.</param>
		/// <returns>Ключ.</returns>
		public static string Key(Type type)
		{
			type.NotNull(nameof(type));

			EnumKeyAttribute enumKeyAttr = type.GetCustomAttribute<EnumKeyAttribute>();
			return (enumKeyAttr != null) ? enumKeyAttr.Value : type.FullName;
		}
	}
}
