using System;

namespace CraB.Core
{
	/// <summary>Указывает, что класс содержит ресурсы локализации, загружаемые реализацией <see cref="ILocalizationService" />.</summary>
	/// <remarks>Без указания культуры используется для инвариантной культуры. При необходимости сегменты префикса следует разделять точкой.</remarks>
	/// <seealso cref="Attribute" />
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class LocalizationAttribute : Attribute
	{
		/// <summary>Культура.</summary>
		public string LanguageId { get; set; } = string.Empty;

		/// <summary>Префикс ключа.</summary>
		public string KeyPrefix { get; set; } = string.Empty;

		/// <summary>Инициализирует новый экземпляр класса <see cref="LocalizationAttribute" />.</summary>
		public LocalizationAttribute()
		{ }
	}
}
