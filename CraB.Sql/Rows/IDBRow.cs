namespace CraB.Sql
{
	/// <summary>Интерфейс строки таблицы базы данных.</summary>
	public interface IDBRow
	{
		/// <summary>Название таблицы базы данных.</summary>
		public string TableName { get; }
	}
}
