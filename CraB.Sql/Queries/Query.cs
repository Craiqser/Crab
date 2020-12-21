using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CraB.Sql
{
	public static class Query
	{
		public static string DefaultConnectionKey { get; } = "App";

		/// <summary>Асинхронно вставляет <typeparamref name="T"/> в таблицу "Ts" (или указанную в <see cref="TableAttribute"/>) и возвращает идентификатор вставленной записи.</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">Класс объекта.</param>
		/// <param name="connectionKey">Ключ соединения (по умолчанию <c>Default</c>).</param>
		/// <returns></returns>
		public static async Task<int> InsertAsync<T>(T value, string connectionKey = null) where T : class
		{
			using IDbConnection connection = Connects.New(connectionKey ?? DefaultConnectionKey);
			return await connection.InsertAsync(value);
		}

		/// <summary>Выполняет запрос, возвращающий одну запись, иначе возвращает значение по-умолчанию для заданного типа.</summary>
		/// <typeparam name="T">Тип объекта запроса.</typeparam>
		/// <param name="sql">Текст запроса.</param>
		/// <param name="param">Параметры запроса.</param>
		/// <param name="connectionKey">Ключ конфигурации строки соединения.</param>
		/// <returns><typeparamref name="T" /></returns>
		public static T SingleOrDefault<T>(string sql, object param = null, string connectionKey = null) where T : class
		{
			using IDbConnection connection = Connects.New(connectionKey ?? DefaultConnectionKey);
			return connection.QuerySingleOrDefault<T>(sql, param);
		}

		/// <summary>Выполняет асинхронный запрос, возвращающий одну запись, иначе возвращает значение по-умолчанию для заданного типа.</summary>
		/// <typeparam name="T">Тип объекта запроса.</typeparam>
		/// <param name="sql">Текст запроса.</param>
		/// <param name="param">Параметры запроса.</param>
		/// <param name="connectionKey">Ключ конфигурации строки соединения.</param>
		/// <returns><typeparamref name="T" /></returns>
		public static async Task<T> SingleOrDefaultAsync<T>(string sql, object param = null, string connectionKey = null) where T : class
		{
			using IDbConnection connection = Connects.New(connectionKey ?? DefaultConnectionKey);
			return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
		}

		/// <summary>Выполняет запрос <c>select</c>, возвращающий все записи.</summary>
		/// <typeparam name="T">Тип объекта запроса.</typeparam>
		/// <param name="sql">Текст запроса.</param>
		/// <param name="param">Параметры запроса.</param>
		/// <param name="connectionKey">Ключ конфигурации строки соединения.</param>
		public static IEnumerable<T> Select<T>(string sql, object param = null, string connectionKey = null) where T : class
		{
			connectionKey ??= Connects.Key<T>();
			using IDbConnection connection = Connects.New(connectionKey ?? DefaultConnectionKey);

			return connection.Query<T>(sql, param);
		}

		/// <summary>Выполняет асинхронный запрос <c>select</c>, возвращающий все записи.</summary>
		/// <typeparam name="T">Тип объекта запроса.</typeparam>
		/// <param name="sql">Текст запроса.</param>
		/// <param name="param">Параметры запроса.</param>
		/// <param name="connectionKey">Ключ конфигурации строки соединения.</param>
		public static async Task<IEnumerable<T>> SelectAsync<T>(string sql, object param = null, string connectionKey = null) where T : class
		{
			connectionKey ??= Connects.Key<T>();
			using IDbConnection connection = Connects.New(connectionKey ?? DefaultConnectionKey);

			return await connection.QueryAsync<T>(sql, param);
		}
	}
}
