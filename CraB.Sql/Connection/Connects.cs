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
	public static class Connects
	{
		private static readonly Dictionary<string, string> connections = new Dictionary<string, string>();

		/// <summary>Получает строку соединения по её ключу.</summary>
		/// <param name="key">Ключ подключения.</param>
		/// <returns>Строка соединения.</returns>
		private static string ConnectionStringGetByKey(string key)
		{
			if (!connections.TryGetValue(key, out string connectionString))
			{
				IConfiguration configuration = Dependencies.Resolve<IConfiguration>();

				if (configuration == null)
				{
					return null;
				}

				connectionString = configuration.GetConnectionString(key);

				if (connectionString != null)
				{
					connections.Add(key, connectionString);
				}
			}

			return connectionString;
		}

		/// <summary>Добавляет в словарь строку соединения с указанным ключом.</summary>
		/// <param name="key">Ключ.</param>
		/// <param name="value">Строка соединения.</param>
		public static void Add(string key, string value)
		{
			if (connections.ContainsKey(key))
			{
				throw new InvalidOperationException($"Ключ соединения с именем '{key}' уже добавлен.");
			}

			connections.Add(key, value);
		}

		/// <summary>Создает новое соединение для указанного ключа соединения.</summary>
		/// <param name="connectionKey">Ключ соединения.</param>
		/// <returns><see cref="IDbConnection" /></returns>
		public static IDbConnection New(string connectionKey)
		{
			return new SqlConnection(ConnectionStringGetByKey(connectionKey));
		}

		/// <summary>Создает новое соединение из типа указанного класса, определяя ключ соединения по его атрибуту <see cref="ConnectionKeyAttribute" />.</summary>
		/// <typeparam name="T">Тип указанного класса.</typeparam>
		/// <returns><see cref="IDbConnection" /></returns>
		/// <exception cref="ArgumentOutOfRangeException" />
		public static IDbConnection New<T>() where T : class
		{
			ConnectionKeyAttribute attr = typeof(T).GetCustomAttribute<ConnectionKeyAttribute>();

			return attr == null
				? throw new ArgumentOutOfRangeException($"Тип {typeof(T).FullName} не имеет атрибута ConnectionKey.")
				: New(attr.ConnectionKey);
		}

		/// <summary>Возвращает ключ соединения из атрибута <see cref="ConnectionKeyAttribute" /> для указанного типа класса.</summary>
		/// <typeparam name="T">Тип указанного класса.</typeparam>
		/// <returns>Ключ соединения или <c>null</c>.</returns>
		public static string Key<T>() where T : class
		{
			ConnectionKeyAttribute attr = typeof(T).GetCustomAttribute<ConnectionKeyAttribute>();
			return attr?.ConnectionKey;
		}
	}
}
