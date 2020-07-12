using Microsoft.Data.SqlClient;
using System;

namespace CraB.Debug
{
	class Program
	{
		static void Main()
		{
			using SqlConnection connection = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Work;Data Source=.;MultipleActiveResultSets=true");


			Console.ReadKey();
		}
	}
}
