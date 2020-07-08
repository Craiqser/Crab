using Dapper;
using System.Data;

namespace CraB.Sql
{
	public static class Query
	{
		public static string DefaultConnectionKey { get; set; } = "Default";

		/// <summary>Выполняет запрос, возвращающий одну запись, иначе возвращает значение по-умолчанию для заданного типа.</summary>
		/// <typeparam name="T">Тип объекта запроса.</typeparam>
		/// <param name="sql">Текст запроса.</param>
		/// <param name="param">Параметры запроса.</param>
		/// <param name="connectionKey">Ключ конфигурации строки соединения.</param>
		/// <returns><typeparamref name="T" /></returns>
		public static T SingleOrDefault<T>(string sql, object param = null, string connectionKey = null)
		{
			using IDbConnection connection = Connections.New(connectionKey ?? DefaultConnectionKey);

			return connection.QuerySingleOrDefault<T>(sql, param);
		}
	}
}
