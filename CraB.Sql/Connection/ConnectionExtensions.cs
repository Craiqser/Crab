using System;
using System.Data;

namespace CraB.Sql
{
	public static class ConnectionExtensions
	{
		/// <summary>Проверяет, что соединение открыто. Если нет, то предпринимается попытка его открыть.</summary>
		/// <param name="connection">Соединение.</param>
		/// <exception cref="ArgumentNullException" />
		public static IDbConnection EnsureOpen(this IDbConnection connection)
		{
			if (connection.State != ConnectionState.Open)
			{
				connection.Open();
			}

			return connection;
		}
	}
}
