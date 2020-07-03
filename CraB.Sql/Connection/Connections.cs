using CraB.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CraB.Sql
{
	/// <summary>Содержит функции, связанные с подключением к базе данных.</summary>
	public static class Connections
	{
		private static readonly Dictionary<string, string> connections = new Dictionary<string, string>();

		/// <summary>Получает строку соединения по её ключу.</summary>
		/// <param name="key">Ключ подключения.</param>
		/// <returns>Строка соединения.</returns>
		public static string ConnectionStringGet(string key)
		{
			if (!connections.TryGetValue(key, out string connectionString))
			{
				IConfiguration configuration = Dependencies.Resolve<IConfiguration>();

				if (configuration == null)
				{
					return null;
				}

				connectionString = configuration.GetConnectionString(key);

				if (connectionString == null)
				{
					return null;
				}

				connections.Add(key, connectionString);
			}

			return connectionString;
		}

		/// <summary>Создает новое соединение для указанного ключа соединения.</summary>
		/// <param name="connectionKey">Ключ соединения.</param>
		/// <returns><see cref="IDbConnection" /></returns>
		public static IDbConnection New(string connectionKey)
		{
			return new SqlConnection(ConnectionStringGet(connectionKey));
		}

		/// <summary>Создает новое соединение из типа указанного класса, определяя ключ соединения по его атрибуту <see cref="ConnectionKeyAttribute" />.</summary>
		/// <typeparam name="T">Тип указанного класса.</typeparam>
		/// <returns><see cref="IDbConnection" /></returns>
		/// <exception cref="ArgumentOutOfRangeException">Type has no ConnectionKey attribute.</exception>
		public static IDbConnection New<T>() where T : class
		{
			ConnectionKeyAttribute attr = typeof(T).GetCustomAttribute<ConnectionKeyAttribute>();

			if (attr == null)
			{
				throw new ArgumentOutOfRangeException($"Тип {typeof(T).FullName} не имеет атрибута ConnectionKey.");
			}

			return New(attr.ConnectionKey);
		}
	}
}
