using System;

namespace CraB.Core
{
	/// <summary>Указывает, что класс содержит ключи прав доступа.</summary>
	/// <seealso cref="Attribute" />
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class PermissionKeyAttribute : Attribute
	{
		/// <summary>Инициализирует новый экземпляр класса <see cref="PermissionKeyAttribute" />.</summary>
		public PermissionKeyAttribute()
		{ }

		/// <summary>Язык локализаций, указанных в атрибуте [DisplayName].</summary>
		/// <value>Язык.</value>
		public string LanguageId { get; set; }
	}
}
