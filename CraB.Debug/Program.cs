using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CraB.Debug
{
	class Program
	{
		static void Main()
		{
			DATAAREA da = new DATAAREA();
			using SqlConnection connection = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Work;Data Source=.;MultipleActiveResultSets=true");

			List<DATAAREA> dataareas = connection.Query<DATAAREA>("select t.* from dbo.DATAAREA as t with(nolock) where (t.ISVIRTUAL = 0)").Where(da => da.ISVIRTUAL == 0).ToList();

			foreach (DATAAREA dataarea in dataareas)
			{
				Console.WriteLine($"{dataarea.ID}	{dataarea.NAME}	{dataarea.ISVIRTUAL}");
			}

			Console.ReadKey();
		}
	}
}
