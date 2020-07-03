using System;

namespace CraB.Sql
{
	/// <summary>Имя таблицы класса строки таблицы базы данных.</summary>
	/// <seealso cref="Attribute" />
	[AttributeUsage(AttributeTargets.Class)]
	public class TableNameAttribute : Attribute
	{
		/// <summary>Получает название таблицы.</summary>
		/// <value>Название таблицы.</value>
		public string TableName { get; }

		/// <summary>Инициализирует новый экземпляр класса <see cref="TableNameAttribute" />.</summary>
		/// <param name="tableName">Название таблицы.</param>
		/// <exception cref="ArgumentNullException">name</exception>
		public TableNameAttribute(string tableName)
		{
			TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
		}
	}
}
