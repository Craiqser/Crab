using Dapper;
using System.Data;

namespace CraB.Sql
{
	public static class ConnectionExtensions
	{
		/// <summary>Выбирает одну запись указанного типа.</summary>
		/// <typeparam name="T">Тип строки.</typeparam>
		/// <param name="dbConnection">Соединение.</param>
		/// <param name="query">Запрос.</param>
		/// <returns><typeparamref name="T"/></returns>
		public static T SelectOne<T>(this IDbConnection dbConnection, string query) where T : DBRow
		{
			return dbConnection.QuerySingle<T>(query);
		}
	}
}
