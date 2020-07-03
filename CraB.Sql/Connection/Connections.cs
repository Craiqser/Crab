using CraB.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace CraB.Sql
{
	/// <summary>Содержит функции, связанные с подключением к базе данных.</summary>
	public static class Connections
	{
		private static Dictionary<string, string> connections = new Dictionary<string, string>();

		/// <summary>Получает строку соединения по её ключу.</summary>
		/// <param name="key">Ключ подключения.</param>
		/// <returns>Строка соединения.</returns>
		public static string ConnectionStringGet(string key)
		{
			if (!connections.TryGetValue(key, out string connectionString))
			{
				IConfigurationManager configuration = Dependencies.Resolve<IConfigurationManager>();

				if (configuration == null)
				{
					return null;
				}

				Tuple<string, string> connectionSetting = configuration.ConnectionString(key);

				if (connectionSetting == null)
				{
					return null;
				}

				connectionString = newConnections[key] = new ConnectionStringInfo(key, connectionSetting.Item1, connectionSetting.Item2);
				connections = newConnections;
			}

			return connectionString;
		}

		/// <summary>Получает имя базы данных для указанного ключа подключения, анализируя его.</summary>
		/// <param name="connectionKey">Ключ подключения.</param>
		/// <returns>Имя базы данных, или <c>null</c>, если ключ не может быть проанализирован.</returns>
		public static string GetDatabaseName(string connectionKey)
		{
			ConnectionStringInfo connection = TryGetConnectionString(connectionKey);

			return connection?.DatabaseName;
		}

		/// <summary>Получает строку подключения для указанного ключа подключения.</summary>
		/// <param name="connectionKey">Ключ подключения.</param>
		/// <returns>Строка подключения.</returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static ConnectionStringInfo GetConnectionString(string connectionKey)
		{
			ConnectionStringInfo connection = TryGetConnectionString(connectionKey);

			if (connection == null)
			{
				throw new InvalidOperationException($"В файле конфигурации отсутствует строка подключения с ключом {connectionKey}.");
			}

			return connection;
		}

		/// <summary>Создает новый <see cref="DbConnection" /> для заданной строки подключения и имени поставщика.</summary>
		/// <param name="connectionString">Строка подключения.</param>
		/// <param name="providerName">Название провайдера.</param>
		/// <returns>Новый объект соединения <see cref="DbConnection" />.</returns>
		public static IDbConnection New(string connectionString, string providerName)
		{
			DbProviderFactory factory = GetFactory(providerName);
			DbConnection connection = factory.CreateConnection();
			connection.ConnectionString = connectionString;
			ISqlDialect dialect = ConnectionStringInfo.GetDialectByProviderName(providerName) ?? SqlSettings.DefaultDialect;

			return new WrappedConnection(connection, dialect);
		}

		/// <summary>Создает новое соединение для указанного ключа соединения.</summary>
		/// <param name="connectionKey">Ключ соединения.</param>
		/// <returns>Новое соединение.</returns>
		public static IDbConnection NewByKey(string connectionKey)
		{
			ConnectionStringInfo connectionSetting = GetConnectionString(connectionKey);
			DbConnection connection = connectionSetting.ProviderFactory.CreateConnection();
			connection.ConnectionString = connectionSetting.ConnectionString;

			return new WrappedConnection(connection, connectionSetting.Dialect);
		}

		/// <summary>Создает новое соединение для указанного класса, определяя ключ соединения по его атрибуту [ConnectionKey].</summary>
		/// <typeparam name="TClass">Тип класса.</typeparam>
		/// <returns>Новое соединение.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Type has no ConnectionKey attribute.</exception>
		public static IDbConnection NewFor<TClass>()
		{
			ConnectionKeyAttribute attr = typeof(TClass).GetCustomAttribute<ConnectionKeyAttribute>();

			if (attr == null)
			{
				throw new ArgumentOutOfRangeException($"Тип {typeof(TClass).FullName} не имеет атрибута ConnectionKey.");
			}

			return NewByKey(attr.Value);
		}
	}
}
